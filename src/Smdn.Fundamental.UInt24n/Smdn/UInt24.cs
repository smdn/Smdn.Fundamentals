// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET6_0_OR_GREATER
#define SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2
#define SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
#endif
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_NUMERICS_BITOPERATIONS_LOG2
#define SYSTEM_NUMERICS_BITOPERATIONS_LEADINGZEROCOUNT
#define SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
#define SYSTEM_NUMERICS_BITOPERATIONS_TRAILINGZEROCOUNT
#endif
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_MATH_CLAMP
#endif

using System;
using System.Globalization;
#if SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2 || SYSTEM_NUMERICS_BITOPERATIONS_LOG2
using System.Numerics;
#endif
using System.Runtime.InteropServices;
#if SYSTEM_MATH_CLAMP
using MathClampShim = System.Math;
#else
using MathClampShim = Smdn.UInt24n;
#endif
#if SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
using MathDivRemShim = System.Math;
#else
using MathDivRemShim = Smdn.UInt24n;
#endif

namespace Smdn;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
[StructLayout(LayoutKind.Explicit, Pack = 1)]
#pragma warning disable IDE0055
public struct UInt24 :
#if FEATURE_GENERIC_MATH
#if false
  IAdditiveIdentity<UInt24, int>,
  IAdditiveIdentity<UInt24, uint>,
#endif
#if false
  IMultiplicativeIdentity<UInt24, int>,
  IMultiplicativeIdentity<UInt24, uint>,
#endif
  IBinaryInteger<UInt24>,
  IBinaryNumber<UInt24>,
  IMinMaxValue<UInt24>,
  IUnsignedNumber<UInt24>,
#else // FEATURE_GENERIC_MATH
  IComparable<UInt24>,
  IComparable,
  IEquatable<UInt24>,
  IFormattable,
#if SYSTEM_ISPANFORMATTABLE
  ISpanFormattable,
