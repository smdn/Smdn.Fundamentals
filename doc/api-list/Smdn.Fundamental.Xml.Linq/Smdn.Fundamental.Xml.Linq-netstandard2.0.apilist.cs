// Smdn.Fundamental.Xml.Linq.dll (Smdn.Fundamental.Xml.Linq-3.0.1)
//   Name: Smdn.Fundamental.Xml.Linq
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+31a6e8af75b332a596dca9d2c754455314498e91
//   TargetFramework: .NETStandard,Version=v2.0
//   Configuration: Release

using System;
using System.Xml;
using System.Xml.Linq;

namespace Smdn.Xml.Linq {
  public static class Extensions {
    public static TValue GetAttributeValue<TValue>(this XElement element, XName attributeName, Converter<string, TValue> converter) {}
    public static string GetAttributeValue(this XElement element, XName attributeName) {}
    public static bool HasAttribute(this XElement element, XName name) {}
    public static bool HasAttribute(this XElement element, XName name, out string @value) {}
    public static bool HasAttributeWithValue(this XElement element, XName attributeName, Predicate<string> predicate) {}
    public static bool HasAttributeWithValue(this XElement element, XName attributeName, string @value) {}
    public static string TextContent(this XContainer container) {}
    public static bool TryGetAttribute(this XElement element, XName attributeName, out XAttribute attribute) {}
  }

  public class XEntityReference : XText {
    public XEntityReference(string name) {}

    public override void WriteTo(XmlWriter writer) {}
  }
}
