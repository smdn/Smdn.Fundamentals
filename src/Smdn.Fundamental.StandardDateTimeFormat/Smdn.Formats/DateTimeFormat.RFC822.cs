// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

#if NET35_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_TIMEZONEINFO_FINDSYSTEMTIMEZONEBYID
#endif
#if NET35_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_TIMEZONENOTFOUNDEXCEPTION
#endif
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

    return string.Concat(
      str,
      dateTime.Kind == DateTimeKind.Utc
        ? "+0000"
        : GetCurrentTimeZoneOffsetString(false)
    );
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
    new RFC5322EasternTimeZoneDefinition(" EST"),
    new RFC5322EasternTimeZoneDefinition(" EDT"),
    new RFC5322CentralTimeZoneDefinition(" CST"),
    new RFC5322CentralTimeZoneDefinition(" CDT"),
    new RFC5322MountainTimeZoneDefinition(" MST"),
    new RFC5322MountainTimeZoneDefinition(" MDT"),
    new RFC5322PacificTimeZoneDefinition(" PST"),
    new RFC5322PacificTimeZoneDefinition(" PDT"),
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

  // EST(-05:00)/EDT(-04:00)
  private class RFC5322EasternTimeZoneDefinition : RFC5322NorthAmericanTimeZoneDefinition {
#if SYSTEM_TIMEZONEINFO_FINDSYSTEMTIMEZONEBYID
    private static readonly IReadOnlyList<string> timeZoneIds = new[] {
      "US/Eastern",
      "Eastern Standard Time",
      "America/New_York",
      "EST",
    };

    public RFC5322EasternTimeZoneDefinition(string prefix)
      : base(prefix, "EST", timeZoneIds)
    {
    }
#else
    public RFC5322EasternTimeZoneDefinition(string prefix)
      : base(
        prefix,
        prefix.EndsWith("EST", StringComparison.Ordinal)
          ? TimeSpan.FromHours(-5.0)
          : TimeSpan.FromHours(-4.0)
      )
    {
    }
#endif
  }

  // CST(-06:00)/CDT(-05:00)
  private class RFC5322CentralTimeZoneDefinition : RFC5322NorthAmericanTimeZoneDefinition {
#if SYSTEM_TIMEZONEINFO_FINDSYSTEMTIMEZONEBYID
    private static readonly IReadOnlyList<string> timeZoneIds = new[] {
      "US/Central",
      "Central Standard Time",
      "America/Chicago",
      "CST",
    };

    public RFC5322CentralTimeZoneDefinition(string prefix)
      : base(prefix, "CST", timeZoneIds)
    {
    }
#else
    public RFC5322CentralTimeZoneDefinition(string prefix)
      : base(
        prefix,
        prefix.EndsWith("CST", StringComparison.Ordinal)
          ? TimeSpan.FromHours(-6.0)
          : TimeSpan.FromHours(-5.0)
      )
    {
    }
#endif
  }

  // MST(-07:00)/MDT(-06:00)
  private class RFC5322MountainTimeZoneDefinition : RFC5322NorthAmericanTimeZoneDefinition {
#if SYSTEM_TIMEZONEINFO_FINDSYSTEMTIMEZONEBYID
    private static readonly IReadOnlyList<string> timeZoneIds = new[] {
      "US/Mountain",
      "Mountain Standard Time",
      "America/Denver",
      "MST",
    };

    public RFC5322MountainTimeZoneDefinition(string prefix)
      : base(prefix, "MST", timeZoneIds)
    {
    }
#else
    public RFC5322MountainTimeZoneDefinition(string prefix)
      : base(
        prefix,
        prefix.EndsWith("MST", StringComparison.Ordinal)
          ? TimeSpan.FromHours(-7.0)
          : TimeSpan.FromHours(-6.0)
      )
    {
    }
#endif
  }

  // PST(-08:00)/PDT(-07:00)
  private class RFC5322PacificTimeZoneDefinition : RFC5322NorthAmericanTimeZoneDefinition {
#if SYSTEM_TIMEZONEINFO_FINDSYSTEMTIMEZONEBYID
    private static readonly IReadOnlyList<string> timeZoneIds = new[] {
      "US/Pacific",
      "Pacific Standard Time",
      "America/Los_Angeles",
      "PST",
    };

    public RFC5322PacificTimeZoneDefinition(string prefix)
      : base(prefix, "PST", timeZoneIds)
    {
    }
#else
    public RFC5322PacificTimeZoneDefinition(string prefix)
      : base(
        prefix,
        prefix.EndsWith("PST", StringComparison.Ordinal)
          ? TimeSpan.FromHours(-8.0)
          : TimeSpan.FromHours(-7.0)
      )
    {
    }
#endif
  }

  private abstract class RFC5322NorthAmericanTimeZoneDefinition : TimeZoneDefinition {
#if SYSTEM_TIMEZONEINFO_FINDSYSTEMTIMEZONEBYID
    private readonly string timeZoneName;
    private readonly TimeZoneInfo timeZoneInfo;

    protected RFC5322NorthAmericanTimeZoneDefinition(string suffix, string timeZoneName, IReadOnlyList<string> timeZoneIds)
      : base(suffix)
    {
      this.timeZoneName = timeZoneName;

      foreach (var id in timeZoneIds) {
        try {
          // ref: https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
          timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(id);
          break;
        }
#pragma warning disable CS0168
        catch (Exception ex)
#if SYSTEM_TIMEZONENOTFOUNDEXCEPTION
          when (ex is TimeZoneNotFoundException)
#endif
#pragma warning restore CS0168
        {
          continue;
        }
      }
    }

    private TimeSpan GetUtcOffset(DateTime dateAndTime)
    {
      if (timeZoneInfo is null) {
#pragma warning disable SA1114
#if SYSTEM_TIMEZONENOTFOUNDEXCEPTION
        throw new TimeZoneNotFoundException(
#else
        throw new InvalidOperationException(
#endif
          $"could not find TimeZoneInfo for the time zone '{timeZoneName}'"
        );
#pragma warning restore SA1114
      }

      return timeZoneInfo.GetUtcOffset(dateAndTime);
    }
#else // SYSTEM_TIMEZONEINFO_FINDSYSTEMTIMEZONEBYID
    private readonly TimeSpan utcOffset;

    protected RFC5322NorthAmericanTimeZoneDefinition(string suffix, TimeSpan utcOffset)
      : base(suffix)
    {
      this.utcOffset = utcOffset;
    }

#pragma warning disable IDE0060
    private TimeSpan GetUtcOffset(DateTime dateAndTime) => utcOffset;
#pragma warning restore IDE0060
#endif

    public override DateTime AdjustToTimeZone(DateTime dateAndTime)
      => new DateTimeOffset(dateAndTime, GetUtcOffset(dateAndTime)).UtcDateTime;

    public override DateTimeOffset AdjustToTimeZone(DateTimeOffset dateAndTime)
      => new(dateAndTime.DateTime, GetUtcOffset(dateAndTime.DateTime));
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
    "ddd',' d MMM yy H':'m':'s'.'fffffff zzz",
    "ddd',' d MMM yy H':'m':'s'.'fffffff",
    "ddd',' d MMM yy H':'m':'s'.'ffffff zzz",
    "ddd',' d MMM yy H':'m':'s'.'ffffff",
    "ddd',' d MMM yy H':'m':'s'.'fffff zzz",
    "ddd',' d MMM yy H':'m':'s'.'fffff",
    "ddd',' d MMM yy H':'m':'s'.'ffff zzz",
    "ddd',' d MMM yy H':'m':'s'.'ffff",
    "ddd',' d MMM yy H':'m':'s'.'fff zzz",
    "ddd',' d MMM yy H':'m':'s'.'fff",
    "ddd',' d MMM yy H':'m':'s'.'ff zzz",
    "ddd',' d MMM yy H':'m':'s'.'ff",
    "ddd',' d MMM yy H':'m':'s'.'f zzz",
    "ddd',' d MMM yy H':'m':'s'.'f",
    "ddd',' d MMM yy H':'m':'s zzz",
    "ddd',' d MMM yy H':'m':'s",
    "ddd',' d MMM yy H':'m zzz",
    "ddd',' d MMM yy H':'m",
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
    "d MMM yy H':'m':'s'.'fffffff zzz",
    "d MMM yy H':'m':'s'.'fffffff",
    "d MMM yy H':'m':'s'.'ffffff zzz",
    "d MMM yy H':'m':'s'.'ffffff",
    "d MMM yy H':'m':'s'.'fffff zzz",
    "d MMM yy H':'m':'s'.'fffff",
    "d MMM yy H':'m':'s'.'ffff zzz",
    "d MMM yy H':'m':'s'.'ffff",
    "d MMM yy H':'m':'s'.'fff zzz",
    "d MMM yy H':'m':'s'.'fff",
    "d MMM yy H':'m':'s'.'ff zzz",
    "d MMM yy H':'m':'s'.'ff",
    "d MMM yy H':'m':'s'.'f zzz",
    "d MMM yy H':'m':'s'.'f",
    "d MMM yy H':'m':'s zzz",
    "d MMM yy H':'m':'s",
    "d MMM yy H':'m zzz",
    "d MMM yy H':'m",
  };
}
