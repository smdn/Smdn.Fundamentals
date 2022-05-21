// Smdn.Fundamental.PrintableEncoding.Hexadecimal.dll (Smdn.Fundamental.PrintableEncoding.Hexadecimal-3.0.1)
//   Name: Smdn.Fundamental.PrintableEncoding.Hexadecimal
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+c73d3360cd4c334c1a6c2d66a2f54b0097a6b969
//   TargetFramework: .NETStandard,Version=v1.0
//   Configuration: Release

using System;
using System.Runtime.CompilerServices;

namespace Smdn.Formats {
  public static class Hexadecimal {
    [return: Nullable(1)] public static string ToLowerCaseString(ArraySegment<byte> dataSequence) {}
    [return: Nullable(1)] public static string ToUpperCaseString(ArraySegment<byte> dataSequence) {}
    public static bool TryDecode(ArraySegment<byte> data, out byte decodedData) {}
    public static bool TryDecode(ArraySegment<char> data, out byte decodedData) {}
    public static bool TryDecode(ArraySegment<char> dataSequence, ArraySegment<byte> destination, out int decodedLength) {}
    public static bool TryDecodeLowerCase(ArraySegment<byte> lowerCaseData, out byte decodedData) {}
    public static bool TryDecodeLowerCase(ArraySegment<char> lowerCaseData, out byte decodedData) {}
    public static bool TryDecodeLowerCase(ArraySegment<char> lowerCaseDataSequence, ArraySegment<byte> destination, out int decodedLength) {}
    public static bool TryDecodeLowerCaseValue(byte lowerCaseData, out byte decodedValue) {}
    public static bool TryDecodeLowerCaseValue(char lowerCaseData, out byte decodedValue) {}
    public static bool TryDecodeUpperCase(ArraySegment<byte> upperCaseData, out byte decodedData) {}
    public static bool TryDecodeUpperCase(ArraySegment<char> upperCaseData, out byte decodedData) {}
    public static bool TryDecodeUpperCase(ArraySegment<char> upperCaseDataSequence, ArraySegment<byte> destination, out int decodedLength) {}
    public static bool TryDecodeUpperCaseValue(byte upperCaseData, out byte decodedValue) {}
    public static bool TryDecodeUpperCaseValue(char upperCaseData, out byte decodedValue) {}
    public static bool TryDecodeValue(byte data, out byte decodedValue) {}
    public static bool TryDecodeValue(char data, out byte decodedValue) {}
    public static bool TryEncodeLowerCase(ArraySegment<byte> dataSequence, ArraySegment<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeLowerCase(ArraySegment<byte> dataSequence, ArraySegment<char> destination, out int charsEncoded) {}
    [NullableContext(1)]
    public static bool TryEncodeLowerCase(byte data, byte[] destination, int index, out int bytesEncoded) {}
    [NullableContext(1)]
    public static bool TryEncodeLowerCase(byte data, char[] destination, int index, out int charsEncoded) {}
    public static bool TryEncodeUpperCase(ArraySegment<byte> dataSequence, ArraySegment<byte> destination, out int bytesEncoded) {}
    public static bool TryEncodeUpperCase(ArraySegment<byte> dataSequence, ArraySegment<char> destination, out int charsEncoded) {}
    [NullableContext(1)]
    public static bool TryEncodeUpperCase(byte data, byte[] destination, int index, out int bytesEncoded) {}
    [NullableContext(1)]
    public static bool TryEncodeUpperCase(byte data, char[] destination, int index, out int charsEncoded) {}
  }
}

