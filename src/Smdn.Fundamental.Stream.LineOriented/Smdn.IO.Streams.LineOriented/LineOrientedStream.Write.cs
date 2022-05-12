// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.LineOriented;

#pragma warning disable IDE0040
partial class LineOrientedStream {
  public override void SetLength(long @value)
  {
    ThrowIfDisposed();

    bufRemain = 0; // discard buffered

    stream.SetLength(@value);
  }

  public override void Flush()
  {
    ThrowIfDisposed();

    stream.Flush();
  }

  public override Task FlushAsync(CancellationToken cancellationToken)
  {
    ThrowIfDisposed();

    return stream.FlushAsync(cancellationToken);
  }

#pragma warning restore IDE0040
  public override void Write(byte[] buffer, int offset, int count)
  {
    ThrowIfDisposed();

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#endif

    stream.Write(buffer, offset, count);
  }

  public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
  {
    ThrowIfDisposed();

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#endif

    return stream.WriteAsync(buffer, offset, count, cancellationToken);
  }

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
  public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();

    return stream.WriteAsync(buffer, cancellationToken);
  }
#endif
}
