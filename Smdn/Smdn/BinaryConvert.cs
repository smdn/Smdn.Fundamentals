// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010-2014 smdn
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
  [Obsolete("use Smdn.IO.Binary.BinaryConversion instead")]
  public /*static*/ abstract class BinaryConvert {
    protected BinaryConvert() {}

    public static Int16 ByteSwap(Int16 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.ByteSwap(@value);
    }

    [CLSCompliant(false)]
    public static UInt16 ByteSwap(UInt16 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.ByteSwap(@value);
    }

    public static Int32 ByteSwap(Int32 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.ByteSwap(@value);
    }

    [CLSCompliant(false)]
    public static UInt32 ByteSwap(UInt32 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.ByteSwap(@value);
    }

    public static Int64 ByteSwap(Int64 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.ByteSwap(@value);
    }

    [CLSCompliant(false)]
    public static UInt64 ByteSwap(UInt64 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.ByteSwap(@value);
    }

    public static Int16 ToInt16LE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToInt16LE(@value, startIndex);
    }

    public static Int16 ToInt16BE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToInt16BE(@value, startIndex);
    }

    public static Int16 ToInt16(byte[] @value, int startIndex, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.ToInt16(@value, startIndex, endian);
    }

    [CLSCompliant(false)]
    public static UInt16 ToUInt16LE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt16LE(@value, startIndex);
    }

    [CLSCompliant(false)]
    public static UInt16 ToUInt16BE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt16BE(@value, startIndex);
    }

    [CLSCompliant(false)]
    public static UInt16 ToUInt16(byte[] @value, int startIndex, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt16(@value, startIndex, endian);
    }

    public static Int32 ToInt32LE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToInt32LE(@value, startIndex);
    }

    public static Int32 ToInt32BE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToInt32BE(@value, startIndex);
    }

    public static Int32 ToInt32(byte[] @value, int startIndex, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.ToInt32(@value, startIndex, endian);
    }

    [CLSCompliant(false)]
    public static UInt32 ToUInt32LE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt32LE(@value, startIndex);
    }

    [CLSCompliant(false)]
    public static UInt32 ToUInt32BE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt32BE(@value, startIndex);
    }

    [CLSCompliant(false)]
    public static UInt32 ToUInt32(byte[] @value, int startIndex, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt32(@value, startIndex, endian);
    }

    public static Int64 ToInt64LE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToInt64LE(@value, startIndex);
    }

    public static Int64 ToInt64BE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToInt64BE(@value, startIndex);
    }

    public static Int64 ToInt64(byte[] @value, int startIndex, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.ToInt64(@value, startIndex, endian);
    }

    [CLSCompliant(false)]
    public static UInt64 ToUInt64LE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt64LE(@value, startIndex);
    }

    [CLSCompliant(false)]
    public static UInt64 ToUInt64BE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt64BE(@value, startIndex);
    }

    [CLSCompliant(false)]
    public static UInt64 ToUInt64(byte[] @value, int startIndex, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt64(@value, startIndex, endian);
    }

    public static void GetBytesLE(Int16 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value, bytes, startIndex);
    }

    public static void GetBytesBE(Int16 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value, bytes, startIndex);
    }

    public static void GetBytes(Int16 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesLE(UInt16 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesBE(UInt16 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt16 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian, bytes, startIndex);
    }

    public static void GetBytesLE(Int32 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value, bytes, startIndex);
    }

    public static void GetBytesBE(Int32 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value, bytes, startIndex);
    }

    public static void GetBytes(Int32 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesLE(UInt32 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesBE(UInt32 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt32 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian, bytes, startIndex);
    }

    public static void GetBytesLE(Int64 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value, bytes, startIndex);
    }

    public static void GetBytesBE(Int64 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value, bytes, startIndex);
    }

    public static void GetBytes(Int64 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesLE(UInt64 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesBE(UInt64 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt64 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian, bytes, startIndex);
    }

    public static byte[] GetBytesLE(Int16 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value);
    }

    public static byte[] GetBytesBE(Int16 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value);
    }

    public static byte[] GetBytes(Int16 @value, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian);
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesLE(UInt16 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value);
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesBE(UInt16 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value);
    }

    [CLSCompliant(false)]
    public static byte[] GetBytes(UInt16 @value, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian);
    }

    public static byte[] GetBytesLE(Int32 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value);
    }

    public static byte[] GetBytesBE(Int32 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value);
    }

    public static byte[] GetBytes(Int32 @value, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian);
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesLE(UInt32 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value);
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesBE(UInt32 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value);
    }

    [CLSCompliant(false)]
    public static byte[] GetBytes(UInt32 @value, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian);
    }

    public static byte[] GetBytesLE(Int64 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value);
    }

    public static byte[] GetBytesBE(Int64 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value);
    }

    public static byte[] GetBytes(Int64 @value, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian);
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesLE(UInt64 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value);
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesBE(UInt64 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value);
    }

    [CLSCompliant(false)]
    public static byte[] GetBytes(UInt64 @value, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian);
    }
  }
}