#endif
#endif // FEATURE_GENERIC_MATH
  IConvertible,
  IComparable<int>,
  IComparable<uint>,
  IEquatable<int>,
  IEquatable<uint>
{
#pragma warning restore IDE0055
  private const int SizeOfSelf = 3;
  private const int BitsOfSelf = 8 * SizeOfSelf;

  // big endian
  [FieldOffset(0)] public byte Byte0; // 0x 00ff0000
  [FieldOffset(1)] public byte Byte1; // 0x 0000ff00
  [FieldOffset(2)] public byte Byte2; // 0x 000000ff

  private const int maxValue = 0xffffff;
  private const int minValue = 0x000000;
  private const uint UnusedBitMask = 0xFF000000;

  public static readonly UInt24 MaxValue = new(maxValue);
  public static readonly UInt24 MinValue = new(minValue);
  public static readonly UInt24 Zero     = new(0);
  public static readonly UInt24 One      = new(1);

#if FEATURE_GENERIC_MATH
  // INumber
  static UInt24 INumber<UInt24>.Zero => Zero;
  static UInt24 INumber<UInt24>.One => One;

  // IMinMaxValue
  static UInt24 IMinMaxValue<UInt24>.MinValue => MinValue;
  static UInt24 IMinMaxValue<UInt24>.MaxValue => MaxValue;

  // IAdditiveIdentity
  static UInt24 IAdditiveIdentity<UInt24, UInt24>.AdditiveIdentity => Zero;
#if false
  static int IAdditiveIdentity<UInt24, int>.AdditiveIdentity => 0;
  static uint IAdditiveIdentity<UInt24, uint>.AdditiveIdentity => 0u;
#endif

  // IMultiplicativeIdentity
  static UInt24 IMultiplicativeIdentity<UInt24, UInt24>.MultiplicativeIdentity => One;
#if false
  static int IMultiplicativeIdentity<UInt24, int>.MultiplicativeIdentity => 1;
  static uint IMultiplicativeIdentity<UInt24, uint>.MultiplicativeIdentity => 1u;
#endif
#endif

  public UInt24(byte[] value, bool isBigEndian = false)
    : this(UInt24n.ValidateAndGetSpan(value, 0, nameof(value), SizeOfSelf), isBigEndian)
  {
  }

  public UInt24(byte[] value, int startIndex, bool isBigEndian = false)
    : this(UInt24n.ValidateAndGetSpan(value, startIndex, nameof(value), SizeOfSelf), isBigEndian)
  {
  }

  public UInt24(ReadOnlySpan<byte> value, bool isBigEndian = false)
  {
    if (value.Length < SizeOfSelf)
      throw ExceptionUtils.CreateArgumentMustHaveLengthAtLeast(nameof(value), SizeOfSelf);

    if (isBigEndian) {
      Byte0 = value[0];
      Byte1 = value[1];
      Byte2 = value[2];
    }
    else {
      Byte0 = value[2];
      Byte1 = value[1];
      Byte2 = value[0];
    }
  }

  private UInt24(uint value)
  {
    unchecked {
      Byte0 = (byte)(value >> 16);
      Byte1 = (byte)(value >> 8);
      Byte2 = (byte)value;
    }
  }

  [CLSCompliant(false)]
  public static explicit operator UInt24(uint val)
    => maxValue < val ? throw UInt24n.CreateOverflowException<UInt24>(val) : new(val);

  public static explicit operator UInt24(int val)
    => val is < minValue or > maxValue ? throw UInt24n.CreateOverflowException<UInt24>(val) : new(unchecked((uint)val));

  [CLSCompliant(false)]
  public static explicit operator UInt24(ushort val)
    => new((uint)val);

  public static explicit operator UInt24(short val)
    => val < minValue ? throw UInt24n.CreateOverflowException<UInt24>(val) : new((uint)val);

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
      throw UInt24n.CreateArgumentIsNotComparableException<UInt24>(obj, nameof(obj));
  }

  public int CompareTo(UInt24 other) => ToUInt32().CompareTo(other.ToUInt32());

  [CLSCompliant(false)]
  public int CompareTo(uint other) => ToUInt32().CompareTo(other);

  public int CompareTo(int other) => ToInt32().CompareTo(other);

  /*
   * IComparisonOperators
   */
  public static bool operator <(UInt24 x, UInt24 y) => x.ToUInt32() < y.ToUInt32();
  public static bool operator <=(UInt24 x, UInt24 y) => x.ToUInt32() <= y.ToUInt32();
  public static bool operator >(UInt24 x, UInt24 y) => x.ToUInt32() > y.ToUInt32();
  public static bool operator >=(UInt24 x, UInt24 y) => x.ToUInt32() >= y.ToUInt32();

  public bool Equals(UInt24 other) => this == other;

  [CLSCompliant(false)]
  public bool Equals(uint other) => ToUInt32() == other;

  public bool Equals(int other) => ToInt32() == other;

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

  /*
   * IEqualityOperators
   */
  public static bool operator ==(UInt24 x, UInt24 y)
    =>
      x.Byte0 == y.Byte0 &&
      x.Byte1 == y.Byte1 &&
      x.Byte2 == y.Byte2;

  public static bool operator !=(UInt24 x, UInt24 y)
    =>
      x.Byte0 != y.Byte0 ||
      x.Byte1 != y.Byte1 ||
      x.Byte2 != y.Byte2;

  public override int GetHashCode() => (Byte2 << 16) | (Byte1 << 8) | Byte0;

  public override string ToString() => ToString(null, null);
  public string ToString(string format) => ToString(format, null);
  public string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);
  public string ToString(string format, IFormatProvider formatProvider)
    => ToUInt32().ToString(format, formatProvider);

#if SYSTEM_ISPANFORMATTABLE
#nullable enable
  public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    => ToUInt32().TryFormat(destination, out charsWritten, format, provider);
#nullable restore
#endif

#nullable enable
  // IParseable<TSelf>.Parse(String, IFormatProvider)
  public static UInt24 Parse(string s, IFormatProvider? provider = null)
    => Parse(s, NumberStyles.Integer, provider);

  // INumber<TSelf>.Parse(String, NumberStyles, IFormatProvider)
  public static UInt24 Parse(string s, NumberStyles style, IFormatProvider? provider = null)
    => (UInt24)UInt32.Parse(s, style, provider);

