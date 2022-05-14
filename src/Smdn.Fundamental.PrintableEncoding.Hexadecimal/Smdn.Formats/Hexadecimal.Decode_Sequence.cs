// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class Hexadecimal {
#pragma warning restore IDE0040
#if SYSTEM_READONLYSPAN
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
#endif
}
