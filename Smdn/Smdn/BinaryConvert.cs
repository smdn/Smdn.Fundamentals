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

    public static Int16 ByteSwap(Int16 @value)
    {
      unchecked {
        return (Int16)(((@value >> 8) & 0x00ff) | (@value << 8));
      }
    }

    [CLSCompliant(false)]
    public static UInt16 ByteSwap(UInt16 @value)
    {
      unchecked {
        return (UInt16)(((@value >> 8) & 0x00ff) | (@value << 8));
      }
    }

    public static Int32 ByteSwap(Int32 @value)
    {
      unchecked {
        return (Int32)(((@value >> 24) & 0x000000ff) |
                       ((@value >>  8) & 0x0000ff00) |
                       ((@value <<  8) & 0x00ff0000) |
                        (@value << 24));
      }
    }

    [CLSCompliant(false)]
    public static UInt32 ByteSwap(UInt32 @value)
    {
      unchecked {
        return (UInt32)(((@value >> 24) & 0x000000ff) |
                        ((@value >>  8) & 0x0000ff00) |
                        ((@value <<  8) & 0x00ff0000) |
                         (@value << 24));
      }
    }

    public static Int64 ByteSwap(Int64 @value)
    {
      unchecked {
        return (Int64)(((@value >> 56) & 0x00000000000000ff) |
                       ((@value >> 40) & 0x000000000000ff00) |
                       ((@value >> 24) & 0x0000000000ff0000) |
                       ((@value >>  8) & 0x00000000ff000000) |
                       ((@value <<  8) & 0x000000ff00000000) |
                       ((@value << 24) & 0x0000ff0000000000) |
                       ((@value << 40) & 0x00ff000000000000) |
                        (@value << 56));
      }
    }

    [CLSCompliant(false)]
    public static UInt64 ByteSwap(UInt64 @value)
    {
      unchecked {
        return (UInt64)(((@value >> 56) & 0x00000000000000ff) |
                        ((@value >> 40) & 0x000000000000ff00) |
                        ((@value >> 24) & 0x0000000000ff0000) |
                        ((@value >>  8) & 0x00000000ff000000) |
                        ((@value <<  8) & 0x000000ff00000000) |
                        ((@value << 24) & 0x0000ff0000000000) |
                        ((@value << 40) & 0x00ff000000000000) |
                         (@value << 56));
      }
    }

    public static Int16 ToInt16LE(byte[] @value, int startIndex)
    {
      return unchecked((Int16)ToUInt16LE(@value, startIndex));
    }

    public static Int16 ToInt16BE(byte[] @value, int startIndex)
    {
      return unchecked((Int16)ToUInt16BE(@value, startIndex));
    }

    public static Int16 ToInt16(byte[] @value, int startIndex, Endianness endian)
    {
      return unchecked((Int16)ToUInt16(@value, startIndex, endian));
    }

    [CLSCompliant(false)]
    public static UInt16 ToUInt16LE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 2);

      return (UInt16)(@value[startIndex] |
                      @value[startIndex + 1] << 8);
    }

    [CLSCompliant(false)]
    public static UInt16 ToUInt16BE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 2);

      return (UInt16)(@value[startIndex] << 8 |
                      @value[startIndex + 1]);
    }

    [CLSCompliant(false)]
    public static UInt16 ToUInt16(byte[] @value, int startIndex, Endianness endian)
    {
      switch (endian) {
        case Endianness.LittleEndian: return ToUInt16LE(@value, startIndex);
        case Endianness.BigEndian:    return ToUInt16BE(@value, startIndex);
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static Int32 ToInt32LE(byte[] @value, int startIndex)
    {
      return unchecked((Int32)ToUInt32LE(@value, startIndex));
    }

    public static Int32 ToInt32BE(byte[] @value, int startIndex)
    {
      return unchecked((Int32)ToUInt32BE(@value, startIndex));
    }

    public static Int32 ToInt32(byte[] @value, int startIndex, Endianness endian)
    {
      return unchecked((Int32)ToUInt32(@value, startIndex, endian));
    }

    [CLSCompliant(false)]
    public static UInt32 ToUInt32LE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 4);

      return (UInt32)(@value[startIndex] |
                      @value[startIndex + 1] << 8 |
                      @value[startIndex + 2] << 16 |
                      @value[startIndex + 3] << 24);
    }

    [CLSCompliant(false)]
    public static UInt32 ToUInt32BE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 4);

      return (UInt32)(@value[startIndex] << 24 |
                      @value[startIndex + 1] << 16 |
                      @value[startIndex + 2] << 8 |
                      @value[startIndex + 3]);
    }

    [CLSCompliant(false)]
    public static UInt32 ToUInt32(byte[] @value, int startIndex, Endianness endian)
    {
      switch (endian) {
        case Endianness.LittleEndian: return ToUInt32LE(@value, startIndex);
        case Endianness.BigEndian:    return ToUInt32BE(@value, startIndex);
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static Int64 ToInt64LE(byte[] @value, int startIndex)
    {
      return unchecked((Int64)ToUInt64LE(@value, startIndex));
    }

    public static Int64 ToInt64BE(byte[] @value, int startIndex)
    {
      return unchecked((Int64)ToUInt64BE(@value, startIndex));
    }

    public static Int64 ToInt64(byte[] @value, int startIndex, Endianness endian)
    {
      return unchecked((Int64)ToUInt64(@value, startIndex, endian));
    }

    [CLSCompliant(false)]
    public static UInt64 ToUInt64LE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 8);

      UInt64 low  = (UInt32)(@value[startIndex] |
                             @value[startIndex + 1] << 8 |
                             @value[startIndex + 2] << 16 |
                             @value[startIndex + 3] << 24);
      UInt64 high = (UInt32)(@value[startIndex + 4 ] |
                             @value[startIndex + 5] << 8 |
                             @value[startIndex + 6] << 16 |
                             @value[startIndex + 7] << 24);

      return high << 32 | low;
    }

    [CLSCompliant(false)]
    public static UInt64 ToUInt64BE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 8);

      UInt64 high = (UInt32)(@value[startIndex] << 24 |
                             @value[startIndex + 1] << 16 |
                             @value[startIndex + 2] << 8 |
                             @value[startIndex + 3]);
      UInt64 low  = (UInt32)(@value[startIndex + 4] << 24 |
                             @value[startIndex + 5] << 16 |
                             @value[startIndex + 6] << 8 |
                             @value[startIndex + 7]);

      return high << 32 | low;
    }

    [CLSCompliant(false)]
    public static UInt64 ToUInt64(byte[] @value, int startIndex, Endianness endian)
    {
      switch (endian) {
        case Endianness.LittleEndian: return ToUInt64LE(@value, startIndex);
        case Endianness.BigEndian:    return ToUInt64BE(@value, startIndex);
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static void GetBytesLE(Int16 @value, byte[] bytes, int startIndex)
    {
      GetBytesLE(unchecked((UInt16)@value), bytes, startIndex);
    }

    public static void GetBytesBE(Int16 @value, byte[] bytes, int startIndex)
    {
      GetBytesBE(unchecked((UInt16)@value), bytes, startIndex);
    }

    public static void GetBytes(Int16 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      GetBytes(unchecked((UInt16)@value), endian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesLE(UInt16 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 2);

      unchecked {
        bytes[startIndex    ] = (byte)(@value);
        bytes[startIndex + 1] = (byte)(@value >> 8);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytesBE(UInt16 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 2);

      unchecked {
        bytes[startIndex    ] = (byte)(@value >> 8);
        bytes[startIndex + 1] = (byte)(@value);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt16 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      switch (endian) {
        case Endianness.LittleEndian: GetBytesLE(@value, bytes, startIndex); break;
        case Endianness.BigEndian:    GetBytesBE(@value, bytes, startIndex); break;
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static void GetBytesLE(Int32 @value, byte[] bytes, int startIndex)
    {
      GetBytesLE(unchecked((UInt32)@value), bytes, startIndex);
    }

    public static void GetBytesBE(Int32 @value, byte[] bytes, int startIndex)
    {
      GetBytesBE(unchecked((UInt32)@value), bytes, startIndex);
    }

    public static void GetBytes(Int32 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      GetBytes(unchecked((UInt32)@value), endian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesLE(UInt32 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 4);

      unchecked {
        bytes[startIndex    ] = (byte)(@value);
        bytes[startIndex + 1] = (byte)(@value >> 8);
        bytes[startIndex + 2] = (byte)(@value >> 16);
        bytes[startIndex + 3] = (byte)(@value >> 24);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytesBE(UInt32 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 4);

      unchecked {
        bytes[startIndex    ] = (byte)(@value >> 24);
        bytes[startIndex + 1] = (byte)(@value >> 16);
        bytes[startIndex + 2] = (byte)(@value >> 8);
        bytes[startIndex + 3] = (byte)(@value);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt32 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      switch (endian) {
        case Endianness.LittleEndian: GetBytesLE(@value, bytes, startIndex); break;
        case Endianness.BigEndian:    GetBytesBE(@value, bytes, startIndex); break;
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static void GetBytesLE(Int64 @value, byte[] bytes, int startIndex)
    {
      GetBytesLE(unchecked((UInt64)@value), bytes, startIndex);
    }

    public static void GetBytesBE(Int64 @value, byte[] bytes, int startIndex)
    {
      GetBytesBE(unchecked((UInt64)@value), bytes, startIndex);
    }

    public static void GetBytes(Int64 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      GetBytes(unchecked((UInt64)@value), endian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesLE(UInt64 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 8);

      unchecked {
        bytes[startIndex    ] = (byte)(@value);
        bytes[startIndex + 1] = (byte)(@value >> 8);
        bytes[startIndex + 2] = (byte)(@value >> 16);
        bytes[startIndex + 3] = (byte)(@value >> 24);
        bytes[startIndex + 4] = (byte)(@value >> 32);
        bytes[startIndex + 5] = (byte)(@value >> 40);
        bytes[startIndex + 6] = (byte)(@value >> 48);
        bytes[startIndex + 7] = (byte)(@value >> 56);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytesBE(UInt64 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 8);

      unchecked {
        bytes[startIndex    ] = (byte)(@value >> 56);
        bytes[startIndex + 1] = (byte)(@value >> 48);
        bytes[startIndex + 2] = (byte)(@value >> 40);
        bytes[startIndex + 3] = (byte)(@value >> 32);
        bytes[startIndex + 4] = (byte)(@value >> 24);
        bytes[startIndex + 5] = (byte)(@value >> 16);
        bytes[startIndex + 6] = (byte)(@value >> 8);
        bytes[startIndex + 7] = (byte)(@value);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt64 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      switch (endian) {
        case Endianness.LittleEndian: GetBytesLE(@value, bytes, startIndex); break;
        case Endianness.BigEndian:    GetBytesBE(@value, bytes, startIndex); break;
        default:
          throw GetUnsupportedEndianException(endian);
      }
    }

    public static byte[] GetBytesLE(Int16 @value)
    {
      var bytes = new byte[2];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesBE(Int16 @value)
    {
      var bytes = new byte[2];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(Int16 @value, Endianness endian)
    {
      var bytes = new byte[2];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesLE(UInt16 @value)
    {
      var bytes = new byte[2];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesBE(UInt16 @value)
    {
      var bytes = new byte[2];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytes(UInt16 @value, Endianness endian)
    {
      var bytes = new byte[2];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesLE(Int32 @value)
    {
      var bytes = new byte[4];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesBE(Int32 @value)
    {
      var bytes = new byte[4];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(Int32 @value, Endianness endian)
    {
      var bytes = new byte[4];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesLE(UInt32 @value)
    {
      var bytes = new byte[4];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesBE(UInt32 @value)
    {
      var bytes = new byte[4];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytes(UInt32 @value, Endianness endian)
    {
      var bytes = new byte[4];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesLE(Int64 @value)
    {
      var bytes = new byte[8];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesBE(Int64 @value)
    {
      var bytes = new byte[8];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(Int64 @value, Endianness endian)
    {
      var bytes = new byte[8];

      GetBytes(@value, endian, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesLE(UInt64 @value)
    {
      var bytes = new byte[8];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesBE(UInt64 @value)
    {
      var bytes = new byte[8];

      GetBytesBE(@value, bytes, 0);

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

