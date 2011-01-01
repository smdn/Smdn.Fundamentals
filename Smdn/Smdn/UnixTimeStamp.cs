// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2011 smdn
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

namespace Smdn {
  public static class UnixTimeStamp {
    public readonly static DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long UtcNow {
      get { return ToInt64(DateTime.UtcNow); }
    }

    public static long Now {
      get { return ToInt64(DateTime.Now); }
    }

    public static int ToInt32(DateTimeOffset dateTimeOffset)
    {
      return ToInt32(dateTimeOffset.UtcDateTime);
    }

    public static int ToInt32(DateTime dateTime)
    {
      if (dateTime.Kind != DateTimeKind.Utc)
        dateTime = dateTime.ToUniversalTime();

      return (int)dateTime.Subtract(Epoch).TotalSeconds;
    }

    public static long ToInt64(DateTimeOffset dateTimeOffset)
    {
      return ToInt64(dateTimeOffset.UtcDateTime);
    }

    public static long ToInt64(DateTime dateTime)
    {
      if (dateTime.Kind != DateTimeKind.Utc)
        dateTime = dateTime.ToUniversalTime();

      return (long)dateTime.Subtract(Epoch).TotalSeconds;
    }

    public static DateTime ToLocalDateTime(int unixTime)
    {
      return ToLocalDateTime((long)unixTime);
    }

    public static DateTime ToUtcDateTime(int unixTime)
    {
      return ToUtcDateTime((long)unixTime);
    }

    public static DateTime ToLocalDateTime(long unixTime)
    {
      // this might overflow
      return Epoch.AddSeconds(unixTime).ToLocalTime();
    }

    public static DateTime ToUtcDateTime(long unixTime)
    {
      // this might overflow
      return Epoch.AddSeconds(unixTime);
    }
  }
}
