// Smdn.Fundamental.UInt24n.dll (Smdn.Fundamental.UInt24n-3.0.0 (netstandard2.1))
//   Name: Smdn.Fundamental.UInt24n
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard2.1)
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

using System;
using Smdn;

namespace Smdn {
  // Forwarded to "Smdn.Fundamental.UInt24n, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct UInt24 :
    IComparable,
    IComparable<UInt24>,
    IComparable<int>,
    IComparable<uint>,
    IConvertible,
    IEquatable<UInt24>,
    IEquatable<int>,
    IEquatable<uint>,
    IFormattable
  {
    [FieldOffset(0)]
    public byte Byte0;
    [FieldOffset(1)]
    public byte Byte1;
    [FieldOffset(2)]
    public byte Byte2;
    public static readonly UInt24 MaxValue; // = "16777215"
    public static readonly UInt24 MinValue; // = "0"
    public static readonly UInt24 Zero; // = "0"

    public UInt24(byte[] @value, int startIndex, bool isBigEndian = false) {}

    public int CompareTo(UInt24 other) {}
    public int CompareTo(int other) {}
    public int CompareTo(object obj) {}
    public int CompareTo(uint other) {}
    public bool Equals(UInt24 other) {}
    public bool Equals(int other) {}
    public bool Equals(uint other) {}
    public override bool Equals(object obj) {}
    public override int GetHashCode() {}
    TypeCode IConvertible.GetTypeCode() {}
    bool IConvertible.ToBoolean(IFormatProvider provider) {}
    byte IConvertible.ToByte(IFormatProvider provider) {}
    char IConvertible.ToChar(IFormatProvider provider) {}
    DateTime IConvertible.ToDateTime(IFormatProvider provider) {}
    decimal IConvertible.ToDecimal(IFormatProvider provider) {}
    double IConvertible.ToDouble(IFormatProvider provider) {}
    short IConvertible.ToInt16(IFormatProvider provider) {}
    int IConvertible.ToInt32(IFormatProvider provider) {}
    long IConvertible.ToInt64(IFormatProvider provider) {}
    sbyte IConvertible.ToSByte(IFormatProvider provider) {}
    float IConvertible.ToSingle(IFormatProvider provider) {}
    string IConvertible.ToString(IFormatProvider provider) {}
    object IConvertible.ToType(Type conversionType, IFormatProvider provider) {}
    ushort IConvertible.ToUInt16(IFormatProvider provider) {}
    uint IConvertible.ToUInt32(IFormatProvider provider) {}
    ulong IConvertible.ToUInt64(IFormatProvider provider) {}
    public int ToInt32() {}
    public override string ToString() {}
    public string ToString(IFormatProvider formatProvider) {}
    public string ToString(string format) {}
    public string ToString(string format, IFormatProvider formatProvider) {}
    public uint ToUInt32() {}
    public static bool operator == (UInt24 x, UInt24 y) {}
    public static explicit operator UInt24(int val) {}
    public static explicit operator UInt24(short val) {}
    public static explicit operator UInt24(uint val) {}
    public static explicit operator UInt24(ushort val) {}
    public static explicit operator int(UInt24 val) {}
    public static explicit operator short(UInt24 val) {}
    public static explicit operator uint(UInt24 val) {}
    public static explicit operator ushort(UInt24 val) {}
    public static bool operator != (UInt24 x, UInt24 y) {}
  }

  // Forwarded to "Smdn.Fundamental.UInt24n, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct UInt48 :
    IComparable,
    IComparable<UInt48>,
    IComparable<long>,
    IComparable<ulong>,
    IConvertible,
    IEquatable<UInt48>,
    IEquatable<long>,
    IEquatable<ulong>,
    IFormattable
  {
    [FieldOffset(0)]
    public byte Byte0;
    [FieldOffset(1)]
    public byte Byte1;
    [FieldOffset(2)]
    public byte Byte2;
    [FieldOffset(3)]
    public byte Byte3;
    [FieldOffset(4)]
    public byte Byte4;
    [FieldOffset(5)]
    public byte Byte5;
    public static readonly UInt48 MaxValue; // = "281474976710655"
    public static readonly UInt48 MinValue; // = "0"
    public static readonly UInt48 Zero; // = "0"

    public UInt48(byte[] @value, int startIndex, bool isBigEndian = false) {}

    public int CompareTo(UInt48 other) {}
    public int CompareTo(long other) {}
    public int CompareTo(object obj) {}
    public int CompareTo(ulong other) {}
    public bool Equals(UInt48 other) {}
    public bool Equals(long other) {}
    public bool Equals(ulong other) {}
    public override bool Equals(object obj) {}
    public override int GetHashCode() {}
    TypeCode IConvertible.GetTypeCode() {}
    bool IConvertible.ToBoolean(IFormatProvider provider) {}
    byte IConvertible.ToByte(IFormatProvider provider) {}
    char IConvertible.ToChar(IFormatProvider provider) {}
    DateTime IConvertible.ToDateTime(IFormatProvider provider) {}
    decimal IConvertible.ToDecimal(IFormatProvider provider) {}
    double IConvertible.ToDouble(IFormatProvider provider) {}
    short IConvertible.ToInt16(IFormatProvider provider) {}
    int IConvertible.ToInt32(IFormatProvider provider) {}
    long IConvertible.ToInt64(IFormatProvider provider) {}
    sbyte IConvertible.ToSByte(IFormatProvider provider) {}
    float IConvertible.ToSingle(IFormatProvider provider) {}
    string IConvertible.ToString(IFormatProvider provider) {}
    object IConvertible.ToType(Type conversionType, IFormatProvider provider) {}
    ushort IConvertible.ToUInt16(IFormatProvider provider) {}
    uint IConvertible.ToUInt32(IFormatProvider provider) {}
    ulong IConvertible.ToUInt64(IFormatProvider provider) {}
    public long ToInt64() {}
    public override string ToString() {}
    public string ToString(IFormatProvider formatProvider) {}
    public string ToString(string format) {}
    public string ToString(string format, IFormatProvider formatProvider) {}
    public ulong ToUInt64() {}
    public static bool operator == (UInt48 x, UInt48 y) {}
    public static explicit operator UInt48(int val) {}
    public static explicit operator UInt48(long val) {}
    public static explicit operator UInt48(uint val) {}
    public static explicit operator UInt48(ulong val) {}
    public static explicit operator int(UInt48 val) {}
    public static explicit operator long(UInt48 val) {}
    public static explicit operator uint(UInt48 val) {}
    public static explicit operator ulong(UInt48 val) {}
    public static bool operator != (UInt48 x, UInt48 y) {}
  }
}

