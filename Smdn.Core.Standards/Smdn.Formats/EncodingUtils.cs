// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
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

      try {
        string alias;

        if (encodingAliases.TryGetValue(name, out alias))
          name = alias;

        return Encoding.GetEncoding(name);
      }
      catch (ArgumentException) {
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

    private static Dictionary<string, string> encodingAliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
      /* UTF-8 */
      {"utf8",        "utf-8"},
      {"utf_8",       "utf-8"},
      /* Shift_JIS */
      {"x-sjis",      "shift_jis"},
      {"shift-jis",   "shift_jis"},
      /* EUC-JP */
      {"x-euc-jp",    "euc-jp"},
    };
  }
}
