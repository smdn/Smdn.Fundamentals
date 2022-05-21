// Smdn.Fundamental.Buffer.dll (Smdn.Fundamental.Buffer-3.0.3)
//   Name: Smdn.Fundamental.Buffer
//   AssemblyVersion: 3.0.3.0
//   InformationalVersion: 3.0.3+0eca389a90f3c44689f04f1d6d623d990adf601a
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release

using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Smdn.Buffers {
  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  public sealed class DuplicateBufferWriter {
    public DuplicateBufferWriter() {}

    public static IBufferWriter<T> Create<T>(IBufferWriter<T> firstWriter, IBufferWriter<T> secondWriter) {}
  }

  public static class ReadOnlySequenceExtensions {
    [return: Nullable(1)] public static string CreateString(this ReadOnlySequence<byte> sequence) {}
    public static bool SequenceEqual<T>([Nullable] this ReadOnlySequence<T> sequence, [Nullable] ReadOnlySpan<T> @value) where T : IEquatable<T> {}
    public static bool SequenceEqualIgnoreCase(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> @value) {}
    public static bool StartsWith<T>([Nullable] this ReadOnlySequence<T> sequence, [Nullable] ReadOnlySpan<T> @value) where T : IEquatable<T> {}
  }
}

