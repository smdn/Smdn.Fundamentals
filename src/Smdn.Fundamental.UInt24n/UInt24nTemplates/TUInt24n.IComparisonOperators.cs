// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n {
#pragma warning restore IDE0040
  /*
   * IComparisonOperators
   */
  public static bool operator <(TUInt24n x, TUInt24n y) => x.Widen() < y.Widen();
  public static bool operator <=(TUInt24n x, TUInt24n y) => x.Widen() <= y.Widen();
  public static bool operator >(TUInt24n x, TUInt24n y) => x.Widen() > y.Widen();
  public static bool operator >=(TUInt24n x, TUInt24n y) => x.Widen() >= y.Widen();
}
