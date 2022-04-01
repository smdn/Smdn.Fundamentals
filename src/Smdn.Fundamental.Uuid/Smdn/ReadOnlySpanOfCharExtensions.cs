// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.CompilerServices;

namespace Smdn;

internal static class ReadOnlySpanOfCharExtensions {
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static
#if SYSTEM_INUMBER_TRYPARSE_READONLYSPAN_OF_CHAR
  ReadOnlySpan<char>
#else
  string
#endif
  ToParseableType(this ReadOnlySpan<char> s)
#if SYSTEM_INUMBER_TRYPARSE_READONLYSPAN_OF_CHAR
    => s;
#elif SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
    => new(s);
#else
    => new(s.ToArray());
#endif
}
