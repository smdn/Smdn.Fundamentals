// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Smdn.Formats.DateAndTime;

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

  private const DateTimeStyles WhiteSpaceStyles = DateTimeStyles.AllowWhiteSpaces;

  private static DateTime FromDateTimeString(
    string s,
    string[] formats,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions
  )
  {
    if (s is null)
      throw new ArgumentNullException(nameof(s));

    s = ProcessTimeZoneSpecifier(
      s,
      timeZoneDefinitions,
      out var timeZoneStyles,
      out var tz
    );

    var dateAndTime = DateTime.ParseExact(
      s,
      formats,
      CultureInfo.InvariantCulture,
      timeZoneStyles | WhiteSpaceStyles
    );

    return tz is null || tz.IsUniversal
      ? dateAndTime
      : tz.AdjustToTimeZone(dateAndTime);
  }

  private static DateTimeOffset FromDateTimeOffsetString(
    string s,
    string[] formats,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions
  )
  {
    if (s is null)
      throw new ArgumentNullException(nameof(s));

    s = ProcessTimeZoneSpecifier(
      s,
      timeZoneDefinitions,
      out var timeZoneStyles,
      out var tz
    );

    var dateAndTime = DateTimeOffset.ParseExact(
      s,
      formats,
      CultureInfo.InvariantCulture,
      timeZoneStyles | WhiteSpaceStyles
    );

    return tz is null || tz.IsUniversal
      ? dateAndTime
      : tz.AdjustToTimeZone(dateAndTime);
  }

  private static string ProcessTimeZoneSpecifier(
    string s,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions,
    out DateTimeStyles dateTimeStylesOfTimeZone,
    out TimeZoneDefinition? timeZone
  )
  {
    dateTimeStylesOfTimeZone = DateTimeStyles.AssumeLocal;

    timeZone = timeZoneDefinitions.FirstOrDefault(
      tz => s.EndsWith(tz.Suffix, StringComparison.Ordinal)
    );

    if (timeZone is null)
      return s;

    dateTimeStylesOfTimeZone = timeZone.IsUniversal
      ? DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
      : DateTimeStyles.RoundtripKind;

    return s.Substring(0, s.Length - timeZone.Suffix.Length);
  }
}
