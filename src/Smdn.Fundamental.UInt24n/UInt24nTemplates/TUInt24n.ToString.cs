// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace Smdn;

partial struct TUInt24n {
  public override string ToString() => ToString(null, null);
  public string ToString(string? format) => ToString(format, null);
  public string ToString(IFormatProvider? formatProvider) => ToString(null, formatProvider);
}
