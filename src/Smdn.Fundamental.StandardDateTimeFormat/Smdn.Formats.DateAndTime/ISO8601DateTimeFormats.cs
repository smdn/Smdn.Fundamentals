// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_GLOBALIZATION_ISOWEEK
#endif
#if NET6_0_OR_GREATER
#define SYSTEM_DATEONLY
#endif
#if NET45_OR_GREATER || NETSTANDARD1_1_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_READONLYSPAN
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

  public static DateTime ParseDateTime(string s)
    => DateAndTimeParser.ParseDateTime(
      s,
      W3CDateTimeFormats.DateAndTimeFormatStrings,
      W3CDateTimeFormats.DateOnlyFormatStrings,
      timeZoneDefinitions
    );

#if SYSTEM_READONLYSPAN
  public static DateTime ParseDateTime(ReadOnlySpan<char> s)
    => DateAndTimeParser.ParseDateTime(
      s,
      W3CDateTimeFormats.DateAndTimeFormatStrings,
      W3CDateTimeFormats.DateOnlyFormatStrings,
      timeZoneDefinitions
    );
#endif

  public static bool TryParseDateTime(string? s, out DateTime result)
    => DateAndTimeParser.TryParseDateTime(
      s,
      W3CDateTimeFormats.DateAndTimeFormatStrings,
      W3CDateTimeFormats.DateOnlyFormatStrings,
      timeZoneDefinitions,
      out result
    );

#if SYSTEM_READONLYSPAN
  public static bool TryParseDateTime(ReadOnlySpan<char> s, out DateTime result)
    => DateAndTimeParser.TryParseDateTime(
      s,
      W3CDateTimeFormats.DateAndTimeFormatStrings,
      W3CDateTimeFormats.DateOnlyFormatStrings,
      timeZoneDefinitions,
      out result
    );
#endif

  public static DateTimeOffset ParseDateTimeOffset(string s)
    => DateAndTimeParser.ParseDateTimeOffset(
      s,
      W3CDateTimeFormats.DateAndTimeFormatStrings,
      W3CDateTimeFormats.DateOnlyFormatStrings,
      timeZoneDefinitions
    );

#if SYSTEM_READONLYSPAN
  public static DateTimeOffset ParseDateTimeOffset(ReadOnlySpan<char> s)
    => DateAndTimeParser.ParseDateTimeOffset(
      s,
      W3CDateTimeFormats.DateAndTimeFormatStrings,
      W3CDateTimeFormats.DateOnlyFormatStrings,
      timeZoneDefinitions
    );
#endif

  public static bool TryParseDateTimeOffset(string? s, out DateTimeOffset result)
    => DateAndTimeParser.TryParseDateTimeOffset(
      s,
      W3CDateTimeFormats.DateAndTimeFormatStrings,
      W3CDateTimeFormats.DateOnlyFormatStrings,
      timeZoneDefinitions,
      out result
    );

#if SYSTEM_READONLYSPAN
  public static bool TryParseDateTimeOffset(ReadOnlySpan<char> s, out DateTimeOffset result)
    => DateAndTimeParser.TryParseDateTimeOffset(
      s,
      W3CDateTimeFormats.DateAndTimeFormatStrings,
      W3CDateTimeFormats.DateOnlyFormatStrings,
      timeZoneDefinitions,
      out result
    );
#endif

  private static readonly IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions = new[] {
    new UniversalTimeZoneDefinition("Z"),
  };

#if SYSTEM_GLOBALIZATION_ISOWEEK
#if SYSTEM_DATEONLY
  public static string ToWeekDateString(DateOnly date)
    => ToWeekDateString(date.ToDateTime(time: default, DateTimeKind.Unspecified));
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
