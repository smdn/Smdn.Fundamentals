// Smdn.Fundamental.Encoding.dll (Smdn.Fundamental.Encoding-3.0.3)
//   Name: Smdn.Fundamental.Encoding
//   AssemblyVersion: 3.0.3.0
//   InformationalVersion: 3.0.3+2101d134632778e05b53d9a72f315e0bed535e40
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
//   Referenced assemblies:
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
#nullable enable annotations

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Smdn.Text.Encodings;

namespace Smdn.Text.Encodings {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public delegate Encoding? EncodingSelectionCallback(string name);

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [Serializable]
  public class EncodingNotSupportedException : NotSupportedException {
    protected EncodingNotSupportedException(SerializationInfo info, StreamingContext context) {}
    public EncodingNotSupportedException() {}
    public EncodingNotSupportedException(string encodingName) {}
    public EncodingNotSupportedException(string encodingName, Exception innerException) {}
    public EncodingNotSupportedException(string encodingName, string message) {}
    public EncodingNotSupportedException(string encodingName, string message, Exception innerException) {}

    public string EncodingName { get; }

    public override void GetObjectData(SerializationInfo info, StreamingContext context) {}
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
