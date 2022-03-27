// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

#if SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
using ShimSystemNumericsBitOperationsPopCount = System.Numerics.BitOperations;
#else
using ShimSystemNumericsBitOperationsPopCount = Smdn.BitOperationsShim;
#endif

#if SYSTEM_NUMERICS_BITOPERATIONS_LEADINGZEROCOUNT
using ShimSystemNumericsBitOperationsLeadingZeroCount = System.Numerics.BitOperations;
#else
using ShimSystemNumericsBitOperationsLeadingZeroCount = Smdn.BitOperationsShim;
#endif

#if SYSTEM_NUMERICS_BITOPERATIONS_TRAILINGZEROCOUNT
using ShimSystemNumericsBitOperationsTrailingZeroCount = System.Numerics.BitOperations;
#else
using ShimSystemNumericsBitOperationsTrailingZeroCount = Smdn.BitOperationsShim;
#endif
namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n
#if FEATURE_GENERIC_MATH
  : IBinaryInteger<TUInt24n>
#endif
{
#pragma warning restore IDE0040
  /*
   * IBinaryInteger
   */
  public static TUInt24n RotateLeft(TUInt24n value, int rotateAmount)
  {
    if (rotateAmount == 0)
      return value;
    if (rotateAmount < 0)
      return RotateRight(value, -rotateAmount);

    rotateAmount = UInt24n.RegularizeRotateAmount(rotateAmount, BitsOfSelf);

    var val = value.Widen();

    return new((val << rotateAmount) | (val >> (BitsOfSelf - rotateAmount)));
  }

  public static TUInt24n RotateRight(TUInt24n value, int rotateAmount)
  {
    if (rotateAmount == 0)
      return value;
    if (rotateAmount < 0)
      return RotateLeft(value, -rotateAmount);

    rotateAmount = UInt24n.RegularizeRotateAmount(rotateAmount, BitsOfSelf);

    var val = value.Widen();

    return new((val >> rotateAmount) | (val << (BitsOfSelf - rotateAmount)));
  }

#if FEATURE_GENERIC_MATH
  static TUInt24n IBinaryInteger<TUInt24n>.LeadingZeroCount(TUInt24n value) => new((TUIntWide)LeadingZeroCount(value));
#endif
  public static int LeadingZeroCount(TUInt24n value)
    => ShimSystemNumericsBitOperationsLeadingZeroCount.LeadingZeroCount(value.Widen()) - (bitCountOfTUIntWide - BitsOfSelf);

  private const int bitCountOfTUIntWide = sizeof(TUIntWide) * 8;

#if FEATURE_GENERIC_MATH
  static TUInt24n IBinaryInteger<TUInt24n>.PopCount(TUInt24n value) => new((TUIntWide)PopCount(value));
#endif
  public static int PopCount(TUInt24n value)
    => ShimSystemNumericsBitOperationsPopCount.PopCount(value.Widen());

#if FEATURE_GENERIC_MATH
  static TUInt24n IBinaryInteger<TUInt24n>.TrailingZeroCount(TUInt24n value) => new((TUIntWide)TrailingZeroCount(value));
#endif
  public static int TrailingZeroCount(TUInt24n value)
    => ShimSystemNumericsBitOperationsTrailingZeroCount.TrailingZeroCount(value.Widen() | UnusedBitMask);
}
