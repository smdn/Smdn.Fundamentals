// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

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

  [TestCase("2008-04-11T12:34:56.7893333Z")]
  [TestCase("2008-04-11T12:34:56.789Z")]
  [TestCase("2008-04-11T12:34:56Z")]
  [TestCase("2008-04-11T12:34Z")]

  public void FromISO8601DateTimeString_DateTime_ReturnValueEqualsToFromW3CDateTimeString(string s)
    => Assert.AreEqual(
      DateTimeFormat.FromW3CDateTimeString(s),
      DateTimeFormat.FromISO8601DateTimeString(s)
    );

  [TestCase("2008-04-11T12:34:56.7893333 +09:00")]
  [TestCase("2008-04-11T12:34:56.7893333 -04:00")]
  [TestCase("2008-04-11T12:34:56.7893333 -12:45")]
  [TestCase("2008-04-11T12:34:56.789 +09:00")]
  [TestCase("2008-04-11T12:34:56 +09:00")]
  [TestCase("2008-04-11T12:34 +09:00")]
  public void FromISO8601DateTimeString_DateTimeOffset_ReturnValueEqualsToFromW3CDateTimeString(string s)
    => Assert.AreEqual(
      DateTimeFormat.FromW3CDateTimeString(s),
      DateTimeFormat.FromISO8601DateTimeString(s)
    );
}
