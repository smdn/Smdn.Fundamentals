// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Smdn.Xml.Xhtml;

public static class HtmlConvert {
  public static string EscapeHtml(ReadOnlySpan<char> s)
    => EscapeXhtml(s, asXhtml: false);

  public static string EscapeXhtml(ReadOnlySpan<char> s)
    => EscapeXhtml(s, asXhtml: true);

  private static string EscapeXhtml(ReadOnlySpan<char> s, bool asXhtml)
  {
    if (s.IsEmpty)
      return string.Empty;

    var sb = new StringBuilder(s.Length);
    var len = s.Length;

    for (var i = 0; i < len; i++) {
      switch (s[i]) {
        case '&': sb.Append("&amp;"); break;
        case '<': sb.Append("&lt;"); break;
        case '>': sb.Append("&gt;"); break;
        case '"': sb.Append("&quot;"); break;
        case '\'':
          if (asXhtml)
            sb.Append("&apos;");
          else
            sb.Append('\'');
          break;
        default: sb.Append(s[i]); break;
      }
    }

    return sb.ToString();
  }

  public static string UnescapeHtml(ReadOnlySpan<char> s)
    => UnescapeXhtml(s, asXhtml: false);

  public static string UnescapeXhtml(ReadOnlySpan<char> s)
    => UnescapeXhtml(s, asXhtml: true);

  private static string UnescapeXhtml(ReadOnlySpan<char> s, bool asXhtml)
  {
    if (s.IsEmpty)
      return string.Empty;

    var sb = new StringBuilder(s.Length);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER // SYSTEM_TEXT_STRINGBUILDER_APPEND_READONLYSPAN_OF_CHAR
    sb.Append(s);
#else
    unsafe {
      fixed (char* _s = s) {
        sb.Append(new string(_s, 0, s.Length));
      }
    }
#endif

    sb.Replace("&lt;", "<");
    sb.Replace("&gt;", ">");
    sb.Replace("&quot;", "\"");

    if (asXhtml)
      sb.Replace("&apos;", "'");

    sb.Replace("&amp;", "&");

    return sb.ToString();
  }

  private static readonly Regex regexNumericReference = new(@"&#(?<hex>x?)(?<number>[0-9a-fA-F]{1,});", RegexOptions.Singleline |  RegexOptions.CultureInvariant | RegexOptions.Compiled);

  public static string DecodeNumericCharacterReference(string s)
  {
    if (s == null)
      throw new ArgumentNullException(nameof(s));

    return regexNumericReference.Replace(
      s,
      m => ((char)Convert.ToUInt16(m.Groups["number"].Value, m.Groups["hex"].Length == 0 ? 10 : 16)).ToString()
    );
  }
}
