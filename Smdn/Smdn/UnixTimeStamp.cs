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

namespace Smdn {
  [Obsolete("This class is no longer supported. Use System.DateTimeOffset instead.", true)]
  public static class UnixTimeStamp {
    public readonly static DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long UtcNow {
      get { throw new NotSupportedException("This member is no longer supported."); }
    }

    public static long Now {
      get { throw new NotSupportedException("This member is no longer supported."); }
    }

    public static int ToInt32(DateTimeOffset dateTimeOffset)
    {
      throw new NotSupportedException("This member is no longer supported.");
    }

    public static int ToInt32(DateTime dateTime)
    {
      throw new NotSupportedException("This member is no longer supported.");
    }

    public static long ToInt64(DateTimeOffset dateTimeOffset)
    {
      throw new NotSupportedException("This member is no longer supported.");
    }

    public static long ToInt64(DateTime dateTime)
    {
      throw new NotSupportedException("This member is no longer supported.");
    }

    public static DateTime ToLocalDateTime(int unixTime)
    {
      throw new NotSupportedException("This member is no longer supported.");
    }

    public static DateTime ToUtcDateTime(int unixTime)
    {
      throw new NotSupportedException("This member is no longer supported.");
    }

    public static DateTime ToLocalDateTime(long unixTime)
    {
      throw new NotSupportedException("This member is no longer supported.");
    }

    public static DateTime ToUtcDateTime(long unixTime)
    {
      throw new NotSupportedException("This member is no longer supported.");
    }
  }
}
