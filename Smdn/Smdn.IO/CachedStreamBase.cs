// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2010 smdn
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

namespace Smdn.IO {
  public abstract class CachedStreamBase : Stream {
    public Stream InnerStream {
      get { CheckDisposed(); return stream; }
    }

    public override bool CanSeek {
      get { CheckDisposed(); return true; }
    }

    public override bool CanRead {
      get { CheckDisposed(); return true; }
    }

    public override bool CanWrite {
      get { CheckDisposed(); return false; }
    }

    public override bool CanTimeout {
      get { CheckDisposed(); return false; }
    }

    public override long Length {
      get { CheckDisposed(); return stream.Length; }
    }

    public override long Position {
      get { CheckDisposed(); return position; }
      set
      {
        CheckDisposed();

        if (value < 0)
          throw new ArgumentOutOfRangeException("Position", value, "must be zero or positive number");

        position = value;
      }
    }

    public int BlockSize {
      get { CheckDisposed(); return blockSize; }
    }

    public bool LeaveInnerStreamOpen {
      get { CheckDisposed(); return leaveInnerStreamOpen; }
    }

    protected CachedStreamBase(Stream innerStream, int blockSize, bool leaveInnerStreamOpen)
    {
      if (innerStream == null)
        throw new ArgumentNullException("innerStream");
      else if (!innerStream.CanSeek)
        throw new ArgumentException("stream must be seekable", "innerStream");
      else if (!innerStream.CanRead)
        throw new ArgumentException("stream must be readable", "innerStream");

      if (blockSize <= 0)
        throw new ArgumentOutOfRangeException("blockSize", blockSize, "must be non-zero positive number");

      this.stream = innerStream;
      this.blockSize = blockSize;
      this.leaveInnerStreamOpen = leaveInnerStreamOpen;

      this.position = stream.Position;
    }

    public override void Close()
    {
      if (stream != null) {
        if (!leaveInnerStreamOpen)
          stream.Close();
        stream = null;
      }
    }

    public override void SetLength(long @value)
    {
      CheckDisposed();

      throw WriteNotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      CheckDisposed();

      // Stream.Seek spec: Seeking to any location beyond the length of the stream is supported.
      switch (origin) {
        case SeekOrigin.Current:
          offset += position;
          goto case SeekOrigin.Begin;

        case SeekOrigin.End:
          offset += Length;
          goto case SeekOrigin.Begin;

        case SeekOrigin.Begin:
          if (offset < 0L)
            throw new IOException("Attempted to seek before start of stream.");
          position = offset;
          return position;

        default:
          throw new ArgumentException(string.Format("unsupported seek origin {0}", origin), "origin");
      }
    }

    public override int ReadByte()
    {
      CheckDisposed();

      int blockOffset;
      var block = GetBlock(position, out blockOffset);

      if (block.Length <= blockOffset) {
        return -1;
      }
      else {
        position++;
        return block[blockOffset];
      }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      CheckDisposed();

      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset", offset, "must be zero or positive number");
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", count, "must be zero or positive number");
      if (buffer.Length < offset + count)
        throw new ArgumentException("invalid range");

      var ret = 0;

      for (;;) {
        int blockOffset;
        var block = GetBlock(position, out blockOffset);

        var bytesToCopy = Math.Min(block.Length - blockOffset, count);

        if (bytesToCopy <= 0)
          return ret; // end of stream

        Buffer.BlockCopy(block, blockOffset, buffer, offset, bytesToCopy);

        position  += bytesToCopy;
        ret       += bytesToCopy;
        offset    += bytesToCopy;
        count     -= bytesToCopy;

        if (count <= 0)
          return ret;
      }
    }

    private byte[] GetBlock(long offset, out int offsetInBlock)
    {
      long blockOffset;
      var blockIndex = Math.DivRem(position, (long)blockSize, out blockOffset);

      offsetInBlock = (int)blockOffset;

      return GetBlock(blockIndex);
    }

    protected abstract byte[] GetBlock(long blockIndex);

    protected byte[] ReadBlock(long blockIndex)
    {
      var block = new byte[blockSize];

      stream.Seek(blockIndex * blockSize, SeekOrigin.Begin);

      var read = stream.Read(block, 0, blockSize);

      if (read < blockSize)
        Array.Resize(ref block, read);

      return block;
    }

    public override void WriteByte(byte @value)
    {
      CheckDisposed();

      throw WriteNotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      CheckDisposed();

      throw WriteNotSupportedException();
    }

    public override void Flush()
    {
      CheckDisposed();

      throw WriteNotSupportedException();
    }

    private void CheckDisposed()
    {
      if (stream == null)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private Exception WriteNotSupportedException()
    {
      return new NotSupportedException("stream is read only");
    }

    private Stream stream;
    private readonly int blockSize;
    private readonly bool leaveInnerStreamOpen;
    private long position;
  }
}
