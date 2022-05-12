// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.LineOriented;

#pragma warning disable IDE0040
partial class LineOrientedStream {
#pragma warning restore IDE0040
  public Task<long> ReadAsync(
    Stream targetStream,
    long length,
    CancellationToken cancellationToken = default
  )
  {
    CheckDisposed();

    ValidateReadToStreamArguments(targetStream, length);

    if (cancellationToken.IsCancellationRequested)
#if SYSTEM_THREADING_TASKS_TASK_FROMCANCELED
      return Task.FromCanceled<long>(cancellationToken);
#else
      return new Task<long>(() => default, cancellationToken);
#endif

    if (length == 0L)
      return Task.FromResult(0L); // do nothing

    return ReadToStreamAsync(
      targetStream,
      length,
      cancellationToken
    );
  }

  private async Task<long> ReadToStreamAsync(
    Stream destination,
    long? bytesToRead,
    CancellationToken cancellationToken = default
  )
  {
    var read = await ReadToStreamFromBufferAsync(
      destination,
      bytesToRead,
      cancellationToken
    ).ConfigureAwait(false);

    if (bytesToRead.HasValue) {
      var bytesToReadFromUnderlyingStream = bytesToRead.Value - read;

      if (bytesToReadFromUnderlyingStream == 0L)
        return read;

      return read + await ReadToStreamCopyUnderlyingStreamAsync(
        bytesToRead: bytesToReadFromUnderlyingStream,
        destination: destination,
        cancellationToken: cancellationToken
      ).ConfigureAwait(false);
    }
    else {
      return read + await ReadToStreamCopyEntireUnderlyingStreamAsync(
        destination: destination,
        cancellationToken: cancellationToken
      ).ConfigureAwait(false);
    }
  }

  private async Task<long> ReadToStreamFromBufferAsync(
    Stream destination,
    long? bytesToRead,
    CancellationToken cancellationToken = default
  )
  {
    if (bufRemain == 0)
      return 0;

    var bytesToReadFromBuffer = bytesToRead.HasValue
      ? (int)Math.Min(bufRemain, bytesToRead.Value)
      : bufRemain;

    await destination.WriteAsync(
#pragma warning disable SA1114, SA1117
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      buffer.AsMemory(bufOffset, bytesToReadFromBuffer),
#else
      buffer, bufOffset, bytesToReadFromBuffer,
#endif
      cancellationToken
#pragma warning restore SA1114, SA1117
    ).ConfigureAwait(false);

    bufRemain -= bytesToReadFromBuffer;
    bufOffset += bytesToReadFromBuffer;

    return bytesToReadFromBuffer;
  }

  private async Task<long> ReadToStreamCopyEntireUnderlyingStreamAsync(
    Stream destination,
    CancellationToken cancellationToken = default
  )
  {
#if DEBUG
    if (0 < bufRemain)
      throw new InvalidOperationException($"call {nameof(ReadToStreamFromBufferAsync)} first");
#endif

    var read = 0L;

    for (; ; ) {
      var r =
        await stream.ReadAsync(
#pragma warning disable SA1114, SA1117
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          buffer.AsMemory(),
#else
          buffer, 0, buffer.Length,
#endif
          cancellationToken
#pragma warning restore SA1114, SA1117
        ).ConfigureAwait(false);

      await destination.WriteAsync(
#pragma warning disable SA1114, SA1117
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
        buffer.AsMemory(0, r),
#else
        buffer, 0, r,
#endif
        cancellationToken
#pragma warning restore SA1114, SA1117
      ).ConfigureAwait(false);

      read += r;

      if (r < buffer.Length)
        break;
    }

    return read;
  }

  private async Task<long> ReadToStreamCopyUnderlyingStreamAsync(
    Stream destination,
    long bytesToRead,
    CancellationToken cancellationToken = default
  )
  {
#if DEBUG
    if (0 < bufRemain)
      throw new InvalidOperationException($"call {nameof(ReadToStreamFromBufferAsync)} first");
#endif

    var read = 0L;

    for (; ; ) {
      if (bytesToRead <= 0)
        break;

      var bytesToReadFromUnderlyingStream = (int)Math.Min(bytesToRead, buffer.Length);
      var r = await stream.ReadAsync(
#pragma warning disable SA1114, SA1117
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
        buffer.AsMemory(0, bytesToReadFromUnderlyingStream),
#else
        buffer, 0, bytesToReadFromUnderlyingStream,
#endif
        cancellationToken
#pragma warning restore SA1114, SA1117
      ).ConfigureAwait(false);

      if (r <= 0)
        break;

      await destination.WriteAsync(
#pragma warning disable SA1114, SA1117
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
        buffer.AsMemory(0, r),
#else
        buffer, 0, r,
#endif
        cancellationToken
#pragma warning restore SA1114, SA1117
      ).ConfigureAwait(false);

      bytesToRead -= r;
      read += r;
    }

    return read;
  }
}
