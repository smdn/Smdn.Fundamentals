// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.LineOriented;

#pragma warning disable IDE0040
partial class LineOrientedStream {
#pragma warning restore IDE0040
  public long Read(Stream targetStream, long length)
    => ReadAsync(targetStream, length).GetAwaiter().GetResult();

  public Task<long> ReadAsync(
    Stream targetStream,
    long length,
    CancellationToken cancellationToken = default
  )
  {
    CheckDisposed();

    if (targetStream == null)
      throw new ArgumentNullException(nameof(targetStream));
    if (cancellationToken.IsCancellationRequested)
#if SYSTEM_THREADING_TASKS_TASK_FROMCANCELED
      return Task.FromCanceled<long>(cancellationToken);
#else
      return new Task<long>(() => default, cancellationToken);
#endif
    if (length == 0L)
      return Task.FromResult(0L); // do nothing
    if (length < 0L)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(length), length);

    return ReadAsyncCore(
      targetStream,
      length,
      cancellationToken
    );
  }

  private async Task<long> ReadAsyncCore(
    Stream targetStream,
    long? bytesToRead,
    CancellationToken cancellationToken = default
  )
  {
    if (bytesToRead.HasValue && bytesToRead <= bufRemain) {
      var count = (int)bytesToRead;

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      await targetStream.WriteAsync(
        buffer.AsMemory(bufOffset, count),
#else
#pragma warning disable CA1835
      await targetStream.WriteAsync(
        buffer,
        bufOffset,
        count,
#pragma warning restore CA1835
#endif
        cancellationToken
      ).ConfigureAwait(false);

      bufOffset += count;
      bufRemain -= count;

      return count;
    }

    var read = 0L;

    if (0 < bufRemain) {
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      await targetStream.WriteAsync(
        buffer.AsMemory(bufOffset, bufRemain),
#else
#pragma warning disable CA1835
      await targetStream.WriteAsync(
        buffer,
        bufOffset,
        bufRemain,
#pragma warning restore CA1835
#endif
        cancellationToken
      ).ConfigureAwait(false);

      read = bufRemain;
      bytesToRead -= bufRemain;

      bufRemain = 0;
    }

    // read from base stream
    if (bytesToRead.HasValue) {
      for (; ; ) {
        if (bytesToRead <= 0)
          break;

        var r =
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          await stream.ReadAsync(
            buffer.AsMemory(0, (int)Math.Min(bytesToRead.Value, buffer.Length)),
#else
#pragma warning disable CA1835
          await stream.ReadAsync(
            buffer,
            0,
            (int)Math.Min(bytesToRead.Value, buffer.Length),
#pragma warning restore CA1835
#endif
            cancellationToken
          ).ConfigureAwait(false);

        if (r <= 0)
          break;

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
        await targetStream.WriteAsync(
          buffer.AsMemory(0, r),
#else
#pragma warning disable CA1835
        await targetStream.WriteAsync(
          buffer,
          0,
          r,
#pragma warning restore CA1835
#endif
          cancellationToken
        ).ConfigureAwait(false);

        bytesToRead -= r;
        read += r;
      }
    }
    else {
      for (; ; ) {
        var r =
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          await stream.ReadAsync(
            buffer.AsMemory(),
#else
#pragma warning disable CA1835
          await stream.ReadAsync(
            buffer,
            0,
            buffer.Length,
#pragma warning restore CA1835
#endif
            cancellationToken
          ).ConfigureAwait(false);

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
        await targetStream.WriteAsync(
          buffer.AsMemory(0, r),
#else
#pragma warning disable CA1835
        await targetStream.WriteAsync(
          buffer,
          0,
          r,
#pragma warning restore CA1835
#endif
          cancellationToken
        ).ConfigureAwait(false);

        read += r;

        if (r < buffer.Length)
          break;
      }
    }

    return read;
  }
}
