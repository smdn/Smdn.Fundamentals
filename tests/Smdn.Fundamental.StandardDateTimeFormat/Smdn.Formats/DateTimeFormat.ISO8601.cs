// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

using Smdn.Formats.DateAndTime;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class DateTimeFormatTests {
#pragma warning restore IDE0040
  [Test]
  public void ToISO8601DateTimeString_DateTime_ReturnValueEqualsToToW3CDateTimeString()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

    Assert.That(
      DateTimeFormat.ToISO8601DateTimeString(dtm),
      Is.EqualTo(DateTimeFormat.ToW3CDateTimeString(dtm))
    );
  }

  [Test]
  public void ToISO8601DateTimeString_DateTimeOffset_ReturnValueEqualsToToW3CDateTimeString()
  {
    var dtm = new DateTimeOffset(2008, 2, 25, 15, 1, 12, TimeSpan.FromHours(+9));

    Assert.That(
      DateTimeFormat.ToISO8601DateTimeString(dtm),
      Is.EqualTo(DateTimeFormat.ToW3CDateTimeString(dtm))
    );
  }

  [TestCaseSource(typeof(ISO8601DateTimeFormatsTests), nameof(ISO8601DateTimeFormatsTests.YieldTestCases_ParseDateTime))]
  public void FromISO8601DateTimeString(string s, DateTime expected)
    => Assert.That(DateTimeFormat.FromISO8601DateTimeString(s), Is.EqualTo(expected));

  [TestCaseSource(typeof(ISO8601DateTimeFormatsTests), nameof(ISO8601DateTimeFormatsTests.YieldTestCases_ParseDateTimeOffset))]
  public void FromISO8601DateTimeOffsetString(string s, DateTimeOffset expected)
    => Assert.That(DateTimeFormat.FromISO8601DateTimeOffsetString(s), Is.EqualTo(expected));
}
