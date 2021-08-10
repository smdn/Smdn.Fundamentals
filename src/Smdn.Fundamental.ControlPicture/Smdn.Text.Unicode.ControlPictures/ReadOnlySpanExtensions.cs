// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;
using System.Text.Unicode;

namespace Smdn.Text.Unicode.ControlPictures {
  public static class ReadOnlySpanExtensions {
    public static bool TryPicturizeControlChars(this ReadOnlySpan<byte> span, Span<char> destination)
    {
      if (destination.Length < span.Length)
        return false;

      for (var i = 0; i < span.Length; i++) {
        // replace control characters to control pictures
        if (0x00 <= span[i] && span[i] <= 0x20) // CO Control chars + SP
          destination[i] = (char)(UnicodeRanges.ControlPictures.FirstCodePoint + span[i]); // U+2400-U+2420
        else if (span[i] == 0x7F) // DEL
          destination[i] = (char)(UnicodeRanges.ControlPictures.FirstCodePoint + 0x21); // U+2421 SYMBOL FOR DELETE
        else if (span[i] == 0x85) // C1 NEL
          destination[i] = (char)(UnicodeRanges.ControlPictures.FirstCodePoint + 0x24); // U+2424 SYMBOL FOR NEWLINE
        else
          destination[i] = (char)span[i];
      }

      return true;
    }

    public static bool TryPicturizeControlChars(this ReadOnlySpan<char> span, Span<char> destination)
    {
      if (destination.Length < span.Length)
        return false;

      for (var i = 0; i < span.Length; i++) {
        // replace control characters to control pictures
        if (char.IsSurrogate(span[i]))
          destination[i] = span[i];
        else if (0x00 <= span[i] && span[i] <= 0x20) // CO Control chars + SP
          destination[i] = (char)(UnicodeRanges.ControlPictures.FirstCodePoint + span[i]); // U+2400-U+2420
        else if (span[i] == 0x7F) // DEL
          destination[i] = (char)(UnicodeRanges.ControlPictures.FirstCodePoint + 0x21); // U+2421 SYMBOL FOR DELETE
        else if (span[i] == 0x85) // C1 NEL
          destination[i] = (char)(UnicodeRanges.ControlPictures.FirstCodePoint + 0x24); // U+2424 SYMBOL FOR NEWLINE
        else
          destination[i] = (char)span[i];
      }

      return true;
    }

    public static string ToControlCharsPicturizedString(this ReadOnlySpan<byte> span)
    {
      if (span.IsEmpty)
        return string.Empty;

      char[] buffer = null;

      try {
        buffer = ArrayPool<char>.Shared.Rent(span.Length);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        var buf = buffer.AsSpan(0, span.Length);

        TryPicturizeControlChars(span, buf);

        // string.Create(ReadOnlySpan) https://github.com/dotnet/runtime/issues/30175
        return new string(buf);
#else
        TryPicturizeControlChars(span, buffer.AsSpan(0, span.Length));

        return new string(buffer, 0, span.Length);
#endif
      }
      finally {
        if (buffer is not null)
          ArrayPool<char>.Shared.Return(buffer);
      }
    }

    public static string ToControlCharsPicturizedString(this ReadOnlySpan<char> span)
    {
      if (span.IsEmpty)
        return string.Empty;

      char[] buffer = null;

      try {
        buffer = ArrayPool<char>.Shared.Rent(span.Length);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        var buf = buffer.AsSpan(0, span.Length);

        TryPicturizeControlChars(span, buf);

        // string.Create(ReadOnlySpan) https://github.com/dotnet/runtime/issues/30175
        return new string(buf);
#else
        TryPicturizeControlChars(span, buffer.AsSpan(0, span.Length));

        return new string(buffer, 0, span.Length);
#endif
      }
      finally {
        if (buffer is not null)
          ArrayPool<char>.Shared.Return(buffer);
      }
    }
  }
}