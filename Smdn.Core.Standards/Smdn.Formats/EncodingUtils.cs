// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010 smdn
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
using System.Text;

namespace Smdn.Formats {
  public static class EncodingUtils {
    public static Encoding GetEncoding(string name)
    {
      if (name == null)
        throw new ArgumentNullException("name");

      // remove leading and trailing whitespaces (\x20, \n, \t, etc.)
      name = name.Trim();

      string legalName;

      if (encodingCollationTable.TryGetValue(name.RemoveChars('-', '_', ' '), out legalName))
        name = legalName;

      try {
        return Encoding.GetEncoding(name);
      }
      catch (ArgumentException) {
        // illegal or unsupported
        return null;
      }
    }

    public static Encoding GetEncodingThrowException(string name)
    {
      var encoding = GetEncoding(name);

      if (encoding == null)
        throw new NotSupportedException(string.Format("charset '{0}' is not supported by runtime", name));
      else
        return encoding;
    }

    private static Dictionary<string, string> encodingCollationTable
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
