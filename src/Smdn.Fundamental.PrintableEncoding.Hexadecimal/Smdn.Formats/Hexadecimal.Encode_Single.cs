// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET10_0_OR_GREATER
#define SYSTEM_CONVERT_TRYTOHEXSTRING
#define SYSTEM_CONVERT_TRYTOHEXSTRINGLOWER
#endif

#if SYSTEM_SPAN
using System;
#endif
using System.Runtime.CompilerServices;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class Hexadecimal {
#pragma warning restore IDE0040
#if SYSTEM_SPAN
  private static bool ValidateTryEncodeDestination(byte[] destination, int index)
    => !(destination is null || index < 0 || destination.Length - 2 < index);

  private static bool ValidateTryEncodeDestination(char[] destination, int index)
    => !(destination is null || index < 0 || destination.Length - 2 < index);
#endif

  public static bool TryEncodeUpperCase(byte data, byte[] destination, int index, out int bytesEncoded)
  {
#if SYSTEM_SPAN
    bytesEncoded = 0;

    return
      ValidateTryEncodeDestination(destination, index) &&
      TryEncode(data, destination.AsSpan(index), UpperCaseHexOctets, out bytesEncoded);
#else
    return TryEncode(data, destination, index, UpperCaseHexOctetBytes, out bytesEncoded);
#endif
  }

  public static bool TryEncodeLowerCase(byte data, byte[] destination, int index, out int bytesEncoded)
  {
#if SYSTEM_SPAN
    bytesEncoded = 0;

    return
      ValidateTryEncodeDestination(destination, index) &&
      TryEncode(data, destination.AsSpan(index), LowerCaseHexOctets, out bytesEncoded);
#else
    return TryEncode(data, destination, index, LowerCaseHexOctetBytes, out bytesEncoded);
#endif
  }

  public static bool TryEncodeUpperCase(byte data, char[] destination, int index, out int charsEncoded)
  {
#if SYSTEM_CONVERT_TRYTOHEXSTRING
    charsEncoded = default;

    return
      ValidateTryEncodeDestination(destination, index) &&
      Convert.TryToHexString(
        source: [data],
        destination: destination.AsSpan(index),
        charsWritten: out charsEncoded
      );
#elif SYSTEM_SPAN
    charsEncoded = default;

    return
      ValidateTryEncodeDestination(destination, index) &&
      TryEncode(data, destination.AsSpan(index), UpperCaseHexChars, out charsEncoded);
#else
    return TryEncode(data, destination, index, UpperCaseHexChars, out charsEncoded);
#endif
  }

  public static bool TryEncodeLowerCase(byte data, char[] destination, int index, out int charsEncoded)
  {
#if SYSTEM_CONVERT_TRYTOHEXSTRINGLOWER
    charsEncoded = default;

    return
      ValidateTryEncodeDestination(destination, index) &&
      Convert.TryToHexStringLower(
        source: [data],
        destination: destination.AsSpan(index),
        charsWritten: out charsEncoded
      );
#elif SYSTEM_SPAN
    charsEncoded = default;

    return
      ValidateTryEncodeDestination(destination, index) &&
      TryEncode(data, destination.AsSpan(index), LowerCaseHexChars, out charsEncoded);
#else
    return TryEncode(data, destination, index, LowerCaseHexChars, out charsEncoded);
#endif
  }

#if SYSTEM_SPAN
  public static bool TryEncodeUpperCase(byte data, Span<byte> destination, out int bytesEncoded)
    => TryEncode(data, destination, UpperCaseHexOctets, out bytesEncoded);

  public static bool TryEncodeLowerCase(byte data, Span<byte> destination, out int bytesEncoded)
    => TryEncode(data, destination, LowerCaseHexOctets, out bytesEncoded);

  public static bool TryEncodeUpperCase(byte data, Span<char> destination, out int charsEncoded)
#if SYSTEM_CONVERT_TRYTOHEXSTRING
    => Convert.TryToHexString(source: [data], destination: destination, charsWritten: out charsEncoded);
#else
    => TryEncode(data, destination, UpperCaseHexChars, out charsEncoded);
#endif

  public static bool TryEncodeLowerCase(byte data, Span<char> destination, out int charsEncoded)
#if SYSTEM_CONVERT_TRYTOHEXSTRINGLOWER
    => Convert.TryToHexStringLower(source: [data], destination: destination, charsWritten: out charsEncoded);
#else
    => TryEncode(data, destination, LowerCaseHexChars, out charsEncoded);
#endif
#endif

#if SYSTEM_SPAN
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
#else
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool TryEncode<T>(byte data, T[] destination, int index, T[] hex, out int lengthEncoded)
  {
    lengthEncoded = default;

    if (destination is null)
      return false;
    if (index < 0)
      return false;
    if (destination.Length - 2 < index)
      return false;

    destination[index + lengthEncoded++] = hex[data >> 4];
    destination[index + lengthEncoded++] = hex[data & 0xf];

    return true;
  }
#endif
}
