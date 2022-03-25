// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n : IFormattable {
#pragma warning restore IDE0040
  // IFormattable
  public string ToString(string? format, IFormatProvider? formatProvider)
    => Widen().ToString(format, formatProvider);
}
