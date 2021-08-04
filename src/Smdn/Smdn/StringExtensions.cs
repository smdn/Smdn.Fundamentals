// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
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
        throw new ArgumentNullException(nameof(evaluator));

      return ReplaceInternal(str, oldChars, evaluator);
    }

    private static string ReplaceInternal(string str, char[] oldChars, ReplaceCharEvaluator evaluator)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));
      if (oldChars == null)
        throw new ArgumentNullException(nameof(oldChars));

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
        throw new ArgumentNullException(nameof(evaluator));

      return ReplaceInternal(str, oldValues, evaluator);
    }

    private static string ReplaceInternal(string str, string[] oldValues, ReplaceStringEvaluator evaluator)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));
      if (oldValues == null)
        throw new ArgumentNullException(nameof(oldValues));

      foreach (var oldValue in oldValues) {
        var lastIndex = 0;
        var sb = new StringBuilder();

        if (oldValue.Length == 0)
          continue;

        for (;;) {
          var index = str.IndexOf(oldValue, lastIndex, StringComparison.Ordinal);

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

    // #if !(NETCOREAPP2_0 || NETCOREAPP2_1)
    public static bool StartsWith(this string str, char @value)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));

      if (str.Length == 0)
        return false;
      else
        return str[0] == @value;
    }

    // #if !(NETCOREAPP2_0 || NETCOREAPP2_1)
    public static bool EndsWith(this string str, char @value)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));

      if (str.Length == 0)
        return false;
      else
        return str[str.Length - 1] == @value;
    }

    public static int Count(this string str, char c)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));

      var count = 0;

      foreach (var ch in str) {
        if (ch == c)
          count++;
      }

      return count;
    }

    public static int Count(this string str, string substr)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));
      if (substr == null)
        throw new ArgumentNullException(nameof(substr));
      if (substr.Length == 0)
        throw ExceptionUtils.CreateArgumentMustBeNonEmptyString(nameof(substr));

      for (int count = 0, lastIndex = 0;; count++) {
        var index = str.IndexOf(substr, lastIndex, StringComparison.Ordinal);

        if (index < 0)
          return count;
        else
          lastIndex = index + substr.Length;
      }
    }

    public static string Slice(this string str, int from, int to)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));
      if (from < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(from), from);
      if (str.Length <= from)
        throw ExceptionUtils.CreateArgumentMustBeLessThan(nameof(str.Length), nameof(from), from);
      if (to < from)
        throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(nameof(from), nameof(to), to);
      if (str.Length < to)
        throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(nameof(str.Length), nameof(to), to);

      return str.Substring(from, to - from);
    }

    public static int IndexOfNot(this string str, char @value)
    {
      return IndexOfNot(str, @value, 0);
    }

    public static int IndexOfNot(this string str, char @value, int startIndex)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
      if (str.Length < startIndex)
        //throw new ArgumentException("startIndex + count is larger than length");
        throw new ArgumentException("startIndex is larger than length");

      for (var index = startIndex; index < str.Length; index++) {
        if (str[index] != @value)
          return index;
      }

      return -1;
    }
  }
}
