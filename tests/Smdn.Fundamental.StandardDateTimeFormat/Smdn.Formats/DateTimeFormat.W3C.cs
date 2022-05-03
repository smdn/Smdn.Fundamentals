// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

using Smdn.Formats.DateAndTime;

namespace Smdn.Formats;

partial class DateTimeFormatTests {
  [Test]
  public void ToW3CDateTimeString_DateTime_DateTimeKindUtc()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, 456, DateTimeKind.Utc);

    Assert.AreEqual(
      "2008-02-25T15:01:12.4560000Z",
      DateTimeFormat.ToW3CDateTimeString(dtm)
    );
  }

  [Test]
  public void ToW3CDateTimeString_DateTime_DateTimeKindLocal()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, 456, DateTimeKind.Local);

    Assert.AreEqual(
      "2008-02-25T15:01:12.4560000" + timezoneOffset,
      DateTimeFormat.ToW3CDateTimeString(dtm)
    );
  }

  [Test]
  public void ToW3CDateTimeString_DateTime_DateTimeKindUnspecified()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, 456, DateTimeKind.Unspecified);

    Assert.AreEqual(
      "2008-02-25T15:01:12.4560000",
      DateTimeFormat.ToW3CDateTimeString(dtm)
    );
  }

  [Test]
  [SetCulture("it-IT")] // '.' is used instead of ':'
  public void ToW3CDateTimeString_DateTime_TimeSeparator()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, 456, DateTimeKind.Utc);

    Assert.AreEqual(
      "2008-02-25T15:01:12.4560000Z",
      DateTimeFormat.ToW3CDateTimeString(dtm)
    );
  }

  [Test]
  public void ToW3CDateTimeString_DateTimeOffset()
  {
    var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, 456, DateTimeOffset.Now.Offset);

    Assert.AreEqual(
      "2008-02-25T15:01:12.4560000" + timezoneOffset,
      DateTimeFormat.ToW3CDateTimeString(dto)
    );
  }

  [Test]
  [SetCulture("it-IT")] // '.' is used instead of ':'
  public void ToW3CDateTimeString_DateTimeOffset_TimeSeparator()
  {
    var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, 456, DateTimeOffset.Now.Offset);

    Assert.AreEqual(
      "2008-02-25T15:01:12.4560000" + timezoneOffset,
      DateTimeFormat.ToW3CDateTimeString(dto)
    );
  }

  [TestCaseSource(typeof(W3CDateTimeFormatsTests), nameof(W3CDateTimeFormatsTests.YieldTestCases_ParseDateTime))]
  public void FromW3CDateTimeString(string s, DateTime expected)
    => Assert.AreEqual(expected, DateTimeFormat.FromW3CDateTimeString(s));

  [TestCaseSource(typeof(W3CDateTimeFormatsTests), nameof(W3CDateTimeFormatsTests.YieldTestCases_ParseDateTimeOffset))]
  public void FromW3CDateTimeString(string s, DateTimeOffset expected)
    => Assert.AreEqual(expected, DateTimeFormat.FromW3CDateTimeOffsetString(s));
}
