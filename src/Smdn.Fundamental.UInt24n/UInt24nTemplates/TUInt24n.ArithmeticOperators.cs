// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n {
#pragma warning restore IDE0040
  // IAdditionOperators
  public static TUInt24n operator +(TUInt24n left, TUInt24n right) => new(unchecked(left.Widen() + right.Widen())); // TODO: checked

  // ISubtractionOperators
  public static TUInt24n operator -(TUInt24n left, TUInt24n right) => new(unchecked(left.Widen() - right.Widen())); // TODO: checked

  // IMultiplyOperators
  public static TUInt24n operator *(TUInt24n left, TUInt24n right) => new(unchecked(left.Widen() * right.Widen())); // TODO: checked

  // IDivisionOperators
  public static TUInt24n operator /(TUInt24n left, TUInt24n right) => new(left.Widen() / right.Widen()); // TODO: checked

  // IModulusOperators
  public static TUInt24n operator %(TUInt24n left, TUInt24n right) => new(left.Widen() % right.Widen()); // TODO: checked

  // IUnaryPlusOperators
  public static TUInt24n operator +(TUInt24n value) => new(+value.Widen()); // TODO: checked

  // IUnaryNegationOperators
  public static TUInt24n operator -(TUInt24n value) => new(unchecked(0 - value.Widen())); // TODO: checked

  // IIncrementOperators
  public static TUInt24n operator ++(TUInt24n value) => new(unchecked(value.Widen() + 1)); // TODO: checked

  // IDecrementOperators
  public static TUInt24n operator --(TUInt24n value) => new(unchecked(value.Widen() - 1)); // TODO: checked
}
