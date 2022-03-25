// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n :
#if !FEATURE_GENERIC_MATH
  IEquatable<TUInt24n>,
#endif
  IEquatable<TUIntWide>,
  IEquatable<TIntWide>
{
#pragma warning restore IDE0040
  public bool Equals(TUInt24n other) => this.Widen() == other.Widen();

  [CLSCompliant(false)]
  public bool Equals(TUIntWide other) => Widen() == other;

  public bool Equals(TIntWide other) => ToTIntWide() == other;

  public override bool Equals(object obj)
    => obj switch {
      TUInt24n valTUInt24n => Equals(valTUInt24n),
      TUIntWide valTUIntWide => Equals(valTUIntWide),
      TIntWide valTIntWide => Equals(valTIntWide),
      _ => false,
    };
}
