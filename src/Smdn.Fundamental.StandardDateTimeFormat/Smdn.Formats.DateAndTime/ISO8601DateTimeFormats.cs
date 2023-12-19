// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#if SYSTEM_GLOBALIZATION_ISOWEEK
using System.Globalization;
#endif

namespace Smdn.Formats.DateAndTime;

public static class ISO8601DateTimeFormats {
  internal static string ToString(DateTime dateTime)
    => dateTime.ToString("o");

  internal static string ToString(DateTimeOffset dateTimeOffset)
    => dateTimeOffset.ToString("o");

  public static DateTime ParseDateTime(string s)
    => DateAndTimeParser.ParseDateTime(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      TimeZoneDefinitions
    );

#if SYSTEM_READONLYSPAN
  public static DateTime ParseDateTime(ReadOnlySpan<char> s)
    => DateAndTimeParser.ParseDateTime(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      TimeZoneDefinitions
    );
#endif

  public static bool TryParseDateTime(string? s, out DateTime result)
    => DateAndTimeParser.TryParseDateTime(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      TimeZoneDefinitions,
      out result
    );

#if SYSTEM_READONLYSPAN
  public static bool TryParseDateTime(ReadOnlySpan<char> s, out DateTime result)
    => DateAndTimeParser.TryParseDateTime(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      TimeZoneDefinitions,
      out result
    );
#endif

  public static DateTimeOffset ParseDateTimeOffset(string s)
    => DateAndTimeParser.ParseDateTimeOffset(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      TimeZoneDefinitions
    );

#if SYSTEM_READONLYSPAN
  public static DateTimeOffset ParseDateTimeOffset(ReadOnlySpan<char> s)
    => DateAndTimeParser.ParseDateTimeOffset(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      TimeZoneDefinitions
    );
#endif

  public static bool TryParseDateTimeOffset(string? s, out DateTimeOffset result)
    => DateAndTimeParser.TryParseDateTimeOffset(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      TimeZoneDefinitions,
      out result
    );

#if SYSTEM_READONLYSPAN
  public static bool TryParseDateTimeOffset(ReadOnlySpan<char> s, out DateTimeOffset result)
    => DateAndTimeParser.TryParseDateTimeOffset(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      TimeZoneDefinitions,
      out result
    );
#endif

  internal static readonly IReadOnlyList<TimeZoneDefinition> TimeZoneDefinitions = new[] {
    new UniversalTimeZoneDefinition("Z"),
  };

  internal static readonly string[] DateAndTimeFormatStrings = new string[] {
    "o",
    // "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffzzz", // is coverd by format string 'o'
    // "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff", // is coverd by format string 'o'
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffff",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffff",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f",
    "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
    "yyyy'-'MM'-'dd'T'HH':'mmzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm",
    "yyyy'-'MM'-'dd'T'HHzzz",
    "yyyy'-'MM'-'dd'T'HH",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffffffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffffff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'f",
    "yyyy'-'MM'-'dd' 'HH':'mm':'sszzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss",
    "yyyy'-'MM'-'dd' 'HH':'mmzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm",
    "yyyy'-'MM'-'dd' 'HHzzz",
    "yyyy'-'MM'-'dd' 'HH",
  };

  internal static readonly string[] DateOnlyFormatStrings = new string[] {
    // date-only formats
    "yyyy'-'MM'-'dd",
    "yyyy'-'MM",
    "yyyy",
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
