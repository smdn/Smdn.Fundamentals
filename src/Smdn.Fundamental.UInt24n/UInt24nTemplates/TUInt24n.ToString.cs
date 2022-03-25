// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n {
#pragma warning restore IDE0040
  public override string ToString() => ToString(null, null);
  public string ToString(string? format) => ToString(format, null);
  public string ToString(IFormatProvider? formatProvider) => ToString(null, formatProvider);
}
