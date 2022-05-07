// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class NonClosingStream : Stream {
  public Stream InnerStream {
    get { CheckDisposed(); return stream; }
  }

  public override bool CanSeek => !IsClosed && stream.CanSeek;
  public override bool CanRead => !IsClosed && stream.CanRead;
  public override bool CanWrite => !IsClosed && !readOnly && stream.CanWrite;
  public override bool CanTimeout => !IsClosed && stream.CanTimeout;
  private bool IsClosed => stream is null;

  public override long Position {
    get { CheckDisposed(); return stream.Position; }
    set { CheckDisposed(); stream.Position = value; }
  }

  public override long Length {
    get { CheckDisposed(); return stream.Length; }
  }

  public NonClosingStream(Stream innerStream)
    : this(innerStream, true)
  {
  }

  public NonClosingStream(Stream innerStream, bool writable)
  {
    this.stream = innerStream ?? throw new ArgumentNullException(nameof(innerStream));
    this.readOnly = !writable;
  }

#if SYSTEM_IO_STREAM_CLOSE
  public override void Close()
#else
  protected override void Dispose(bool disposing)
#endif
  {
    if (stream != null && stream.CanWrite)
      stream.Flush();

    stream = null;

#if SYSTEM_IO_STREAM_CLOSE
    base.Close();
#else
    base.Dispose(disposing);
#endif
  }

  public override void SetLength(long @value)
  {
    CheckDisposed();

    if (readOnly)
      throw ExceptionUtils.CreateNotSupportedSettingStreamLength();

    stream.SetLength(@value);
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    CheckDisposed();

    return stream.Seek(offset, origin);
  }

  public override void Flush()
  {
    CheckDisposed();

    stream.Flush();
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    CheckDisposed();

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#endif

    return stream.Read(buffer, offset, count);
  }

  public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
  {
    CheckDisposed();

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#endif

    return stream.ReadAsync(buffer, offset, count, cancellationToken);
  }

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
  {
    CheckDisposed();

    return stream.ReadAsync(buffer, cancellationToken);
  }
#endif
  public override void Write(byte[] buffer, int offset, int count)
  {
    CheckDisposed();

    if (readOnly)
      throw ExceptionUtils.CreateNotSupportedWritingStream();

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#endif

    stream.Write(buffer, offset, count);
  }

  public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
  {
    CheckDisposed();

    if (readOnly)
      throw ExceptionUtils.CreateNotSupportedWritingStream();

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#endif

    return stream.WriteAsync(buffer, offset, count, cancellationToken);
  }

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
  public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
  {
    CheckDisposed();

    if (readOnly)
      throw ExceptionUtils.CreateNotSupportedWritingStream();

    return stream.WriteAsync(buffer, cancellationToken);
  }
#endif

  private void CheckDisposed()
  {
    if (IsClosed)
      throw new ObjectDisposedException(GetType().FullName);
  }

  private Stream stream;
  private readonly bool readOnly;
}
