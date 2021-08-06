// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

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

    public static bool TryEncodeUpperCase(byte data, Span<byte> destination, out int bytesEncoded)
      => TryEncode(data, destination, UpperCaseHexOctets, out bytesEncoded);

    public static bool TryEncodeLowerCase(byte data, Span<byte> destination, out int bytesEncoded)
      => TryEncode(data, destination, LowerCaseHexOctets, out bytesEncoded);

    internal static bool TryEncode(byte data, Span<byte> destination, ReadOnlySpan<byte> octets, out int bytesEncoded)
    {
      bytesEncoded = default;

      if (destination.Length < 2)
        return false;

      destination[bytesEncoded++] = octets[data >> 4];
      destination[bytesEncoded++] = octets[data & 0xf];

      return true;
    }

    public static bool TryDecodeUpperCase(ReadOnlySpan<byte> upperCaseData, out byte decodedData)
      => TryDecode(upperCaseData, allowUpperCase: true, allowLowerCase: false, out decodedData);

    public static bool TryDecodeLowerCase(ReadOnlySpan<byte> lowerCaseData, out byte decodedData)
      => TryDecode(lowerCaseData, allowUpperCase: false, allowLowerCase: true, out decodedData);

    public static bool TryDecode(ReadOnlySpan<byte> data, out byte decodedData)
      => TryDecode(data, allowUpperCase: true, allowLowerCase: true, out decodedData);

    internal static bool TryDecode(ReadOnlySpan<byte> data, bool allowUpperCase, bool allowLowerCase, out byte decodedData)
    {
      decodedData = 0;

      if (data.Length < 2)
        return false;

      var high = data[0];

      if (0x30 <= high && high <= 0x39) // '0' 0x30 to '9' 0x39
        decodedData = (byte)((high - 0x30) << 4);
      else if (allowUpperCase && 0x41 <= high && high <= 0x46) // 'A' 0x41 to 'F' 0x46
        decodedData = (byte)((high - 0x37) << 4);
      else if (allowLowerCase && 0x61 <= high && high <= 0x66) // 'a' 0x61 to 'f' 0x66
        decodedData = (byte)((high - 0x57) << 4);
      else
        return false;

      var low = data[1];

      if (0x30 <= low && low <= 0x39) // '0' 0x30 to '9' 0x39
        decodedData |= (byte)(low - 0x30);
      else if (allowUpperCase && 0x41 <= low && low <= 0x46) // 'A' 0x41 to 'F' 0x46
        decodedData |= (byte)(low - 0x37);
      else if (allowLowerCase && 0x61 <= low && low <= 0x66) // 'a' 0x61 to 'f' 0x66
        decodedData |= (byte)(low - 0x57);
      else
        return false;

      return true;
    }
  }
}
