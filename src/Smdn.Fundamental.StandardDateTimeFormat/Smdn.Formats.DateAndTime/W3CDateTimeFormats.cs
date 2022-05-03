// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET45_OR_GREATER || NETSTANDARD1_1_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_READONLYSPAN
#endif

using System;
using System.Collections.Generic;

namespace Smdn.Formats.DateAndTime;

public static class W3CDateTimeFormats {
  internal static string ToString(DateTime dateTime)
    => dateTime.ToString("o");

  internal static string ToString(DateTimeOffset dateTimeOffset)
    => dateTimeOffset.ToString("o");

  public static DateTime ParseDateTime(string s)
    => DateAndTimeParser.ParseDateTime(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      timeZoneDefinitions
    );

#if SYSTEM_READONLYSPAN
  public static DateTime ParseDateTime(ReadOnlySpan<char> s)
    => DateAndTimeParser.ParseDateTime(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      timeZoneDefinitions
    );
#endif

  public static bool TryParseDateTime(string? s, out DateTime result)
    => DateAndTimeParser.TryParseDateTime(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      timeZoneDefinitions,
      out result
    );

#if SYSTEM_READONLYSPAN
  public static bool TryParseDateTime(ReadOnlySpan<char> s, out DateTime result)
    => DateAndTimeParser.TryParseDateTime(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      timeZoneDefinitions,
      out result
    );
#endif

  public static DateTimeOffset ParseDateTimeOffset(string s)
    => DateAndTimeParser.ParseDateTimeOffset(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      timeZoneDefinitions
    );

#if SYSTEM_READONLYSPAN
  public static DateTimeOffset ParseDateTimeOffset(ReadOnlySpan<char> s)
    => DateAndTimeParser.ParseDateTimeOffset(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      timeZoneDefinitions
    );
#endif

  public static bool TryParseDateTimeOffset(string? s, out DateTimeOffset result)
    => DateAndTimeParser.TryParseDateTimeOffset(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      timeZoneDefinitions,
      out result
    );

#if SYSTEM_READONLYSPAN
  public static bool TryParseDateTimeOffset(ReadOnlySpan<char> s, out DateTimeOffset result)
    => DateAndTimeParser.TryParseDateTimeOffset(
      s,
      DateAndTimeFormatStrings,
      DateOnlyFormatStrings,
      timeZoneDefinitions,
      out result
    );
#endif

  private static readonly IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions = new[] {
    new UniversalTimeZoneDefinition("Z"),
  };

  internal static readonly string[] DateAndTimeFormatStrings = new string[] {
    "u",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff",
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
}
