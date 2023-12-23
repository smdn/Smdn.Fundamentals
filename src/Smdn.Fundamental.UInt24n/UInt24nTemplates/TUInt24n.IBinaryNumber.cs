// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IBINARYNUMBER
using System.Numerics;
#endif

using ShimTypeSystemNumericsBitOperationsIsPow2 =
#if SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

using ShimTypeSystemNumericsBitOperationsLog2 =
#if SYSTEM_NUMERICS_BITOPERATIONS_LOG2
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n
#if SYSTEM_NUMERICS_IBINARYNUMBER
  : IBinaryNumber<TUInt24n>
#endif
{
#pragma warning restore IDE0040
  /*
   * IBinaryNumber
   */
  public static bool IsPow2(TUInt24n value) => ShimTypeSystemNumericsBitOperationsIsPow2.IsPow2(value.Widen());

  public static int Log2(TUInt24n value) => ShimTypeSystemNumericsBitOperationsLog2.Log2(value.Widen());
#if SYSTEM_NUMERICS_IBINARYNUMBER
  static TUInt24n IBinaryNumber<TUInt24n>.Log2(TUInt24n value) => new((TUIntWide)Log2(value), check: false);
#endif
}
