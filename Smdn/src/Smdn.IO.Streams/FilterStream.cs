// 
// Copyright (c) 2020 smdn <smdn@smdn.jp>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams {
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

    private IEnumerable<IFilter> filters;
    public IEnumerable<IFilter> Filters {
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
      this.filters = filters ?? throw new ArgumentNullException(nameof(filters));
      this.leaveStreamOpen = leaveStreamOpen;
      this.rawBuffer = new byte[
        MinimumBufferSize <= bufferSize
          ? bufferSize
          : throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(MinimumBufferSize, nameof(bufferSize), bufferSize)
      ];

      AdvanceBufferTo(stream.CanSeek ? stream.Position : 0L);
    }

    public override void Close()
    {
      if (!leaveStreamOpen)
        stream?.Close();

      stream = null;
    }

    private Task ThrowNotSupportedWritingStream() { ThrowIfDisposed(); throw ExceptionUtils.CreateNotSupportedWritingStream(); }

    public override void Flush() => ThrowNotSupportedWritingStream();
    public override Task FlushAsync(CancellationToken cancellationToken) => ThrowNotSupportedWritingStream();

    public override void Write(byte[] buffer, int offset, int count) => ThrowNotSupportedWritingStream();
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => ThrowNotSupportedWritingStream();

    public override void SetLength(long value){ ThrowIfDisposed(); throw ExceptionUtils.CreateNotSupportedSettingStreamLength(); }

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
        return Task.FromCanceled<int>(cancellationToken);
      if (count == 0)
        return Task.FromResult(count);

      var dest = buffer.AsMemory(offset, count);

      if (dest.Length <= bufferReadCursor.Length)
        return Task.FromResult(ReadBuffer(ref dest));

      return ReadAsyncCore();

      async Task<int> ReadAsyncCore()
      {
        var read = 0;

        for (; ; ) {
          if (bufferReadCursor.Length == 0) {
            if (hasReachedEndOfStream)
              return read;

            await FillBufferAsync(cancellationToken).ConfigureAwait(false);
          }

          read += ReadBuffer(ref dest);

          if (dest.Length == 0)
            return read;
        }
      }
    }

    private byte[] rawBuffer;
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
      var read = await stream.ReadAsync(rawBuffer, 0, rawBuffer.Length, cancellationToken).ConfigureAwait(false);

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
}
