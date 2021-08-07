// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;

namespace Smdn.Buffers {
  public static class ReadOnlySequenceExtensions {
    public static bool SequenceEqual<T>(this ReadOnlySequence<T> sequence, ReadOnlySpan<T> value)
      where T : IEquatable<T>
    {
      if (sequence.Length != value.Length)
        return false;

      var offset = 0;
      var pos = sequence.Start;

      while (sequence.TryGet(ref pos, out var memory, advance: true)) {
        if (memory.Length == 0)
          continue; // XXX: never happen?

        if (!value.Slice(offset, memory.Length).SequenceEqual<T>(memory.Span))
          return false;

        offset += memory.Length;
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
  }
}