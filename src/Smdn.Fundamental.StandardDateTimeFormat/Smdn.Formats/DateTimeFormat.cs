// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Globalization;

namespace Smdn.Formats;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static partial class DateTimeFormat {
  public static string GetCurrentTimeZoneOffsetString(bool delimiter)
  {
    var offset = TimeZoneInfo.Local.BaseUtcOffset;
    var sign = TimeSpan.Zero <= offset ? "+" : "-";
    var delim = delimiter ? ":" : string.Empty;

#if SYSTEM_FORMATTABLESTRING
    return FormattableString.Invariant($"{sign}{offset.Hours:d2}{delim}{offset.Minutes:d2}");
#else
    return string.Concat(sign, offset.Hours.ToString("d2", CultureInfo.InvariantCulture), delim, offset.Minutes.ToString("d2", CultureInfo.InvariantCulture));
#endif
  }
}
