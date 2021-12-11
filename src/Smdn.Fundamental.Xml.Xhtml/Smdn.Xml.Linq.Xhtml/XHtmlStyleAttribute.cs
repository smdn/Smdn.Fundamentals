// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Smdn.Xml.Linq.Xhtml;

#pragma warning disable CA1711
public class XHtmlStyleAttribute : XAttribute {
#pragma warning restore CA1711
  protected static string ToJoined(KeyValuePair<string, string> style) => string.Concat(style.Key, ": ", style.Value, ";");

  protected static string ToJoined(IEnumerable<KeyValuePair<string, string>> styles)
  {
    if (styles == null)
      throw new ArgumentNullException(nameof(styles));

    return string.Join(" ", styles.Select(ToJoined));
  }

  public XHtmlStyleAttribute(IReadOnlyDictionary<string, string> styles)
    : base(XHtmlAttributeNames.Style, ToJoined(styles))
  {
  }

  public XHtmlStyleAttribute(IEnumerable<KeyValuePair<string, string>> styles)
    : base(XHtmlAttributeNames.Style, ToJoined(styles))
  {
  }

  public XHtmlStyleAttribute(params KeyValuePair<string, string>[] styles)
    : base(XHtmlAttributeNames.Style, ToJoined(styles))
  {
  }

  public XHtmlStyleAttribute(KeyValuePair<string, string> style)
    : base(XHtmlAttributeNames.Style, ToJoined(style))
  {
  }
}
