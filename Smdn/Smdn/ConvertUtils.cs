// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2013 smdn
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
  public static class ConvertUtils {
    public static Uri ToUri(string val)
    {
      if (val == null)
        throw new ArgumentNullException("val");

      return new Uri(val);
    }

    public static Uri ToUriNullable(string val)
    {
      return (val == null) ? null : new Uri(val);
    }

    public static string ToString(Uri val)
    {
      if (val == null)
        throw new ArgumentNullException("val");

      return val.ToString();
    }

    public static string ToStringNullable(Uri val)
    {
      return (val == null) ? null : val.ToString();
    }

    public static int? ToInt32Nullable(string val)
    {
      return (val == null) ? (int?)null : int.Parse(val);
    }

    public static string ToStringNullable(int? val)
    {
      return (val == null) ? null : val.Value.ToString();
    }

    public static bool? ToBooleanNullable(string val)
    {
      return (val == null) ? (bool?)null : bool.Parse(val);
    }

    public static string ToStringNullable(bool? val)
    {
      return (val == null) ? null : val.Value.ToString().ToLowerInvariant();
    }

    public static TEnum? ToEnumNullable<TEnum>(string val) where TEnum : struct /*instead of Enum*/
    {
      return (val == null) ? (TEnum?)null : EnumUtils.Parse<TEnum>(val, true);
    }
  }
}
