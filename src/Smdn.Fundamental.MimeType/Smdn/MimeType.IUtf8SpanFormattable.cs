// SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_TEXT_ASCII && SYSTEM_IUTF8SPANFORMATTABLE
using System;
using System.Buffers;
using System.Text;

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType : IUtf8SpanFormattable {
#pragma warning restore IDE0040
  public bool TryFormat(
    Span<byte> utf8Destination,
    out int bytesWritten,
    ReadOnlySpan<char> format,
    IFormatProvider? provider
  )
  {
    bytesWritten = default;

    if (utf8Destination.Length < value.Length)
      return false;

    // format string can be only '' currently
    if (!format.IsEmpty)
      throw new FormatException("unsupported format string: " + format.ToString());

    return OperationStatus.Done == Ascii.FromUtf16(value.Span, utf8Destination, out bytesWritten);
  }
}
#endif
