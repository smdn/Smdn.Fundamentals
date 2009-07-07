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
using System.Runtime.InteropServices;

namespace Smdn {
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct UInt48 :
    IEquatable<UInt48>,
    IComparable,
    IComparable<UInt48>
  {
    // big endian
    [FieldOffset(0)] public byte Byte0; // 0x 0000ff00 00000000
    [FieldOffset(1)] public byte Byte1; // 0x 000000ff 00000000
    [FieldOffset(2)] public byte Byte2; // 0x 00000000 ff000000
    [FieldOffset(3)] public byte Byte3; // 0x 00000000 00ff0000
    [FieldOffset(4)] public byte Byte4; // 0x 00000000 0000ff00
    [FieldOffset(5)] public byte Byte5; // 0x 00000000 000000ff

    private const long maxValue = 0xffffffffffff;
    private const long minValue = 0x000000000000;

    public static readonly UInt48 MaxValue = (UInt48)maxValue;
    public static readonly UInt48 MinValue = (UInt48)minValue;

    internal UInt48(byte[] bytes, bool bigEndian)
    {
      if (bigEndian) {
        Byte0 = bytes[0];
        Byte1 = bytes[1];
        Byte2 = bytes[2];
        Byte3 = bytes[3];
        Byte4 = bytes[4];
        Byte5 = bytes[5];
      }
      else {
        Byte0 = bytes[5];
        Byte1 = bytes[4];
        Byte2 = bytes[3];
        Byte3 = bytes[2];
        Byte4 = bytes[1];
        Byte5 = bytes[0];
      }
    }

    public static explicit operator UInt48(ulong val)
    {
      if (maxValue < val)
        throw new OverflowException();

      var uint48 = new UInt48();

      uint48.Byte0 = (byte)((val & 0x0000ff0000000000) >> 40);
      uint48.Byte1 = (byte)((val & 0x000000ff00000000) >> 32);
      uint48.Byte2 = (byte)((val & 0x00000000ff000000) >> 24);
      uint48.Byte3 = (byte)((val & 0x0000000000ff0000) >> 16);
      uint48.Byte4 = (byte)((val & 0x000000000000ff00) >> 8);
      uint48.Byte5 = (byte)((val & 0x00000000000000ff));

      return uint48;
    }

    public static explicit operator UInt48(long val)
    {
      if (val < minValue || maxValue < val)
        throw new OverflowException();

      var uint48 = new UInt48();

      uint48.Byte0 = (byte)((val & 0x0000ff0000000000) >> 40);
      uint48.Byte1 = (byte)((val & 0x000000ff00000000) >> 32);
      uint48.Byte2 = (byte)((val & 0x00000000ff000000) >> 24);
      uint48.Byte3 = (byte)((val & 0x0000000000ff0000) >> 16);
      uint48.Byte4 = (byte)((val & 0x000000000000ff00) >> 8);
      uint48.Byte5 = (byte)((val & 0x00000000000000ff));

      return uint48;
    }

    public static explicit operator UInt48(uint val)
    {
      var uint48 = new UInt48();

      uint48.Byte0 = 0;
      uint48.Byte1 = 0;
      uint48.Byte2 = (byte)((val & 0x00000000ff000000) >> 24);
      uint48.Byte3 = (byte)((val & 0x0000000000ff0000) >> 16);
      uint48.Byte4 = (byte)((val & 0x000000000000ff00) >> 8);
      uint48.Byte5 = (byte)((val & 0x00000000000000ff));

      return uint48;
    }

    public static explicit operator UInt48(int val)
    {
      if (val < minValue)
        throw new OverflowException();

      var uint48 = new UInt48();

      uint48.Byte0 = 0;
      uint48.Byte1 = 0;
      uint48.Byte2 = (byte)((val & 0x000000007f000000) >> 24);
      uint48.Byte3 = (byte)((val & 0x0000000000ff0000) >> 16);
      uint48.Byte4 = (byte)((val & 0x000000000000ff00) >> 8);
      uint48.Byte5 = (byte)((val & 0x00000000000000ff));

      return uint48;
    }

    public Int64 ToInt64()
    {
      return (Int64)ToUInt64();
    }

    public UInt64 ToUInt64()
    {
      return ((UInt64)Byte0 << 40 |
              (UInt64)Byte1 << 32 |
              (UInt64)Byte2 << 24 |
              (UInt64)Byte3 << 16 |
              (UInt64)Byte4 << 8 |
              (UInt64)Byte5);
    }

    public byte[] ToBigEndianByteArray()
    {
      return new[] {
        Byte0,
        Byte1,
        Byte2,
        Byte3,
        Byte4,
        Byte5,
      };
    }

    public byte[] ToLittleEndianByteArray()
    {
      return new[] {
        Byte5,
        Byte4,
        Byte3,
        Byte2,
        Byte1,
        Byte0,
      };
    }

    public int CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      else if (obj is UInt48)
        return CompareTo((UInt48)obj);
      else
        throw new ArgumentException("ojb is not UInt48");
    }

    public int CompareTo(UInt48 other)
    {
      return this.ToUInt64().CompareTo(other.ToUInt64());
    }

    public bool Equals(UInt48 other)
    {
      return (this.Byte0 == other.Byte0 &&
              this.Byte1 == other.Byte1 &&
              this.Byte2 == other.Byte2 &&
              this.Byte3 == other.Byte3 &&
              this.Byte4 == other.Byte4 &&
              this.Byte5 == other.Byte5);
    }

    public override bool Equals(object obj)
    {
      if (obj is UInt48)
        return Equals((UInt48)obj);
      else
        return false;
    }

    public override int GetHashCode()
    {
      return (Byte3 << 24 | Byte2 << 16 | Byte1 << 8 | Byte0) ^ (Byte5 << 8 | Byte4);
    }

    public override string ToString()
    {
      return ToUInt64().ToString();
    }
  }
}
