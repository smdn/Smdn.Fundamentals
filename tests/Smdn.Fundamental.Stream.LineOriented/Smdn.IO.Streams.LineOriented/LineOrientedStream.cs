// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

using Is = Smdn.Test.NUnit.Constraints.Buffers.Is;

namespace Smdn.IO.Streams.LineOriented;

[TestFixture]
public class LineOrientedStreamTests {
  private const byte CR = (byte)'\r';
  private const byte LF = (byte)'\n';

  public enum StreamType {
    Strict,
    Loose,
  }

  private static LineOrientedStream CreateStream(
    StreamType type,
    Stream baseStream,
    int bufferSize,
    bool leaveStreamOpen = false
  )
    => type switch {
      StreamType.Loose => new LooseLineOrientedStream(baseStream, bufferSize, leaveStreamOpen),
      StreamType.Strict => new StrictLineOrientedStream(baseStream, bufferSize, leaveStreamOpen),
      _ => throw new InvalidOperationException("invalid stream type"),
    };

  [Test]
  public void Construct_FromMemoryStream(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    Assert.IsTrue(stream.CanRead, nameof(stream.CanRead));
    Assert.IsTrue(stream.CanWrite, nameof(stream.CanWrite));
    Assert.IsTrue(stream.CanSeek, nameof(stream.CanSeek));
    Assert.IsFalse(stream.CanTimeout, nameof(stream.CanTimeout));
    Assert.AreEqual(8L, stream.Length, nameof(stream.Length));
    Assert.AreEqual(0L, stream.Position, nameof(stream.Position));
    Assert.AreEqual(bufferSize, stream.BufferSize, nameof(stream.BufferSize));
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
    foreach (var type in new[] { StreamType.Strict, StreamType.Loose }) {
      using var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws(expectedExceptionType, () => stream.Read(buffer, offset, count), $"type = {type}");
    }
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
    foreach (var type in new[] { StreamType.Strict, StreamType.Loose }) {
      using var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws(expectedExceptionType, () => stream.ReadAsync(buffer, offset, count), $"type = {type}");
    }
  }

  [Test]
  public async Task ReadLineAsync_EndOfStream(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43};

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    stream.CopyTo(Stream.Null);

    var ret = await stream.ReadLineAsync();

