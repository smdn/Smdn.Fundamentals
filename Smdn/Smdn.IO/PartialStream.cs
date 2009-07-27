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
  public class PartialStream : Stream {
    public Stream InnerStream {
      get { CheckDisposed(); return stream; }
    }

    public override bool CanSeek {
      get { CheckDisposed(); return stream.CanSeek; }
    }

    public override bool CanRead {
      get { CheckDisposed(); return stream.CanRead; }
    }

    public override bool CanWrite {
      get { CheckDisposed(); return writable && stream.CanWrite; }
    }

    public override bool CanTimeout {
      get { CheckDisposed(); return stream.CanTimeout; }
    }

    public override long Position {
      get { CheckDisposed(); return stream.Position - offset; }
      set
      {
        CheckDisposed();

        if (value < 0)
          throw new ArgumentOutOfRangeException("Position", "must be zero or positive number");
        stream.Position = value + offset;
      }
    }

    public override long Length {
      get
      {
        CheckDisposed();
        if (length == null)
          return stream.Length - offset;
        else
          return length.Value;
      }
    }

    public bool LeaveInnerStreamOpen {
      get { CheckDisposed(); return leaveInnerStreamOpen; }
    }

    public PartialStream(Stream innerStream, long offset)
      : this(innerStream, offset, null, false, true)
    {
    }

    public PartialStream(Stream innerStream, long offset, bool leaveInnerStreamOpen)
      : this(innerStream, offset, null, false, leaveInnerStreamOpen)
    {
    }

    public PartialStream(Stream innerStream, long offset, long length)
      : this(innerStream, offset, length, false, true)
    {
    }

    public PartialStream(Stream innerStream, long offset, long length, bool leaveInnerStreamOpen)
      : this(innerStream, offset, (long?)length, false, leaveInnerStreamOpen)
    {
    }

    public PartialStream(Stream innerStream, long offset, long length, bool @readonly, bool leaveInnerStreamOpen)
      : this(innerStream, offset, (long?)length, @readonly, leaveInnerStreamOpen)
    {
    }

    private PartialStream(Stream innerStream, long offset, long? length, bool @readonly, bool leaveInnerStreamOpen)
    {
      if (innerStream == null)
        throw new ArgumentNullException("innerStream");
      if (!innerStream.CanSeek)
        throw new ArgumentException("innerStream", "stream must be seekable");

      this.stream = innerStream;
      this.offset = offset;
      this.length = length;
      this.writable = !@readonly;
      this.leaveInnerStreamOpen = leaveInnerStreamOpen;

      this.Position = 0;
    }

    public override void Close()
    {
      if (!leaveInnerStreamOpen)
        stream.Close();

      stream = null;
    }

    public override void SetLength(long @value)
    {
      CheckDisposed();

      throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      CheckDisposed();

      // Stream.Seek spec: Seeking to any location beyond the length of the stream is supported.
      switch (origin) {
        case SeekOrigin.Begin:
          if (offset < 0)
            break;
          return stream.Seek(this.offset + offset, SeekOrigin.Begin) - this.offset;
        case SeekOrigin.Current:
          if (Position + offset < 0)
            break;
          return stream.Seek(offset, SeekOrigin.Current) - this.offset;
        case SeekOrigin.End: {
          var position = Length + offset;

          if (position < 0)
            break;
          else
            return stream.Seek(this.offset + position, SeekOrigin.Begin) - this.offset;
        }
        default:
          throw new ArgumentException(string.Format("unsupported seek origin {0}", origin), "origin");
      }

      throw new IOException("Attempted to seek before start of MemoryStream.");
    }

    public override void Flush()
    {
      CheckDisposed();
      CheckWritable();

      stream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      CheckDisposed();

      if (count < 0)
        throw new ArgumentOutOfRangeException("count", "must be zero or positive number");

      if (length == null)
        return stream.Read(buffer, offset, count);

      var remainder = (int)(length.Value - Position); // XXX: long -> int

      if (remainder <= 0)
        return 0;
      else if (remainder < count)
        return stream.Read(buffer, offset, remainder);
      else
        return stream.Read(buffer, offset, count);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      CheckDisposed();
      CheckWritable();

      if (count < 0)
        throw new ArgumentOutOfRangeException("count", "must be zero or positive number");

      if (length == null) {
        stream.Write(buffer, offset, count);
      }
      else {
        var remainder = (int)(length.Value - Position); // XXX: long -> int

        if (remainder <= 0 || remainder < count)
          throw new IOException("end of stream");
        else
          stream.Write(buffer, offset, count);
      }
    }

    private void CheckDisposed()
    {
      if (stream == null)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private void CheckWritable()
    {
      if (!writable)
        throw new InvalidOperationException("stream is read only");
    }

    private Stream stream;
    private readonly long offset;
    private readonly long? length;
    private readonly bool writable;
    private readonly bool leaveInnerStreamOpen;
  }
}
