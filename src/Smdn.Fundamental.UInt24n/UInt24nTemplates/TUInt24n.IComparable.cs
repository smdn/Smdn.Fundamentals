// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n :
#if !FEATURE_GENERIC_MATH
  IComparable<TUInt24n>,
  IComparable,
#endif
  IComparable<TUIntWide>,
  IComparable<TIntWide>
{
#pragma warning restore IDE0040
  public int CompareTo(TUInt24n other) => Widen().CompareTo(other.Widen());

  [CLSCompliant(false)]
  public int CompareTo(TUIntWide other) => Widen().CompareTo(other);

  public int CompareTo(TIntWide other) => ToTIntWide().CompareTo(other);

  public int CompareTo(object obj)
    => obj switch {
      TUInt24n valTUInt24n => CompareTo(valTUInt24n),
      TUIntWide valTUIntWide => CompareTo(valTUIntWide),
      TIntWide valTIntWide => CompareTo(valTIntWide),
      null => 1,
      _ => throw UInt24n.CreateArgumentIsNotComparableException<TUInt24n>(obj, nameof(obj)),
    };
}
