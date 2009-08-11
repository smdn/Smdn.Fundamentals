// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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

namespace Smdn.IO {
  public class InterruptStream : Stream {
    public Stream InnerStream {
      get { return stream; }
    }

    public override bool CanSeek {
      get { return stream.CanSeek; }
    }

    public override bool CanRead {
      get { return stream.CanWrite; }
    }

    public override bool CanWrite {
      get { return stream.CanRead; }
    }

    public override bool CanTimeout {
      get { return stream.CanTimeout; }
    }

    public override long Position {
      get { return stream.Position; }
      set { stream.Position = value; }
    }

    public override long Length {
      get { return stream.Length; }
    }

    public InterruptStream(Stream innerStream)
    {
      if (innerStream == null)
        throw new ArgumentNullException("innerStream");

      this.stream = innerStream;
    }

    public override void Close()
    {
      stream.Close();

      disposed = true;
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

    protected virtual void OnReading(byte[] dest, int offset, int count, out bool abortRead)
    {
      // do nothing
      abortRead = false;
    }

    protected virtual int OnRead(byte[] dest, int offset, int count)
    {
      // do nothing
      return count;
    }

    public override int Read(byte[] dest, int offset, int count)
    {
      CheckDisposed();

      bool abortRead;

      OnReading(dest, offset, count, out abortRead);

      if (abortRead)
        return 0;

      return OnRead(dest, offset, stream.Read(dest, offset, count));
    }

    protected virtual void OnWriting(byte[] src, int offset, int count, out bool abortWrite)
    {
      // do nothing
      abortWrite = false;
    }

    protected virtual void OnWritten(byte[] src, int offset, int count)
    {
      // do nothing
    }

    public override void Write(byte[] src, int offset, int count)
    {
      CheckDisposed();

      bool abortWrite;

      OnWriting(src, offset, count, out abortWrite);

      if (abortWrite)
        return;

      stream.Write(src, offset, count);

      OnWritten(src, offset, count);
    }

    private void CheckDisposed()
    {
      if (disposed)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private bool disposed = false;
    private Stream stream;
  }
}
