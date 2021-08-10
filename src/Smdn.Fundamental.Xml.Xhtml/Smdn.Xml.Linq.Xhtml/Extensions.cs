// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Smdn.Xml.Linq.Xhtml {
  public static class Extensions {
    public static XElement GetElementById(this XContainer container, string id)
    {
      return container.Descendants()
                      .Attributes(XHtmlAttributeNames.Id)
                      .FirstOrDefault(a => string.Equals(a.Value, id, StringComparison.Ordinal))
                      ?.Parent;
    }

    public static bool HasHtmlClass(this XElement element, string @class)
    {
      var attr = element.Attribute(XHtmlAttributeNames.Class);

      if (attr == null)
        return false;

      return string.Concat(" ", attr.Value, " ").Contains(string.Concat(" ", @class, " "));
    }

    public static bool HasHtmlClass(this XElement element, IEnumerable<string> classList)
    {
      var l = element.GetAttributeValue(XHtmlAttributeNames.Class, XHtmlClassAttribute.SplitClassList);

      if (l == null)
        return false;

      return (new HashSet<string>(l, StringComparer.Ordinal)).IsSupersetOf(classList);
    }
  }
}
