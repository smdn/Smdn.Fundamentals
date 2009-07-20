// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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

using Smdn.Text;

namespace Smdn.IO {
  public class LineOrientedStream : Stream {
    protected const int DefaultBufferSize = 1024;
    protected const int MinimumBufferSize = 8;

    public override bool CanSeek {
      get { return stream.CanSeek; }
    }

    public override bool CanRead {
      get { return stream.CanWrite; }
    }

    public override bool CanWrite {
      get { return stream.CanRead; }
    }

    public override bool CanTimeout {
      get { return stream.CanTimeout; }
    }

    public override long Position {
      get { return stream.Position; }
      set { Seek(value, SeekOrigin.Begin); }
    }

    public override long Length {
      get { return stream.Length; }
    }

    public byte[] NewLine {
      get { return newLine; }
    }

    public int BufferSize {
      get { return buffer.Length; }
    }

    protected Stream InnerStream {
      get { return stream; }
    }

    protected LineOrientedStream(Stream stream, byte[] newLine, bool strictEOL, int bufferSize)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");
      if (strictEOL) {
        if (newLine == null)
          throw new ArgumentNullException("newLine");
        if (newLine.Length == 0)
          throw new ArgumentException("newLine", "must be non-zero positive length");
      }
      if (bufferSize < MinimumBufferSize)
        throw new ArgumentOutOfRangeException("bufferSize", string.Format("must be greater than or equals to {0}", MinimumBufferSize));

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
      var retBuffer = new byte[bufRemain];
      var retOffset = 0;
      var bufCopyFrom = bufOffset;
      var eol = false;
      var eos = false;

      for (;;) {
        if (strictEOL) {
          if (buffer[bufOffset] == newLine[newLineOffset])
            eol = (newLine.Length == ++newLineOffset);
          else
            newLineOffset = 0;
        }
        else {
          if (buffer[bufOffset] == Octets.CR || buffer[bufOffset] == Octets.LF) {
            eol = true;
            newLineOffset = 1;
          }
        }

        bufRemain--;
        bufOffset++;

        if (!eol && bufRemain == 0) {
          var newRetBuffer = new byte[retBuffer.Length + BufferSize];
          var count = bufOffset - bufCopyFrom;

          Buffer.BlockCopy(retBuffer, 0, newRetBuffer, 0, retOffset);
          Buffer.BlockCopy(buffer, bufCopyFrom, newRetBuffer, retOffset, count);

          retBuffer = newRetBuffer;
          retOffset += count;

          eos = (FillBuffer() <= 0);

          bufCopyFrom = bufOffset;
        }

        if (eol || eos) {
          var retLength = retOffset + (bufOffset - bufCopyFrom);

          if (eol && !strictEOL) {
            var crlf = (bufOffset == 0)
              ? retBuffer[retOffset - 1] == Octets.CR && buffer[bufOffset] == Octets.LF
              :    buffer[bufOffset - 1] == Octets.CR && buffer[bufOffset] == Octets.LF;

            if (crlf) {
              retLength++;
              newLineOffset++;

              bufOffset++;
              bufRemain--;
            }
          }

          if (!keepEOL)
            retLength -= newLineOffset;

          if (0 < retLength - retOffset)
            Buffer.BlockCopy(buffer, bufCopyFrom, retBuffer, retOffset, retLength - retOffset);

          if (retLength == retOffset) {
            return retBuffer;
          }
          else {
            var ret = new byte[retLength];

            Buffer.BlockCopy(retBuffer, 0, ret, 0, retLength);

            return ret;
          }
        }
      }
    }

    public override int Read(byte[] dest, int offset, int count)
    {
      CheckDisposed();

      if (dest == null)
        throw new ArgumentNullException("dest");
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset", "must be greater than or equals to 0");
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", "must be greater than or equals to 0");
      if (dest.Length < offset + count)
        throw new ArgumentException("invalid range");

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

      if (buffer.Length <= count) {
        // read from base stream
        read += stream.Read(dest, offset, count);
      }
      else {
        FillBuffer();

        if (count <= bufRemain) {
          Buffer.BlockCopy(buffer, 0, dest, offset, count);

          read += count;

          bufOffset += count;
          bufRemain -= count;
        }
        else {
          Buffer.BlockCopy(buffer, 0, dest, offset, bufRemain);

          read += bufRemain;

          bufRemain = 0;
        }
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
      if (stream == null)
        throw new ObjectDisposedException(GetType().Name);
    }

    private Stream stream;
    private byte[] newLine;
    private bool strictEOL;
    private byte[] buffer;
    private int bufOffset = 0;
    private int bufRemain  = 0;
  }
}
