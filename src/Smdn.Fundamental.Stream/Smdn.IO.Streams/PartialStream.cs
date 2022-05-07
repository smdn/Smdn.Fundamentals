// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class PartialStream :
  Stream
#pragma warning disable SA1001
#if SYSTEM_ICLONEABLE
  , ICloneable
#endif
#pragma warning restore SA1001
{
  public static PartialStream CreateNonNested(Stream innerOrPartialStream, long length)
    => CreateNonNested(innerOrPartialStream, innerOrPartialStream.Position, length, true);

  public static PartialStream CreateNonNested(Stream innerOrPartialStream, long length, bool seekToBegin)
    => CreateNonNested(innerOrPartialStream, innerOrPartialStream.Position, length, seekToBegin);

  public static PartialStream CreateNonNested(Stream innerOrPartialStream, long offset, long length)
    => CreateNonNested(innerOrPartialStream, offset, length, true);

  public static PartialStream CreateNonNested(Stream innerOrPartialStream, long offset, long length, bool seekToBegin)
  {
    if (innerOrPartialStream is null)
      throw new ArgumentNullException(nameof(innerOrPartialStream));
    if (offset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);

    return innerOrPartialStream is PartialStream partialStream
      ? new(partialStream.InnerStream, partialStream.offset + offset, length, !partialStream.writable, partialStream.LeaveInnerStreamOpen, seekToBegin)
      : new(innerOrPartialStream, offset, length, true, true, seekToBegin);
  }

  public Stream InnerStream {
    get { CheckDisposed(); return stream; }
  }

  public override bool CanSeek => !IsClosed && stream.CanSeek;
  public override bool CanRead => !IsClosed && stream.CanRead;
  public override bool CanWrite => !IsClosed && writable && stream.CanWrite;
  public override bool CanTimeout => !IsClosed && stream.CanTimeout;

  private bool IsClosed => stream is null;

  public override long Position {
    get { CheckDisposed(); return stream.Position - offset; }
    set {
      CheckDisposed();

      if (value < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(Position), value);
      stream.Position = value + offset;
    }
  }

  public override long Length {
    get {
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

#if SYSTEM_IO_STREAM_CLOSE
  public override void Close()
#else
  protected override void Dispose(bool disposing)
#endif
  {
    if (!leaveInnerStreamOpen)
      stream?.Close();

    stream = null;

#if SYSTEM_IO_STREAM_CLOSE
    base.Close();
#else
    base.Dispose(disposing);
#endif
  }

#if SYSTEM_ICLONEABLE
  object ICloneable.Clone() => Clone();
#endif

  public PartialStream Clone() => (PartialStream)MemberwiseClone();

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

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
  {
    CheckDisposed();

    var remainder = GetRemainderLength();

    if (0L < remainder)
      return stream.ReadAsync(buffer.Slice(0, (int)Math.Min(buffer.Length, remainder)), cancellationToken); // XXX: long -> int

    return new(0);
  }
#endif

  private void CheckWriteRemainder(int count, string nameOfCountParameter)
  {
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameOfCountParameter, count);

    if (GetRemainderLength() - count < 0L)
      throw new IOException("attempted to write after end of stream");
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    CheckDisposed();
    CheckWritable();
    CheckWriteRemainder(count, nameof(count));

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#endif

    stream.Write(buffer, offset, count);
  }

  public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
  {
    CheckDisposed();
    CheckWritable();
    CheckWriteRemainder(count, nameof(count));

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#endif

    return stream.WriteAsync(buffer, offset, count, cancellationToken);
  }

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
  public override ValueTask WriteAsync(
    ReadOnlyMemory<byte> buffer,
    CancellationToken cancellationToken = default
  )
  {
    CheckDisposed();
    CheckWritable();
    CheckWriteRemainder(buffer.Length, nameof(buffer));

    return stream.WriteAsync(buffer, cancellationToken);
  }
#endif

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
