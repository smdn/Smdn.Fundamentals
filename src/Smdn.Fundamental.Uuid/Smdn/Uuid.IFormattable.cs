// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;
#if !SYSTEM_FORMATTABLESTRING
using System.Globalization;
#endif

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid :
#pragma warning restore IDE0040
  IFormattable
#if SYSTEM_ISPANFORMATTABLE
#pragma warning disable SA1001
  ,
#pragma warning restore SA1001
  ISpanFormattable
#endif
{
  public string ToString(string format)
    => ToString(format, null);

  public string ToString(string? format, IFormatProvider? formatProvider)
  {
    if (format == "X") {
      const string xopen = "{";
      const string xclose = "}";

#if SYSTEM_FORMATTABLESTRING
      return FormattableString.Invariant(
        $"{xopen}0x{time_low:x8},0x{time_mid:x4},0x{time_hi_and_version:x4},{xopen}0x{clock_seq_hi_and_reserved:x2},0x{clock_seq_low:x2},0x{node.N0:x2},0x{node.N1:x2},0x{node.N2:x2},0x{node.N3:x2},0x{node.N4:x2},0x{node.N5:x2}{xclose}{xclose}"
      );
#else
      return string.Format(
        CultureInfo.InvariantCulture,
        "{0}0x{1:x8},0x{2:x4},0x{3:x4},{0}0x{4:x2},0x{5:x2},0x{6:x2},0x{7:x2},0x{8:x2},0x{9:x2},0x{10:x2},0x{11:x2}{12}{12}",
        xopen,
        time_low,
        time_mid,
        time_hi_and_version,
        clock_seq_hi_and_reserved,
        clock_seq_low,
        node.N0,
        node.N1,
        node.N2,
        node.N3,
        node.N4,
        node.N5,
        xclose
      );
#endif
    }

    var (open, close, separator) = format switch {
      null or "" or "D" => (null, null, "-"),
      "B" => ("{", "}", "-"),
      "P" => ("(", ")", "-"),
      "N" => (null, null, null),
      _ => throw new FormatException($"invalid format: {format}"),
    };

#if SYSTEM_FORMATTABLESTRING
    return FormattableString.Invariant(
      $"{open}{time_low:x8}{separator}{time_mid:x4}{separator}{time_hi_and_version:x4}{separator}{clock_seq_hi_and_reserved:x2}{clock_seq_low:x2}{separator}{node.N0:x2}{node.N1:x2}{node.N2:x2}{node.N3:x2}{node.N4:x2}{node.N5:x2}{close}"
    );
#else
    return string.Format(
      CultureInfo.InvariantCulture,
      "{0}{3:x8}{1}{4:x4}{1}{5:x4}{1}{6:x2}{7:x2}{1}{8:x2}{9:x2}{10:x2}{11:x2}{12:x2}{13:x2}{2}",
      open,
      separator,
      close,
      time_low,
      time_mid,
      time_hi_and_version,
      clock_seq_hi_and_reserved,
      clock_seq_low,
      node.N0,
      node.N1,
      node.N2,
      node.N3,
      node.N4,
      node.N5
    );
#endif
  }

  public bool TryFormat(
    Span<char> destination,
    out int charsWritten,
    ReadOnlySpan<char> format = default,
#pragma warning disable IDE0060
    IFormatProvider? provider = null
#pragma warning restore IDE0060
  )
    => TryFormatCore(destination, out charsWritten, format) == TryFormatResult.Success;

  private enum TryFormatResult {
    Success,
    InvalidFormat,
    Failed,
  }

  private TryFormatResult TryFormatCore(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
  {
    charsWritten = default;

    const char defaultFormatChar = 'D';
    char formatChar;

    if (format.Length == 1)
      formatChar = format[0];
    else if (format.IsEmpty)
      formatChar = defaultFormatChar;
    else
      return TryFormatResult.InvalidFormat;

    // D: "00000000-0000-0000-0000-000000000000"
    // B: "{00000000-0000-0000-0000-000000000000}"
    // P: "(00000000-0000-0000-0000-000000000000)"
    // N: "00000000000000000000000000000000"
    // X: {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
    (
      bool isValidFormat,
      bool prefixedHex,
      char outerOpen,
      char outerClose,
      char outerDelimiter,
      char clockDelimiter,
      char innerOpen,
      char innerClose,
      char innerDelimiter
    ) = formatChar switch {
      'D' => (true, false, default, default, '-', default, default, default, default),
      'B' => (true, false, '{', '}', '-', default, default, default, default),
      'P' => (true, false, '(', ')', '-', default, default, default, default),
      'N' => (true, false, default, default, default, default, default, default, default),
      'X' => (true, true, '{', '}', ',', ',', '{', '}', ','),
      _ => default,
    };

    if (!isValidFormat)
      return TryFormatResult.InvalidFormat;

    if (outerOpen != default) {
      if (!TryAppendChar(ref destination, ref charsWritten, outerOpen))
        return TryFormatResult.Failed;
    }

    if (!TryFormatHex(ref destination, ref charsWritten, time_low, 4, prefixedHex, outerDelimiter))
      return TryFormatResult.Failed;
    if (!TryFormatHex(ref destination, ref charsWritten, time_mid, 2, prefixedHex, outerDelimiter))
      return TryFormatResult.Failed;
    if (!TryFormatHex(ref destination, ref charsWritten, time_hi_and_version, 2, prefixedHex, outerDelimiter))
      return TryFormatResult.Failed;

    if (innerOpen != default) {
      if (!TryAppendChar(ref destination, ref charsWritten, innerOpen))
        return TryFormatResult.Failed;
    }

    if (!TryFormatHex(ref destination, ref charsWritten, clock_seq_hi_and_reserved, 1, prefixedHex, clockDelimiter))
      return TryFormatResult.Failed;
    if (!TryFormatHex(ref destination, ref charsWritten, clock_seq_low, 1, prefixedHex, outerDelimiter))
      return TryFormatResult.Failed;

    if (!TryFormatHex(ref destination, ref charsWritten, node.N0, 1, prefixedHex, innerDelimiter))
      return TryFormatResult.Failed;
    if (!TryFormatHex(ref destination, ref charsWritten, node.N1, 1, prefixedHex, innerDelimiter))
      return TryFormatResult.Failed;
    if (!TryFormatHex(ref destination, ref charsWritten, node.N2, 1, prefixedHex, innerDelimiter))
      return TryFormatResult.Failed;
    if (!TryFormatHex(ref destination, ref charsWritten, node.N3, 1, prefixedHex, innerDelimiter))
      return TryFormatResult.Failed;
    if (!TryFormatHex(ref destination, ref charsWritten, node.N4, 1, prefixedHex, innerDelimiter))
      return TryFormatResult.Failed;
    if (!TryFormatHex(ref destination, ref charsWritten, node.N5, 1, prefixedHex, delimiter: default))
      return TryFormatResult.Failed;

    if (innerClose != default) {
      if (!TryAppendChar(ref destination, ref charsWritten, innerClose))
        return TryFormatResult.Failed;
    }

    if (outerClose != default) {
      if (!TryAppendChar(ref destination, ref charsWritten, outerClose))
        return TryFormatResult.Failed;
    }

    return TryFormatResult.Success;

    static bool TryAppendChar(ref Span<char> destination, ref int charsWritten, char ch)
    {
      if (destination.Length < 1)
        return false;

      destination[0] = ch;
      destination = destination.Slice(1);

      charsWritten += 1;

      return true;
    }

    static bool TryFormatHex(
      ref Span<char> destination,
      ref int charsWritten,
      uint value,
      int valueSize,
      bool prefixedHex,
      char delimiter
    )
    {
      if (prefixedHex) {
        if (destination.Length < 2)
          return false;

        destination[0] = '0';
        destination[1] = 'x';
        destination = destination.Slice(2);
        charsWritten += 2;
      }

      var valueFormat = valueSize switch {
        1 => "x2",
        2 => "x4",
        4 => "x8",
        _ => throw new NotSupportedException($"undefined value size: {valueSize}"),
      };

#if SYSTEM_ISPANFORMATTABLE
      if (!value.TryFormat(destination, out var written, valueFormat, provider: null))
        return false;
#else
      var formattedValue = value.ToString(valueFormat, provider: null);

      if (destination.Length < formattedValue.Length)
        return false;

      var written = 0;

      for (var i = 0; i < formattedValue.Length; i++) {
        destination[i] = formattedValue[i];
        written++;
      }
#endif

      destination = destination.Slice(written);
      charsWritten += written;

      if (delimiter != default) {
        if (destination.Length < 1)
          return false;

        destination[0] = delimiter;
        destination = destination.Slice(1);
        charsWritten += 1;
      }

      return true;
    }
  }
}
