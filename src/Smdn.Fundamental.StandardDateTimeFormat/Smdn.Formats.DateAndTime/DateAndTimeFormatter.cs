// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Globalization;

namespace Smdn.Formats.DateAndTime;

public static class DateAndTimeFormatter {
  public static string FormatOffset(TimeSpan offset, bool delimiter)
  {
#if SYSTEM_READONLYSPAN && SYSTEM_INT32_TRYFORMAT && SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
    // "+0000"/"+00:00"
    Span<char> buffer = stackalloc char[5 + (delimiter ? 1 : 0)];
    var buf = buffer;

    // sign
    buf[0] = (TimeSpan.Zero <= offset) ? '+' : '-';
    buf = buf.Slice(1);

    // hours
    var duration = offset.Duration();

    duration.Hours.TryFormat(buf, out _, "D2", CultureInfo.InvariantCulture.NumberFormat);
    buf = buf.Slice(2);

    // delimiter
    if (delimiter) {
      buf[0] = ':';
      buf = buf.Slice(1);
    }

    // minutes
    duration.Minutes.TryFormat(buf, out _, "D2", CultureInfo.InvariantCulture.NumberFormat);

    return new string(buffer);
#else // SYSTEM_READONLYSPAN
    var format = TimeSpan.Zero <= offset
      ? delimiter
        ? "'+'hh':'mm"
        : "'+'hhmm"
      : delimiter
        ? "'-'hh':'mm"
        : "'-'hhmm";

    return offset.Duration().ToString(format, CultureInfo.InvariantCulture.NumberFormat);
#endif
  }
}
