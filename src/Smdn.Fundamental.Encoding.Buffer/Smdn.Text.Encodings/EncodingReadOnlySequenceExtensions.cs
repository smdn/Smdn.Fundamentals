// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;
using System.Text;

namespace Smdn.Text.Encodings;

public static class EncodingReadOnlySequenceExtensions {
#if !NET5_0_OR_GREATER
  public static string GetString(this Encoding encoding, ReadOnlySequence<byte> sequence)
  {
    var sb = new StringBuilder((int)Math.Min(int.MaxValue, sequence.Length));
    var decoder = encoding.GetDecoder();
    var pos = sequence.Start;
    char[] buffer = null;

    try {
      while (sequence.TryGet(ref pos, out var memory, advance: true)) {
        var doFlush = sequence.End.Equals(pos);

#if NETSTANDARD2_1_OR_GREATER
        var count = decoder.GetCharCount(memory.Span, doFlush);
#else
        var chars = memory.ToArray();
        var count = decoder.GetCharCount(chars, 0, chars.Length, doFlush);
#endif

        if (buffer is null) {
          buffer = ArrayPool<char>.Shared.Rent(count);
        }
        else if (buffer.Length < count) {
          ArrayPool<char>.Shared.Return(buffer);
          buffer = ArrayPool<char>.Shared.Rent(count);
        }

#if NETSTANDARD2_1_OR_GREATER
        var len = decoder.GetChars(memory.Span, buffer, doFlush);
#else
        var len = decoder.GetChars(chars, 0, chars.Length, buffer, 0, doFlush);
#endif

        sb.Append(buffer, 0, len);
      }
    }
    finally {
      if (buffer is not null)
        ArrayPool<char>.Shared.Return(buffer);
    }

    return sb.ToString();
  }
#endif
}
