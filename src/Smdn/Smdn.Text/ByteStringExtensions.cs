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

    [Obsolete("use Smdn.Buffers.ReadOnlySequenceExtensions.SequenceEqual instead")]
    public static bool SequenceEqual(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> value)
      => Smdn.Buffers.ReadOnlySequenceExtensions.SequenceEqual(sequence, value);

    [Obsolete("use Smdn.Buffers.ReadOnlySequenceExtensions.StartsWith instead")]
    public static bool StartsWith(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> value)
      => Smdn.Buffers.ReadOnlySequenceExtensions.StartsWith(sequence, value);

    public static unsafe byte[] ToArrayUpperCase(this ReadOnlySequence<byte> sequence)
    {
      if (sequence.IsEmpty)
#if NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NET5_0_OR_GREATER
        return Array.Empty<byte>();
#else
        return ArrayExtensions.Empty<byte>();
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
