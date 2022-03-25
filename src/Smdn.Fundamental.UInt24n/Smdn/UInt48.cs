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
public struct UInt48 :
#if FEATURE_GENERIC_MATH
#if false
  IAdditiveIdentity<UInt48, long>,
  IAdditiveIdentity<UInt48, ulong>,
#endif
#if false
  IMultiplicativeIdentity<UInt48, long>,
  IMultiplicativeIdentity<UInt48, ulong>,
#endif
  IBinaryInteger<UInt48>,
  IBinaryNumber<UInt48>,
  IMinMaxValue<UInt48>,
  IUnsignedNumber<UInt48>,
#else // FEATURE_GENERIC_MATH
  IComparable<UInt48>,
  IComparable,
  IEquatable<UInt48>,
  IFormattable,
#if SYSTEM_ISPANFORMATTABLE
  ISpanFormattable,
#endif
#endif // FEATURE_GENERIC_MATH
  IConvertible,
  IComparable<long>,
  IComparable<ulong>,
  IEquatable<long>,
  IEquatable<ulong>
{
#pragma warning restore IDE0055
  private const int SizeOfSelf = 6;
  private const int BitsOfSelf = 8 * SizeOfSelf;

  // big endian
  [FieldOffset(0)] public byte Byte0; // 0x 0000ff00 00000000
  [FieldOffset(1)] public byte Byte1; // 0x 000000ff 00000000
  [FieldOffset(2)] public byte Byte2; // 0x 00000000 ff000000
  [FieldOffset(3)] public byte Byte3; // 0x 00000000 00ff0000
  [FieldOffset(4)] public byte Byte4; // 0x 00000000 0000ff00
  [FieldOffset(5)] public byte Byte5; // 0x 00000000 000000ff

  private const long maxValue = 0xffffffffffff;
  private const long minValue = 0x000000000000;
  private const ulong UnusedBitMask = 0xFFFF0000_00000000;

  public static readonly UInt48 MaxValue = new(maxValue);
  public static readonly UInt48 MinValue = new(minValue);
  public static readonly UInt48 Zero     = new(0);
  public static readonly UInt48 One      = new(1);

#if FEATURE_GENERIC_MATH
  // INumber
  static UInt48 INumber<UInt48>.Zero => Zero;
  static UInt48 INumber<UInt48>.One => One;

  // IMinMaxValue
  static UInt48 IMinMaxValue<UInt48>.MinValue => MinValue;
  static UInt48 IMinMaxValue<UInt48>.MaxValue => MaxValue;

  // IAdditiveIdentity
  static UInt48 IAdditiveIdentity<UInt48, UInt48>.AdditiveIdentity => Zero;
#if false
  static long IAdditiveIdentity<UInt48, long>.AdditiveIdentity => 0L;
  static ulong IAdditiveIdentity<UInt48, ulong>.AdditiveIdentity => 0uL;
#endif

  // IMultiplicativeIdentity
  static UInt48 IMultiplicativeIdentity<UInt48, UInt48>.MultiplicativeIdentity => One;
#if false
  static long IMultiplicativeIdentity<UInt48, long>.MultiplicativeIdentity => 1L;
  static ulong IMultiplicativeIdentity<UInt48, ulong>.MultiplicativeIdentity => 1uL;
#endif
#endif

  public UInt48(byte[] value, bool isBigEndian = false)
    : this(UInt24n.ValidateAndGetSpan(value, 0, nameof(value), SizeOfSelf), isBigEndian)
  {
  }

  public UInt48(byte[] value, int startIndex, bool isBigEndian = false)
    : this(UInt24n.ValidateAndGetSpan(value, startIndex, nameof(value), SizeOfSelf), isBigEndian)
  {
  }

  public UInt48(ReadOnlySpan<byte> value, bool isBigEndian = false)
  {
    if (value.Length < SizeOfSelf)
      throw ExceptionUtils.CreateArgumentMustHaveLengthAtLeast(nameof(value), SizeOfSelf);

    if (isBigEndian) {
      Byte0 = value[0];
      Byte1 = value[1];
      Byte2 = value[2];
      Byte3 = value[3];
      Byte4 = value[4];
      Byte5 = value[5];
    }
    else {
      Byte0 = value[5];
      Byte1 = value[4];
      Byte2 = value[3];
      Byte3 = value[2];
      Byte4 = value[1];
      Byte5 = value[0];
    }
  }

  private UInt48(ulong value)
  {
    unchecked {
      Byte0 = (byte)(value >> 40);
      Byte1 = (byte)(value >> 32);
      Byte2 = (byte)(value >> 24);
      Byte3 = (byte)(value >> 16);
      Byte4 = (byte)(value >> 8);
      Byte5 = (byte)value;
    }
  }

  [CLSCompliant(false)]
  public static explicit operator UInt48(ulong val)
    => maxValue < val ? throw UInt24n.CreateOverflowException<UInt48>(val) : new(val);

  public static explicit operator UInt48(long val)
    => val is < minValue or > maxValue ? throw UInt24n.CreateOverflowException<UInt48>(val) : new(unchecked((ulong)val));

  [CLSCompliant(false)]
  public static explicit operator UInt48(uint val)
    => new((ulong)val);

  public static explicit operator UInt48(int val)
    => val < minValue ? throw UInt24n.CreateOverflowException<UInt48>(val) : new((ulong)val);

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
      throw UInt24n.CreateArgumentIsNotComparableException<UInt48>(obj, nameof(obj));
  }

  public int CompareTo(UInt48 other) => ToUInt64().CompareTo(other.ToUInt64());

  [CLSCompliant(false)]
  public int CompareTo(ulong other) => ToUInt64().CompareTo(other);

  public int CompareTo(long other) => ToInt64().CompareTo(other);

  /*
   * IComparisonOperators
   */
  public static bool operator <(UInt48 x, UInt48 y) => x.ToUInt64() < y.ToUInt64();
  public static bool operator <=(UInt48 x, UInt48 y) => x.ToUInt64() <= y.ToUInt64();
  public static bool operator >(UInt48 x, UInt48 y) => x.ToUInt64() > y.ToUInt64();
  public static bool operator >=(UInt48 x, UInt48 y) => x.ToUInt64() >= y.ToUInt64();

  public bool Equals(UInt48 other) => this == other;

  [CLSCompliant(false)]
  public bool Equals(ulong other) => ToUInt64() == other;

  public bool Equals(long other) => ToInt64() == other;

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

  /*
   * IEqualityOperators
   */
  public static bool operator ==(UInt48 x, UInt48 y)
    =>
      x.Byte0 == y.Byte0 &&
      x.Byte1 == y.Byte1 &&
      x.Byte2 == y.Byte2 &&
      x.Byte3 == y.Byte3 &&
      x.Byte4 == y.Byte4 &&
      x.Byte5 == y.Byte5;

  public static bool operator !=(UInt48 x, UInt48 y)
    =>
      x.Byte0 != y.Byte0 ||
      x.Byte1 != y.Byte1 ||
      x.Byte2 != y.Byte2 ||
      x.Byte3 != y.Byte3 ||
      x.Byte4 != y.Byte4 ||
      x.Byte5 != y.Byte5;

  public override int GetHashCode()
    => ((Byte3 << 24) | (Byte2 << 16) | (Byte1 << 8) | Byte0) ^ ((Byte5 << 8) | Byte4);

  public override string ToString() => ToString(null, null);
  public string ToString(string format) => ToString(format, null);
  public string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);
  public string ToString(string format, IFormatProvider formatProvider)
    => ToUInt64().ToString(format, formatProvider);

