// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

#if SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2
using ShimSystemNumericsBitOperationsIsPow2 = System.Numerics.BitOperations;
#else
using ShimSystemNumericsBitOperationsIsPow2 = Smdn.BitOperationsShim;
#endif

#if SYSTEM_NUMERICS_BITOPERATIONS_LOG2
using ShimSystemNumericsBitOperationsLog2 = System.Numerics.BitOperations;
#else
using ShimSystemNumericsBitOperationsLog2 = Smdn.BitOperationsShim;
#endif

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n
#if FEATURE_GENERIC_MATH
  : IBinaryNumber<TUInt24n>
#endif
{
#pragma warning restore IDE0040
  /*
   * IBinaryNumber
   */
  public static bool IsPow2(TUInt24n value) => ShimSystemNumericsBitOperationsIsPow2.IsPow2(value.Widen());

  public static int Log2(TUInt24n value) => ShimSystemNumericsBitOperationsLog2.Log2(value.Widen());
#if FEATURE_GENERIC_MATH
  static TUInt24n IBinaryNumber<TUInt24n>.Log2(TUInt24n value) => new((TUIntWide)Log2(value));
#endif
}
