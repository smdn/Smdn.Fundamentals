// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.Filtering;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public partial class FilterStream : Stream {
  private Stream stream = null;
  private bool IsClosed => stream == null;
  public override bool CanSeek => !IsClosed && stream.CanSeek;
  public override bool CanRead => !IsClosed && stream.CanRead;
  public override bool CanWrite => /*!IsClosed &&*/ false;
  public override bool CanTimeout => !IsClosed && stream.CanTimeout;
  public override long Length { get { ThrowIfDisposed(); return stream.Length; } }
  public override long Position {
    get { ThrowIfDisposed(); return offset; }
    set { ThrowIfDisposed(); AdvanceBufferTo(stream.Position = value); }
  }

  protected void ThrowIfDisposed()
  {
    if (stream == null)
      throw new ObjectDisposedException(GetType().FullName);
  }

  private readonly IReadOnlyList<IFilter> filters;
  public IReadOnlyList<IFilter> Filters {
    get => filters;
    protected set => throw new NotImplementedException();
  }

  private readonly bool leaveStreamOpen;
  protected const bool DefaultLeaveStreamOpen = false;

  protected const int DefaultBufferSize = 1024;
  protected const int MinimumBufferSize = 2;

  public FilterStream(
    Stream stream,
    IFilter filter,
    int bufferSize = DefaultBufferSize,
    bool leaveStreamOpen = DefaultLeaveStreamOpen
  )
    : this(
      stream,
      Enumerable.Repeat(filter ?? throw new ArgumentNullException(nameof(filter)), 1),
      bufferSize,
      leaveStreamOpen
    )
  {
  }

  public FilterStream(
    Stream stream,
    IEnumerable<IFilter> filters,
    int bufferSize = DefaultBufferSize,
    bool leaveStreamOpen = DefaultLeaveStreamOpen
  )
  {
    this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
    this.filters = filters.Where(f => f is not NullFilterImpl).ToList() ?? throw new ArgumentNullException(nameof(filters));
    this.leaveStreamOpen = leaveStreamOpen;
    this.rawBuffer = new byte[
      MinimumBufferSize <= bufferSize
        ? bufferSize
        : throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(MinimumBufferSize, nameof(bufferSize), bufferSize)
    ];

    AdvanceBufferTo(stream.CanSeek ? stream.Position : 0L);
  }

#if SYSTEM_IO_STREAM_CLOSE
  public override void Close()
#else
  protected override void Dispose(bool disposing)
#endif
  {
    if (!leaveStreamOpen)
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

  private Task ThrowNotSupportedWritingStream() { ThrowIfDisposed(); throw ExceptionUtils.CreateNotSupportedWritingStream(); }
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
  private ValueTask ThrowNotSupportedWritingStreamValueTask() { ThrowIfDisposed(); throw ExceptionUtils.CreateNotSupportedWritingStream(); }
#endif

  public override void Flush() => ThrowNotSupportedWritingStream();
  public override Task FlushAsync(CancellationToken cancellationToken) => ThrowNotSupportedWritingStream();

  public override void Write(byte[] buffer, int offset, int count) => ThrowNotSupportedWritingStream();
  public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => ThrowNotSupportedWritingStream();
#if SYSTEM_IO_STREAM_WRITEASYNC_READONLYMEMORY_OF_BYTE
  public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => ThrowNotSupportedWritingStreamValueTask();
#endif
  public override void SetLength(long value) { ThrowIfDisposed(); throw ExceptionUtils.CreateNotSupportedSettingStreamLength(); }

  public override long Seek(long offset, SeekOrigin origin)
  {
    ThrowIfDisposed();

    return AdvanceBufferTo(stream.Seek(offset, origin));
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    ThrowIfDisposed();

    if (buffer == null)
      throw new ArgumentNullException(nameof(buffer));
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
    if (offset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);
    if (buffer.Length < count + offset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(offset), buffer, offset, count);

    return ReadUnchecked(buffer, offset, count);
  }

  protected virtual int ReadUnchecked(byte[] buffer, int offset, int count)
  {
    var read = 0;
    var dest = buffer.AsMemory(offset, count);

    for (; ; ) {
      if (bufferReadCursor.Length == 0) {
        if (hasReachedEndOfStream)
          return read;

        FillBuffer();
      }

      read += ReadBuffer(ref dest);

      if (dest.Length == 0)
        return read;
    }
  }

  public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
  {
    ThrowIfDisposed();

    if (buffer == null)
      throw new ArgumentNullException(nameof(buffer));
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
    if (offset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);
    if (buffer.Length < count + offset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(offset), buffer, offset, count);

    if (cancellationToken.IsCancellationRequested)
#if SYSTEM_THREADING_TASKS_TASK_FROMCANCELED
      return Task.FromCanceled<int>(cancellationToken);
#else
      return new Task<int>(() => default, cancellationToken);
#endif
    if (count == 0)
      return Task.FromResult(count);

    return ReadAsyncUnchecked(
      buffer.AsMemory(offset, count),
      cancellationToken
#if SYSTEM_THREADING_TASKS_VALUETASK
    ).AsTask();
#else
    );
#endif
  }

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
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

    return ReadAsyncUnchecked(buffer, cancellationToken);
  }
