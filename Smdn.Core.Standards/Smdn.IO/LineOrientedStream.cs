// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2012 smdn
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
using System.IO;

using Smdn.Formats;

namespace Smdn.IO {
  public class LineOrientedStream : Stream {
    protected static readonly int DefaultBufferSize = 1024;
    protected static readonly int MinimumBufferSize = 8;

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

    public byte[] NewLine {
      get { CheckDisposed(); return (newLine == null) ? null : (byte[])newLine.Clone(); }
    }

    public int BufferSize {
      get { CheckDisposed(); return buffer.Length; }
    }

    public virtual Stream InnerStream {
      get { CheckDisposed(); return stream; }
    }

    protected LineOrientedStream(Stream stream, byte[] newLine, bool strictEOL, int bufferSize)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");
      if (strictEOL) {
        if (newLine == null)
          throw new ArgumentNullException("newLine");
        if (newLine.Length == 0)
          throw ExceptionUtils.CreateArgumentMustBeNonEmptyArray("newLine");
      }

      if (bufferSize < MinimumBufferSize)
        throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(MinimumBufferSize, "bufferSize", bufferSize);

      this.stream = stream;
      this.strictEOL = strictEOL;
      this.newLine = newLine;
      this.buffer = new byte[bufferSize];
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing) {
        if (stream != null)
          stream.Close();
      }

      stream = null;
      newLine = null;
      buffer = null;
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

    public byte[] ReadLine()
    {
      return ReadLine(true);
    }

    public byte[] ReadLine(bool keepEOL)
    {
      CheckDisposed();

      if (bufRemain == 0 && FillBuffer() <= 0)
        // end of stream
        return null;

      var newLineOffset = 0;
      var bufCopyFrom = bufOffset;
      var eol = EolState.NotMatched;
      var eos = false;
      var retCount = 0;
      byte[] retBuffer = null;

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
          if (buffer[bufOffset] == Octets.CR) {
            eol = EolState.CR;
            newLineOffset = 1;
          }
          else if (buffer[bufOffset] == Octets.LF) {
            eol = EolState.LF;
            newLineOffset = 1;
          }
        }

        bufRemain--;
        bufOffset++;

        if (bufRemain == 0 &&
            (eol == EolState.NotMatched || eol == EolState.CR /* read ahead; CRLF */)) {
          var count = bufOffset - bufCopyFrom;

          if (retBuffer == null) {
            retBuffer = new byte[count + BufferSize];

            Buffer.BlockCopy(buffer, bufCopyFrom, retBuffer, 0, count);
          }
          else {
            var newRetBuffer = new byte[retBuffer.Length + BufferSize];

            Buffer.BlockCopy(retBuffer, 0, newRetBuffer, 0, retCount);
            Buffer.BlockCopy(buffer, bufCopyFrom, newRetBuffer, retCount, count);

            retBuffer = newRetBuffer;
          }

          retCount += count;

          eos = (FillBuffer() <= 0);

          bufCopyFrom = bufOffset;
        }

        if (eol != EolState.NotMatched || eos)
          break;
      }

      var retLength = retCount + (bufOffset - bufCopyFrom);

      if (eol == EolState.CR && buffer[bufOffset] == Octets.LF) {
        // CRLF
        retLength++;
        newLineOffset++;

        bufOffset++;
        bufRemain--;
      }

      if (!keepEOL && eol != EolState.NotMatched)
        retLength -= newLineOffset;

      if (retBuffer == null || 0 < retLength - retCount) {
        if (retBuffer == null)
          retBuffer = new byte[retLength];

        Buffer.BlockCopy(buffer, bufCopyFrom, retBuffer, retCount, retLength - retCount);
      }

      if (retLength == retBuffer.Length) {
        return retBuffer;
      }
      else {
        var ret = new byte[retLength];

        Buffer.BlockCopy(retBuffer, 0, ret, 0, retLength);

        return ret;
      }
    }

    public long Read(Stream targetStream, long length)
    {
      CheckDisposed();

      if (length < 0L)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("length", length);
      if (targetStream == null)
        throw new ArgumentNullException("targetStream");

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

    public override int Read(byte[] dest, int offset, int count)
    {
      CheckDisposed();

      if (dest == null)
        throw new ArgumentNullException("dest");
      if (offset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("offset", offset);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("count", count);
      if (dest.Length - count < offset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("offset", dest, offset, count);

      if (count <= bufRemain) {
        Buffer.BlockCopy(buffer, bufOffset, dest, offset, count);
        bufOffset += count;
        bufRemain -= count;

        return count;
      }

      var read = 0;

      if (bufRemain != 0) {
        Buffer.BlockCopy(buffer, bufOffset, dest, offset, bufRemain);

        read    = bufRemain;
        offset += bufRemain;
        count  -= bufRemain;

        bufRemain = 0;
      }

      // read from base stream
      for (;;) {
        if (count <= 0)
          break;

        var r = stream.Read(dest, offset, count);

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
    private byte[] newLine;
    private bool strictEOL;
    private byte[] buffer;
    private int bufOffset = 0;
    private int bufRemain  = 0;
  }
}
