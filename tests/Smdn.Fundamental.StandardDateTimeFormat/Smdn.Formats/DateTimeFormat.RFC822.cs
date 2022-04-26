// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

using NUnit.Framework;

namespace Smdn.Formats;

partial class DateTimeFormatTests {
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
  [SetCulture("it-IT")] // '.' is used instead of ':'
  public void ToRFC822DateTimeString_DateTime_TimeSeparator()
  {
    var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

    Assert.AreEqual(
      "Mon, 25 Feb 2008 15:01:12 GMT",
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

  [Test]
  [SetCulture("it-IT")] // '.' is used instead of ':'
  public void ToRFC822DateTimeString_DateTimeOffset_TimeSeparator()
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

  [SetCulture("it-IT")] // '.' is used instead of ':'
  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeOffsetString))]
  public void FromRFC822DateTimeOffsetString_TimeSeparator(string s, DateTimeOffset expected)
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
}
