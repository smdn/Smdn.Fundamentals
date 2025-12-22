// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;

namespace Smdn.Text.Unicode.ControlPictures;

public static class ReadOnlySequenceExtensions {
  public static bool TryPicturizeControlChars(this ReadOnlySequence<byte> sequence, Span<char> destination)
  {
    if (sequence.IsEmpty)
      return true;

    var pos = sequence.Start;

    while (sequence.TryGet(ref pos, out var memory, advance: true)) {
      if (!ReadOnlySpanExtensions.TryPicturizeControlChars(memory.Span, destination))
        return false;

      destination = destination.Slice(memory.Length);
    }

    return true;
  }

  public static string ToControlCharsPicturizedString(this ReadOnlySequence<byte> sequence)
  {
#if SYSTEM_STRING_CREATE
    return string.Create(
      (int)Math.Min(int.MaxValue, sequence.Length),
      sequence,
      static (span, seq) => seq.TryPicturizeControlChars(span)
    );
#else
    char[]? buffer = null;

    try {
      var length = (int)Math.Min(int.MaxValue, sequence.Length);

      buffer = ArrayPool<char>.Shared.Rent(length);

      TryPicturizeControlChars(sequence, buffer.AsSpan(0, length));

      return new string(buffer, 0, length);
    }
    finally {
      if (buffer is not null)
        ArrayPool<char>.Shared.Return(buffer);
    }
#endif
  }
}
