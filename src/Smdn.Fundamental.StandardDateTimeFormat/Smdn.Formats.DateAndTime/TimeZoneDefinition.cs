// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats.DateAndTime;

internal abstract class TimeZoneDefinition {
  public string Suffix { get; }
  public virtual bool IsUniversal => false;
  public abstract DateTime AdjustToTimeZone(DateTime dateAndTime);
  public abstract DateTimeOffset AdjustToTimeZone(DateTimeOffset dateAndTime);

  protected TimeZoneDefinition(string suffix)
  {
    Suffix = suffix;
  }
}
