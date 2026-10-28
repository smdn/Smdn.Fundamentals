// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

partial struct TUInt24n {
  /*
   * IEqualityOperators
   */
  public static bool operator ==(TUInt24n x, TUInt24n y) => x.Widen() == y.Widen();
  public static bool operator !=(TUInt24n x, TUInt24n y) => x.Widen() != y.Widen();
}
