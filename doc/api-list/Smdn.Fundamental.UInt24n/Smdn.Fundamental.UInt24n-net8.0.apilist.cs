// Smdn.Fundamental.UInt24n.dll (Smdn.Fundamental.UInt24n-4.0.0)
//   Name: Smdn.Fundamental.UInt24n
//   AssemblyVersion: 4.0.0.0
//   InformationalVersion: 4.0.0+67b803a21469d1c9d14f260a6414b9e666a8e282
//   TargetFramework: .NETCoreApp,Version=v8.0
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     System.Memory, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using Smdn;

namespace Smdn {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public readonly struct UInt24 :
    IBinaryInteger<UInt24>,
    IComparable<int>,
    IComparable<uint>,
    IConvertible,
    IEquatable<int>,
    IEquatable<uint>,
    IMinMaxValue<UInt24>,
    IUnsignedNumber<UInt24>
  {
    public static readonly UInt24 MaxValue; // = "16777215"
    public static readonly UInt24 MinValue; // = "0"
    public static readonly UInt24 One; // = "1"
    public static readonly UInt24 Zero; // = "0"

    static UInt24 IAdditiveIdentity<UInt24, UInt24>.AdditiveIdentity { get; }
    static UInt24 IMinMaxValue<UInt24>.MaxValue { get; }
    static UInt24 IMinMaxValue<UInt24>.MinValue { get; }
    static UInt24 IMultiplicativeIdentity<UInt24, UInt24>.MultiplicativeIdentity { get; }
    static UInt24 INumberBase<UInt24>.One { get; }
    static int INumberBase<UInt24>.Radix { get; }
    static UInt24 INumberBase<UInt24>.Zero { get; }

    public static UInt24 Abs(UInt24 @value) {}
    public static UInt24 Clamp(UInt24 @value, UInt24 min, UInt24 max) {}
    [Obsolete("Use CreateChecked")]
    public static UInt24 Create<TOther>(TOther @value) where TOther : INumberBase<TOther> {}
    public static UInt24 CreateChecked<TOther>(TOther @value) where TOther : INumberBase<TOther> {}
    public static UInt24 CreateSaturating<TOther>(TOther @value) where TOther : INumberBase<TOther> {}
    public static UInt24 CreateTruncating<TOther>(TOther @value) where TOther : INumberBase<TOther> {}
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
    static UInt24 ISpanParsable<UInt24>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) {}
    static UInt24 IBinaryInteger<UInt24>.LeadingZeroCount(UInt24 @value) {}
    static UInt24 IBinaryInteger<UInt24>.PopCount(UInt24 @value) {}
    static UInt24 IBinaryInteger<UInt24>.TrailingZeroCount(UInt24 @value) {}
    static UInt24 IBinaryNumber<UInt24>.Log2(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsCanonical(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsComplexNumber(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsEvenInteger(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsFinite(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsImaginaryNumber(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsInfinity(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsInteger(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsNaN(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsNegative(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsNegativeInfinity(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsNormal(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsOddInteger(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsPositive(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsPositiveInfinity(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsRealNumber(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsSubnormal(UInt24 @value) {}
    static bool INumberBase<UInt24>.IsZero(UInt24 @value) {}
    static UInt24 INumberBase<UInt24>.MaxMagnitude(UInt24 x, UInt24 y) {}
    static UInt24 INumberBase<UInt24>.MaxMagnitudeNumber(UInt24 x, UInt24 y) {}
    static UInt24 INumberBase<UInt24>.MinMagnitude(UInt24 x, UInt24 y) {}
    static UInt24 INumberBase<UInt24>.MinMagnitudeNumber(UInt24 x, UInt24 y) {}
    static bool INumberBase<UInt24>.TryConvertToChecked<TOther>(UInt24 @value, out TOther result) where TOther : INumberBase<TOther> {}
    static bool INumberBase<UInt24>.TryConvertToSaturating<TOther>(UInt24 @value, out TOther result) where TOther : INumberBase<TOther> {}
    static bool INumberBase<UInt24>.TryConvertToTruncating<TOther>(UInt24 @value, out TOther result) where TOther : INumberBase<TOther> {}
    public static int TrailingZeroCount(UInt24 @value) {}
    public static bool TryConvertFromChecked<TOther>(TOther @value, out UInt24 result) where TOther : INumberBase<TOther> {}
    public static bool TryConvertFromSaturating<TOther>(TOther @value, out UInt24 result) where TOther : INumberBase<TOther> {}
    public static bool TryConvertFromTruncating<TOther>(TOther @value, out UInt24 result) where TOther : INumberBase<TOther> {}
    [Obsolete("Use TryConvertFromTruncating or TryConvertFromChecked")]
    public static bool TryCreate<TOther>(TOther @value, out UInt24 result) where TOther : INumberBase<TOther> {}
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
    int IBinaryInteger<UInt24>.GetByteCount() {}
    int IBinaryInteger<UInt24>.GetShortestBitLength() {}
    public int ToInt32() {}
    public override string ToString() {}
    public string ToString(IFormatProvider formatProvider) {}
    public string ToString(string format) {}
    public string ToString(string format, IFormatProvider formatProvider) {}
    public uint ToUInt32() {}
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) {}
    public bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten) {}
    public bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public readonly struct UInt48 :
    IBinaryInteger<UInt48>,
    IComparable<long>,
    IComparable<ulong>,
    IConvertible,
    IEquatable<long>,
    IEquatable<ulong>,
    IMinMaxValue<UInt48>,
    IUnsignedNumber<UInt48>
  {
    public static readonly UInt48 MaxValue; // = "281474976710655"
    public static readonly UInt48 MinValue; // = "0"
    public static readonly UInt48 One; // = "1"
    public static readonly UInt48 Zero; // = "0"

    static UInt48 IAdditiveIdentity<UInt48, UInt48>.AdditiveIdentity { get; }
    static UInt48 IMinMaxValue<UInt48>.MaxValue { get; }
    static UInt48 IMinMaxValue<UInt48>.MinValue { get; }
    static UInt48 IMultiplicativeIdentity<UInt48, UInt48>.MultiplicativeIdentity { get; }
    static UInt48 INumberBase<UInt48>.One { get; }
    static int INumberBase<UInt48>.Radix { get; }
    static UInt48 INumberBase<UInt48>.Zero { get; }

    public static UInt48 Abs(UInt48 @value) {}
    public static UInt48 Clamp(UInt48 @value, UInt48 min, UInt48 max) {}
    [Obsolete("Use CreateChecked")]
    public static UInt48 Create<TOther>(TOther @value) where TOther : INumberBase<TOther> {}
    public static UInt48 CreateChecked<TOther>(TOther @value) where TOther : INumberBase<TOther> {}
    public static UInt48 CreateSaturating<TOther>(TOther @value) where TOther : INumberBase<TOther> {}
    public static UInt48 CreateTruncating<TOther>(TOther @value) where TOther : INumberBase<TOther> {}
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
    static UInt48 ISpanParsable<UInt48>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) {}
    static UInt48 IBinaryInteger<UInt48>.LeadingZeroCount(UInt48 @value) {}
    static UInt48 IBinaryInteger<UInt48>.PopCount(UInt48 @value) {}
    static UInt48 IBinaryInteger<UInt48>.TrailingZeroCount(UInt48 @value) {}
    static UInt48 IBinaryNumber<UInt48>.Log2(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsCanonical(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsComplexNumber(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsEvenInteger(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsFinite(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsImaginaryNumber(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsInfinity(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsInteger(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsNaN(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsNegative(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsNegativeInfinity(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsNormal(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsOddInteger(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsPositive(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsPositiveInfinity(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsRealNumber(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsSubnormal(UInt48 @value) {}
    static bool INumberBase<UInt48>.IsZero(UInt48 @value) {}
    static UInt48 INumberBase<UInt48>.MaxMagnitude(UInt48 x, UInt48 y) {}
    static UInt48 INumberBase<UInt48>.MaxMagnitudeNumber(UInt48 x, UInt48 y) {}
    static UInt48 INumberBase<UInt48>.MinMagnitude(UInt48 x, UInt48 y) {}
    static UInt48 INumberBase<UInt48>.MinMagnitudeNumber(UInt48 x, UInt48 y) {}
    static bool INumberBase<UInt48>.TryConvertToChecked<TOther>(UInt48 @value, out TOther result) where TOther : INumberBase<TOther> {}
    static bool INumberBase<UInt48>.TryConvertToSaturating<TOther>(UInt48 @value, out TOther result) where TOther : INumberBase<TOther> {}
    static bool INumberBase<UInt48>.TryConvertToTruncating<TOther>(UInt48 @value, out TOther result) where TOther : INumberBase<TOther> {}
    public static int TrailingZeroCount(UInt48 @value) {}
    public static bool TryConvertFromChecked<TOther>(TOther @value, out UInt48 result) where TOther : INumberBase<TOther> {}
    public static bool TryConvertFromSaturating<TOther>(TOther @value, out UInt48 result) where TOther : INumberBase<TOther> {}
    public static bool TryConvertFromTruncating<TOther>(TOther @value, out UInt48 result) where TOther : INumberBase<TOther> {}
    [Obsolete("Use TryConvertFromTruncating or TryConvertFromChecked")]
    public static bool TryCreate<TOther>(TOther @value, out UInt48 result) where TOther : INumberBase<TOther> {}
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
    int IBinaryInteger<UInt48>.GetByteCount() {}
    int IBinaryInteger<UInt48>.GetShortestBitLength() {}
    public long ToInt64() {}
    public override string ToString() {}
    public string ToString(IFormatProvider formatProvider) {}
    public string ToString(string format) {}
    public string ToString(string format, IFormatProvider formatProvider) {}
    public ulong ToUInt64() {}
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) {}
    public bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten) {}
    public bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.3.2.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
