// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Smdn.IO.Streams.LineOriented;

partial class LineOrientedStreamTests {
  [Test]
  public void ReadToStream_ArgumentNull_TargetStream(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    Assert.Throws<ArgumentNullException>(() => stream.Read(null!, 16L));
    Assert.Throws<ArgumentNullException>(() => stream.ReadAsync(null!, 16L));
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
  public async Task ReadToStream_LengthZero(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(true, false)] bool runAsync
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    var copyStream = new MemoryStream();

    Assert.AreEqual(
      0L,
      runAsync
        ? await stream.ReadAsync(copyStream, 0L)
        : stream.Read(copyStream, 0L)
    );
    Assert.AreEqual(0L, stream.Position, "Position");

    copyStream.Dispose();

    Assert.AreEqual(new byte[0], copyStream.ToArray());
  }

  [Test]
  public async Task ReadToStreamAsync_LengthZero(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(true, false)] bool runAsync
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    var copyStream = new MemoryStream();

    Assert.AreEqual(
      0L,
      runAsync
        ? await stream.ReadAsync(copyStream, 0L)
        : stream.Read(copyStream, 0L)
    );

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
  public async Task ReadToStream_BufferEmpty(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var copyStream = new MemoryStream();

    Assert.AreEqual(
      12L,
      runAsync
        ? await stream.ReadAsync(copyStream, 12L)
        : stream.Read(copyStream, 12L)
    );

    Assert.AreEqual(12L, stream.Position, "Position");

    copyStream.Dispose();

    Assert.AreEqual(data, copyStream.ToArray());
  }

  [Test]
  public async Task ReadToStream_LessThanBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(16, 32)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(4).ToArray(), line);

    var copyStream = new MemoryStream();

    Assert.AreEqual(
      4L,
      runAsync
        ? await stream.ReadAsync(copyStream, 4L)
        : stream.Read(copyStream, 4L)
    );

    Assert.AreEqual(8L, stream.Position, "Position");

    copyStream.Dispose();

    Assert.AreEqual(data.Skip(4).Take(4).ToArray(), copyStream.ToArray());
  }

  [Test]
  public async Task ReadToStream_LongerThanBuffered(
    [Values(StreamType.Strict, StreamType.Loose)] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values(true, false)] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.AreEqual(4L, stream.Position, "Position");

    Assert.AreEqual(data.Skip(0).Take(4).ToArray(), line);

    var copyStream = new MemoryStream();

    Assert.AreEqual(
      8L,
      runAsync
        ? await stream.ReadAsync(copyStream, 10L)
        : stream.Read(copyStream, 10L)
    );

    Assert.AreEqual(12L, stream.Position, "Position");

    copyStream.Dispose();

    Assert.AreEqual(data.Skip(4).Take(8).ToArray(), copyStream.ToArray());
  }
}
