// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010 smdn
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

namespace Smdn.IO {
  public abstract class BinaryWriterBase : IDisposable {
    public bool LeaveBaseStreamOpen {
      get { CheckDisposed(); return leaveBaseStreamOpen; }
    }

    public Stream BaseStream {
      get { CheckDisposed(); return stream; }
    }

    protected bool Disposed {
      get { return disposed; }
    }

    protected BinaryWriterBase(Stream baseStream, bool leaveBaseStreamOpen)
    {
      if (baseStream == null)
        throw new ArgumentNullException("baseStream");
      if (!baseStream.CanWrite)
        throw ExceptionUtils.CreateArgumentMustBeWritableStream("baseStream");

      this.stream = baseStream;
      this.leaveBaseStreamOpen = leaveBaseStreamOpen;
    }

    public virtual void Close()
    {
      Dispose(true);
    }

    void IDisposable.Dispose()
    {
      Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && stream != null) {
        if (!LeaveBaseStreamOpen)
          stream.Close();

        stream = null;
      }

      disposed = true;
    }

    public void Flush()
    {
      CheckDisposed();

      stream.Flush();
    }

    public virtual void Write(Byte @value)
    {
      stream.WriteByte(@value);
    }

    [CLSCompliant(false)]
    public virtual void Write(SByte @value)
    {
      stream.WriteByte(unchecked((byte)@value));
    }

    public abstract void Write(Int16 @value);

    [CLSCompliant(false)]
    public virtual void Write(UInt16 @value)
    {
      Write(unchecked((Int16)@value));
    }

    public abstract void Write(Int32 @value);

    [CLSCompliant(false)]
    public virtual void Write(UInt32 @value)
    {
      Write(unchecked((Int32)@value));
    }

    public abstract void Write(Int64 @value);

    [CLSCompliant(false)]
    public virtual void Write(UInt64 @value)
    {
      Write(unchecked((Int64)@value));
    }

    public void Write(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException("buffer");

      Write(buffer, 0, buffer.Length);
    }

    public void Write(byte[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("count", count);
      if (index < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("index", index);
      if (buffer.Length - count < index)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("index", buffer, index, count);

      if (count == 0)
        return;

      WriteUnchecked(buffer, index, count);
    }

    public void Write(ArraySegment<byte> @value)
    {
      if (@value.Array == null)
        throw new ArgumentException("value.Array is null", "value");

      if (@value.Count == 0)
        return;

      WriteUnchecked(@value.Array, @value.Offset, @value.Count);
    }

    protected void WriteUnchecked(byte[] buffer, int index, int count)
    {
      CheckDisposed();

      stream.Write(buffer, index, count);
    }

    public void WriteZero(int count)
    {
      WriteZero((long)count);
    }

    public void WriteZero(long count)
    {
      if (count < 0L)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("count", count);
      if (count == 0L)
        return;

      CheckDisposed();

      var zeroes = new byte[Math.Min(count, 4096)];

      for (; 0 < count; count -= zeroes.Length) {
        if (zeroes.Length < count)
          stream.Write(zeroes, 0, zeroes.Length);
        else
          stream.Write(zeroes, 0, (int)count);
      }
    }

    protected void CheckDisposed()
    {
      if (disposed)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private Stream stream;
    private readonly bool leaveBaseStreamOpen;
    private bool disposed = false;
  }
}

