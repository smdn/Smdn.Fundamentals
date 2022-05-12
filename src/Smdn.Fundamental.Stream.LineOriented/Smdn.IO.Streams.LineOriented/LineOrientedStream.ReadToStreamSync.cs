// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO.Streams.LineOriented;

#pragma warning disable IDE0040
partial class LineOrientedStream {
#pragma warning restore IDE0040
  private static void ValidateReadToStreamArguments(Stream targetStream, long length)
  {
    if (targetStream is null)
      throw new ArgumentNullException(nameof(targetStream));
    if (length < 0L)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(length), length);
  }

  public long Read(Stream targetStream, long length)
  {
    CheckDisposed();

    ValidateReadToStreamArguments(targetStream, length);

    if (length == 0L)
      return 0L; // do nothing

    return ReadToStream(
      targetStream,
      length
    );
  }

  private long ReadToStream(
    Stream destination,
    long? bytesToRead
  )
  {
    var read = ReadToStreamFromBuffer(
      destination,
      bytesToRead
    );

    if (bytesToRead.HasValue) {
      var bytesToReadFromUnderlyingStream = bytesToRead.Value - read;

      if (bytesToReadFromUnderlyingStream == 0L)
        return read;

      return read + ReadToStreamCopyUnderlyingStream(
        bytesToRead: bytesToReadFromUnderlyingStream,
        destination: destination
      );
    }
    else {
      return read + ReadToStreamCopyEntireUnderlyingStream(
        destination: destination
      );
    }
  }

  private long ReadToStreamFromBuffer(
    Stream destination,
    long? bytesToRead
  )
  {
    if (bufRemain == 0)
      return 0;

    var bytesToReadFromBuffer = bytesToRead.HasValue
      ? (int)Math.Min(bufRemain, bytesToRead.Value)
      : bufRemain;

    destination.Write(buffer, bufOffset, bytesToReadFromBuffer);

    bufRemain -= bytesToReadFromBuffer;
    bufOffset += bytesToReadFromBuffer;

    return bytesToReadFromBuffer;
  }

  private long ReadToStreamCopyEntireUnderlyingStream(
    Stream destination
  )
  {
#if DEBUG
    if (0 < bufRemain)
      throw new InvalidOperationException($"call {nameof(ReadToStreamFromBufferAsync)} first");
#endif

    var read = 0L;

    for (; ; ) {
      var r = stream.Read(
#pragma warning disable SA1114
#if SYSTEM_IO_STREAM_READ_SPAN_OF_BYTE
        buffer.AsSpan()
#else
        buffer, 0, buffer.Length
#endif
#pragma warning restore SA1114
      );

      destination.Write(
#pragma warning disable SA1114
#if SYSTEM_IO_STREAM_WRITE_READONLYSPAN_OF_BYTE
        buffer.AsSpan(0, r)
#else
        buffer, 0, r
#endif
#pragma warning restore SA1114
      );

      read += r;

      if (r < buffer.Length)
        break;
    }

    return read;
  }

  private long ReadToStreamCopyUnderlyingStream(
    Stream destination,
    long bytesToRead
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
      var r = stream.Read(
#pragma warning disable SA1114
#if SYSTEM_IO_STREAM_READ_SPAN_OF_BYTE
        buffer.AsSpan(0, bytesToReadFromUnderlyingStream)
#else
        buffer, 0, bytesToReadFromUnderlyingStream
#endif
#pragma warning restore SA1114
      );

      if (r <= 0)
        break;

      destination.Write(
#pragma warning disable SA1114
#if SYSTEM_IO_STREAM_WRITE_READONLYSPAN_OF_BYTE
        buffer.AsSpan(0, r)
#else
        buffer, 0, r
#endif
#pragma warning restore CA1835
      );

      bytesToRead -= r;
      read += r;
    }

    return read;
  }
}
