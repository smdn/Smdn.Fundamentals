// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace Smdn;

partial struct TUInt24n : IFormattable {
  // IFormattable
  public string ToString(string? format, IFormatProvider? formatProvider)
    => Widen().ToString(format, formatProvider);
}
