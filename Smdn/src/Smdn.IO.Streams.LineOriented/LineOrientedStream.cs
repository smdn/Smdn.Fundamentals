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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Smdn.Text;

namespace Smdn.IO.Streams.LineOriented {
  public class LineOrientedStream : Stream {
    protected static readonly int DefaultBufferSize = 1024;
    protected static readonly int MinimumBufferSize = 8;
    protected static readonly bool DefaultLeaveStreamOpen = false;

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

    public IReadOnlyList<byte> NewLine {
      get { CheckDisposed(); return newLine; }
    }

    public int BufferSize {
      get { CheckDisposed(); return buffer.Length; }
    }

    public virtual Stream InnerStream {
      get { CheckDisposed(); return stream; }
    }

    protected LineOrientedStream(Stream stream, byte[] newLine, bool strictEOL, int bufferSize, bool leaveStreamOpen)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));
      if (strictEOL) {
        if (newLine == null)
          throw new ArgumentNullException(nameof(newLine));
        if (newLine.Length == 0)
          throw ExceptionUtils.CreateArgumentMustBeNonEmptyArray(nameof(newLine));
      }

      if (bufferSize < MinimumBufferSize)
        throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(MinimumBufferSize, nameof(bufferSize), bufferSize);

      this.stream = stream;
      this.strictEOL = strictEOL;
      this.newLine = newLine;
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

    public override int ReadByte()
    {
      CheckDisposed();

      if (bufRemain == 0 && FillBuffer() <= 0)
        return -1;

      bufRemain--;

      return buffer[bufOffset++];
    }

    public byte[] ReadLine(bool keepEOL = true)
      => ReadLineAsync(keepEOL: keepEOL).GetAwaiter().GetResult()?.ToArray();

    public async Task<ReadOnlySequence<byte>?> ReadLineAsync(
      bool keepEOL = true,
      CancellationToken cancellationToken = default
    )
    {
      CheckDisposed();

      if (bufRemain == 0 && await FillBufferAsync(cancellationToken).ConfigureAwait(false) <= 0)
        // end of stream
        return null;

      var newLineOffset = 0;
      var bufCopyFrom = bufOffset;
      var eol = EolState.NotMatched;
      var eos = false;
      var retCount = 0;
      LineSequenceSegment segmentHead = null;
      LineSequenceSegment segmentTail = null;

      for (;;) {
        if (strictEOL) {
          if (buffer[bufOffset] == newLine[newLineOffset]) {
            if (newLine.Length == ++newLineOffset)
              eol = EolState.NewLine;
          }
          else {
            newLineOffset = 0;
          }
        }
        else {
          if (buffer[bufOffset] == Ascii.Octets.CR) {
            eol = EolState.CR;
            newLineOffset = 1;
          }
          else if (buffer[bufOffset] == Ascii.Octets.LF) {
            eol = EolState.LF;
            newLineOffset = 1;
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
        newLineOffset++;

        bufOffset++;
        bufRemain--;
      }

      if (!keepEOL && eol != EolState.NotMatched)
        retLength -= newLineOffset;

      if (segmentHead == null || 0 < retLength - retCount) {
        segmentTail = new LineSequenceSegment(segmentTail, buffer.AsSpan(bufCopyFrom, retLength - retCount).ToArray()); // XXX
        segmentHead = segmentHead ?? segmentTail;
      }

      return new ReadOnlySequence<byte>(segmentHead, 0, segmentTail, segmentTail.Memory.Length).Slice(0, retLength);
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
    {
      CheckDisposed();

      if (length < 0L)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(length), length);
      if (targetStream == null)
        throw new ArgumentNullException(nameof(targetStream));

      if (length <= bufRemain) {
        var count = (int)length;

        targetStream.Write(buffer, bufOffset, count);

        bufOffset += count;
        bufRemain -= count;

        return count;
      }

      var read = 0L;

      if (bufRemain != 0) {
        targetStream.Write(buffer, bufOffset, bufRemain);

        read    = bufRemain;
        length -= bufRemain;

        bufRemain = 0;
      }

      // read from base stream
      for (;;) {
        var r = stream.Read(buffer, 0, (int)Math.Min(length, buffer.Length));

        if (r <= 0)
          break;

        targetStream.Write(buffer, 0, r);

        length -= r;
        read   += r;

        if (length <= 0)
          break;
      }

      return read;
    }

    public override int Read(byte[] buffer, int offset, int count)
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

      if (count <= bufRemain) {
        Buffer.BlockCopy(this.buffer, bufOffset, buffer, offset, count);
        bufOffset += count;
        bufRemain -= count;

        return count;
      }

      var read = 0;

      if (bufRemain != 0) {
        Buffer.BlockCopy(this.buffer, bufOffset, buffer, offset, bufRemain);

        read    = bufRemain;
        offset += bufRemain;
        count  -= bufRemain;

        bufRemain = 0;
      }

      // read from base stream
      for (;;) {
        if (count <= 0)
          break;

        var r = stream.Read(buffer, offset, count);

        if (r <= 0)
          break;

        offset += r;
        count  -= r;
        read   += r;
      }

      return read;
    }

    private int FillBuffer()
    {
      bufOffset = 0;
      bufRemain = stream.Read(buffer, 0, buffer.Length);

      return bufRemain;
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

    private void CheckDisposed()
    {
      if (IsClosed)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private Stream stream;
    private readonly byte[] newLine;
    private readonly bool strictEOL;
    private readonly bool leaveStreamOpen;
    private byte[] buffer;
    private int bufOffset = 0;
    private int bufRemain  = 0;
  }
}
