// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;

namespace Smdn.Text {
  public static class ByteStringExtensions {
    public static ReadOnlySequence<byte> AsReadOnlySequence(this ByteString str)
    {
      return new ReadOnlySequence<byte>(str.Segment.AsMemory());
    }

    [Obsolete()]
    public static ByteString ToByteString(this ReadOnlySequence<byte> sequence)
    {
      return ByteString.CreateImmutable(sequence.ToArray());
    }

    public static bool SequenceEqual(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> value)
    {
      if (sequence.Length != value.Length)
        return false;

      var offset = 0;
      var pos = sequence.Start;

      while (sequence.TryGet(ref pos, out var memory, advance: true)) {
        if (memory.Length == 0)
          continue; // XXX: never happen?

        if (!value.Slice(offset, memory.Length).SequenceEqual(memory.Span))
          return false;

        offset += memory.Length;
      }

      return true;
    }

    public static bool StartsWith(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> value)
    {
      if (value.Length == 0)
        return true;
      if (sequence.Length < value.Length)
        return false;

      return SequenceEqual(sequence.Slice(0, value.Length), value);
    }

    public static unsafe byte[] ToArrayUpperCase(this ReadOnlySequence<byte> sequence)
    {
      if (sequence.IsEmpty)
#if NET45 || NET452
        return ArrayExtensions.Empty<byte>();
#else
        return Array.Empty<byte>();
#endif

      var bytes = new byte[sequence.Length];

      fixed (byte* b0 = bytes) {
        byte* b = b0;

        var position = sequence.Start;

        while (sequence.TryGet(ref position, out var memory, advance: true)) {
          var span = memory.Span;

          fixed (byte* s0 = span) {
            byte* s = s0;

            for (var i = 0; i < span.Length; i++) {
              *b++ = Ascii.Octets.ToUpperCaseAsciiTableArray[*s++];
            }
          }
        }
      }

      return bytes;
    }
  }
}
