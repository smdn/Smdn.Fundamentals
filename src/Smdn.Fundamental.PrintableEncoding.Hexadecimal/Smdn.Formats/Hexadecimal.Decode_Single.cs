// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.CompilerServices;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class Hexadecimal {
#pragma warning restore IDE0040
#if SYSTEM_READONLYSPAN
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
#endif

#if SYSTEM_READONLYSPAN
  public static bool TryDecodeUpperCase(ReadOnlySpan<char> upperCaseData, out byte decodedData)
    => TryDecode(upperCaseData, allowUpperCase: true, allowLowerCase: false, out decodedData);

  public static bool TryDecodeLowerCase(ReadOnlySpan<char> lowerCaseData, out byte decodedData)
    => TryDecode(lowerCaseData, allowUpperCase: false, allowLowerCase: true, out decodedData);

  public static bool TryDecode(ReadOnlySpan<char> data, out byte decodedData)
    => TryDecode(data, allowUpperCase: true, allowLowerCase: true, out decodedData);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool TryDecode(ReadOnlySpan<char> data, bool allowUpperCase, bool allowLowerCase, out byte decodedData)
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
#endif

  public static bool TryDecodeUpperCaseValue(byte upperCaseData, out byte decodedValue)
    => TryDecodeValue(upperCaseData, allowUpperCase: true, allowLowerCase: false, out decodedValue);

  public static bool TryDecodeLowerCaseValue(byte lowerCaseData, out byte decodedValue)
    => TryDecodeValue(lowerCaseData, allowUpperCase: false, allowLowerCase: true, out decodedValue);

  public static bool TryDecodeValue(byte data, out byte decodedValue)
    => TryDecodeValue(data, allowUpperCase: true, allowLowerCase: true, out decodedValue);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool TryDecodeValue(byte data, bool allowUpperCase, bool allowLowerCase, out byte decodedValue)
  {
    if (data is >= 0x30 and <= 0x39) { // '0' 0x30 to '9' 0x39
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

  public static bool TryDecodeUpperCaseValue(char upperCaseData, out byte decodedValue)
    => TryDecodeValue(upperCaseData, allowUpperCase: true, allowLowerCase: false, out decodedValue);

  public static bool TryDecodeLowerCaseValue(char lowerCaseData, out byte decodedValue)
    => TryDecodeValue(lowerCaseData, allowUpperCase: false, allowLowerCase: true, out decodedValue);

  public static bool TryDecodeValue(char data, out byte decodedValue)
    => TryDecodeValue(data, allowUpperCase: true, allowLowerCase: true, out decodedValue);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool TryDecodeValue(char data, bool allowUpperCase, bool allowLowerCase, out byte decodedValue)
  {
    if (data is >= (char)0x30 and <= (char)0x39) { // '0' 0x30 to '9' 0x39
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
