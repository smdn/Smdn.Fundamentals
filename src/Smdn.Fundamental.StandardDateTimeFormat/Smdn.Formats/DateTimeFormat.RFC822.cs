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
    "r",
    "ddd',' d MMM yyyy H':'m':'s'.'fffffff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'fffffff GMT",
    "ddd',' d MMM yyyy H':'m':'s'.'fffffff",
    "ddd',' d MMM yyyy H':'m':'s'.'ffffff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'ffffff GMT",
    "ddd',' d MMM yyyy H':'m':'s'.'ffffff",
    "ddd',' d MMM yyyy H':'m':'s'.'fffff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'fffff GMT",
    "ddd',' d MMM yyyy H':'m':'s'.'fffff",
    "ddd',' d MMM yyyy H':'m':'s'.'ffff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'ffff GMT",
    "ddd',' d MMM yyyy H':'m':'s'.'ffff",
    "ddd',' d MMM yyyy H':'m':'s'.'fff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'fff GMT",
    "ddd',' d MMM yyyy H':'m':'s'.'fff",
    "ddd',' d MMM yyyy H':'m':'s'.'ff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'ff GMT",
    "ddd',' d MMM yyyy H':'m':'s'.'ff",
    "ddd',' d MMM yyyy H':'m':'s'.'f zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'f GMT",
    "ddd',' d MMM yyyy H':'m':'s'.'f",
    "ddd',' d MMM yyyy H':'m':'s zzz",
    "ddd',' d MMM yyyy H':'m':'s GMT",
    "ddd',' d MMM yyyy H':'m':'s",
    "ddd',' d MMM yyyy H':'m zzz",
    "ddd',' d MMM yyyy H':'m GMT",
    "ddd',' d MMM yyyy H':'m",
    "d MMM yyyy H':'m':'s'.'fffffff zzz",
    "d MMM yyyy H':'m':'s'.'fffffff GMT",
    "d MMM yyyy H':'m':'s'.'fffffff",
    "d MMM yyyy H':'m':'s'.'ffffff zzz",
    "d MMM yyyy H':'m':'s'.'ffffff GMT",
    "d MMM yyyy H':'m':'s'.'ffffff",
    "d MMM yyyy H':'m':'s'.'fffff zzz",
    "d MMM yyyy H':'m':'s'.'fffff GMT",
    "d MMM yyyy H':'m':'s'.'fffff",
    "d MMM yyyy H':'m':'s'.'ffff zzz",
    "d MMM yyyy H':'m':'s'.'ffff GMT",
    "d MMM yyyy H':'m':'s'.'ffff",
    "d MMM yyyy H':'m':'s'.'fff zzz",
    "d MMM yyyy H':'m':'s'.'fff GMT",
    "d MMM yyyy H':'m':'s'.'fff",
    "d MMM yyyy H':'m':'s'.'ff zzz",
    "d MMM yyyy H':'m':'s'.'ff GMT",
    "d MMM yyyy H':'m':'s'.'ff",
    "d MMM yyyy H':'m':'s'.'f zzz",
    "d MMM yyyy H':'m':'s'.'f GMT",
    "d MMM yyyy H':'m':'s'.'f",
    "d MMM yyyy H':'m':'s zzz",
    "d MMM yyyy H':'m':'s GMT",
    "d MMM yyyy H':'m':'s",
    "d MMM yyyy H':'m zzz",
    "d MMM yyyy H':'m GMT",
    "d MMM yyyy H':'m",
  };
}
