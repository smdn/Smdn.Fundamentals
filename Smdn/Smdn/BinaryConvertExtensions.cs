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
  public class BinaryConvertExtensions : BinaryConvert {
    protected BinaryConvertExtensions() {}

    public static UInt24 ToUInt24LE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt24LE(@value, startIndex);
    }

    public static UInt24 ToUInt24BE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt24BE(@value, startIndex);
    }

    public static UInt24 ToUInt24(byte[] @value, int startIndex, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt24(@value, startIndex, endian);
    }

    public static UInt48 ToUInt48LE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt48LE(@value, startIndex);
    }

    public static UInt48 ToUInt48BE(byte[] @value, int startIndex)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt48BE(@value, startIndex);
    }

    public static UInt48 ToUInt48(byte[] @value, int startIndex, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.ToUInt48(@value, startIndex, endian);
    }

    public static void GetBytesLE(UInt24 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value, bytes, startIndex);
    }

    public static void GetBytesBE(UInt24 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value, bytes, startIndex);
    }

    public static void GetBytes(UInt24 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian, bytes, startIndex);
    }

    public static void GetBytesLE(UInt48 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value, bytes, startIndex);
    }

    public static void GetBytesBE(UInt48 @value, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value, bytes, startIndex);
    }

    public static void GetBytes(UInt48 @value, Endianness endian, byte[] bytes, int startIndex)
    {
      Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian, bytes, startIndex);
    }

    public static byte[] GetBytesLE(UInt24 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value);
    }

    public static byte[] GetBytesBE(UInt24 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value);
    }

    public static byte[] GetBytes(UInt24 @value, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian);
    }

    public static byte[] GetBytesLE(UInt48 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesLE(@value);
    }

    public static byte[] GetBytesBE(UInt48 @value)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytesBE(@value);
    }

    public static byte[] GetBytes(UInt48 @value, Endianness endian)
    {
      return Smdn.IO.Binary.BinaryConversion.GetBytes(@value, endian);
    }
  }
}