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
  public /*static*/ abstract class BinaryConvert {
    protected BinaryConvert() {}

    protected static void CheckSourceArray(byte[] @value, int startIndex, int count)
    {
      if (@value == null)
        throw new ArgumentNullException("value");
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", "must be zero or positive number");
      if (@value.Length - count < startIndex)
        throw new ArgumentOutOfRangeException("startIndex, value");
    }

    protected static void CheckDestArray(byte[] @bytes, int startIndex, int count)
    {
      if (@bytes == null)
        throw new ArgumentNullException("bytes");
      if (startIndex < 0)
        throw new ArgumentOutOfRangeException("startIndex", "must be zero or positive number");
      if (@bytes.Length - count < startIndex)
        throw new ArgumentOutOfRangeException("startIndex, bytes");
    }

    protected static Exception GetUnsupportedEndianException(Endianness endian)
    {
      return new NotSupportedException(string.Format("endian '{0}' is not supported", endian));
    }

    public static Int16 ToInt16(byte[] @value, int startIndex, Endianness endian)
    {
      return unchecked((Int16)ToUInt16(@value, startIndex, endian));
    }

    [CLSCompliant(false)]
    public static UInt16 ToUInt16(byte[] @value, int startIndex, Endianness endian)
    {
      CheckSourceArray(@value, startIndex, 2);

      switch (endian) {
        case Endianness.LittleEndian:
          return (UInt16)(@value[startIndex + 1] << 8 | @value[startIndex]);
        case Endianness.BigEndian:
          return (UInt16)(@value[startIndex] << 8 | @value[startIndex + 1]);
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static Int32 ToInt32(byte[] @value, int startIndex, Endianness endian)
    {
      return unchecked((Int32)ToUInt32(@value, startIndex, endian));
    }

    [CLSCompliant(false)]
    public static UInt32 ToUInt32(byte[] @value, int startIndex, Endianness endian)
    {
      CheckSourceArray(@value, startIndex, 4);

      switch (endian) {
        case Endianness.LittleEndian:
          return (UInt32)(@value[startIndex + 3] << 24 | @value[startIndex + 2] << 16 | @value[startIndex + 1] << 8 | @value[startIndex]);
        case Endianness.BigEndian:
          return (UInt32)(@value[startIndex] << 24 | @value[startIndex + 1] << 16 | @value[startIndex + 2] << 8 | @value[startIndex + 3]);
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static Int64 ToInt64(byte[] @value, int startIndex, Endianness endian)
    {
      return unchecked((Int64)ToUInt64(@value, startIndex, endian));
    }

    [CLSCompliant(false)]
    public static UInt64 ToUInt64(byte[] @value, int startIndex, Endianness endian)
    {
      switch (endian) {
        case Endianness.LittleEndian: {
          UInt64 low  = (UInt64)ToUInt32(@value, startIndex + 0, endian);
          UInt64 high = (UInt64)ToUInt32(@value, startIndex + 4, endian);

          return high << 32 | low;
        }

        case Endianness.BigEndian: {
          UInt64 high = (UInt64)ToUInt32(@value, startIndex + 0, endian);
          UInt64 low  = (UInt64)ToUInt32(@value, startIndex + 4, endian);

          return high << 32 | low;
        }

        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static void GetBytes(Int16 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      GetBytes(unchecked((UInt16)@value), endian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt16 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 2);

      switch (endian) {
        case Endianness.LittleEndian:
          unchecked {
            bytes[startIndex++] = (byte)(@value);
            bytes[startIndex++] = (byte)(@value >> 8);
          }
          break;

        case Endianness.BigEndian:
          unchecked {
            bytes[startIndex++] = (byte)(@value >> 8);
            bytes[startIndex++] = (byte)(@value);
          }
          break;

        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static void GetBytes(Int32 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      GetBytes(unchecked((UInt32)@value), endian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt32 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 4);

      switch (endian) {
        case Endianness.LittleEndian:
          unchecked {
            bytes[startIndex++] = (byte)(@value);
            bytes[startIndex++] = (byte)(@value >> 8);
            bytes[startIndex++] = (byte)(@value >> 16);
            bytes[startIndex++] = (byte)(@value >> 24);
          }
          break;

        case Endianness.BigEndian:
          unchecked {
            bytes[startIndex++] = (byte)(@value >> 24);
            bytes[startIndex++] = (byte)(@value >> 16);
            bytes[startIndex++] = (byte)(@value >> 8);
            bytes[startIndex++] = (byte)(@value);
          }
          break;

        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static void GetBytes(Int64 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      GetBytes(unchecked((UInt64)@value), endian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt64 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 8);

      switch (endian) {
        case Endianness.LittleEndian:
          unchecked {
            bytes[startIndex++] = (byte)(@value);
            bytes[startIndex++] = (byte)(@value >> 8);
            bytes[startIndex++] = (byte)(@value >> 16);
            bytes[startIndex++] = (byte)(@value >> 24);
            bytes[startIndex++] = (byte)(@value >> 32);
            bytes[startIndex++] = (byte)(@value >> 40);
            bytes[startIndex++] = (byte)(@value >> 48);
            bytes[startIndex++] = (byte)(@value >> 56);
          }
          break;

        case Endianness.BigEndian:
          unchecked {
            bytes[startIndex++] = (byte)(@value >> 56);
            bytes[startIndex++] = (byte)(@value >> 48);
            bytes[startIndex++] = (byte)(@value >> 40);
            bytes[startIndex++] = (byte)(@value >> 32);
            bytes[startIndex++] = (byte)(@value >> 24);
            bytes[startIndex++] = (byte)(@value >> 16);
            bytes[startIndex++] = (byte)(@value >> 8);
            bytes[startIndex++] = (byte)(@value);
          }
          break;

        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static byte[] GetBytes(Int16 @value, Endianness endian)
    {
      var bytes = new byte[2];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytes(UInt16 @value, Endianness endian)
    {
      var bytes = new byte[2];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(Int32 @value, Endianness endian)
    {
      var bytes = new byte[4];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytes(UInt32 @value, Endianness endian)
    {
      var bytes = new byte[4];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(Int64 @value, Endianness endian)
    {
      var bytes = new byte[8];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytes(UInt64 @value, Endianness endian)
    {
      var bytes = new byte[8];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }
  }
}

