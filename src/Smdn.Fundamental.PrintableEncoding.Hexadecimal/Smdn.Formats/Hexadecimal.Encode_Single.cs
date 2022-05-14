// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.CompilerServices;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class Hexadecimal {
#pragma warning restore IDE0040
#if SYSTEM_READONLYSPAN
  public static bool TryEncodeUpperCase(byte data, Span<byte> destination, out int bytesEncoded)
    => TryEncode(data, destination, UpperCaseHexOctets, out bytesEncoded);

  public static bool TryEncodeLowerCase(byte data, Span<byte> destination, out int bytesEncoded)
    => TryEncode(data, destination, LowerCaseHexOctets, out bytesEncoded);

  public static bool TryEncodeUpperCase(byte data, Span<char> destination, out int charsEncoded)
    => TryEncode(data, destination, UpperCaseHexChars, out charsEncoded);

  public static bool TryEncodeLowerCase(byte data, Span<char> destination, out int charsEncoded)
    => TryEncode(data, destination, LowerCaseHexChars, out charsEncoded);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool TryEncode<T>(byte data, Span<T> destination, ReadOnlySpan<T> hex, out int lengthEncoded)
  {
    lengthEncoded = default;

    if (destination.Length < 2)
      return false;

    destination[lengthEncoded++] = hex[data >> 4];
    destination[lengthEncoded++] = hex[data & 0xf];

    return true;
  }
#endif
}
