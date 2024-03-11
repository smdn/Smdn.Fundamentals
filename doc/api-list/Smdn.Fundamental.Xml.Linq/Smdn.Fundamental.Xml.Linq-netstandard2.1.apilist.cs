// Smdn.Fundamental.Xml.Linq.dll (Smdn.Fundamental.Xml.Linq-3.1.0)
//   Name: Smdn.Fundamental.Xml.Linq
//   AssemblyVersion: 3.1.0.0
//   InformationalVersion: 3.1.0+e34f21bacc4532dd418d4d17faf2e22cf6f32d65
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release
//   Referenced assemblies:
//     netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
#nullable enable annotations

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Smdn.Xml.Linq {
  public static class Extensions {
    public static TValue GetAttributeValue<TValue>(this XElement element, XName attributeName, Converter<string?, TValue> converter) {}
    public static string? GetAttributeValue(this XElement element, XName attributeName) {}
    public static bool HasAttribute(this XElement element, XName name) {}
    public static bool HasAttribute(this XElement element, XName name, [NotNullWhen(true)] out string? @value) {}
    public static bool HasAttributeWithValue(this XElement element, XName attributeName, Predicate<string> predicate) {}
    public static bool HasAttributeWithValue(this XElement element, XName attributeName, string @value) {}
    public static string TextContent(this XContainer container) {}
    public static bool TryGetAttribute(this XElement element, XName attributeName, [NotNullWhen(true)] out XAttribute? attribute) {}
  }

  public class XEntityReference : XText {
    public XEntityReference(string name) {}

    public override void WriteTo(XmlWriter writer) {}
    public override Task WriteToAsync(XmlWriter writer, CancellationToken cancellationToken) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.4.0.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.3.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
