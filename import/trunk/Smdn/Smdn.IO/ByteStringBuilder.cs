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

namespace Smdn.IO {
  public class ByteStringBuilder {
    public int Length {
      get { return length; }
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException("Length", "must be greater than zero");
        length = value;
      }
    }

    public int Capacity {
      get { return buffer.Length; }
    }

    public int MaxCapacity {
      get { return maxCapacity; }
    }

    public ByteStringBuilder()
      : this(16, int.MaxValue)
    {
    }

    public ByteStringBuilder(int capacity)
      : this(capacity, int.MaxValue)
    {
    }

    public ByteStringBuilder(int capacity, int maxCapacity)
    {
      if (capacity <= 0)
        throw new ArgumentOutOfRangeException("capacity", "must be non-zero positive number");
      if (maxCapacity < capacity)
        throw new ArgumentOutOfRangeException("maxCapacity", "must be greater than capacity");

      this.buffer = new byte[capacity];
      this.length = 0;
      this.maxCapacity = maxCapacity;
    }

    public ByteStringBuilder Append(byte b)
    {
      EnsureCapacity(length + 1);

      buffer[length++] = b;

      return this;
    }

    public ByteStringBuilder Append(ByteString str)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      return Append(str.ByteArray);
    }

    public ByteStringBuilder Append(byte[] str)
    {
      return Append(str, 0, str.Length);
    }

    public ByteStringBuilder Append(byte[] str, int index, int count)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (index < 0)
        throw new ArgumentOutOfRangeException("index", "must be zero or positive number");
      if (count < 0)
        throw new ArgumentOutOfRangeException("index", "must be zero or positive number");
      if (str.Length < index + count)
        throw new ArgumentException("index + count is larger than length");

      if (count == 0)
        return this;

      EnsureCapacity(length + count);

      Buffer.BlockCopy(str, index, buffer, length, count);

      length += count;

      return this;
    }

    private void EnsureCapacity(int capacity)
    {
      if (capacity <= buffer.Length)
        return;

      if (maxCapacity < capacity)
        capacity = maxCapacity;

      var newBuffer = new byte[capacity];

      Buffer.BlockCopy(buffer, 0, newBuffer, 0, length);

      buffer = newBuffer;
    }

    public ByteString ToByteString()
    {
      return new ByteString(buffer, 0, length);
    }

    public override string ToString()
    {
      return ToByteString().ToString();
    }

    private byte[] buffer;
    private int length;
    private int maxCapacity;
  }
}
