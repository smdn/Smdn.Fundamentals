// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Smdn.Formats.DateAndTime;

namespace Smdn.Formats;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static partial class DateTimeFormat {
  public static string GetCurrentTimeZoneOffsetString(bool delimiter)
  {
    var offset = TimeZoneInfo.Local.BaseUtcOffset;
    var sign = TimeSpan.Zero <= offset ? "+" : "-";
    var delim = delimiter ? ":" : string.Empty;

#if SYSTEM_FORMATTABLESTRING
    return FormattableString.Invariant($"{sign}{offset.Hours:d2}{delim}{offset.Minutes:d2}");
#else
    return string.Concat(sign, offset.Hours.ToString("d2", CultureInfo.InvariantCulture), delim, offset.Minutes.ToString("d2", CultureInfo.InvariantCulture));
#endif
  }

  private static DateTime FromDateTimeString(
    string s,
    string[] formats,
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions
  )
  {
    if (s is null)
      throw new ArgumentNullException(nameof(s));

    (s, var tz) = ProcessTimeZoneSpecifier(
      s,
      timeZoneDefinitions,
      out var dateTimeStylesOfTimeZone
    );

    var dateAndTime = DateTime.ParseExact(
      s,
      formats,
      CultureInfo.InvariantCulture,
      dateTimeStylesOfTimeZone | DateTimeStyles.AllowWhiteSpaces
    );

    return tz is null || tz.IsUniversal
      ? dateAndTime
      : tz.AdjustToTimeZone(dateAndTime); // TODO: JST, EST, etc; use TimeZoneInfo
  }

  private static DateTimeOffset FromDateTimeOffsetString(
    string s,
    string[] formats,
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions
  )
  {
    if (s is null)
      throw new ArgumentNullException(nameof(s));

    (s, var tz) = ProcessTimeZoneSpecifier(
      s,
      timeZoneDefinitions,
      out var dateTimeStylesOfTimeZone
    );

    var dateAndTime = DateTimeOffset.ParseExact(
      s,
      formats,
      CultureInfo.InvariantCulture,
      dateTimeStylesOfTimeZone | DateTimeStyles.AllowWhiteSpaces
    );

    return tz is null || tz.IsUniversal
      ? dateAndTime
      : tz.AdjustToTimeZone(dateAndTime); // TODO: JST, EST, etc; use TimeZoneInfo
  }

  private static (string StringWithoutTimeZoneSpecifier, TimeZoneDefinition TimeZone) ProcessTimeZoneSpecifier(
    string s,
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions,
    out DateTimeStyles dateTimeStylesOfTimeZone
  )
  {
    dateTimeStylesOfTimeZone = DateTimeStyles.AssumeLocal;

    var tz = timeZoneDefinitions.FirstOrDefault(
      tz => s.EndsWith(tz.Suffix, StringComparison.Ordinal)
    );

    if (tz is null)
      return (s, null);

    dateTimeStylesOfTimeZone = tz.IsUniversal
      ? DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
      : DateTimeStyles.RoundtripKind;

    return (s.Substring(0, s.Length - tz.Suffix.Length), tz);
  }
}
