// Smdn.Fundamental.MimeType.dll (Smdn.Fundamental.MimeType-3.1.0)
//   Name: Smdn.Fundamental.MimeType
//   AssemblyVersion: 3.1.0.0
//   InformationalVersion: 3.1.0+73609d14a9c7ae47c72f6d32f87ad6ebc0a2d166
//   TargetFramework: .NETStandard,Version=v1.3
//   Configuration: Release
//   Referenced assemblies:
//     Microsoft.Win32.Registry, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     System.Buffers, Version=4.0.2.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Diagnostics.Debug, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.IO.FileSystem, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Runtime, Version=4.0.20.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.Extensions, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.InteropServices, Version=4.0.20.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.InteropServices.RuntimeInformation, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.ValueTuple, Version=4.0.3.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
#nullable enable annotations

using System;
using System.Collections.Generic;
using Smdn;

namespace Smdn {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class MimeType :
    IEquatable<MimeType>,
    IEquatable<string>,
    IFormattable
  {
    public static readonly MimeType ApplicationOctetStream; // = "application/octet-stream"
    public static readonly MimeType ApplicationXWwwFormUrlEncoded; // = "application/x-www-form-urlencoded"
    public static readonly MimeType MessageExternalBody; // = "message/external-body"
    public static readonly MimeType MessagePartial; // = "message/partial"
    public static readonly MimeType MessageRfc822; // = "message/rfc822"
    public static readonly MimeType MultipartAlternative; // = "multipart/alternative"
    public static readonly MimeType MultipartDigest; // = "multipart/digest"
    public static readonly MimeType MultipartFormData; // = "multipart/form-data"
    public static readonly MimeType MultipartMixed; // = "multipart/mixed"
    public static readonly MimeType MultipartParallel; // = "multipart/parallel"
    public static readonly MimeType TextHtml; // = "text/html"
    public static readonly MimeType TextPlain; // = "text/plain"
    public static readonly MimeType TextRtf; // = "text/rtf"

    public static MimeType CreateApplicationType(string subtype) {}
    public static MimeType CreateAudioType(string subtype) {}
    public static MimeType CreateFontType(string subtype) {}
    public static MimeType CreateImageType(string subtype) {}
    public static MimeType CreateModelType(string subtype) {}
    public static MimeType CreateMultipartType(string subtype) {}
    public static MimeType CreateTextType(string subtype) {}
    public static MimeType CreateVideoType(string subtype) {}
    public static IEnumerable<string> FindExtensionsByMimeType(MimeType mimeType) {}
    public static IEnumerable<string> FindExtensionsByMimeType(MimeType mimeType, string mimeTypesFile) {}
    public static IEnumerable<string> FindExtensionsByMimeType(string mimeType) {}
    public static IEnumerable<string> FindExtensionsByMimeType(string mimeType, string mimeTypesFile) {}
    public static MimeType? FindMimeTypeByExtension(string extensionOrPath) {}
    public static MimeType? FindMimeTypeByExtension(string extensionOrPath, string mimeTypesFile) {}
    [Obsolete("The return type of this method will be changed to MimeType in the future release. Use Smdn.Formats.Mime.MimeTypeStringExtensions.Split() instead.")]
    public static (string type, string subType) Parse(string s) {}
    public static MimeType Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) {}
    public static MimeType Parse(string s, IFormatProvider? provider = null) {}
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out MimeType result) {}
    public static bool TryParse(string? s, IFormatProvider? provider, out MimeType result) {}
    [Obsolete("The method will be deprecated in the future release. Use Smdn.Formats.Mime.MimeTypeStringExtensions.TrySplit() instead.")]
    public static bool TryParse(string? s, out (string type, string subType) result) {}
    public static bool TryParse(string? s, out MimeType? result) {}
    public static explicit operator string?(MimeType? mimeType) {}

    public MimeType((string type, string subType) mimeType) {}
    public MimeType(string mimeType) {}
    public MimeType(string type, string subType) {}

    public bool IsApplication { get; }
    public bool IsApplicationOctetStream { get; }
    public bool IsApplicationXWwwFormUrlEncoded { get; }
    public bool IsAudio { get; }
    public bool IsFont { get; }
    public bool IsImage { get; }
    public bool IsMessage { get; }
    public bool IsMessageExternalBody { get; }
    public bool IsMessagePartial { get; }
    public bool IsMessageRfc822 { get; }
    public bool IsModel { get; }
    public bool IsMultipart { get; }
    public bool IsMultipartAlternative { get; }
    public bool IsMultipartDigest { get; }
    public bool IsMultipartFormData { get; }
    public bool IsMultipartMixed { get; }
    public bool IsMultipartParallel { get; }
    public bool IsText { get; }
    public bool IsTextHtml { get; }
    public bool IsTextPlain { get; }
    public bool IsVideo { get; }
    public string SubType { get; }
    public string Type { get; }

    public void Deconstruct(out string type, out string subType) {}
    [Obsolete("Use `Equals(MimeType, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
    public bool Equals(MimeType? other) {}
    public bool Equals(MimeType? other, StringComparison comparisonType) {}
    public bool Equals(ReadOnlySpan<char> other, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase) {}
    [Obsolete("Use `Equals(string, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
    public bool Equals(string? other) {}
    public bool Equals(string? other, StringComparison comparisonType) {}
    public override bool Equals(object? obj) {}
    [Obsolete("Use `Equals(MimeType, StringComparison)` instead.")]
    public bool EqualsIgnoreCase(MimeType? other) {}
    [Obsolete("Use `Equals(string, StringComparison)` instead.")]
    public bool EqualsIgnoreCase(string? other) {}
    public override int GetHashCode() {}
    [Obsolete("Use `SubTypeEquals(MimeType, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
    public bool SubTypeEquals(MimeType? mimeType) {}
    public bool SubTypeEquals(MimeType? mimeType, StringComparison comparisonType) {}
    public bool SubTypeEquals(ReadOnlySpan<char> subType, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase) {}
    [Obsolete("Use `SubTypeEquals(string, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
    public bool SubTypeEquals(string? subType) {}
    public bool SubTypeEquals(string? subType, StringComparison comparisonType) {}
    [Obsolete("Use `SubTypeEquals(MimeType, StringComparison)` instead.")]
    public bool SubTypeEqualsIgnoreCase(MimeType? mimeType) {}
    [Obsolete("Use `SubTypeEquals(string, StringComparison)` instead.")]
    public bool SubTypeEqualsIgnoreCase(string? subType) {}
    public override string ToString() {}
    public string ToString(string? format, IFormatProvider? formatProvider) {}
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {}
    [Obsolete("Use `TypeEquals(MimeType, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
    public bool TypeEquals(MimeType? mimeType) {}
    public bool TypeEquals(MimeType? mimeType, StringComparison comparisonType) {}
    public bool TypeEquals(ReadOnlySpan<char> type, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase) {}
    [Obsolete("Use `TypeEquals(string, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
    public bool TypeEquals(string? type) {}
    public bool TypeEquals(string? type, StringComparison comparisonType) {}
    [Obsolete("Use `TypeEquals(MimeType, StringComparison)` instead.")]
    public bool TypeEqualsIgnoreCase(MimeType? mimeType) {}
    [Obsolete("Use `TypeEquals(string, StringComparison)` instead.")]
    public bool TypeEqualsIgnoreCase(string? type) {}
  }
}

namespace Smdn.Formats.Mime {
  public static class MimeTypeStringExtensions {
    public static (string Type, string SubType) Split(string? mimeType) {}
    public static bool TrySplit(string? mimeType, out (string Type, string SubType) result) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.1.7.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
