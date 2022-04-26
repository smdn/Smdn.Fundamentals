// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class DateTimeFormat {
#pragma warning restore IDE0040
  public static string ToISO8601DateTimeString(DateTime dateTime)
    => ToW3CDateTimeString(dateTime);

  public static string ToISO8601DateTimeString(DateTimeOffset dateTimeOffset)
    => ToW3CDateTimeString(dateTimeOffset);

  public static DateTime FromISO8601DateTimeString(string s)
    => FromW3CDateTimeString(s);

  public static DateTimeOffset FromISO8601DateTimeOffsetString(string s)
    => FromW3CDateTimeOffsetString(s);
}
