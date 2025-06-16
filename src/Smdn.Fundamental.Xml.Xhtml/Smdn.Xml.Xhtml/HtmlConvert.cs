// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:ignore apos
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Smdn.Xml.Xhtml;

public static partial class HtmlConvert {
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

#pragma warning disable SA1114
    sb.Append(
#if SYSTEM_TEXT_STRINGBUILDER_APPEND_READONLYSPAN_OF_CHAR
      s
#else
      StringShim.Construct(s)
#endif
    );
#pragma warning restore SA1114

    sb.Replace("&lt;", "<");
    sb.Replace("&gt;", ">");
    sb.Replace("&quot;", "\"");

    if (asXhtml)
      sb.Replace("&apos;", "'");

    sb.Replace("&amp;", "&");

    return sb.ToString();
  }

  private const string RegexNumericReferencePattern = @"&#(?<hex>x?)(?<number>[0-9a-fA-F]{1,});";

#if SYSTEM_TEXT_REGULAREXPRESSIONS_GENERATEDREGEXATTRIBUTE
  private static Regex RegexNumericReference => GetRegexNumericReference();

  [GeneratedRegex(
    pattern: RegexNumericReferencePattern,
    options: RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled
  )]
  private static partial Regex GetRegexNumericReference(); // TODO: use C# 13 partial property
#else
  private static Regex RegexNumericReference { get; } = new(
    pattern: RegexNumericReferencePattern,
    options: RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled
  );
#endif

  public static string DecodeNumericCharacterReference(string s)
  {
    if (s == null)
      throw new ArgumentNullException(nameof(s));

    return RegexNumericReference.Replace(
      s,
      m => ((char)Convert.ToUInt16(m.Groups["number"].Value, m.Groups["hex"].Length == 0 ? 10 : 16)).ToString()
    );
  }
}
