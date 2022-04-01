// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Globalization;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

#pragma warning disable IDE0040
partial struct Node {
#pragma warning restore IDE0040
  public static Node Parse(string s)
    => TryParse(s ?? throw new ArgumentNullException(nameof(s)), out var result)
      ? result
      : throw new FormatException("invalid format");

  public static bool TryParse(string s, out Node result)
  {
    result = default;

    if (s is null)
      return false;

    var p =
#if SYSTEM_STRING_SPLIT_CHAR
      s.Split(':', StringSplitOptions.None);
#else
      s.Split(new[] { ':' }, StringSplitOptions.None);
#endif

    if (p.Length != SizeOfSelf)
      return false;
    if (!byte.TryParse(p[0], NumberStyles.HexNumber, provider: null, out var n0))
      return false;
    if (!byte.TryParse(p[1], NumberStyles.HexNumber, provider: null, out var n1))
      return false;
    if (!byte.TryParse(p[2], NumberStyles.HexNumber, provider: null, out var n2))
      return false;
    if (!byte.TryParse(p[3], NumberStyles.HexNumber, provider: null, out var n3))
      return false;
    if (!byte.TryParse(p[4], NumberStyles.HexNumber, provider: null, out var n4))
      return false;
    if (!byte.TryParse(p[5], NumberStyles.HexNumber, provider: null, out var n5))
      return false;

    result = new(n0, n1, n2, n3, n4, n5);

    return true;
  }
}
