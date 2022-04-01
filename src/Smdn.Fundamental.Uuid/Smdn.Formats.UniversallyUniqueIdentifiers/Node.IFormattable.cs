// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

#pragma warning disable IDE0040
partial struct Node :
#pragma warning restore IDE0040
  IFormattable
#if SYSTEM_ISPANFORMATTABLE
#pragma warning disable SA1001
  ,
#pragma warning restore SA1001
  ISpanFormattable
#endif
{
  public string ToString(string? format, IFormatProvider? formatProvider = null)
  {
    if (string.IsNullOrEmpty(format))
      format = "X"; // as default

    return format switch {
      "X" => $"{N0:X2}:{N1:X2}:{N2:X2}:{N3:X2}:{N4:X2}:{N5:X2}",
      "x" => $"{N0:x2}:{N1:x2}:{N2:x2}:{N3:x2}:{N4:x2}:{N5:x2}",
      _ => throw new FormatException($"invalid format: {format}"),
    };
  }

#if SYSTEM_ISPANFORMATTABLE
  public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
  {
    charsWritten = 0;

    var lowerCase = 1 == format.Length && format[0] == 'x';

    if (destination.Length < 17 /* "XX:XX:XX:XX:XX:XX".Length */)
      return false;

    var fmt = lowerCase ? "x2" : "X2";

    N0.TryFormat(destination, out _, fmt, provider: null); destination[2] = ':';
    N1.TryFormat(destination.Slice(3), out _, fmt, provider: null); destination[5] = ':';
    N2.TryFormat(destination.Slice(6), out _, fmt, provider: null); destination[8] = ':';
    N3.TryFormat(destination.Slice(9), out _, fmt, provider: null); destination[11] = ':';
    N4.TryFormat(destination.Slice(12), out _, fmt, provider: null); destination[14] = ':';
    N5.TryFormat(destination.Slice(15), out _, fmt, provider: null);

    charsWritten = 17;

    return true;
  }
#endif
}
