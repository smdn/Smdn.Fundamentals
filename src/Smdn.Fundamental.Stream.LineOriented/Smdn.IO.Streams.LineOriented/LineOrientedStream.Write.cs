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
    CheckDisposed();

    bufRemain = 0; // discard buffered

    stream.SetLength(@value);
  }

  public override void Flush()
  {
    CheckDisposed();

    stream.Flush();
  }

  public override Task FlushAsync(CancellationToken cancellationToken)
  {
    CheckDisposed();

    return stream.FlushAsync(cancellationToken);
  }

#pragma warning restore IDE0040
  public override void Write(byte[] buffer, int offset, int count)
  {
    CheckDisposed();

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#endif

    stream.Write(buffer, offset, count);
  }

  public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
  {
    CheckDisposed();

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#endif

    return stream.WriteAsync(buffer, offset, count, cancellationToken);
  }

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
  public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
  {
    CheckDisposed();

    return stream.WriteAsync(buffer, cancellationToken);
  }
#endif
}
