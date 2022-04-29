// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using Smdn.Formats.DateAndTime;

namespace Smdn.Formats;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static partial class DateTimeFormat {
  public static string GetCurrentTimeZoneOffsetString(bool delimiter)
    => DateAndTimeFormatter.FormatOffset(
      TimeZoneInfo.Local.BaseUtcOffset,
      delimiter
    );
}
