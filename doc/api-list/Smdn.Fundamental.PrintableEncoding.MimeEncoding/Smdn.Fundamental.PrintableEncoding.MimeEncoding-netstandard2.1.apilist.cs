// Smdn.Fundamental.PrintableEncoding.MimeEncoding.dll (Smdn.Fundamental.PrintableEncoding.MimeEncoding-3.0.1 (netstandard2.1))
//   Name: Smdn.Fundamental.PrintableEncoding.MimeEncoding
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1 (netstandard2.1)
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

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
    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset) {}
    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset, bool leaveStreamOpen) {}
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
    public static ContentTransferEncodingMethod GetEncodingMethod(string encoding) {}
    public static ContentTransferEncodingMethod GetEncodingMethodThrowException(string encoding) {}
    public static string GetEncodingName(ContentTransferEncodingMethod method) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class MimeEncoding {
    public static string Decode(string str) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string Decode(string str, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string DecodeNullable(string str) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string DecodeNullable(string str, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string Encode(string str, MimeEncodingMethod encoding) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset, string foldingString) {}
    public static string Encode(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset, string foldingString) {}
  }
}

