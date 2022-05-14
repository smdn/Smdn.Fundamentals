// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class Hexadecimal {
#pragma warning restore IDE0040
  public static bool TryEncodeUpperCase(ArraySegment<byte> dataSequence, ArraySegment<byte> destination, out int bytesEncoded)
#if SYSTEM_READONLYSPAN
    => TryEncode(dataSequence.AsSpan(), destination.AsSpan(), UpperCaseHexOctets, out bytesEncoded);
#else
    => TryEncode(dataSequence, destination, upperCaseHexOctets, out bytesEncoded);
#endif

  public static bool TryEncodeLowerCase(ArraySegment<byte> dataSequence, ArraySegment<byte> destination, out int bytesEncoded)
#if SYSTEM_READONLYSPAN
    => TryEncode(dataSequence.AsSpan(), destination.AsSpan(), LowerCaseHexOctets, out bytesEncoded);
#else
    => TryEncode(dataSequence, destination, lowerCaseHexOctets, out bytesEncoded);
#endif

  public static bool TryEncodeUpperCase(ArraySegment<byte> dataSequence, ArraySegment<char> destination, out int charsEncoded)
#if SYSTEM_READONLYSPAN
    => TryEncode(dataSequence.AsSpan(), destination.AsSpan(), UpperCaseHexChars, out charsEncoded);
#else
    => TryEncode(dataSequence, destination, upperCaseHexChars, out charsEncoded);
#endif

  public static bool TryEncodeLowerCase(ArraySegment<byte> dataSequence, ArraySegment<char> destination, out int charsEncoded)
#if SYSTEM_READONLYSPAN
    => TryEncode(dataSequence.AsSpan(), destination.AsSpan(), LowerCaseHexChars, out charsEncoded);
#else
    => TryEncode(dataSequence, destination, lowerCaseHexChars, out charsEncoded);
#endif

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
#else
  internal static bool TryEncode<T>(ArraySegment<byte> dataSequence, ArraySegment<T> destination, T[] hex, out int lengthEncoded)
  {
    lengthEncoded = 0;

    if (dataSequence.Count == 0)
      return true;

    if (destination.Count < dataSequence.Count * 2)
      return false; // destination too short

    while (dataSequence.Count != 0) {
      if (!TryEncode(dataSequence.Array[dataSequence.Offset], destination.Array, destination.Offset, hex, out var len))
        return false;

      lengthEncoded += len;
      dataSequence = new(dataSequence.Array, dataSequence.Offset + 1, dataSequence.Count - 1);
      destination = new(destination.Array, destination.Offset + 2, destination.Count - 2);
    }

    return true;
  }
#endif
}
