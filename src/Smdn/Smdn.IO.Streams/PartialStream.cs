// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Smdn.IO;

namespace Smdn.IO.Streams {
  public class PartialStream : 
    Stream
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    , ICloneable
#endif
  {
#region "class members"
    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long length)
    {
      return CreateNonNested(innerOrPartialStream, innerOrPartialStream.Position, length, true);
    }

    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long length, bool seekToBegin)
    {
      return CreateNonNested(innerOrPartialStream, innerOrPartialStream.Position, length, seekToBegin);
    }

    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long offset, long length)
    {
      return CreateNonNested(innerOrPartialStream, offset, length, true);
    }

    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long offset, long length, bool seekToBegin)
    {
      if (innerOrPartialStream == null)
        throw new ArgumentNullException(nameof(innerOrPartialStream));
      if (offset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);

      var partialStream = innerOrPartialStream as PartialStream;

      if (partialStream == null) {
        return new PartialStream(innerOrPartialStream, offset, length, true, true, seekToBegin);
      }
      else {
        var innerStream = partialStream.InnerStream;

        return new PartialStream(innerStream, partialStream.offset + offset, length, !partialStream.writable, partialStream.LeaveInnerStreamOpen, seekToBegin);
      }
    }
#endregion

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
      get { return !IsClosed && writable && stream.CanWrite; }
    }

    public override bool CanTimeout {
      get { return !IsClosed && stream.CanTimeout; }
    }

    private bool IsClosed {
      get { return stream == null; }
    }

    public override long Position {
      get { CheckDisposed(); return stream.Position - offset; }
      set
      {
        CheckDisposed();

        if (value < 0)
          throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(Position), value);
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
      : this(innerStream, offset, null, false, true, true)
    {
    }

    public PartialStream(Stream innerStream, long offset, bool leaveInnerStreamOpen)
      : this(innerStream, offset, null, false, leaveInnerStreamOpen, true)
    {
    }

    public PartialStream(Stream innerStream, long offset, bool @readonly, bool leaveInnerStreamOpen)
      : this(innerStream, offset, null, @readonly, leaveInnerStreamOpen, true)
    {
    }

    public PartialStream(Stream innerStream, long offset, bool @readonly, bool leaveInnerStreamOpen, bool seekToBegin)
      : this(innerStream, offset, null, @readonly, leaveInnerStreamOpen, seekToBegin)
    {
    }

    public PartialStream(Stream innerStream, long offset, long length)
      : this(innerStream, offset, length, false, true, true)
    {
    }

    public PartialStream(Stream innerStream, long offset, long length, bool leaveInnerStreamOpen)
      : this(innerStream, offset, (long?)length, false, leaveInnerStreamOpen, true)
    {
    }

    public PartialStream(Stream innerStream, long offset, long length, bool @readonly, bool leaveInnerStreamOpen)
      : this(innerStream, offset, (long?)length, @readonly, leaveInnerStreamOpen, true)
    {
    }

    public PartialStream(Stream innerStream, long offset, long length, bool @readonly, bool leaveInnerStreamOpen, bool seekToBegin)
      : this(innerStream, offset, (long?)length, @readonly, leaveInnerStreamOpen, seekToBegin)
    {
    }

    private PartialStream(Stream innerStream, long offset, long? length, bool @readonly, bool leaveInnerStreamOpen, bool seekToBegin)
    {
      if (innerStream == null)
        throw new ArgumentNullException(nameof(innerStream));
      if (!innerStream.CanSeek)
        throw ExceptionUtils.CreateArgumentMustBeSeekableStream(nameof(innerStream));
      if (offset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);
      if (length.HasValue && length.Value < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(length), length.Value);

      this.stream = innerStream;
      this.offset = offset;
      this.length = length;
      this.writable = !@readonly;
      this.leaveInnerStreamOpen = leaveInnerStreamOpen;

      if (seekToBegin)
        this.Position = 0;
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

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    object ICloneable.Clone()
    {
      return Clone();
    }
#endif

    public PartialStream Clone()
    {
      return (PartialStream)MemberwiseClone();
    }

    public override void SetLength(long @value)
    {
      CheckDisposed();

      throw ExceptionUtils.CreateNotSupportedSettingStreamLength();
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
          throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(origin), origin);
      }

      throw ExceptionUtils.CreateIOAttemptToSeekBeforeStartOfStream();
    }

    public override void Flush()
    {
      CheckDisposed();

      stream.Flush();
    }
  
    protected long GetRemainderLength()
    {
      if (length.HasValue)
        return length.Value - (stream.Position - offset);
      else
        return long.MaxValue;
    }

    public override int ReadByte()
    {
      CheckDisposed();

      if (0L < GetRemainderLength())
        return stream.ReadByte();
      else
        return -1;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      CheckDisposed();

      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);

      var remainder = GetRemainderLength();

      if (0L < remainder)
        return stream.Read(buffer, offset, (int)Math.Min(count, remainder)); // XXX: long -> int
      else
        return 0;
    }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
      CheckDisposed();

      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);

      var remainder = GetRemainderLength();

      if (0L < remainder)
        return stream.ReadAsync(buffer, offset, (int)Math.Min(count, remainder), cancellationToken); // XXX: long -> int
      else
        return Task.FromResult(0);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      CheckDisposed();
      CheckWritable();

      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);

      var remainder = GetRemainderLength() - count;

      if (remainder < 0L)
        throw new IOException("attempted to write after end of stream");
      else
        stream.Write(buffer, offset, count);
    }

    private void CheckDisposed()
    {
      if (IsClosed)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private void CheckWritable()
    {
      if (!writable)
        throw ExceptionUtils.CreateNotSupportedWritingStream();
    }

    private Stream stream;
    private readonly long offset;
    private readonly long? length;
    private readonly bool writable;
    private readonly bool leaveInnerStreamOpen;
  }
}
