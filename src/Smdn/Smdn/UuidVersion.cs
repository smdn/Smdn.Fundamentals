// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn {
  /*
   * RFC 4122 - A Universally Unique IDentifier (UUID) URN Namespace
   * http://tools.ietf.org/html/rfc4122
   */
  public enum UuidVersion : byte {
    None      = 0x0,

    /// <summary>The time-based version</summary>
    Version1  = 0x1,
    /// <summary>DCE Security version, with embedded POSIX UIDs</summary>
    Version2  = 0x2,
    /// <summary>The name-based version that uses MD5 hashing</summary>
    Version3  = 0x3,
    /// <summary>The randomly or pseudo-randomly generated version</summary>
    Version4  = 0x4,
    /// <summary>The name-based version that uses SHA-1 hashing</summary>
    Version5  = 0x5,

    TimeBased           = Version1,
    NameBasedMD5Hash    = Version3,
    NameBasedSHA1Hash   = Version5,
    RandomNumber        = Version4,
  }
}
