// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable IDE0040
#pragma warning disable CA1716
#pragma warning disable CA1008

namespace Smdn;

partial struct Uuid {
  public enum Namespace : int {
    RFC4122Dns      = 0x6ba7b810,
    RFC4122Url      = 0x6ba7b811,
    RFC4122IsoOid   = 0x6ba7b812,
    RFC4122X500     = 0x6ba7b814,
  }
}
