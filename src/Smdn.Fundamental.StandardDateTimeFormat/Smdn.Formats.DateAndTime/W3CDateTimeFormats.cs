// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats.DateAndTime;

public static class W3CDateTimeFormats {
  internal static string ToString(DateTime dateTime)
    => dateTime.ToString("o");

  internal static string ToString(DateTimeOffset dateTimeOffset)
    => dateTimeOffset.ToString("o");

  public static DateTime ParseDateTime(string s)
    => DateAndTimeParser.ParseDateTime(
      s,
      ISO8601DateTimeFormats.DateAndTimeFormatStrings,
      ISO8601DateTimeFormats.DateOnlyFormatStrings,
      ISO8601DateTimeFormats.TimeZoneDefinitions
    );

#if SYSTEM_READONLYSPAN
  public static DateTime ParseDateTime(ReadOnlySpan<char> s)
    => DateAndTimeParser.ParseDateTime(
      s,
      ISO8601DateTimeFormats.DateAndTimeFormatStrings,
      ISO8601DateTimeFormats.DateOnlyFormatStrings,
      ISO8601DateTimeFormats.TimeZoneDefinitions
    );
#endif

  public static bool TryParseDateTime(string? s, out DateTime result)
    => DateAndTimeParser.TryParseDateTime(
      s,
      ISO8601DateTimeFormats.DateAndTimeFormatStrings,
      ISO8601DateTimeFormats.DateOnlyFormatStrings,
      ISO8601DateTimeFormats.TimeZoneDefinitions,
      out result
    );

#if SYSTEM_READONLYSPAN
  public static bool TryParseDateTime(ReadOnlySpan<char> s, out DateTime result)
    => DateAndTimeParser.TryParseDateTime(
      s,
      ISO8601DateTimeFormats.DateAndTimeFormatStrings,
      ISO8601DateTimeFormats.DateOnlyFormatStrings,
      ISO8601DateTimeFormats.TimeZoneDefinitions,
      out result
    );
#endif

  public static DateTimeOffset ParseDateTimeOffset(string s)
    => DateAndTimeParser.ParseDateTimeOffset(
      s,
      ISO8601DateTimeFormats.DateAndTimeFormatStrings,
      ISO8601DateTimeFormats.DateOnlyFormatStrings,
      ISO8601DateTimeFormats.TimeZoneDefinitions
    );

#if SYSTEM_READONLYSPAN
  public static DateTimeOffset ParseDateTimeOffset(ReadOnlySpan<char> s)
    => DateAndTimeParser.ParseDateTimeOffset(
      s,
      ISO8601DateTimeFormats.DateAndTimeFormatStrings,
      ISO8601DateTimeFormats.DateOnlyFormatStrings,
      ISO8601DateTimeFormats.TimeZoneDefinitions
    );
#endif

  public static bool TryParseDateTimeOffset(string? s, out DateTimeOffset result)
    => DateAndTimeParser.TryParseDateTimeOffset(
      s,
      ISO8601DateTimeFormats.DateAndTimeFormatStrings,
      ISO8601DateTimeFormats.DateOnlyFormatStrings,
      ISO8601DateTimeFormats.TimeZoneDefinitions,
      out result
    );

#if SYSTEM_READONLYSPAN
  public static bool TryParseDateTimeOffset(ReadOnlySpan<char> s, out DateTimeOffset result)
    => DateAndTimeParser.TryParseDateTimeOffset(
      s,
      ISO8601DateTimeFormats.DateAndTimeFormatStrings,
      ISO8601DateTimeFormats.DateOnlyFormatStrings,
      ISO8601DateTimeFormats.TimeZoneDefinitions,
      out result
    );
#endif
}
