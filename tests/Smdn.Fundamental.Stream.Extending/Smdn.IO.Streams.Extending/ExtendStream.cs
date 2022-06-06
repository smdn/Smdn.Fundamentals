// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

#if SYSTEM_ARRAY_EMPTY
using ArrayEmptyShim = System.Array; // System.Array.Empty
#else
using ArrayEmptyShim = Smdn.ArrayShim; // Smdn.ArrayShim.Empty
#endif

namespace Smdn.IO.Streams.Extending;

[TestFixture]
public class ExtendStreamTests {
  [Test]
  public void Construct()
  {
    Assert.AreEqual(0L, new ExtendStream(Stream.Null, (byte[])null, null).Length, "construct null/null");
    Assert.AreEqual(0L, new ExtendStream(Stream.Null, new byte[0], null).Length, "construct byte[]/null");
    Assert.AreEqual(0L, new ExtendStream(Stream.Null, null, new byte[0]).Length, "construct null/byte[]");
    Assert.AreEqual(0L, new ExtendStream(Stream.Null, new byte[0], new byte[0]).Length, "construct byte[]/byte[]");
    Assert.AreEqual(0L, new ExtendStream(Stream.Null, Stream.Null, null).Length, "construct Stream/null");
    Assert.AreEqual(0L, new ExtendStream(Stream.Null, null, Stream.Null).Length, "construct null/Stream");
    Assert.AreEqual(0L, new ExtendStream(Stream.Null, Stream.Null, Stream.Null).Length, "construct Stream/Stream");
  }

  [Test]
  public void Construct_StreamNull()
  {
    Assert.Throws<ArgumentNullException>(() => new ExtendStream(null, Stream.Null, Stream.Null));
    Assert.Throws<ArgumentNullException>(() => new ExtendStream(null, new byte[0], new byte[0]));
  }

