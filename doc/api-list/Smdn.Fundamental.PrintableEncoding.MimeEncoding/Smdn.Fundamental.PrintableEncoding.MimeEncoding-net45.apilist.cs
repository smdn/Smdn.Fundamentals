// Smdn.Fundamental.PrintableEncoding.MimeEncoding.dll (Smdn.Fundamental.PrintableEncoding.MimeEncoding-3.1.0)
//   Name: Smdn.Fundamental.PrintableEncoding.MimeEncoding
//   AssemblyVersion: 3.1.0.0
//   InformationalVersion: 3.1.0+0f59827eced400bac9539e303fb615aaa372eb7f
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.CryptoTransform, Version=3.0.2.0, Culture=neutral
//     Smdn.Fundamental.Encoding, Version=3.0.2.0, Culture=neutral
//     Smdn.Fundamental.Exception, Version=3.0.0.0, Culture=neutral
//     Smdn.Fundamental.PrintableEncoding.Base64, Version=3.0.3.0, Culture=neutral
//     Smdn.Fundamental.PrintableEncoding.QuotedPrintable, Version=3.0.2.0, Culture=neutral
//     Smdn.Fundamental.PrintableEncoding.UUEncoding, Version=3.0.2.0, Culture=neutral
//     Smdn.Fundamental.Stream, Version=3.0.3.0, Culture=neutral
//     System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
//     System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.ValueTuple, Version=4.0.3.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
#nullable enable annotations

using System;
using System.IO;
using System.Text;
using Smdn.Formats.Mime;
using Smdn.Text.Encodings;

namespace Smdn.Formats.Mime {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public delegate string MimeEncodedWordConverter(Encoding charset, string encodingMethod, string encodedText);

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum ContentTransferEncodingMethod : int {
    Base64 = 3,
    Binary = 2,
    EightBit = 1,
    GZip64 = 6,
    QuotedPrintable = 4,
    SevenBit = 0,
    UUEncode = 5,
    Unknown = 7,
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum MimeEncodingMethod : int {
    BEncoding = 1,
    Base64 = 1,
    None = 0,
    QEncoding = 2,
    QuotedPrintable = 2,
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ContentTransferEncoding {
    public const string HeaderName = "Content-Transfer-Encoding";

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding) {}
    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding? charset) {}
    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding? charset, bool leaveStreamOpen) {}
    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, bool leaveStreamOpen) {}
    public static BinaryReader CreateBinaryReader(Stream stream, string encoding) {}
    public static BinaryReader CreateBinaryReader(Stream stream, string encoding, bool leaveStreamOpen) {}
    public static Stream CreateDecodingStream(Stream stream, ContentTransferEncodingMethod encoding) {}
    public static Stream CreateDecodingStream(Stream stream, ContentTransferEncodingMethod encoding, bool leaveStreamOpen) {}
    public static Stream CreateDecodingStream(Stream stream, string encoding) {}
    public static Stream CreateDecodingStream(Stream stream, string encoding, bool leaveStreamOpen) {}
    public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset) {}
    public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset, bool leaveStreamOpen) {}
    public static StreamReader CreateTextReader(Stream stream, string encoding, string charset) {}
    public static StreamReader CreateTextReader(Stream stream, string encoding, string charset, bool leaveStreamOpen) {}
    [Obsolete("Use TryParse() instead.")]
    public static ContentTransferEncodingMethod GetEncodingMethod(string encoding) {}
    [Obsolete("Use Parse() instead.")]
    public static ContentTransferEncodingMethod GetEncodingMethodThrowException(string encoding) {}
    public static string GetEncodingName(ContentTransferEncodingMethod method) {}
    public static ContentTransferEncodingMethod Parse(string str) {}
    public static bool TryFormat(ContentTransferEncodingMethod encoding, Span<char> destination, out int charsWritten) {}
    public static bool TryParse(string str, out ContentTransferEncodingMethod encoding) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class MimeEncoding {
    public static string Decode(string str) {}
    public static string Decode(string str, EncodingSelectionCallback? selectFallbackEncoding) {}
    public static string Decode(string str, EncodingSelectionCallback? selectFallbackEncoding, MimeEncodedWordConverter? decodeMalformedOrUnsupported) {}
    public static string Decode(string str, EncodingSelectionCallback? selectFallbackEncoding, MimeEncodedWordConverter? decodeMalformedOrUnsupported, out MimeEncodingMethod encoding, out Encoding? charset) {}
    public static string Decode(string str, EncodingSelectionCallback? selectFallbackEncoding, out MimeEncodingMethod encoding, out Encoding? charset) {}
    public static string Decode(string str, out MimeEncodingMethod encoding, out Encoding? charset) {}
    public static string? DecodeNullable(string? str) {}
    public static string? DecodeNullable(string? str, EncodingSelectionCallback? selectFallbackEncoding) {}
    public static string? DecodeNullable(string? str, EncodingSelectionCallback? selectFallbackEncoding, MimeEncodedWordConverter? decodeMalformedOrUnsupported) {}
    public static string? DecodeNullable(string? str, EncodingSelectionCallback? selectFallbackEncoding, MimeEncodedWordConverter? decodeMalformedOrUnsupported, out MimeEncodingMethod encoding, out Encoding? charset) {}
    public static string? DecodeNullable(string? str, EncodingSelectionCallback? selectFallbackEncoding, out MimeEncodingMethod encoding, out Encoding? charset) {}
    public static string? DecodeNullable(string? str, out MimeEncodingMethod encoding, out Encoding? charset) {}
    public static string Encode(string str, MimeEncodingMethod encoding) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset, string foldingString) {}
    public static string Encode(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset, string foldingString) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.1.7.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
