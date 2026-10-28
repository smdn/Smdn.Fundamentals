// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using Smdn.Formats.DateAndTime;

namespace Smdn.Formats;

partial class DateTimeFormat {
  public static string ToISO8601DateTimeString(DateTime dateTime)
    => ISO8601DateTimeFormats.ToString(dateTime);

  public static string ToISO8601DateTimeString(DateTimeOffset dateTimeOffset)
    => ISO8601DateTimeFormats.ToString(dateTimeOffset);

  public static DateTime FromISO8601DateTimeString(string s)
    => ISO8601DateTimeFormats.ParseDateTime(s);

  public static DateTimeOffset FromISO8601DateTimeOffsetString(string s)
    => ISO8601DateTimeFormats.ParseDateTimeOffset(s);
}
