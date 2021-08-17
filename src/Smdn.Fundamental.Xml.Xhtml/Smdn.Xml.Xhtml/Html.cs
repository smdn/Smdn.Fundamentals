// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;
using System.Text.RegularExpressions;

using Smdn.Text;

namespace Smdn.Formats {
  public static class Html {
    public static string ToHtmlEscapedString(string str)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));

      return ToXhtmlEscapedString(str, false);
    }

    public static string ToXhtmlEscapedString(string str)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));

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

    public static string FromHtmlEscapedStringNullable(string str)
    {
      if (str == null)
        return null;
      else
        return FromXhtmlEscapedString(str, false);
    }

    public static string FromXhtmlEscapedString(string str)
    {
      return FromXhtmlEscapedString(str, true);
    }

    public static string FromXhtmlEscapedStringNullable(string str)
    {
      if (str == null)
        return null;
      else
        return FromXhtmlEscapedString(str, true);
    }

    private static string FromXhtmlEscapedString(string str, bool xhtml)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));

      var sb = new StringBuilder(str);

      sb.Replace("&lt;", "<");
      sb.Replace("&gt;", ">");
      sb.Replace("&quot;", "\"");

      if (xhtml)
        sb.Replace("&apos;", "'");

      sb.Replace("&amp;", "&");

      return sb.ToString();
    }

    public static string FromNumericCharacterReferenceNullable(string str)
    {
      if (str == null)
        return null;
      else
        return FromNumericCharacterReference(str);
    }

    public static string FromNumericCharacterReference(string str)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));

      return Regex.Replace(str, @"&#(?<hex>x?)(?<number>[0-9a-fA-F]+);", delegate(Match m) {
        if (m.Groups["hex"].Length == 0)
          return ((char)Convert.ToUInt16(m.Groups["number"].Value, 10)).ToString();
        else
          return ((char)Convert.ToUInt16(m.Groups["number"].Value, 16)).ToString();
      });
    }
  }
}
