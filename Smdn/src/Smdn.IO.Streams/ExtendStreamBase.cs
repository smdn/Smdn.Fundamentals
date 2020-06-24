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
using System.Threading;
using System.Threading.Tasks;

using Smdn.IO;

namespace Smdn.IO.Streams {
  public abstract class ExtendStreamBase : Stream {
    private Stream stream = null;
    private bool IsClosed => stream == null;

    public Stream InnerStream { get { ThrowIfDisposed(); return stream; } }

    private readonly bool leaveInnerStreamOpen;
    public bool LeaveInnerStreamOpen { get { ThrowIfDisposed(); return leaveInnerStreamOpen; } }

    public override bool CanSeek => !IsClosed && stream.CanSeek && CanSeekPrependedData && CanSeekPrependedData;
    public override bool CanRead => !IsClosed && stream.CanRead;
    public override bool CanWrite => /*!IsClosed &&*/ false;
    public override bool CanTimeout => !IsClosed && stream.CanTimeout;
    public override long Length { get { ThrowIfDisposed(); return prependLength + stream.Length + appendLength; } }

    private long position;
    public override long Position {
      get { ThrowIfDisposed(); return position; }
      set {
        ThrowIfDisposed();
        ThrowIfNotSeekable();

        if (value < 0)
          throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(Position), value);

        SetPosition(value);
      }
    }

    private readonly long prependLength;
    private readonly long appendLength;

    protected abstract bool CanSeekPrependedData { get; }
    protected abstract bool CanSeekAppendedData { get; }

    protected enum StreamSection {
      Prepend,
      Stream,
      Append,
      EndOfStream,
    }

    protected StreamSection Section { get; private set; }

    protected void ThrowIfDisposed()
    {
      if (stream == null)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private void ThrowIfNotSeekable()
    {
      if (!CanSeek)
        throw ExceptionUtils.CreateNotSupportedSeekingStream();
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
      this.position = 0L;
      this.leaveInnerStreamOpen = leaveInnerStreamOpen;
      this.Section = prependLength == 0L ? StreamSection.Stream : StreamSection.Prepend;
    }

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    public override void Close()
#else
    protected override void Dispose(bool disposing)
#endif
    {
      if (!leaveInnerStreamOpen)
        stream?.Close();

      stream = null;

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
      base.Close();
#else
      base.Dispose(disposing);
#endif
    }

    private Task ThrowNotSupportedWritingStream() { ThrowIfDisposed(); throw ExceptionUtils.CreateNotSupportedWritingStream(); }

    public override void Flush() => ThrowNotSupportedWritingStream();
    public override Task FlushAsync(CancellationToken cancellationToken) => ThrowNotSupportedWritingStream();

    public override void Write(byte[] buffer, int offset, int count) => ThrowNotSupportedWritingStream();
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => ThrowNotSupportedWritingStream();

    public override void SetLength(long value) { ThrowIfDisposed(); throw ExceptionUtils.CreateNotSupportedSettingStreamLength(); }

    public override long Seek(long offset, SeekOrigin origin)
    {
      ThrowIfDisposed();
      ThrowIfNotSeekable();

      // Stream.Seek spec: Seeking to any location beyond the length of the stream is supported.
      long newOffset;

      switch (origin) {
        case SeekOrigin.Begin:    newOffset = offset; break;
        case SeekOrigin.Current:  newOffset = offset + position; break;
        case SeekOrigin.End:      newOffset = offset + Length; break;
        default:
          throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(origin), origin);
      }

      if (newOffset < 0L)
        throw ExceptionUtils.CreateIOAttemptToSeekBeforeStartOfStream();

      return SetPosition(newOffset);
    }

    protected abstract void SetPrependedDataPosition(long position);
    protected abstract void SetAppendedDataPosition(long position);

    private long SetPosition(long newPosition)
    {
      if (0L < prependLength && newPosition < prependLength) {
        Section = StreamSection.Prepend;
        stream.Seek(0L, SeekOrigin.Begin);
        SetPrependedDataPosition(newPosition);
      }
      else if (newPosition < prependLength + stream.Length) {
        Section = StreamSection.Stream;
        stream.Seek(newPosition - prependLength, SeekOrigin.Begin);
      }
      else if (0L < appendLength && newPosition < prependLength + stream.Length + appendLength) {
        Section = StreamSection.Append;
        stream.Seek(0L, SeekOrigin.End);
        SetAppendedDataPosition(newPosition - (stream.Length + appendLength));
      }
      else {
        Section = StreamSection.EndOfStream;
      }

      return position = newPosition;
    }

    protected abstract int ReadPrependedData(byte[] buffer, int offset, int count);
    protected abstract int ReadAppendedData(byte[] buffer, int offset, int count);

    public override int Read(byte[] buffer, int offset, int count)
    {
      ThrowIfDisposed();

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
        switch (Section) {
          case StreamSection.Prepend: {
            var readCount = ReadPrependedData(buffer, offset, count);

            if (readCount == 0) {
              Section = StreamSection.Stream;
              continue;
            }

            ret       += readCount;
            count     -= readCount;
            offset    += readCount;
            position  += readCount;

            break;
          }

          case StreamSection.Stream: {
            var readCount = stream.Read(buffer, offset, count);

            if (readCount == 0) {
              Section = (appendLength == 0L) ? StreamSection.EndOfStream : StreamSection.Append;
              continue;
            }

            ret       += readCount;
            count     -= readCount;
            offset    += readCount;
            position  += readCount;

            break;
          }

          case StreamSection.Append: {
            var readCount = ReadAppendedData(buffer, offset, count);

            if (readCount == 0) {
              Section = StreamSection.EndOfStream;
              continue;
            }

            ret       += readCount;
            count     -= readCount;
            offset    += readCount;
            position  += readCount;

            break;
          }

          default:
          //case StreamSection.EndOfStream:
            return ret;
        } // switch
      } // while

      return ret;
    }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
      ThrowIfDisposed();

      if (buffer == null)
        throw new ArgumentNullException(nameof(buffer));
      if (offset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
      if (buffer.Length - count < offset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(offset), buffer, offset, count);

      return base.ReadAsync(buffer, offset, count, cancellationToken); // TODO
    }
  }
}
