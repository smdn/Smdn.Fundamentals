// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

using NUnit.Framework;

namespace Smdn.Formats;

[TestFixture()]
public class DateTimeFormatTests {
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

  [Test]
  public void ToRFC822DateTimeString_DateTime_DateTimeKindUtc()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

    Assert.AreEqual(
      "Mon, 25 Feb 2008 15:01:12 GMT",
      DateTimeFormat.ToRFC822DateTimeString(dtm)
    );
  }

  [Test]
  public void ToRFC822DateTimeString_DateTime_DateTimeKindLocal()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Local);

    Assert.AreEqual(
      "Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
      DateTimeFormat.ToRFC822DateTimeString(dtm)
    );
  }

  [Test]
  public void ToRFC822DateTimeString_DateTime_DateTimeKindUnspecified()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Unspecified);

    Assert.AreEqual(
      "Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
      DateTimeFormat.ToRFC822DateTimeString(dtm)
    );
  }

  [Test]
  public void ToRFC822DateTimeString_DateTimeOffset()
  {
    var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, DateTimeOffset.Now.Offset);

    Assert.AreEqual(
      "Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
      DateTimeFormat.ToRFC822DateTimeString(dto)
    );
  }

  private static IEnumerable YieldTestCases_FromRFC822DateTimeString_Utc()
  {
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01.1234567 GMT", new DateTime(2003, 6, 10, 9, 41, 1, 123, DateTimeKind.Utc).AddTicks(4567) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01.123 GMT", new DateTime(2003, 6, 10, 9, 41, 1, 123, DateTimeKind.Utc) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01 GMT", new DateTime(2003, 6, 10, 9, 41, 1, DateTimeKind.Utc) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41 GMT", new DateTime(2003, 6, 10, 9, 41, 0, DateTimeKind.Utc) };
  }

  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeString_Utc))]
  public void FromRFC822DateTimeString_Utc(string s, DateTime expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromRFC822DateTimeString(s)
    );

  private static IEnumerable YieldTestCases_FromRFC822DateTimeString_Local()
  {
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01.1234567 +0900", new DateTime(2003, 6, 10, 0, 41, 1, 123, DateTimeKind.Local).AddTicks(4567) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01.1234567 -0400", new DateTime(2003, 6, 10, 13, 41, 1, 123, DateTimeKind.Local).AddTicks(4567) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01.1234567 +1245", new DateTime(2003, 6,  9, 20, 56, 1, 123, DateTimeKind.Local).AddTicks(4567) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01.123 +0900", new DateTime(2003, 6, 10, 0, 41, 1, 123, DateTimeKind.Local) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01 +0900", new DateTime(2003, 6, 10, 0, 41, 1, DateTimeKind.Local) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41 +0900", new DateTime(2003, 6, 10, 0, 41, 0, DateTimeKind.Local) };
  }

  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeString_Local))]
  public void FromRFC822DateTimeString_Local(string s, DateTime expected)
  {
    var actual = DateTimeFormat.FromRFC822DateTimeString(s);

    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(actual),
      actual
    );
  }

  private static IEnumerable YieldTestCases_FromRFC822DateTimeOffsetString()
  {
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01.1234567 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)).AddTicks(4567) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01.1234567 -0400", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(-4)).AddTicks(4567) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01.1234567 +1245", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(12) + TimeSpan.FromMinutes(45)).AddTicks(4567) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01.123 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41:01 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, TimeSpan.FromHours(+9)) };
    yield return new object[] { "Tue, 10 Jun 2003 09:41 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 0, TimeSpan.FromHours(+9)) };
  }

  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeOffsetString))/*, Ignore("Mono Bug #547675")*/]
  public void FromRFC822DateTimeOffsetString(string s, DateTimeOffset expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromRFC822DateTimeOffsetString(s)
    );

  private static IEnumerable YieldTestCases_FromRFC822DateTimeOffsetString_Gmt()
  {
    yield return new object[] { "Fri, 13 Apr 2001 19:23:02.1234567 GMT", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.FromHours(0)).AddTicks(4567) };
    yield return new object[] { "Fri, 13 Apr 2001 19:23:02.123 GMT", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.FromHours(0)) };
    yield return new object[] { "Fri, 13 Apr 2001 19:23:02 GMT", new DateTimeOffset(2001, 4, 13, 19, 23, 2, TimeSpan.FromHours(0)) };
    yield return new object[] { "Fri, 13 Apr 2001 19:23 GMT", new DateTimeOffset(2001, 4, 13, 19, 23, 0, TimeSpan.FromHours(0)) };
  }

  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeOffsetString_Gmt))]
  public void FromRFC822DateTimeOffsetString_Gmt(string s, DateTimeOffset expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromRFC822DateTimeOffsetString(s)
    );

  [Test]
  public void ToISO8601DateTimeString_DateTime_DateTimeKindUtc()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

    Assert.AreEqual(
      DateTimeFormat.ToW3CDateTimeString(dtm),
      DateTimeFormat.ToISO8601DateTimeString(dtm)
    );
  }

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
  public void ToW3CDateTimeString_DateTimeOffset()
  {
    var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, 456, DateTimeOffset.Now.Offset);

    Assert.AreEqual(
      "2008-02-25T15:01:12.4560000" + timezoneOffset,
      DateTimeFormat.ToW3CDateTimeString(dto)
    );
  }

  [TestCase("2008-04-11T12:34:56.7893333Z")]
  [TestCase("2008-04-11T12:34:56.789Z")]
  [TestCase("2008-04-11T12:34:56Z")]
  [TestCase("2008-04-11T12:34Z")]

  public void FromISO8601DateTimeString_ReturnValueEqualsToFromW3CDateTimeString(string s)
    => Assert.AreEqual(
      DateTimeFormat.FromW3CDateTimeString(s),
      DateTimeFormat.FromISO8601DateTimeString(s)
    );

  private static IEnumerable YieldTestCases_FromW3CDateTimeString_Utc()
  {
    yield return new object[] { "2008-04-11T12:34:56.7893333Z", new DateTime(2008, 4, 11, 12, 34, 56, 789, DateTimeKind.Utc).AddTicks(3333) };
    yield return new object[] { "2008-04-11T12:34:56.789Z", new DateTime(2008, 4, 11, 12, 34, 56, 789, DateTimeKind.Utc) };
    yield return new object[] { "2008-04-11T12:34:56Z", new DateTime(2008, 4, 11, 12, 34, 56, DateTimeKind.Utc) };
    yield return new object[] { "2008-04-11T12:34Z", new DateTime(2008, 4, 11, 12, 34, 0, DateTimeKind.Utc) };
  }

  [TestCaseSource(nameof(YieldTestCases_FromW3CDateTimeString_Utc))]
  public void FromW3CDateTimeString_Utc(string s, DateTime expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromW3CDateTimeString(s)
    );

  private static IEnumerable YieldTestCases_FromW3CDateTimeString_Local()
  {
    yield return new object[] { "2008-04-11T12:34:56.7893333 +09:00", new DateTime(2008, 4, 11, 3, 34, 56, 789, DateTimeKind.Local).AddTicks(3333) };
    yield return new object[] { "2008-04-11T12:34:56.7893333 -04:00", new DateTime(2008, 4, 11, 16, 34, 56, 789, DateTimeKind.Local).AddTicks(3333) };
    yield return new object[] { "2008-04-11T12:34:56.7893333 +12:45", new DateTime(2008, 4, 10, 23, 49, 56, 789, DateTimeKind.Local).AddTicks(3333) };
    yield return new object[] { "2008-04-11T12:34:56.789 +09:00", new DateTime(2008, 4, 11, 3, 34, 56, 789, DateTimeKind.Local) };
    yield return new object[] { "2008-04-11T12:34:56 +09:00", new DateTime(2008, 4, 11, 3, 34, 56, DateTimeKind.Local) };
    yield return new object[] { "2008-04-11T12:34 +09:00", new DateTime(2008, 4, 11, 3, 34, 0, DateTimeKind.Local) };
  }

  [TestCaseSource(nameof(YieldTestCases_FromW3CDateTimeString_Local))]
  public void FromW3CDateTimeString_Local(string s, DateTime expected)
  {
    var actual = DateTimeFormat.FromW3CDateTimeString(s);

    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(actual),
      actual
    );
  }

  private static IEnumerable YieldTestCases_FromW3CDateTimeOffsetString()
  {
    yield return new object[] { "2008-04-11T12:34:56.7893333 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)).AddTicks(3333) };
    yield return new object[] { "2008-04-11T12:34:56.7893333 -04:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(-4)).AddTicks(3333) };
    yield return new object[] { "2008-04-11T12:34:56.7893333 +12:45", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(12) + TimeSpan.FromMinutes(45)).AddTicks(3333) };
    yield return new object[] { "2008-04-11T12:34:56.789 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)) };
    yield return new object[] { "2008-04-11T12:34:56 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, TimeSpan.FromHours(+9)) };
    yield return new object[] { "2008-04-11T12:34 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 0, TimeSpan.FromHours(+9)) };
  }

  [TestCaseSource(nameof(YieldTestCases_FromW3CDateTimeOffsetString))]
  public void FromW3CDateTimeOffsetString(string s, DateTimeOffset expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromW3CDateTimeOffsetString(s)
    );
}