#if SYSTEM_ISPANFORMATTABLE
#nullable enable
  public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    => ToUInt64().TryFormat(destination, out charsWritten, format, provider);
#nullable restore
#endif

#nullable enable
  // IParseable<TSelf>.Parse(String, IFormatProvider)
  public static UInt48 Parse(string s, IFormatProvider? provider = null)
    => Parse(s, NumberStyles.Integer, provider);

  // INumber<TSelf>.Parse(String, NumberStyles, IFormatProvider)
  public static UInt48 Parse(string s, NumberStyles style, IFormatProvider? provider = null)
    => (UInt48)UInt64.Parse(s, style, provider);

#if SYSTEM_INUMBER_PARSE_READONLYSPAN_OF_CHAR
#if FEATURE_GENERIC_MATH
  static UInt48 ISpanParseable<UInt48>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    => Parse(s, NumberStyles.Integer, provider);
#endif

  // INumber<TSelf>.Parse(ReadOnlySpan<Char>, NumberStyles, IFormatProvider)
  public static UInt48 Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    => (UInt48)UInt64.Parse(s, style, provider);
#endif

  public static bool TryParse(string? s, out UInt48 result)
    => TryParse(s, NumberStyles.Integer, provider: null, out result);

  // IParseable<TSelf>.TryParse(String, IFormatProvider, TSelf)
  public static bool TryParse(string? s, IFormatProvider? provider, out UInt48 result)
    => TryParse(s, NumberStyles.Integer, provider, out result);

  // INumber<TSelf>.TryParse(ReadOnlySpan<char>, NumberStyles, IFormatProvider?, out TSelf)
  public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out UInt48 result)
  {
    result = default;

    if (!UInt64.TryParse(s, style, provider, out var result64))
      return false;

    if (maxValue < result64)
      return false; // overflow

    result = new(result64);

    return true;
  }

