// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO.Streams;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public sealed class ChunkedMemoryStream : Stream {
  public static readonly int DefaultChunkSize = 40960;

  public delegate Chunk Allocator(int chunkSize);

#pragma warning disable CA1063 // TODO: Implement IDisposable correctly
  public abstract class Chunk : IDisposable {
    public abstract void Dispose();

#pragma warning disable SA1401, CA1051, CA2213
    public byte[] Data;
    internal Chunk Next = null;
#pragma warning restore SA1401, CA1051, CA2213
  }
#pragma warning restore CA1063

  private sealed class DefaultChunk : Chunk {
    public static DefaultChunk Allocate(int chunkSize) => new(chunkSize);

    private DefaultChunk(int chunkSize)
    {
      Data = new byte[chunkSize];
    }

    public override void Dispose()
    {
      Data = null;
    }
  }

  private sealed class ChunkChain : IDisposable {
    public int ChunkSize { get; }
    public long Length => ((chunkCount - 1) * ChunkSize) + lastChunkLength;
    public long Position => (currentChunkIndex * ChunkSize) + currentChunkOffset;

    private int ReadableByteCount => ((currentChunk.Next is null) ? lastChunkLength : ChunkSize) - currentChunkOffset;

    public ChunkChain(int chunkSize, Allocator allocator)
    {
      this.ChunkSize = chunkSize;
      this.allocator = allocator;

      currentChunkOffset = 0;
    }

    private Chunk TryGetFirstChunk(bool ensureCreated)
    {
      if (ensureCreated && firstChunk == null)
        currentChunk = firstChunk = allocator(ChunkSize);

      return firstChunk;
    }

    public void Dispose()
    {
      var chunk = TryGetFirstChunk(false);

      for (; ; ) {
        if (chunk == null)
          break;

        var next = chunk.Next;

        chunk.Dispose();

        chunk = next;
      }

      firstChunk = null;
      currentChunk = null;
    }

    public void SetLength(long length)
    {
      if (length == Length)
        return; // do nothing

      chunkCount = 1;

      var chunk = TryGetFirstChunk(true);

      // allocate
      while (ChunkSize <= length) {
        chunk.Next ??= allocator(ChunkSize);

        chunk = chunk.Next;
        chunkCount++;

        length -= ChunkSize;
      }

      lastChunkLength = (int)length;

      // dispose and remove from chain
      var next = chunk.Next;

      chunk.Next = null;
      chunk = next;

      while (chunk != null) {
        next = chunk.Next;

        chunk.Next = null;
        chunk.Dispose();

        chunk = next;
      }

      // set new position
      if (Length < Position)
        SetPosition(Length);
    }

    public void SetPosition(long offset)
    {
      if (Position == offset)
        return;

      if (Length < offset)
        SetLength(offset);

      currentChunk = TryGetFirstChunk(true);
      currentChunkIndex = 0;

      while (ChunkSize <= offset) {
        currentChunk = currentChunk.Next;
        currentChunkIndex++;

        offset -= ChunkSize;
      }

      currentChunkOffset = (int)offset;
    }

    public int ReadByte()
    {
      if (TryGetFirstChunk(false) == null)
        return -1; // stream is empty

      if (ReadableByteCount == 0) {
        if (!MoveToNextChunk(false))
          return -1; // end of stream
      }

      return currentChunk.Data[currentChunkOffset++];
    }

    public int Read(byte[] buffer, int offset, int count)
    {
      if (TryGetFirstChunk(false) == null)
        return 0; // stream is empty

      var read = 0;

      for (; ; ) {
        if (ReadableByteCount == 0) {
          if (!MoveToNextChunk(false))
            return read; // end of stream
        }

        var bytesToRead = Math.Min(ReadableByteCount, count);

        Buffer.BlockCopy(currentChunk.Data, currentChunkOffset, buffer, offset, bytesToRead);

        offset += bytesToRead;
        count -= bytesToRead;
        read += bytesToRead;

        currentChunkOffset += bytesToRead;

        if (count <= 0)
          return read;
      }
    }

    public void WriteByte(byte @value)
    {
      TryGetFirstChunk(true);

      if (ChunkSize == currentChunkOffset)
        MoveToNextChunk(true);

      currentChunk.Data[currentChunkOffset++] = @value;

      if (currentChunk.Next == null)
        lastChunkLength = currentChunkOffset;
    }

    public void Write(byte[] buffer, int offset, int count)
    {
      TryGetFirstChunk(true);

      for (; ; ) {
        if (currentChunkOffset == ChunkSize)
          MoveToNextChunk(true);

        var bytesToWrite = Math.Min(ChunkSize - currentChunkOffset, count);

        Buffer.BlockCopy(buffer, offset, currentChunk.Data, currentChunkOffset, bytesToWrite);

        offset += bytesToWrite;
        count -= bytesToWrite;

        currentChunkOffset += bytesToWrite;

        if (currentChunk.Next == null)
          lastChunkLength = currentChunkOffset;

        if (count <= 0)
          return;
      }
    }

    private bool MoveToNextChunk(bool write)
    {
      if (currentChunk.Next == null) {
        if (!write)
          return false;

        currentChunk.Next = allocator(ChunkSize);
        chunkCount++;
        lastChunkLength = 0;
      }

      currentChunk = currentChunk.Next;
      currentChunkIndex++;

      currentChunkOffset = 0;

      return true;
    }

    public byte[] ToArray()
    {
      var buffer = new byte[Length];
#if SYSTEM_ARRAY_COPY_INT64LENGTH
      long offset = 0L;
#else
      int offset = 0;
#endif
      var chunk = TryGetFirstChunk(false);

      if (chunk == null)
        return buffer; // stream is empty (length must be zero)

      for (; ; ) {
        if (chunk.Next == null) {
          Array.Copy(chunk.Data, 0, buffer, offset, lastChunkLength);
          // Buffer.BlockCopy(chunk.Data, 0, buffer, offset, lastChunkLength);

          return buffer;
        }
        else {
          Array.Copy(chunk.Data, 0, buffer, offset, ChunkSize);
          // Buffer.BlockCopy(chunk.Data, 0, buffer, offset, chunkSize);

          chunk = chunk.Next;
          offset += ChunkSize;
        }
      }
    }

    private readonly Allocator allocator;
    private Chunk firstChunk;
#pragma warning disable CA2213
    private Chunk currentChunk;
#pragma warning restore CA2213

    private int currentChunkOffset;
    private long currentChunkIndex = 0;
    private long chunkCount = 1;
    private int lastChunkLength = 0;
  }

  public override bool CanSeek => !IsClosed /*&& true*/;
  public override bool CanRead => !IsClosed /*&& true*/;
  public override bool CanWrite => !IsClosed /*&& true*/;
  public override bool CanTimeout => false;
  private bool IsClosed => chain is null;

  public override long Position {
    get { CheckDisposed(); return chain.Position; }
    set {
      CheckDisposed();

      if (value < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(Position), value);

      chain.SetPosition(value);
    }
  }

  public override long Length {
    get { CheckDisposed(); return chain.Length; }
  }

  public int ChunkSize {
    get { CheckDisposed(); return chain.ChunkSize; }
  }

  public ChunkedMemoryStream()
    : this(DefaultChunkSize, DefaultChunk.Allocate)
  {
  }

  public ChunkedMemoryStream(int chunkSize)
    : this(chunkSize, DefaultChunk.Allocate)
  {
  }

  public ChunkedMemoryStream(Allocator allocator)
    : this(DefaultChunkSize, allocator)
  {
  }

  public ChunkedMemoryStream(int chunkSize, Allocator allocator)
  {
    if (chunkSize <= 0)
      throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(chunkSize), chunkSize);
    if (allocator == null)
      throw new ArgumentNullException(nameof(allocator));

    this.chain = new ChunkChain(chunkSize, allocator);
  }

