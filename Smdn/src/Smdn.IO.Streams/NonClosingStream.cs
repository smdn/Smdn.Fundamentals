//
// Copyright (c) 2009 smdn <smdn@smdn.jp>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;

namespace Smdn.IO.Streams {
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
      get { return !IsClosed && stream.CanWrite; }
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
    {
      if (innerStream == null)
        throw new ArgumentNullException(nameof(innerStream));

      this.stream = innerStream;
    }

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    public override void Close()
#else
    protected override void Dispose(bool disposing)
#endif
    {
      if (stream != null && stream.CanWrite)
        stream.Flush();

      stream = null;

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
      base.Close();
#else
      base.Dispose(disposing);
#endif
    }

    public override void SetLength(long @value)
    {
      CheckDisposed();

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

      stream.Write(buffer, offset, count);
    }

    private void CheckDisposed()
    {
      if (IsClosed)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private Stream stream;
  }
}
