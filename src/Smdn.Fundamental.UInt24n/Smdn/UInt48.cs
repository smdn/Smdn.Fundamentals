// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.InteropServices;

namespace Smdn;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
[StructLayout(LayoutKind.Explicit, Pack = 1)]
#pragma warning disable IDE0055
public struct UInt48 :
  IEquatable<UInt48>,
  IEquatable<ulong>,
  IEquatable<long>,
  IComparable,
  IComparable<UInt48>,
  IComparable<ulong>,
  IComparable<long>,
  IConvertible,
  IFormattable
{
#pragma warning restore IDE0055

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

  public UInt48(byte[] value, int startIndex, bool isBigEndian = false)
  {
    const int sizeOfUInt48 = 6;

    if (value == null)
      throw new ArgumentNullException(nameof(value));
    if (startIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
    if (value.Length - sizeOfUInt48 < startIndex)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(startIndex), value, startIndex, sizeOfUInt48);

    if (isBigEndian) {
      Byte0 = value[startIndex + 0];
      Byte1 = value[startIndex + 1];
      Byte2 = value[startIndex + 2];
      Byte3 = value[startIndex + 3];
      Byte4 = value[startIndex + 4];
      Byte5 = value[startIndex + 5];
    }
    else {
      Byte0 = value[startIndex + 5];
      Byte1 = value[startIndex + 4];
      Byte2 = value[startIndex + 3];
      Byte3 = value[startIndex + 2];
      Byte4 = value[startIndex + 1];
      Byte5 = value[startIndex + 0];
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
    if (val is < minValue or > maxValue)
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
  public static explicit operator UInt48(uint val)
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

  public static explicit operator int(UInt48 val) => checked((int)val.ToInt64());

  [CLSCompliant(false)]
  public static explicit operator uint(UInt48 val) => checked((uint)val.ToUInt64());

  public static explicit operator long(UInt48 val) => val.ToInt64();

  [CLSCompliant(false)]
  public static explicit operator ulong(UInt48 val) => val.ToUInt64();

  public Int64 ToInt64() => unchecked((Int64)ToUInt64());

  [CLSCompliant(false)]
  public UInt64 ToUInt64()
    => (UInt64)((UInt64)Byte0 << 40) |
       (UInt64)((UInt64)Byte1 << 32) |
       (UInt64)((UInt64)Byte2 << 24) |
       (UInt64)((UInt64)Byte3 << 16) |
       (UInt64)((UInt64)Byte4 << 8) |
       (UInt64)Byte5;

  TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

  string IConvertible.ToString(IFormatProvider provider) => ToString(null, provider);

  byte IConvertible.ToByte(IFormatProvider provider) => checked((byte)ToUInt64());

  ushort IConvertible.ToUInt16(IFormatProvider provider) => checked((ushort)ToUInt64());

  uint IConvertible.ToUInt32(IFormatProvider provider) => checked((uint)ToUInt64());

  ulong IConvertible.ToUInt64(IFormatProvider provider) => ToUInt64();

  sbyte IConvertible.ToSByte(IFormatProvider provider) => checked((sbyte)ToInt64());

  short IConvertible.ToInt16(IFormatProvider provider) => checked((short)ToInt64());

  int IConvertible.ToInt32(IFormatProvider provider) => checked((int)ToInt64());

  long IConvertible.ToInt64(IFormatProvider provider) => ToInt64();

  bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(ToUInt64());

  char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(ToUInt64());

  DateTime IConvertible.ToDateTime(IFormatProvider provider) => ((IConvertible)ToUInt64()).ToDateTime(provider);

  decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(ToUInt64());

  double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(ToUInt64());

  float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(ToUInt64());

  object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(ToUInt64(), conversionType, provider);

  public int CompareTo(object obj)
  {
    if (obj == null)
      return 1;
    else if (obj is UInt48 valUInt48)
      return CompareTo(valUInt48);
    else if (obj is ulong valULong)
      return CompareTo(valULong);
    else if (obj is long valLong)
      return CompareTo(valLong);
    else
      throw new ArgumentException("ojb is not UInt48", nameof(obj));
  }

  public int CompareTo(UInt48 other) => this.ToUInt64().CompareTo(other.ToUInt64());

  [CLSCompliant(false)]
  public int CompareTo(ulong other) => this.ToUInt64().CompareTo(other);

  public int CompareTo(long other) => this.ToInt64().CompareTo(other);

  public static bool operator <(UInt48 x, UInt48 y) => x.ToUInt64() < y.ToUInt64();
  public static bool operator <=(UInt48 x, UInt48 y) => x.ToUInt64() <= y.ToUInt64();
  public static bool operator >(UInt48 x, UInt48 y) => x.ToUInt64() > y.ToUInt64();
  public static bool operator >=(UInt48 x, UInt48 y) => x.ToUInt64() >= y.ToUInt64();

  public bool Equals(UInt48 other) => this == other;

  [CLSCompliant(false)]
  public bool Equals(ulong other) => this.ToUInt64() == other;

  public bool Equals(long other) => this.ToInt64() == other;

  public override bool Equals(object obj)
  {
    if (obj is UInt48 valUInt48)
      return Equals(valUInt48);
    else if (obj is ulong valULong)
      return Equals(valULong);
    else if (obj is long valLong)
      return Equals(valLong);
    else
      return false;
  }

  public static bool operator ==(UInt48 x, UInt48 y)
  {
    return (x.Byte0 == y.Byte0 &&
            x.Byte1 == y.Byte1 &&
            x.Byte2 == y.Byte2 &&
            x.Byte3 == y.Byte3 &&
            x.Byte4 == y.Byte4 &&
            x.Byte5 == y.Byte5);
  }

  public static bool operator !=(UInt48 x, UInt48 y)
  {
    return (x.Byte0 != y.Byte0 ||
            x.Byte1 != y.Byte1 ||
            x.Byte2 != y.Byte2 ||
            x.Byte3 != y.Byte3 ||
            x.Byte4 != y.Byte4 ||
            x.Byte5 != y.Byte5);
  }

  public override int GetHashCode()
    => ((Byte3 << 24) | (Byte2 << 16) | (Byte1 << 8) | Byte0) ^ ((Byte5 << 8) | Byte4);

  public override string ToString() => ToString(null, null);
  public string ToString(string format) => ToString(format, null);
  public string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);
  public string ToString(string format, IFormatProvider formatProvider)
    => ToUInt64().ToString(format, formatProvider);
}
