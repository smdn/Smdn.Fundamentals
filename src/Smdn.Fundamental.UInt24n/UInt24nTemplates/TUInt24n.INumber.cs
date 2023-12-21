// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif

using ShimTypeSystemMathClamp =
#if SYSTEM_MATH_CLAMP
  System.Math;
#else
  Smdn.MathShim;
#endif

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n {
#pragma warning restore IDE0040
  /*
   * INumber<TOther>.Sign
   */
  public static int Sign(TUInt24n value) => value == Zero ? 0 : 1;

  /*
   * INumber<TOther>.Min/Max/Clamp
   */
  public static TUInt24n Min(TUInt24n x, TUInt24n y) => x < y ? x : y;
  public static TUInt24n Max(TUInt24n x, TUInt24n y) => x > y ? x : y;
  public static TUInt24n Clamp(TUInt24n value, TUInt24n min, TUInt24n max)
    => max < min
      ? throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max))
      : new(ShimTypeSystemMathClamp.Clamp(value.Widen(), min.Widen(), max.Widen()));
}
