// Smdn.Fundamental.Buffer.dll (Smdn.Fundamental.Buffer-3.0.1 (netstandard1.6))
//   Name: Smdn.Fundamental.Buffer
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1 (netstandard1.6)
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release

using System;
using System.Buffers;

namespace Smdn.Buffers {
  public sealed class DuplicateBufferWriter {
    public DuplicateBufferWriter() {}

    public static IBufferWriter<T> Create<T>(IBufferWriter<T> firstWriter, IBufferWriter<T> secondWriter) {}
  }

  public static class ReadOnlySequenceExtensions {
    public static string CreateString(this ReadOnlySequence<byte> sequence) {}
    public static bool SequenceEqual<T>(this ReadOnlySequence<T> sequence, ReadOnlySpan<T> @value) where T : IEquatable<T> {}
    public static bool SequenceEqualIgnoreCase(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> @value) {}
    public static bool StartsWith<T>(this ReadOnlySequence<T> sequence, ReadOnlySpan<T> @value) where T : IEquatable<T> {}
  }
}

