// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class DateTimeFormat {
#pragma warning restore IDE0040
  public static string ToISO8601DateTimeString(DateTime dateTime)
    => dateTime.ToString("o");

  public static string ToISO8601DateTimeString(DateTimeOffset dateTimeOffset)
    => dateTimeOffset.ToString("o");

  public static DateTime FromISO8601DateTimeString(string s)
    => FromDateTimeString(s, w3cDateTimeFormats, ISO8601UniversalTimeString);

  public static DateTimeOffset FromISO8601DateTimeOffsetString(string s)
    => FromDateTimeOffsetString(s, w3cDateTimeFormats, ISO8601UniversalTimeString);

  private const string ISO8601UniversalTimeString = "Z";
  // private static readonly string[] iso8601DateTimeFormats = w3cDateTimeFormats;
}