#if SYSTEM_INUMBER_PARSE_READONLYSPAN_OF_CHAR
#if FEATURE_GENERIC_MATH
  static UInt24 ISpanParseable<UInt24>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    => Parse(s, NumberStyles.Integer, provider);
#endif

  // INumber<TSelf>.Parse(ReadOnlySpan<Char>, NumberStyles, IFormatProvider)
  public static UInt24 Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    => (UInt24)UInt32.Parse(s, style, provider);
#endif

  public static bool TryParse(string? s, out UInt24 result)
    => TryParse(s, NumberStyles.Integer, provider: null, out result);

  // IParseable<TSelf>.TryParse(String, IFormatProvider, TSelf)
  public static bool TryParse(string? s, IFormatProvider? provider, out UInt24 result)
    => TryParse(s, NumberStyles.Integer, provider, out result);

  // INumber<TSelf>.TryParse(ReadOnlySpan<char>, NumberStyles, IFormatProvider?, out TSelf)
  public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out UInt24 result)
  {
    result = default;

    if (!UInt32.TryParse(s, style, provider, out var result32))
      return false;

    if (maxValue < result32)
      return false; // overflow

    result = new(result32);

    return true;
  }

#if SYSTEM_INUMBER_TRYPARSE_READONLYSPAN_OF_CHAR
  public static bool TryParse(ReadOnlySpan<char> s, out UInt24 result)
    => TryParse(s, NumberStyles.Integer, provider: null, out result);

  // ISpanParseable<TSelf>.TryParse(ReadOnlySpan<Char>, IFormatProvider, TSelf)
  public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out UInt24 result)
    => TryParse(s, NumberStyles.Integer, provider, out result);

  // INumber<TSelf>.TryParse(ReadOnlySpan<char>, NumberStyles, IFormatProvider?, out TSelf)
  public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out UInt24 result)
  {
    result = default;

    if (!UInt32.TryParse(s, style, provider, out var result32))
      return false;

    if (maxValue < result32)
      return false; // overflow

    result = new(result32);

    return true;
  }
#endif
#nullable restore

  // IAdditionOperators
  public static UInt24 operator +(UInt24 left, UInt24 right) => new(unchecked(left.ToUInt32() + right.ToUInt32())); // TODO: checked

  // ISubtractionOperators
  public static UInt24 operator -(UInt24 left, UInt24 right) => new(unchecked(left.ToUInt32() - right.ToUInt32())); // TODO: checked

  // IMultiplyOperators
  public static UInt24 operator *(UInt24 left, UInt24 right) => new(unchecked(left.ToUInt32() * right.ToUInt32())); // TODO: checked

  // IDivisionOperators
  public static UInt24 operator /(UInt24 left, UInt24 right) => new(left.ToUInt32() / right.ToUInt32()); // TODO: checked

  // IModulusOperators
  public static UInt24 operator %(UInt24 left, UInt24 right) => new(left.ToUInt32() % right.ToUInt32()); // TODO: checked

  // IUnaryPlusOperators
  public static UInt24 operator +(UInt24 value) => new(+value.ToUInt32()); // TODO: checked

  // IUnaryNegationOperators
  public static UInt24 operator -(UInt24 value) => new(unchecked(0 - value.ToUInt32())); // TODO: checked

  // IIncrementOperators
  public static UInt24 operator ++(UInt24 value) => new(unchecked(value.ToUInt32() + 1)); // TODO: checked

  // IDecrementOperators
  public static UInt24 operator --(UInt24 value) => new(unchecked(value.ToUInt32() - 1)); // TODO: checked

  /*
   * INumber<TOther>.Abs/Sign
   */
  public static UInt24 Abs(UInt24 value) => value;
  public static UInt24 Sign(UInt24 value) => value == Zero ? Zero : One;

  /*
   * INumber<TOther>.Min/Max/Clamp
   */
  public static UInt24 Min(UInt24 x, UInt24 y) => x < y ? x : y;
  public static UInt24 Max(UInt24 x, UInt24 y) => x > y ? x : y;
  public static UInt24 Clamp(UInt24 value, UInt24 min, UInt24 max)
    => max < min
      ? throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max))
      : new(MathClampShim.Clamp(value.ToUInt32(), min.ToUInt32(), max.ToUInt32()));

  /*
   * INumber<TOther>.DivRem
   */
  public static (UInt24 Quotient, UInt24 Remainder) DivRem(UInt24 left, UInt24 right)
  {
    var (quot, rem) = MathDivRemShim.DivRem(left.ToUInt32(), right.ToUInt32());

    return (new(quot), new(rem));
  }

