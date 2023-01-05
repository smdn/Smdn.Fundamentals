// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid :
#pragma warning restore IDE0040
#pragma warning disable format
#if FEATURE_GENERIC_MATH
  IEqualityOperators<Uuid, Uuid, bool>,
#else
  IEquatable<Uuid>,
#endif
  IEquatable<Guid>
{
#pragma warning restore format
  public static bool operator ==(Uuid x, Uuid y) => x.fields_high == y.fields_high && x.fields_low == y.fields_low;
  public static bool operator !=(Uuid x, Uuid y) => x.fields_high != y.fields_high || x.fields_low != y.fields_low;

  public override bool Equals(object obj)
    => obj switch {
      Uuid uuid => Equals(uuid),
      Guid guid => Equals(guid),
      _ => false,
    };

  public bool Equals(Guid other) => this == (Uuid)other;
  public bool Equals(Uuid other) => this == other;
}
