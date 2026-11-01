// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

using NUnit.Framework;

namespace Smdn.IO.Streams.LineOriented;

[TestFixture]
public partial class LineOrientedStreamTests {
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
    [Values] StreamType type,
    [Values(1, 2, 3, 4, 8)] int bufferSize
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};

    using var stream = CreateStream(type, new MemoryStream(data), bufferSize);

    Assert.That(stream.CanRead, Is.True, nameof(stream.CanRead));
    Assert.That(stream.CanWrite, Is.True, nameof(stream.CanWrite));
    Assert.That(stream.CanSeek, Is.True, nameof(stream.CanSeek));
    Assert.That(stream.CanTimeout, Is.False, nameof(stream.CanTimeout));
    Assert.That(stream.Length, Is.EqualTo(8L), nameof(stream.Length));
    Assert.That(stream.Position, Is.Zero, nameof(stream.Position));
    Assert.That(stream.BufferSize, Is.EqualTo(bufferSize), nameof(stream.BufferSize));
  }

  [Test]
  public void Close(
    [Values] StreamType type,
    [Values] bool leaveStreamOpen
  )
  {
    var data = new byte[] {0x40, 0x41, 0x42, 0x43, CR, LF, 0x44, 0x45};

    using var baseStream = new MemoryStream(data);
    using var stream = CreateStream(type, baseStream, 8, leaveStreamOpen);

    stream.Dispose();

    Assert.That(stream.CanRead, Is.False, nameof(stream.CanRead));
    Assert.That(stream.CanWrite, Is.False, nameof(stream.CanWrite));
    Assert.That(stream.CanSeek, Is.False, nameof(stream.CanSeek));
    Assert.That(stream.CanTimeout, Is.False, nameof(stream.CanTimeout));

    var buffer = new byte[8];

    Assert.That(() => Assert.That(stream.Position, Is.Zero), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => Assert.That(stream.Length, Is.EqualTo(8)), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => Assert.That(stream.BufferSize, Is.EqualTo(8)), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => Assert.That(stream.InnerStream, Is.Not.Null), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => Assert.That(stream.NewLine.IsEmpty, Is.False), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(stream.ReadLine, Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => stream.ReadLine(keepEOL: true), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => stream.ReadLineAsync(), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => stream.ReadLineAsync(keepEOL: true), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(stream.ReadByte, Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => _ = stream.Read(buffer, 0, 8), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => stream.ReadAsync(buffer, 0, 8), Throws.TypeOf<ObjectDisposedException>());
#if SYSTEM_IO_STREAM_READ_SPAN_OF_BYTE
    Assert.That(() => _ = stream.Read(Span<byte>.Empty), Throws.TypeOf<ObjectDisposedException>());
#endif
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
    Assert.That(
      async () => _ = await stream.ReadAsync(Memory<byte>.Empty),
      Throws.TypeOf<ObjectDisposedException>()
    );
#endif
    Assert.That(() => stream.Read(Stream.Null, 8), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => stream.ReadAsync(Stream.Null, 8), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(stream.Flush, Throws.TypeOf<ObjectDisposedException>());
    Assert.That(stream.FlushAsync, Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => stream.WriteByte(0x00), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => stream.Write(buffer, 0, 8), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => stream.WriteAsync(buffer, 0, 8), Throws.TypeOf<ObjectDisposedException>());
#if SYSTEM_IO_STREAM_WRITE_READONLYSPAN_OF_BYTE
    Assert.That(() => stream.Write(Span<byte>.Empty), Throws.TypeOf<ObjectDisposedException>());
#endif
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
    Assert.That(
      async () => await stream.WriteAsync(Memory<byte>.Empty),
      Throws.TypeOf<ObjectDisposedException>()
    );
#endif
    Assert.That(() => stream.CopyTo(Stream.Null), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => stream.CopyTo(Stream.Null, bufferSize: 1024), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => stream.CopyToAsync(Stream.Null), Throws.TypeOf<ObjectDisposedException>());
    Assert.That(() => stream.CopyToAsync(Stream.Null, bufferSize: 1024), Throws.TypeOf<ObjectDisposedException>());

    Assert.That(stream.Dispose, Throws.Nothing, "Dispose() multiple time");

    if (leaveStreamOpen) {
      Assert.That(baseStream.ReadByte, Throws.Nothing, "Read, base stream");
      Assert.That(() => baseStream.WriteByte(0x00), Throws.Nothing, "Write, base stream");
    }
    else {
      Assert.That(baseStream.ReadByte, Throws.TypeOf<ObjectDisposedException>(), "Read, base stream");
      Assert.That(() => baseStream.WriteByte(0x00), Throws.TypeOf<ObjectDisposedException>(), "Write, base stream");
    }
  }
}
