// Smdn.Fundamental.Buffer.dll (Smdn.Fundamental.Buffer-3.0.3)
//   Name: Smdn.Fundamental.Buffer
//   AssemblyVersion: 3.0.3.0
//   InformationalVersion: 3.0.3+0eca389a90f3c44689f04f1d6d623d990adf601a
//   TargetFramework: .NETCoreApp,Version=v6.0
//   Configuration: Release
#nullable enable annotations

using System;
using System.Buffers;

namespace Smdn.Buffers {
  public sealed class DuplicateBufferWriter {
    public DuplicateBufferWriter() {}

    public static IBufferWriter<T?> Create<T>(IBufferWriter<T?> firstWriter, IBufferWriter<T?> secondWriter) {}
  }

  public static class ReadOnlySequenceExtensions {
    public static string CreateString(this ReadOnlySequence<byte> sequence) {}
    public static bool SequenceEqual<T>(this ReadOnlySequence<T> sequence, ReadOnlySpan<T> @value) where T : IEquatable<T> {}
    public static bool SequenceEqualIgnoreCase(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> @value) {}
    public static bool StartsWith<T>(this ReadOnlySequence<T> sequence, ReadOnlySpan<T> @value) where T : IEquatable<T> {}
  }

  public static class SequenceReaderExtensions {
    public static ReadOnlySequence<T> GetUnreadSequence<T>(this SequenceReader<T> sequenceReader) where T : unmanaged, IEquatable<T> {}
  }
}