  [TestCase(true)]
  [TestCase(false)]
  public void Construct_LeaveInnerStreamOpen(bool leaveOpen)
  {
    var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03 });

    using (var stream = new ExtendStream(baseStream, Stream.Null, Stream.Null, leaveInnerStreamOpen: leaveOpen)) {
      Assert.AreEqual(leaveOpen, stream.LeaveInnerStreamOpen);
    }

    if (leaveOpen)
      Assert.DoesNotThrow(() => baseStream.ReadByte());
    else
      Assert.Throws<ObjectDisposedException>(() => baseStream.ReadByte());
  }

  [TestCase(true)]
  [TestCase(false)]
  public void Construct_LeavePrependStreamOpen(bool leaveOpen)
  {
    var prependStream = new MemoryStream(new byte[] { 0x00, 0x01 });
    var baseStream = new MemoryStream(new byte[] { 0x02, 0x03 });
    var appendStream = new MemoryStream(new byte[] { 0x04, 0x05 });

    using (var stream = new ExtendStream(baseStream, prependStream, appendStream, leavePrependStreamOpen: leaveOpen)) {
    }

    Assert.DoesNotThrow(() => appendStream.ReadByte(), "leave open as default");

    if (leaveOpen)
      Assert.DoesNotThrow(() => prependStream.ReadByte());
    else
      Assert.Throws<ObjectDisposedException>(() => prependStream.ReadByte());
  }

  [TestCase(true)]
  [TestCase(false)]
  public void Construct_LeaveAppependStreamOpen(bool leaveOpen)
  {
    var prependStream = new MemoryStream(new byte[] { 0x00, 0x01 });
    var baseStream = new MemoryStream(new byte[] { 0x02, 0x03 });
    var appendStream = new MemoryStream(new byte[] { 0x04, 0x05 });

    using (var stream = new ExtendStream(baseStream, prependStream, appendStream, leaveAppendStreamOpen: leaveOpen)) {
    }

    Assert.DoesNotThrow(() => prependStream.ReadByte(), "leave open as default");

    if (leaveOpen)
      Assert.DoesNotThrow(() => appendStream.ReadByte());
    else
      Assert.Throws<ObjectDisposedException>(() => appendStream.ReadByte());
  }

  [Test]
  public void Close_LeaveStreamOpen()
  {
    var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03 });

    using var stream = new ExtendStream(baseStream, Stream.Null, Stream.Null, leaveInnerStreamOpen: true);

    Assert.IsTrue(stream.LeaveInnerStreamOpen);

    Close(stream);
  }

  [Test]
  public void Close_LeaveStreamNotOpen()
  {
    var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03 });

    using var stream = new ExtendStream(baseStream, Stream.Null, Stream.Null, leaveInnerStreamOpen: false);

    Assert.IsFalse(stream.LeaveInnerStreamOpen);

    Close(stream);
  }

  private void Close(ExtendStream stream)
  {
    stream.Dispose();

    Assert.IsFalse(stream.CanRead, nameof(stream.CanRead));
    Assert.IsFalse(stream.CanWrite, nameof(stream.CanWrite));
    Assert.IsFalse(stream.CanSeek, nameof(stream.CanSeek));
    Assert.IsFalse(stream.CanTimeout, nameof(stream.CanTimeout));

    Assert.Throws<ObjectDisposedException>(() => Assert.IsNotNull(stream.InnerStream), nameof(stream.InnerStream));
    Assert.Throws<ObjectDisposedException>(() => Assert.IsNotNull(stream.LeaveInnerStreamOpen), nameof(stream.LeaveInnerStreamOpen));
    Assert.Throws<ObjectDisposedException>(() => stream.ReadByte(), nameof(stream.ReadByte));
    Assert.Throws<ObjectDisposedException>(() => stream.Read(ArrayEmptyShim.Empty<byte>(), 0, 0), nameof(stream.Read));
    Assert.Throws<ObjectDisposedException>(() => stream.ReadAsync(ArrayEmptyShim.Empty<byte>(), 0, 0), nameof(stream.ReadAsync));
    Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0x00), nameof(stream.WriteByte));
    Assert.Throws<ObjectDisposedException>(() => stream.Write(ArrayEmptyShim.Empty<byte>(), 0, 0), nameof(stream.Write));
    Assert.Throws<ObjectDisposedException>(() => stream.WriteAsync(ArrayEmptyShim.Empty<byte>(), 0, 0), nameof(stream.WriteAsync));
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
    Assert.ThrowsAsync<ObjectDisposedException>(async () => await stream.WriteAsync(ReadOnlyMemory<byte>.Empty), nameof(stream.WriteAsync));
#endif
    Assert.Throws<ObjectDisposedException>(() => stream.Flush(), nameof(stream.Flush));
    Assert.Throws<ObjectDisposedException>(() => stream.FlushAsync(), nameof(stream.FlushAsync));
    Assert.Throws<ObjectDisposedException>(() => stream.Seek(0, SeekOrigin.Begin), nameof(stream.Seek));
    Assert.Throws<ObjectDisposedException>(() => stream.SetLength(0L), nameof(stream.SetLength));
    Assert.Throws<ObjectDisposedException>(() => Assert.AreEqual(-1, stream.Length), nameof(stream.Length));
    Assert.Throws<ObjectDisposedException>(() => Assert.AreEqual(-1, stream.Position), nameof(stream.Length));

#if SYSTEM_IO_STREAM_CLOSE
    Assert.DoesNotThrow(() => stream.Close(), nameof(stream.Close));
