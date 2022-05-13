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

partial class LineOrientedStreamTests {

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
  public async Task Read_BufferEmpty(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var buffer = new byte[12];

    Assert.AreEqual(
      12L,
      runAsync
        ? await stream.ReadAsync(buffer, 0, 12)
        : stream.Read(buffer, 0, 12)
    );

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
  public async Task Read_LengthZero(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(true, false)] bool runAsync
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    var buffer = new byte[1];

    Assert.AreEqual(
      0,
      runAsync
        ? await stream.ReadAsync(buffer, 0, 0)
        : stream.Read(buffer, 0, 0)
    );

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

  [Test]
  public async Task Read_LessThanBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(16, 32)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), 16);

    var line = stream.ReadLine(true);

    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(4).ToArray(), line);

    var buffer = new byte[4];

    Assert.AreEqual(
      4,
      runAsync
        ? await stream.ReadAsync(buffer, 0, 4)
        : stream.Read(buffer, 0, 4)
    );

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
  public async Task Read_LongerThanBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(4).ToArray(), line);

    var buffer = new byte[10];

    Assert.AreEqual(
      8,
      runAsync
        ? await stream.ReadAsync(buffer, 0, 10)
        : stream.Read(buffer, 0, 10)
    );

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
}
