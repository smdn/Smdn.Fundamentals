// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn.Formats;

[TestFixture()]
public partial class DateTimeFormatTests {
  private string timezoneOffset = string.Empty;
  private string timezoneOffsetNoDelim = string.Empty;

  [SetUp]
  public void Setup()
  {
    var offset = DateTimeOffset.Now.Offset;
    var hh = offset.Hours;
    var mm = offset.Minutes;

    if (TimeSpan.Zero <= offset) {
      timezoneOffset        = $"+{hh:d2}:{mm:d2}";
      timezoneOffsetNoDelim = $"+{hh:d2}{mm:d2}";
    }
    else {
      timezoneOffset        = $"-{hh:d2}:{mm:d2}";
      timezoneOffsetNoDelim = $"-{hh:d2}{mm:d2}";
    }
  }

  [Test]
  public void GetCurrentTimeZoneOffsetString()
  {
    Assert.AreEqual(timezoneOffset, DateTimeFormat.GetCurrentTimeZoneOffsetString(true));
    Assert.AreEqual(timezoneOffsetNoDelim, DateTimeFormat.GetCurrentTimeZoneOffsetString(false));
  }
}