#endif
  }

  [Test]
  public void Length()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x00, 0x00 });

    Assert.AreEqual(2L, new ExtendStream(innerStream, new byte[0], new byte[0], true).Length);
    Assert.AreEqual(4L, new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[0], true).Length);
    Assert.AreEqual(4L, new ExtendStream(innerStream, new byte[0], new byte[] { 0x00, 0x00 }, true).Length);
    Assert.AreEqual(6L, new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, true).Length);
  }

  [Test]
  public void WritingOperation()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x00, 0x00 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[] { 0x00, 0x00 });

    Assert.IsFalse(extended.CanWrite);

    var len = extended.Length;
    var pos = extended.Position;

    Assert.Throws<NotSupportedException>(() => extended.SetLength(10L));
    Assert.Throws<NotSupportedException>(() => extended.WriteByte(0xff));
    Assert.Throws<NotSupportedException>(() => extended.Write(new byte[] { 0xff, 0xff }, 0, 2));
    Assert.Throws<NotSupportedException>(() => extended.WriteAsync(new byte[] { 0xff, 0xff }, 0, 2));
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
    Assert.ThrowsAsync<NotSupportedException>(async () => await extended.WriteAsync(new ReadOnlyMemory<byte>(new byte[] { 0xff, 0xff }, 0, 2)));
