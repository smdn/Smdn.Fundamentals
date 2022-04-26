// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class DateTimeFormat {
#pragma warning restore IDE0040
  public static string ToW3CDateTimeString(DateTime dateTime)
    => dateTime.ToString("o");

  public static string ToW3CDateTimeString(DateTimeOffset dateTimeOffset)
    => dateTimeOffset.ToString("o");

  public static string ToW3CDateTimeStringNullable(DateTimeOffset? dateTimeOffset)
    => dateTimeOffset.HasValue
      ? ToW3CDateTimeString(dateTimeOffset.Value)
      : null;

  public static DateTime FromW3CDateTimeString(string s)
    => FromDateTimeString(s, w3cDateTimeFormats, W3cUniversalTimeString);

  public static DateTimeOffset FromW3CDateTimeOffsetString(string s)
    => FromDateTimeOffsetString(s, w3cDateTimeFormats, W3cUniversalTimeString);

  public static DateTimeOffset? FromW3CDateTimeOffsetStringNullable(string s)
    => s is null
      ? null
      : FromW3CDateTimeOffsetString(s);

  private const string W3cUniversalTimeString = "Z";

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
