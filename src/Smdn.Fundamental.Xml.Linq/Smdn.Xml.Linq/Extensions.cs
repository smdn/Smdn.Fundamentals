// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Smdn.Xml.Linq;

public static class Extensions {
  public static string? GetAttributeValue(this XElement element, XName attributeName)
    => element?.Attribute(attributeName)?.Value;

  public static TValue GetAttributeValue<TValue>(
    this XElement element,
    XName attributeName,
#if SYSTEM_CONVERTER
    Converter<string?, TValue> converter
#else
    Func<string?, TValue> converter
#endif
  )
  {
    if (converter == null)
      throw new ArgumentNullException(nameof(converter));

    return converter(element?.Attribute(attributeName)?.Value);
  }

  public static bool HasAttribute(this XElement element, XName name)
    => element?.Attribute(name) != null;

  public static bool HasAttribute(
    this XElement element,
    XName name,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out string? value
  )
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

#if !NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
#pragma warning disable CS8604
#endif
  public static bool HasAttributeWithValue(this XElement element, XName attributeName, Predicate<string> predicate)
    => HasAttribute(element, attributeName, out var attributeValue) &&
       (predicate?.Invoke(attributeValue) ?? false);
#pragma warning restore CS8604

  public static bool TryGetAttribute(
    this XElement element,
    XName attributeName,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out XAttribute? attribute
  )
  {
    if (element is null)
      throw new ArgumentNullException(nameof(element));

    attribute = element.Attribute(attributeName);

    return attribute is not null;
  }

  public static string TextContent(this XContainer container)
  {
    if (container is null)
      throw new ArgumentNullException(nameof(container));

    return string.Concat(
      container
        .DescendantNodes()
        .Where(static n => n.NodeType == XmlNodeType.Text)
        .Cast<XText>()
        .Select(static t => t.Value)
    );
  }
}
