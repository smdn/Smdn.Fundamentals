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
#if FEATURE_GENERIC_MATH
  /*
   * INumber<TOther>.Zero/One
   */
  static TUInt24n INumber<TUInt24n>.Zero => Zero;
  static TUInt24n INumber<TUInt24n>.One => One;
#endif

  /*
   * INumber<TOther>.Abs/Sign
   */
  public static TUInt24n Abs(TUInt24n value) => value;
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
#if FEATURE_GENERIC_MATH
  /*
   * INumber<TOther>.Create/TryCreate
   */
  public static TUInt24n Create<TOther>(TOther value) where TOther : INumber<TOther>
  {
    if (TryCreateCore(value, out var result))
      return result;

    throw UInt24n.CreateOverflowException<TUInt24n>(value);
  }

  public static TUInt24n CreateTruncating<TOther>(TOther value) where TOther : INumber<TOther>
  {
    if (TryCreateCore(value, out var result))
      return result;

    throw UInt24n.CreateOverflowException<TUInt24n>(value);
  }

  public static bool TryCreate<TOther>(TOther value, out TUInt24n result) where TOther : INumber<TOther>
    => TryCreateCore(value, out result);
#endif
}