#if FEATURE_GENERIC_MATH
  /*
   * INumber<TOther>.Create/TryCreate
   */
  public static UInt24 Create<TOther>(TOther value) where TOther : INumber<TOther>
  {
    if (TryCreateCore(value, out var result))
      return result;

    throw UInt24n.CreateOverflowException<UInt24>(value);
  }

  public static UInt24 CreateTruncating<TOther>(TOther value) where TOther : INumber<TOther>
  {
    if (TryCreateCore(value, out var result))
      return result;

    throw UInt24n.CreateOverflowException<UInt24>(value);
  }

  public static bool TryCreate<TOther>(TOther value, out UInt24 result) where TOther : INumber<TOther>
    => TryCreateCore(value, out result);

  private static bool TryCreateCore<TOther>(
    TOther value,
    out UInt24 result
  ) where TOther : INumber<TOther>
  {
    result = default;

    int val;

    if (typeof(TOther) == typeof(byte))
      val = (int)((byte)(object)value);
    else if (typeof(TOther) == typeof(sbyte))
      val = (int)((sbyte)(object)value);
    else if (typeof(TOther) == typeof(char))
      val = (int)((char)(object)value);
    else if (typeof(TOther) == typeof(ushort))
      val = (int)((ushort)(object)value);
    else if (typeof(TOther) == typeof(short))
      val = (int)((short)(object)value);
    else if (typeof(TOther) == typeof(uint))
      val = unchecked((int)((uint)(object)value));
    else if (typeof(TOther) == typeof(int))
      val = (int)(object)value;
    else if (typeof(TOther) == typeof(ulong))
      val = unchecked((int)((ulong)(object)value));
    else if (typeof(TOther) == typeof(long))
      val = unchecked((int)((long)(object)value));
    else if (typeof(TOther) == typeof(nuint))
      val = unchecked((int)((nuint)(object)value));
    else if (typeof(TOther) == typeof(nint))
      val = unchecked((int)((nint)(object)value));
    else if (typeof(TOther) == typeof(Half))
      val = (int)((Half)(object)value);
    else if (typeof(TOther) == typeof(float))
      val = unchecked((int)((float)(object)value));
    else if (typeof(TOther) == typeof(double))
      val = unchecked((int)((double)(object)value));
#pragma warning disable IDE0045
    else if (typeof(TOther) == typeof(decimal))
      val = unchecked((int)((decimal)(object)value));
#pragma warning restore IDE0045
    else
      throw UInt24n.CreateTypeIsNotConvertibleException<UInt24, TOther>();

    if (val is < minValue or > maxValue)
      return false; // overflow

    result = new(unchecked((uint)val));

    return true;
  }

  public static UInt24 CreateSaturating<TOther>(TOther value) where TOther : INumber<TOther>
  {
    if (typeof(TOther) == typeof(byte)) {
      return new((uint)((byte)(object)value));
    }
    else if (typeof(TOther) == typeof(sbyte)) {
      var val = (sbyte)(object)value;

      return val < minValue ? MinValue : new((uint)val);
    }
    else if (typeof(TOther) == typeof(char)) {
      return new((uint)((char)(object)value));
    }
    else if (typeof(TOther) == typeof(ushort)) {
      return new((uint)((ushort)(object)value));
    }
    else if (typeof(TOther) == typeof(short)) {
      var val = (short)(object)value;

      return val < 0 ? Zero : new((uint)val);
    }
    else if (typeof(TOther) == typeof(uint)) {
      var val = (uint)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new(val);
    }
    else if (typeof(TOther) == typeof(int)) {
      var val = (int)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(ulong)) {
      var val = (ulong)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(long)) {
      var val = (long)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(nuint)) {
      var val = (nuint)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(nint)) {
      var val = (nint)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(Half)) {
      var val = (float)((Half)(object)value);

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(float)) {
      var val = (float)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(double)) {
      var val = (double)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(decimal)) {
      var val = (decimal)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }

    throw UInt24n.CreateTypeIsNotConvertibleException<UInt24, TOther>();
  }
