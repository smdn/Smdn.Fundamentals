// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType :
#pragma warning restore IDE0040
  IFormattable {
  public string ToString(string? format, IFormatProvider? formatProvider)
  {
    char[]? destination = null;

    try {
      destination = ArrayPool<char>.Shared.Rent(value.Length);

      _ = TryFormat(
        destination,
        out var charsWritten,
        (format ?? string.Empty).AsSpan(),
        formatProvider
      );

      return new(destination, 0, charsWritten);
    }
    finally {
      if (destination is not null)
        ArrayPool<char>.Shared.Return(destination);
    }
  }
}
