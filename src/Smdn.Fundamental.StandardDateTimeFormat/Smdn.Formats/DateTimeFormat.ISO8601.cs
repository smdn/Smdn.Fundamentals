// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class DateTimeFormat {
#pragma warning restore IDE0040
  public static string ToISO8601DateTimeString(DateTime dateTime)
    => dateTime.ToString("o");

  public static string ToISO8601DateTimeString(DateTimeOffset dateTimeOffset)
    => dateTimeOffset.ToString("o");

  public static DateTime FromISO8601DateTimeString(string s)
    => FromDateTimeString(s, w3cDateTimeFormats, ISO8601UniversalTimeStrings);

  public static DateTimeOffset FromISO8601DateTimeOffsetString(string s)
    => FromDateTimeOffsetString(s, w3cDateTimeFormats, ISO8601UniversalTimeStrings);

  private static readonly IReadOnlyList<string> ISO8601UniversalTimeStrings = new[] { "Z" };
}
