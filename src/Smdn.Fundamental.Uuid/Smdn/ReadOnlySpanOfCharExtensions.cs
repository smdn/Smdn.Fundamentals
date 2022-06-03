// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.CompilerServices;

namespace Smdn;

internal static class ReadOnlySpanOfCharExtensions {
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static
#if SYSTEM_INT32_TRYPARSE_READONLYSPAN_OF_CHAR
  ReadOnlySpan<char>
#else
  string
#endif
  ToParseableType(this ReadOnlySpan<char> s)
#if SYSTEM_INT32_TRYPARSE_READONLYSPAN_OF_CHAR
    => s;
#else
    => StringShim.Construct(s);
#endif
}