#if SYSTEM_INUMBER_TRYPARSE_READONLYSPAN_OF_CHAR
  public static bool TryParse(ReadOnlySpan<char> s, out UInt48 result)
    => TryParse(s, NumberStyles.Integer, provider: null, out result);

  // ISpanParseable<TSelf>.TryParse(ReadOnlySpan<Char>, IFormatProvider, TSelf)
  public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out UInt48 result)
    => TryParse(s, NumberStyles.Integer, provider, out result);

  // INumber<TSelf>.TryParse(ReadOnlySpan<char>, NumberStyles, IFormatProvider?, out TSelf)
  public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out UInt48 result)
  {
    result = default;

    if (!UInt64.TryParse(s, style, provider, out var result64))
      return false;

    if (maxValue < result64)
      return false; // overflow

    result = new(result64);

    return true;
  }
#endif
#nullable restore

  // IAdditionOperators
  public static UInt48 operator +(UInt48 left, UInt48 right) => new(unchecked(left.ToUInt64() + right.ToUInt64())); // TODO: checked

  // ISubtractionOperators
  public static UInt48 operator -(UInt48 left, UInt48 right) => new(unchecked(left.ToUInt64() - right.ToUInt64())); // TODO: checked

  // IMultiplyOperators
  public static UInt48 operator *(UInt48 left, UInt48 right) => new(unchecked(left.ToUInt64() * right.ToUInt64())); // TODO: checked

  // IDivisionOperators
  public static UInt48 operator /(UInt48 left, UInt48 right) => new(left.ToUInt64() / right.ToUInt64()); // TODO: checked

  // IModulusOperators
  public static UInt48 operator %(UInt48 left, UInt48 right) => new(left.ToUInt64() % right.ToUInt64()); // TODO: checked

  // IUnaryPlusOperators
  public static UInt48 operator +(UInt48 value) => new(+value.ToUInt64()); // TODO: checked

  // IUnaryNegationOperators
  public static UInt48 operator -(UInt48 value) => new(unchecked(0 - value.ToUInt64())); // TODO: checked

  // IIncrementOperators
  public static UInt48 operator ++(UInt48 value) => new(unchecked(value.ToUInt64() + 1)); // TODO: checked

  // IDecrementOperators
  public static UInt48 operator --(UInt48 value) => new(unchecked(value.ToUInt64() - 1)); // TODO: checked

  /*
   * INumber<TOther>.Abs/Sign
   */
  public static UInt48 Abs(UInt48 value) => value;
  public static UInt48 Sign(UInt48 value) => value == Zero ? Zero : One;

  /*
   * INumber<TOther>.Min/Max/Clamp
   */
  public static UInt48 Min(UInt48 x, UInt48 y) => x < y ? x : y;
  public static UInt48 Max(UInt48 x, UInt48 y) => x > y ? x : y;
  public static UInt48 Clamp(UInt48 value, UInt48 min, UInt48 max)
    => max < min
      ? throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max))
      : new(MathClampShim.Clamp(value.ToUInt64(), min.ToUInt64(), max.ToUInt64()));

  /*
   * INumber<TOther>.DivRem
   */
  public static (UInt48 Quotient, UInt48 Remainder) DivRem(UInt48 left, UInt48 right)
  {
    var (quot, rem) = MathDivRemShim.DivRem(left.ToUInt64(), right.ToUInt64());

    return (new(quot), new(rem));
  }

