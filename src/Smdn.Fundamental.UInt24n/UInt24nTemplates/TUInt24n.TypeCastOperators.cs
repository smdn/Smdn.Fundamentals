// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n {
#pragma warning restore IDE0040
  /*
   * widening type castings (UInt24n <-> UInt32n)
   */
  [CLSCompliant(false)]
  public static explicit operator TUInt24n(TUIntWide val)
    => maxValue < val ? throw UInt24n.CreateOverflowException<TUInt24n>(val) : new(val);

  public static explicit operator TUInt24n(TIntWide val)
    => val is < minValue or > maxValue ? throw UInt24n.CreateOverflowException<TUInt24n>(val) : new(unchecked((TUIntWide)val));

  [CLSCompliant(false)]
  public static explicit operator TUIntWide(TUInt24n val) => val.Widen();

  public static explicit operator TIntWide(TUInt24n val) => unchecked((TIntWide)val.Widen());

  /*
   * narrowing type castings (UInt24n <-> UInt16n)
   */

  [CLSCompliant(false)]
  public static explicit operator TUInt24n(TUIntNarrow val)
    => new((TUIntWide)val);

  public static explicit operator TUInt24n(TIntNarrow val)
    => val < minValue ? throw UInt24n.CreateOverflowException<TUInt24n>(val) : new((TUIntWide)val);

  [CLSCompliant(false)]
  public static explicit operator TUIntNarrow(TUInt24n val) => checked((TUIntNarrow)val.Widen());

  public static explicit operator TIntNarrow(TUInt24n val) => checked((TIntNarrow)val.Widen());
}
