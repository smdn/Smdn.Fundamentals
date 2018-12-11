// 
// Copyright (c) 2018 smdn <smdn@smdn.jp>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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

    public static TValue GetAttributeValue<TValue>(this XElement element, XName attributeName, Converter<string, TValue> converter)
    {
      if (converter == null)
        throw new ArgumentNullException(nameof(converter));

      return converter(element?.Attribute(attributeName)?.Value);
    }

    public static bool HasAttribute(this XElement element, XName name)
    {
      return element?.Attribute(name) != null;
    }

    public static bool HasAttributeWithValue(this XElement element, XName attributeName, string @value)
    {
      var attr = element?.Attribute(attributeName);

      if (attr == null)
        return false;

      return string.Equals(attr.Value, @value, StringComparison.Ordinal);
    }

    public static bool HasAttributeWithValue(this XElement element, XName attributeName, Predicate<string> predicate)
    {
      var attr = element?.Attribute(attributeName);

      if (attr == null)
        return false;

      return predicate?.Invoke(attr.Value) ?? false;
    }

    public static string TextContent(this XContainer container)
    {
      return string.Concat(container.DescendantNodes()
                                    .Where(n => n.NodeType == XmlNodeType.Text)
                                    .Select(n => (n as XText).Value));
    }
  }
}