    Assert.IsNull(ret);
  }

  [Test]
  public async Task ReadLineAsync_EndOfStream_NothingBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, Stream.Null, 8);

    var ret = await stream.ReadLineAsync();

    Assert.IsNull(ret);
  }

  [Test]
  public async Task ReadLineAsync_SingleSegment(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(6, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF};

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var ret = await stream.ReadLineAsync();

    Assert.IsNotNull(ret);
    Assert.IsFalse(ret.Value.IsEmpty);
    Assert.IsTrue(ret.Value.SequenceWithNewLine.IsSingleSegment);
    Assert.IsTrue(ret.Value.Sequence.IsSingleSegment);
    CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43, CR, LF },
                              ret.Value.SequenceWithNewLine.ToArray());
    CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43 },
                              ret.Value.Sequence.ToArray());
  }

  [Test]
  public async Task ReadLineAsync_MultipleSegment(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, CR, LF};

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var ret = await stream.ReadLineAsync();

    Assert.IsNotNull(ret);
    Assert.IsFalse(ret.Value.IsEmpty);
    Assert.IsFalse(ret.Value.SequenceWithNewLine.IsSingleSegment);
    Assert.IsFalse(ret.Value.Sequence.IsSingleSegment);
    CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, CR, LF },
                              ret.Value.SequenceWithNewLine.ToArray());
    CollectionAssert.AreEqual(new byte[] { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f },
                              ret.Value.Sequence.ToArray());
  }

  [Test]
  public void ReadByte(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 3, 4, 5, 6)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var index = 0;

    for (; ; ) {
      var val = stream.ReadByte();

      if (index == data.Length) {
        Assert.AreEqual(-1, val);
      }
      else {
        Assert.AreEqual(data[index++], val, "data");
        Assert.AreEqual(index, stream.Position, "Position");
      }

      if (val == -1)
        break;
    }
  }

  [Test]
  public void ReadToStream_ArgumentNull_TargetStream(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    Assert.Throws<ArgumentNullException>(() => stream.Read(null, 16L));
    Assert.Throws<ArgumentNullException>(() => stream.ReadAsync(null, 16L));
  }

  [Test]
  public void ReadToStream_ArgumentOutOfRange_Length(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(Stream.Null, -1L));
    Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadAsync(Stream.Null, -1L));
  }

  [Test]
  public void ReadToStream_LengthZero(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    var copyStream = new MemoryStream();

    Assert.AreEqual(0L, stream.Read(copyStream, 0L));
    Assert.AreEqual(0L, stream.Position, "Position");

    copyStream.Dispose();

    Assert.AreEqual(new byte[0], copyStream.ToArray());
  }

  [Test]
  public void ReadToStreamAsync_LengthZero(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    var copyStream = new MemoryStream();

    var t = stream.ReadAsync(copyStream, 0L);

    Assert.IsTrue(t.IsCompleted);
    Assert.AreEqual(0L, t.GetAwaiter().GetResult());
    Assert.AreEqual(0L, stream.Position, "Position");

    copyStream.Dispose();

    Assert.AreEqual(new byte[0], copyStream.ToArray());
  }

  [Test]
  public void ReadToStreamAsync_CancelledToken(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    using var cts = new CancellationTokenSource();

    cts.Cancel();

    var t = stream.ReadAsync(Stream.Null, 0L, cts.Token);

    Assert.IsTrue(t.IsCanceled);
    Assert.That(async () => await t, Throws.InstanceOf<OperationCanceledException>());

    Assert.AreEqual(0L, stream.Position, "Position");
  }

  [Test]
  public void ReadToStream_BufferEmpty(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var copyStream = new MemoryStream();

    Assert.AreEqual(12L, stream.Read(copyStream, 12L));

    Assert.AreEqual(12L, stream.Position, "Position");

    copyStream.Dispose();

    Assert.AreEqual(data, copyStream.ToArray());
  }

  [Test]
  public void Read_BufferEmpty(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var buffer = new byte[12];

    Assert.AreEqual(12L, stream.Read(buffer, 0, 12));

    Assert.AreEqual(12L, stream.Position, "Position");

    Assert.AreEqual(data, buffer);
  }

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  [Test]
  public async Task ReadAsync_ToMemory_BufferEmpty(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    Memory<byte> buffer = new byte[12];

    Assert.AreEqual(12L, await stream.ReadAsync(buffer));

    Assert.AreEqual(12L, stream.Position, "Position");

    Assert.That(buffer, Is.EqualTo(data.AsMemory()), nameof(buffer));
  }
#endif

  [Test]
  public void Read_LessThanBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(16, 32)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), 16);

    var line = stream.ReadLine(true);

    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(4).ToArray(), line);

    var buffer = new byte[4];

    Assert.AreEqual(4, stream.Read(buffer, 0, 4));

    Assert.AreEqual(8L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(4).Take(4).ToArray(), buffer);
  }

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  [Test]
  public async Task ReadAsync_ToMemory_LessThanBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(16, 32)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(4).ToArray(), line);

    Memory<byte> buffer = new byte[4];

    Assert.AreEqual(4, await stream.ReadAsync(buffer));

    Assert.AreEqual(8L, stream.Position, "Position");

    Assert.That(buffer, Is.EqualTo(data.AsMemory(4, 4)), nameof(buffer));
  }
