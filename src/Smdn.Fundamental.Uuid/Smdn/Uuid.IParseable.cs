// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Globalization;

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid
#pragma warning restore IDE0040
#if FEATURE_GENERIC_MATH
  :
  IParseable<Uuid>,
  ISpanParseable<Uuid>
#endif
{
  private enum TryParseResult {
    Success,
    FormatErrorOnTimeLow,
    FormatErrorOnTimeMid,
    FormatErrorOnTimeHiAndVersion,
    FormatErrorOnClockSeqHiAndReserved,
    FormatErrorOnClockSeqLow,
    FormatErrorOnNode,
    FormatError,
  }

  public static Uuid Parse(string s, IFormatProvider? provider = null)
    => Parse((s ?? throw new ArgumentNullException(nameof(s))).AsSpan(), provider);

  public static bool TryParse(string? s, IFormatProvider? provider, out Uuid result)
    => TryParse((s ?? throw new ArgumentNullException(nameof(s))).AsSpan(), provider, out result);

  public static Uuid Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    => TryParseCore(s, provider, out var result) switch {
      TryParseResult.Success => result,
      TryParseResult.FormatErrorOnTimeLow => throw new FormatException($"invalid time_low value: {s.ToString()}"),
      TryParseResult.FormatErrorOnTimeMid => throw new FormatException($"invalid time_mid value: {s.ToString()}"),
      TryParseResult.FormatErrorOnTimeHiAndVersion => throw new FormatException($"invalid time_hi_and_version value: {s.ToString()}"),
      TryParseResult.FormatErrorOnClockSeqHiAndReserved => throw new FormatException($"invalid clock_seq_hi_and_reserved value: {s.ToString()}"),
      TryParseResult.FormatErrorOnClockSeqLow => throw new FormatException($"invalid clock_seq_low value: {s.ToString()}"),
      TryParseResult.FormatErrorOnNode => throw new FormatException($"invalid node value: {s.ToString()}"),
      _ /* TryParseResult.FormatError */ => throw new FormatException($"invalid UUID: {s.ToString()}"),
    };

  public static bool TryParse(ReadOnlySpan<char> s, out Uuid result)
    => TryParseCore(s, provider: null, out result) == TryParseResult.Success;

  public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Uuid result)
    => TryParseCore(s, provider, out result) == TryParseResult.Success;

  /// <summary>Attempts to parse UUID string that must to be in format of "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx".</summary>
#pragma warning disable IDE0060
  private static TryParseResult TryParseCore(ReadOnlySpan<char> s, IFormatProvider? provider, out Uuid result)
#pragma warning restore IDE0060
  {
    static bool TryGetSpanUpToNextDelimiter(ref ReadOnlySpan<char> span, char delimiter, out ReadOnlySpan<char> result)
    {
      result = default;

      var index = span.IndexOf(delimiter);

      if (0 <= index) {
        result = span.Slice(0, index);
        span = index == span.Length - 1 ? default : span.Slice(index + 1);
        return true;
      }

      return false;
    }

    result = default;

    if (s.Length != 36)
      return TryParseResult.FormatError;

    const char delimiter = '-';

    if (!TryGetSpanUpToNextDelimiter(ref s, delimiter, out var fieldTimeLow))
      return TryParseResult.FormatError;
    if (!(fieldTimeLow.Length == 8 && uint.TryParse(fieldTimeLow.ToParseableType(), NumberStyles.HexNumber, provider: null, out var fieldValueTimeLow)))
      return TryParseResult.FormatErrorOnTimeLow;

    if (!TryGetSpanUpToNextDelimiter(ref s, delimiter, out var fieldTimeMid))
      return TryParseResult.FormatError;
    if (!(fieldTimeMid.Length == 4 && ushort.TryParse(fieldTimeMid.ToParseableType(), NumberStyles.HexNumber, provider: null, out var fieldValueTimeMid)))
      return TryParseResult.FormatErrorOnTimeMid;

    if (!TryGetSpanUpToNextDelimiter(ref s, delimiter, out var fieldTimeHighAndVersion))
      return TryParseResult.FormatError;
    if (!(fieldTimeHighAndVersion.Length == 4 && ushort.TryParse(fieldTimeHighAndVersion.ToParseableType(), NumberStyles.HexNumber, provider: null, out var fieldValueTimeHiAndVersion)))
      return TryParseResult.FormatErrorOnTimeHiAndVersion;

    if (!TryGetSpanUpToNextDelimiter(ref s, delimiter, out var fieldClockSeq))
      return TryParseResult.FormatError;
    if (fieldClockSeq.Length != 4)
      return TryParseResult.FormatErrorOnClockSeqHiAndReserved;
    if (!byte.TryParse(fieldClockSeq.Slice(0, 2).ToParseableType(), NumberStyles.HexNumber, null, out var fieldValueClockSeqHiAndReserved))
      return TryParseResult.FormatErrorOnClockSeqHiAndReserved;
    if (!byte.TryParse(fieldClockSeq.Slice(2, 2).ToParseableType(), NumberStyles.HexNumber, null, out var fieldValueClockSeqLow))
      return TryParseResult.FormatErrorOnClockSeqLow;

    if (
      !(
        byte.TryParse(s.Slice(0, 2).ToParseableType(), NumberStyles.HexNumber, null, out var n0) &&
        byte.TryParse(s.Slice(2, 2).ToParseableType(), NumberStyles.HexNumber, null, out var n1) &&
        byte.TryParse(s.Slice(4, 2).ToParseableType(), NumberStyles.HexNumber, null, out var n2) &&
        byte.TryParse(s.Slice(6, 2).ToParseableType(), NumberStyles.HexNumber, null, out var n3) &&
        byte.TryParse(s.Slice(8, 2).ToParseableType(), NumberStyles.HexNumber, null, out var n4) &&
        byte.TryParse(s.Slice(10, 2).ToParseableType(), NumberStyles.HexNumber, null, out var n5)
      )
    ) {
      return TryParseResult.FormatErrorOnNode;
    }

    result = new(
      fieldValueTimeLow,
      fieldValueTimeMid,
      fieldValueTimeHiAndVersion,
      fieldValueClockSeqHiAndReserved,
      fieldValueClockSeqLow,
      n0,
      n1,
      n2,
      n3,
      n4,
      n5
    );

    return TryParseResult.Success;
  }
}
