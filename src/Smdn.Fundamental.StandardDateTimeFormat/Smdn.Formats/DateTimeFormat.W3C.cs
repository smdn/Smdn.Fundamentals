// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using Smdn.Formats.DateAndTime;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class DateTimeFormat {
#pragma warning restore IDE0040
  public static string ToW3CDateTimeString(DateTime dateTime)
    => W3CDateTimeFormats.ToString(dateTime);

  public static string ToW3CDateTimeString(DateTimeOffset dateTimeOffset)
    => W3CDateTimeFormats.ToString(dateTimeOffset);

  public static string? ToW3CDateTimeStringNullable(DateTimeOffset? dateTimeOffset)
    => dateTimeOffset.HasValue
      ? W3CDateTimeFormats.ToString(dateTimeOffset.Value)
      : null;

  public static DateTime FromW3CDateTimeString(string s)
    => W3CDateTimeFormats.ParseDateTime(s);

  public static DateTimeOffset FromW3CDateTimeOffsetString(string s)
    => W3CDateTimeFormats.ParseDateTimeOffset(s);

  public static DateTimeOffset? FromW3CDateTimeOffsetStringNullable(string s)
    => s is null
      ? null
      : W3CDateTimeFormats.ParseDateTimeOffset(s);
}
