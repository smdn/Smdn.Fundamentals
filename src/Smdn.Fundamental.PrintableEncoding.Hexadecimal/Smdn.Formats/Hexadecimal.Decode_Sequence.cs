// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class Hexadecimal {
#pragma warning restore IDE0040
  public static bool TryDecodeUpperCase(ArraySegment<char> upperCaseDataSequence, ArraySegment<byte> destination, out int decodedLength)
#if SYSTEM_READONLYSPAN
    => TryDecode(upperCaseDataSequence.AsSpan(), destination.AsSpan(), allowUpperCase: true, allowLowerCase: false, out decodedLength);
#else
    => TryDecode(upperCaseDataSequence, destination, allowUpperCase: true, allowLowerCase: false, out decodedLength);
#endif

  public static bool TryDecodeLowerCase(ArraySegment<char> lowerCaseDataSequence, ArraySegment<byte> destination, out int decodedLength)
#if SYSTEM_READONLYSPAN
    => TryDecode(lowerCaseDataSequence.AsSpan(), destination.AsSpan(), allowUpperCase: false, allowLowerCase: true, out decodedLength);
#else
    => TryDecode(lowerCaseDataSequence, destination, allowUpperCase: false, allowLowerCase: true, out decodedLength);
#endif

  public static bool TryDecode(ArraySegment<char> dataSequence, ArraySegment<byte> destination, out int decodedLength)
#if SYSTEM_READONLYSPAN
    => TryDecode(dataSequence.AsSpan(), destination.AsSpan(), allowUpperCase: true, allowLowerCase: true, out decodedLength);
#else
    => TryDecode(dataSequence, destination, allowUpperCase: true, allowLowerCase: true, out decodedLength);
#endif

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
      if (!TryDecode(dataSequence, allowUpperCase, allowLowerCase, out byte decodedData))
        return false;

      destination[i] = decodedData;

      dataSequence = dataSequence.Slice(2);
      decodedLength++;
    }

    return true;
  }
#else
  internal static bool TryDecode(ArraySegment<char> dataSequence, ArraySegment<byte> destination, bool allowUpperCase, bool allowLowerCase, out int decodedLength)
  {
    decodedLength = 0;

    var length = dataSequence.Count;

    if (length == 0)
      return true;

    if ((length & 0x1) != 0)
      return false; // incorrect form

    length >>= 1;

    if (destination.Count < length)
      return false; // destination too short

    for (var i = 0; i < length; i++) {
      if (!TryDecode(dataSequence, allowUpperCase, allowLowerCase, out byte decodedData))
        return false;

      destination.Array[destination.Offset + i] = decodedData;

      dataSequence = new(dataSequence.Array, dataSequence.Offset + 2, dataSequence.Count - 2);
      decodedLength++;
    }

    return true;
  }
#endif
}
