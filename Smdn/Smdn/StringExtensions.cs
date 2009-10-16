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
using System.Text;

namespace Smdn {
  public static class StringExtensions {
    public delegate string ReplaceCharEvaluator(char ch, string str, int index);
    public delegate string ReplaceStringEvaluator(string matched, string str, int index);

    private static string ReplaceToEmpty(char ch, string str, int index)
    {
      return string.Empty;
    }

    private static string ReplaceToEmpty(string matched, string str, int index)
    {
      return string.Empty;
    }

    public static string Remove(this string str, params char[] oldChars)
    {
      return Replace(str, oldChars, ReplaceToEmpty);
    }

    public static string Remove(this string str, params string[] oldValues)
    {
      return Replace(str, oldValues, ReplaceToEmpty);
    }

    public static string Replace(this string str, char[] oldChars, ReplaceCharEvaluator evaluator)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (oldChars == null)
        throw new ArgumentNullException("oldChars");
      if (evaluator == null)
        throw new ArgumentNullException("evaluator");

      foreach (var ch in oldChars) {
        var lastIndex = 0;
        var sb = new StringBuilder();

        for (;;) {
          var index = str.IndexOf(ch, lastIndex);

          if (index < 0) {
            sb.Append(str.Substring(lastIndex));
            break;
          }
          else {
            sb.Append(str.Substring(lastIndex, index - lastIndex));
            sb.Append(evaluator(ch, str, index));

            lastIndex = index + 1;
          }
        }

        str = sb.ToString();
      }

      return str;
    }

    public static string Replace(this string str, string[] oldValues, ReplaceStringEvaluator evaluator)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (oldValues == null)
        throw new ArgumentNullException("oldValues");
      if (evaluator == null)
        throw new ArgumentNullException("evaluator");

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
      if (string.IsNullOrEmpty(substr))
        throw new ArgumentException("length must be greater than 1", "substr");

      for (int count = 0, lastIndex = 0;; count++) {
        var index = str.IndexOf(substr, lastIndex);

        if (index < 0)
          return count;
        else
          lastIndex = index + substr.Length;
      }
    }
  }
}
