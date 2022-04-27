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
      "Mon, 25 Feb 2008 15:01:12 +0000",
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
      "Mon, 25 Feb 2008 15:01:12 +0000",
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

  private static IEnumerable YieldTestCases_FromRFC822DateTimeString_UniversalTime()
  {
    foreach (var dayOfWeek in new[] { "Tue, ", string.Empty }) {
      foreach (var year in new[] { "2003", "03" }) {
        foreach (var zone in new[] { "GMT", "UT" }) {
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 09:41:01.1234567 {zone}", new DateTime(2003, 6, 10, 9, 41, 1, 123, DateTimeKind.Utc).AddTicks(4567) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 09:41:01.123456 {zone}", new DateTime(2003, 6, 10, 9, 41, 1, 123, DateTimeKind.Utc).AddTicks(4560) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 09:41:01.12345 {zone}", new DateTime(2003, 6, 10, 9, 41, 1, 123, DateTimeKind.Utc).AddTicks(4500) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 09:41:01.1234 {zone}", new DateTime(2003, 6, 10, 9, 41, 1, 123, DateTimeKind.Utc).AddTicks(4000) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 09:41:01.123 {zone}", new DateTime(2003, 6, 10, 9, 41, 1, 123, DateTimeKind.Utc) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 09:41:01.12 {zone}", new DateTime(2003, 6, 10, 9, 41, 1, 120, DateTimeKind.Utc) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 09:41:01.1 {zone}", new DateTime(2003, 6, 10, 9, 41, 1, 100, DateTimeKind.Utc) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 09:41:01 {zone}", new DateTime(2003, 6, 10, 9, 41, 1, DateTimeKind.Utc) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 09:41:1 {zone}", new DateTime(2003, 6, 10, 9, 41, 1, DateTimeKind.Utc) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 09:41 {zone}", new DateTime(2003, 6, 10, 9, 41, 0, DateTimeKind.Utc) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 09:4 {zone}", new DateTime(2003, 6, 10, 9, 4, 0, DateTimeKind.Utc) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 9:04 {zone}", new DateTime(2003, 6, 10, 9, 4, 0, DateTimeKind.Utc) };
          yield return new object[] { $"{dayOfWeek}10 Jun {year} 9:4 {zone}", new DateTime(2003, 6, 10, 9, 4, 0, DateTimeKind.Utc) };
        }
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeString_UniversalTime))]
  public void FromRFC822DateTimeString_UniversalTime(string s, DateTime expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromRFC822DateTimeString(s),
      s
    );

  private static IEnumerable YieldTestCases_FromRFC822DateTimeString_Local()
  {
    foreach (var dayOfWeek in new[] { "Tue, ", string.Empty }) {
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.1234567 +0900", new DateTime(2003, 6, 10, 0, 41, 1, 123, DateTimeKind.Local).AddTicks(4567) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.123456 +0900", new DateTime(2003, 6, 10, 0, 41, 1, 123, DateTimeKind.Local).AddTicks(4560) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.12345 +0900", new DateTime(2003, 6, 10, 0, 41, 1, 123, DateTimeKind.Local).AddTicks(4500) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.1234 +0900", new DateTime(2003, 6, 10, 0, 41, 1, 123, DateTimeKind.Local).AddTicks(4000) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.1234567 -0400", new DateTime(2003, 6, 10, 13, 41, 1, 123, DateTimeKind.Local).AddTicks(4567) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.1234567 +1245", new DateTime(2003, 6,  9, 20, 56, 1, 123, DateTimeKind.Local).AddTicks(4567) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.123 +0900", new DateTime(2003, 6, 10, 0, 41, 1, 123, DateTimeKind.Local) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.12 +0900", new DateTime(2003, 6, 10, 0, 41, 1, 120, DateTimeKind.Local) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.1 +0900", new DateTime(2003, 6, 10, 0, 41, 1, 100, DateTimeKind.Local) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01 +0900", new DateTime(2003, 6, 10, 0, 41, 1, DateTimeKind.Local) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:1 +0900", new DateTime(2003, 6, 10, 0, 41, 1, DateTimeKind.Local) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41 +0900", new DateTime(2003, 6, 10, 0, 41, 0, DateTimeKind.Local) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:4 +0900", new DateTime(2003, 6, 10, 0, 4, 0, DateTimeKind.Local) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 9:04 +0900", new DateTime(2003, 6, 10, 0, 4, 0, DateTimeKind.Local) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 9:4 +0900", new DateTime(2003, 6, 10, 0, 4, 0, DateTimeKind.Local) };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeString_Local))]
  public void FromRFC822DateTimeString_Local(string s, DateTime expected)
  {
    var actual = DateTimeFormat.FromRFC822DateTimeString(s);

    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(actual),
      actual,
      s
    );
  }

  private static IEnumerable YieldTestCases_FromRFC822DateTimeString_Local_MilitaryTimeZones()
  {
    var expected = new DateTime(2022, 4, 27, 20, 54, 1, 123, DateTimeKind.Unspecified).AddTicks(4567);

    foreach (var dayOfWeek in new[] { "Wed, ", string.Empty }) {
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 A", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 B", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 C", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 D", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 E", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 F", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 G", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 H", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 I", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 K", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 L", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 M", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 N", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 O", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 P", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 Q", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 R", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 S", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 T", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 U", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 V", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 W", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 X", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 Y", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 Z", expected };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeString_Local_MilitaryTimeZones))]
  public void FromRFC822DateTimeString_Local_MilitaryTimeZones(string s, DateTime expected)
  {
    var actual = DateTimeFormat.FromRFC822DateTimeString(s);

    Assert.AreEqual(
      expected,
      actual,
      s
    );
  }

  private static IEnumerable YieldTestCases_FromRFC822DateTimeOffsetString()
  {
    foreach (var dayOfWeek in new[] { "Tue, ", string.Empty }) {
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.1234567 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)).AddTicks(4567) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.123456 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)).AddTicks(4560) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.12345 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)).AddTicks(4500) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.1234 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)).AddTicks(4000) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.1234567 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)).AddTicks(4567) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.1234567 -0400", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(-4)).AddTicks(4567) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.1234567 +1245", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(12) + TimeSpan.FromMinutes(45)).AddTicks(4567) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.123 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 123, TimeSpan.FromHours(+9)) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.12 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 120, TimeSpan.FromHours(+9)) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01.1 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, 100, TimeSpan.FromHours(+9)) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:01 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, TimeSpan.FromHours(+9)) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41:1 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 1, TimeSpan.FromHours(+9)) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:41 +0900", new DateTimeOffset(2003, 6, 10, 9, 41, 0, TimeSpan.FromHours(+9)) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 09:4 +0900", new DateTimeOffset(2003, 6, 10, 9, 4, 0, TimeSpan.FromHours(+9)) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 9:04 +0900", new DateTimeOffset(2003, 6, 10, 9, 4, 0, TimeSpan.FromHours(+9)) };
      yield return new object[] { dayOfWeek + "10 Jun 2003 9:4 +0900", new DateTimeOffset(2003, 6, 10, 9, 4, 0, TimeSpan.FromHours(+9)) };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeOffsetString))/*, Ignore("Mono Bug #547675")*/]
  public void FromRFC822DateTimeOffsetString(string s, DateTimeOffset expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromRFC822DateTimeOffsetString(s),
      s
    );

  [SetCulture("it-IT")] // '.' is used instead of ':'
  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeOffsetString))]
  public void FromRFC822DateTimeOffsetString_TimeSeparator(string s, DateTimeOffset expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromRFC822DateTimeOffsetString(s),
      s
    );

  private static IEnumerable YieldTestCases_FromRFC822DateTimeOffsetString_UniversalTime()
  {
    foreach (var dayOfWeek in new[] { "Fri, ", string.Empty }) {
      foreach (var year in new[] { "2001", "01" }) {
        foreach (var zone in new[] { "GMT", "UT" }) {
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 19:23:02.1234567 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.FromHours(0)).AddTicks(4567) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 19:23:02.123456 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.FromHours(0)).AddTicks(4560) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 19:23:02.12345 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.FromHours(0)).AddTicks(4500) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 19:23:02.1234 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.FromHours(0)).AddTicks(4000) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 19:23:02.123 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 123, TimeSpan.FromHours(0)) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 19:23:02.12 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 120, TimeSpan.FromHours(0)) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 19:23:02.1 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, 100, TimeSpan.FromHours(0)) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 19:23:02 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, TimeSpan.FromHours(0)) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 19:23:2 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 2, TimeSpan.FromHours(0)) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 19:23 {zone}", new DateTimeOffset(2001, 4, 13, 19, 23, 0, TimeSpan.FromHours(0)) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 19:2 {zone}", new DateTimeOffset(2001, 4, 13, 19, 2, 0, TimeSpan.FromHours(0)) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 01:2 {zone}", new DateTimeOffset(2001, 4, 13, 1, 2, 0, TimeSpan.FromHours(0)) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 1:02 {zone}", new DateTimeOffset(2001, 4, 13, 1, 2, 0, TimeSpan.FromHours(0)) };
          yield return new object[] { $"{dayOfWeek}13 Apr {year} 1:2 {zone}", new DateTimeOffset(2001, 4, 13, 1, 2, 0, TimeSpan.FromHours(0)) };
        }
      }
    }
  }

  private static IEnumerable YieldTestCases_FromRFC822DateTimeOffsetString_Local_MilitaryTimeZones()
  {
    var expected = new DateTimeOffset(2022, 4, 27, 20, 54, 1, 123, TimeSpan.FromHours(-0.0)).AddTicks(4567);

    foreach (var dayOfWeek in new[] { "Wed, ", string.Empty }) {
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 A", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 B", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 C", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 D", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 E", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 F", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 G", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 H", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 I", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 K", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 L", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 M", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 N", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 O", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 P", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 Q", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 R", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 S", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 T", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 U", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 V", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 W", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 X", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 Y", expected };
      yield return new object[] { dayOfWeek + "27 Apr 2022 20:54:01.1234567 Z", expected };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeOffsetString_Local_MilitaryTimeZones))]
  public void FromRFC822DateTimeOffsetString_MilitaryTimeZones(string s, DateTimeOffset expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromRFC822DateTimeOffsetString(s),
      s
    );

  private static IEnumerable YieldTestCases_FromRFC822DateTimeString_Local_NorthAmericanTimeZones()
  {
    yield return new object[] { "12 Jun 2006 11:00:00.0000000 EDT", "2006-06-12T11:00:00.0000000-04:00" };
    yield return new object[] { "04 Nov 2007 01:00:00.0000000 EST", "2007-11-04T01:00:00.0000000-05:00" };
    yield return new object[] { "10 Dec 2006 15:00:00.0000000 EST", "2006-12-10T15:00:00.0000000-05:00" };
    yield return new object[] { "11 Mar 2007 02:30:00.0000000 EST", "2007-03-11T02:30:00.0000000-05:00" };
    yield return new object[] { "12 Jun 2006 11:00:00.0000000 CDT", "2006-06-12T11:00:00.0000000-05:00" };
    yield return new object[] { "04 Nov 2007 01:00:00.0000000 CST", "2007-11-04T01:00:00.0000000-06:00" };
    yield return new object[] { "10 Dec 2006 15:00:00.0000000 CST", "2006-12-10T15:00:00.0000000-06:00" };
    yield return new object[] { "11 Mar 2007 02:30:00.0000000 CST", "2007-03-11T02:30:00.0000000-06:00" };
    yield return new object[] { "12 Jun 2006 11:00:00.0000000 MDT", "2006-06-12T11:00:00.0000000-06:00" };
    yield return new object[] { "04 Nov 2007 01:00:00.0000000 MST", "2007-11-04T01:00:00.0000000-07:00" };
    yield return new object[] { "10 Dec 2006 15:00:00.0000000 MST", "2006-12-10T15:00:00.0000000-07:00" };
    yield return new object[] { "11 Mar 2007 02:30:00.0000000 MST", "2007-03-11T02:30:00.0000000-07:00" };
    yield return new object[] { "12 Jun 2006 11:00:00.0000000 PDT", "2006-06-12T11:00:00.0000000-07:00" };
    yield return new object[] { "04 Nov 2007 01:00:00.0000000 PST", "2007-11-04T01:00:00.0000000-08:00" };
    yield return new object[] { "10 Dec 2006 15:00:00.0000000 PST", "2006-12-10T15:00:00.0000000-08:00" };
    yield return new object[] { "11 Mar 2007 02:30:00.0000000 PST", "2007-03-11T02:30:00.0000000-08:00" };
  }

  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeString_Local_NorthAmericanTimeZones))]
  public void FromRFC822DateTimeString_NorthAmericanTimeZones(string s, string expected)
    => Assert.AreEqual(
      DateTimeOffset.ParseExact(expected, "o", null).UtcDateTime,
      DateTimeFormat.FromRFC822DateTimeString(s),
      s
    );

  [TestCaseSource(nameof(YieldTestCases_FromRFC822DateTimeString_Local_NorthAmericanTimeZones))]
  public void FromRFC822DateTimeOffsetString_NorthAmericanTimeZones(string s, string expected)
    => Assert.AreEqual(
      DateTimeOffset.ParseExact(expected, "o", null),
      DateTimeFormat.FromRFC822DateTimeOffsetString(s),
      s
    );
}
