// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if !SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
using System.Buffers; // ArrayPool
#endif
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.LineOriented;

#pragma warning disable IDE0040
partial class LineOrientedStream {
#pragma warning restore IDE0040
  public override int ReadByte()
  {
    ThrowIfDisposed();

    if (bufRemain == 0 && FillBuffer() <= 0)
      return -1;

    bufRemain--;

    return buffer[bufOffset++];
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    ThrowIfDisposed();

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

    var destination = buffer.AsSpan(offset, count);

#if SYSTEM_IO_STREAM_READ_SPAN_OF_BYTE
    return Read(destination);
#else
    if (destination.IsEmpty)
      return 0; // do nothing

    destination = ReadFromBuffer(destination, out var bytesRead);

    if (destination.IsEmpty)
      return bytesRead;

    return ReadFromUnderlyingStream(
      bytesAlreadyReadIntoDestination: bytesRead,
      destination: new ArraySegment<byte>(buffer, offset + bytesRead, count - bytesRead)
    );
#endif
  }

#if SYSTEM_IO_STREAM_READ_SPAN_OF_BYTE
  public override int Read(Span<byte> buffer)
  {
    if (buffer.IsEmpty)
      return 0; // do nothing

    var destination = ReadFromBuffer(buffer, out var bytesRead);

    if (destination.IsEmpty)
      return bytesRead;

    return ReadFromUnderlyingStream(
      bytesAlreadyReadIntoDestination: bytesRead,
      destination: destination
    );
  }
#endif

  public override Task<int> ReadAsync(
    byte[] buffer,
    int offset,
    int count,
    CancellationToken cancellationToken
  )
  {
    ThrowIfDisposed();

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

    if (cancellationToken.IsCancellationRequested)
#if SYSTEM_THREADING_TASKS_TASK_FROMCANCELED
      return Task.FromCanceled<int>(cancellationToken);
#else
      return new Task<int>(() => default, cancellationToken);
#endif

    if (count == 0)
      return Task.FromResult(0); // do nothing

    var destination = buffer.AsMemory(offset, count);

    ReadFromBuffer(ref destination, out var bytesRead);

    if (destination.IsEmpty)
      return Task.FromResult(bytesRead);

    return ReadFromUnderlyingStreamAsync(
      bytesAlreadyReadIntoDestination: bytesRead,
      destination: destination,
      cancellationToken: cancellationToken
#if SYSTEM_THREADING_TASKS_VALUETASK
    ).AsTask();
#else
    );
#endif
  }

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  public override ValueTask<int> ReadAsync(
    Memory<byte> buffer,
    CancellationToken cancellationToken = default
  )
  {
    ThrowIfDisposed();

    if (cancellationToken.IsCancellationRequested)
#if SYSTEM_THREADING_TASKS_VALUETASK_FROMCANCELED
      return ValueTask.FromCanceled<int>(cancellationToken);
#else
#if SYSTEM_THREADING_TASKS_TASK_FROMCANCELED
      return new(Task.FromCanceled<int>(cancellationToken));
#else
      return new(new Task<int>(() => default, cancellationToken));
#endif
#endif

    if (buffer.IsEmpty)
      return new(0); // do nothing

    ReadFromBuffer(ref buffer, out var bytesRead);

    if (buffer.IsEmpty)
      return new(bytesRead);

    return ReadFromUnderlyingStreamAsync(
      bytesAlreadyReadIntoDestination: bytesRead,
      destination: buffer,
      cancellationToken: cancellationToken
    );
  }
#endif

  private void ReadFromBuffer(ref Memory<byte> destination, out int bytesRead)
  {
    ReadFromBuffer(destination.Span, out bytesRead);

    destination = destination.Slice(bytesRead);
  }

  private Span<byte> ReadFromBuffer(Span<byte> destination, out int bytesRead)
  {
    var bytesToRead = Math.Min(destination.Length, bufRemain);

    if (0 < bytesToRead) {
      buffer.AsSpan(bufOffset, bytesToRead).CopyTo(destination);

      bufOffset += bytesToRead;
      bufRemain -= bytesToRead;

      destination = destination.Slice(bytesToRead);
    }

    bytesRead = bytesToRead;

    return destination;
  }

  private int ReadFromUnderlyingStream(
    int bytesAlreadyReadIntoDestination,
#if SYSTEM_IO_STREAM_READ_SPAN_OF_BYTE
    Span<byte> destination
#else
    ArraySegment<byte> destination
#endif
  )
  {
#if DEBUG
    if (0 < bufRemain)
      throw new InvalidOperationException($"call {nameof(ReadFromBuffer)} first");
#endif

    var read = bytesAlreadyReadIntoDestination;

    for (; ; ) {
      if (
#if SYSTEM_IO_STREAM_READ_SPAN_OF_BYTE
        destination.IsEmpty
#else
        destination.Count == 0
#endif
      ) {
        break;
      }

      var r =
#if SYSTEM_IO_STREAM_READ_SPAN_OF_BYTE
        stream.Read(destination);
#else
        stream.Read(destination.Array, destination.Offset, destination.Count);
#endif

      if (r <= 0)
        break;

      destination =
#if SYSTEM_IO_STREAM_READ_SPAN_OF_BYTE
        destination.Slice(r);
#else
        new(destination.Array, destination.Offset + r, destination.Count - r);
#endif

      read += r;
    }

    return read;
  }

  private async
#if SYSTEM_THREADING_TASKS_VALUETASK
  ValueTask<int>
#else
  Task<int>
#endif
  ReadFromUnderlyingStreamAsync(
    int bytesAlreadyReadIntoDestination,
    Memory<byte> destination,
    CancellationToken cancellationToken
  )
  {
#if DEBUG
    if (0 < bufRemain)
      throw new InvalidOperationException($"call {nameof(ReadFromBuffer)} first");
#endif

    var read = bytesAlreadyReadIntoDestination;

    for (; ; ) {
      if (destination.IsEmpty)
        break;

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
      var r = await stream.ReadAsync(
        destination,
        cancellationToken
      ).ConfigureAwait(false);
#else
#pragma warning disable CA1835
      byte[] readBuffer = null;
      int r = 0;

      try {
        readBuffer = ArrayPool<byte>.Shared.Rent(destination.Length);

        r = await stream.ReadAsync(
          readBuffer,
          0,
          destination.Length,
          cancellationToken
        ).ConfigureAwait(false);
      }
      finally {
        if (readBuffer is not null) {
          if (0 < r)
            readBuffer.AsMemory(0, r).CopyTo(destination);

          ArrayPool<byte>.Shared.Return(readBuffer);
        }
      }
#pragma warning restore CA1835
#endif

      if (r <= 0)
        break;

      destination = destination.Slice(r);
      read += r;
    }

    return read;
  }
}
