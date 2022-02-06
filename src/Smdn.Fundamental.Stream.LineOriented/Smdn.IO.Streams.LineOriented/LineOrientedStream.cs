// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.LineOriented;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class LineOrientedStream : Stream {
  protected const int DefaultBufferSize = 1024;
  protected const int MinimumBufferSize = 8;
  protected const bool DefaultLeaveStreamOpen = false;

  private enum EolState {
    NotMatched = 0,
    NewLine,
    CR,
    LF,
  }

  public override bool CanSeek => !IsClosed && stream.CanSeek;
  public override bool CanRead => !IsClosed && stream.CanRead;
  public override bool CanWrite => !IsClosed && stream.CanWrite;
  public override bool CanTimeout => !IsClosed && stream.CanTimeout;
  private bool IsClosed => stream is null;

  public override long Position {
    get { CheckDisposed(); return stream.Position - bufRemain; }
    set => Seek(value, SeekOrigin.Begin);
  }

  public override long Length {
    get { CheckDisposed(); return stream.Length; }
  }

  public ReadOnlySpan<byte> NewLine {
    get { CheckDisposed(); return newLine; }
  }

  public bool IsStrictNewLine {
    get { CheckDisposed(); return newLine != null; }
  }

  public int BufferSize {
    get { CheckDisposed(); return buffer.Length; }
  }

  public virtual Stream InnerStream {
    get { CheckDisposed(); return stream; }
  }

  public LineOrientedStream(
    Stream stream,
    ReadOnlySpan<byte> newLine,
    int bufferSize = DefaultBufferSize,
    bool leaveStreamOpen = DefaultLeaveStreamOpen
  )
  {
    if (bufferSize < MinimumBufferSize)
      throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(MinimumBufferSize, nameof(bufferSize), bufferSize);

    this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
    this.newLine = newLine.IsEmpty ? null : newLine.ToArray(); // XXX: allocation
    this.buffer = new byte[bufferSize];
    this.leaveStreamOpen = leaveStreamOpen;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing) {
      if (!leaveStreamOpen)
#if SYSTEM_IO_STREAM_CLOSE
        stream?.Close();
#else
        stream?.Dispose();
#endif
    }

    stream = null;
    buffer = null;

    base.Dispose(disposing);
  }

  public override void SetLength(long @value)
  {
    CheckDisposed();

    bufRemain = 0; // discard buffered

    stream.SetLength(@value);
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    CheckDisposed();

    bufRemain = 0; // discard buffered

    return stream.Seek(offset, origin);
  }

  public override void Flush()
  {
    CheckDisposed();

    stream.Flush();
  }

  public override Task FlushAsync(CancellationToken cancellationToken)
  {
    CheckDisposed();

    return stream.FlushAsync(cancellationToken);
  }

  public override int ReadByte()
  {
    CheckDisposed();

    if (bufRemain == 0 && FillBufferAsync(default).GetAwaiter().GetResult() <= 0)
      return -1;

    bufRemain--;

    return buffer[bufOffset++];
  }

  public Line? ReadLine()
    => ReadLineAsync().GetAwaiter().GetResult();

  public byte[] ReadLine(bool keepEOL)
    => ReadLine()?.GetLine(keepEOL)?.ToArray();

  public readonly struct Line {
    public ReadOnlySequence<byte> SequenceWithNewLine { get; }
    public SequencePosition PositionOfNewLine { get; }

    public ReadOnlySequence<byte> Sequence => SequenceWithNewLine.Slice(0, PositionOfNewLine);
    public ReadOnlySequence<byte> NewLine => SequenceWithNewLine.Slice(PositionOfNewLine);
    public bool IsEmpty => SequenceWithNewLine.IsEmpty || PositionOfNewLine.Equals(SequenceWithNewLine.Start);

    public Line(ReadOnlySequence<byte> sequenceWithNewLine, SequencePosition positionOfNewLine)
    {
      SequenceWithNewLine = sequenceWithNewLine;
      PositionOfNewLine = positionOfNewLine;
    }

    internal ReadOnlySequence<byte>? GetLine(bool keepEOL) => keepEOL ? SequenceWithNewLine : Sequence;
  }

  public Task<Line?> ReadLineAsync(CancellationToken cancellationToken = default)
  {
    CheckDisposed();

    return ReadLineAsyncCore(cancellationToken: cancellationToken);
  }

  private async Task<Line?> ReadLineAsyncCore(CancellationToken cancellationToken = default)
  {
    if (bufRemain == 0 && await FillBufferAsync(cancellationToken).ConfigureAwait(false) <= 0)
      return null;

    const byte CR = 0x0d;
    const byte LF = 0x0a;
    var newLineLength = 0;
    var bufCopyFrom = bufOffset;
    var eol = EolState.NotMatched;
    var eos = false;
    var retCount = 0;
    LineSequenceSegment segmentHead = null;
    LineSequenceSegment segmentTail = null;

    for (; ; ) {
      if (newLine == null) {
        // loose EOL (CR/LF/CRLF)
        if (buffer[bufOffset] == CR) {
          eol = EolState.CR;
          newLineLength = 1;
        }
        else if (buffer[bufOffset] == LF) {
          eol = EolState.LF;
          newLineLength = 1;
        }
      }
      else {
        // strict EOL
        if (buffer[bufOffset] == newLine[newLineLength]) {
          if (newLine.Length == ++newLineLength)
            eol = EolState.NewLine;
        }
        else {
          newLineLength = 0;
        }
      }

      bufRemain--;
      bufOffset++;

      if (bufRemain == 0 &&
          (eol == EolState.NotMatched || eol == EolState.CR /* read ahead; CRLF */)) {
        var count = bufOffset - bufCopyFrom;

        segmentTail = new LineSequenceSegment(segmentTail, buffer.AsSpan(bufCopyFrom, count).ToArray()); // XXX
        segmentHead ??= segmentTail;

        retCount += count;

        eos = await FillBufferAsync(cancellationToken).ConfigureAwait(false) <= 0;

        bufCopyFrom = bufOffset;
      }

      if (eol != EolState.NotMatched || eos)
        break;
    }

    var retLength = retCount + (bufOffset - bufCopyFrom);

    if (eol == EolState.CR && buffer[bufOffset] == LF) {
      // CRLF
      retLength++;
      newLineLength++;

      bufOffset++;
      bufRemain--;
    }

    if (eol == EolState.NotMatched)
      newLineLength = 0;

    if (segmentHead == null || 0 < retLength - retCount) {
      segmentTail = new LineSequenceSegment(segmentTail, buffer.AsSpan(bufCopyFrom, retLength - retCount).ToArray()); // XXX
      segmentHead ??= segmentTail;
    }

    var sequenceWithNewLine = new ReadOnlySequence<byte>(segmentHead, 0, segmentTail, segmentTail.Memory.Length).Slice(0, retLength);

    return new Line(
      sequenceWithNewLine: sequenceWithNewLine,
      positionOfNewLine: sequenceWithNewLine.GetPosition(retLength - newLineLength)
    );
  }

  private class LineSequenceSegment : ReadOnlySequenceSegment<byte> {
    public LineSequenceSegment(LineSequenceSegment prev, ReadOnlyMemory<byte> memory)
    {
      Memory = memory;

      if (prev == null) {
        RunningIndex = 0;
      }
      else {
        RunningIndex = prev.RunningIndex + prev.Memory.Length;
        prev.Next = this;
      }
    }
  }

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

    if (buffer == null)
      throw new ArgumentNullException(nameof(buffer));
    if (offset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
    if (buffer.Length - count < offset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(offset), buffer, offset, count);

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

  private async Task<int> FillBufferAsync(CancellationToken cancellationToken)
  {
    bufOffset = 0;

    bufRemain =
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

    return bufRemain;
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    CheckDisposed();

    stream.Write(buffer, offset, count);
  }

  public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
  {
    CheckDisposed();

    return stream.WriteAsync(buffer, offset, count, cancellationToken);
  }

#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
  public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
  {
    CheckDisposed();

    return stream.WriteAsync(buffer, cancellationToken);
  }
#endif

  public override Task CopyToAsync(
    Stream destination,
    int bufferSize = 0, // don't care
    CancellationToken cancellationToken = default
  )
  {
    CheckDisposed();

    return ReadAsyncCore(
      destination ?? throw new ArgumentNullException(nameof(destination)),
      null, // read to end
      cancellationToken
    );
  }

  private void CheckDisposed()
  {
    if (IsClosed)
      throw new ObjectDisposedException(GetType().FullName);
  }

  private Stream stream;
  private readonly byte[] newLine;
  private readonly bool leaveStreamOpen;
  private byte[] buffer;
  private int bufOffset = 0;
  private int bufRemain  = 0;
}