#if FEATURE_GENERIC_MATH
  /*
   * INumber<TOther>.Create/TryCreate
   */
  public static UInt48 Create<TOther>(TOther value) where TOther : INumber<TOther>
  {
    if (TryCreateCore(value, out var result))
      return result;

    throw UInt24n.CreateOverflowException<UInt48>(value);
  }

  public static UInt48 CreateTruncating<TOther>(TOther value) where TOther : INumber<TOther>
  {
    if (TryCreateCore(value, out var result))
      return result;

    throw UInt24n.CreateOverflowException<UInt48>(value);
  }

  public static bool TryCreate<TOther>(TOther value, out UInt48 result) where TOther : INumber<TOther>
    => TryCreateCore(value, out result);

  private static bool TryCreateCore<TOther>(
    TOther value,
    out UInt48 result
  ) where TOther : INumber<TOther>
  {
    result = default;

    long val;

    if (typeof(TOther) == typeof(byte))
      val = (long)((byte)(object)value);
    else if (typeof(TOther) == typeof(sbyte))
      val = (long)((sbyte)(object)value);
    else if (typeof(TOther) == typeof(char))
      val = (long)((char)(object)value);
    else if (typeof(TOther) == typeof(ushort))
      val = (long)((ushort)(object)value);
    else if (typeof(TOther) == typeof(short))
      val = (long)((short)(object)value);
    else if (typeof(TOther) == typeof(uint))
      val = (long)((uint)(object)value);
    else if (typeof(TOther) == typeof(int))
      val = (long)((int)(object)value);
    else if (typeof(TOther) == typeof(ulong))
      val = unchecked((long)((ulong)(object)value));
    else if (typeof(TOther) == typeof(long))
      val = (long)(object)value;
    else if (typeof(TOther) == typeof(nuint))
      val = unchecked((long)((nuint)(object)value));
    else if (typeof(TOther) == typeof(nint))
      val = unchecked((long)((nint)(object)value));
    else if (typeof(TOther) == typeof(Half))
      val = (long)((Half)(object)value);
    else if (typeof(TOther) == typeof(float))
      val = unchecked((long)((float)(object)value));
    else if (typeof(TOther) == typeof(double))
      val = unchecked((long)((double)(object)value));
#pragma warning disable IDE0045
    else if (typeof(TOther) == typeof(decimal))
      val = unchecked((long)((decimal)(object)value));
#pragma warning restore IDE0045
    else
      throw UInt24n.CreateTypeIsNotConvertibleException<UInt48, TOther>();

    if (val is < minValue or > maxValue)
      return false; // overflow

    result = new(unchecked((ulong)val));

    return true;
  }

  public static UInt48 CreateSaturating<TOther>(TOther value) where TOther : INumber<TOther>
  {
    if (typeof(TOther) == typeof(byte)) {
      return new((ulong)((byte)(object)value));
    }
    else if (typeof(TOther) == typeof(sbyte)) {
      var val = (sbyte)(object)value;

      return val < minValue ? MinValue : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(char)) {
      return new((ulong)((char)(object)value));
    }
    else if (typeof(TOther) == typeof(ushort)) {
      return new((ulong)((ushort)(object)value));
    }
    else if (typeof(TOther) == typeof(short)) {
      var val = (short)(object)value;

      return val < minValue ? MinValue : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(uint)) {
      return new((ulong)((uint)(object)value));
    }
    else if (typeof(TOther) == typeof(int)) {
      var val = (int)(object)value;

      return val < minValue ? MinValue : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(ulong)) {
      var val = (ulong)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new(val);
    }
    else if (typeof(TOther) == typeof(long)) {
      var val = (long)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(nuint)) {
      var val = (nuint)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(nint)) {
      var val = (nint)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(Half)) {
      var val = (float)((Half)(object)value);

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(float)) {
      var val = (double)((float)(object)value);

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(double)) {
      var val = (double)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(decimal)) {
      var val = (decimal)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }

    throw UInt24n.CreateTypeIsNotConvertibleException<UInt48, TOther>();
  }
