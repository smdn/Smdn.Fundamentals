// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;

using Smdn.Formats.DateAndTime;

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
    => FromDateTimeString(s, rfc822DateTimeFormats, RFC822TimeZoneDefinitions);

  public static DateTimeOffset FromRFC822DateTimeOffsetString(string s)
    => FromDateTimeOffsetString(s, rfc822DateTimeFormats, RFC822TimeZoneDefinitions);

  public static DateTimeOffset? FromRFC822DateTimeOffsetStringNullable(string s)
    => s is null
      ? null
      : FromRFC822DateTimeOffsetString(s);

  private static readonly IReadOnlyList<TimeZoneDefinition> RFC822TimeZoneDefinitions = new TimeZoneDefinition[] {
    new UniversalTimeZoneDefinition(" GMT"),
    new UniversalTimeZoneDefinition(" UT"),
    new RFC5322MilitaryTimeZoneDefinition(" A"),
    new RFC5322MilitaryTimeZoneDefinition(" B"),
    new RFC5322MilitaryTimeZoneDefinition(" C"),
    new RFC5322MilitaryTimeZoneDefinition(" D"),
    new RFC5322MilitaryTimeZoneDefinition(" E"),
    new RFC5322MilitaryTimeZoneDefinition(" F"),
    new RFC5322MilitaryTimeZoneDefinition(" G"),
    new RFC5322MilitaryTimeZoneDefinition(" H"),
    new RFC5322MilitaryTimeZoneDefinition(" I"),
    new RFC5322MilitaryTimeZoneDefinition(" K"),
    new RFC5322MilitaryTimeZoneDefinition(" L"),
    new RFC5322MilitaryTimeZoneDefinition(" M"),
    new RFC5322MilitaryTimeZoneDefinition(" N"),
    new RFC5322MilitaryTimeZoneDefinition(" O"),
    new RFC5322MilitaryTimeZoneDefinition(" P"),
    new RFC5322MilitaryTimeZoneDefinition(" Q"),
    new RFC5322MilitaryTimeZoneDefinition(" R"),
    new RFC5322MilitaryTimeZoneDefinition(" S"),
    new RFC5322MilitaryTimeZoneDefinition(" T"),
    new RFC5322MilitaryTimeZoneDefinition(" U"),
    new RFC5322MilitaryTimeZoneDefinition(" V"),
    new RFC5322MilitaryTimeZoneDefinition(" W"),
    new RFC5322MilitaryTimeZoneDefinition(" X"),
    new RFC5322MilitaryTimeZoneDefinition(" Y"),
    new RFC5322MilitaryTimeZoneDefinition(" Z"),
  };

  private class RFC5322MilitaryTimeZoneDefinition : TimeZoneDefinition {
    public RFC5322MilitaryTimeZoneDefinition(string suffix)
      : base(suffix)
    {
    }

    /*
     * [RFC5322] Internet Message Format 4.3.  Obsolete Date and Time
     * 'However, because of
     * the error in [RFC0822], they SHOULD all be considered equivalent to
     * "-0000" unless there is out-of-band information confirming their
     * meaning.'
     */
    public override DateTime AdjustToTimeZone(DateTime dateAndTime)
      => dateAndTime; // DateTime.SpecifyKind(dateAndTime, DateTimeKind.Unspecified);
    public override DateTimeOffset AdjustToTimeZone(DateTimeOffset dateAndTime)
      => new(dateAndTime.DateTime, TimeSpan.FromHours(-0.0));
  }

  private static readonly string[] rfc822DateTimeFormats = new[]
  {
    "r",
    "ddd',' d MMM yyyy H':'m':'s'.'fffffff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'fffffff",
    "ddd',' d MMM yyyy H':'m':'s'.'ffffff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'ffffff",
    "ddd',' d MMM yyyy H':'m':'s'.'fffff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'fffff",
    "ddd',' d MMM yyyy H':'m':'s'.'ffff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'ffff",
    "ddd',' d MMM yyyy H':'m':'s'.'fff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'fff",
    "ddd',' d MMM yyyy H':'m':'s'.'ff zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'ff",
    "ddd',' d MMM yyyy H':'m':'s'.'f zzz",
    "ddd',' d MMM yyyy H':'m':'s'.'f",
    "ddd',' d MMM yyyy H':'m':'s zzz",
    "ddd',' d MMM yyyy H':'m':'s",
    "ddd',' d MMM yyyy H':'m zzz",
    "ddd',' d MMM yyyy H':'m",
    "d MMM yyyy H':'m':'s'.'fffffff zzz",
    "d MMM yyyy H':'m':'s'.'fffffff",
    "d MMM yyyy H':'m':'s'.'ffffff zzz",
    "d MMM yyyy H':'m':'s'.'ffffff",
    "d MMM yyyy H':'m':'s'.'fffff zzz",
    "d MMM yyyy H':'m':'s'.'fffff",
    "d MMM yyyy H':'m':'s'.'ffff zzz",
    "d MMM yyyy H':'m':'s'.'ffff",
    "d MMM yyyy H':'m':'s'.'fff zzz",
    "d MMM yyyy H':'m':'s'.'fff",
    "d MMM yyyy H':'m':'s'.'ff zzz",
    "d MMM yyyy H':'m':'s'.'ff",
    "d MMM yyyy H':'m':'s'.'f zzz",
    "d MMM yyyy H':'m':'s'.'f",
    "d MMM yyyy H':'m':'s zzz",
    "d MMM yyyy H':'m':'s",
    "d MMM yyyy H':'m zzz",
    "d MMM yyyy H':'m",
  };
}
