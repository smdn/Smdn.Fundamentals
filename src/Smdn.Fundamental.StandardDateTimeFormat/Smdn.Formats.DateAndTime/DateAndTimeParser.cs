// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Smdn.Formats.DateAndTime;

internal static class DateAndTimeParser {
  private const DateTimeStyles WhiteSpaceStyles = DateTimeStyles.AllowWhiteSpaces;

  internal static DateTime ParseDateTime(
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

    var timeZoneStyles = DateTimeStyles.AssumeLocal;

    s = ProcessTimeZoneSpecifier(
      s,
      timeZoneDefinitions,
      ref timeZoneStyles,
      out var tz
    );

    var dateAndTime = DateTime.ParseExact(
      s,
      formats,
      CultureInfo.InvariantCulture.DateTimeFormat,
      timeZoneStyles | WhiteSpaceStyles
    );

    return tz is null || tz.IsUniversal
      ? dateAndTime
      : tz.AdjustToTimeZone(dateAndTime);
  }

  internal static DateTimeOffset ParseDateTimeOffset(
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

    var timeZoneStyles = DateTimeStyles.AssumeUniversal;

    s = ProcessTimeZoneSpecifier(
      s,
      timeZoneDefinitions,
      ref timeZoneStyles,
      out var tz
    );

    var dateAndTime = DateTimeOffset.ParseExact(
      s,
      formats,
      CultureInfo.InvariantCulture.DateTimeFormat,
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
    ref DateTimeStyles dateTimeStylesOfTimeZone,
    out TimeZoneDefinition? timeZone
  )
  {
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
