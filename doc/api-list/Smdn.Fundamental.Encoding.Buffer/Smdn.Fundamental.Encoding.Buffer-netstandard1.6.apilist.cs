// Smdn.Fundamental.Encoding.Buffer.dll (Smdn.Fundamental.Encoding.Buffer-3.0.0)
//   Name: Smdn.Fundamental.Encoding.Buffer
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard1.6)
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release
//   Referenced assemblies:
//     System.Buffers, Version=4.0.2.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Runtime, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.Extensions, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Text.Encoding, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a

using System.Buffers;
using System.Text;

namespace Smdn.Text.Encodings {
  public static class EncodingReadOnlySequenceExtensions {
    public static string GetString(this Encoding encoding, ReadOnlySequence<byte> sequence) {}
  }
}
