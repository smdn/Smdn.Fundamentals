// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;

namespace Smdn {
  public static class StringReplacementExtensions {
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
  }
}
