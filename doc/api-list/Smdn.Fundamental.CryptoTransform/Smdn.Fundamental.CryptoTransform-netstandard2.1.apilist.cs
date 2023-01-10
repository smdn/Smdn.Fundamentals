// Smdn.Fundamental.CryptoTransform.dll (Smdn.Fundamental.CryptoTransform-3.0.2)
//   Name: Smdn.Fundamental.CryptoTransform
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+9cea7b339599949ee66f26f57ac11445d6975b49
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Encoding.OctetEncoding, Version=3.0.2.0, Culture=neutral
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
#nullable enable annotations

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
