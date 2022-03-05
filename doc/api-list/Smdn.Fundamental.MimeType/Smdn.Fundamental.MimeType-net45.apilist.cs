// Smdn.Fundamental.MimeType.dll (Smdn.Fundamental.MimeType-3.0.2)
//   Name: Smdn.Fundamental.MimeType
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+53f78e8e599784381721a51f463d3202aeade3d8
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release

using System;
using System.Collections.Generic;
using Smdn;

namespace Smdn {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class MimeType :
    IEquatable<MimeType>,
    IEquatable<string>
  {
    public static readonly MimeType ApplicationOctetStream; // = "application/octet-stream"
    public static readonly MimeType MessageExternalBody; // = "message/external-body"
    public static readonly MimeType MessagePartial; // = "message/partial"
    public static readonly MimeType MessageRfc822; // = "message/rfc822"
    public static readonly MimeType MultipartAlternative; // = "multipart/alternative"
    public static readonly MimeType MultipartMixed; // = "multipart/mixed"
    public static readonly MimeType TextPlain; // = "text/plain"

    public MimeType((string type, string subType) mimeType) {}
    public MimeType(string mimeType) {}
    public MimeType(string type, string subType) {}

    public string SubType { get; }
    public string Type { get; }

    public static MimeType CreateApplicationType(string subtype) {}
    public static MimeType CreateAudioType(string subtype) {}
    public static MimeType CreateImageType(string subtype) {}
    public static MimeType CreateMultipartType(string subtype) {}
    public static MimeType CreateTextType(string subtype) {}
    public static MimeType CreateVideoType(string subtype) {}
    public void Deconstruct(out string type, out string subType) {}
    public bool Equals(MimeType other) {}
    public bool Equals(string other) {}
    public override bool Equals(object obj) {}
    public bool EqualsIgnoreCase(MimeType other) {}
    public bool EqualsIgnoreCase(string other) {}
    public static IEnumerable<string> FindExtensionsByMimeType(MimeType mimeType) {}
    public static IEnumerable<string> FindExtensionsByMimeType(MimeType mimeType, string mimeTypesFile) {}
    public static IEnumerable<string> FindExtensionsByMimeType(string mimeType) {}
    public static IEnumerable<string> FindExtensionsByMimeType(string mimeType, string mimeTypesFile) {}
    public static MimeType FindMimeTypeByExtension(string extensionOrPath) {}
    public static MimeType FindMimeTypeByExtension(string extensionOrPath, string mimeTypesFile) {}
    public override int GetHashCode() {}
    public static (string type, string subType) Parse(string s) {}
    public bool SubTypeEquals(MimeType mimeType) {}
    public bool SubTypeEquals(string subType) {}
    public bool SubTypeEqualsIgnoreCase(MimeType mimeType) {}
    public bool SubTypeEqualsIgnoreCase(string subType) {}
    public override string ToString() {}
    public static bool TryParse(string s, out (string type, string subType) result) {}
    public static bool TryParse(string s, out MimeType result) {}
    public bool TypeEquals(MimeType mimeType) {}
    public bool TypeEquals(string type) {}
    public bool TypeEqualsIgnoreCase(MimeType mimeType) {}
    public bool TypeEqualsIgnoreCase(string type) {}
    public static explicit operator string(MimeType mimeType) {}
  }
}

