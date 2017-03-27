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
using System.Collections.Generic;
using System.Linq;

namespace Smdn {
  [Obsolete("use Smdn.Text.StringConversion instead")]
  public static class ConvertUtils {
    public static Uri ToUri(string val)
    {
      return Smdn.Text.StringConversion.ToUri(val);
    }

    public static Uri ToUriNullable(string val)
    {
      return Smdn.Text.StringConversion.ToUriNullable(val);
    }

    public static string ToString(Uri val)
    {
      return Smdn.Text.StringConversion.ToString(val);
    }

    public static string ToStringNullable(Uri val)
    {
      return Smdn.Text.StringConversion.ToStringNullable(val);
    }

    public static int? ToInt32Nullable(string val)
    {
      return Smdn.Text.StringConversion.ToInt32Nullable(val);
    }

    public static string ToStringNullable(int? val)
    {
      return Smdn.Text.StringConversion.ToStringNullable(val);
    }

    public static bool? ToBooleanNullable(string val)
    {
      return Smdn.Text.StringConversion.ToBooleanNullable(val);
    }

    public static string ToStringNullable(bool? val)
    {
      return Smdn.Text.StringConversion.ToStringNullable(val);
    }

    public static TEnum? ToEnumNullable<TEnum>(string val) where TEnum : struct /*instead of Enum*/
    {
      return Smdn.Text.StringConversion.ToEnumNullable<TEnum>(val);
    }

    public static string ToJoinedString<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
    {
      return Smdn.Text.StringConversion.ToJoinedString(pairs);
    }
  }
}
