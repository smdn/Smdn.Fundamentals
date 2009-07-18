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
  public class ByteString {
    [System.Runtime.CompilerServices.IndexerName("Bytes")]
    public byte this[int index] {
      get { return bytes[index]; }
      set { bytes[index] = value; }
    }

    public int Length {
      get { return bytes.Length; }
    }

    public bool Empty {
      get { return bytes.Length == 0; }
    }

    public byte[] ByteArray {
      get { return bytes; }
    }

    public ByteString(params byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      this.bytes = @value;
    }

    public ByteString(byte[] @value, int index)
      : this(@value, index, @value.Length)
    {
    }

    public ByteString(byte[] @value, int index, int count)
    {
      if (@value == null)
        throw new ArgumentNullException("value");
      if (index < 0)
        throw new ArgumentOutOfRangeException("index", "must be zero or positive number");
      if (@value.Length <= index)
        throw new ArgumentOutOfRangeException("index", "larger than length");
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", "must be zero or positive number");
      if (@value.Length < count)
        throw new ArgumentOutOfRangeException("index", "larger than length");

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

    public bool Contains(params byte[] @value)
    {
      return 0 <= IndexOf(@value);
    }

    public bool StartsWith(ByteString @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return StartsWith(@value.bytes);
    }

    public bool StartsWith(params byte[] @value)
    {
      if (bytes.Length < @value.Length)
        return false;

      for (var index = 0; index < @value.Length; index++) {
        if (bytes[index] != @value[index])
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

    public bool EndsWith(params byte[] @value)
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

    public int IndexOf(char @value)
    {
      return IndexOf((byte)@value);
    }

    public int IndexOf(byte @value)
    {
      for (var index = 0; index < bytes.Length; index++) {
        if (bytes[index] == @value)
          return index;
      }

      return -1;
    }

    public int IndexOf(ByteString @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return IndexOf(@value.bytes);
    }

    public int IndexOf(params byte[] @value)
    {
      var matchedIndex = 0;

      for (var index = 0; index < bytes.Length; index++) {
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

    public ByteString Substring(int index)
    {
      return new ByteString(bytes, index, bytes.Length - index);
    }

    public ByteString Substring(int index, int count)
    {
      return new ByteString(bytes, index, count);
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

    public override string ToString()
    {
      var chars = new char[bytes.Length];

      for (var index = 0; index < bytes.Length; index++) {
        chars[index] = (char)bytes[index];
      }

      return new string(chars);
    }

    private byte[] bytes;
  }
}
