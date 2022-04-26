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
    "u",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff'Z'",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffff'Z'",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffff",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffff'Z'",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffff",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff'Z'",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f'Z'",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f",
    "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
    "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
    "yyyy'-'MM'-'dd'T'HH':'mmzzz",
    "yyyy'-'MM'-'dd'T'HH':'mm'Z'",
    "yyyy'-'MM'-'dd'T'HH':'mm",
    "yyyy'-'MM'-'dd'T'HHzzz",
    "yyyy'-'MM'-'dd'T'HH'Z'",
    "yyyy'-'MM'-'dd'T'HH",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffff'Z'",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffffffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffffff'Z'",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffffff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffff'Z'",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffff'Z'",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff'Z'",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ffzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ff'Z'",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'ff",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'f'Z'",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'f",
    "yyyy'-'MM'-'dd' 'HH':'mm':'sszzz",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss'Z'",
    "yyyy'-'MM'-'dd' 'HH':'mm':'ss",
    "yyyy'-'MM'-'dd' 'HH':'mmzzz",
    "yyyy'-'MM'-'dd' 'HH':'mm'Z'",
    "yyyy'-'MM'-'dd' 'HH':'mm",
    "yyyy'-'MM'-'dd' 'HHzzz",
    "yyyy'-'MM'-'dd' 'HH'Z'",
    "yyyy'-'MM'-'dd' 'HH",
  };
}
