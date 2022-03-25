// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n {
#pragma warning restore IDE0040
  public TUInt24n(byte[] value, bool isBigEndian = false)
    : this(UInt24n.ValidateAndGetSpan(value, 0, nameof(value), SizeOfSelf), isBigEndian)
  {
  }

  public TUInt24n(byte[] value, int startIndex, bool isBigEndian = false)
    : this(UInt24n.ValidateAndGetSpan(value, startIndex, nameof(value), SizeOfSelf), isBigEndian)
  {
  }
}
