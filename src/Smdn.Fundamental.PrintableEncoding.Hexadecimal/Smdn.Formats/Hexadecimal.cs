// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats;

public static partial class Hexadecimal {
  private const string UpperCaseHexCharsInString = "0123456789ABCDEF";
  private const string LowerCaseHexCharsInString = "0123456789abcdef";

  private static readonly
#if SYSTEM_READONLYMEMORY
  ReadOnlyMemory<byte>
#else
  byte[]
#endif
  UpperCaseHexOctetBytes = new byte[] {
    0x30, 0x31, 0x32, 0x33,
    0x34, 0x35, 0x36, 0x37,
    0x38, 0x39, 0x41, 0x42,
    0x43, 0x44, 0x45, 0x46,
  };

  private static readonly
#if SYSTEM_READONLYMEMORY
  ReadOnlyMemory<byte>
#else
  byte[]
#endif
  LowerCaseHexOctetBytes = new byte[] {
    0x30, 0x31, 0x32, 0x33,
    0x34, 0x35, 0x36, 0x37,
    0x38, 0x39, 0x61, 0x62,
    0x63, 0x64, 0x65, 0x66,
  };

#if SYSTEM_READONLYSPAN && SYSTEM_READONLYMEMORY
  public static ReadOnlySpan<byte> UpperCaseHexOctets => UpperCaseHexOctetBytes.Span;
  public static ReadOnlySpan<byte> LowerCaseHexOctets => LowerCaseHexOctetBytes.Span;
#endif

#if SYSTEM_READONLYSPAN
  public static ReadOnlySpan<char> UpperCaseHexChars => UpperCaseHexCharsInString.AsSpan();
  public static ReadOnlySpan<char> LowerCaseHexChars => LowerCaseHexCharsInString.AsSpan();
#else
  private static readonly char[] UpperCaseHexChars = UpperCaseHexCharsInString.ToCharArray();
  private static readonly char[] LowerCaseHexChars = LowerCaseHexCharsInString.ToCharArray();
#endif
}
