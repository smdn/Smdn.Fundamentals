// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

using Smdn.Formats.DateAndTime;

namespace Smdn.Formats;

partial class DateTimeFormatTests {
  [Test]
  public void ToISO8601DateTimeString_DateTime_ReturnValueEqualsToToW3CDateTimeString()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

    Assert.AreEqual(
      DateTimeFormat.ToW3CDateTimeString(dtm),
      DateTimeFormat.ToISO8601DateTimeString(dtm)
    );
  }

  [Test]
  public void ToISO8601DateTimeString_DateTimeOffset_ReturnValueEqualsToToW3CDateTimeString()
  {
    var dtm = new DateTimeOffset(2008, 2, 25, 15, 1, 12, TimeSpan.FromHours(+9));

    Assert.AreEqual(
      DateTimeFormat.ToW3CDateTimeString(dtm),
      DateTimeFormat.ToISO8601DateTimeString(dtm)
    );
  }

  [TestCaseSource(typeof(ISO8601DateTimeFormatsTests), nameof(ISO8601DateTimeFormatsTests.YieldTestCases_ParseDateTime))]
  public void FromISO8601DateTimeString(string s, DateTime expected)
    => Assert.AreEqual(expected, DateTimeFormat.FromISO8601DateTimeString(s));

  [TestCaseSource(typeof(ISO8601DateTimeFormatsTests), nameof(ISO8601DateTimeFormatsTests.YieldTestCases_ParseDateTimeOffset))]
  public void FromISO8601DateTimeOffsetString(string s, DateTimeOffset expected)
    => Assert.AreEqual(expected, DateTimeFormat.FromISO8601DateTimeOffsetString(s));
}
