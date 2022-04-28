// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Smdn.Formats.DateAndTime;

internal static class W3CDateTimeFormats {
  public static string ToString(DateTime dateTime)
    => dateTime.ToString("o");

  public static string ToString(DateTimeOffset dateTimeOffset)
    => dateTimeOffset.ToString("o");

  public static DateTime ParseDateTime(string s)
    => DateAndTimeParser.ParseDateTime(s, FormatStrings, timeZoneDefinitions);

  public static DateTimeOffset ParseDateTimeOffset(string s)
    => DateAndTimeParser.ParseDateTimeOffset(s, FormatStrings, timeZoneDefinitions);

  private static readonly IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions = new[] {
    new UniversalTimeZoneDefinition("Z"),
  };

  internal static readonly string[] FormatStrings = new string[]
  {
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
}
