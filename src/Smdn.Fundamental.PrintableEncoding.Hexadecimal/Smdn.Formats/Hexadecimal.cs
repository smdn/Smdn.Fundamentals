// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Smdn.Formats {
  public static class Hexadecimal {
    private const string UpperCaseHexCharsInString = "0123456789ABCDEF";
    private const string LowerCaseHexCharsInString = "0123456789abcdef";

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
    public static ReadOnlySpan<char> UpperCaseHexChars => UpperCaseHexCharsInString.AsSpan();
    public static ReadOnlySpan<char> LowerCaseHexChars => LowerCaseHexCharsInString.AsSpan();

    public static string ToUpperCaseString(ReadOnlySpan<byte> dataSequence)
    {
#if false && SYSTEM_STRING_CREATE
      // XXX: string.Create does not accept ReadOnlySpan<T>, dotnet/runtime#30175
      return string.Create(
        dataSequence.Length * 2,
        dataSequence,
        static (destination, sequence) => TryEncodeUpperCase(sequence, destination, out _)
      );
#else
      char[] destination = null;

      try {
        var length = dataSequence.Length * 2;

        destination = ArrayPool<char>.Shared.Rent(length);

        TryEncodeUpperCase(dataSequence, destination.AsSpan(0, length), out _);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        return new string(destination.AsSpan(0, length));
#else
        return new string(destination, 0, length);
#endif
      }
      finally {
        if (destination is not null)
          ArrayPool<char>.Shared.Return(destination);
      }
#endif
    }

    public static string ToLowerCaseString(ReadOnlySpan<byte> dataSequence)
    {
#if false && SYSTEM_STRING_CREATE
      // XXX: string.Create does not accept ReadOnlySpan<T>, dotnet/runtime#30175
      return string.Create(
        dataSequence.Length * 2,
        dataSequence,
        static (destination, sequence) => TryEncodeLowerCase(sequence, destination, out _)
      );
#else
      char[] destination = null;

      try {
        var length = dataSequence.Length * 2;

        destination = ArrayPool<char>.Shared.Rent(length);

        TryEncodeLowerCase(dataSequence, destination.AsSpan(0, length), out _);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        return new string(destination.AsSpan(0, length));
#else
        return new string(destination, 0, length);
#endif
      }
      finally {
        if (destination is not null)
          ArrayPool<char>.Shared.Return(destination);
      }
#endif
    }

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
        return false; // destination to short

      while (!dataSequence.IsEmpty) {
        if (!TryEncode(dataSequence[0], destination, hex, out var len))
          return false;

        lengthEncoded += len;
        dataSequence = dataSequence.Slice(1);
        destination = destination.Slice(2);
      }

      return true;
    }

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

    public static bool TryDecodeUpperCase(ReadOnlySpan<char> upperCaseDataSequence, Span<byte> destination, out int decodedLength)
      => TryDecode(upperCaseDataSequence, destination, allowUpperCase: true, allowLowerCase: false, out decodedLength);

    public static bool TryDecodeLowerCase(ReadOnlySpan<char> lowerCaseDataSequence, Span<byte> destination, out int decodedLength)
      => TryDecode(lowerCaseDataSequence, destination, allowUpperCase: false, allowLowerCase: true, out decodedLength);

    public static bool TryDecode(ReadOnlySpan<char> dataSequence, Span<byte> destination, out int decodedLength)
      => TryDecode(dataSequence, destination, allowUpperCase: true, allowLowerCase: true, out decodedLength);

    internal static bool TryDecode(ReadOnlySpan<char> dataSequence, Span<byte> destination, bool allowUpperCase, bool allowLowerCase, out int decodedLength)
    {
      decodedLength = 0;

      if (dataSequence.IsEmpty)
        return true;

      var length = dataSequence.Length;

      if ((length & 0x1) != 0)
        return false; // incorrect form

      length >>= 1;

      if (destination.Length < length)
        return false; // destination too short

      for (var i = 0; i < length; i++) {
        if (!TryDecode(dataSequence.Slice(0, 2), allowUpperCase, allowLowerCase, out byte decodedData))
          return false;

        destination[i] = decodedData;

        dataSequence = dataSequence.Slice(2);
        decodedLength++;
      }

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

    public static bool TryDecodeUpperCaseValue(char upperCaseData, out byte decodedValue)
      => TryDecodeValue(upperCaseData, allowUpperCase: true, allowLowerCase: false, out decodedValue);

    public static bool TryDecodeLowerCaseValue(char lowerCaseData, out byte decodedValue)
      => TryDecodeValue(lowerCaseData, allowUpperCase: false, allowLowerCase: true, out decodedValue);

    public static bool TryDecodeValue(char data, out byte decodedValue)
      => TryDecodeValue(data, allowUpperCase: true, allowLowerCase: true, out decodedValue);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryDecodeValue(char data, bool allowUpperCase, bool allowLowerCase, out byte decodedValue)
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
