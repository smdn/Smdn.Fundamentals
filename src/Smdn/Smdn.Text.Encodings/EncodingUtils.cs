// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text;

namespace Smdn.Text.Encodings {
  public static class EncodingUtils {
    public static Encoding GetEncoding(string name)
    {
      return GetEncoding(name, null);
    }

    private static readonly char[] whiteSpaceChars = new[] {'-', '_', ' '};

    public static Encoding GetEncoding(string name,
                                       EncodingSelectionCallback selectFallbackEncoding)
    {
      if (name == null)
        throw new ArgumentNullException(nameof(name));

      // remove leading and trailing whitespaces (\x20, \n, \t, etc.)
      name = name.Trim();

      if (!encodingCollationTable.TryGetValue(name.RemoveChars(whiteSpaceChars), out var encodingName))
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
    {
      return GetEncodingThrowException(name, null);
    }

    public static Encoding GetEncodingThrowException(string name,
                                                     EncodingSelectionCallback selectFallbackEncoding)
    {
      var encoding = GetEncoding(name, selectFallbackEncoding);

      if (encoding == null)
        throw new EncodingNotSupportedException(name);
      else
        return encoding;
    }

    private static readonly Dictionary<string, string> encodingCollationTable
      = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
      /* UTF-16 */
      {"utf16",       "utf-16"},
      /* UTF-8 */
      {"utf8",        "utf-8"},
      /* Shift_JIS */
      {"shiftjis",    "shift_jis"},     // shift_jis
      {"xsjis",       "shift_jis"},     // x-sjis
      /* EUC-JP */
      {"eucjp",       "euc-jp"},        // euc-jp
      {"xeucjp",      "euc-jp"},        // x-euc-jp
      /* ISO-2022-JP */
      {"iso2022jp",   "iso-2022-jp"},   // iso-2022-jp

      // TODO
      // {"utf16be",     "utf-16"},
      // {"utf16le",     "utf-16"},
    };
  }
}
