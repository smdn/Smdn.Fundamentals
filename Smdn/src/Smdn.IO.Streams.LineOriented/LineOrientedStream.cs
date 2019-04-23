// 
// Copyright (c) 2009 smdn <smdn@smdn.jp>
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
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Smdn.Text;

namespace Smdn.IO.Streams.LineOriented {
  public class LineOrientedStream : Stream {
    protected const int DefaultBufferSize = 1024;
    protected const int MinimumBufferSize = 8;
    protected const bool DefaultLeaveStreamOpen = false;

    private enum EolState {
      NotMatched = 0,
      NewLine,
      CR,
      LF
    }

    public override bool CanSeek {
      get { return !IsClosed && stream.CanSeek; }
    }

    public override bool CanRead {
      get { return !IsClosed && stream.CanRead; }
    }

    public override bool CanWrite {
      get { return !IsClosed && stream.CanWrite; }
    }

    public override bool CanTimeout {
      get { return !IsClosed && stream.CanTimeout; }
    }

    private bool IsClosed {
      get { return stream == null; }
    }

    public override long Position {
      get { CheckDisposed(); return stream.Position - bufRemain; }
      set { Seek(value, SeekOrigin.Begin); }
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
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));
      if (bufferSize < MinimumBufferSize)
        throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(MinimumBufferSize, nameof(bufferSize), bufferSize);

      this.stream = stream;
      this.newLine = newLine.IsEmpty ? null : newLine.ToArray(); // XXX: allocation
      this.buffer = new byte[bufferSize];
      this.leaveStreamOpen = leaveStreamOpen;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing) {
        if (!leaveStreamOpen)
          stream?.Close();
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

    public byte[] ReadLine(bool keepEOL = true)
      => ReadLineAsync().GetAwaiter().GetResult().GetLine(keepEOL)?.ToArray();

    public readonly struct ReadLineResult {
      internal static readonly ReadLineResult EndOfStream = default;

      private readonly ReadOnlySequence<byte>? lineWithNewLine;
      public long LengthOfNewLine { get; }

      public bool IsEndOfStream => lineWithNewLine == null;
      public ReadOnlySequence<byte> LineWithNewLine => lineWithNewLine ?? throw CreateEOSException();
      public ReadOnlySequence<byte> Line => lineWithNewLine?.Slice(0, lineWithNewLine.Value.Length - LengthOfNewLine) ?? throw CreateEOSException();
      public bool IsEmptyLine => lineWithNewLine.HasValue ? lineWithNewLine.Value.Length == LengthOfNewLine : throw CreateEOSException();

      private static Exception CreateEOSException() => new InvalidOperationException("has reached to end of the stream");

      internal ReadLineResult(ReadOnlySequence<byte>? lineWithNewLine, int lengthOfNewLine)
      {
        this.lineWithNewLine = lineWithNewLine;
        LengthOfNewLine = lengthOfNewLine;
      }

      internal ReadOnlySequence<byte>? GetLine(bool keepEOL)
      {
        if (IsEndOfStream)
          return null;

        return keepEOL ? LineWithNewLine : Line;
      }
    }

    public Task<ReadLineResult> ReadLineAsync(CancellationToken cancellationToken = default)
    {
      CheckDisposed();

      return ReadLineAsyncCore(cancellationToken: cancellationToken);
    }

    private async Task<ReadLineResult> ReadLineAsyncCore(CancellationToken cancellationToken = default)
    {
      if (bufRemain == 0 && await FillBufferAsync(cancellationToken).ConfigureAwait(false) <= 0)
        return ReadLineResult.EndOfStream;

      var newLineLength = 0;
      var bufCopyFrom = bufOffset;
      var eol = EolState.NotMatched;
      var eos = false;
      var retCount = 0;
      LineSequenceSegment segmentHead = null;
      LineSequenceSegment segmentTail = null;

      for (;;) {
        if (newLine == null) {
          // loose EOL (CR/LF/CRLF)
          if (buffer[bufOffset] == Ascii.Octets.CR) {
            eol = EolState.CR;
            newLineLength = 1;
          }
          else if (buffer[bufOffset] == Ascii.Octets.LF) {
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
          segmentHead = segmentHead ?? segmentTail;

          retCount += count;

          eos = (await FillBufferAsync(cancellationToken).ConfigureAwait(false) <= 0);

          bufCopyFrom = bufOffset;
        }

        if (eol != EolState.NotMatched || eos)
          break;
      }

      var retLength = retCount + (bufOffset - bufCopyFrom);

      if (eol == EolState.CR && buffer[bufOffset] == Ascii.Octets.LF) {
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
        segmentHead = segmentHead ?? segmentTail;
      }

      return new ReadLineResult(
        lineWithNewLine: new ReadOnlySequence<byte>(segmentHead, 0, segmentTail, segmentTail.Memory.Length).Slice(0, retLength),
        lengthOfNewLine: newLineLength
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
#if NET45
        return new Task<long>(() => default, cancellationToken);
#else
        return Task.FromCanceled<long>(cancellationToken);
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

        await targetStream.WriteAsync(buffer, bufOffset, count, cancellationToken).ConfigureAwait(false);

        bufOffset += count;
        bufRemain -= count;

        return count;
      }

      var read = 0L;

      if (0 < bufRemain) {
        await targetStream.WriteAsync(buffer, bufOffset, bufRemain, cancellationToken).ConfigureAwait(false);

        read         = bufRemain;
        bytesToRead -= bufRemain;

        bufRemain = 0;
      }

      // read from base stream
      if (bytesToRead.HasValue) {
        for (;;) {
          if (bytesToRead <= 0)
            break;

          var r = await stream.ReadAsync(buffer, 0, (int)Math.Min(bytesToRead.Value, buffer.Length)).ConfigureAwait(false);

          if (r <= 0)
            break;

          await targetStream.WriteAsync(buffer, 0, r).ConfigureAwait(false);

          bytesToRead -= r;
          read        += r;
        }
      }
      else {
        for (;;) {
          var r = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

          await targetStream.WriteAsync(buffer, 0, r).ConfigureAwait(false);

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
#if NET45
        return new Task<int>(() => default, cancellationToken);
#else
        return Task.FromCanceled<int>(cancellationToken);
#endif
      if (count == 0L)
        return Task.FromResult(0); // do nothing

      return ReadAsyncCore(
        destination: buffer,
        offset: offset,
        count: count,
        cancellationToken: cancellationToken
      );
    }

    private async Task<int> ReadAsyncCore(
      byte[] destination,
      int offset,
      int count,
      CancellationToken cancellationToken
    )
    {
      if (count <= bufRemain) {
        Buffer.BlockCopy(buffer, bufOffset, destination, offset, count);
        bufOffset += count;
        bufRemain -= count;

        return count;
      }

      var read = 0;

      if (bufRemain != 0) {
        Buffer.BlockCopy(buffer, bufOffset, destination, offset, bufRemain);

        read    = bufRemain;
        offset += bufRemain;
        count  -= bufRemain;

        bufRemain = 0;
      }

      // read from base stream
      for (;;) {
        if (count <= 0)
          break;

        var r = await stream.ReadAsync(destination, offset, count, cancellationToken).ConfigureAwait(false);

        if (r <= 0)
          break;

        offset += r;
        count  -= r;
        read   += r;
      }

      return read;
    }

    private async Task<int> FillBufferAsync(CancellationToken cancellationToken)
    {
      bufOffset = 0;
      bufRemain = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);

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
}
