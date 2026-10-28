// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IEQUALITYOPERATORS
using System.Numerics;
#endif

namespace Smdn;

partial struct Uuid :
#pragma warning disable format
#if SYSTEM_NUMERICS_IEQUALITYOPERATORS
  IEqualityOperators<Uuid, Uuid, bool>,
#endif
  IEquatable<Uuid>,
  IEquatable<Guid>
{
#pragma warning restore format

#pragma warning disable SA1315
  // In `IEqualityOperators`, the parameter names are `left` and `right`,
  // but since this has already been released as part of the public API,
  // the current names must be kept.
  public static bool operator ==(Uuid x, Uuid y) => x.fields_high == y.fields_high && x.fields_low == y.fields_low;
  public static bool operator !=(Uuid x, Uuid y) => x.fields_high != y.fields_high || x.fields_low != y.fields_low;
#pragma warning restore SA1315

  public override bool Equals(object? obj)
    => obj switch {
      Uuid uuid => Equals(uuid),
      Guid guid => Equals(guid),
      _ => false,
    };

  public bool Equals(Guid other) => this == (Uuid)other;
  public bool Equals(Uuid other) => this == other;
}
