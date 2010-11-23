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
  public struct UInt48 :
    IEquatable<UInt48>,
    IComparable,
    IComparable<UInt48>,
    IConvertible,
    IFormattable
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
    public static readonly UInt48 Zero     = (UInt48)0;

    internal UInt48(byte[] bytes, int startIndex, bool bigEndian)
    {
      if (bigEndian) {
        Byte0 = bytes[startIndex + 0];
        Byte1 = bytes[startIndex + 1];
        Byte2 = bytes[startIndex + 2];
        Byte3 = bytes[startIndex + 3];
        Byte4 = bytes[startIndex + 4];
        Byte5 = bytes[startIndex + 5];
      }
      else {
        Byte0 = bytes[startIndex + 5];
        Byte1 = bytes[startIndex + 4];
        Byte2 = bytes[startIndex + 3];
        Byte3 = bytes[startIndex + 2];
        Byte4 = bytes[startIndex + 1];
        Byte5 = bytes[startIndex + 0];
      }
    }

    [CLSCompliant(false)]
    public static explicit operator UInt48(ulong val)
    {
      if (maxValue < val)
        throw new OverflowException();

      var uint48 = new UInt48();

      unchecked {
        uint48.Byte0 = (byte)(val >> 40);
        uint48.Byte1 = (byte)(val >> 32);
        uint48.Byte2 = (byte)(val >> 24);
        uint48.Byte3 = (byte)(val >> 16);
        uint48.Byte4 = (byte)(val >> 8);
        uint48.Byte5 = (byte)(val);
      }

      return uint48;
    }

    public static explicit operator UInt48(long val)
    {
      if (val < minValue || maxValue < val)
        throw new OverflowException();

      var uint48 = new UInt48();

      unchecked {
        uint48.Byte0 = (byte)(val >> 40);
        uint48.Byte1 = (byte)(val >> 32);
        uint48.Byte2 = (byte)(val >> 24);
        uint48.Byte3 = (byte)(val >> 16);
        uint48.Byte4 = (byte)(val >> 8);
        uint48.Byte5 = (byte)(val);
      }

      return uint48;
    }

    [CLSCompliant(false)]
    public static implicit operator UInt48(uint val)
    {
      var uint48 = new UInt48();

      unchecked {
        uint48.Byte0 = 0;
        uint48.Byte1 = 0;
        uint48.Byte2 = (byte)(val >> 24);
        uint48.Byte3 = (byte)(val >> 16);
        uint48.Byte4 = (byte)(val >> 8);
        uint48.Byte5 = (byte)(val);
      }

      return uint48;
    }

    public static explicit operator UInt48(int val)
    {
      if (val < minValue)
        throw new OverflowException();

      var uint48 = new UInt48();

      unchecked {
        uint48.Byte0 = 0;
        uint48.Byte1 = 0;
        uint48.Byte2 = (byte)(val >> 24);
        uint48.Byte3 = (byte)(val >> 16);
        uint48.Byte4 = (byte)(val >> 8);
        uint48.Byte5 = (byte)(val);
      }

      return uint48;
    }

    public static explicit operator int(UInt48 val)
    {
      return checked((int)val.ToInt64());
    }

    [CLSCompliant(false)]
    public static explicit operator uint(UInt48 val)
    {
      return checked((uint)val.ToUInt64());
    }

    public static implicit operator long(UInt48 val)
    {
      return val.ToInt64();
    }

    [CLSCompliant(false)]
    public static implicit operator ulong(UInt48 val)
    {
      return val.ToUInt64();
    }

    public Int64 ToInt64()
    {
      return unchecked((Int64)ToUInt64());
    }

    [CLSCompliant(false)]
    public UInt64 ToUInt64()
    {
      return ((UInt64)Byte0 << 40 |
              (UInt64)Byte1 << 32 |
              (UInt64)Byte2 << 24 |
              (UInt64)Byte3 << 16 |
              (UInt64)Byte4 << 8 |
              (UInt64)Byte5);
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
      return checked((byte)ToUInt64());
    }

    ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
      return checked((ushort)ToUInt64());
    }

    uint IConvertible.ToUInt32(IFormatProvider provider)
    {
      return checked((uint)ToUInt64());
    }

    ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
      return ToUInt64();
    }

    sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
      return checked((sbyte)ToInt64());
    }

    short IConvertible.ToInt16(IFormatProvider provider)
    {
      return checked((short)ToInt64());
    }

    int IConvertible.ToInt32(IFormatProvider provider)
    {
      return checked((int)ToInt64());
    }

    long IConvertible.ToInt64(IFormatProvider provider)
    {
      return ToInt64();
    }

    bool IConvertible.ToBoolean(IFormatProvider provider)
    {
      return Convert.ToBoolean(ToUInt64());
    }

    char IConvertible.ToChar(IFormatProvider provider)
    {
      return Convert.ToChar(ToUInt64());
    }

    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
      return Convert.ToDateTime(ToUInt64());
    }

    decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
      return Convert.ToDecimal(ToUInt64());
    }

    double IConvertible.ToDouble(IFormatProvider provider)
    {
      return Convert.ToDouble(ToUInt64());
    }

    float IConvertible.ToSingle(IFormatProvider provider)
    {
      return Convert.ToSingle(ToUInt64());
    }

    object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    {
      return Convert.ChangeType(ToUInt64(), conversionType, provider);
    }
#endregion

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
      return ToUInt64().ToString(format, formatProvider);
    }
  }
}
