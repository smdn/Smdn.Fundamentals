// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;

namespace Smdn {
  public static class StringExtensions {
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
