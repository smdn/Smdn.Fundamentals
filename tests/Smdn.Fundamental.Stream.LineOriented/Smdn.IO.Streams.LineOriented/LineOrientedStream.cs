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
    Assert.Throws<ObjectDisposedException>(() => stream.ReadLine(keepEOL: true));
    Assert.Throws<ObjectDisposedException>(() => stream.ReadLineAsync());
    Assert.Throws<ObjectDisposedException>(() => stream.ReadLineAsync(keepEOL: true));
    Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
    Assert.Throws<ObjectDisposedException>(() => stream.Read(buffer, 0, 8));
    Assert.Throws<ObjectDisposedException>(() => stream.ReadAsync(buffer, 0, 8));
#if SYSTEM_IO_STREAM_READ_SPAN_OF_BYTE
    Assert.Throws<ObjectDisposedException>(() => stream.Read(Span<byte>.Empty));
#endif
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
#if SYSTEM_IO_STREAM_WRITE_READONLYSPAN_OF_BYTE
    Assert.Throws<ObjectDisposedException>(() => stream.Write(Span<byte>.Empty));
#endif
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
    Assert.ThrowsAsync<ObjectDisposedException>(async () => await stream.WriteAsync(Memory<byte>.Empty));
#endif
    Assert.Throws<ObjectDisposedException>(() => stream.CopyTo(Stream.Null));
    Assert.Throws<ObjectDisposedException>(() => stream.CopyTo(Stream.Null, bufferSize: 1024));
    Assert.Throws<ObjectDisposedException>(() => stream.CopyToAsync(Stream.Null));
    Assert.Throws<ObjectDisposedException>(() => stream.CopyToAsync(Stream.Null, bufferSize: 1024));

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
