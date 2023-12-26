// SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_INUMBERBASE
using System.Numerics;
#endif

using ShimTypeSystemMathClamp =
#if SYSTEM_MATH_CLAMP
  System.Math;
#else
  Smdn.MathShim;
#endif

using ShimTypeSystemMathDivRemReturnValueTuple2 =
#if SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
  System.Math;
#else
  Smdn.MathShim;
#endif

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n {
#pragma warning restore IDE0040
#if SYSTEM_NUMERICS_INUMBERBASE
  /*
   * INumberBase<TOther>.Zero/One
   */
  static TUInt24n INumberBase<TUInt24n>.Zero => Zero;
  static TUInt24n INumberBase<TUInt24n>.One => One;

  /*
   * INumberBase<TOther>.Radix
   */
  static int INumberBase<TUInt24n>.Radix => 2;

  /*
   * INumberBase<TOther>.IsCanonical/IsNormal/IsSubnormal
   */
  static bool INumberBase<TUInt24n>.IsCanonical(TUInt24n value) => true;
  static bool INumberBase<TUInt24n>.IsNormal(TUInt24n value) => value != Zero;
  static bool INumberBase<TUInt24n>.IsSubnormal(TUInt24n value) => false;

  /*
   * INumberBase<TOther>.IsComplexNumber/IsImaginaryNumber/IsRealNumber
   */
  static bool INumberBase<TUInt24n>.IsComplexNumber(TUInt24n value) => false;
  static bool INumberBase<TUInt24n>.IsImaginaryNumber(TUInt24n value) => false;
  static bool INumberBase<TUInt24n>.IsRealNumber(TUInt24n value) => true;

  /*
   * INumberBase<TOther>.IsEvenInteger/IsOddInteger
   */
  static bool INumberBase<TUInt24n>.IsEvenInteger(TUInt24n value) => (value & One) == Zero;
  static bool INumberBase<TUInt24n>.IsOddInteger(TUInt24n value) => (value & One) != Zero;

  /*
   * INumberBase<TOther>.IsFinite/IsInfinity/IsPositiveInfinity/IsNegativeInfinity
   */
  static bool INumberBase<TUInt24n>.IsFinite(TUInt24n value) => true;
  static bool INumberBase<TUInt24n>.IsInfinity(TUInt24n value) => false;
  static bool INumberBase<TUInt24n>.IsPositiveInfinity(TUInt24n value) => false;
  static bool INumberBase<TUInt24n>.IsNegativeInfinity(TUInt24n value) => false;

  /*
   * INumberBase<TOther>.IsNaN/IsInteger/IsZero
   */
  static bool INumberBase<TUInt24n>.IsNaN(TUInt24n value) => false;
  static bool INumberBase<TUInt24n>.IsInteger(TUInt24n value) => true;
  static bool INumberBase<TUInt24n>.IsZero(TUInt24n value) => value == Zero;

  /*
   * INumberBase<TOther>.IsPositive/IsNegative
   */
  static bool INumberBase<TUInt24n>.IsPositive(TUInt24n value) => true;
  static bool INumberBase<TUInt24n>.IsNegative(TUInt24n value) => false; // unsigned
#endif

  /*
   * INumberBase<TOther>.Abs
   */
  public static TUInt24n Abs(TUInt24n value) => value;

#if SYSTEM_NUMERICS_INUMBERBASE
  /*
   * INumberBase<TOther>.MaxMagnitude/MaxMagnitudeNumber
   */
  static TUInt24n INumberBase<TUInt24n>.MaxMagnitude(TUInt24n x, TUInt24n y) => Max(x, y);
  static TUInt24n INumberBase<TUInt24n>.MaxMagnitudeNumber(TUInt24n x, TUInt24n y) => Max(x, y);

  /*
   * INumberBase<TOther>.MinMagnitude/MinMagnitudeNumber
   */
  static TUInt24n INumberBase<TUInt24n>.MinMagnitude(TUInt24n x, TUInt24n y) => Min(x, y);
  static TUInt24n INumberBase<TUInt24n>.MinMagnitudeNumber(TUInt24n x, TUInt24n y) => Min(x, y);

  /*
   * INumberBase<TOther>.CreateChecked/CreateSaturating/CreateTruncating
   */
  public static TUInt24n CreateChecked<TOther>(TOther value) where TOther : INumberBase<TOther>
  {
    if (!TryConvertFromTruncating(value, throwIfOverflow: true, out var result, out _))
      throw UInt24n.CreateTypeIsNotConvertibleException<TUInt24n, TOther>();

    return result;
  }

  public static TUInt24n CreateSaturating<TOther>(TOther value) where TOther : INumberBase<TOther>
  {
    if (!TryConvertFromSaturating(value, out var result))
      throw UInt24n.CreateTypeIsNotConvertibleException<TUInt24n, TOther>();

    return result;
  }

  public static TUInt24n CreateTruncating<TOther>(TOther value) where TOther : INumberBase<TOther>
  {
    if (!TryConvertFromTruncating(value, throwIfOverflow: false, out var result, out _))
      throw UInt24n.CreateTypeIsNotConvertibleException<TUInt24n, TOther>();

    return result;
  }

  /*
   * INumberBase<TOther>.TryConvertFromChecked/TryConvertFromTruncating
   */
  public static bool TryConvertFromChecked<TOther>(TOther value, out TUInt24n result) where TOther : INumberBase<TOther>
    => TryConvertFromTruncating(value, throwIfOverflow: true, out result, out _);

  static bool INumberBase<TUInt24n>.TryConvertFromTruncating<TOther>(TOther value, out TUInt24n result)
    => TryConvertFromTruncating(value, throwIfOverflow: false, out result, out _);

  /*
   * INumberBase<TOther>.TryConvertToChecked/TryConvertToSaturating/TryConvertToTruncating
   */
  static bool INumberBase<TUInt24n>.TryConvertToChecked<TOther>(TUInt24n value, out TOther result)
    => throw new NotImplementedException();

  static bool INumberBase<TUInt24n>.TryConvertToSaturating<TOther>(TUInt24n value, out TOther result)
    => throw new NotImplementedException();

  static bool INumberBase<TUInt24n>.TryConvertToTruncating<TOther>(TUInt24n value, out TOther result)
    => throw new NotImplementedException();

  [Obsolete($"Use {nameof(CreateChecked)}")]
  public static TUInt24n Create<TOther>(TOther value) where TOther : INumberBase<TOther>
    => CreateChecked(value);

  [Obsolete($"Use {nameof(TryConvertFromTruncating)} or {nameof(TryConvertFromChecked)}")]
  public static bool TryCreate<TOther>(TOther value, out TUInt24n result) where TOther : INumberBase<TOther>
    => TryConvertFromTruncating(value, throwIfOverflow: false, out result, out var overflow) && !overflow;
#endif
}
