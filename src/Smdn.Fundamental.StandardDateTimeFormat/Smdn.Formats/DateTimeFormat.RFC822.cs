// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Globalization;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class DateTimeFormat {
#pragma warning restore IDE0040
  public static string ToRFC822DateTimeString(DateTime dateTime)
  {
    var str = dateTime.ToString("ddd, d MMM yyyy HH:mm:ss ", CultureInfo.InvariantCulture);

    if (dateTime.Kind == DateTimeKind.Utc)
      return str + "GMT";
    else
      return str + GetCurrentTimeZoneOffsetString(false);
  }

  public static string ToRFC822DateTimeString(DateTimeOffset dateTimeOffset)
  {
    return dateTimeOffset.ToString("ddd, d MMM yyyy HH:mm:ss ", CultureInfo.InvariantCulture) +
      dateTimeOffset.ToString("zzz", CultureInfo.InvariantCulture).Replace(":", string.Empty);
  }

  public static string ToRFC822DateTimeStringNullable(DateTimeOffset? dateTimeOffset)
    => dateTimeOffset.HasValue
      ? ToRFC822DateTimeString(dateTimeOffset.Value)
      : null;

  public static DateTime FromRFC822DateTimeString(string s)
    => FromDateTimeString(s, rfc822DateTimeFormats, RFC822UniversalTimeString);

  public static DateTimeOffset FromRFC822DateTimeOffsetString(string s)
    => FromDateTimeOffsetString(s, rfc822DateTimeFormats, RFC822UniversalTimeString);

  public static DateTimeOffset? FromRFC822DateTimeOffsetStringNullable(string s)
    => s is null
      ? null
      : FromRFC822DateTimeOffsetString(s);

  private const string RFC822UniversalTimeString = " GMT";

  private static readonly string[] rfc822DateTimeFormats = new[]
  {
    // TODO: f1-f6
    "r",
    "ddd, d MMM yyyy HH:mm:ss.fffffff zzz",
    "ddd, d MMM yyyy HH:mm:ss.fffffff GMT",
    "ddd, d MMM yyyy HH:mm:ss.fffffff",
    "ddd, d MMM yyyy HH:mm:ss.fff zzz",
    "ddd, d MMM yyyy HH:mm:ss.fff GMT",
    "ddd, d MMM yyyy HH:mm:ss.fff",
    "ddd, d MMM yyyy HH:mm:ss zzz",
    "ddd, d MMM yyyy HH:mm:ss GMT",
    "ddd, d MMM yyyy HH:mm:ss",
    "ddd, d MMM yyyy HH:mm zzz",
    "ddd, d MMM yyyy HH:mm GMT",
    "ddd, d MMM yyyy HH:mm",
    "d MMM yyyy HH:mm:ss.fffffff zzz",
    "d MMM yyyy HH:mm:ss.fffffff GMT",
    "d MMM yyyy HH:mm:ss.fffffff",
    "d MMM yyyy HH:mm:ss.fff zzz",
    "d MMM yyyy HH:mm:ss.fff GMT",
    "d MMM yyyy HH:mm:ss.fff",
    "d MMM yyyy HH:mm:ss zzz",
    "d MMM yyyy HH:mm:ss GMT",
    "d MMM yyyy HH:mm:ss",
    "d MMM yyyy HH:mm zzz",
    "d MMM yyyy HH:mm GMT",
    "d MMM yyyy HH:mm",
  };
}
