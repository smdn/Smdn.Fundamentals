// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n {
#pragma warning restore IDE0040
  // ref: https://learn.microsoft.com/ja-jp/dotnet/csharp/language-reference/proposals/csharp-11.0/checked-user-defined-operators

  // IAdditionOperators
  public static TUInt24n operator +(TUInt24n left, TUInt24n right) => new(unchecked(left.Widen() + right.Widen()), check: false);
  public static TUInt24n operator checked +(TUInt24n left, TUInt24n right) => new(checked(left.Widen() + right.Widen()), check: true);

  // ISubtractionOperators
  public static TUInt24n operator -(TUInt24n left, TUInt24n right) => new(unchecked(left.Widen() - right.Widen()), check: false);
  public static TUInt24n operator checked -(TUInt24n left, TUInt24n right) => new(checked(left.Widen() - right.Widen()), check: true);

  // IMultiplyOperators
  public static TUInt24n operator *(TUInt24n left, TUInt24n right) => new(unchecked(left.Widen() * right.Widen()), check: false);
  public static TUInt24n operator checked *(TUInt24n left, TUInt24n right) => new(checked(left.Widen() * right.Widen()), check: true);

  // IDivisionOperators
  public static TUInt24n operator /(TUInt24n left, TUInt24n right) => new(unchecked(left.Widen() / right.Widen()), check: false);
  public static TUInt24n operator checked /(TUInt24n left, TUInt24n right) => new(checked(left.Widen() / right.Widen()), check: true);

  // IModulusOperators
  public static TUInt24n operator %(TUInt24n left, TUInt24n right) => new(unchecked(left.Widen() % right.Widen()), check: false);

  // IUnaryPlusOperators
  public static TUInt24n operator +(TUInt24n value) => new(+value.Widen(), check: false);

  // IUnaryNegationOperators
  public static TUInt24n operator -(TUInt24n value) => new(unchecked(0 - value.Widen()), check: false);
  public static TUInt24n operator checked -(TUInt24n value) => new(checked(0 - value.Widen()), check: true);

  // IIncrementOperators
  public static TUInt24n operator ++(TUInt24n value) => new(unchecked(value.Widen() + 1), check: false);
  public static TUInt24n operator checked ++(TUInt24n value) => new(checked(value.Widen() + 1), check: true);

  // IDecrementOperators
  public static TUInt24n operator --(TUInt24n value) => new(unchecked(value.Widen() - 1), check: false);
  public static TUInt24n operator checked --(TUInt24n value) => new(checked(value.Widen() - 1), check: true);
}
