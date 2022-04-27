// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

  private static DateTime FromDateTimeString(
    string s,
    string[] formats,
    IReadOnlyList<string> universalTimeStrings
  )
  {
    if (s is null)
      throw new ArgumentNullException(nameof(s));

    var styles = DateTimeStyles.AllowWhiteSpaces;

    if (TryRemoveUniversalTimeSuffix(s, universalTimeStrings, out var str)) {
      styles |= DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;
      s = str;
    }
    else {
      // TODO: JST, EST, etc; use TimeZoneInfo
      styles |= DateTimeStyles.AssumeLocal;
    }

    return DateTime.ParseExact(s, formats, CultureInfo.InvariantCulture, styles);
  }

  private static DateTimeOffset FromDateTimeOffsetString(
    string s,
    string[] formats,
    IReadOnlyList<string> universalTimeStrings
  )
  {
    if (s is null)
      throw new ArgumentNullException(nameof(s));

    var styles = DateTimeStyles.AllowWhiteSpaces;

    if (TryRemoveUniversalTimeSuffix(s, universalTimeStrings, out var str)) {
      styles |= DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;
      s = str;
    }
    else {
      // TODO: JST, EST, etc; use TimeZoneInfo
      styles |= DateTimeStyles.AssumeLocal;
    }

    return DateTimeOffset.ParseExact(s, formats, CultureInfo.InvariantCulture, styles);
  }

  private static bool TryRemoveUniversalTimeSuffix(
    string s,
    IReadOnlyList<string> universalTimeStrings,
    out string stringWithoutUniversalTimeSuffix
  )
  {
    stringWithoutUniversalTimeSuffix = default;

    var universalTimeSuffix = universalTimeStrings.FirstOrDefault(
      ut => s.EndsWith(ut, StringComparison.Ordinal)
    );

    if (universalTimeSuffix is not null) {
      stringWithoutUniversalTimeSuffix = s.Substring(0, s.Length - universalTimeSuffix.Length);
      return true;
    }

    return false;
  }
}
