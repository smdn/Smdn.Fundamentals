// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid :
#pragma warning restore IDE0040
  IEquatable<Uuid>,
  IEquatable<Guid>
{
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
