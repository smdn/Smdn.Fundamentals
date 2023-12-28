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

    var requiredLength = Type.Length + 1 + SubType.Length;

    if (utf8Destination.Length < requiredLength)
      return false;

    // format string can be only '' currently
    if (!format.IsEmpty)
      return false;

    bytesWritten = 0;

    if (OperationStatus.Done != Ascii.FromUtf16(Type.AsSpan(), utf8Destination, out var bytesWrittenForType))
      return false;

    bytesWritten += bytesWrittenForType;
    utf8Destination = utf8Destination.Slice(bytesWrittenForType);

    utf8Destination[0] = (byte)'/';

    bytesWritten += 1;
    utf8Destination = utf8Destination.Slice(1);

    if (OperationStatus.Done != Ascii.FromUtf16(SubType.AsSpan(), utf8Destination, out var bytesWrittenForSubType))
      return false;

    bytesWritten += bytesWrittenForSubType;

    return true;
  }
}
#endif
