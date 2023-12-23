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
  public static explicit operator TUInt24n(TUIntWide val) => new(val, check: false);

  [CLSCompliant(false)]
  public static explicit operator checked TUInt24n(TUIntWide val) => new(val, check: true);

  public static explicit operator TUInt24n(TIntWide val) => new(unchecked((TUIntWide)val), check: false);

  public static explicit operator checked TUInt24n(TIntWide val) => new(checked((TUIntWide)val), check: true);

  [CLSCompliant(false)]
  public static explicit operator TUIntWide(TUInt24n val) => val.Widen();

  public static explicit operator TIntWide(TUInt24n val) => unchecked((TIntWide)val.Widen());

  public static explicit operator checked TIntWide(TUInt24n val) => checked((TIntWide)val.Widen());

  /*
   * narrowing type castings (UInt24n <-> UInt16n)
   */

  [CLSCompliant(false)]
  public static explicit operator TUInt24n(TUIntNarrow val) => new((TUIntWide)val, check: false);

  public static explicit operator TUInt24n(TIntNarrow val) => new(unchecked((TUIntWide)val), check: false);

  public static explicit operator checked TUInt24n(TIntNarrow val) => new(checked((TUIntWide)val), check: true);

  [CLSCompliant(false)]
  public static explicit operator TUIntNarrow(TUInt24n val) => unchecked((TUIntNarrow)val.Widen());

  [CLSCompliant(false)]
  public static explicit operator checked TUIntNarrow(TUInt24n val) => checked((TUIntNarrow)val.Widen());

  public static explicit operator TIntNarrow(TUInt24n val) => unchecked((TIntNarrow)val.Widen());

  public static explicit operator checked TIntNarrow(TUInt24n val) => checked((TIntNarrow)val.Widen());
}
