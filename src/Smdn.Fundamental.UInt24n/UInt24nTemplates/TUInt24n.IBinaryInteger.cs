// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IBINARYINTEGER
using System.Numerics;
#endif

using ShimTypeSystemMathDivRemReturnValueTuple2 =
#if SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
  System.Math;
#else
  Smdn.MathShim;
#endif

using ShimTypeSystemNumericsBitOperationsPopCount =
#if SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

using ShimTypeSystemNumericsBitOperationsLeadingZeroCount =
#if SYSTEM_NUMERICS_BITOPERATIONS_LEADINGZEROCOUNT
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

using ShimTypeSystemNumericsBitOperationsTrailingZeroCount =
#if SYSTEM_NUMERICS_BITOPERATIONS_TRAILINGZEROCOUNT
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n
#if SYSTEM_NUMERICS_IBINARYINTEGER
  : IBinaryInteger<TUInt24n>
#endif
{
#pragma warning restore IDE0040
  /*
   * IBinaryInteger
   */
  public static (TUInt24n Quotient, TUInt24n Remainder) DivRem(TUInt24n left, TUInt24n right)
  {
    var (quot, rem) = ShimTypeSystemMathDivRemReturnValueTuple2.DivRem(left.Widen(), right.Widen());

    return (new(quot, check: false), new(rem, check: false));
  }

  public static TUInt24n RotateLeft(TUInt24n value, int rotateAmount)
  {
    if (rotateAmount == 0)
      return value;
    if (rotateAmount < 0)
      return RotateRight(value, -rotateAmount);

    rotateAmount = UInt24n.RegularizeRotateAmount(rotateAmount, BitsOfSelf);

    var val = value.Widen();

    return new((val << rotateAmount) | (val >> (BitsOfSelf - rotateAmount)), check: false);
  }

  public static TUInt24n RotateRight(TUInt24n value, int rotateAmount)
  {
    if (rotateAmount == 0)
      return value;
    if (rotateAmount < 0)
      return RotateLeft(value, -rotateAmount);

    rotateAmount = UInt24n.RegularizeRotateAmount(rotateAmount, BitsOfSelf);

    var val = value.Widen();

    return new((val >> rotateAmount) | (val << (BitsOfSelf - rotateAmount)), check: false);
  }

#if SYSTEM_NUMERICS_IBINARYINTEGER
  static TUInt24n IBinaryInteger<TUInt24n>.LeadingZeroCount(TUInt24n value) => new((TUIntWide)LeadingZeroCount(value), check: false);
#endif
  public static int LeadingZeroCount(TUInt24n value)
    => ShimTypeSystemNumericsBitOperationsLeadingZeroCount.LeadingZeroCount(value.Widen()) - (bitCountOfTUIntWide - BitsOfSelf);

  private const int bitCountOfTUIntWide = sizeof(TUIntWide) * 8;

#if SYSTEM_NUMERICS_IBINARYINTEGER
  static TUInt24n IBinaryInteger<TUInt24n>.PopCount(TUInt24n value) => new((TUIntWide)PopCount(value), check: false);
#endif
  public static int PopCount(TUInt24n value)
    => ShimTypeSystemNumericsBitOperationsPopCount.PopCount(value.Widen());

#if SYSTEM_NUMERICS_IBINARYINTEGER
  static TUInt24n IBinaryInteger<TUInt24n>.TrailingZeroCount(TUInt24n value) => new((TUIntWide)TrailingZeroCount(value), check: false);
#endif
  public static int TrailingZeroCount(TUInt24n value)
    => ShimTypeSystemNumericsBitOperationsTrailingZeroCount.TrailingZeroCount(value.Widen() | UnusedBitMask);

#if SYSTEM_NUMERICS_IBINARYINTEGER
  int IBinaryInteger<TUInt24n>.GetByteCount() => SizeOfSelf;

  int IBinaryInteger<TUInt24n>.GetShortestBitLength() => BitsOfSelf - LeadingZeroCount(this);
#endif

  public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out TUInt24n value)
  {
    value = default;

    if (source.Length < SizeOfSelf)
      return false;

    value = new(source, isBigEndian: true);

    return true;
  }

  public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out TUInt24n value)
  {
    value = default;

    if (source.Length < SizeOfSelf)
      return false;

    value = new(source, isBigEndian: false);

    return true;
  }
}
