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

namespace Smdn {
  public class ByteString : IEquatable<ByteString>, IEquatable<byte[]>, IEquatable<string> {
    [System.Runtime.CompilerServices.IndexerName("Bytes")]
    public byte this[int index] {
      get { return bytes[index]; }
      set { bytes[index] = value; }
    }

    public int Length {
      get { return bytes.Length; }
    }

    public bool IsEmpty {
      get { return bytes.Length == 0; }
    }

    public byte[] ByteArray {
      get { return bytes; }
    }

    public static ByteString CreateEmpty()
    {
      return new ByteString(new byte[] {});
    }

    public ByteString(params byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      this.bytes = @value;
    }

    public ByteString(byte[] @value, int index)
      : this(@value, index, @value.Length - index)
    {
    }

    public ByteString(byte[] @value, int index, int count)
    {
      if (@value == null)
        throw new ArgumentNullException("value");
      if (index < 0)
        throw new ArgumentOutOfRangeException("index", "must be zero or positive number");
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", "must be zero or positive number");
      if (@value.Length < index + count)
        throw new ArgumentException("index + count is larger than length");

      this.bytes = new byte[count];

      Buffer.BlockCopy(@value, index, this.bytes, 0, count);
    }

    public ByteString(string @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      var chars = @value.ToCharArray();

      this.bytes = new byte[@value.Length];

      for (var index = 0; index < chars.Length; index++) {
        this.bytes[index] = (byte)chars[index];
      }
    }

    public bool Contains(ByteString @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return Contains(@value.bytes);
    }

    public bool Contains(byte[] @value)
    {
      return 0 <= IndexOf(@value, 0);
    }

    public bool StartsWith(ByteString @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return StartsWith(@value.bytes);
    }

    public bool StartsWith(byte[] @value)
    {
      if (bytes.Length < @value.Length)
        return false;

      for (var index = 0; index < @value.Length; index++) {
        if (bytes[index] != @value[index])
          return false;
      }

      return true;
    }

    public bool StartsWith(string @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      if (bytes.Length < @value.Length)
        return false;

      var chars = @value.ToCharArray();

      for (var index = 0; index < chars.Length; index++) {
        if (bytes[index] != chars[index])
          return false;
      }

      return true;
    }

    public bool EndsWith(ByteString @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return EndsWith(@value.bytes);
    }

    public bool EndsWith(byte[] @value)
    {
      if (bytes.Length < @value.Length)
        return false;

      var offset = bytes.Length - @value.Length;

      for (var index = 0; index < @value.Length; index++, offset++) {
        if (bytes[offset] != @value[index])
          return false;
      }

      return true;
    }

    public bool EndsWith(string @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      if (bytes.Length < @value.Length)
        return false;

      var chars = @value.ToCharArray();
      var offset = bytes.Length - @value.Length;

      for (var index = 0; index < chars.Length; index++, offset++) {
        if (bytes[offset] != chars[index])
          return false;
      }

      return true;
    }

    public int IndexOf(char @value)
    {
      return IndexOf(@value, 0);
    }

    public int IndexOf(char @value, int startIndex)
    {
      for (var index = startIndex; index < bytes.Length; index++) {
        if (bytes[index] == @value)
          return index;
      }

      return -1;
    }

    public int IndexOf(byte @value)
    {
      return IndexOf(@value, 0);
    }

    public int IndexOf(byte @value, int startIndex)
    {
      for (var index = startIndex; index < bytes.Length; index++) {
        if (bytes[index] == @value)
          return index;
      }

      return -1;
    }

    public int IndexOf(ByteString @value)
    {
      return IndexOf(@value, 0);
    }

    public int IndexOf(ByteString @value, int startIndex)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return IndexOf(@value.bytes, startIndex);
    }

    public int IndexOf(byte[] @value)
    {
      return IndexOf(@value, 0);
    }

    public int IndexOf(byte[] @value, int startIndex)
    {
      if (@value == null)
        throw new ArgumentNullException("value");
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", "must be zero or positive number");

      if (bytes.Length < @value.Length)
        return -1;

      var matchedIndex = 0;

      for (var index = startIndex; index < bytes.Length; index++) {
      recheck:
        if (bytes[index] == @value[matchedIndex]) {
          if (@value.Length == ++matchedIndex)
            return index - matchedIndex + 1;
        }
        else if (0 < matchedIndex) {
          matchedIndex = 0;
          goto recheck;
        }
      }

      return -1;
    }

    public int IndexOf(string @value)
    {
      return IndexOf(@value, 0);
    }

    public int IndexOf(string @value, int startIndex)
    {
      if (@value == null)
        throw new ArgumentNullException("value");
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", "must be zero or positive number");

      if (bytes.Length < @value.Length)
        return -1;

      var chars = @value.ToCharArray();
      var matchedIndex = 0;

      for (var index = startIndex; index < bytes.Length; index++) {
      recheck:
        if (bytes[index] == chars[matchedIndex]) {
          if (@value.Length == ++matchedIndex)
            return index - matchedIndex + 1;
        }
        else if (0 < matchedIndex) {
          matchedIndex = 0;
          goto recheck;
        }
      }

      return -1;
    }

    public ByteString Substring(int index)
    {
      return new ByteString(bytes, index, bytes.Length - index);
    }

    public ByteString Substring(int index, int count)
    {
      return new ByteString(bytes, index, count);
    }

