// Smdn.Fundamental.Encoding.Buffer.dll (Smdn.Fundamental.Encoding.Buffer-3.0.0)
//   Name: Smdn.Fundamental.Encoding.Buffer
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard2.1)
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release
#nullable enable annotations

using System.Buffers;
using System.Text;

namespace Smdn.Text.Encodings {
  public static class EncodingReadOnlySequenceExtensions {
    public static string GetString(this Encoding encoding, ReadOnlySequence<byte> sequence) {}
  }
}
