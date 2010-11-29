// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2010 smdn
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
    IEquatable<uint>,
    IEquatable<int>,
    IComparable,
    IComparable<UInt24>,
    IComparable<uint>,
    IComparable<int>,
    IConvertible,
    IFormattable
  {
    // big endian
    [FieldOffset(0)] public byte Byte0; // 0x 00ff0000
    [FieldOffset(1)] public byte Byte1; // 0x 0000ff00
    [FieldOffset(2)] public byte Byte2; // 0x 000000ff

    private const int maxValue = 0xffffff;
    private const int minValue = 0x000000;

    public static readonly UInt24 MaxValue = (UInt24)maxValue;
    public static readonly UInt24 MinValue = (UInt24)minValue;
    public static readonly UInt24 Zero     = (UInt24)0;

    internal UInt24(byte[] bytes, int startIndex, bool bigEndian)
    {
      if (bigEndian) {
        Byte0 = bytes[startIndex + 0];
        Byte1 = bytes[startIndex + 1];
        Byte2 = bytes[startIndex + 2];
      }
      else {
        Byte0 = bytes[startIndex + 2];
        Byte1 = bytes[startIndex + 1];
        Byte2 = bytes[startIndex + 0];
      }
    }

    [CLSCompliant(false)]
    public static explicit operator UInt24(uint val)
    {
      if (maxValue < val)
        throw new OverflowException();

      var uint24 = new UInt24();

      unchecked {
        uint24.Byte0 = (byte)(val >> 16);
        uint24.Byte1 = (byte)(val >> 8);
        uint24.Byte2 = (byte)(val);
      }

      return uint24;
    }

    public static explicit operator UInt24(int val)
    {
      if (val < minValue || maxValue < val)
        throw new OverflowException();

      var uint24 = new UInt24();

      unchecked {
        uint24.Byte0 = (byte)(val >> 16);
        uint24.Byte1 = (byte)(val >> 8);
        uint24.Byte2 = (byte)(val);
      }

      return uint24;
    }

    [CLSCompliant(false)]
    public static explicit operator UInt24(ushort val)
    {
      var uint24 = new UInt24();

      unchecked {
        uint24.Byte0 = 0;
        uint24.Byte1 = (byte)(val >> 8);
        uint24.Byte2 = (byte)(val);
      }

      return uint24;
    }

    public static explicit operator UInt24(short val)
    {
      if (val < minValue)
        throw new OverflowException();

      var uint24 = new UInt24();

      unchecked {
        uint24.Byte0 = 0;
        uint24.Byte1 = (byte)(val >> 8);
        uint24.Byte2 = (byte)(val);
      }

      return uint24;
    }

    public static explicit operator short(UInt24 val)
    {
      return checked((short)val.ToInt32());
    }

    [CLSCompliant(false)]
    public static explicit operator ushort(UInt24 val)
    {
      return checked((ushort)val.ToUInt32());
    }

    public static explicit operator int(UInt24 val)
    {
      return val.ToInt32();
    }

    [CLSCompliant(false)]
    public static explicit operator uint(UInt24 val)
    {
      return val.ToUInt32();
    }

    public Int32 ToInt32()
    {
      return unchecked((Int32)ToUInt32());
    }

    [CLSCompliant(false)]
    public UInt32 ToUInt32()
    {
      return ((UInt32)Byte0 << 16 |
              (UInt32)Byte1 << 8 |
              (UInt32)Byte2);
    }

#region "IConvertible implementation"
    TypeCode IConvertible.GetTypeCode()
    {
      return TypeCode.Object;
    }

    string IConvertible.ToString(IFormatProvider provider)
    {
      return ToString(null, provider);
    }

    byte IConvertible.ToByte(IFormatProvider provider)
    {
      return checked((byte)ToUInt32());
    }

    ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
      return checked((ushort)ToUInt32());
    }

    uint IConvertible.ToUInt32(IFormatProvider provider)
    {
      return ToUInt32();
    }

    ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
      return (ulong)ToUInt32();
    }

    sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
      return checked((sbyte)ToInt32());
    }

    short IConvertible.ToInt16(IFormatProvider provider)
    {
      return checked((short)ToInt32());
    }

    int IConvertible.ToInt32(IFormatProvider provider)
    {
      return ToInt32();
    }

    long IConvertible.ToInt64(IFormatProvider provider)
    {
      return (long)ToInt32();
    }

    bool IConvertible.ToBoolean(IFormatProvider provider)
    {
      return Convert.ToBoolean(ToUInt32());
    }

    char IConvertible.ToChar(IFormatProvider provider)
    {
      return Convert.ToChar(ToUInt32());
    }

    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
      return Convert.ToDateTime(ToUInt32());
    }

    decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
      return Convert.ToDecimal(ToUInt32());
    }

    double IConvertible.ToDouble(IFormatProvider provider)
    {
      return Convert.ToDouble(ToUInt32());
    }

    float IConvertible.ToSingle(IFormatProvider provider)
    {
      return Convert.ToSingle(ToUInt32());
    }

    object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    {
      return Convert.ChangeType(ToUInt32(), conversionType, provider);
    }
#endregion

    public int CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      else if (obj is UInt24)
        return CompareTo((UInt24)obj);
      else if (obj is uint)
        return CompareTo((uint)obj);
      else if (obj is int)
        return CompareTo((int)obj);
      else
        throw new ArgumentException("ojb is not UInt24");
    }

    public int CompareTo(UInt24 other)
    {
      return this.ToUInt32().CompareTo(other.ToUInt32());
    }

    [CLSCompliant(false)]
    public int CompareTo(uint other)
    {
      return this.ToUInt32().CompareTo(other);
    }

    public int CompareTo(int other)
    {
      return this.ToInt32().CompareTo(other);
    }

    public bool Equals(UInt24 other)
    {
      return this == other;
    }

    [CLSCompliant(false)]
    public bool Equals(uint other)
    {
      return this.ToUInt32() == other;
    }

    public bool Equals(int other)
    {
      return this.ToInt32() == other;
    }

    public override bool Equals(object obj)
    {
      if (obj is UInt24)
        return Equals((UInt24)obj);
      else if (obj is uint)
        return Equals((uint)obj);
      else if (obj is int)
        return Equals((int)obj);
      else
        return false;
    }

    public static bool operator == (UInt24 x, UInt24 y)
    {
      return (x.Byte0 == y.Byte0 &&
              x.Byte1 == y.Byte1 &&
              x.Byte2 == y.Byte2);
    }

    public static bool operator != (UInt24 x, UInt24 y)
    {
      return (x.Byte0 != y.Byte0 ||
              x.Byte1 != y.Byte1 ||
              x.Byte2 != y.Byte2);
    }

    public override int GetHashCode()
    {
      return (Byte2 << 16 | Byte1 << 8 | Byte0);
    }

    public override string ToString()
    {
      return ToString(null, null);
    }

    public string ToString(string format)
    {
      return ToString(format, null);
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return ToString(null, formatProvider);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      return ToUInt32().ToString(format, formatProvider);
    }
  }
}
