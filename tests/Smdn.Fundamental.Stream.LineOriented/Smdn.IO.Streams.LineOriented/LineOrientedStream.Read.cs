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

#pragma warning disable IDE0040
partial class LineOrientedStreamTests {
#pragma warning restore IDE0040

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

      Assert.Throws(expectedExceptionType, () => _ = stream.Read(buffer, offset, count), $"type = {type}");
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
    [Values] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var buffer = new byte[12];

    Assert.That(
      runAsync
        ? await stream.ReadAsync(buffer, 0, 12)
        : stream.Read(buffer, 0, 12),
      Is.EqualTo(12L)
    );

    Assert.That(stream.Position, Is.EqualTo(12L), "Position");

    Assert.That(buffer, Is.EqualTo(data));
  }

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  [Test]
  public async Task ReadAsync_ToMemory_BufferEmpty(
    [Values] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    Memory<byte> buffer = new byte[12];

    Assert.That(await stream.ReadAsync(buffer), Is.EqualTo(12L));

    Assert.That(stream.Position, Is.EqualTo(12L), "Position");

    Assert.That(buffer, Is.EqualTo(data.AsMemory()), nameof(buffer));
  }
#endif

  [Test]
  public async Task Read_LengthZero(
    [Values] StreamType type,
    [Values] bool runAsync
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    var buffer = new byte[1];

    Assert.That(
      runAsync
        ? await stream.ReadAsync(buffer, 0, 0)
        : stream.Read(buffer, 0, 0),
      Is.Zero
    );

    Assert.That(stream.Position,Is.Zero, "Position");
  }

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  [Test]
  public async Task ReadAsync_ToMemory_LengthZero(
    [Values] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    Assert.That(await stream.ReadAsync(Memory<byte>.Empty),Is.Zero);
    Assert.That(stream.Position,Is.Zero, "Position");
  }
#endif

  [Test]
  public void ReadAsync_CancelledToken(
    [Values] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    using var cts = new CancellationTokenSource();

    cts.Cancel();

    var t = stream.ReadAsync(new byte[1], 0, 1, cts.Token);

    Assert.That(t.IsCanceled, Is.True);
    Assert.That(async () => await t, Throws.InstanceOf<OperationCanceledException>());

    Assert.That(stream.Position,Is.Zero, "Position");
  }

  [Test]
  public async Task Read_LessThanBuffered(
    [Values] StreamType type,
    [Values(16, 32)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), 16);

    var line = stream.ReadLine(true);

    Assert.That(stream.Position, Is.EqualTo(4L), "Position");

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(4).ToArray()));

    var buffer = new byte[4];

    Assert.That(
      runAsync
        ? await stream.ReadAsync(buffer, 0, 4)
        : stream.Read(buffer, 0, 4),
      Is.EqualTo(4)
    );

    Assert.That(stream.Position, Is.EqualTo(8L), "Position");

    Assert.That(buffer, Is.EqualTo(data.Skip(4).Take(4).ToArray()));
  }

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  [Test]
  public async Task ReadAsync_ToMemory_LessThanBuffered(
    [Values] StreamType type,
    [Values(16, 32)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.That(stream.Position, Is.EqualTo(4L), "Position");

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(4).ToArray()));

    Memory<byte> buffer = new byte[4];

    Assert.That(await stream.ReadAsync(buffer), Is.EqualTo(4));

    Assert.That(stream.Position, Is.EqualTo(8L), "Position");

    Assert.That(buffer, Is.EqualTo(data.AsMemory(4, 4)), nameof(buffer));
  }
#endif

  [Test]
  public async Task Read_LongerThanBuffered(
    [Values] StreamType type,
    [Values(1, 2, 3, 8)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.That(stream.Position, Is.EqualTo(4L), "Position");

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(4).ToArray()));

    var buffer = new byte[10];

    Assert.That(
      runAsync
        ? await stream.ReadAsync(buffer, 0, 10)
        : stream.Read(buffer, 0, 10),
      Is.EqualTo(8)
    );

    Assert.That(stream.Position, Is.EqualTo(12L), "Position");

    Assert.That(buffer.Skip(0).Take(8).ToArray(), Is.EqualTo(data.Skip(4).Take(8).ToArray()));
  }

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  [Test]
  public async Task ReadAsync_ToMemory_LongerThanBuffered(
    [Values] StreamType type,
    [Values(1, 2, 3, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.That(stream.Position, Is.EqualTo(4L), "Position");

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(4).ToArray()));

    Memory<byte> buffer = new byte[10];

    Assert.That(await stream.ReadAsync(buffer), Is.EqualTo(8));

    Assert.That(stream.Position, Is.EqualTo(12L), "Position");

    Assert.That(buffer.Slice(0, 8), Is.EqualTo(data.AsMemory(4, 8)), nameof(buffer));
  }
#endif

  [Test]
  public void ReadByte(
    [Values] StreamType type,
    [Values(1, 3, 4, 5, 6)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var index = 0;

    for (; ; ) {
      var val = stream.ReadByte();

      if (index == data.Length) {
        Assert.That(val, Is.EqualTo(-1));
      }
      else {
        Assert.That(val, Is.EqualTo(data[index++]), "data");
        Assert.That(stream.Position, Is.EqualTo(index), "Position");
      }

      if (val == -1)
        break;
    }
  }
}
