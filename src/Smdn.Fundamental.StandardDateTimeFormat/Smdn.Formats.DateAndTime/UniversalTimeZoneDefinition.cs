// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats.DateAndTime;

internal class UniversalTimeZoneDefinition : TimeZoneDefinition {
  public override bool IsUniversal => true;

  public UniversalTimeZoneDefinition(string suffix)
    : base(suffix)
  {
  }

  public override DateTime AdjustToTimeZone(DateTime dateAndTime) => dateAndTime; // do nothing
  public override DateTimeOffset AdjustToTimeZone(DateTimeOffset dateAndTime) => dateAndTime; // do nothing
}
