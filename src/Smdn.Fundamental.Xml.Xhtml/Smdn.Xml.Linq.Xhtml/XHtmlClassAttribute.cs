// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Smdn.Xml.Linq.Xhtml;

#pragma warning disable CA1711
public class XHtmlClassAttribute : XAttribute {
#pragma warning restore CA1711
  private static readonly char[] ClassListSeparator = { ' ' };

  internal static string[] SplitClassList(string classList)
    => classList?.Split(ClassListSeparator, StringSplitOptions.RemoveEmptyEntries);

  public static string JoinClassList(IEnumerable<string> classList)
  {
    if (classList == null)
      throw new ArgumentNullException(nameof(classList));

    return string.Join(" ", classList);
  }

  public XHtmlClassAttribute(string @class)
    : base(XHtmlAttributeNames.Class, @class)
  {
  }

  public XHtmlClassAttribute(params string[] classList)
    : base(XHtmlAttributeNames.Class, JoinClassList(classList))
  {
  }

  public XHtmlClassAttribute(IEnumerable<string> classList)
    : base(XHtmlAttributeNames.Class, JoinClassList(classList))
  {
  }
}
