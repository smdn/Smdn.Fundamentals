// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.Globalization;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n {
#pragma warning restore IDE0040
  // IParsable<TSelf>.Parse(String, IFormatProvider)
  public static TUInt24n Parse(string s, IFormatProvider? provider = null)
    => Parse(s, NumberStyles.Integer, provider);

  // INumber<TSelf>.Parse(String, NumberStyles, IFormatProvider)
  public static TUInt24n Parse(string s, NumberStyles style, IFormatProvider? provider = null)
    => new(TUIntWide.Parse(s, style, provider), check: true);

#if SYSTEM_ISPANPARSABLE
  static TUInt24n ISpanParsable<TUInt24n>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    => Parse(s, NumberStyles.Integer, provider);
#endif

#if SYSTEM_INT32_PARSE_READONLYSPAN_OF_CHAR
  // INumber<TSelf>.Parse(ReadOnlySpan<Char>, NumberStyles, IFormatProvider)
  public static TUInt24n Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    => new(TUIntWide.Parse(s, style, provider), check: true);
#endif

  public static bool TryParse(
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)] string? s,
#else
    string? s,
#endif
    out TUInt24n result
  )
    => TryParse(s, NumberStyles.Integer, provider: null, out result);

  // IParsable<TSelf>.TryParse(String, IFormatProvider, TSelf)
  public static bool TryParse(
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)] string? s,
#else
    string? s,
#endif
    IFormatProvider? provider,
    out TUInt24n result
  )
    => TryParse(s, NumberStyles.Integer, provider, out result);

  // INumber<TSelf>.TryParse(ReadOnlySpan<char>, NumberStyles, IFormatProvider?, out TSelf)
  public static bool TryParse(
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)] string? s,
#else
    string? s,
#endif
    NumberStyles style,
    IFormatProvider? provider,
    out TUInt24n result
  )
  {
    result = default;

    if (!TUIntWide.TryParse(s, style, provider, out var resultUIntWide))
      return false;

    if (maxValue < resultUIntWide)
      return false; // overflow

    result = new(resultUIntWide, check: false);

    return true;
  }

#if SYSTEM_INT32_TRYPARSE_READONLYSPAN_OF_CHAR
  public static bool TryParse(ReadOnlySpan<char> s, out TUInt24n result)
    => TryParse(s, NumberStyles.Integer, provider: null, out result);

  // ISpanParsable<TSelf>.TryParse(ReadOnlySpan<Char>, IFormatProvider, TSelf)
  public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TUInt24n result)
    => TryParse(s, NumberStyles.Integer, provider, out result);

  // INumber<TSelf>.TryParse(ReadOnlySpan<char>, NumberStyles, IFormatProvider?, out TSelf)
  public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out TUInt24n result)
  {
    result = default;

    if (!TUIntWide.TryParse(s, style, provider, out var resultUIntWide))
      return false;

    if (maxValue < resultUIntWide)
      return false; // overflow

    result = new(resultUIntWide, check: false);

    return true;
  }
#endif
}
