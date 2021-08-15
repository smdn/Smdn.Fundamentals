// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn {
#if !SYSTEM_STRING_STARTSWITH_CHAR
  public static class StringShim {
    public static bool StartsWith(this string str, char @value)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));

      if (str.Length == 0)
        return false;
      else
        return str[0] == @value;
    }

    public static bool EndsWith(this string str, char @value)
    {
      if (str == null)
        throw new ArgumentNullException(nameof(str));

      if (str.Length == 0)
        return false;
      else
        return str[str.Length - 1] == @value;
    }
  }
#endif
}
