// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.LineOriented;

#pragma warning disable IDE0040
partial class LineOrientedStream {
#pragma warning restore IDE0040
  public override int ReadByte()
  {
    CheckDisposed();

    if (bufRemain == 0 && FillBufferAsync(default).GetAwaiter().GetResult() <= 0)
      return -1;

    bufRemain--;

    return buffer[bufOffset++];
  }

  public override int Read(byte[] buffer, int offset, int count)
    => ReadAsync(buffer, offset, count, default).GetAwaiter().GetResult();

  public override Task<int> ReadAsync(
    byte[] buffer,
    int offset,
    int count,
    CancellationToken cancellationToken
  )
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

    if (cancellationToken.IsCancellationRequested)
#if SYSTEM_THREADING_TASKS_TASK_FROMCANCELED
      return Task.FromCanceled<int>(cancellationToken);
#else
      return new Task<int>(() => default, cancellationToken);
#endif
    if (count == 0L)
      return Task.FromResult(0); // do nothing

    return ReadAsyncCore(
      destination: buffer.AsMemory(offset, count),
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
    CheckDisposed();

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

    return ReadAsyncCore(
      destination: buffer,
      cancellationToken: cancellationToken
    );
  }
#endif

  private async
#if SYSTEM_THREADING_TASKS_VALUETASK
  ValueTask<int>
#else
  Task<int>
#endif
  ReadAsyncCore(
    Memory<byte> destination,
    CancellationToken cancellationToken
  )
  {
    if (destination.Length <= bufRemain) {
      buffer.AsSpan(bufOffset, destination.Length).CopyTo(destination.Span);
      bufOffset += destination.Length;
      bufRemain -= destination.Length;

      return destination.Length;
    }

    var read = 0;

    if (bufRemain != 0) {
      buffer.AsSpan(bufOffset, bufRemain).CopyTo(destination.Span);

      read = bufRemain;

      destination = destination.Slice(bufRemain);

      bufRemain = 0;
    }

    // read from base stream
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