#endif

  protected virtual
#if SYSTEM_THREADING_TASKS_VALUETASK
  ValueTask<int>
#else
  Task<int>
#endif
  ReadAsyncUnchecked(
    Memory<byte> destination,
    CancellationToken cancellationToken
  )
  {
    if (destination.Length <= bufferReadCursor.Length)
#if SYSTEM_THREADING_TASKS_VALUETASK
      return new(ReadBuffer(ref destination));
#else
      return Task.FromResult(ReadBuffer(ref destination));
#endif

    return ReadAsyncCore();

    async
#if SYSTEM_THREADING_TASKS_VALUETASK
    ValueTask<int>
#else
    Task<int>
#endif
    ReadAsyncCore()
    {
      var read = 0;

      for (; ; ) {
        if (bufferReadCursor.Length == 0) {
          if (hasReachedEndOfStream)
            return read;

          await FillBufferAsync(cancellationToken).ConfigureAwait(false);
        }

        read += ReadBuffer(ref destination);

        if (destination.IsEmpty)
          return read;
      }
    }
  }

  private readonly byte[] rawBuffer;
  private ReadOnlyMemory<byte> bufferReadCursor = ReadOnlyMemory<byte>.Empty;
  private long offset;
  private bool hasReachedEndOfStream;

  private int ReadBuffer(ref Memory<byte> dest)
  {
    var count = Math.Min(dest.Length, bufferReadCursor.Length);

    bufferReadCursor.Slice(0, count).CopyTo(dest);
    offset += count;

    dest = dest.Slice(count);
    bufferReadCursor = bufferReadCursor.Slice(count);

    return count;
  }

  private long AdvanceBufferTo(long newOffset)
  {
    var advance = newOffset - offset;

    if (0 <= advance && advance < bufferReadCursor.Length)
      bufferReadCursor = bufferReadCursor.Slice((int)advance); // advance read cursor
    else
      bufferReadCursor = ReadOnlyMemory<byte>.Empty; // discard read cursor

    offset = newOffset;
    hasReachedEndOfStream = false;

    return offset;
  }

  private void FillBuffer()
  {
    var read = stream.Read(rawBuffer, 0, rawBuffer.Length);

    hasReachedEndOfStream |= read < rawBuffer.Length;

    bufferReadCursor = FilterBuffer(rawBuffer.AsMemory(0, read));
  }

  private async Task FillBufferAsync(CancellationToken cancellationToken)
  {
    var read =
#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
      await stream.ReadAsync(
        rawBuffer.AsMemory(),
#else
      await stream.ReadAsync(
        rawBuffer,
        0,
        rawBuffer.Length,
#endif
        cancellationToken
      ).ConfigureAwait(false);

    hasReachedEndOfStream |= read < rawBuffer.Length;

    bufferReadCursor = FilterBuffer(rawBuffer.AsMemory(0, read));
  }

  private Memory<byte> FilterBuffer(Memory<byte> buffer)
  {
    foreach (var filter in Filters) {
      if (filter.Offset + filter.Length < offset)
        continue;
      if (offset + buffer.Length < filter.Offset)
        continue;

      var relativeOffset = filter.Offset - offset;
      var length = relativeOffset < 0L ? filter.Length + relativeOffset : filter.Length;

      if (length == 0L)
        continue;

      var span = buffer.Span.Slice(Math.Max(0, (int)relativeOffset));

      if (length < span.Length)
        span = span.Slice(0, (int)length);

      filter.Apply(span, Math.Max(0, -relativeOffset));
    }

    return buffer;
  }
}
