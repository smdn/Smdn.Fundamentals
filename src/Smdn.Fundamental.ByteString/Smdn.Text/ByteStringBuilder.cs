// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Text {
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class ByteStringBuilder {
    [System.Runtime.CompilerServices.IndexerName("Bytes")]
    public byte this[int index] {
      get
      {
        if (index < 0 || length <= index)
          throw new IndexOutOfRangeException();
        return buffer[index];
      }
      set
      {
        if (index < 0 || length <= index)
          throw new ArgumentOutOfRangeException(nameof(index), index, "index out of range");
        buffer[index] = value;
      }
    }

    public int Length {
      get { return length; }
      set
      {
        if (value < 0)
          throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(Length), value);
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
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(capacity), capacity);
      if (maxCapacity < capacity)
        throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(nameof(capacity), nameof(maxCapacity), maxCapacity);

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
        throw new ArgumentNullException(nameof(str));

      return Append(str.Segment);
    }

    public ByteStringBuilder Append(ByteString str, int startIndex, int count)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));

      return Append(str.GetSubSegment(startIndex, count));
    }

    public ByteStringBuilder Append(byte[] str)
    {
      return Append(new ArraySegment<byte>(str));
    }

    public ByteStringBuilder Append(byte[] str, int startIndex, int count)
    {
      return Append(new ArraySegment<byte>(str, startIndex, count));
    }

    public ByteStringBuilder Append(ArraySegment<byte> segment)
    {
      if (segment.Count == 0)
        return this;

      EnsureCapacity(length + segment.Count);

      Buffer.BlockCopy(segment.Array, segment.Offset, buffer, length, segment.Count);

      length += segment.Count;

      return this;
    }

    public ByteStringBuilder Append(string str)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));

      if (str.Length == 0)
        return this;

      EnsureCapacity(length + str.Length);

      for (var index = 0; index < str.Length; index++, length++) {
        buffer[length] = (byte)str[index];
      }

      return this;
    }

    private void EnsureCapacity(int capacity)
    {
      if (capacity <= buffer.Length)
        return;

      capacity = Math.Max(capacity, buffer.Length * 2);

      if (maxCapacity < capacity)
        throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(nameof(MaxCapacity), nameof(capacity), capacity);

      var newBuffer = new byte[capacity];

      Buffer.BlockCopy(buffer, 0, newBuffer, 0, length);

      buffer = newBuffer;
    }

    public ArraySegment<byte> GetSegment()
    {
      return new ArraySegment<byte>(buffer, 0, length);
    }

    public ArraySegment<byte> GetSegment(int offset, int count)
    {
      if (offset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
      if (length - count < offset)
        throw new ArgumentException("index + count is larger than length");

      return new ArraySegment<byte>(buffer, offset, count);
    }

    public byte[] ToByteArray()
    {
      var bytes = new byte[length];

      Buffer.BlockCopy(buffer, 0, bytes, 0, length);

      return bytes;
    }

    public ByteString ToByteString(bool asMutable)
    {
      return ByteString.Create(asMutable, buffer, 0, length);
    }

    public override string ToString()
    {
      return ByteString.ToString(null, buffer.AsSpan(0, length));
    }

    private byte[] buffer;
    private int length;
    private int maxCapacity;
  }
}
