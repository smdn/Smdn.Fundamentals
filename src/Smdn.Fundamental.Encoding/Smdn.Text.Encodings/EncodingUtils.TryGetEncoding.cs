// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_TEXT_ENCODING_GETENCODING_CODEPAGE
using System;
using System.Collections.Generic;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.Text;

namespace Smdn.Text.Encodings;

#pragma warning disable IDE0040
partial class EncodingUtils {
#pragma warning restore IDE0040
  public static Encoding GetEncoding(
    string name,
    IReadOnlyDictionary<string, int>? codePageCollationTable,
    EncodingSelectionCallback? selectFallbackEncoding
  )
  {
    var result = TryGetEncoding(
      name ?? throw new ArgumentNullException(nameof(name)),
      codePageCollationTable,
      selectFallbackEncoding,
      out var encoding
    );

    return result
      ? encoding!
      : throw new EncodingNotSupportedException(name);
  }

  public static bool TryGetEncoding(
    string? name,
    IReadOnlyDictionary<string, int>? codePageCollationTable,
    EncodingSelectionCallback? selectFallbackEncoding,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out Encoding? encoding
  )
  {
    encoding = null;

    if (name is null)
      return false;

    // remove leading and trailing whitespaces (\x20, \n, \t, etc.)
    name = name.Trim();

    // attempt to get Encoding with the code page collated by the specified encoding name
    if (
      codePageCollationTable is not null &&
      codePageCollationTable.TryGetValue(name, out var codePage) &&
      0 <= codePage
    ) {
      try {
        encoding = Encoding.GetEncoding(codePage);
        return true;
      }
      catch (ArgumentException) {
        // continue
      }
      catch (NotSupportedException) {
        // continue
      }
    }

    // attempt to get Encoding with the specified encoding name
    try {
      encoding = Encoding.GetEncoding(name);
      return true;
    }
    catch (ArgumentException) {
      // continue
    }
    catch (NotSupportedException) {
      // continue
    }

    // select fallback encoding
    if (selectFallbackEncoding is not null)
      encoding = selectFallbackEncoding(name);

    return encoding is not null;
  }
}
#endif
