// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

public static class StringShim {
  /*
   * SYSTEM_STRING_STARTSWITH_CHAR
   */
  public static bool StartsWith(this string str, char @value)
  {
    if (str == null)
      throw new ArgumentNullException(nameof(str));

    if (str.Length == 0)
      return false;
    else
      return str[0] == @value;
  }

  /*
   * SYSTEM_STRING_ENDSWITH_CHAR
   */
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
