// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010-2014 smdn
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
using System.Text;
using System.Text.RegularExpressions;

using Smdn.Text;

namespace Smdn.Formats {
  public static class Html {
    public static string ToHtmlEscapedString(string str)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      return ToXhtmlEscapedString(str, false);
    }

    public static string ToXhtmlEscapedString(string str)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      return ToXhtmlEscapedString(str, true);
    }

    public static string ToHtmlEscapedStringNullable(string str)
    {
      if (str == null)
        return null;
      else
        return ToXhtmlEscapedString(str, false);
    }

    public static string ToXhtmlEscapedStringNullable(string str)
    {
      if (str == null)
        return null;
      else
        return ToXhtmlEscapedString(str, true);
    }

    private static string ToXhtmlEscapedString(string str, bool xhtml)
    {
      var sb = new StringBuilder(str.Length);
      var len = str.Length;

      for (var i = 0; i < len; i++) {
        var ch = str[i];

        switch (ch) {
          case Ascii.Chars.Ampersand:   sb.Append("&amp;"); break;
          case Ascii.Chars.LessThan:    sb.Append("&lt;"); break;
          case Ascii.Chars.GreaterThan: sb.Append("&gt;"); break;
          case Ascii.Chars.DQuote:      sb.Append("&quot;"); break;
          case Ascii.Chars.Quote:
            if (xhtml) sb.Append("&apos;");
            else sb.Append(Ascii.Chars.Quote);
            break;
          default: sb.Append(ch); break;
        }
      }

      return sb.ToString();
    }

    public static string FromHtmlEscapedString(string str)
    {
      return FromXhtmlEscapedString(str, false);
    }

    public static string FromXhtmlEscapedString(string str)
    {
      return FromXhtmlEscapedString(str, true);
    }

    private static string FromXhtmlEscapedString(string str, bool xhtml)
    {
      var sb = new StringBuilder(str);

      sb.Replace("&lt;", "<");
      sb.Replace("&gt;", ">");
      sb.Replace("&quot;", "\"");

      if (xhtml)
        sb.Replace("&apos;", "'");

      sb.Replace("&amp;", "&");

      return sb.ToString();
    }

    public static string FromNumericCharacterReference(string str)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      return Regex.Replace(str, @"&#(?<hex>x?)(?<number>[0-9a-fA-F]+);", delegate(Match m) {
        if (m.Groups["hex"].Length == 0)
          return ((char)Convert.ToUInt16(m.Groups["number"].Value, 10)).ToString();
        else
          return ((char)Convert.ToUInt16(m.Groups["number"].Value, 16)).ToString();
      });
    }
  }
}
