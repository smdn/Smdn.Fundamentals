// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

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
  public static bool IsPow2(TUInt24n value) => ShimTypeSystemNumericsBitOperationsIsPow2.IsPow2(value.Widen());

  public static int Log2(TUInt24n value) => ShimTypeSystemNumericsBitOperationsLog2.Log2(value.Widen());
#if FEATURE_GENERIC_MATH
  static TUInt24n IBinaryNumber<TUInt24n>.Log2(TUInt24n value) => new((TUIntWide)Log2(value));
#endif
}
