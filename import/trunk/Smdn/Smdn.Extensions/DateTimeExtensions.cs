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

namespace Smdn.Extensions {
  /// <summary>
  /// extension methods for System.DateTime
  /// </summary>
  public static class DateTimeExtensions {
#region "Unix time extensions"
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

#region "ISO date time extensions"
    public static DateTime FromIsoDateTime(ulong isoDateTime)
    {
      // this might overflow
      return IsoDateTimeEpoch.AddSeconds(isoDateTime);
    }

    public static DateTime FromIsoDateTime(uint isoDateTime)
    {
      return IsoDateTimeEpoch.AddSeconds(isoDateTime);
    }

    public static ulong ToIsoDateTime64(this DateTime dateTime)
    {
      if (dateTime.Kind != DateTimeKind.Utc)
        dateTime = dateTime.ToUniversalTime();

      return (ulong)dateTime.Subtract(IsoDateTimeEpoch).TotalSeconds;
    }

    public static uint ToIsoDateTime32(this DateTime dateTime)
    {
      if (dateTime.Kind != DateTimeKind.Utc)
        dateTime = dateTime.ToUniversalTime();

      return (uint)dateTime.Subtract(IsoDateTimeEpoch).TotalSeconds;
    }

    public readonly static DateTime IsoDateTimeEpoch = new DateTime(1904, 1, 1, 0, 0, 0, DateTimeKind.Utc);
#endregion
  }
}
