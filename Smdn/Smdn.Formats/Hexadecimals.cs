// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2014 smdn
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

namespace Smdn.Formats {
  public static class Hexadecimals {
    public static string ToLowerString(byte[] bytes)
    {
      return new string(ConvertByteArrayToHex(bytes, Chars.LowerCaseHexCharArray));
    }

    public static string ToUpperString(byte[] bytes)
    {
      return new string(ConvertByteArrayToHex(bytes, Chars.UpperCaseHexCharArray));
    }

    public static byte[] ToLowerByteArray(byte[] bytes)
    {
      return ConvertByteArrayToHex(bytes, Octets.LowerCaseHexOctetArray);
    }

    public static byte[] ToUpperByteArray(byte[] bytes)
    {
      return ConvertByteArrayToHex(bytes, Octets.UpperCaseHexOctetArray);
    }

    private static T[] ConvertByteArrayToHex<T>(byte[] bytes, T[] table)
    {
      var hex = new T[bytes.Length * 2];

      for (int b = 0, c = 0; b < bytes.Length;) {
        hex[c++] = table[bytes[b] >> 4];
        hex[c++] = table[bytes[b] & 0xf];
        b++;
      }

      return hex;
    }

    public static byte[] ToByteArray(string hexString)
    {
      return ConvertStringToByteArray(hexString, true, true);
    }

    public static byte[] ToByteArrayFromLowerString(string lowerCasedString)
    {
      return ConvertStringToByteArray(lowerCasedString, true, false);
    }

    public static byte[] ToByteArrayFromUpperString(string upperCasedString)
    {
      return ConvertStringToByteArray(upperCasedString, false, true);
    }

    private static byte[] ConvertStringToByteArray(string str, bool allowLowerCaseChar, bool allowUpperCaseChar)
    {
      if ((str.Length & 0x1) != 0)
        throw new FormatException("incorrect form");

      var chars = str.ToCharArray();
      var bytes = new byte[chars.Length / 2];
      var high = true;

      for (int c = 0, b = 0; c < chars.Length;) {
        int val;

        if ('0' <= chars[c] && chars[c] <= '9') {
          val = (int)(chars[c] - '0');
        }
        else if ('a' <= chars[c] && chars[c] <= 'f') {
          if (allowLowerCaseChar)
            val = 0xa + (int)(chars[c] - 'a');
          else
            throw new FormatException("incorrect form");
        }
        else if ('A' <= chars[c] && chars[c] <= 'F') {
          if (allowUpperCaseChar)
            val = 0xa + (int)(chars[c] - 'A');
          else
            throw new FormatException("incorrect form");
        }
        else {
          throw new FormatException("incorrect form");
        }

        if (high) {
          bytes[b] = (byte)(val << 4);
        }
        else {
          bytes[b] = (byte)(bytes[b] | val);
          b++;
        }

        c++;
        high = !high;
      }

      return bytes;
    }
  }
}