    public ByteString ToUpper()
    {
      var uppercased = new byte[bytes.Length];

      Buffer.BlockCopy(bytes, 0, uppercased, 0, bytes.Length);

      for (var index = 0; index < uppercased.Length; index++) {
        if (0x61 <= uppercased[index] && uppercased[index] <= 0x7a)
          uppercased[index] = (byte)(uppercased[index] - 0x20);
      }

      return new ByteString(uppercased);
    }

    public ByteString ToLower()
    {
      var lowercased = new byte[bytes.Length];

      Buffer.BlockCopy(bytes, 0, lowercased, 0, bytes.Length);

      for (var index = 0; index < lowercased.Length; index++) {
        if (0x41 <= lowercased[index] && lowercased[index] <= 0x5a)
          lowercased[index] = (byte)(lowercased[index] + 0x20);
      }

      return new ByteString(lowercased);
    }

    public ByteString TrimStart()
    {
      for (var index = 0; index < bytes.Length; index++) {
        if (!(bytes[index] == 0x20 ||
              bytes[index] == 0xa0 ||
              bytes[index] == 0x09 ||
              bytes[index] == 0x0a ||
              bytes[index] == 0x0b ||
              bytes[index] == 0x0c ||
              bytes[index] == 0x0d))
          return new ByteString(bytes, index, bytes.Length - index);
      }

      return CreateEmpty();
    }

    public ByteString TrimEnd()
    {
      for (var index = bytes.Length - 1; 0 <= index; index--) {
        if (!(bytes[index] == 0x20 ||
              bytes[index] == 0xa0 ||
              bytes[index] == 0x09 ||
              bytes[index] == 0x0a ||
              bytes[index] == 0x0b ||
              bytes[index] == 0x0c ||
              bytes[index] == 0x0d))
          return new ByteString(bytes, 0, index + 1);
      }

      return CreateEmpty();
    }

    public ByteString Trim()
    {
      return TrimStart().TrimEnd();
    }

    public static bool IsNullOrEmpty(ByteString str)
    {
      if (str == null)
        return true;
      else if (str.Length == 0)
        return true;
      else
        return false;
    }

    public bool Equals(ByteString other)
    {
      if (Object.ReferenceEquals(this, other))
        return true;
      else
        return this == other;
    }

    public override bool Equals(object obj)
    {
      if (obj is ByteString)
        return Equals((obj as ByteString).bytes);
      else if (obj is byte[])
        return Equals(obj as byte[]);
      else if (obj is string)
        return Equals(obj as string);
      else
        return false;
    }

    public bool Equals(byte[] other)
    {
      if (other == null)
        return false;
      if (bytes.Length != other.Length)
        return false;

      for (var index = 0; index < bytes.Length; index++) {
        if (bytes[index] != other[index])
          return false;
      }

      return true;
    }

    public bool Equals(string other)
    {
      if (other == null)
        return false;
      if (bytes.Length != other.Length)
        return false;

      var chars = other.ToCharArray();

      for (var index = 0; index < bytes.Length; index++) {
        if (bytes[index] != chars[index])
          return false;
      }

      return true;
    }

    public static bool operator == (ByteString x, ByteString y)
    {
      if (null == (object)x || null == (object)y) {
        if (null == (object)x && null == (object)y)
          return true;
        else
          return false;
      }

      return x.Equals(y.bytes);
    }

    public static bool operator != (ByteString x, ByteString y)
    {
      return !(x == y);
    }

    public bool EqualsIgnoreCase(ByteString other)
    {
      if (other == null)
        return false;
      else if (bytes.Length != other.Length)
        return false;

      return this.ToLower().Equals(other.ToLower()); // XXX
    }

    public bool EqualsIgnoreCase(string other)
    {
      if (other == null)
        return false;
      else if (bytes.Length != other.Length)
        return false;

      return this.ToLower().Equals(other.ToLower()); // XXX
    }

    public static ByteString operator + (ByteString x, ByteString y)
    {
      if (x == null)
        throw new ArgumentNullException("x");
      if (y == null)
        throw new ArgumentNullException("y");

      return Concat(x, y);
    }

    public static ByteString operator * (ByteString x, int y)
    {
      if (x == null)
        throw new ArgumentNullException("x");
      if (y < 0)
        throw new ArgumentOutOfRangeException("y", "must be non-zero positive number");

      if (x == null)
        return CreateEmpty();

      var bytes = new byte[x.Length * y];

      for (int count = 0, offset = 0; count < y; count++, offset += x.Length) {
        Buffer.BlockCopy(x.bytes, 0, bytes, offset, x.Length);
      }

      return new ByteString(bytes);
    }

    public static ByteString Concat(params ByteString[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");

      var length = 0;

      foreach (var val in values) {
        if (val != null)
          length += val.Length;
      }

      var bytes = new byte[length];
      var offset = 0;

      foreach (var val in values) {
        if (val != null) {
          Buffer.BlockCopy(val.bytes, 0, bytes, offset, val.Length);
          offset += val.Length;
        }
      }

      return new ByteString(bytes);
    }

    public override int GetHashCode()
    {
      var h = 0;

      for (var index = 0; index < bytes.Length; index++) {
        h = unchecked(h * 37 + bytes[index]);
      }

      return h;
    }

    public override string ToString()
    {
#if true
      var chars = new char[bytes.Length];

      for (var index = 0; index < bytes.Length; index++) {
        chars[index] = (char)bytes[index];
      }

      return new string(chars);
#else
      return System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
#endif
    }

    private byte[] bytes;
  }
}
