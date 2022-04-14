// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.CompilerServices;

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

  /*
   * SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
   */
#if SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
  public static string Construct(ReadOnlySpan<char> s)
  {
    if (s.IsEmpty)
      return string.Empty;

#if SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
    return new(s);
#else
    unsafe {
      fixed (char* sequence = s) {
        return new(sequence, 0, s.Length);
      }
    }
#endif
  }
}
