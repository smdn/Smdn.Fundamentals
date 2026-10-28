// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

partial struct TUInt24n {
  /*
   * IBitwiseOperators
   */
  public static TUInt24n operator &(TUInt24n left, TUInt24n right) => new(left.Widen() & right.Widen(), check: false);
  public static TUInt24n operator |(TUInt24n left, TUInt24n right) => new(left.Widen() | right.Widen(), check: false);
  public static TUInt24n operator ^(TUInt24n left, TUInt24n right) => new(left.Widen() ^ right.Widen(), check: false);
  public static TUInt24n operator ~(TUInt24n value) => new(~value.Widen(), check: false);
}
