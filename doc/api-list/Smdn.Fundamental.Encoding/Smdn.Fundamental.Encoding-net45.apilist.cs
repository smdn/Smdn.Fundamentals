// Smdn.Fundamental.Encoding.dll (Smdn.Fundamental.Encoding-3.0.1)
//   Name: Smdn.Fundamental.Encoding
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+403df09df7ef4576307947fac83e89eb7693a163
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release

using System;
using System.Runtime.Serialization;
using System.Text;
using Smdn.Text.Encodings;

namespace Smdn.Text.Encodings {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public delegate Encoding EncodingSelectionCallback(string name);

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
    public static Encoding GetEncoding(string name) {}
    public static Encoding GetEncoding(string name, EncodingSelectionCallback selectFallbackEncoding) {}
    public static Encoding GetEncodingThrowException(string name) {}
    public static Encoding GetEncodingThrowException(string name, EncodingSelectionCallback selectFallbackEncoding) {}
  }
}

