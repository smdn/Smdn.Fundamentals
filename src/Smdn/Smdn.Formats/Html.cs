// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using Smdn.Xml.Xhtml;

namespace Smdn.Formats {
  public static class Html {
    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.EscapeHtml instead")]
    public static string ToHtmlEscapedString(string str)
      => HtmlConvert.EscapeHtml((str ?? throw new ArgumentNullException(nameof(str))).AsSpan());

    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.EscapeXhtml instead")]
    public static string ToXhtmlEscapedString(string str)
      => HtmlConvert.EscapeXhtml((str ?? throw new ArgumentNullException(nameof(str))).AsSpan());

    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.EscapeHtml instead")]
    public static string ToHtmlEscapedStringNullable(string str)
      => str is null ? null : HtmlConvert.EscapeHtml(str.AsSpan());

    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.EscapeXhtml instead")]
    public static string ToXhtmlEscapedStringNullable(string str)
      => str is null ? null : HtmlConvert.EscapeXhtml(str.AsSpan());

    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.UnescapeHtml instead")]
    public static string FromHtmlEscapedString(string str)
      => HtmlConvert.UnescapeHtml((str ?? throw new ArgumentNullException(nameof(str))).AsSpan());

    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.UnescapeHtml instead")]
    public static string FromHtmlEscapedStringNullable(string str)
      => str is null ? null : HtmlConvert.UnescapeHtml(str.AsSpan());

    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.UnescapeXhtml instead")]
    public static string FromXhtmlEscapedString(string str)
      => HtmlConvert.UnescapeXhtml((str ?? throw new ArgumentNullException(nameof(str))).AsSpan());

    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.UnescapeXhtml instead")]
    public static string FromXhtmlEscapedStringNullable(string str)
      => str is null ? null : HtmlConvert.UnescapeXhtml(str.AsSpan());

    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.DecodeNumericCharacterReference instead")]
    public static string FromNumericCharacterReferenceNullable(string str)
      => str is null ? null : HtmlConvert.DecodeNumericCharacterReference(str);

    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.DecodeNumericCharacterReference instead")]
    public static string FromNumericCharacterReference(string str)
      => HtmlConvert.DecodeNumericCharacterReference(str ?? throw new ArgumentNullException(nameof(str)));
  }
}
