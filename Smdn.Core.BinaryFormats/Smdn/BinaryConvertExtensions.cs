// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010-2012 smdn
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

    public static UInt24 ToUInt24LE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 3);

      return new UInt24(@value, startIndex, false);
    }

    public static UInt24 ToUInt24BE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 3);

      return new UInt24(@value, startIndex, true);
    }

    public static UInt24 ToUInt24(byte[] @value, int startIndex, Endianness endian)
    {
      switch (endian) {
        case Endianness.LittleEndian: return ToUInt24LE(@value, startIndex);
        case Endianness.BigEndian:    return ToUInt24BE(@value, startIndex);
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static UInt48 ToUInt48LE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 6);

      return new UInt48(@value, startIndex, false);
    }

    public static UInt48 ToUInt48BE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 6);

      return new UInt48(@value, startIndex, true);
    }

    public static UInt48 ToUInt48(byte[] @value, int startIndex, Endianness endian)
    {
      switch (endian) {
        case Endianness.LittleEndian: return ToUInt48LE(@value, startIndex);
        case Endianness.BigEndian:    return ToUInt48BE(@value, startIndex);
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static void GetBytesLE(UInt24 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 3);

      UInt32 val = @value.ToUInt32();

      unchecked {
        bytes[startIndex    ] = (byte)(val);
        bytes[startIndex + 1] = (byte)(val >> 8);
        bytes[startIndex + 2] = (byte)(val >> 16);
      }
    }

    public static void GetBytesBE(UInt24 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 3);

      UInt32 val = @value.ToUInt32();

      unchecked {
        bytes[startIndex    ] = (byte)(val >> 16);
        bytes[startIndex + 1] = (byte)(val >> 8);
        bytes[startIndex + 2] = (byte)(val);
      }
    }

    public static void GetBytes(UInt24 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      switch (endian) {
        case Endianness.LittleEndian: GetBytesLE(@value, bytes, startIndex); break;
        case Endianness.BigEndian:    GetBytesBE(@value, bytes, startIndex); break;
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static void GetBytesLE(UInt48 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 6);

      UInt64 val = @value.ToUInt64();

      unchecked {
        bytes[startIndex    ] = (byte)(val);
        bytes[startIndex + 1] = (byte)(val >> 8);
        bytes[startIndex + 2] = (byte)(val >> 16);
        bytes[startIndex + 3] = (byte)(val >> 24);
        bytes[startIndex + 4] = (byte)(val >> 32);
        bytes[startIndex + 5] = (byte)(val >> 40);
      }
    }

    public static void GetBytesBE(UInt48 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 6);

      UInt64 val = @value.ToUInt64();

      unchecked {
        bytes[startIndex    ] = (byte)(val >> 40);
        bytes[startIndex + 1] = (byte)(val >> 32);
        bytes[startIndex + 2] = (byte)(val >> 24);
        bytes[startIndex + 3] = (byte)(val >> 16);
        bytes[startIndex + 4] = (byte)(val >> 8);
        bytes[startIndex + 5] = (byte)(val);
      }
    }

    public static void GetBytes(UInt48 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      switch (endian) {
        case Endianness.LittleEndian: GetBytesLE(@value, bytes, startIndex); break;
        case Endianness.BigEndian:    GetBytesBE(@value, bytes, startIndex); break;
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static byte[] GetBytesLE(UInt24 @value)
    {
      var bytes = new byte[3];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesBE(UInt24 @value)
    {
      var bytes = new byte[3];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(UInt24 @value, Endianness endian)
    {
      var bytes = new byte[3];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesLE(UInt48 @value)
    {
      var bytes = new byte[6];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesBE(UInt48 @value)
    {
      var bytes = new byte[6];

      GetBytesBE(@value, bytes, 0);

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