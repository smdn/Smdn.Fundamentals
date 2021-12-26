// Smdn.Fundamental.CryptoTransform.dll (Smdn.Fundamental.CryptoTransform-3.0.1 (net45))
//   Name: Smdn.Fundamental.CryptoTransform
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1 (net45)
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release

using System.Security.Cryptography;
using System.Text;

namespace Smdn.Security.Cryptography {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ICryptoTransformExtensions {
    public static byte[] TransformBytes(this ICryptoTransform transform, byte[] inputBuffer) {}
    public static byte[] TransformBytes(this ICryptoTransform transform, byte[] inputBuffer, int inputOffset, int inputCount) {}
    public static string TransformStringFrom(this ICryptoTransform transform, string str, Encoding encoding) {}
    public static string TransformStringTo(this ICryptoTransform transform, string str, Encoding encoding) {}
  }
}

