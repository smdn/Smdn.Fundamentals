// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO.Streams {
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class NonClosingStream : Stream {
    public Stream InnerStream {
      get { CheckDisposed(); return stream; }
    }

    public override bool CanSeek {
      get { return !IsClosed && stream.CanSeek; }
    }

    public override bool CanRead {
      get { return !IsClosed && stream.CanRead; }
    }

    public override bool CanWrite {
      get { return !IsClosed && !readOnly && stream.CanWrite; }
    }

    public override bool CanTimeout {
      get { return !IsClosed && stream.CanTimeout; }
    }

    private bool IsClosed {
      get { return stream == null; }
    }

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

      return stream.Read(buffer, offset, count);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      CheckDisposed();

      if (readOnly)
        throw ExceptionUtils.CreateNotSupportedWritingStream();

      stream.Write(buffer, offset, count);
    }

    private void CheckDisposed()
    {
      if (IsClosed)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private Stream stream;
    private readonly bool readOnly;
  }
}
