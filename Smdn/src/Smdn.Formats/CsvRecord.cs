// 
// Copyright (c) 2009 smdn <smdn@smdn.jp>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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
