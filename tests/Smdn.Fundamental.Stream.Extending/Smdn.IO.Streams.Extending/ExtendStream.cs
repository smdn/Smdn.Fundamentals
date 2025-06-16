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
    Assert.That(new ExtendStream(Stream.Null, (byte[])null, null).Length,Is.Zero, "construct null/null");
    Assert.That(new ExtendStream(Stream.Null, new byte[0], null).Length,Is.Zero, "construct byte[]/null");
    Assert.That(new ExtendStream(Stream.Null, null, new byte[0]).Length,Is.Zero, "construct null/byte[]");
    Assert.That(new ExtendStream(Stream.Null, new byte[0], new byte[0]).Length,Is.Zero, "construct byte[]/byte[]");
    Assert.That(new ExtendStream(Stream.Null, Stream.Null, null).Length,Is.Zero, "construct Stream/null");
    Assert.That(new ExtendStream(Stream.Null, null, Stream.Null).Length,Is.Zero, "construct null/Stream");
    Assert.That(new ExtendStream(Stream.Null, Stream.Null, Stream.Null).Length,Is.Zero, "construct Stream/Stream");
  }

  [Test]
  public void Construct_StreamNull()
  {
    Assert.Throws<ArgumentNullException>(() => new ExtendStream(null!, Stream.Null, Stream.Null));
    Assert.Throws<ArgumentNullException>(() => new ExtendStream(null!, new byte[0], new byte[0]));
  }

  [TestCase(true)]
  [TestCase(false)]
  public void Construct_LeaveInnerStreamOpen(bool leaveOpen)
  {
    var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03 });

    using (var stream = new ExtendStream(baseStream, Stream.Null, Stream.Null, leaveInnerStreamOpen: leaveOpen)) {
      Assert.That(stream.LeaveInnerStreamOpen, Is.EqualTo(leaveOpen));
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
  public void Construct_LeaveAppendStreamOpen(bool leaveOpen)
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

    Assert.That(stream.LeaveInnerStreamOpen, Is.True);

    Close(stream);
  }

  [Test]
  public void Close_LeaveStreamNotOpen()
  {
    var baseStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03 });

    using var stream = new ExtendStream(baseStream, Stream.Null, Stream.Null, leaveInnerStreamOpen: false);

    Assert.That(stream.LeaveInnerStreamOpen, Is.False);

    Close(stream);
  }

  private void Close(ExtendStream stream)
  {
    stream.Dispose();

    Assert.That(stream.CanRead, Is.False, nameof(stream.CanRead));
    Assert.That(stream.CanWrite, Is.False, nameof(stream.CanWrite));
    Assert.That(stream.CanSeek, Is.False, nameof(stream.CanSeek));
    Assert.That(stream.CanTimeout, Is.False, nameof(stream.CanTimeout));

    Assert.Throws<ObjectDisposedException>(() => Assert.That(stream.InnerStream, Is.Not.Null), nameof(stream.InnerStream));
    Assert.Throws<ObjectDisposedException>(() => Assert.That(stream.LeaveInnerStreamOpen, Is.Not.Zero), nameof(stream.LeaveInnerStreamOpen));
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
    Assert.Throws<ObjectDisposedException>(() => Assert.That(stream.Length, Is.EqualTo(-1)), nameof(stream.Length));
    Assert.Throws<ObjectDisposedException>(() => Assert.That(stream.Position, Is.EqualTo(-1)), nameof(stream.Length));

#if SYSTEM_IO_STREAM_CLOSE
    Assert.DoesNotThrow(() => stream.Close(), nameof(stream.Close));
#endif
  }

  [Test]
  public void Length()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x00, 0x00 });

    Assert.That(new ExtendStream(innerStream, new byte[0], new byte[0], true).Length, Is.EqualTo(2L));
    Assert.That(new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[0], true).Length, Is.EqualTo(4L));
    Assert.That(new ExtendStream(innerStream, new byte[0], new byte[] { 0x00, 0x00 }, true).Length, Is.EqualTo(4L));
    Assert.That(new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[] { 0x00, 0x00 }, true).Length, Is.EqualTo(6L));
  }

  [Test]
  public void WritingOperation()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x00, 0x00 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[] { 0x00, 0x00 });

    Assert.That(extended.CanWrite, Is.False);

    var len = extended.Length;
    var pos = extended.Position;

    Assert.Throws<NotSupportedException>(() => extended.SetLength(10L));
    Assert.Throws<NotSupportedException>(() => extended.WriteByte(0xff));
    Assert.Throws<NotSupportedException>(() => extended.Write(new byte[] { 0xff, 0xff }, 0, 2));
    Assert.Throws<NotSupportedException>(() => extended.WriteAsync(new byte[] { 0xff, 0xff }, 0, 2));
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
    Assert.ThrowsAsync<NotSupportedException>(async () => await extended.WriteAsync(new ReadOnlyMemory<byte>(new byte[] { 0xff, 0xff }, 0, 2)));
