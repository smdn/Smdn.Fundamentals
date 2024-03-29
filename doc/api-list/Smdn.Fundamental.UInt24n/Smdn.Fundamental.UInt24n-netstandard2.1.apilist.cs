// Smdn.Fundamental.UInt24n.dll (Smdn.Fundamental.UInt24n-4.0.0)
//   Name: Smdn.Fundamental.UInt24n
//   AssemblyVersion: 4.0.0.0
//   InformationalVersion: 4.0.0+67b803a21469d1c9d14f260a6414b9e666a8e282
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     Smdn.Fundamental.Shim, Version=3.1.4.0, Culture=neutral
//     netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Smdn;

namespace Smdn {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public readonly struct UInt24 :
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
    public static readonly UInt24 MaxValue; // = "16777215"
    public static readonly UInt24 MinValue; // = "0"
    public static readonly UInt24 One; // = "1"
    public static readonly UInt24 Zero; // = "0"

    public static UInt24 Abs(UInt24 @value) {}
    public static UInt24 Clamp(UInt24 @value, UInt24 min, UInt24 max) {}
    public static (UInt24 Quotient, UInt24 Remainder) DivRem(UInt24 left, UInt24 right) {}
    public static bool IsPow2(UInt24 @value) {}
    public static int LeadingZeroCount(UInt24 @value) {}
    public static int Log2(UInt24 @value) {}
    public static UInt24 Max(UInt24 x, UInt24 y) {}
    public static UInt24 Min(UInt24 x, UInt24 y) {}
    public static UInt24 Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null) {}
    public static UInt24 Parse(string s, IFormatProvider provider = null) {}
    public static UInt24 Parse(string s, NumberStyles style, IFormatProvider provider = null) {}
    public static int PopCount(UInt24 @value) {}
    public static UInt24 RotateLeft(UInt24 @value, int rotateAmount) {}
    public static UInt24 RotateRight(UInt24 @value, int rotateAmount) {}
    public static int Sign(UInt24 @value) {}
    public static int TrailingZeroCount(UInt24 @value) {}
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out UInt24 result) {}
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out UInt24 result) {}
    public static bool TryParse(ReadOnlySpan<char> s, out UInt24 result) {}
    public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out UInt24 result) {}
    public static bool TryParse([NotNullWhen(true)] string s, NumberStyles style, IFormatProvider provider, out UInt24 result) {}
    public static bool TryParse([NotNullWhen(true)] string s, out UInt24 result) {}
    public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt24 @value) {}
    public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt24 @value) {}
    public static UInt24 operator + (UInt24 left, UInt24 right) {}
    public static UInt24 operator & (UInt24 left, UInt24 right) {}
    public static UInt24 operator | (UInt24 left, UInt24 right) {}
    public static UInt24 operator checked + (UInt24 left, UInt24 right) {}
    public static UInt24 operator checked -- (UInt24 @value) {}
    public static UInt24 operator checked / (UInt24 left, UInt24 right) {}
    public static explicit operator checked UInt24(int val) {}
    public static explicit operator checked UInt24(short val) {}
    public static explicit operator checked UInt24(uint val) {}
    public static explicit operator checked int(UInt24 val) {}
    public static explicit operator checked short(UInt24 val) {}
    public static explicit operator checked ushort(UInt24 val) {}
    public static UInt24 operator checked ++ (UInt24 @value) {}
    public static UInt24 operator checked * (UInt24 left, UInt24 right) {}
    public static UInt24 operator checked - (UInt24 left, UInt24 right) {}
    public static UInt24 operator checked - (UInt24 @value) {}
    public static UInt24 operator -- (UInt24 @value) {}
    public static UInt24 operator / (UInt24 left, UInt24 right) {}
    public static bool operator == (UInt24 x, UInt24 y) {}
    public static UInt24 operator ^ (UInt24 left, UInt24 right) {}
    public static explicit operator UInt24(int val) {}
    public static explicit operator UInt24(short val) {}
    public static explicit operator UInt24(uint val) {}
    public static explicit operator UInt24(ushort val) {}
    public static explicit operator int(UInt24 val) {}
    public static explicit operator short(UInt24 val) {}
    public static explicit operator uint(UInt24 val) {}
    public static explicit operator ushort(UInt24 val) {}
    public static bool operator > (UInt24 x, UInt24 y) {}
    public static bool operator >= (UInt24 x, UInt24 y) {}
    public static UInt24 operator ++ (UInt24 @value) {}
    public static bool operator != (UInt24 x, UInt24 y) {}
    public static UInt24 operator << (UInt24 @value, int shiftAmount) {}
    public static bool operator < (UInt24 x, UInt24 y) {}
    public static bool operator <= (UInt24 x, UInt24 y) {}
    public static UInt24 operator % (UInt24 left, UInt24 right) {}
    public static UInt24 operator * (UInt24 left, UInt24 right) {}
    public static UInt24 operator ~ (UInt24 @value) {}
    public static UInt24 operator >> (UInt24 @value, int shiftAmount) {}
    public static UInt24 operator - (UInt24 left, UInt24 right) {}
    public static UInt24 operator - (UInt24 @value) {}
    public static UInt24 operator + (UInt24 @value) {}
    public static UInt24 operator >>> (UInt24 @value, int shiftAmount) {}

    public UInt24(ReadOnlySpan<byte> @value, bool isBigEndian = false) {}
    public UInt24(byte[] @value, bool isBigEndian = false) {}
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
    public bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten) {}
    public bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public readonly struct UInt48 :
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
    public static readonly UInt48 MaxValue; // = "281474976710655"
    public static readonly UInt48 MinValue; // = "0"
    public static readonly UInt48 One; // = "1"
    public static readonly UInt48 Zero; // = "0"

    public static UInt48 Abs(UInt48 @value) {}
    public static UInt48 Clamp(UInt48 @value, UInt48 min, UInt48 max) {}
    public static (UInt48 Quotient, UInt48 Remainder) DivRem(UInt48 left, UInt48 right) {}
    public static bool IsPow2(UInt48 @value) {}
    public static int LeadingZeroCount(UInt48 @value) {}
    public static int Log2(UInt48 @value) {}
    public static UInt48 Max(UInt48 x, UInt48 y) {}
    public static UInt48 Min(UInt48 x, UInt48 y) {}
    public static UInt48 Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null) {}
    public static UInt48 Parse(string s, IFormatProvider provider = null) {}
    public static UInt48 Parse(string s, NumberStyles style, IFormatProvider provider = null) {}
    public static int PopCount(UInt48 @value) {}
    public static UInt48 RotateLeft(UInt48 @value, int rotateAmount) {}
    public static UInt48 RotateRight(UInt48 @value, int rotateAmount) {}
    public static int Sign(UInt48 @value) {}
    public static int TrailingZeroCount(UInt48 @value) {}
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out UInt48 result) {}
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out UInt48 result) {}
    public static bool TryParse(ReadOnlySpan<char> s, out UInt48 result) {}
    public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out UInt48 result) {}
    public static bool TryParse([NotNullWhen(true)] string s, NumberStyles style, IFormatProvider provider, out UInt48 result) {}
    public static bool TryParse([NotNullWhen(true)] string s, out UInt48 result) {}
    public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt48 @value) {}
    public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt48 @value) {}
    public static UInt48 operator + (UInt48 left, UInt48 right) {}
    public static UInt48 operator & (UInt48 left, UInt48 right) {}
    public static UInt48 operator | (UInt48 left, UInt48 right) {}
    public static UInt48 operator checked + (UInt48 left, UInt48 right) {}
    public static UInt48 operator checked -- (UInt48 @value) {}
    public static UInt48 operator checked / (UInt48 left, UInt48 right) {}
    public static explicit operator checked UInt48(int val) {}
    public static explicit operator checked UInt48(long val) {}
    public static explicit operator checked UInt48(ulong val) {}
    public static explicit operator checked int(UInt48 val) {}
    public static explicit operator checked long(UInt48 val) {}
    public static explicit operator checked uint(UInt48 val) {}
    public static UInt48 operator checked ++ (UInt48 @value) {}
    public static UInt48 operator checked * (UInt48 left, UInt48 right) {}
    public static UInt48 operator checked - (UInt48 left, UInt48 right) {}
    public static UInt48 operator checked - (UInt48 @value) {}
    public static UInt48 operator -- (UInt48 @value) {}
    public static UInt48 operator / (UInt48 left, UInt48 right) {}
    public static bool operator == (UInt48 x, UInt48 y) {}
    public static UInt48 operator ^ (UInt48 left, UInt48 right) {}
    public static explicit operator UInt48(int val) {}
    public static explicit operator UInt48(long val) {}
    public static explicit operator UInt48(uint val) {}
    public static explicit operator UInt48(ulong val) {}
    public static explicit operator int(UInt48 val) {}
    public static explicit operator long(UInt48 val) {}
    public static explicit operator uint(UInt48 val) {}
    public static explicit operator ulong(UInt48 val) {}
    public static bool operator > (UInt48 x, UInt48 y) {}
    public static bool operator >= (UInt48 x, UInt48 y) {}
    public static UInt48 operator ++ (UInt48 @value) {}
    public static bool operator != (UInt48 x, UInt48 y) {}
    public static UInt48 operator << (UInt48 @value, int shiftAmount) {}
    public static bool operator < (UInt48 x, UInt48 y) {}
    public static bool operator <= (UInt48 x, UInt48 y) {}
    public static UInt48 operator % (UInt48 left, UInt48 right) {}
    public static UInt48 operator * (UInt48 left, UInt48 right) {}
    public static UInt48 operator ~ (UInt48 @value) {}
    public static UInt48 operator >> (UInt48 @value, int shiftAmount) {}
    public static UInt48 operator - (UInt48 left, UInt48 right) {}
    public static UInt48 operator - (UInt48 @value) {}
    public static UInt48 operator + (UInt48 @value) {}
    public static UInt48 operator >>> (UInt48 @value, int shiftAmount) {}

    public UInt48(ReadOnlySpan<byte> @value, bool isBigEndian = false) {}
    public UInt48(byte[] @value, bool isBigEndian = false) {}
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
    public bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten) {}
    public bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.3.2.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
