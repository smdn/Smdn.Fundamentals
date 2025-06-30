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

#pragma warning disable IDE0040
partial class LineOrientedStreamTests {
#pragma warning restore IDE0040
  [Test]
  public void ReadToStream_ArgumentNull_TargetStream(
    [Values] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    Assert.Throws<ArgumentNullException>(() => stream.Read(null!, 16L));
    Assert.Throws<ArgumentNullException>(() => stream.ReadAsync(null!, 16L));
  }

  [Test]
  public void ReadToStream_ArgumentOutOfRange_Length(
    [Values] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);

    Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(Stream.Null, -1L));
    Assert.Throws<ArgumentOutOfRangeException>(() => stream.ReadAsync(Stream.Null, -1L));
  }

  [Test]
  public async Task ReadToStream_LengthZero(
    [Values] StreamType type,
    [Values] bool runAsync
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    var copyStream = new MemoryStream();

    Assert.That(
      runAsync
        ? await stream.ReadAsync(copyStream, 0L)
        : stream.Read(copyStream, 0L),
     Is.Zero
    );
    Assert.That(stream.Position,Is.Zero, "Position");

    copyStream.Dispose();

    Assert.That(copyStream.ToArray(), Is.EqualTo(new byte[0]));
  }

  [Test]
  public async Task ReadToStreamAsync_LengthZero(
    [Values] StreamType type,
    [Values] bool runAsync
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    var copyStream = new MemoryStream();

    Assert.That(
      runAsync
        ? await stream.ReadAsync(copyStream, 0L)
        : stream.Read(copyStream, 0L),
     Is.Zero
    );

    Assert.That(stream.Position,Is.Zero, "Position");

    copyStream.Dispose();

    Assert.That(copyStream.ToArray(), Is.EqualTo(new byte[0]));
  }

  [Test]
  public void ReadToStreamAsync_CancelledToken(
    [Values] StreamType type
  )
  {
    using var stream = CreateStream(type, new MemoryStream(), 8);
    using var cts = new CancellationTokenSource();

    cts.Cancel();

    var t = stream.ReadAsync(Stream.Null, 0L, cts.Token);

    Assert.That(t.IsCanceled, Is.True);
    Assert.That(async () => await t, Throws.InstanceOf<OperationCanceledException>());

    Assert.That(stream.Position,Is.Zero, "Position");
  }

  [Test]
  public async Task ReadToStream_BufferEmpty(
    [Values] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var copyStream = new MemoryStream();

    Assert.That(
      runAsync
        ? await stream.ReadAsync(copyStream, 12L)
        : stream.Read(copyStream, 12L),
      Is.EqualTo(12L)
    );

    Assert.That(stream.Position, Is.EqualTo(12L), "Position");

    copyStream.Dispose();

    Assert.That(copyStream.ToArray(), Is.EqualTo(data));
  }

  [Test]
  public async Task ReadToStream_LessThanBuffered(
    [Values] StreamType type,
    [Values(16, 32)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.That(stream.Position, Is.EqualTo(4L), "Position");

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(4).ToArray()));

    var copyStream = new MemoryStream();

    Assert.That(
      runAsync
        ? await stream.ReadAsync(copyStream, 4L)
        : stream.Read(copyStream, 4L),
      Is.EqualTo(4L)
    );

    Assert.That(stream.Position, Is.EqualTo(8L), "Position");

    copyStream.Dispose();

    Assert.That(copyStream.ToArray(), Is.EqualTo(data.Skip(4).Take(4).ToArray()));
  }

  [Test]
  public async Task ReadToStream_LongerThanBuffered(
    [Values] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize,
    [Values] bool runAsync
  )
  {
    var data = new byte[] {0x40, 0x41, CR, LF, 0x42, 0x43, 0x44, CR, LF, 0x45, 0x46, 0x47};
    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    var line = stream.ReadLine(true);

    Assert.That(stream.Position, Is.EqualTo(4L), "Position");

    Assert.That(line, Is.EqualTo(data.Skip(0).Take(4).ToArray()));

    var copyStream = new MemoryStream();

    Assert.That(
      runAsync
        ? await stream.ReadAsync(copyStream, 10L)
        : stream.Read(copyStream, 10L),
      Is.EqualTo(8L)
    );

    Assert.That(stream.Position, Is.EqualTo(12L), "Position");

    copyStream.Dispose();

    Assert.That(copyStream.ToArray(), Is.EqualTo(data.Skip(4).Take(8).ToArray()));
  }
}
