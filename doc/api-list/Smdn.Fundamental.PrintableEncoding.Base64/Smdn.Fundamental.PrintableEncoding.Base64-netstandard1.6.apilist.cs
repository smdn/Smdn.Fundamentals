// Smdn.Fundamental.PrintableEncoding.Base64.dll (Smdn.Fundamental.PrintableEncoding.Base64-3.0.3)
//   Name: Smdn.Fundamental.PrintableEncoding.Base64
//   AssemblyVersion: 3.0.3.0
//   InformationalVersion: 3.0.3+4acdf8653895790ff473d032b1c94abfe4aeb215
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.CryptoTransform, Version=3.0.2.0, Culture=neutral
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     Smdn.Fundamental.Stream, Version=3.0.3.0, Culture=neutral
//     System.IO, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.Extensions, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Security.Cryptography.Primitives, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Text.Encoding, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
#nullable enable annotations

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Smdn.Formats {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class Base64 {
    public static Stream CreateDecodingStream(Stream stream, bool leaveStreamOpen = false) {}
    public static Stream CreateEncodingStream(Stream stream, bool leaveStreamOpen = false) {}
    public static ICryptoTransform CreateFromBase64Transform(bool ignoreWhiteSpaces = true) {}
    public static ICryptoTransform CreateToBase64Transform() {}
    public static byte[] Decode(byte[] bytes) {}
    public static byte[] Decode(byte[] bytes, int offset, int count) {}
    public static byte[] Decode(string str) {}
    public static byte[] Encode(byte[] bytes) {}
    public static byte[] Encode(byte[] bytes, int offset, int count) {}
    public static byte[] Encode(string str) {}
    public static byte[] Encode(string str, Encoding encoding) {}
    public static string GetDecodedString(byte[] bytes) {}
    public static string GetDecodedString(byte[] bytes, int offset, int count) {}
    public static string GetDecodedString(string str) {}
    public static string GetDecodedString(string str, Encoding encoding) {}
    public static string GetEncodedString(byte[] bytes) {}
    public static string GetEncodedString(byte[] bytes, int offset, int count) {}
    public static string GetEncodedString(string str) {}
    public static string GetEncodedString(string str, Encoding encoding) {}
  }
}
