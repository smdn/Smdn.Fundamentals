// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType
#pragma warning restore IDE0040
#if SYSTEM_ISPANFORMATTABLE
  :
  ISpanFormattable
#endif
{
  public bool TryFormat(
    Span<char> destination,
    out int charsWritten,
    ReadOnlySpan<char> format,
#pragma warning disable IDE0060
    IFormatProvider? provider
#pragma warning restore IDE0060
  )
  {
    charsWritten = default;

    if (destination.Length < value.Length)
      return false;

    // format string can be only '' currently
    if (!format.IsEmpty)
      throw new FormatException("unsupported format string: " + format.ToString());

    value.Span.CopyTo(destination);

    charsWritten = value.Length;

    return true;
  }
}
