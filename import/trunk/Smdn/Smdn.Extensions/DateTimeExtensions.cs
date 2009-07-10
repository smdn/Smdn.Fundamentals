// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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

namespace Smdn.Extensions {
  /// <summary>
  /// extension methods for System.DateTime
  /// </summary>
  public static class DateTimeExtensions {
#region "formatting and parsing extensions"
    public static string ToRfc822DateTimeString(this DateTime dateTime)
    {
      if (dateTime.Kind == DateTimeKind.Utc)
        return dateTime.ToString("ddd, d MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
      else
        return string.Format("{0} {1}",
                             dateTime.ToString("ddd, d MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                             dateTime.ToString("zzz", CultureInfo.InvariantCulture).Replace(":", string.Empty));
    }

    public static string ToISO8601DateTimeString(this DateTime dateTime)
    {
      return ToW3CDateTimeString(dateTime);
    }

    public static string ToW3CDateTimeString(this DateTime dateTime)
    {
      if (dateTime.Kind == DateTimeKind.Utc)
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss'Z'", CultureInfo.InvariantCulture);
      else
        return dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
    }

    public static DateTime FromRFC822DateTimeString(string s)
    {
      return FromDateTimeString(s, rfc822Formats, "GMT");
    }

    public static DateTime FromISO8601DateTimeString(string s)
    {
      return FromW3CDateTimeString(s);
    }

    public static DateTime FromW3CDateTimeString(string s)
    {
      return FromDateTimeString(s, w3cFormats, "Z");
    }

    private static DateTime FromDateTimeString(string s, string[] formats, string utc)
    {
      if (s == null)
        throw new ArgumentNullException("str");

      var dateTime = DateTime.ParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);

      if (s.EndsWith(utc))
        return new DateTime(dateTime.Ticks, DateTimeKind.Utc);
      else
        return new DateTime(dateTime.Ticks, DateTimeKind.Local);
    }

    private static readonly string[] rfc822Formats = new[]
    {
      "d MMM yyyy HH:mm",
      "d MMM yyyy HH:mm z",
      "d MMM yyyy HH:mm zz",
      "d MMM yyyy HH:mm zzz",
      "d MMM yyyy HH:mm 'GMT'",
      "d MMM yyyy HH:mm:ss",
      "d MMM yyyy HH:mm:ss z",
      "d MMM yyyy HH:mm:ss zz",
      "d MMM yyyy HH:mm:ss zzz",
      "d MMM yyyy HH:mm:ss 'GMT'",
      "ddd, d MMM yyyy HH:mm",
      "ddd, d MMM yyyy HH:mm z",
      "ddd, d MMM yyyy HH:mm zz",
      "ddd, d MMM yyyy HH:mm zzz",
      "ddd, d MMM yyyy HH:mm 'GMT'",
      "ddd, d MMM yyyy HH:mm:ss",
      "ddd, d MMM yyyy HH:mm:ss z",
      "ddd, d MMM yyyy HH:mm:ss zz",
      "ddd, d MMM yyyy HH:mm:ss zzz",
      "ddd, d MMM yyyy HH:mm:ss 'GMT'",
    };

    private static string[] w3cFormats = new string[]
    {
      "yyyy-MM-ddTHH:mm'Z'",
      "yyyy-MM-ddTHH:mm:ss'Z'",
      "yyyy-MM-dd HH:mm'Z'",
      "yyyy-MM-dd HH:mm:ss'Z'",
      "yyyy-MM-ddTHH:mmzzz",
      "yyyy-MM-ddTHH:mm:sszzz",
      "yyyy-MM-dd HH:mmzzz",
      "yyyy-MM-dd HH:mm:sszzz",
    };
#endregion

#region "extensions for unix time"
    public static int ToUnixTime32(this DateTime dateTime)
    {
      if (dateTime.Kind != DateTimeKind.Utc)
        dateTime = dateTime.ToUniversalTime();

      return (int)dateTime.Subtract(UnixEpoch).TotalSeconds;
    }

    public static long ToUnixTime64(this DateTime dateTime)
    {
      if (dateTime.Kind != DateTimeKind.Utc)
        dateTime = dateTime.ToUniversalTime();

      return (long)dateTime.Subtract(UnixEpoch).TotalSeconds;
    }

    public static DateTime FromUnixTime(int unixTime)
    {
      return FromUnixTime((long)unixTime);
    }

    public static DateTime FromUnixTimeUtc(int unixTime)
    {
      return FromUnixTimeUtc((long)unixTime);
    }

    public static DateTime FromUnixTime(long unixTime)
    {
      // this might overflow
      return UnixEpoch.AddSeconds(unixTime).ToLocalTime();
    }

    public static DateTime FromUnixTimeUtc(long unixTime)
    {
      // this might overflow
      return UnixEpoch.AddSeconds(unixTime);
    }

    public readonly static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
#endregion

#region "extensions for date time of ISO base media file format"
    public static DateTime FromIso14496DateTime(ulong isoDateTime)
    {
      // this might overflow
      return Iso14496DateTimeEpoch.AddSeconds(isoDateTime);
    }

    public static DateTime FromIso14496DateTime(uint isoDateTime)
    {
      return Iso14496DateTimeEpoch.AddSeconds(isoDateTime);
    }

    public static ulong ToIso14496DateTime64(this DateTime dateTime)
    {
      if (dateTime.Kind != DateTimeKind.Utc)
        dateTime = dateTime.ToUniversalTime();

      return (ulong)dateTime.Subtract(Iso14496DateTimeEpoch).TotalSeconds;
    }

    public static uint ToIso14496DateTime32(this DateTime dateTime)
    {
      if (dateTime.Kind != DateTimeKind.Utc)
        dateTime = dateTime.ToUniversalTime();

      return (uint)dateTime.Subtract(Iso14496DateTimeEpoch).TotalSeconds;
    }

    public readonly static DateTime Iso14496DateTimeEpoch = new DateTime(1904, 1, 1, 0, 0, 0, DateTimeKind.Utc);
#endregion
  }
}
