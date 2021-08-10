// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO {
  public static class StreamExtensions {
#if !SYSTEM_IO_STREAM_CLOSE
    public static void Close(this Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

      stream.Dispose();
    }
#endif

    private const int defaultCopyBufferSize = 10 * 1024;

    public static void CopyTo(
      this Stream stream,
      System.IO.BinaryWriter writer,
      int bufferSize = defaultCopyBufferSize
    )
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));
      if (writer == null)
        throw new ArgumentNullException(nameof(writer));
      if (bufferSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(bufferSize), bufferSize);

      var buffer = new byte[bufferSize]; // TODO: array pool

      for (;;) {
        var read = stream.Read(buffer, 0, bufferSize);

        if (read <= 0)
          break;

        writer.Write(buffer, 0, read);
      }
    }

    public static Task CopyToAsync(
      this Stream stream,
      System.IO.BinaryWriter writer,
      int bufferSize = defaultCopyBufferSize,
      CancellationToken cancellationToken = default
    )
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));
      if (writer == null)
        throw new ArgumentNullException(nameof(writer));
      if (bufferSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(bufferSize), bufferSize);

      return _CopyToAsync();

      async Task _CopyToAsync()
      {
        var buffer = new byte[bufferSize]; // TODO: array pool

        for (; ; ) {
          var read = await stream.ReadAsync(buffer, 0, bufferSize, cancellationToken).ConfigureAwait(false);

          if (read <= 0)
            break;

          writer.Write(buffer, 0, read/*, cancellationToken*/);
        }
      }
    }

    private const int defaultReadBufferSize = 4096;
    private const int defaultInitialBufferCapacity = 4096;

    public static byte[] ReadToEnd(
      this Stream stream,
      int readBufferSize = defaultReadBufferSize,
      int initialCapacity = defaultInitialBufferCapacity
    )
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));
      if (readBufferSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(readBufferSize), readBufferSize);
      if (initialCapacity < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(initialCapacity), initialCapacity);

      using (var outStream = new MemoryStream(initialCapacity)) {
        stream.CopyTo(outStream, readBufferSize);

        outStream.Close();

        return outStream.ToArray();
      }
    }

    public static Task<byte[]> ReadToEndAsync(
      this Stream stream,
      int readBufferSize = defaultReadBufferSize,
      int initialCapacity = defaultInitialBufferCapacity,
      CancellationToken cancellationToken = default
    )
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));
      if (readBufferSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(readBufferSize), readBufferSize);
      if (initialCapacity < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(initialCapacity), initialCapacity);

      return _ReadToEndAsync();

      async Task<byte[]> _ReadToEndAsync()
      {
        using (var outStream = new MemoryStream(initialCapacity)) {
          await stream.CopyToAsync(outStream, readBufferSize, cancellationToken).ConfigureAwait(false);

          outStream.Close();

          return outStream.ToArray();
        }
      }
    }

    public static void Write(this Stream stream, ArraySegment<byte> segment)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

      stream.Write(segment.Array, segment.Offset, segment.Count);
    }

    public static void Write(
      this Stream stream,
      ReadOnlySequence<byte> sequence
    )
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

      if (sequence.IsEmpty)
        return;

      var pos = sequence.Start;
#if !NETSTANDARD2_1
      byte[] buffer = null;
#endif

      while (sequence.TryGet(ref pos, out var memory, advance: true)) {
#if NETSTANDARD2_1
        stream.Write(memory.Span);
#else
        if (buffer == null || buffer.Length < memory.Length)
          buffer = new byte[memory.Length]; // TODO: ArrayPool.Shared.Rent()

        memory.CopyTo(buffer);

        stream.Write(buffer, 0, memory.Length);
#endif
      }
    }

    public static Task WriteAsync(
      this Stream stream,
      ReadOnlySequence<byte> sequence,
      CancellationToken cancellationToken = default
    )
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

      if (sequence.IsEmpty)
#if SYSTEM_THREADING_TASKS_TASK_COMPLETEDTASK
        return Task.CompletedTask;
#else
        return Task.FromResult(0);
#endif
      else
        return WriteAsyncCore(stream, sequence, cancellationToken);
    }

    private static async Task WriteAsyncCore(
      Stream stream,
      ReadOnlySequence<byte> sequence,
      CancellationToken cancellationToken
    )
    {
      var pos = sequence.Start;
#if !NETSTANDARD2_1
      byte[] buffer = null;
#endif

      while (sequence.TryGet(ref pos, out var memory, advance: true)) {
#if NETSTANDARD2_1
        await stream.WriteAsync(memory, cancellationToken).ConfigureAwait(false);
#else
        if (buffer == null || buffer.Length < memory.Length)
          buffer = new byte[memory.Length]; // TODO: ArrayPool.Shared.Rent()

        memory.CopyTo(buffer);

        await stream.WriteAsync(buffer, 0, memory.Length, cancellationToken).ConfigureAwait(false);
#endif
      }
    }
  }
}
