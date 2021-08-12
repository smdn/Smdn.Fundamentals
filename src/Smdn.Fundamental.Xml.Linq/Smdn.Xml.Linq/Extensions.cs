// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Smdn.Xml.Linq {
  public static class Extensions {
    public static string GetAttributeValue(this XElement element, XName attributeName)
    {
      return element?.Attribute(attributeName)?.Value;
    }

    public static TValue GetAttributeValue<TValue>(
      this XElement element,
      XName attributeName,
#if SYSTEM_CONVERTER
      Converter<string, TValue> converter
#else
      Func<string, TValue> converter
#endif
    )
    {
      if (converter == null)
        throw new ArgumentNullException(nameof(converter));

      return converter(element?.Attribute(attributeName)?.Value);
    }

    public static bool HasAttribute(this XElement element, XName name)
      => element?.Attribute(name) != null;

    public static bool HasAttribute(this XElement element, XName name, out string value)
    {
      var attr = element?.Attribute(name);

      if (attr == null) {
        value = default;
        return false;
      }
      else {
        value = attr.Value;
        return true;
      }
    }

    public static bool HasAttributeWithValue(this XElement element, XName attributeName, string @value)
      => HasAttribute(element, attributeName, out var attributeValue) &&
         string.Equals(attributeValue, @value, StringComparison.Ordinal);

    public static bool HasAttributeWithValue(this XElement element, XName attributeName, Predicate<string> predicate)
      => HasAttribute(element, attributeName, out var attributeValue) &&
         (predicate?.Invoke(attributeValue) ?? false);

    public static string TextContent(this XContainer container)
    {
      return string.Concat(container.DescendantNodes()
                                    .Where(n => n.NodeType == XmlNodeType.Text)
                                    .Select(n => (n as XText).Value));
    }
  }
}
