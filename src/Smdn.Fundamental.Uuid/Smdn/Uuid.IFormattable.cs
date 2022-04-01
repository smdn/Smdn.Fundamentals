// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if !SYSTEM_FORMATTABLESTRING
using System.Globalization;
#endif

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid :
#pragma warning restore IDE0040
  IFormattable
{
  public string ToString(string format)
    => ToString(format, null);

  public string ToString(string format, IFormatProvider formatProvider)
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
}
