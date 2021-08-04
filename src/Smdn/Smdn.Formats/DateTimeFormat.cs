// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Globalization;

namespace Smdn.Formats {
  public static class DateTimeFormat {
    public static string GetCurrentTimeZoneOffsetString(bool delimiter)
    {
      var offset = TimeZoneInfo.Local.BaseUtcOffset;

      if (delimiter) {
        if (TimeSpan.Zero <= offset)
          return string.Format(CultureInfo.InvariantCulture, "+{0:d2}:{1:d2}", offset.Hours, offset.Minutes);
        else
          return string.Format(CultureInfo.InvariantCulture, "-{0:d2}:{1:d2}", offset.Hours, offset.Minutes);
      }
      else {
        if (TimeSpan.Zero <= offset)
          return string.Format(CultureInfo.InvariantCulture, "+{0:d2}{1:d2}", offset.Hours, offset.Minutes);
        else
          return string.Format(CultureInfo.InvariantCulture, "-{0:d2}{1:d2}", offset.Hours, offset.Minutes);
      }
    }

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
    {
      return (dateTimeOffset == null) ? null : ToRFC822DateTimeString(dateTimeOffset.Value);
    }

    public static DateTime FromRFC822DateTimeString(string s)
    {
      return FromDateTimeString(s, rfc822DateTimeFormats, rfc822UniversalTimeString);
    }

    public static DateTimeOffset FromRFC822DateTimeOffsetString(string s)
    {
      return FromDateTimeOffsetString(s, rfc822DateTimeFormats, rfc822UniversalTimeString);
    }

    public static DateTimeOffset? FromRFC822DateTimeOffsetStringNullable(string s)
    {
      return (s == null) ? (DateTimeOffset?)null : (DateTimeOffset?)FromRFC822DateTimeOffsetString(s);
    }

    public static string ToISO8601DateTimeString(DateTime dateTime)
    {
      return ToW3CDateTimeString(dateTime);
    }

    public static string ToISO8601DateTimeString(DateTimeOffset dateTimeOffset)
    {
      return ToW3CDateTimeString(dateTimeOffset);
    }

    public static string ToW3CDateTimeString(DateTime dateTime)
    {
      return dateTime.ToString("o");
    }

    public static string ToW3CDateTimeString(DateTimeOffset dateTimeOffset)
    {
      return dateTimeOffset.ToString("o");
    }

    public static string ToW3CDateTimeStringNullable(DateTimeOffset? dateTimeOffset)
    {
      return (dateTimeOffset == null) ? null : ToW3CDateTimeString(dateTimeOffset.Value);
    }

    public static DateTime FromISO8601DateTimeString(string s)
    {
      return FromW3CDateTimeString(s);
    }

    public static DateTime FromW3CDateTimeString(string s)
    {
      return FromDateTimeString(s, w3cDateTimeFormats, w3cUniversalTimeString);
    }

    public static DateTimeOffset FromISO8601DateTimeOffsetString(string s)
    {
      return FromW3CDateTimeOffsetString(s);
    }

    public static DateTimeOffset FromW3CDateTimeOffsetString(string s)
    {
      return FromDateTimeOffsetString(s, w3cDateTimeFormats, w3cUniversalTimeString);
    }

    public static DateTimeOffset? FromW3CDateTimeOffsetStringNullable(string s)
    {
      return (s == null) ? (DateTimeOffset?)null : (DateTimeOffset?)FromW3CDateTimeOffsetString(s);
    }

    private static DateTime FromDateTimeString(string s, string[] formats, string universalTimeString)
    {
      if (s == null)
        throw new ArgumentNullException(nameof(s));

      var universal = s.EndsWith(universalTimeString, StringComparison.Ordinal);
      var styles = DateTimeStyles.AllowWhiteSpaces;

      if (universal)
        styles |= DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;
      else
        // TODO: JST, EST, etc; use TimeZoneInfo
        styles |= DateTimeStyles.AssumeLocal;

      return DateTime.ParseExact(s, formats, CultureInfo.InvariantCulture, styles);
    }

    private static DateTimeOffset FromDateTimeOffsetString(string s, string[] formats, string universalTimeString)
    {
      if (s == null)
        throw new ArgumentNullException(nameof(s));

      var universal = s.EndsWith(universalTimeString, StringComparison.Ordinal);
      var styles = DateTimeStyles.AllowWhiteSpaces;

      if (universal)
        styles |= DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;
      else
        // TODO: JST, EST, etc; use TimeZoneInfo
        styles |= DateTimeStyles.AssumeLocal;

      return DateTimeOffset.ParseExact(s, formats, CultureInfo.InvariantCulture, styles);
    }

    private static readonly string rfc822UniversalTimeString = " GMT";

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

    private static readonly string w3cUniversalTimeString = "Z";

    private static readonly string[] w3cDateTimeFormats = new string[]
    {
      // TODO: f1-f6
      "u",
      "yyyy-MM-ddTHH:mm:ss.fffffffzzz",
      "yyyy-MM-ddTHH:mm:ss.fffffff'Z'",
      "yyyy-MM-ddTHH:mm:ss.fffffff",
      "yyyy-MM-ddTHH:mm:ss.fffzzz",
      "yyyy-MM-ddTHH:mm:ss.fff'Z'",
      "yyyy-MM-ddTHH:mm:ss.fff",
      "yyyy-MM-ddTHH:mm:sszzz",
      "yyyy-MM-ddTHH:mm:ss'Z'",
      "yyyy-MM-ddTHH:mm:ss",
      "yyyy-MM-ddTHH:mmzzz",
      "yyyy-MM-ddTHH:mm'Z'",
      "yyyy-MM-ddTHH:mm",
      "yyyy-MM-dd HH:mm:ss.fffffffzzz",
      "yyyy-MM-dd HH:mm:ss.fffffff'Z'",
      "yyyy-MM-dd HH:mm:ss.fffffff",
      "yyyy-MM-dd HH:mm:ss.fffzzz",
      "yyyy-MM-dd HH:mm:ss.fff'Z'",
      "yyyy-MM-dd HH:mm:ss.fff",
      "yyyy-MM-dd HH:mm:sszzz",
      "yyyy-MM-dd HH:mm:ss'Z'",
      "yyyy-MM-dd HH:mm:ss",
      "yyyy-MM-dd HH:mmzzz",
      "yyyy-MM-dd HH:mm'Z'",
      "yyyy-MM-dd HH:mm",
    };
  }
}