#endif

  [Test]
  public void Read_LongerThanBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(4).ToArray(), line);

    var buffer = new byte[10];

    Assert.AreEqual(8, stream.Read(buffer, 0, 10));

    Assert.AreEqual(12L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(4).Take(8).ToArray(), buffer.Skip(0).Take(8).ToArray());
  }

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  [Test]
  public async Task ReadAsync_ToMemory_LongerThanBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(4).ToArray(), line);

    Memory<byte> buffer = new byte[10];

    Assert.AreEqual(8, await stream.ReadAsync(buffer));

    Assert.AreEqual(12L, stream.Position, "Position");

    Assert.That(buffer.Slice(0, 8), Is.EqualTo(data.AsMemory(4, 8)), nameof(buffer));
  }
#endif

  [Test]
  public void ReadToStream_LessThanBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(16, 32)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(4).ToArray(), line);

    var copyStream = new MemoryStream();

    Assert.AreEqual(4L, stream.Read(copyStream, 4L));

    Assert.AreEqual(8L, stream.Position, "Position");

    copyStream.Dispose();

    Assert.AreEqual(data.Skip(4).Take(4).ToArray(), copyStream.ToArray());
  }

  [Test]
  public void ReadToStream_LongerThanBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(4).ToArray(), line);

    var copyStream = new MemoryStream();

    Assert.AreEqual(8L, stream.Read(copyStream, 10L));

    Assert.AreEqual(12L, stream.Position, "Position");

    copyStream.Dispose();

    Assert.AreEqual(data.Skip(4).Take(8).ToArray(), copyStream.ToArray());
  }

  [Test]
  public void Read_LengthZero(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    var buffer = new byte[1];

    Assert.AreEqual(0, stream.Read(buffer, 0, 0));

    Assert.AreEqual(0L, stream.Position, "Position");
  }

  [Test]
  public void ReadAsync_LengthZero(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    var buffer = new byte[1];

    var t = stream.ReadAsync(buffer, 0, 0);

    Assert.IsTrue(t.IsCompleted);
    Assert.AreEqual(0L, t.GetAwaiter().GetResult());
    Assert.AreEqual(0L, stream.Position, "Position");
  }

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  [Test]
  public async Task ReadAsync_ToMemory_LengthZero(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    Assert.AreEqual(0L, await stream.ReadAsync(Memory<byte>.Empty));
    Assert.AreEqual(0L, stream.Position, "Position");
  }
