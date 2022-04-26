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

    if (TimeSpan.Zero <= offset) {
      timezoneOffset        = string.Format("+{0:d2}:{1:d2}", offset.Hours, offset.Minutes);
      timezoneOffsetNoDelim = string.Format("+{0:d2}{1:d2}",  offset.Hours, offset.Minutes);
    }
    else {
      timezoneOffset        = string.Format("-{0:d2}:{1:d2}", offset.Hours, offset.Minutes);
      timezoneOffsetNoDelim = string.Format("-{0:d2}{1:d2}",  offset.Hours, offset.Minutes);
    }
  }

  [Test]
  public void GetCurrentTimeZoneOffsetString()
  {
    Assert.AreEqual(timezoneOffset, DateTimeFormat.GetCurrentTimeZoneOffsetString(true));
    Assert.AreEqual(timezoneOffsetNoDelim, DateTimeFormat.GetCurrentTimeZoneOffsetString(false));
  }
}
