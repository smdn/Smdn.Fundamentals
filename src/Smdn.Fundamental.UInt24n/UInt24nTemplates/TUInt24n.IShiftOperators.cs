// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n {
#pragma warning restore IDE0040
  /*
   * IShiftOperators
   */
  public static TUInt24n operator <<(TUInt24n value, int shiftAmount) => new(value.Widen() << UInt24n.RegularizeShiftAmount(shiftAmount, BitsOfSelf));
  public static TUInt24n operator >>(TUInt24n value, int shiftAmount) => new(value.Widen() >> UInt24n.RegularizeShiftAmount(shiftAmount, BitsOfSelf));
  public static TUInt24n operator >>>(TUInt24n value, int shiftAmount) => new(value.Widen() >>> UInt24n.RegularizeShiftAmount(shiftAmount, BitsOfSelf));
}
