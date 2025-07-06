// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable IDE0040
#pragma warning disable CA1028
#pragma warning disable CA1700

namespace Smdn;

partial struct Uuid {
  public enum Variant : byte {
    NCSReserved           = 0x00,
    RFC4122               = 0x80,
    MicrosoftReserved     = 0xc0,
    Reserved              = 0xe0,
  }
}
