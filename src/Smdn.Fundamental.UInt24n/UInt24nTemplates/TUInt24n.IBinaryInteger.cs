// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_NUMERICS_BITOPERATIONS_LEADINGZEROCOUNT
#define SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
#define SYSTEM_NUMERICS_BITOPERATIONS_TRAILINGZEROCOUNT
#endif

using System;
#if SYSTEM_NUMERICS_BITOPERATIONS_LEADINGZEROCOUNT || SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT || SYSTEM_NUMERICS_BITOPERATIONS_TRAILINGZEROCOUNT
using System.Numerics;
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

#if SYSTEM_NUMERICS_BITOPERATIONS_LEADINGZEROCOUNT
#if FEATURE_GENERIC_MATH
  static TUInt24n IBinaryInteger<TUInt24n>.LeadingZeroCount(TUInt24n value) => new((TUIntWide)LeadingZeroCount(value));
#endif
  public static int LeadingZeroCount(TUInt24n value)
    => BitOperations.LeadingZeroCount(value.Widen()) - (bitCountOfTUIntWide - BitsOfSelf);

  private const int bitCountOfTUIntWide = sizeof(TUIntWide) * 8;
#endif

#if SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
#if FEATURE_GENERIC_MATH
  static TUInt24n IBinaryInteger<TUInt24n>.PopCount(TUInt24n value) => new((TUIntWide)PopCount(value));
#endif
  public static int PopCount(TUInt24n value)
    => BitOperations.PopCount(value.Widen());
#endif

#if SYSTEM_NUMERICS_BITOPERATIONS_TRAILINGZEROCOUNT
#if FEATURE_GENERIC_MATH
  static TUInt24n IBinaryInteger<TUInt24n>.TrailingZeroCount(TUInt24n value) => new((TUIntWide)TrailingZeroCount(value));
#endif
  public static int TrailingZeroCount(TUInt24n value)
    => BitOperations.TrailingZeroCount(value.Widen() | UnusedBitMask);
#endif
}
