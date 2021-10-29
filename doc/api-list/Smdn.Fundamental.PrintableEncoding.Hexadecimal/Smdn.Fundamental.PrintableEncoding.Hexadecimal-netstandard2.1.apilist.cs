// Smdn.Fundamental.PrintableEncoding.Hexadecimal.dll (Smdn.Fundamental.PrintableEncoding.Hexadecimal-3.0.0 (netstandard2.1))
//   Name: Smdn.Fundamental.PrintableEncoding.Hexadecimal
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard2.1)
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

using System;

namespace Smdn.Formats {
  public static class Hexadecimal {
    public static ReadOnlySpan<char> LowerCaseHexChars { get; }
    public static ReadOnlySpan<byte> LowerCaseHexOctets { get; }
    public static ReadOnlySpan<char> UpperCaseHexChars { get; }
    public static ReadOnlySpan<byte> UpperCaseHexOctets { get; }

    public static string ToLowerCaseString(ReadOnlySpan<byte> dataSequence) {}
    public static string ToUpperCaseString(ReadOnlySpan<byte> dataSequence) {}
    public static bool TryDecode(ReadOnlySpan<byte> data, out byte decodedData) {}
    public static bool TryDecode(ReadOnlySpan<char> data, out byte decodedData) {}
    public static bool TryDecode(ReadOnlySpan<char> dataSequence, Span<byte> destination, out int decodedLength) {}
    public static bool TryDecodeLowerCase(ReadOnlySpan<byte> lowerCaseData, out byte decodedData) {}
    public static bool TryDecodeLowerCase(ReadOnlySpan<char> lowerCaseData, out byte decodedData) {}
    public static bool TryDecodeLowerCase(ReadOnlySpan<char> lowerCaseDataSequence, Span<byte> destination, out int decodedLength) {}
    public static bool TryDecodeLowerCaseValue(byte lowerCaseData, out byte decodedValue) {}
    public static bool TryDecodeLowerCaseValue(char lowerCaseData, out byte decodedValue) {}
    public static bool TryDecodeUpperCase(ReadOnlySpan<byte> upperCaseData, out byte decodedData) {}
    public static bool TryDecodeUpperCase(ReadOnlySpan<char> upperCaseData, out byte decodedData) {}
    public static bool TryDecodeUpperCase(ReadOnlySpan<char> upperCaseDataSequence, Span<byte> destination, out int decodedLength) {}
    public static bool TryDecodeUpperCaseValue(byte upperCaseData, out byte decodedValue) {}
    public static bool TryDecodeUpperCaseValue(char upperCaseData, out byte decodedValue) {}
    public static bool TryDecodeValue(byte data, out byte decodedValue) {}
    public static bool TryDecodeValue(char data, out byte decodedValue) {}
    public static bool TryEncodeLowerCase(ReadOnlySpan<byte> dataSequence, Span<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeLowerCase(ReadOnlySpan<byte> dataSequence, Span<char> destination, out int charsEncoded) {}
    public static bool TryEncodeLowerCase(byte data, Span<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeLowerCase(byte data, Span<char> destination, out int charsEncoded) {}
    public static bool TryEncodeUpperCase(ReadOnlySpan<byte> dataSequence, Span<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeUpperCase(ReadOnlySpan<byte> dataSequence, Span<char> destination, out int charsEncoded) {}
    public static bool TryEncodeUpperCase(byte data, Span<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeUpperCase(byte data, Span<char> destination, out int charsEncoded) {}
  }
}

