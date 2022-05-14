// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class Hexadecimal {
#pragma warning restore IDE0040
#if SYSTEM_READONLYSPAN
  public static bool TryEncodeUpperCase(ReadOnlySpan<byte> dataSequence, Span<byte> destination, out int bytesEncoded)
    => TryEncode(dataSequence, destination, UpperCaseHexOctets, out bytesEncoded);

  public static bool TryEncodeLowerCase(ReadOnlySpan<byte> dataSequence, Span<byte> destination, out int bytesEncoded)
    => TryEncode(dataSequence, destination, LowerCaseHexOctets, out bytesEncoded);

  public static bool TryEncodeUpperCase(ReadOnlySpan<byte> dataSequence, Span<char> destination, out int charsEncoded)
    => TryEncode(dataSequence, destination, UpperCaseHexChars, out charsEncoded);

  public static bool TryEncodeLowerCase(ReadOnlySpan<byte> dataSequence, Span<char> destination, out int charsEncoded)
    => TryEncode(dataSequence, destination, LowerCaseHexChars, out charsEncoded);

  internal static bool TryEncode<T>(ReadOnlySpan<byte> dataSequence, Span<T> destination, ReadOnlySpan<T> hex, out int lengthEncoded)
  {
    lengthEncoded = 0;

    if (dataSequence.IsEmpty)
      return true;

    if (destination.Length < dataSequence.Length * 2)
      return false; // destination too short

    while (!dataSequence.IsEmpty) {
      if (!TryEncode(dataSequence[0], destination, hex, out var len))
        return false;

      lengthEncoded += len;
      dataSequence = dataSequence.Slice(1);
      destination = destination.Slice(2);
    }

    return true;
  }
#endif
}