#endif

    Assert.AreEqual(len, extended.Length);
    Assert.AreEqual(pos, extended.Position);
  }

  [Test]
  public void Flush()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x00, 0x00 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[] { 0x00, 0x00 });

    Assert.IsFalse(extended.CanWrite);

    var len = extended.Length;
    var pos = extended.Position;

    Assert.Throws<NotSupportedException>(() => extended.Flush());
    Assert.Throws<NotSupportedException>(() => extended.FlushAsync());

    Assert.AreEqual(len, extended.Length);
    Assert.AreEqual(pos, extended.Position);
  }

  [TestCaseSource(
    typeof(StreamTestCaseSource),
    nameof(StreamTestCaseSource.YieldTestCases_InvalidReadBufferArguments)
  )]
  public void Read_InvalidBufferArguments(
    byte[] buffer,
    int offset,
    int count,
    Type expectedExceptionType
  )
  {
    using var stream = new ExtendStream(Stream.Null, Stream.Null, Stream.Null);

    Assert.Throws(expectedExceptionType, () => stream.Read(buffer, offset, count));
  }

  [TestCaseSource(
    typeof(StreamTestCaseSource),
    nameof(StreamTestCaseSource.YieldTestCases_InvalidReadBufferArguments)
  )]
  public void ReadAsync_InvalidBufferArguments(
    byte[] buffer,
    int offset,
    int count,
    Type expectedExceptionType
  )
  {
    using var stream = new ExtendStream(Stream.Null, Stream.Null, Stream.Null);

    Assert.Throws(expectedExceptionType, () => stream.ReadAsync(buffer, offset, count));
  }

  public enum ReadMethod {
    Read,
    ReadAsyncArray,
    ReadAsyncMemory,
  }

  private static IEnumerable YieldTestCases_Read()
  {
    yield return new object[] { ReadMethod.Read };
    yield return new object[] { ReadMethod.ReadAsyncArray };
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    yield return new object[] { ReadMethod.ReadAsyncMemory };
#endif
  }

  [TestCaseSource(nameof(YieldTestCases_Read))]
  public async Task Read_AcrossRange(ReadMethod readMethod)
  {
    var expected = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b };

    // ExtendStream: 0x00 0x01 0x02 0x03 | 0x04 0x05 0x06 0x07 | 0x08 0x09 0x0a 0x0b
    // read          |--------- 5 bytes ------|
    // read                         |--------- 5 bytes ------|
    // read                                               |--------- 5 bytes ------|
    // read          |----------9 bytes ----------------------------|
    // read                         |----------9 bytes ----------------------------|
    // read                         |----------- 6 bytes -----------|

    foreach (var len in new[] { 5, 6, 9 }) {
      var buffer = new byte[len];

      for (var offset = 0L; offset < 12L; offset++) {
        using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
        using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b });

        extended.Position = offset;

        var ret = readMethod switch {
          ReadMethod.Read => extended.Read(buffer, 0, buffer.Length),
          ReadMethod.ReadAsyncArray => await extended.ReadAsync(buffer, 0, buffer.Length),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncMemory => await extended.ReadAsync(buffer.AsMemory(0, buffer.Length)),
#endif
          _ => throw new InvalidOperationException(),
        };

        var expectedReadLength = Math.Min(buffer.Length, extended.Length - offset);
        var expectedPosition = Math.Min(offset + buffer.Length, extended.Length);

        Assert.AreEqual(expectedReadLength, ret, "read length {0}+{1}", offset, len);
        Assert.AreEqual(
          expected.Skip((int)offset).Take((int)expectedReadLength).ToArray(),
          buffer.Skip(0).Take(ret).ToArray(),
          "read content {0}+{1}", offset, len
        );
        Assert.AreEqual(
          expectedPosition,
          extended.Position,
          "position {0}+{1}", offset, len
        );

        if (ret < len)
          Assert.AreEqual(-1, extended.ReadByte());
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_Read))]
  public async Task Read_AcrossRange_PrependAppendNull(ReadMethod readMethod)
  {
    var expected = new byte[] { 0x04, 0x05, 0x06, 0x07 };

    // ExtendStream: <null> | 0x04 0x05 0x06 0x07 | <null>
    // read          |- 1 bytes -|
    // read                                  |- 1 bytes -|
    // read          |---------- 5 bytes ---------|
    // read                 |---------- 5 bytes ---------|

    foreach (var len in new[] { 1, 4, 5 }) {
      var buffer = new byte[len];

      for (var offset = 0L; offset < 4; offset++) {
        using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
        using var extended = new ExtendStream(innerStream, (byte[])null, (byte[])null);

        extended.Position = offset;

        var ret = readMethod switch {
          ReadMethod.Read => extended.Read(buffer, 0, buffer.Length),
          ReadMethod.ReadAsyncArray => await extended.ReadAsync(buffer, 0, buffer.Length),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          ReadMethod.ReadAsyncMemory => await extended.ReadAsync(buffer.AsMemory(0, buffer.Length)),
#endif
          _ => throw new InvalidOperationException(),
        };

        var expectedReadLength = Math.Min(buffer.Length, extended.Length - offset);
        var expectedPosition = Math.Min(offset + buffer.Length, extended.Length);

        Assert.AreEqual(expectedReadLength, ret, "read length {0}+{1}", offset, len);
        Assert.AreEqual(
          expected.Skip((int)offset).Take((int)expectedReadLength).ToArray(),
          buffer.Skip(0).Take(ret).ToArray(),
          "read content {0}+{1}", offset, len
        );
        Assert.AreEqual(
          expectedPosition,
          extended.Position,
          "position {0}+{1}", offset, len
        );

        if (ret < len)
          Assert.AreEqual(-1, extended.ReadByte());
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_Read))]
  public async Task Read_ExtendedStream(ReadMethod readMethod)
  {
    var expected = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0xff, 0xff, 0xff, 0xff };

    for (var len = 0; len < 13; len++) {
      var buffer = new byte[len];

      using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
      using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b });

      var ret = readMethod switch {
        ReadMethod.Read => extended.Read(buffer, 0, len),
        ReadMethod.ReadAsyncArray => await extended.ReadAsync(buffer, 0, len),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
        ReadMethod.ReadAsyncMemory => await extended.ReadAsync(buffer.AsMemory(0, len)),
#endif
        _ => throw new InvalidOperationException(),
      };

      if (extended.Length < len) {
        Assert.AreEqual(extended.Length, ret, "read length {0}", len);
        Assert.AreEqual(expected.Skip(0).Take((int)extended.Length).ToArray(), buffer, "read content {0}", len);
        Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
        Assert.AreEqual(-1, extended.ReadByte());
      }
      else {
        Assert.AreEqual(len, ret, "read length {0}", len);
        Assert.AreEqual(expected.Skip(0).Take(len).ToArray(), buffer, "read content {0}", len);
        Assert.AreEqual(len, extended.Position, "position {0}", len);
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_Read))]
  public async Task Read_PrependedStream(ReadMethod readMethod)
  {
    var expected = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0xff, 0xff, 0xff, 0xff };

    for (var len = 0; len < 9; len++) {
      var buffer = new byte[len];

      using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
      using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, null);

      var ret = readMethod switch {
        ReadMethod.Read => extended.Read(buffer, 0, len),
        ReadMethod.ReadAsyncArray => await extended.ReadAsync(buffer, 0, len),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
        ReadMethod.ReadAsyncMemory => await extended.ReadAsync(buffer.AsMemory(0, len)),
#endif
        _ => throw new InvalidOperationException(),
      };

      if (extended.Length < len) {
        Assert.AreEqual(extended.Length, ret, "read length {0}", len);
        Assert.AreEqual(expected.Skip(0).Take((int)extended.Length).ToArray(), buffer, "read content {0}", len);
        Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
        Assert.AreEqual(-1, extended.ReadByte());
      }
      else {
        Assert.AreEqual(len, ret, "read length {0}", len);
        Assert.AreEqual(expected.Skip(0).Take(len).ToArray(), buffer, "read content {0}", len);
        Assert.AreEqual(len, extended.Position, "position {0}", len);
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_Read))]
  public async Task Read_AppendedStream(ReadMethod readMethod)
  {
    var expected = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0xff, 0xff, 0xff, 0xff };

    for (var len = 0; len < 9; len++) {
      var buffer = new byte[len];

      using var innerStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03 });
      using var extended = new ExtendStream(innerStream, null, new byte[] { 0x04, 0x05, 0x06, 0x07 });

      var ret = readMethod switch {
        ReadMethod.Read => extended.Read(buffer, 0, len),
        ReadMethod.ReadAsyncArray => await extended.ReadAsync(buffer, 0, len),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
        ReadMethod.ReadAsyncMemory => await extended.ReadAsync(buffer.AsMemory(0, len)),
#endif
        _ => throw new InvalidOperationException(),
      };

      if (extended.Length < len) {
        Assert.AreEqual(extended.Length, ret, "read length {0}", len);
        Assert.AreEqual(expected.Skip(0).Take((int)extended.Length).ToArray(), buffer, "read content {0}", len);
        Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
        Assert.AreEqual(-1, extended.ReadByte());
      }
      else {
        Assert.AreEqual(len, ret, "read length {0}", len);
        Assert.AreEqual(expected.Skip(0).Take(len).ToArray(), buffer, "read content {0}", len);
        Assert.AreEqual(len, extended.Position, "position {0}", len);
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_Read))]
  public async Task Read_NonExtendedStream(ReadMethod readMethod)
  {
    var expected = new byte[] { 0x00, 0x01, 0x02, 0x03, 0xff, 0xff, 0xff, 0xff };

    for (var len = 0; len < 5; len++) {
      var buffer = new byte[len];

      using var innerStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03 });
      using var extended = new ExtendStream(innerStream, (Stream)null, (Stream)null);

      var ret = readMethod switch {
        ReadMethod.Read => extended.Read(buffer, 0, len),
        ReadMethod.ReadAsyncArray => await extended.ReadAsync(buffer, 0, len),
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
        ReadMethod.ReadAsyncMemory => await extended.ReadAsync(buffer.AsMemory(0, len)),
#endif
        _ => throw new InvalidOperationException(),
      };

      if (extended.Length < len) {
        Assert.AreEqual(extended.Length, ret, "read length {0}", len);
        Assert.AreEqual(expected.Skip(0).Take((int)extended.Length).ToArray(), buffer, "read content {0}", len);
        Assert.AreEqual(extended.Length, extended.Position, "position {0}", len);
        Assert.AreEqual(-1, extended.ReadByte());
      }
      else {
        Assert.AreEqual(len, ret, "read length {0}", len);
        Assert.AreEqual(expected.Skip(0).Take(len).ToArray(), buffer, "read content {0}", len);
        Assert.AreEqual(len, extended.Position, "position {0}", len);
      }
    }
  }

  [Test]
  public void ReadByte()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b });

    Assert.AreEqual(12L, extended.Length);
    Assert.AreEqual(0L, extended.Position);
    Assert.IsTrue(extended.CanRead);

    for (var expected = 0L; expected < 12L; expected++) {
      Assert.AreEqual(expected, extended.ReadByte(), "offset: {0}", expected);
      Assert.AreEqual(expected + 1, extended.Position, "position {0}", expected);
    }

    Assert.AreEqual(-1, extended.ReadByte(), "end of stream");
  }

  [Test]
  public void SetPosition()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b });

    Assert.Throws<ArgumentOutOfRangeException>(() => extended.Position = -1L);

    foreach (var expected in new long[] {
      0, 11, 1, 10, 2, 9, 3, 8, 4, 7, 5, 6,
    }) {
      extended.Position = expected;

      Assert.AreEqual(expected, extended.Position);
      Assert.AreEqual(expected, extended.ReadByte());
      Assert.AreEqual(expected + 1L, extended.Position);
    }

    Assert.DoesNotThrow(() => extended.Position = 13L);
    Assert.AreEqual(13L, extended.Position);
  }

  [Test]
  public void Seek_SeekOriginCurrent()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b });

    Assert.IsTrue(extended.CanSeek);

    extended.Position = 6L;

    Assert.Throws<IOException>(() => extended.Seek(-7, SeekOrigin.Current));

    Assert.AreEqual(6L, extended.Position);

    var pair = new long[][] {
      // offset / position
      new long[] {  0, 6 },
      new long[] { -2, 5 },
      new long[] {  1, 7 },
      new long[] { -4, 4 },
      new long[] {  3, 8 },
      new long[] { -6, 3 },
      new long[] {  5, 9 },
      new long[] { -8, 2 },
      new long[] {  7, 10 },
      new long[] { -10, 1 },
      new long[] {  9, 11 },
    };

    for (var index = 0; index < pair.Length; index++) {
      Assert.AreEqual(pair[index][1], extended.Seek(pair[index][0], SeekOrigin.Current), "seeked position {0}", index);

      Assert.AreEqual(pair[index][1], extended.ReadByte(), "read value {0}", index);
    }

    Assert.AreEqual(-1, extended.ReadByte());
    Assert.AreEqual(13, extended.Seek(1, SeekOrigin.Current));
    Assert.AreEqual(-1, extended.ReadByte());
  }

  [Test]
  public void Seek_SeekOriginBegin()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b });

    Assert.IsTrue(extended.CanSeek);

    foreach (var offset in new long[] {
      0, 11, 1, 10, 2, 9, 3, 8, 4, 7, 5, 6,
    }) {
      Assert.AreEqual(offset, extended.Seek(offset, SeekOrigin.Begin), "seeking to {0}", offset);
      Assert.AreEqual(offset, extended.Position);
      Assert.AreEqual(offset, extended.ReadByte());
    }

    extended.Position = 6L;

    Assert.Throws<IOException>(() => extended.Seek(-1L, SeekOrigin.Begin));

    Assert.AreEqual(6L, extended.Position);

    Assert.AreEqual(12L, extended.Seek(12L, SeekOrigin.Begin));
    Assert.AreEqual(-1, extended.ReadByte());
  }

  [Test]
  public void Seek_SeekOriginEnd()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b });

    Assert.IsTrue(extended.CanSeek);

    foreach (var offset in new long[] {
      0, 11, 1, 10, 2, 9, 3, 8, 4, 7, 5, 6,
    }) {
      Assert.AreEqual(offset, extended.Seek(-extended.Length + offset, SeekOrigin.End), "seeking to {0}", offset);
      Assert.AreEqual(offset, extended.Position);
      Assert.AreEqual(offset, extended.ReadByte());
    }

    extended.Position = 6L;

    Assert.Throws<IOException>(() => extended.Seek(-13L, SeekOrigin.End));

    Assert.AreEqual(6L, extended.Position);

    Assert.AreEqual(12L, extended.Seek(0L, SeekOrigin.End));
    Assert.AreEqual(-1, extended.ReadByte());
  }
}
