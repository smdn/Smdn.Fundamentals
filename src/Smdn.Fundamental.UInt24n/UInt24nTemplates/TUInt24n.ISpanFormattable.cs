// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace Smdn;

#if SYSTEM_ISPANFORMATTABLE
#pragma warning disable IDE0040
partial struct TUInt24n : ISpanFormattable {
#pragma warning restore IDE0040
  // ISpanFormattable
  public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    => Widen().TryFormat(destination, out charsWritten, format, provider);
}
#endif
