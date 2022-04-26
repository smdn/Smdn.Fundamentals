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

  private static DateTime FromDateTimeString(string s, string[] formats, string universalTimeString)
  {
    if (s is null)
      throw new ArgumentNullException(nameof(s));

    var universal = s.EndsWith(universalTimeString, StringComparison.Ordinal);
    var styles = DateTimeStyles.AllowWhiteSpaces;

    if (universal)
      styles |= DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;
    else
      // TODO: JST, EST, etc; use TimeZoneInfo
      styles |= DateTimeStyles.AssumeLocal;

    return DateTime.ParseExact(s, formats, CultureInfo.InvariantCulture, styles);
  }

  private static DateTimeOffset FromDateTimeOffsetString(string s, string[] formats, string universalTimeString)
  {
    if (s is null)
      throw new ArgumentNullException(nameof(s));

    var universal = s.EndsWith(universalTimeString, StringComparison.Ordinal);
    var styles = DateTimeStyles.AllowWhiteSpaces;

    if (universal)
      styles |= DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;
    else
      // TODO: JST, EST, etc; use TimeZoneInfo
      styles |= DateTimeStyles.AssumeLocal;

    return DateTimeOffset.ParseExact(s, formats, CultureInfo.InvariantCulture, styles);
  }
}
