// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

using Smdn.Text;

namespace Smdn.Formats {
  public static class CsvRecord {
    // http://www.ietf.org/rfc/rfc4180.txt
    // Common Format and MIME Type for Comma-Separated Values (CSV) Files
    public static IEnumerable<string> ToSplittedNullable(string csv)
      => csv == null ? null : ToSplitted(csv);

    public static IEnumerable<string> ToSplitted(string csv)
    {
      if (csv == null)
        throw new ArgumentNullException(nameof(csv));

      if (csv.Length == 0)
        return Enumerable.Empty<string>();

      return Split(csv);

      static IEnumerable<string> Split(string sequence)
      {
        // append dummy splitter
        sequence += ",";

        var splitAt = 0;
        var quoted = false;
        var inQuote = false;

        for (var index = 0; index < sequence.Length; index++) {
          if (sequence[index] == Ascii.Chars.DQuote) {
            inQuote = !inQuote;
            quoted = true;
          }

          if (inQuote)
            continue;

          if (sequence[index] != Ascii.Chars.Comma)
            continue;

          if (quoted)
            yield return sequence.Substring(splitAt + 1, index - splitAt - 2).Replace("\"\"", "\"");
          else
            yield return sequence.Substring(splitAt, index - splitAt);

          quoted = false;
          splitAt = index + 1;
        }
      }
    }

    public static string ToJoinedNullable(params string[] csv)
    {
      if (csv == null)
        return null;
      else
        return ToJoined(csv);
    }

    public static string ToJoinedNullable(IEnumerable<string> csv)
    {
      if (csv == null)
        return null;
      else
        return ToJoined(csv);
    }

    public static string ToJoined(params string[] csv)
      => ToJoined((IEnumerable<string>)csv ?? throw new ArgumentNullException(nameof(csv)));

    public static string ToJoined(IEnumerable<string> csv)
    {
      return string.Join(
        ",",
        (csv ?? throw new ArgumentNullException(nameof(csv)))
          .Select(s => s != null && s.Contains("\"") ? string.Concat("\"", s.Replace("\"", "\"\""), "\"") : s)
      );
    }
  }
}
