// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:words comparand

using System;
using System.Buffers;

namespace Smdn.Buffers;

public static class ReadOnlySequenceExtensions {
  public static bool SequenceEqual<T>(this ReadOnlySequence<T> sequence, ReadOnlySpan<T> value)
    where T : IEquatable<T>
  {
    if (sequence.Length != value.Length)
      return false;

    var pos = sequence.Start;

    while (sequence.TryGet(ref pos, out var memory, advance: true)) {
      if (memory.Length == 0)
        continue; // XXX: never happen?

      if (!value.Slice(0, memory.Length).SequenceEqual<T>(memory.Span))
        return false;

      value = value.Slice(memory.Length);
    }

    return true;
  }

  public static bool SequenceEqualIgnoreCase(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> value)
  {
    if (sequence.Length != value.Length)
      return false;

    var pos = sequence.Start;

    while (sequence.TryGet(ref pos, out var memory, advance: true)) {
      if (memory.Length == 0)
        continue; // XXX: never happen?

      for (var i = 0; i < memory.Length; i++) {
        var comparand0 = memory.Span[i];
        var comparand1 = value[i];

        if (comparand0 is >= ((byte)'A') and <= ((byte)'Z'))
          comparand0 += 0x20;
        if (comparand1 is >= ((byte)'A') and <= ((byte)'Z'))
          comparand1 += 0x20;

        if (comparand0 != comparand1)
          return false;
      }

      value = value.Slice(memory.Length);
    }

    return true;
  }

  public static bool StartsWith<T>(this ReadOnlySequence<T> sequence, ReadOnlySpan<T> value)
    where T : IEquatable<T>
  {
    if (value.Length == 0)
      return true;
    if (sequence.Length < value.Length)
      return false;

    return SequenceEqual(sequence.Slice(0, value.Length), value);
  }

  public static string CreateString(this ReadOnlySequence<byte> sequence)
  {
    if (sequence.IsEmpty)
      return string.Empty;

    static void ToCharSequence(Span<char> destination, ReadOnlySequence<byte> seq)
    {
      var pos = seq.Start;

      while (seq.TryGet(ref pos, out var seqMemory, advance: true)) {
        var seqSpan = seqMemory.Span;

        for (var i = 0; i < seqSpan.Length; i++) {
          destination[i] = (char)seqSpan[i];
        }

        destination = destination.Slice(seqSpan.Length);
      }
    }

#if SYSTEM_STRING_CREATE
    return string.Create(
      (int)Math.Min(int.MaxValue, sequence.Length),
      sequence,
      ToCharSequence
    );
#else
    char[]? buffer = null;
    var length = (int)Math.Min(int.MaxValue, sequence.Length);

    try {
      buffer = ArrayPool<char>.Shared.Rent(length);

      ToCharSequence(buffer.AsSpan(0, length), sequence);

      return new string(buffer, 0, length);
    }
    finally {
      if (buffer is not null)
        ArrayPool<char>.Shared.Return(buffer);
    }
#endif
  }
}
