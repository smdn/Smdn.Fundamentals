// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2017 smdn
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

using Smdn.IO;

namespace Smdn.IO.Streams {
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
      get { return !IsClosed && stream.CanSeek && CanSeekPrependedData && CanSeekAppendedData; }
    }

    public override bool CanRead {
      get { return !IsClosed && stream.CanRead; }
    }

    public override bool CanWrite {
      get { return /*!IsClosed &&*/ false; }
    }

    public override bool CanTimeout {
      get { return !IsClosed && stream.CanTimeout; }
    }

    private bool IsClosed {
      get { return stream == null; }
    }

    public override long Position {
      get { CheckDisposed(); return position; }
      set
      {
        CheckDisposed();
        CheckSeekable();

        if (value < 0)
          throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(Position), value);
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
        throw new ArgumentNullException(nameof(innerStream));
      if (!innerStream.CanRead)
        throw ExceptionUtils.CreateArgumentMustBeReadableStream(nameof(innerStream));
      if (prependLength < 0L)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(prependLength), prependLength);
      if (appendLength < 0L)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(appendLength), appendLength);

      this.stream = innerStream;
      this.prependLength = prependLength;
      this.appendLength = appendLength;
      this.offsetEndOfInnerStream = prependLength + innerStream.Length;
      this.offsetEndOfStream = offsetEndOfInnerStream + appendLength;
      this.position = 0L;
      this.leaveInnerStreamOpen = leaveInnerStreamOpen;
    }

#if NETFRAMEWORK || NETSTANDARD2_0
    public override void Close()
#else
    protected override void Dispose(bool disposing)
#endif
    {
      if (!leaveInnerStreamOpen)
        stream?.Close();

      stream = null;

#if NETFRAMEWORK || NETSTANDARD2_0
      base.Close();
#else
      base.Dispose(disposing);
#endif
    }

    public override void SetLength(long @value)
    {
      CheckDisposed();

      throw ExceptionUtils.CreateNotSupportedSettingStreamLength();
    }

    public override void Flush()
    {
      CheckDisposed();

      // do nothing
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
          throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(origin), origin);
      }

      throw ExceptionUtils.CreateIOAttemptToSeekBeforeStartOfStream();
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
        throw new ArgumentNullException(nameof(buffer));
      if (offset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
      if (buffer.Length - count < offset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(offset), buffer, offset, count);

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

      throw ExceptionUtils.CreateNotSupportedWritingStream();
    }

    private void CheckDisposed()
    {
      if (IsClosed)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private void CheckSeekable()
    {
      if (!CanSeek)
        throw ExceptionUtils.CreateNotSupportedSeekingStream();
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
