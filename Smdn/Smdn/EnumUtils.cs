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
  [Obsolete("use Smdn.Text.StringConversion instead")]
  public static class EnumUtils {
    public static TEnum Parse<TEnum>(string value) where TEnum : struct /*instead of Enum*/
    {
      return Smdn.Text.StringConversion.ToEnum<TEnum>(value);
    }

    public static TEnum ParseIgnoreCase<TEnum>(string value) where TEnum : struct /*instead of Enum*/
    {
      return Smdn.Text.StringConversion.ToEnumIgnoreCase<TEnum>(value);
    }

    public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct /*instead of Enum*/
    {
      return Smdn.Text.StringConversion.ToEnum<TEnum>(value, ignoreCase);
    }

    [Obsolete("use Enum.TryParse instead")]
    public static bool TryParseIgnoreCase<TEnum>(string value, out TEnum result) where TEnum : struct /*instead of Enum*/
    {
      return Enum.TryParse(value, true, out result);
    }
  }
}
