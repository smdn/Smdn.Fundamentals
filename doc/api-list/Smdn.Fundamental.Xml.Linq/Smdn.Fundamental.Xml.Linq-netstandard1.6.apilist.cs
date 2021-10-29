// Smdn.Fundamental.Xml.Linq.dll (Smdn.Fundamental.Xml.Linq-3.0.0 (netstandard1.6))
//   Name: Smdn.Fundamental.Xml.Linq
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard1.6)
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release

using System;
using System.Xml;
using System.Xml.Linq;

namespace Smdn.Xml.Linq {
  public static class Extensions {
    public static TValue GetAttributeValue<TValue>(this XElement element, XName attributeName, Func<string, TValue> converter) {}
    public static string GetAttributeValue(this XElement element, XName attributeName) {}
    public static bool HasAttribute(this XElement element, XName name) {}
    public static bool HasAttribute(this XElement element, XName name, out string @value) {}
    public static bool HasAttributeWithValue(this XElement element, XName attributeName, Predicate<string> predicate) {}
    public static bool HasAttributeWithValue(this XElement element, XName attributeName, string @value) {}
    public static string TextContent(this XContainer container) {}
  }

  public class XEntityReference : XText {
    public XEntityReference(string name) {}

    public override void WriteTo(XmlWriter writer) {}
  }
}

