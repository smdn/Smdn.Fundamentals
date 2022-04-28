// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Smdn.Formats.DateAndTime;

internal static class ISO8601DateTimeFormats {
  public static string ToString(DateTime dateTime)
    => dateTime.ToString("o");

  public static string ToString(DateTimeOffset dateTimeOffset)
    => dateTimeOffset.ToString("o");

  public static DateTime ParseDateTime(string s)
    => DateAndTimeParser.ParseDateTime(s, W3CDateTimeFormats.FormatStrings, timeZoneDefinitions);

  public static DateTimeOffset ParseDateTimeOffset(string s)
    => DateAndTimeParser.ParseDateTimeOffset(s, W3CDateTimeFormats.FormatStrings, timeZoneDefinitions);

  private static readonly IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions = new[] {
    new UniversalTimeZoneDefinition("Z"),
  };
}
