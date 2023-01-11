// Smdn.Fundamental.PrintableEncoding.Hexadecimal.dll (Smdn.Fundamental.PrintableEncoding.Hexadecimal-3.0.1)
//   Name: Smdn.Fundamental.PrintableEncoding.Hexadecimal
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+c73d3360cd4c334c1a6c2d66a2f54b0097a6b969
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
//   Referenced assemblies:
//     System.Buffers, Version=4.0.3.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
#nullable enable annotations

using System;

namespace Smdn.Formats {
  public static class Hexadecimal {
    public static ReadOnlySpan<char> LowerCaseHexChars { get; }
    public static ReadOnlySpan<byte> LowerCaseHexOctets { get; }
    public static ReadOnlySpan<char> UpperCaseHexChars { get; }
    public static ReadOnlySpan<byte> UpperCaseHexOctets { get; }

    public static string ToLowerCaseString(ArraySegment<byte> dataSequence) {}
    public static string ToLowerCaseString(ReadOnlySpan<byte> dataSequence) {}
    public static string ToUpperCaseString(ArraySegment<byte> dataSequence) {}
    public static string ToUpperCaseString(ReadOnlySpan<byte> dataSequence) {}
    public static bool TryDecode(ArraySegment<byte> data, out byte decodedData) {}
    public static bool TryDecode(ArraySegment<char> data, out byte decodedData) {}
    public static bool TryDecode(ArraySegment<char> dataSequence, ArraySegment<byte> destination, out int decodedLength) {}
    public static bool TryDecode(ReadOnlySpan<byte> data, out byte decodedData) {}
    public static bool TryDecode(ReadOnlySpan<char> data, out byte decodedData) {}
    public static bool TryDecode(ReadOnlySpan<char> dataSequence, Span<byte> destination, out int decodedLength) {}
    public static bool TryDecodeLowerCase(ArraySegment<byte> lowerCaseData, out byte decodedData) {}
    public static bool TryDecodeLowerCase(ArraySegment<char> lowerCaseData, out byte decodedData) {}
    public static bool TryDecodeLowerCase(ArraySegment<char> lowerCaseDataSequence, ArraySegment<byte> destination, out int decodedLength) {}
    public static bool TryDecodeLowerCase(ReadOnlySpan<byte> lowerCaseData, out byte decodedData) {}
    public static bool TryDecodeLowerCase(ReadOnlySpan<char> lowerCaseData, out byte decodedData) {}
    public static bool TryDecodeLowerCase(ReadOnlySpan<char> lowerCaseDataSequence, Span<byte> destination, out int decodedLength) {}
    public static bool TryDecodeLowerCaseValue(byte lowerCaseData, out byte decodedValue) {}
    public static bool TryDecodeLowerCaseValue(char lowerCaseData, out byte decodedValue) {}
    public static bool TryDecodeUpperCase(ArraySegment<byte> upperCaseData, out byte decodedData) {}
    public static bool TryDecodeUpperCase(ArraySegment<char> upperCaseData, out byte decodedData) {}
    public static bool TryDecodeUpperCase(ArraySegment<char> upperCaseDataSequence, ArraySegment<byte> destination, out int decodedLength) {}
    public static bool TryDecodeUpperCase(ReadOnlySpan<byte> upperCaseData, out byte decodedData) {}
    public static bool TryDecodeUpperCase(ReadOnlySpan<char> upperCaseData, out byte decodedData) {}
    public static bool TryDecodeUpperCase(ReadOnlySpan<char> upperCaseDataSequence, Span<byte> destination, out int decodedLength) {}
    public static bool TryDecodeUpperCaseValue(byte upperCaseData, out byte decodedValue) {}
    public static bool TryDecodeUpperCaseValue(char upperCaseData, out byte decodedValue) {}
    public static bool TryDecodeValue(byte data, out byte decodedValue) {}
    public static bool TryDecodeValue(char data, out byte decodedValue) {}
    public static bool TryEncodeLowerCase(ArraySegment<byte> dataSequence, ArraySegment<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeLowerCase(ArraySegment<byte> dataSequence, ArraySegment<char> destination, out int charsEncoded) {}
    public static bool TryEncodeLowerCase(ReadOnlySpan<byte> dataSequence, Span<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeLowerCase(ReadOnlySpan<byte> dataSequence, Span<char> destination, out int charsEncoded) {}
    public static bool TryEncodeLowerCase(byte data, Span<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeLowerCase(byte data, Span<char> destination, out int charsEncoded) {}
    public static bool TryEncodeLowerCase(byte data, byte[] destination, int index, out int bytesEncoded) {}
    public static bool TryEncodeLowerCase(byte data, char[] destination, int index, out int charsEncoded) {}
    public static bool TryEncodeUpperCase(ArraySegment<byte> dataSequence, ArraySegment<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeUpperCase(ArraySegment<byte> dataSequence, ArraySegment<char> destination, out int charsEncoded) {}
    public static bool TryEncodeUpperCase(ReadOnlySpan<byte> dataSequence, Span<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeUpperCase(ReadOnlySpan<byte> dataSequence, Span<char> destination, out int charsEncoded) {}
    public static bool TryEncodeUpperCase(byte data, Span<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeUpperCase(byte data, Span<char> destination, out int charsEncoded) {}
    public static bool TryEncodeUpperCase(byte data, byte[] destination, int index, out int bytesEncoded) {}
    public static bool TryEncodeUpperCase(byte data, char[] destination, int index, out int charsEncoded) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.1.7.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
