// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text.RegularExpressions;

namespace Smdn.Text.RegularExpressions {
  public static class RegexExtensions {
    public static bool IsMatch(this Regex regex, string input, out Match match)
      => IsMatch(regex, input, startIndex: 0, out match);

    public static bool IsMatch(this Regex regex, string input, int startIndex, out Match match)
    {
      if (regex == null)
        throw new ArgumentNullException(nameof(regex));

      match = regex.Match(input, startIndex);

      return match.Success;
    }
  }
}
