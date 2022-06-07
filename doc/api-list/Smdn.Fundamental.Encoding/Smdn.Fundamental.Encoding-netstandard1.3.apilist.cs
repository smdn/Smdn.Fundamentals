// Smdn.Fundamental.Encoding.dll (Smdn.Fundamental.Encoding-3.0.3)
//   Name: Smdn.Fundamental.Encoding
//   AssemblyVersion: 3.0.3.0
//   InformationalVersion: 3.0.3+2101d134632778e05b53d9a72f315e0bed535e40
//   TargetFramework: .NETStandard,Version=v1.3
//   Configuration: Release

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Smdn.Text.Encodings;

namespace Smdn.Text.Encodings {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public delegate Encoding EncodingSelectionCallback(string name);

  [Nullable(byte.MinValue)]
  [NullableContext(1)]
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

  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class EncodingUtils {
    [return: Nullable(2)] public static Encoding GetEncoding(string name) {}
    [NullableContext(2)]
    public static Encoding GetEncoding([Nullable(1)] string name, EncodingSelectionCallback selectFallbackEncoding) {}
    public static Encoding GetEncoding(string name, [Nullable] IReadOnlyDictionary<string, int> codePageCollationTable, [Nullable(2)] EncodingSelectionCallback selectFallbackEncoding) {}
    public static Encoding GetEncodingThrowException(string name) {}
    public static Encoding GetEncodingThrowException(string name, [Nullable(2)] EncodingSelectionCallback selectFallbackEncoding) {}
    [NullableContext(2)]
    public static bool TryGetEncoding(string name, [Nullable] IReadOnlyDictionary<string, int> codePageCollationTable, EncodingSelectionCallback selectFallbackEncoding, out Encoding encoding) {}
  }
}