#endif

    Assert.That(extended.Length, Is.EqualTo(len));
    Assert.That(extended.Position, Is.EqualTo(pos));
  }

  [Test]
  public void Flush()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x00, 0x00 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x00 }, new byte[] { 0x00, 0x00 });

    Assert.That(extended.CanWrite, Is.False);

    var len = extended.Length;
    var pos = extended.Position;

    Assert.Throws<NotSupportedException>(() => extended.Flush());
    Assert.Throws<NotSupportedException>(() => extended.FlushAsync());

    Assert.That(extended.Length, Is.EqualTo(len));
    Assert.That(extended.Position, Is.EqualTo(pos));
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

        Assert.That(ret, Is.EqualTo(expectedReadLength), $"read length {offset}+{len}");
        Assert.That(
          buffer.Skip(0).Take(ret).ToArray(), Is.EqualTo(expected.Skip((int)offset).Take((int)expectedReadLength).ToArray()), $"read content {offset}+{len}");
        Assert.That(
          extended.Position, Is.EqualTo(expectedPosition), $"position {offset}+{len}");

        if (ret < len)
          Assert.That(extended.ReadByte(), Is.EqualTo(-1));
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

        Assert.That(ret, Is.EqualTo(expectedReadLength), $"read length {offset}+{len}");
        Assert.That(
          buffer.Skip(0).Take(ret).ToArray(), Is.EqualTo(expected.Skip((int)offset).Take((int)expectedReadLength).ToArray()), $"read content {offset}+{len}");
        Assert.That(
          extended.Position, Is.EqualTo(expectedPosition), $"position {offset}+{len}");

        if (ret < len)
          Assert.That(extended.ReadByte(), Is.EqualTo(-1));
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
        Assert.That(ret, Is.EqualTo(extended.Length), $"read length {len}");
        Assert.That(buffer, Is.EqualTo(expected.Skip(0).Take((int)extended.Length).ToArray()), $"read content {len}");
        Assert.That(extended.Position, Is.EqualTo(extended.Length), $"position {len}");
        Assert.That(extended.ReadByte(), Is.EqualTo(-1));
      }
      else {
        Assert.That(ret, Is.EqualTo(len), $"read length {len}");
        Assert.That(buffer, Is.EqualTo(expected.Skip(0).Take(len).ToArray()), $"read content {len}");
        Assert.That(extended.Position, Is.EqualTo(len), $"position {len}");
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
        Assert.That(ret, Is.EqualTo(extended.Length), $"read length {len}");
        Assert.That(buffer, Is.EqualTo(expected.Skip(0).Take((int)extended.Length).ToArray()), $"read content {len}");
        Assert.That(extended.Position, Is.EqualTo(extended.Length), $"position {len}");
        Assert.That(extended.ReadByte(), Is.EqualTo(-1));
      }
      else {
        Assert.That(ret, Is.EqualTo(len), $"read length {len}");
        Assert.That(buffer, Is.EqualTo(expected.Skip(0).Take(len).ToArray()), $"read content {len}");
        Assert.That(extended.Position, Is.EqualTo(len), $"position {len}");
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
        Assert.That(ret, Is.EqualTo(extended.Length), $"read length {len}");
        Assert.That(buffer, Is.EqualTo(expected.Skip(0).Take((int)extended.Length).ToArray()), $"read content {len}");
        Assert.That(extended.Position, Is.EqualTo(extended.Length), $"position {len}");
        Assert.That(extended.ReadByte(), Is.EqualTo(-1));
      }
      else {
        Assert.That(ret, Is.EqualTo(len), $"read length {len}");
        Assert.That(buffer, Is.EqualTo(expected.Skip(0).Take(len).ToArray()), $"read content {len}");
        Assert.That(extended.Position, Is.EqualTo(len), $"position {len}");
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
        Assert.That(ret, Is.EqualTo(extended.Length), $"read length {len}");
        Assert.That(buffer, Is.EqualTo(expected.Skip(0).Take((int)extended.Length).ToArray()), $"read content {len}");
        Assert.That(extended.Position, Is.EqualTo(extended.Length), $"position {len}");
        Assert.That(extended.ReadByte(), Is.EqualTo(-1));
      }
      else {
        Assert.That(ret, Is.EqualTo(len), $"read length {len}");
        Assert.That(buffer, Is.EqualTo(expected.Skip(0).Take(len).ToArray()), $"read content {len}");
        Assert.That(extended.Position, Is.EqualTo(len), $"position {len}");
      }
    }
  }

  [Test]
  public void ReadByte()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b });

    Assert.That(extended.Length, Is.EqualTo(12L));
    Assert.That(extended.Position,Is.Zero);
    Assert.That(extended.CanRead, Is.True);

    for (var expected = 0L; expected < 12L; expected++) {
      Assert.That(extended.ReadByte(), Is.EqualTo(expected), $"offset: {expected}");
      Assert.That(extended.Position, Is.EqualTo(expected + 1), $"position {expected}");
    }

    Assert.That(extended.ReadByte(), Is.EqualTo(-1), "end of stream");
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

      Assert.That(extended.Position, Is.EqualTo(expected));
      Assert.That(extended.ReadByte(), Is.EqualTo(expected));
      Assert.That(extended.Position, Is.EqualTo(expected + 1L));
    }

    Assert.DoesNotThrow(() => extended.Position = 13L);
    Assert.That(extended.Position, Is.EqualTo(13L));
  }

  [Test]
  public void Seek_SeekOriginCurrent()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b });

    Assert.That(extended.CanSeek, Is.True);

    extended.Position = 6L;

    Assert.Throws<IOException>(() => extended.Seek(-7, SeekOrigin.Current));

    Assert.That(extended.Position, Is.EqualTo(6L));

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
      Assert.That(extended.Seek(pair[index][0], SeekOrigin.Current), Is.EqualTo(pair[index][1]), $"sought position {index}");

      Assert.That(extended.ReadByte(), Is.EqualTo(pair[index][1]), $"read value {index}");
    }

    Assert.That(extended.ReadByte(), Is.EqualTo(-1));
    Assert.That(extended.Seek(1, SeekOrigin.Current), Is.EqualTo(13));
    Assert.That(extended.ReadByte(), Is.EqualTo(-1));
  }

  [Test]
  public void Seek_SeekOriginBegin()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b });

    Assert.That(extended.CanSeek, Is.True);

    foreach (var offset in new long[] {
      0, 11, 1, 10, 2, 9, 3, 8, 4, 7, 5, 6,
    }) {
      Assert.That(extended.Seek(offset, SeekOrigin.Begin), Is.EqualTo(offset), $"seeking to {offset}");
      Assert.That(extended.Position, Is.EqualTo(offset));
      Assert.That(extended.ReadByte(), Is.EqualTo(offset));
    }

    extended.Position = 6L;

    Assert.Throws<IOException>(() => extended.Seek(-1L, SeekOrigin.Begin));

    Assert.That(extended.Position, Is.EqualTo(6L));

    Assert.That(extended.Seek(12L, SeekOrigin.Begin), Is.EqualTo(12L));
    Assert.That(extended.ReadByte(), Is.EqualTo(-1));
  }

  [Test]
  public void Seek_SeekOriginEnd()
  {
    using var innerStream = new MemoryStream(new byte[] { 0x04, 0x05, 0x06, 0x07 });
    using var extended = new ExtendStream(innerStream, new byte[] { 0x00, 0x01, 0x02, 0x03 }, new byte[] { 0x08, 0x09, 0x0a, 0x0b });

    Assert.That(extended.CanSeek, Is.True);

    foreach (var offset in new long[] {
      0, 11, 1, 10, 2, 9, 3, 8, 4, 7, 5, 6,
    }) {
      Assert.That(extended.Seek(-extended.Length + offset, SeekOrigin.End), Is.EqualTo(offset), $"seeking to {offset}");
      Assert.That(extended.Position, Is.EqualTo(offset));
      Assert.That(extended.ReadByte(), Is.EqualTo(offset));
    }

    extended.Position = 6L;

    Assert.Throws<IOException>(() => extended.Seek(-13L, SeekOrigin.End));

    Assert.That(extended.Position, Is.EqualTo(6L));

    Assert.That(extended.Seek(0L, SeekOrigin.End), Is.EqualTo(12L));
    Assert.That(extended.ReadByte(), Is.EqualTo(-1));
  }
}
