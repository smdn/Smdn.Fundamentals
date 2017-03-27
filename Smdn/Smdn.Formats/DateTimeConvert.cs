// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2014 smdn
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
  [Obsolete("use Smdn.Formats.DateTimeFormat instead")]
  public static class DateTimeConvert {
    public static string GetCurrentTimeZoneOffsetString(bool delimiter)
    {
      return Smdn.Formats.DateTimeFormat.GetCurrentTimeZoneOffsetString(delimiter);
    }

    public static string ToRFC822DateTimeString(DateTime dateTime)
    {
      return Smdn.Formats.DateTimeFormat.ToRFC822DateTimeString(dateTime);
    }

    public static string ToRFC822DateTimeString(DateTimeOffset dateTimeOffset)
    {
      return Smdn.Formats.DateTimeFormat.ToRFC822DateTimeString(dateTimeOffset);
    }

    public static string ToRFC822DateTimeStringNullable(DateTimeOffset? dateTimeOffset)
    {
      return Smdn.Formats.DateTimeFormat.ToRFC822DateTimeStringNullable(dateTimeOffset);
    }

    public static DateTime FromRFC822DateTimeString(string s)
    {
      return Smdn.Formats.DateTimeFormat.FromRFC822DateTimeString(s);
    }

    public static DateTimeOffset FromRFC822DateTimeOffsetString(string s)
    {
      return Smdn.Formats.DateTimeFormat.FromRFC822DateTimeOffsetString(s);
    }

    public static DateTimeOffset? FromRFC822DateTimeOffsetStringNullable(string s)
    {
      return Smdn.Formats.DateTimeFormat.FromRFC822DateTimeOffsetStringNullable(s);
    }

    public static string ToISO8601DateTimeString(DateTime dateTime)
    {
      return Smdn.Formats.DateTimeFormat.ToISO8601DateTimeString(dateTime);
    }

    public static string ToISO8601DateTimeString(DateTimeOffset dateTimeOffset)
    {
      return Smdn.Formats.DateTimeFormat.ToISO8601DateTimeString(dateTimeOffset);
    }

    public static string ToW3CDateTimeString(DateTime dateTime)
    {
      return Smdn.Formats.DateTimeFormat.ToW3CDateTimeString(dateTime);
    }

    public static string ToW3CDateTimeString(DateTimeOffset dateTimeOffset)
    {
      return Smdn.Formats.DateTimeFormat.ToW3CDateTimeString(dateTimeOffset);
    }

    public static string ToW3CDateTimeStringNullable(DateTimeOffset? dateTimeOffset)
    {
      return Smdn.Formats.DateTimeFormat.ToW3CDateTimeStringNullable(dateTimeOffset);
    }

    public static DateTime FromISO8601DateTimeString(string s)
    {
      return Smdn.Formats.DateTimeFormat.FromISO8601DateTimeString(s);
    }

    public static DateTime FromW3CDateTimeString(string s)
    {
      return Smdn.Formats.DateTimeFormat.FromW3CDateTimeString(s);
    }

    public static DateTimeOffset FromISO8601DateTimeOffsetString(string s)
    {
      return Smdn.Formats.DateTimeFormat.FromISO8601DateTimeOffsetString(s);
    }

    public static DateTimeOffset FromW3CDateTimeOffsetString(string s)
    {
      return Smdn.Formats.DateTimeFormat.FromW3CDateTimeOffsetString(s);
    }

    public static DateTimeOffset? FromW3CDateTimeOffsetStringNullable(string s)
    {
      return Smdn.Formats.DateTimeFormat.FromW3CDateTimeOffsetStringNullable(s);
    }
  }
}
