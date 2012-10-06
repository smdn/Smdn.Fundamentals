// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2012 smdn
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
using System.Text;

namespace Smdn {
  public static class StringExtensions {
    public delegate string ReplaceCharEvaluator(char ch, string str, int index);
    public delegate string ReplaceStringEvaluator(string matched, string str, int index);

    public static string RemoveChars(this string str, params char[] oldChars)
    {
      return ReplaceInternal(str, oldChars, null);
    }

    public static string Remove(this string str, params string[] oldValues)
    {
      return ReplaceInternal(str, oldValues, null);
    }

    public static string Replace(this string str, char[] oldChars, ReplaceCharEvaluator evaluator)
    {
      if (evaluator == null)
        throw new ArgumentNullException("evaluator");

      return ReplaceInternal(str, oldChars, evaluator);
    }

    private static string ReplaceInternal(string str, char[] oldChars, ReplaceCharEvaluator evaluator)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (oldChars == null)
        throw new ArgumentNullException("oldChars");

      var lastIndex = 0;
      var sb = new StringBuilder(str.Length);

      for (;;) {
        var index = str.IndexOfAny(oldChars, lastIndex);

        if (index < 0) {
          sb.Append(str.Substring(lastIndex));
          break;
        }
        else {
          sb.Append(str.Substring(lastIndex, index - lastIndex));

          if (evaluator != null)
            sb.Append(evaluator(str[index], str, index));

          lastIndex = index + 1;
        }
      }

      return sb.ToString();
    }

    public static string Replace(this string str, string[] oldValues, ReplaceStringEvaluator evaluator)
    {
      if (evaluator == null)
        throw new ArgumentNullException("evaluator");

      return ReplaceInternal(str, oldValues, evaluator);
    }

    private static string ReplaceInternal(string str, string[] oldValues, ReplaceStringEvaluator evaluator)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (oldValues == null)
        throw new ArgumentNullException("oldValues");

      foreach (var oldValue in oldValues) {
        var lastIndex = 0;
        var sb = new StringBuilder();

        if (oldValue.Length == 0)
          continue;

        for (;;) {
          var index = str.IndexOf(oldValue, lastIndex);

          if (index < 0) {
            sb.Append(str.Substring(lastIndex));
            break;
          }
          else {
            sb.Append(str.Substring(lastIndex, index - lastIndex));

            if (evaluator != null)
              sb.Append(evaluator(oldValue, str, index));

            lastIndex = index + oldValue.Length;
          }
        }

        str = sb.ToString();
      }

      return str;
    }

    public static int Count(this string str, char c)
    {
      if (str == null)
        throw new ArgumentNullException("str");

      var chars = str.ToCharArray();
      var count = 0;

      for (var index = 0; index < chars.Length; index++) {
        if (chars[index] == c)
          count++;
      }

      return count;
    }

    public static int Count(this string str, string substr)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (substr == null)
        throw new ArgumentNullException("substr");
      if (substr.Length == 0)
        throw ExceptionUtils.CreateArgumentMustBeNonEmptyString("substr");

      for (int count = 0, lastIndex = 0;; count++) {
        var index = str.IndexOf(substr, lastIndex);

        if (index < 0)
          return count;
        else
          lastIndex = index + substr.Length;
      }
    }

    public static string Slice(this string str, int from, int to)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (from < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("from", from);
      if (str.Length <= from)
        throw ExceptionUtils.CreateArgumentMustBeLessThan("'str.Length'", "from", from);
      if (to < from)
        throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo("'from'", "to", to);
      if (str.Length < to)
        throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo("'str.Length'", "to", to);

      return str.Substring(from, to - from);
    }

    public static int IndexOfNot(this string str, char @value)
    {
      return IndexOfNot(str, @value, 0);
    }

    public static int IndexOfNot(this string str, char @value, int startIndex)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("startIndex", startIndex);
      if (str.Length < startIndex)
        //throw new ArgumentException("startIndex + count is larger than length");
        throw new ArgumentException("startIndex is larger than length");

      for (var index = startIndex; index < str.Length; index++) {
        if (str[index] != @value)
          return index;
      }

      return -1;
    }

#if false
    public static int IndexOfNot(this string str, char @value, int startIndex, int count)
    {
      throw new NotImplementedException();
    }
#endif
  }
}
