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
  public struct UInt24 :
    IEquatable<UInt24>,
    IComparable,
    IComparable<UInt24>,
    IConvertible
  {
    // big endian
    [FieldOffset(0)] public byte Byte0; // 0x 00ff0000
    [FieldOffset(1)] public byte Byte1; // 0x 0000ff00
    [FieldOffset(2)] public byte Byte2; // 0x 000000ff

    private const int maxValue = 0xffffff;
    private const int minValue = 0x000000;

    public static readonly UInt24 MaxValue = (UInt24)maxValue;
    public static readonly UInt24 MinValue = (UInt24)minValue;

    internal UInt24(byte[] bytes, bool bigEndian)
    {
      if (bigEndian) {
        Byte0 = bytes[0];
        Byte1 = bytes[1];
        Byte2 = bytes[2];
      }
      else {
        Byte0 = bytes[0];
        Byte1 = bytes[1];
        Byte2 = bytes[2];
      }
    }

    public static explicit operator UInt24(uint val)
    {
      if (maxValue < val)
        throw new OverflowException();

      var uint24 = new UInt24();

      uint24.Byte0 = (byte)((val & 0x00ff0000) >> 16);
      uint24.Byte1 = (byte)((val & 0x0000ff00) >> 8);
      uint24.Byte2 = (byte)((val & 0x000000ff));

      return uint24;
    }

    public static explicit operator UInt24(int val)
    {
      if (val < minValue || maxValue < val)
        throw new OverflowException();

      var uint24 = new UInt24();

      uint24.Byte0 = (byte)((val & 0x00ff0000) >> 16);
      uint24.Byte1 = (byte)((val & 0x0000ff00) >> 8);
      uint24.Byte2 = (byte)((val & 0x000000ff));

      return uint24;
    }

    public static explicit operator UInt24(ushort val)
    {
      var uint24 = new UInt24();

      uint24.Byte0 = 0;
      uint24.Byte1 = (byte)((val & 0x0000ff00) >> 8);
      uint24.Byte2 = (byte)((val & 0x000000ff));

      return uint24;
    }

    public static explicit operator UInt24(short val)
    {
      if (val < minValue)
        throw new OverflowException();

      var uint24 = new UInt24();

      uint24.Byte0 = 0;
      uint24.Byte1 = (byte)((val & 0x00007f00) >> 8);
      uint24.Byte2 = (byte)((val & 0x000000ff));

      return uint24;
    }

    public static implicit operator short(UInt24 val)
    {
      return (short)val.ToInt32();
    }

    public static implicit operator ushort(UInt24 val)
    {
      return (ushort)val.ToUInt32();
    }

    public static implicit operator int(UInt24 val)
    {
      return val.ToInt32();
    }

    public static implicit operator uint(UInt24 val)
    {
      return val.ToUInt32();
    }

    public Int32 ToInt32()
    {
      return (Int32)ToUInt32();
    }

    public UInt32 ToUInt32()
    {
      return ((UInt32)Byte0 << 16 |
              (UInt32)Byte1 << 8 |
              (UInt32)Byte2);
    }

    public byte[] ToBigEndianByteArray()
    {
      return new[] {
        Byte0,
        Byte1,
        Byte2,
      };
    }

    public byte[] ToLittleEndianByteArray()
    {
      return new[] {
        Byte2,
        Byte1,
        Byte0,
      };
    }

#region "IConvertible implementation"
    TypeCode IConvertible.GetTypeCode()
    {
      return TypeCode.Object;
    }

    bool IConvertible.ToBoolean(IFormatProvider provider)
    {
      return Convert.ToBoolean(ToInt32());
    }

    byte IConvertible.ToByte(IFormatProvider provider)
    {
      return Convert.ToByte(ToInt32());
    }

    char IConvertible.ToChar(IFormatProvider provider)
    {
      return Convert.ToChar(ToInt32());
    }

    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
      return Convert.ToDateTime(ToInt32());
    }

    decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
      return Convert.ToDecimal(ToInt32());
    }

    double IConvertible.ToDouble(IFormatProvider provider)
    {
      return Convert.ToDouble(ToInt32());
    }

    short IConvertible.ToInt16(IFormatProvider provider)
    {
      return Convert.ToInt16(ToInt32());
    }

    int IConvertible.ToInt32(IFormatProvider provider)
    {
      return ToInt32();
    }

    long IConvertible.ToInt64(IFormatProvider provider)
    {
      return Convert.ToInt64(ToInt32());
    }

    sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
      return Convert.ToSByte(ToInt32());
    }

    float IConvertible.ToSingle(IFormatProvider provider)
    {
      return Convert.ToSingle(ToInt32());
    }

    string IConvertible.ToString(IFormatProvider provider)
    {
      return Convert.ToString(ToInt32(), provider);
    }

    object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    {
      return Convert.ChangeType(ToInt32(), conversionType, provider);
    }

    ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
      return Convert.ToUInt16(ToUInt32());
    }

    uint IConvertible.ToUInt32(IFormatProvider provider)
    {
      return ToUInt32();
    }

    ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
      return Convert.ToUInt64(ToUInt32());
    }
#endregion

    public int CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      else if (obj is UInt24)
        return CompareTo((UInt24)obj);
      else
        throw new ArgumentException("ojb is not UInt24");
    }

    public int CompareTo(UInt24 other)
    {
      return this.ToUInt32().CompareTo(other.ToUInt32());
    }

    public bool Equals(UInt24 other)
    {
      return (this.Byte0 == other.Byte0 &&
              this.Byte1 == other.Byte1 &&
              this.Byte2 == other.Byte2);
    }

    public override bool Equals(object obj)
    {
      if (obj is UInt24)
        return Equals((UInt24)obj);
      else
        return false;
    }

    public override int GetHashCode()
    {
      return (Byte2 << 16 | Byte1 << 8 | Byte0);
    }

    public override string ToString()
    {
      return ToUInt32().ToString();
    }
  }
}
