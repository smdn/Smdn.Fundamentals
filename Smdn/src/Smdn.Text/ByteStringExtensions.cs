// 
// Copyright (c) 2019 smdn <smdn@smdn.jp>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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
