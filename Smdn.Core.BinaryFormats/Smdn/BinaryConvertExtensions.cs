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

namespace Smdn {
  public class BinaryConvertExtensions : BinaryConvert {
    protected BinaryConvertExtensions() {}

    public static UInt24 ToUInt24(byte[] @value, int startIndex, Endianness endian)
    {
      CheckSourceArray(@value, startIndex, 3);

      switch (endian) {
        case Endianness.LittleEndian:
          return new UInt24(@value, startIndex, false);
        case Endianness.BigEndian:
          return new UInt24(@value, startIndex, true);
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static UInt48 ToUInt48(byte[] @value, int startIndex, Endianness endian)
    {
      CheckSourceArray(@value, startIndex, 6);

      switch (endian) {
        case Endianness.LittleEndian:
          return new UInt48(@value, startIndex, false);
        case Endianness.BigEndian:
          return new UInt48(@value, startIndex, true);
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static void GetBytes(UInt24 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 3);

      UInt32 val = @value.ToUInt32();

      switch (endian) {
        case Endianness.LittleEndian:
          unchecked {
            bytes[startIndex++] = (byte)(val);
            bytes[startIndex++] = (byte)(val >> 8);
            bytes[startIndex++] = (byte)(val >> 16);
          }
          break;

        case Endianness.BigEndian:
          unchecked {
            bytes[startIndex++] = (byte)(val >> 16);
            bytes[startIndex++] = (byte)(val >> 8);
            bytes[startIndex++] = (byte)(val);
          }
          break;

        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static void GetBytes(UInt48 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 6);

      UInt64 val = @value.ToUInt64();

      switch (endian) {
        case Endianness.LittleEndian:
          unchecked {
            bytes[startIndex++] = (byte)(val);
            bytes[startIndex++] = (byte)(val >> 8);
            bytes[startIndex++] = (byte)(val >> 16);
            bytes[startIndex++] = (byte)(val >> 24);
            bytes[startIndex++] = (byte)(val >> 32);
            bytes[startIndex++] = (byte)(val >> 40);
          }
          break;

        case Endianness.BigEndian:
          unchecked {
            bytes[startIndex++] = (byte)(val >> 40);
            bytes[startIndex++] = (byte)(val >> 32);
            bytes[startIndex++] = (byte)(val >> 24);
            bytes[startIndex++] = (byte)(val >> 16);
            bytes[startIndex++] = (byte)(val >> 8);
            bytes[startIndex++] = (byte)(val);
          }
          break;

        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static byte[] GetBytes(UInt24 @value, Endianness endian)
    {
      var bytes = new byte[3];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(UInt48 @value, Endianness endian)
    {
      var bytes = new byte[6];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }
  }
}