#endif

  /*
   * IBitwiseOperators
   */
  public static UInt48 operator &(UInt48 left, UInt48 right) => new(left.ToUInt64() & right.ToUInt64());
  public static UInt48 operator |(UInt48 left, UInt48 right) => new(left.ToUInt64() | right.ToUInt64());
  public static UInt48 operator ^(UInt48 left, UInt48 right) => new(left.ToUInt64() ^ right.ToUInt64());
  public static UInt48 operator ~(UInt48 value) => new(~value.ToUInt64());

  /*
   * IBinaryNumber
   */
#if SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2
  public static bool IsPow2(UInt48 value) => BitOperations.IsPow2(value.ToUInt64());
#endif
#if SYSTEM_NUMERICS_BITOPERATIONS_LOG2
  public static int Log2(UInt48 value) => BitOperations.Log2(value.ToUInt64());
#if FEATURE_GENERIC_MATH
  static UInt48 IBinaryNumber<UInt48>.Log2(UInt48 value) => new((ulong)Log2(value));
#endif
#endif

  /*
   * IShiftOperators
   */
  public static UInt48 operator <<(UInt48 value, int shiftAmount) => new(value.ToUInt64() << UInt24n.RegularizeShiftAmount(shiftAmount, BitsOfSelf));
  public static UInt48 operator >>(UInt48 value, int shiftAmount) => new(value.ToUInt64() >> UInt24n.RegularizeShiftAmount(shiftAmount, BitsOfSelf));

  /*
   * IBinaryInteger
   */
  public static UInt48 RotateLeft(UInt48 value, int rotateAmount)
  {
    if (rotateAmount == 0)
      return value;
    if (rotateAmount < 0)
      return RotateRight(value, -rotateAmount);

    rotateAmount = UInt24n.RegularizeRotateAmount(rotateAmount, BitsOfSelf);

    var val = value.ToUInt64();

    return new((val << rotateAmount) | (val >> (BitsOfSelf - rotateAmount)));
  }

  public static UInt48 RotateRight(UInt48 value, int rotateAmount)
  {
    if (rotateAmount == 0)
      return value;
    if (rotateAmount < 0)
      return RotateLeft(value, -rotateAmount);

    rotateAmount = UInt24n.RegularizeRotateAmount(rotateAmount, BitsOfSelf);

    var val = value.ToUInt64();

    return new((val >> rotateAmount) | (val << (BitsOfSelf - rotateAmount)));
  }

#if SYSTEM_NUMERICS_BITOPERATIONS_LEADINGZEROCOUNT
#if FEATURE_GENERIC_MATH
  static UInt48 IBinaryInteger<UInt48>.LeadingZeroCount(UInt48 value) => new((uint)LeadingZeroCount(value));
#endif
  public static int LeadingZeroCount(UInt48 value)
    => BitOperations.LeadingZeroCount(value.ToUInt64()) - (64 - BitsOfSelf);
#endif

#if SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
#if FEATURE_GENERIC_MATH
  static UInt48 IBinaryInteger<UInt48>.PopCount(UInt48 value) => new((uint)PopCount(value));
#endif
  public static int PopCount(UInt48 value)
    => BitOperations.PopCount(value.ToUInt64());
#endif

#if SYSTEM_NUMERICS_BITOPERATIONS_TRAILINGZEROCOUNT
#if FEATURE_GENERIC_MATH
  static UInt48 IBinaryInteger<UInt48>.TrailingZeroCount(UInt48 value) => new((uint)TrailingZeroCount(value));
#endif
  public static int TrailingZeroCount(UInt48 value)
    => BitOperations.TrailingZeroCount(value.ToUInt64() | UnusedBitMask);
#endif
}
