// Smdn.Fundamental.Encoding.dll (Smdn.Fundamental.Encoding-3.0.3)
//   Name: Smdn.Fundamental.Encoding
//   AssemblyVersion: 3.0.3.0
//   InformationalVersion: 3.0.3+2101d134632778e05b53d9a72f315e0bed535e40
//   TargetFramework: .NETStandard,Version=v1.3
//   Configuration: Release
//   Referenced assemblies:
//     System.Collections, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime, Version=4.0.20.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.Extensions, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Text.Encoding, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
#nullable enable annotations

using System;
using System.Collections.Generic;
using System.Text;
using Smdn.Text.Encodings;

namespace Smdn.Text.Encodings {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public delegate Encoding? EncodingSelectionCallback(string name);

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [Serializable]
  public class EncodingNotSupportedException : NotSupportedException {
    public EncodingNotSupportedException() {}
    public EncodingNotSupportedException(string encodingName) {}
    public EncodingNotSupportedException(string encodingName, Exception innerException) {}
    public EncodingNotSupportedException(string encodingName, string message) {}
    public EncodingNotSupportedException(string encodingName, string message, Exception innerException) {}

    public string EncodingName { get; }
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class EncodingUtils {
    public static Encoding GetEncoding(string name, IReadOnlyDictionary<string, int>? codePageCollationTable, EncodingSelectionCallback? selectFallbackEncoding) {}
    public static Encoding? GetEncoding(string name) {}
    public static Encoding? GetEncoding(string name, EncodingSelectionCallback? selectFallbackEncoding) {}
    public static Encoding GetEncodingThrowException(string name) {}
    public static Encoding GetEncodingThrowException(string name, EncodingSelectionCallback? selectFallbackEncoding) {}
    public static bool TryGetEncoding(string? name, IReadOnlyDictionary<string, int>? codePageCollationTable, EncodingSelectionCallback? selectFallbackEncoding, out Encoding? encoding) {}
  }
}
