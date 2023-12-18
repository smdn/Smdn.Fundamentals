// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Globalization;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

#pragma warning disable IDE0040
partial struct Node
#pragma warning restore IDE0040
#if FEATURE_GENERIC_MATH
  :
  IParsable<Node>,
  ISpanParsable<Node>
#endif
{
  public static Node Parse(string s, IFormatProvider? provider = null)
    => TryParse((s ?? throw new ArgumentNullException(nameof(s))).AsSpan(), provider, out var result)
      ? result
      : throw new FormatException("invalid format");

  public static Node Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    => TryParse(s, provider, out var result)
      ? result
      : throw new FormatException("invalid format");

  public static bool TryParse(string? s, out Node result)
    => TryParse(s, provider: null, out result);

  public static bool TryParse(string? s, IFormatProvider? provider, out Node result)
  {
    result = default;

    return s is not null && TryParse(s.AsSpan(), provider, out result);
  }

  public static bool TryParse(ReadOnlySpan<char> s, out Node result)
    => TryParse(s, provider: null, out result);

  public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Node result)
  {
    result = default;

    const char Delimiter = ':';
    Span<byte> node = stackalloc byte[SizeOfSelf];

    for (var n = 0; n < SizeOfSelf; n++) {
      ReadOnlySpan<char> span;

      if (n < SizeOfSelf - 1) {
        var indexOfDelimiter = s.IndexOf(Delimiter);

        if (indexOfDelimiter < 0)
          return false;

        span = s.Slice(0, indexOfDelimiter);
        s = s.Slice(indexOfDelimiter + 1);
      }
      else {
        span = s;
      }

      if (
        !byte.TryParse(
          span.ToParsableType(),
          NumberStyles.HexNumber,
          provider: null,
          out var parsed
        )
      ) {
        return false;
      }

      node[n] = parsed;
    }

    result = new(node);

    return true;
  }
}
