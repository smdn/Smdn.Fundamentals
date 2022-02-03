// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO.Streams.Caching;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
#pragma warning disable CA1710 // Identifiers should have correct suffix
public abstract class CachedStreamBase : Stream {
#pragma warning restore CA1710
  public Stream InnerStream {
    get { CheckDisposed(); return stream; }
  }

  public override bool CanSeek => !IsClosed /*&& true*/;
  public override bool CanRead => !IsClosed /*&& true*/;
  public override bool CanWrite => /*!IsClosed &&*/ false;
  public override bool CanTimeout => false;

  private bool IsClosed => stream is null;

  public override long Length {
    get { CheckDisposed(); return stream.Length; }
  }

  public override long Position {
    get { CheckDisposed(); return position; }
    set {
      CheckDisposed();

      if (value < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(Position), value);

      position = value;
    }
  }

  public int BlockSize {
    get { CheckDisposed(); return blockSize; }
  }

  public bool LeaveInnerStreamOpen {
    get { CheckDisposed(); return leaveInnerStreamOpen; }
  }

  protected CachedStreamBase(Stream innerStream, int blockSize, bool leaveInnerStreamOpen)
  {
    if (innerStream == null)
      throw new ArgumentNullException(nameof(innerStream));
    else if (!innerStream.CanSeek)
      throw ExceptionUtils.CreateArgumentMustBeSeekableStream(nameof(innerStream));
    else if (!innerStream.CanRead)
      throw ExceptionUtils.CreateArgumentMustBeReadableStream(nameof(innerStream));

    if (blockSize <= 0)
      throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(blockSize), blockSize);

    this.stream = innerStream;
    this.blockSize = blockSize;
    this.leaveInnerStreamOpen = leaveInnerStreamOpen;

    this.position = stream.Position;
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
      case SeekOrigin.Current:
        offset += position;
        goto case SeekOrigin.Begin;

      case SeekOrigin.End:
        offset += Length;
        goto case SeekOrigin.Begin;

      case SeekOrigin.Begin:
        if (offset < 0L)
          throw ExceptionUtils.CreateIOAttemptToSeekBeforeStartOfStream();
        position = offset;
        return position;

      default:
        throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(origin), origin);
    }
  }

  public override int ReadByte()
  {
    CheckDisposed();

    if (!TryGetBlock(position, out var block))
      return -1; // end of stream

    position++;

    return block[0];
  }

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

    for (; ; ) {
      if (count <= 0)
        return ret;

      if (!TryGetBlock(position, out var block))
        return ret; // end of stream

      var bytesToCopy = Math.Min(block.Length, count);

      block.Slice(0, bytesToCopy).CopyTo(buffer.AsSpan(offset));

      position += bytesToCopy;
      ret += bytesToCopy;
      offset += bytesToCopy;
      count -= bytesToCopy;
    }
  }

  private bool TryGetBlock(long offset, out ReadOnlySpan<byte> block)
  {
#if SYSTEM_MATH_DIVREM
    var blockIndex = Math.DivRem(position, blockSize, out var blockOffset);
#else
    var blockIndex = MathUtils.DivRem(position, (long)blockSize, out var blockOffset);
#endif

    block = default;

    var b = GetBlock(blockIndex);

    if (b.Length <= blockOffset)
      return false;

    block = b.AsSpan((int)blockOffset);

    return true;
  }

  protected abstract byte[] GetBlock(long blockIndex);

  protected byte[] ReadBlock(long blockIndex)
  {
    var block = new byte[blockSize];

    stream.Seek(blockIndex * blockSize, SeekOrigin.Begin);

    var read = stream.Read(block, 0, blockSize);

    if (read < blockSize)
      Array.Resize(ref block, read);

    return block;
  }

  public override void WriteByte(byte @value)
  {
    CheckDisposed();

    throw ExceptionUtils.CreateNotSupportedWritingStream();
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    CheckDisposed();

    throw ExceptionUtils.CreateNotSupportedWritingStream();
  }

  public override void Flush()
  {
    CheckDisposed();

    // do nothing
  }

  private void CheckDisposed()
  {
    if (IsClosed)
      throw new ObjectDisposedException(GetType().FullName);
  }

  private Stream stream;
  private readonly int blockSize;
  private readonly bool leaveInnerStreamOpen;
  private long position;
}
