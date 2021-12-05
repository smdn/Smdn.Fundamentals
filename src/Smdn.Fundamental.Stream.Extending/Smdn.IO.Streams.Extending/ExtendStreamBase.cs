// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Smdn.IO;

namespace Smdn.IO.Streams.Extending {
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
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

#if SYSTEM_IO_STREAM_CLOSE
    public override void Close()
#else
    protected override void Dispose(bool disposing)
#endif
    {
      if (!leaveInnerStreamOpen)
#if SYSTEM_IO_STREAM_CLOSE
        stream?.Close();
#else
        stream?.Dispose();
#endif

      stream = null;

#if SYSTEM_IO_STREAM_CLOSE
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
        int readCount;
        StreamSection nextSection;

        switch (Section) {
          case StreamSection.Prepend:
            readCount = ReadPrependedData(buffer, offset, count);
            nextSection = StreamSection.Stream;
            break;

          case StreamSection.Stream:
            readCount = stream.Read(buffer, offset, count);
            nextSection = (appendLength == 0L) ? StreamSection.EndOfStream : StreamSection.Append;
            break;

          case StreamSection.Append:
            readCount = ReadAppendedData(buffer, offset, count);
            nextSection = StreamSection.EndOfStream;
            break;

          default:
          // case StreamSection.EndOfStream:
            return ret;
        } // switch

        if (readCount == 0) {
          Section = nextSection;
        }
        else {
          ret       += readCount;
          count     -= readCount;
          offset    += readCount;
          position  += readCount;
        }
      } // while

      return ret;
    }

    protected abstract Task<int> ReadPrependedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
    protected abstract Task<int> ReadAppendedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

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

      return ReadAsyncCore();

      async Task<int> ReadAsyncCore()
      {
        var ret = 0;

        while (0 < count) {
          int readCount;
          StreamSection nextSection;

          switch (Section) {
            case StreamSection.Prepend:
              readCount = await ReadPrependedDataAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
              nextSection = StreamSection.Stream;
              break;

            case StreamSection.Stream:
              readCount = await stream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
              nextSection = (appendLength == 0L) ? StreamSection.EndOfStream : StreamSection.Append;
              break;

            case StreamSection.Append:
              readCount = await ReadAppendedDataAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
              nextSection = StreamSection.EndOfStream;
              break;

            default:
              // case StreamSection.EndOfStream:
              return ret;
          } // switch

          if (readCount == 0) {
            Section = nextSection;
          }
          else {
            ret       += readCount;
            count     -= readCount;
            offset    += readCount;
            position  += readCount;
          }
        } // while

        return ret;
      }
    }
  }
}
