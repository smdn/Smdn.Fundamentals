// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
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

    public static byte[] ToByteArray(string @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      var chars = @value.ToCharArray();
      var bytes = new byte[@value.Length];

      for (var index = 0; index < chars.Length; index++) {
        bytes[index] = (byte)chars[index];
      }

      return bytes;
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
        throw new ArgumentOutOfRangeException("index", index, "must be zero or positive number");
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", count, "must be zero or positive number");
      if (@value.Length < index + count)
        throw new ArgumentException("index + count is larger than length");

      this.bytes = new byte[count];

      Buffer.BlockCopy(@value, index, this.bytes, 0, count);
    }

    public ByteString(string @value)
    {
      this.bytes = ToByteArray(@value);
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
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return StartsWith(@value.bytes);
    }

    public bool StartsWith(byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      if (bytes.Length < @value.Length)
        return false;

      for (var index = 0; index < @value.Length; index++) {
        if (bytes[index] != @value[index])
          return false;
      }

      return true;
    }

    public bool StartsWithIgnoreCase(ByteString @value)
    {
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return StartsWithIgnoreCase(@value.bytes);
    }

    public bool StartsWithIgnoreCase(byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      if (bytes.Length < @value.Length)
        return false;

      for (var index = 0; index < @value.Length; index++) {
        if (bytes[index] == @value[index]) {
          continue;
        }
        else {
          var lower = ToLower(bytes[index]);

          if (lower == -1)
            return false;
          else if (lower == @value[index] || lower == ToLower(@value[index]))
            continue;
          else
            return false;
        }
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
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return EndsWith(@value.bytes);
    }

    public bool EndsWith(byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

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

    public bool IsPrefixOf(ByteString @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return IsPrefixOf(@value.bytes);
    }

    public bool IsPrefixOf(byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      if (@value.Length < bytes.Length)
        return false;

      for (var index = 0; index < bytes.Length; index++) {
        if (@value[index] != bytes[index])
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
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return IndexOf(@value.bytes, 0, true);
    }

    public int IndexOf(ByteString @value, int startIndex)
    {
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return IndexOf(@value.bytes, startIndex, true);
    }

    public int IndexOfIgnoreCase(ByteString @value)
    {
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return IndexOf(@value.bytes, 0, false);
    }

    public int IndexOfIgnoreCase(ByteString @value, int startIndex)
    {
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return IndexOf(@value.bytes, startIndex, false);
    }

    public int IndexOf(byte[] @value)
    {
      return IndexOf(@value, 0, true);
    }

    public int IndexOf(byte[] @value, int startIndex)
    {
      return IndexOf(@value, startIndex, true);
    }

    public int IndexOfIgnoreCase(byte[] @value)
    {
      return IndexOf(@value, 0, false);
    }

    public int IndexOfIgnoreCase(byte[] @value, int startIndex)
    {
      return IndexOf(@value, startIndex, false);
    }

    private int IndexOf(byte[] @value, int startIndex, bool caseSensitive)
    {
      if (@value == null)
        throw new ArgumentNullException("value");
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", startIndex, "must be zero or positive number");

      if (bytes.Length < @value.Length)
        return -1;

      var matchedIndex = 0;

      for (var index = startIndex; index < bytes.Length; index++) {
      recheck:
        var matched = bytes[index] == @value[matchedIndex];

        if (!matched && !caseSensitive) {
          var lower = ToLower(bytes[index]);

          if (lower != -1)
            matched = (lower == @value[matchedIndex] || lower == ToLower(@value[matchedIndex]));
        }

        if (matched) {
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
        throw new ArgumentOutOfRangeException("startIndex", startIndex, "must be zero or positive number");

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

    public int IndexOfNot(char @value)
    {
      return IndexOfNot(@value, 0);
    }

    public int IndexOfNot(char @value, int startIndex)
    {
      for (var index = startIndex; index < bytes.Length; index++) {
        if (bytes[index] != @value)
          return index;
      }

      return -1;
    }

    public int IndexOfNot(byte @value)
    {
      return IndexOfNot(@value, 0);
    }

    public int IndexOfNot(byte @value, int startIndex)
    {
      for (var index = startIndex; index < bytes.Length; index++) {
        if (bytes[index] != @value)
          return index;
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

    public ByteString[] Split(byte delimiter)
    {
      return Split((char)delimiter);
    }

    public ByteString[] Split(char delimiter)
    {
      var splitted = new List<ByteString>();
      var index = 0;
      var lastIndex = 0;

      for (index = 0; index < bytes.Length; index++) {
        if (bytes[index] == delimiter) {
          splitted.Add(Substring(lastIndex, index - lastIndex));
          lastIndex = index + 1;
        }
      }

      splitted.Add(Substring(lastIndex, index - lastIndex));

      return splitted.ToArray();
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

    [CLSCompliant(false)]
    public uint ToUInt32()
    {
      uint val = 0U;

      for (var index = 0; index < bytes.Length; index++) {
        if (0x30 <= bytes[index] && bytes[index] <= 0x39)
          val = checked((val * 10) + (uint)(bytes[index] - 0x30));
        else
          throw new FormatException("contains non-number character");
      }

      return val;
    }

    [CLSCompliant(false)]
    public ulong ToUInt64()
    {
      ulong val = 0UL;

      for (var index = 0; index < bytes.Length; index++) {
        if (0x30 <= bytes[index] && bytes[index] <= 0x39)
          val = checked((val * 10) + (ulong)(bytes[index] - 0x30));
        else
          throw new FormatException("contains non-number character");
      }

      return val;
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
      if ((object)str == null)
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
      var byteString = obj as ByteString;

      if (byteString != null)
        return Equals(byteString.bytes);

      var byteArray = obj as byte[];

      if (byteArray != null)
        return Equals(byteArray);

      var str = obj as string;

      if (str != null)
        return Equals(str);

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
      if ((object)other == null)
        return false;
      else if (bytes.Length != other.Length)
        return false;

      for (var index = 0; index < bytes.Length; index++) {
        if (bytes[index] == other[index])
          continue;
        else if (bytes[index] == ToLower(other[index]))
          continue;
        else
          return false;
      }

      return true;
    }

    public bool EqualsIgnoreCase(string other)
    {
      if ((object)other == null)
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
        throw new ArgumentOutOfRangeException("y", y, "must be non-zero positive number");

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

    private int ToLower(byte b)
    {
      if (0x41 <= b && b <= 0x5a)
        return b + 0x20;
      else if (0x61 <= b && b <= 0x7a)
        return b;
      else
        return -1;
    }

    private byte[] bytes;
  }
}