#endif

  [Test]
  public void ReadAsync_CancelledToken(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    using var cts = new CancellationTokenSource();

    cts.Cancel();

    var t = stream.ReadAsync(new byte[1], 0, 1, cts.Token);

    Assert.IsTrue(t.IsCanceled);
    Assert.That(async () => await t, Throws.InstanceOf<OperationCanceledException>());

    Assert.AreEqual(0L, stream.Position, "Position");
  }

  [TestCaseSource(
    typeof(StreamTestCaseSource),
    nameof(StreamTestCaseSource.YieldTestCases_InvalidCopyToArguments)
  )]
  public void CopyTo_InvalidBufferArguments(
    Stream destination,
    int bufferSize,
    Type expectedExceptionType,
    string message
  )
  {
    foreach (var type in new[] { StreamType.Strict, StreamType.Loose }) {
      using var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws(expectedExceptionType, () => stream.CopyTo(destination, bufferSize), $"{message} (type = {type})");
    }
  }

  [TestCaseSource(
    typeof(StreamTestCaseSource),
    nameof(StreamTestCaseSource.YieldTestCases_InvalidCopyToArguments)
  )]
  public void CopyToAsync_InvalidBufferArguments(
    Stream destination,
    int bufferSize,
    Type expectedExceptionType,
    string message
  )
  {
    foreach (var type in new[] { StreamType.Strict, StreamType.Loose }) {
      using var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws(expectedExceptionType, () => stream.CopyToAsync(destination, bufferSize), $"{message} (type = {type})");
    }
  }

  [Test]
  public async Task CopyToAsync(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
    };

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    Assert.AreEqual(data[0], stream.ReadByte());

    var targetStream = new MemoryStream();

    await stream.CopyToAsync(targetStream, cancellationToken: default);

    CollectionAssert.AreEqual(data.Skip(1).ToArray(), targetStream.ToArray());

    Assert.AreEqual(-1, stream.ReadByte());
  }

  [Test]
  public async Task CopyToAsync_NothingBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
    };

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var targetStream = new MemoryStream();

    await stream.CopyToAsync(targetStream, cancellationToken: default);

    CollectionAssert.AreEqual(data, targetStream.ToArray());

    Assert.AreEqual(-1, stream.ReadByte());
  }

  [Test]
  public void CopyToAsync_ArgumentNull_Destination(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null));
    Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null, bufferSize: 0));
    Assert.Throws<ArgumentNullException>(() => stream.CopyToAsync(destination: null, bufferSize: 0, cancellationToken: default));
  }

  [Test]
  public async Task CopyToAsync_BufferSizeDoesNotAffectToBehaviour(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 1024, int.MaxValue)] int bufferSize
  )
  {
    var data = new byte[] {
      0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
      0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
    };

    using var stream = CreateStream(type, new MemoryStream(data), 8);

    var targetStream = new MemoryStream();

    await stream.CopyToAsync(targetStream, bufferSize: bufferSize, cancellationToken: default);

    CollectionAssert.AreEqual(data, targetStream.ToArray());

    Assert.AreEqual(-1, stream.ReadByte());
  }

  [TestCaseSource(
    typeof(StreamTestCaseSource),
    nameof(StreamTestCaseSource.YieldTestCases_InvalidWriteBufferArguments)
  )]
  public void Write_InvalidBufferArguments(
    byte[] buffer,
    int offset,
    int count,
    Type expectedExceptionType
  )
  {
    foreach (var type in new[] { StreamType.Strict, StreamType.Loose }) {
      using var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws(expectedExceptionType, () => stream.Write(buffer, offset, count), $"type = {type}");
    }
  }

  [TestCaseSource(
    typeof(StreamTestCaseSource),
    nameof(StreamTestCaseSource.YieldTestCases_InvalidWriteBufferArguments)
  )]
  public void WriteAsync_InvalidBufferArguments(
    byte[] buffer,
    int offset,
    int count,
    Type expectedExceptionType
  )
  {
    foreach (var type in new[] { StreamType.Strict, StreamType.Loose }) {
      using var stream = CreateStream(type, new MemoryStream(), 8);

      Assert.Throws(expectedExceptionType, () => stream.WriteAsync(buffer, offset, count), $"type = {type}");
    }
  }

  [Test]
  public void Write(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var baseStream = new MemoryStream(capacity: 8);
    using var stream = CreateStream(type, baseStream, 8);

    Assert.IsTrue(stream.CanWrite, nameof(stream.CanWrite));

    var data = new byte[] { 0x00, 0x01, 0x02, 0x03 };

    stream.Write(data, 0, data.Length);

    Assert.AreEqual(4L, stream.Length, nameof(stream.Length));
    Assert.AreEqual(4L, stream.Position, nameof(stream.Position));

    Assert.That(baseStream.ToArray().AsMemory(), Is.EqualTo(data.AsMemory()));
  }

  [Test]
  public async Task WriteAsync(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var baseStream = new MemoryStream(capacity: 8);
    using var stream = CreateStream(type, baseStream, 8);

    Assert.IsTrue(stream.CanWrite, nameof(stream.CanWrite));

    var data = new byte[] { 0x00, 0x01, 0x02, 0x03 };

    await stream.WriteAsync(data, 0, data.Length);

    Assert.AreEqual(4L, stream.Length, nameof(stream.Length));
    Assert.AreEqual(4L, stream.Position, nameof(stream.Position));

    Assert.That(baseStream.ToArray().AsMemory(), Is.EqualTo(data.AsMemory()));
  }

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
  [Test]
  public async Task WriteAsync_ToReadOnlyMemory(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var baseStream = new MemoryStream(capacity: 8);
    using var stream = CreateStream(type, baseStream, 8);

    Assert.IsTrue(stream.CanWrite, nameof(stream.CanWrite));

    ReadOnlyMemory<byte> data = new byte[] { 0x00, 0x01, 0x02, 0x03 };

    await stream.WriteAsync(data);

    Assert.AreEqual(4L, stream.Length, nameof(stream.Length));
    Assert.AreEqual(4L, stream.Position, nameof(stream.Position));

    Assert.That(baseStream.ToArray().AsMemory(), Is.EqualTo(data));
  }
