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

using Smdn.Formats;

namespace Smdn {
  public static class NullableConvert {
    public static Uri ToUri(string val)
    {
      return (val == null) ? null : new Uri(val);
    }

    public static Uri ToUriIgnoreException(string val)
    {
      try { return ToUri(val); }
      catch { return null; }
    }

    public static string ToString(Uri val)
    {
      return (val == null) ? null : val.ToString();
    }

    public static int? ToInt32(string val)
    {
      return (val == null) ? (int?)null : int.Parse(val);
    }

    public static int? ToInt32IgnoreException(string val)
    {
      try { return ToInt32(val); }
      catch { return null; }
    }

    public static string ToString(int? val)
    {
      return (val == null) ? null : val.Value.ToString();
    }

    public static double? ToDouble(string val)
    {
      return (val == null) ? (double?)null : double.Parse(val);
    }

    public static double? ToDoubleIgnoreException(string val)
    {
      try { return ToDouble(val); }
      catch { return null; }
    }

    public static string ToString(double? val)
    {
      return (val == null) ? null : val.Value.ToString();
    }

    public static bool? ToBoolean(string val)
    {
      return (val == null) ? (bool?)null : bool.Parse(val);
    }

    public static bool? ToBooleanIgnoreException(string val)
    {
      try { return ToBoolean(val); }
      catch { return null; }
    }

    public static string ToString(bool? val)
    {
      return (val == null) ? null : val.Value.ToString().ToLowerInvariant();
    }

    public static TEnum? ToEnum<TEnum>(string val) where TEnum : struct /*instead of Enum*/
    {
      return (val == null) ? (TEnum?)null : EnumUtils.Parse<TEnum>(val, true);
    }

    public static TEnum? ToEnumIgnoreException<TEnum>(string val) where TEnum : struct /*instead of Enum*/
    {
      try { return ToEnum<TEnum>(val); }
      catch { return null; }
    }

    public static Guid? ToGuid(string val)
    {
      if (val == null)
        return null;
      else if (string.Empty.Equals(val))
        return Guid.Empty;
      else
        return new Guid(val);
    }

    public static Guid? ToGuidIgnoreException(string val)
    {
      try { return ToGuid(val); }
      catch { return null; }
    }

#region "DateTime"
    public static string ToRFC822DateTimeString(DateTime? dateTime)
    {
      return (dateTime == null)
        ? null
        : DateTimeConvert.ToRFC822DateTimeString(dateTime.Value);
    }

    public static string ToRFC822DateTimeString(DateTimeOffset? dateTimeOffset)
    {
      return (dateTimeOffset == null)
        ? null
        : DateTimeConvert.ToRFC822DateTimeString(dateTimeOffset.Value);
    }

    public static DateTime? FromRFC822DateTimeString(string s)
    {
      return (s == null)
        ? (DateTime?)null
        : DateTimeConvert.FromRFC822DateTimeString(s);
    }

    public static DateTimeOffset? FromRFC822DateTimeOffsetString(string s)
    {
      return (s == null)
        ? (DateTimeOffset?)null
        : DateTimeConvert.FromRFC822DateTimeOffsetString(s);
    }

    public static DateTimeOffset? FromRFC822DateTimeOffsetStringIgnoreException(string s)
    {
      try { return FromRFC822DateTimeOffsetString(s); }
      catch { return null; }
    }

    public static string ToW3CDateTimeString(DateTime? dateTime)
    {
      return (dateTime == null)
        ? null
        : DateTimeConvert.ToW3CDateTimeString(dateTime.Value);
    }

    public static string ToW3CDateTimeString(DateTimeOffset? dateTimeOffset)
    {
      return (dateTimeOffset == null)
        ? null
        : DateTimeConvert.ToW3CDateTimeString(dateTimeOffset.Value);
    }

    public static DateTime? FromW3CDateTimeString(string s)
    {
      return (s == null)
        ? (DateTime?)null
        : DateTimeConvert.FromW3CDateTimeString(s);
    }

    public static DateTimeOffset? FromW3CDateTimeOffsetString(string s)
    {
      return (s == null)
        ? (DateTimeOffset?)null
        : DateTimeConvert.FromW3CDateTimeOffsetString(s);
    }

    public static DateTimeOffset? FromW3CDateTimeOffsetStringIgnoreException(string s)
    {
      try { return FromW3CDateTimeOffsetString(s); }
      catch { return null; }
    }
#endregion
  }
}
