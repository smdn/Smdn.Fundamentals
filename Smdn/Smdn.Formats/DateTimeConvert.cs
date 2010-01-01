// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009-2010 smdn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Globalization;

namespace Smdn.Formats {
  public static class DateTimeConvert {
    public static string GetCurrentTimeZoneOffsetString(bool delimiter)
    {
      var offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);

      if (delimiter) {
        if (TimeSpan.Zero <= offset)
          return string.Format("+{0:d2}:{1:d2}", offset.Hours, offset.Minutes);
        else
          return string.Format("-{0:d2}:{1:d2}", offset.Hours, offset.Minutes);
      }
      else {
        if (TimeSpan.Zero <= offset)
          return string.Format("+{0:d2}{1:d2}", offset.Hours, offset.Minutes);
        else
          return string.Format("-{0:d2}{1:d2}", offset.Hours, offset.Minutes);
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
      return dateTime.ToString("yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture);
    }

    public static string ToW3CDateTimeString(DateTimeOffset dateTimeOffset)
    {
      return dateTimeOffset.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
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
        throw new ArgumentNullException("s");

      var universal = s.EndsWith(universalTimeString);
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
        throw new ArgumentNullException("s");

      var universal = s.EndsWith(universalTimeString);
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
      "r",
      "ddd, d MMM yyyy HH:mm:ss zzz",
      "ddd, d MMM yyyy HH:mm:ss Z",
      "ddd, d MMM yyyy HH:mm:ss",
      "ddd, d MMM yyyy HH:mm zzz",
      "ddd, d MMM yyyy HH:mm Z",
      "ddd, d MMM yyyy HH:mm",
      "d MMM yyyy HH:mm:ss zzz",
      "d MMM yyyy HH:mm:ss Z",
      "d MMM yyyy HH:mm:ss",
      "d MMM yyyy HH:mm zzz",
      "d MMM yyyy HH:mm Z",
      "d MMM yyyy HH:mm",
    };

    private static readonly string w3cUniversalTimeString = "Z";

    private static string[] w3cDateTimeFormats = new string[]
    {
      "u",
      "yyyy-MM-ddTHH:mm:ss.fzzz",
      "yyyy-MM-ddTHH:mm:ss.f'Z'",
      "yyyy-MM-ddTHH:mm:ss.f",
      "yyyy-MM-ddTHH:mm:sszzz",
      "yyyy-MM-ddTHH:mm:ss'Z'",
      "yyyy-MM-ddTHH:mm:ss",
      "yyyy-MM-ddTHH:mmzzz",
      "yyyy-MM-ddTHH:mm'Z'",
      "yyyy-MM-ddTHH:mm",
      "yyyy-MM-dd HH:mm:ss.fzzz",
      "yyyy-MM-dd HH:mm:ss.f'Z'",
      "yyyy-MM-dd HH:mm:ss.f",
      "yyyy-MM-dd HH:mm:sszzz",
      "yyyy-MM-dd HH:mm:ss'Z'",
      "yyyy-MM-dd HH:mm:ss",
      "yyyy-MM-dd HH:mmzzz",
      "yyyy-MM-dd HH:mm'Z'",
      "yyyy-MM-dd HH:mm",
    };
  }
}
