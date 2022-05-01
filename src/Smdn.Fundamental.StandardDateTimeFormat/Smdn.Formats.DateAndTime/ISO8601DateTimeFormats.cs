// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_GLOBALIZATION_ISOWEEK
#endif
#if NET6_0_OR_GREATER
#define SYSTEM_DATEONLY
#endif

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Smdn.Formats.DateAndTime;

public static class ISO8601DateTimeFormats {
  internal static string ToString(DateTime dateTime)
    => dateTime.ToString("o");

  internal static string ToString(DateTimeOffset dateTimeOffset)
    => dateTimeOffset.ToString("o");

  internal static DateTime ParseDateTime(string s)
    => DateAndTimeParser.ParseDateTime(s, W3CDateTimeFormats.FormatStrings, timeZoneDefinitions);

  internal static DateTimeOffset ParseDateTimeOffset(string s)
    => DateAndTimeParser.ParseDateTimeOffset(s, W3CDateTimeFormats.FormatStrings, timeZoneDefinitions);

  private static readonly IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions = new[] {
    new UniversalTimeZoneDefinition("Z"),
  };

#if SYSTEM_GLOBALIZATION_ISOWEEK
#if SYSTEM_DATEONLY
  public static string ToWeekDateString(DateOnly date)
    => ToWeekDateString(date.ToDateTime(default(TimeOnly), DateTimeKind.Unspecified));
#endif

  public static string ToWeekDateString(DateTimeOffset date)
    => ToWeekDateString(date.Date);

  public static string ToWeekDateString(DateTime date)
    => string.Format(
      CultureInfo.InvariantCulture.NumberFormat,
      "{0:D4}-W{1:D2}-{2:D2}",
      ISOWeek.GetYear(date),
      ISOWeek.GetWeekOfYear(date),
      date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)date.DayOfWeek
    );
#endif
}
