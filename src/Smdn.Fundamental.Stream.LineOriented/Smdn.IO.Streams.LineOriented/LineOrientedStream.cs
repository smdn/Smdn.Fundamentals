// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.LineOriented;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public partial class LineOrientedStream : Stream {
  protected const int DefaultBufferSize = 1024;
  /*private*/ protected const int MinimumBufferSize = 1;
  /*private*/ protected const bool DefaultLeaveStreamOpen = false;

  public override bool CanSeek => !IsClosed && stream.CanSeek;
  public override bool CanRead => !IsClosed && stream.CanRead;
  public override bool CanWrite => !IsClosed && stream.CanWrite;
  public override bool CanTimeout => !IsClosed && stream.CanTimeout;
  private bool IsClosed => stream is null;

  public override long Position {
    get { ThrowIfDisposed(); return stream.Position - bufRemain; }
    set => Seek(value, SeekOrigin.Begin);
  }

  public override long Length {
    get { ThrowIfDisposed(); return stream.Length; }
  }

  public ReadOnlySpan<byte> NewLine {
    get { ThrowIfDisposed(); return newLine; }
  }

  public bool IsStrictNewLine {
    get { ThrowIfDisposed(); return newLine != null; }
  }

  public int BufferSize {
    get { ThrowIfDisposed(); return buffer.Length; }
  }

  public virtual Stream InnerStream {
    get { ThrowIfDisposed(); return stream; }
  }

  public LineOrientedStream(
    Stream stream,
    ReadOnlySpan<byte> newLine,
    int bufferSize = DefaultBufferSize,
    bool leaveStreamOpen = DefaultLeaveStreamOpen
  )
  {
    if (bufferSize < MinimumBufferSize)
      throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(MinimumBufferSize, nameof(bufferSize), bufferSize);

    this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
    this.newLine = newLine.IsEmpty ? null : newLine.ToArray(); // XXX: allocation
    this.buffer = new byte[bufferSize];
    this.leaveStreamOpen = leaveStreamOpen;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing) {
      if (!leaveStreamOpen)
#if SYSTEM_IO_STREAM_CLOSE
        stream?.Close();
#else
        stream?.Dispose();
#endif
    }

    stream = null;
    buffer = null;

    base.Dispose(disposing);
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    ThrowIfDisposed();

    bufRemain = 0; // discard buffered

    return stream.Seek(offset, origin);
  }

  private int FillBuffer()
  {
    bufOffset = 0;

    bufRemain =
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
      stream.Read(
        buffer.AsSpan()
#else
#pragma warning disable CA1835
      stream.Read(
        buffer,
        0,
        buffer.Length
#pragma warning restore CA1835
#endif
      );

    return bufRemain;
  }

  private async Task<int> FillBufferAsync(CancellationToken cancellationToken)
  {
    bufOffset = 0;

    bufRemain =
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
      await stream.ReadAsync(
        buffer.AsMemory(),
#else
#pragma warning disable CA1835
      await stream.ReadAsync(
        buffer,
        0,
        buffer.Length,
#pragma warning restore CA1835
#endif
        cancellationToken
      ).ConfigureAwait(false);

    return bufRemain;
  }

  private void ThrowIfDisposed()
  {
    if (IsClosed)
      throw new ObjectDisposedException(GetType().FullName);
  }

  private Stream stream;
  private readonly byte[] newLine;
  private readonly bool leaveStreamOpen;
  private byte[] buffer;
  private int bufOffset;
  private int bufRemain;
}
