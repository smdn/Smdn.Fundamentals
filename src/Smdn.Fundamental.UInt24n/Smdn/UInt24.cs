// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.InteropServices;

namespace Smdn;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
[StructLayout(LayoutKind.Explicit, Pack = 1)]
#pragma warning disable IDE0055
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
#pragma warning restore IDE0055

  // big endian
  [FieldOffset(0)] public byte Byte0; // 0x 00ff0000
  [FieldOffset(1)] public byte Byte1; // 0x 0000ff00
  [FieldOffset(2)] public byte Byte2; // 0x 000000ff

  private const int maxValue = 0xffffff;
  private const int minValue = 0x000000;

  public static readonly UInt24 MaxValue = (UInt24)maxValue;
  public static readonly UInt24 MinValue = (UInt24)minValue;
  public static readonly UInt24 Zero     = (UInt24)0;

  public UInt24(byte[] value, int startIndex, bool isBigEndian = false)
  {
    const int sizeOfUInt24 = 3;

    if (value == null)
      throw new ArgumentNullException(nameof(value));
    if (startIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
    if (value.Length - sizeOfUInt24 < startIndex)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(startIndex), value, startIndex, sizeOfUInt24);

    if (isBigEndian) {
      Byte0 = value[startIndex + 0];
      Byte1 = value[startIndex + 1];
      Byte2 = value[startIndex + 2];
    }
    else {
      Byte0 = value[startIndex + 2];
      Byte1 = value[startIndex + 1];
      Byte2 = value[startIndex + 0];
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
    if (val is < minValue or > maxValue)
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

  public static explicit operator short(UInt24 val) => checked((short)val.ToInt32());

  [CLSCompliant(false)]
  public static explicit operator ushort(UInt24 val) => checked((ushort)val.ToUInt32());

  public static explicit operator int(UInt24 val) => val.ToInt32();

  [CLSCompliant(false)]
  public static explicit operator uint(UInt24 val) => val.ToUInt32();

  public Int32 ToInt32() => unchecked((Int32)ToUInt32());

  [CLSCompliant(false)]
  public UInt32 ToUInt32()
    => (UInt32)(Byte0 << 16) |
       (UInt32)(Byte1 << 8) |
       (UInt32)Byte2;

  TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

  string IConvertible.ToString(IFormatProvider provider) => ToString(null, provider);
  byte IConvertible.ToByte(IFormatProvider provider) => checked((byte)ToUInt32());
  ushort IConvertible.ToUInt16(IFormatProvider provider) => checked((ushort)ToUInt32());
  uint IConvertible.ToUInt32(IFormatProvider provider) => ToUInt32();

  ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)ToUInt32();
  sbyte IConvertible.ToSByte(IFormatProvider provider) => checked((sbyte)ToInt32());
  short IConvertible.ToInt16(IFormatProvider provider) => checked((short)ToInt32());
  int IConvertible.ToInt32(IFormatProvider provider) => ToInt32();

  long IConvertible.ToInt64(IFormatProvider provider) => (long)ToInt32();
  bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(ToUInt32());
  char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(ToUInt32());

  DateTime IConvertible.ToDateTime(IFormatProvider provider) => ((IConvertible)ToUInt32()).ToDateTime(provider);

  decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(ToUInt32());

  double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(ToUInt32());
  float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(ToUInt32());

  object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    => Convert.ChangeType(ToUInt32(), conversionType, provider);

  public int CompareTo(object obj)
  {
    if (obj == null)
      return 1;
    else if (obj is UInt24 valUInt24)
      return CompareTo(valUInt24);
    else if (obj is uint valUInt)
      return CompareTo(valUInt);
    else if (obj is int valInt)
      return CompareTo(valInt);
    else
      throw new ArgumentException("ojb is not UInt24", nameof(obj));
  }

  public int CompareTo(UInt24 other) => this.ToUInt32().CompareTo(other.ToUInt32());

  [CLSCompliant(false)]
  public int CompareTo(uint other) => this.ToUInt32().CompareTo(other);

  public int CompareTo(int other) => this.ToInt32().CompareTo(other);

  public static bool operator <(UInt24 x, UInt24 y) => x.ToUInt32() < y.ToUInt32();
  public static bool operator <=(UInt24 x, UInt24 y) => x.ToUInt32() <= y.ToUInt32();
  public static bool operator >(UInt24 x, UInt24 y) => x.ToUInt32() > y.ToUInt32();
  public static bool operator >=(UInt24 x, UInt24 y) => x.ToUInt32() >= y.ToUInt32();

  public bool Equals(UInt24 other) => this == other;

  [CLSCompliant(false)]
  public bool Equals(uint other) => this.ToUInt32() == other;

  public bool Equals(int other) => this.ToInt32() == other;

  public override bool Equals(object obj)
  {
    if (obj is UInt24 valUInt24)
      return Equals(valUInt24);
    else if (obj is uint valUInt)
      return Equals(valUInt);
    else if (obj is int valInt)
      return Equals(valInt);
    else
      return false;
  }

  public static bool operator ==(UInt24 x, UInt24 y)
  {
    return (x.Byte0 == y.Byte0 &&
            x.Byte1 == y.Byte1 &&
            x.Byte2 == y.Byte2);
  }

  public static bool operator !=(UInt24 x, UInt24 y)
  {
    return (x.Byte0 != y.Byte0 ||
            x.Byte1 != y.Byte1 ||
            x.Byte2 != y.Byte2);
  }

  public override int GetHashCode() => (Byte2 << 16) | (Byte1 << 8) | Byte0;

  public override string ToString() => ToString(null, null);
  public string ToString(string format) => ToString(format, null);
  public string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);
  public string ToString(string format, IFormatProvider formatProvider)
    => ToUInt32().ToString(format, formatProvider);
}
