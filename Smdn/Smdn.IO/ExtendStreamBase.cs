// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009-2010 smdn
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
  public abstract class ExtendStreamBase : Stream {
    protected enum Range {
      Prepended,
      InnerStream,
      Appended,
      EndOfStream,
    }

    public Stream InnerStream {
      get { CheckDisposed(); return stream; }
    }

    public override bool CanSeek {
      get { CheckDisposed(); return stream.CanSeek && CanSeekPrependedData && CanSeekAppendedData; }
    }

    public override bool CanRead {
      get { CheckDisposed(); return stream.CanRead; }
    }

    public override bool CanWrite {
      get { CheckDisposed(); return false; }
    }

    public override bool CanTimeout {
      get { CheckDisposed(); return stream.CanTimeout; }
    }

    public override long Position {
      get { CheckDisposed(); return position; }
      set
      {
        CheckDisposed();
        CheckSeekable();

        if (value < 0)
          throw new ArgumentOutOfRangeException("Position", "must be zero or positive number");
        position = value;
        SetPosition();
      }
    }

    public override long Length {
      get { CheckDisposed(); return prependLength + stream.Length + appendLength; }
    }

    public bool LeaveInnerStreamOpen {
      get { CheckDisposed(); return leaveInnerStreamOpen; }
    }

    protected abstract bool CanSeekPrependedData { get; }
    protected abstract bool CanSeekAppendedData { get; }

    protected Range DataRange {
      get
      {
        if (offsetEndOfStream <= position)
          return Range.EndOfStream;
        else if (offsetEndOfInnerStream <= position)
          return Range.Appended;
        else if (prependLength <= position)
          return Range.InnerStream;
        else
          return Range.Prepended;
      }
    }

    protected ExtendStreamBase(Stream innerStream, long prependLength, long appendLength, bool leaveInnerStreamOpen)
    {
      if (innerStream == null)
        throw new ArgumentNullException("innerStream");
      if (!innerStream.CanRead)
        throw new ArgumentException("innerStream", "stream must be readable");
      if (appendLength < 0L)
        throw new ArgumentOutOfRangeException("prependLength", "must be zero or positive number");
      if (appendLength < 0L)
        throw new ArgumentOutOfRangeException("prependLength", "must be zero or positive number");

      this.stream = innerStream;
      this.prependLength = prependLength;
      this.appendLength = appendLength;
      this.offsetEndOfInnerStream = prependLength + innerStream.Length;
      this.offsetEndOfStream = offsetEndOfInnerStream + appendLength;
      this.position = 0L;
      this.leaveInnerStreamOpen = leaveInnerStreamOpen;
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

    public override void Flush()
    {
      CheckDisposed();

      throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      CheckDisposed();
      CheckSeekable();

      // Stream.Seek spec: Seeking to any location beyond the length of the stream is supported.
      switch (origin) {
        case SeekOrigin.Begin:
          if (offset < 0L)
            break;
          position = offset;
          SetPosition();
          return position;

        case SeekOrigin.Current:
          if (position + offset < 0L)
            break;
          position += offset;
          SetPosition();
          return position;

        case SeekOrigin.End:
          if (Length + offset < 0L)
            break;
          position = Length + offset;
          SetPosition();
          return position;

        default:
          throw new ArgumentException(string.Format("unsupported seek origin {0}", origin), "origin");
      }

      throw new IOException("Attempted to seek before start of stream.");
    }

    protected abstract void SetPrependedDataPosition(long position);
    protected abstract void SetAppendedDataPosition(long position);

    private void SetPosition()
    {
      switch (DataRange) {
        case Range.Prepended:
          stream.Seek(0L, SeekOrigin.Begin);
          SetPrependedDataPosition(position);
          break;

        case Range.InnerStream:
          stream.Seek(position - prependLength, SeekOrigin.Begin);
          break;

        case Range.Appended:
          stream.Seek(0L, SeekOrigin.End);
          SetAppendedDataPosition(position - offsetEndOfInnerStream);
          break;

        default:
          stream.Seek(0L, SeekOrigin.End);
          break;
      }
    }

    protected abstract void ReadPrependedData(byte[] buffer, int offset, int count);
    protected abstract void ReadAppendedData(byte[] buffer, int offset, int count);

    public override int Read(byte[] buffer, int offset, int count)
    {
      CheckDisposed();

      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (offset < 0)
        throw new ArgumentOutOfRangeException("count", "must be zero or positive number");
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", "must be zero or positive number");
      if (buffer.Length < offset + count)
        throw new ArgumentException("invalid range");

      var ret = 0;

      while (0 < count) {
        switch (DataRange) {
          case Range.EndOfStream:
            return ret;

          case Range.Prepended: {
            if (prependLength <= position + count) {
              var readCount = (int)(prependLength - position);

              ReadPrependedData(buffer, offset, readCount);

              ret       += readCount;
              count     -= readCount;
              offset    += readCount;
              position  += readCount;

              stream.Position = 0L;
            }
            else {
              ReadPrependedData(buffer, offset, count);

              ret       += count;
              offset    += count;
              position  += count;
              count      = 0;
            }

            break;
          }

          case Range.InnerStream: {
            var read = stream.Read(buffer, offset, count);

            if (read <= 0)
              return ret;

            ret       += read;
            count     -= read;
            offset    += read;
            position  += read;

            if (offsetEndOfInnerStream < position)
              position = offsetEndOfInnerStream;

            break;
          }

          case Range.Appended: {
            var readCount = (int)Math.Min(count, offsetEndOfStream - position);

            ReadAppendedData(buffer, offset, readCount);

            ret       += readCount;
            count     -= readCount;
            offset    += readCount;
            position  += readCount;

            break;
          }
        } // switch
      } // while

      return ret;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      CheckDisposed();

      throw new NotSupportedException();
    }

    private void CheckDisposed()
    {
      if (stream == null)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private void CheckSeekable()
    {
      if (!CanSeek)
        throw new NotSupportedException("cannot seek stream");
    }

    private Stream stream;
    private long position;
    private readonly long prependLength;
    private readonly long appendLength;
    private readonly long offsetEndOfInnerStream;
    private readonly long offsetEndOfStream;
    private readonly bool leaveInnerStreamOpen;
  }
}
