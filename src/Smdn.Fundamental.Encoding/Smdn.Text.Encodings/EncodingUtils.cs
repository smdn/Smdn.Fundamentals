// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text;

namespace Smdn.Text.Encodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static partial class EncodingUtils {
  public static Encoding? GetEncoding(string name)
    => GetEncoding(name, null);

  private static readonly char[] WhiteSpaceChars = new[] { '-', '_', ' ' };

  private static string NormalizeEncodingName(string name)
  {
    var normalizedName = name.Trim(); // remove leading and trailing whitespaces (\x20, \n, \t, etc.)
    var normalizedNameBuffer = new StringBuilder(normalizedName.Length);
    var lastIndex = 0;

    for (; ; ) {
      var index = normalizedName.IndexOfAny(WhiteSpaceChars, lastIndex);

      if (index < 0) {
#if SYSTEM_TEXT_STRINGBUILDER_APPEND_READONLYSPAN_OF_CHAR
        normalizedNameBuffer.Append(normalizedName.AsSpan(lastIndex));
#else
        normalizedNameBuffer.Append(normalizedName.Substring(lastIndex));
#endif

        if (normalizedName.Length == normalizedNameBuffer.Length)
          return normalizedName;
        else
          return normalizedNameBuffer.ToString();
      }
      else {
#if SYSTEM_TEXT_STRINGBUILDER_APPEND_READONLYSPAN_OF_CHAR
        normalizedNameBuffer.Append(normalizedName.AsSpan(lastIndex, index - lastIndex));
#else
        normalizedNameBuffer.Append(normalizedName.Substring(lastIndex, index - lastIndex));
#endif

        lastIndex = index + 1;
      }
    }
  }

  public static Encoding? GetEncoding(
    string name,
    EncodingSelectionCallback? selectFallbackEncoding
  )
  {
    if (name == null)
      throw new ArgumentNullException(nameof(name));

    if (!EncodingCollationTable.TryGetValue(NormalizeEncodingName(name), out var encodingName))
      encodingName = name;

    try {
      return Encoding.GetEncoding(encodingName);
    }
    catch (ArgumentException) {
      // illegal or unsupported
      if (selectFallbackEncoding == null)
        return null;
      else
        return selectFallbackEncoding(name); // trimmed name
    }
  }

  public static Encoding GetEncodingThrowException(string name)
    => GetEncodingThrowException(name, null);

  public static Encoding GetEncodingThrowException(
    string name,
    EncodingSelectionCallback? selectFallbackEncoding
  )
    => GetEncoding(name, selectFallbackEncoding) ?? throw new EncodingNotSupportedException(name);

  // cSpell:disable
  private static readonly Dictionary<string, string> EncodingCollationTable
    = new(StringComparer.OrdinalIgnoreCase) {
      /* UTF-16 */
      { "utf16",       "utf-16" },
      /* UTF-8 */
      { "utf8",        "utf-8" },
      /* Shift_JIS */
      { "shiftjis",    "shift_jis" },     // shift_jis
      { "xsjis",       "shift_jis" },     // x-sjis
      /* EUC-JP */
      { "eucjp",       "euc-jp" },        // euc-jp
      { "xeucjp",      "euc-jp" },        // x-euc-jp
      /* ISO-2022-JP */
      { "iso2022jp",   "iso-2022-jp" },   // iso-2022-jp

      // TODO
      // {"utf16be",     "utf-16"},
      // {"utf16le",     "utf-16"},
    };
  // cSpell:enable
}
