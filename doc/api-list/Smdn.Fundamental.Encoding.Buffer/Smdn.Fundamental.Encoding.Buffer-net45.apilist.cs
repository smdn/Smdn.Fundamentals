// Smdn.Fundamental.Encoding.Buffer.dll (Smdn.Fundamental.Encoding.Buffer-3.0.0)
//   Name: Smdn.Fundamental.Encoding.Buffer
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net45)
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
//   Referenced assemblies:
//     System.Buffers, Version=4.0.3.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089

using System.Buffers;
using System.Text;

namespace Smdn.Text.Encodings {
  public static class EncodingReadOnlySequenceExtensions {
    public static string GetString(this Encoding encoding, ReadOnlySequence<byte> sequence) {}
  }
}