#endif

  [Test]
  public void Close(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(true, false)] bool leaveStreamOpen
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};

    using var baseStream = new MemoryStream(data);
    using var stream = CreateStream(type, baseStream, 8, leaveStreamOpen);

    stream.Dispose();

    Assert.IsFalse(stream.CanRead, nameof(stream.CanRead));
    Assert.IsFalse(stream.CanWrite, nameof(stream.CanWrite));
    Assert.IsFalse(stream.CanSeek, nameof(stream.CanSeek));
    Assert.IsFalse(stream.CanTimeout, nameof(stream.CanTimeout));

    var buffer = new byte[8];

    Assert.Throws<ObjectDisposedException>(() => Assert.AreEqual(0, stream.Position));
    Assert.Throws<ObjectDisposedException>(() => Assert.AreEqual(8, stream.Length));
    Assert.Throws<ObjectDisposedException>(() => Assert.AreEqual(8, stream.BufferSize));
    Assert.Throws<ObjectDisposedException>(() => Assert.IsNotNull(stream.InnerStream));
    Assert.Throws<ObjectDisposedException>(() => Assert.IsFalse(stream.NewLine.IsEmpty));
    Assert.Throws<ObjectDisposedException>(() => stream.ReadLine());
    Assert.Throws<ObjectDisposedException>(() => stream.ReadLineAsync());
    Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
    Assert.Throws<ObjectDisposedException>(() => stream.Read(buffer, 0, 8));
    Assert.Throws<ObjectDisposedException>(() => stream.ReadAsync(buffer, 0, 8));
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    Assert.ThrowsAsync<ObjectDisposedException>(async () => await stream.ReadAsync(Memory<byte>.Empty));
#endif
    Assert.Throws<ObjectDisposedException>(() => stream.Read(Stream.Null, 8));
    Assert.Throws<ObjectDisposedException>(() => stream.ReadAsync(Stream.Null, 8));
    Assert.Throws<ObjectDisposedException>(() => stream.Flush());
    Assert.Throws<ObjectDisposedException>(() => stream.FlushAsync());
    Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0x00));
    Assert.Throws<ObjectDisposedException>(() => stream.Write(buffer, 0, 8));
    Assert.Throws<ObjectDisposedException>(() => stream.WriteAsync(buffer, 0, 8));
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
    Assert.ThrowsAsync<ObjectDisposedException>(async () => await stream.WriteAsync(Memory<byte>.Empty));
#endif
    Assert.Throws<ObjectDisposedException>(() => stream.CopyToAsync(Stream.Null));

    Assert.DoesNotThrow(() => stream.Dispose(), "Dispose() multiple time");

    if (leaveStreamOpen) {
      Assert.DoesNotThrow(() => baseStream.ReadByte(), "Read, base stream");
      Assert.DoesNotThrow(() => baseStream.WriteByte(0x00), "Write, base stream");
    }
    else {
      Assert.Throws<ObjectDisposedException>(() => baseStream.ReadByte(), "Read, base stream");
      Assert.Throws<ObjectDisposedException>(() => baseStream.WriteByte(0x00), "Write, base stream");
    }
  }
}
