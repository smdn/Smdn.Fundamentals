// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Smdn.Xml.Linq.Xhtml {
  public class XHtmlClassAttribute : XAttribute {
    private static readonly char[] classListSeparator = { ' ' };

    internal static string[] SplitClassList(string classList)
    {
      return classList?.Split(classListSeparator, StringSplitOptions.RemoveEmptyEntries);
    }

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
}
