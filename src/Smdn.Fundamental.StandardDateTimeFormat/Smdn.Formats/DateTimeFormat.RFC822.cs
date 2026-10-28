// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

using Smdn.Formats.DateAndTime;

namespace Smdn.Formats;

partial class DateTimeFormat {
  public static string ToRFC822DateTimeString(DateTime dateTime)
    => RFC822DateTimeFormats.ToString(dateTime);

  public static string ToRFC822DateTimeString(DateTimeOffset dateTimeOffset)
    => RFC822DateTimeFormats.ToString(dateTimeOffset);

  public static string? ToRFC822DateTimeStringNullable(DateTimeOffset? dateTimeOffset)
    => dateTimeOffset.HasValue
      ? RFC822DateTimeFormats.ToString(dateTimeOffset.Value)
      : null;

  public static DateTime FromRFC822DateTimeString(string s)
    => RFC822DateTimeFormats.ParseDateTime(s);

  public static DateTimeOffset FromRFC822DateTimeOffsetString(string s)
    => RFC822DateTimeFormats.ParseDateTimeOffset(s);

  public static DateTimeOffset? FromRFC822DateTimeOffsetStringNullable(string? s)
    => s is null
      ? null
      : RFC822DateTimeFormats.ParseDateTimeOffset(s);
}