#endif

  /*
   * IBitwiseOperators
   */
  public static UInt24 operator &(UInt24 left, UInt24 right) => new(left.ToUInt32() & right.ToUInt32());
  public static UInt24 operator |(UInt24 left, UInt24 right) => new(left.ToUInt32() | right.ToUInt32());
  public static UInt24 operator ^(UInt24 left, UInt24 right) => new(left.ToUInt32() ^ right.ToUInt32());
  public static UInt24 operator ~(UInt24 value) => new(~value.ToUInt32());

  /*
   * IBinaryNumber
   */
#if SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2
  public static bool IsPow2(UInt24 value) => BitOperations.IsPow2(value.ToUInt32());
#endif
#if SYSTEM_NUMERICS_BITOPERATIONS_LOG2
  public static int Log2(UInt24 value) => BitOperations.Log2(value.ToUInt32());
#if FEATURE_GENERIC_MATH
  static UInt24 IBinaryNumber<UInt24>.Log2(UInt24 value) => new((uint)Log2(value));
#endif
#endif

  /*
   * IShiftOperators
   */
  public static UInt24 operator <<(UInt24 value, int shiftAmount) => new(value.ToUInt32() << UInt24n.RegularizeShiftAmount(shiftAmount, BitsOfSelf));
  public static UInt24 operator >>(UInt24 value, int shiftAmount) => new(value.ToUInt32() >> UInt24n.RegularizeShiftAmount(shiftAmount, BitsOfSelf));

  /*
   * IBinaryInteger
   */
  public static UInt24 RotateLeft(UInt24 value, int rotateAmount)
  {
    if (rotateAmount == 0)
      return value;
    if (rotateAmount < 0)
      return RotateRight(value, -rotateAmount);

    rotateAmount = UInt24n.RegularizeRotateAmount(rotateAmount, BitsOfSelf);

    var val = value.ToUInt32();

    return new((val << rotateAmount) | (val >> (BitsOfSelf - rotateAmount)));
  }

  public static UInt24 RotateRight(UInt24 value, int rotateAmount)
  {
    if (rotateAmount == 0)
      return value;
    if (rotateAmount < 0)
      return RotateLeft(value, -rotateAmount);

    rotateAmount = UInt24n.RegularizeRotateAmount(rotateAmount, BitsOfSelf);

    var val = value.ToUInt32();

    return new((val >> rotateAmount) | (val << (BitsOfSelf - rotateAmount)));
  }

#if SYSTEM_NUMERICS_BITOPERATIONS_LEADINGZEROCOUNT
#if FEATURE_GENERIC_MATH
  static UInt24 IBinaryInteger<UInt24>.LeadingZeroCount(UInt24 value) => new((uint)LeadingZeroCount(value));
#endif
  public static int LeadingZeroCount(UInt24 value)
    => BitOperations.LeadingZeroCount(value.ToUInt32()) - (32 - BitsOfSelf);
#endif

#if SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
#if FEATURE_GENERIC_MATH
  static UInt24 IBinaryInteger<UInt24>.PopCount(UInt24 value) => new((uint)PopCount(value));
#endif
  public static int PopCount(UInt24 value)
    => BitOperations.PopCount(value.ToUInt32());
#endif

#if SYSTEM_NUMERICS_BITOPERATIONS_TRAILINGZEROCOUNT
#if FEATURE_GENERIC_MATH
  static UInt24 IBinaryInteger<UInt24>.TrailingZeroCount(UInt24 value) => new((uint)TrailingZeroCount(value));
#endif
  public static int TrailingZeroCount(UInt24 value)
    => BitOperations.TrailingZeroCount(value.ToUInt32() | UnusedBitMask);
#endif
}
