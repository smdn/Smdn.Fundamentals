// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2 || SYSTEM_NUMERICS_BITOPERATIONS_LOG2
using System.Numerics;
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
#if SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2
  public static bool IsPow2(TUInt24n value) => BitOperations.IsPow2(value.Widen());
#endif
#if SYSTEM_NUMERICS_BITOPERATIONS_LOG2
  public static int Log2(TUInt24n value) => BitOperations.Log2(value.Widen());
#if FEATURE_GENERIC_MATH
  static TUInt24n IBinaryNumber<TUInt24n>.Log2(TUInt24n value) => new((TUIntWide)Log2(value));
#endif
#endif
}
