// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_BUFFERS_READONLYSEQUENCE
using System.Buffers;
#endif
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class StreamExtensions {
#if !SYSTEM_IO_STREAM_CLOSE
  public static void Close(this Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof(stream));

    stream.Dispose();
  }
#endif

  private const int DefaultCopyBufferSize = 10 * 1024;

  public static void CopyTo(
    this Stream stream,
    BinaryWriter writer,
    int bufferSize = DefaultCopyBufferSize
  )
  {
    if (stream == null)
      throw new ArgumentNullException(nameof(stream));
    if (writer == null)
      throw new ArgumentNullException(nameof(writer));
    if (bufferSize <= 0)
      throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(bufferSize), bufferSize);

    var buffer = new byte[bufferSize]; // TODO: array pool

    for (; ; ) {
      var read = stream.Read(buffer, 0, bufferSize);

      if (read <= 0)
        break;

      writer.Write(buffer, 0, read);
    }
  }

  public static Task CopyToAsync(
    this Stream stream,
    BinaryWriter writer,
    int bufferSize = DefaultCopyBufferSize,
    CancellationToken cancellationToken = default
  )
  {
    if (stream == null)
      throw new ArgumentNullException(nameof(stream));
    if (writer == null)
      throw new ArgumentNullException(nameof(writer));
    if (bufferSize <= 0)
      throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(bufferSize), bufferSize);

    return CopyToAsyncCore();

    async Task CopyToAsyncCore()
    {
      var buffer = new byte[bufferSize]; // TODO: array pool

      for (; ; ) {
        var read =
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
          await stream.ReadAsync(buffer.AsMemory(0, bufferSize), cancellationToken)
#else
          await stream.ReadAsync(buffer, 0, bufferSize, cancellationToken)
#endif
          .ConfigureAwait(false);

        if (read <= 0)
          break;

        writer.Write(buffer, 0, read/*, cancellationToken*/);
      }
    }
  }

  private const int DefaultReadBufferSize = 4096;
  private const int DefaultInitialBufferCapacity = 4096;

  public static byte[] ReadToEnd(
    this Stream stream,
    int readBufferSize = DefaultReadBufferSize,
    int initialCapacity = DefaultInitialBufferCapacity
  )
  {
    if (stream == null)
      throw new ArgumentNullException(nameof(stream));
    if (readBufferSize <= 0)
      throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(readBufferSize), readBufferSize);
    if (initialCapacity < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(initialCapacity), initialCapacity);

    using var outStream = new MemoryStream(initialCapacity);

    stream.CopyTo(outStream, readBufferSize);

    outStream.Close();

    return outStream.ToArray();
  }

  public static Task<byte[]> ReadToEndAsync(
    this Stream stream,
    int readBufferSize = DefaultReadBufferSize,
    int initialCapacity = DefaultInitialBufferCapacity,
    CancellationToken cancellationToken = default
  )
  {
    if (stream == null)
      throw new ArgumentNullException(nameof(stream));
    if (readBufferSize <= 0)
      throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(readBufferSize), readBufferSize);
    if (initialCapacity < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(initialCapacity), initialCapacity);

    return ReadToEndAsyncCore();

    async Task<byte[]> ReadToEndAsyncCore()
    {
      using var outStream = new MemoryStream(initialCapacity);

      await stream.CopyToAsync(outStream, readBufferSize, cancellationToken).ConfigureAwait(false);

      outStream.Close();

      return outStream.ToArray();
    }
  }

  public static void Write(this Stream stream, ArraySegment<byte> segment)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof(stream));

    // TODO: allow empty segment or throw exception
    stream.Write(segment.Array!, segment.Offset, segment.Count);
  }

#if SYSTEM_BUFFERS_READONLYSEQUENCE
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
#if !SYSTEM_IO_STREAM_WRITE_READONLYSPAN_OF_BYTE
    byte[] buffer = null;
#endif

    while (sequence.TryGet(ref pos, out var memory, advance: true)) {
#if SYSTEM_IO_STREAM_WRITE_READONLYSPAN_OF_BYTE
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
#if !SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
    byte[] buffer = null;
#endif

    while (sequence.TryGet(ref pos, out var memory, advance: true)) {
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
      await stream.WriteAsync(memory, cancellationToken).ConfigureAwait(false);
#else
      if (buffer == null || buffer.Length < memory.Length)
        buffer = new byte[memory.Length]; // TODO: ArrayPool.Shared.Rent()

      memory.CopyTo(buffer);

      await stream.WriteAsync(buffer, 0, memory.Length, cancellationToken).ConfigureAwait(false);
#endif
    }
  }
#endif // SYSTEM_BUFFERS_READONLYSEQUENCE
}
