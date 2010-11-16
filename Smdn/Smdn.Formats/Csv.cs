// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2010 smdn
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

namespace Smdn.Formats {
  public static class Csv {
    // http://www.ietf.org/rfc/rfc4180.txt
    // Common Format and MIME Type for Comma-Separated Values (CSV) Files
    public static string[] ToSplitted(string csv)
    {
      // append dummy splitter
      csv += ",";

      var splitted = new List<string>();
      var splitAt = 0;
      var quoted = false;
      var inQuote = false;

      for (var index = 0; index < csv.Length; index++) {
        if (csv[index] == Chars.DQuote) {
          inQuote = !inQuote;
          quoted = true;
        }

        if (inQuote)
          continue;

        if (csv[index] != Chars.Comma)
          continue;

        if (quoted)
          splitted.Add(csv.Substring(splitAt + 1, index - splitAt - 2).Replace("\"\"", "\""));
        else
          splitted.Add(csv.Substring(splitAt, index - splitAt));

        quoted = false;
        splitAt = index + 1;
      }

      return splitted.ToArray();
    }

    public static string ToJoined(params string[] csv)
    {
      if (csv.Length == 0)
        return string.Empty;

      return string.Join(",", Array.ConvertAll(csv, delegate(string s) {
        if (s.Contains("\""))
          return string.Concat("\"", s.Replace("\"", "\"\""), "\"");
        else
          return s;
      }));
    }
  }
}
