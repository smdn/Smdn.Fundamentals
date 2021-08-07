// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.CompilerServices;

namespace Smdn.Formats {
  public static class Hexadecimal {
    private static readonly ReadOnlyMemory<byte> upperCaseHexOctets = new byte[] {
      0x30, 0x31, 0x32, 0x33,
      0x34, 0x35, 0x36, 0x37,
      0x38, 0x39, 0x41, 0x42,
      0x43, 0x44, 0x45, 0x46,
    };

    public static ReadOnlySpan<byte> UpperCaseHexOctets => upperCaseHexOctets.Span;

    private static readonly ReadOnlyMemory<byte> lowerCaseHexOctets = new byte[] {
      0x30, 0x31, 0x32, 0x33,
      0x34, 0x35, 0x36, 0x37,
      0x38, 0x39, 0x61, 0x62,
      0x63, 0x64, 0x65, 0x66,
    };

    public static ReadOnlySpan<byte> LowerCaseHexOctets => lowerCaseHexOctets.Span;

    private const string upperCaseHexChars = "0123456789ABCDEF";

    public static ReadOnlySpan<char> UpperCaseHexChars => upperCaseHexChars.AsSpan();

    private const string lowerCaseHexChars = "0123456789abcdef";

    public static ReadOnlySpan<char> LowerCaseHexChars => lowerCaseHexChars.AsSpan();

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

    public static bool TryDecodeUpperCase(ReadOnlySpan<byte> upperCaseData, out byte decodedData)
      => TryDecode(upperCaseData, allowUpperCase: true, allowLowerCase: false, out decodedData);

    public static bool TryDecodeLowerCase(ReadOnlySpan<byte> lowerCaseData, out byte decodedData)
      => TryDecode(lowerCaseData, allowUpperCase: false, allowLowerCase: true, out decodedData);

    public static bool TryDecode(ReadOnlySpan<byte> data, out byte decodedData)
      => TryDecode(data, allowUpperCase: true, allowLowerCase: true, out decodedData);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryDecode(ReadOnlySpan<byte> data, bool allowUpperCase, bool allowLowerCase, out byte decodedData)
    {
      decodedData = 0;

      if (data.Length < 2)
        return false;
      if (!TryDecodeValue(data[0], allowUpperCase, allowLowerCase, out var high))
        return false;
      if (!TryDecodeValue(data[1], allowUpperCase, allowLowerCase, out var low))
        return false;

      decodedData = (byte)((high << 4) | low);

      return true;
    }

    public static bool TryDecodeUpperCaseValue(byte upperCaseData, out byte decodedValue)
      => TryDecodeValue(upperCaseData, allowUpperCase: true, allowLowerCase: false, out decodedValue);

    public static bool TryDecodeLowerCaseValue(byte lowerCaseData, out byte decodedValue)
      => TryDecodeValue(lowerCaseData, allowUpperCase: false, allowLowerCase: true, out decodedValue);

    public static bool TryDecodeValue(byte data, out byte decodedValue)
      => TryDecodeValue(data, allowUpperCase: true, allowLowerCase: true, out decodedValue);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryDecodeValue(byte data, bool allowUpperCase, bool allowLowerCase, out byte decodedValue)
    {
      if (0x30 <= data && data <= 0x39) { // '0' 0x30 to '9' 0x39
        decodedValue = (byte)(data - 0x30);
        return true;
      }
      else if (allowUpperCase && 0x41 <= data && data <= 0x46) { // 'A' 0x41 to 'F' 0x46
        decodedValue = (byte)(data - 0x37);
        return true;
      }
      else if (allowLowerCase && 0x61 <= data && data <= 0x66) { // 'a' 0x61 to 'f' 0x66
        decodedValue = (byte)(data - 0x57);
        return true;
      }
      else {
        decodedValue = default;
        return false;
      }
    }
  }
}