#if SYSTEM_IO_STREAM_CLOSE
  public override void Close()
#else
  protected override void Dispose(bool disposing)
#endif
  {
    if (chain != null) {
      chain.Dispose();
      chain = null;
    }

#if SYSTEM_IO_STREAM_CLOSE
    base.Close();
#else
    base.Dispose(disposing);
#endif
  }

  public override void SetLength(long @value)
  {
    CheckDisposed();

    if (@value < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(value), @value);

    chain.SetLength(@value);
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    CheckDisposed();

    // Stream.Seek spec: Seeking to any location beyond the length of the stream is supported.
    switch (origin) {
      case SeekOrigin.Current:
        offset += chain.Position;
        goto case SeekOrigin.Begin;

      case SeekOrigin.End:
        offset += chain.Length;
        goto case SeekOrigin.Begin;

      case SeekOrigin.Begin:
        if (offset < 0L)
          throw ExceptionUtils.CreateIOAttemptToSeekBeforeStartOfStream();
        chain.SetPosition(offset);
        return chain.Position;

      default:
        throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(origin), origin);
    }
  }

  public override int ReadByte()
  {
    CheckDisposed();

    return chain.ReadByte();
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    CheckDisposed();

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#else
    if (buffer == null)
      throw new ArgumentNullException(nameof(buffer));
    if (offset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
    if (buffer.Length - count < offset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(offset), buffer, offset, count);
#endif

    if (count == 0)
      return 0;
    else
      return chain.Read(buffer, offset, count);
  }

  public override void WriteByte(byte @value)
  {
    CheckDisposed();

    chain.WriteByte(@value);
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    CheckDisposed();

#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
    ValidateBufferArguments(buffer, offset, count);
#else
    if (buffer == null)
      throw new ArgumentNullException(nameof(buffer));
    if (offset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
    if (buffer.Length - count < offset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(offset), buffer, offset, count);
#endif

    if (count == 0)
      return;
    else
      chain.Write(buffer, offset, count);
  }

  public override void Flush()
  {
    CheckDisposed();

    // do nothing
  }

  public byte[] ToArray()
  {
    CheckDisposed();

    return chain.ToArray();
  }

  private void CheckDisposed()
  {
    if (IsClosed)
      throw new ObjectDisposedException(GetType().FullName);
  }

  private ChunkChain chain = null;
}
