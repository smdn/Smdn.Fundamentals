// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

using NUnit.Framework;

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

  private static IEnumerable YieldTestCases_FromW3CDateTimeString_Utc()
  {
    foreach (var T in new[] { "T", " "}) {
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333 Z", new DateTime(2008, 4, 11, 12, 34, 56, 789, DateTimeKind.Utc).AddTicks(3333) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333Z", new DateTime(2008, 4, 11, 12, 34, 56, 789, DateTimeKind.Utc).AddTicks(3333) };
      yield return new object[] { $"2008-04-11{T}12:34:56.789333Z", new DateTime(2008, 4, 11, 12, 34, 56, 789, DateTimeKind.Utc).AddTicks(3330) };
      yield return new object[] { $"2008-04-11{T}12:34:56.78933Z", new DateTime(2008, 4, 11, 12, 34, 56, 789, DateTimeKind.Utc).AddTicks(3300) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7893Z", new DateTime(2008, 4, 11, 12, 34, 56, 789, DateTimeKind.Utc).AddTicks(3000) };
      yield return new object[] { $"2008-04-11{T}12:34:56.789Z", new DateTime(2008, 4, 11, 12, 34, 56, 789, DateTimeKind.Utc) };
      yield return new object[] { $"2008-04-11{T}12:34:56.78Z", new DateTime(2008, 4, 11, 12, 34, 56, 780, DateTimeKind.Utc) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7Z", new DateTime(2008, 4, 11, 12, 34, 56, 700, DateTimeKind.Utc) };
      yield return new object[] { $"2008-04-11{T}12:34:56Z", new DateTime(2008, 4, 11, 12, 34, 56, DateTimeKind.Utc) };
      yield return new object[] { $"2008-04-11{T}12:34Z", new DateTime(2008, 4, 11, 12, 34, 0, DateTimeKind.Utc) };
      yield return new object[] { $"2008-04-11{T}12Z", new DateTime(2008, 4, 11, 12, 0, 0, DateTimeKind.Utc) };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_FromW3CDateTimeString_Utc))]
  public void FromW3CDateTimeString_Utc(string s, DateTime expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromW3CDateTimeString(s),
      s
    );

  private static IEnumerable YieldTestCases_FromW3CDateTimeString_Local()
  {
    foreach (var T in new[] { "T", " "}) {
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333-0400", new DateTime(2008, 4, 11, 16, 34, 56, 789, DateTimeKind.Local).AddTicks(3333) }; // invalid but acceptable
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333 -0400", new DateTime(2008, 4, 11, 16, 34, 56, 789, DateTimeKind.Local).AddTicks(3333) }; // invalid but acceptable
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333-04:00", new DateTime(2008, 4, 11, 16, 34, 56, 789, DateTimeKind.Local).AddTicks(3333) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333 -04:00", new DateTime(2008, 4, 11, 16, 34, 56, 789, DateTimeKind.Local).AddTicks(3333) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333 +12:45", new DateTime(2008, 4, 10, 23, 49, 56, 789, DateTimeKind.Local).AddTicks(3333) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333 +09:00", new DateTime(2008, 4, 11, 3, 34, 56, 789, DateTimeKind.Local).AddTicks(3333) };
      yield return new object[] { $"2008-04-11{T}12:34:56.789333 +09:00", new DateTime(2008, 4, 11, 3, 34, 56, 789, DateTimeKind.Local).AddTicks(3330) };
      yield return new object[] { $"2008-04-11{T}12:34:56.78933 +09:00", new DateTime(2008, 4, 11, 3, 34, 56, 789, DateTimeKind.Local).AddTicks(3300) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7893 +09:00", new DateTime(2008, 4, 11, 3, 34, 56, 789, DateTimeKind.Local).AddTicks(3000) };
      yield return new object[] { $"2008-04-11{T}12:34:56.789 +09:00", new DateTime(2008, 4, 11, 3, 34, 56, 789, DateTimeKind.Local) };
      yield return new object[] { $"2008-04-11{T}12:34:56.78 +09:00", new DateTime(2008, 4, 11, 3, 34, 56, 780, DateTimeKind.Local) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7 +09:00", new DateTime(2008, 4, 11, 3, 34, 56, 700, DateTimeKind.Local) };
      yield return new object[] { $"2008-04-11{T}12:34:56 +09:00", new DateTime(2008, 4, 11, 3, 34, 56, DateTimeKind.Local) };
      yield return new object[] { $"2008-04-11{T}12:34 +09:00", new DateTime(2008, 4, 11, 3, 34, 0, DateTimeKind.Local) };
      yield return new object[] { $"2008-04-11{T}12 +09:00", new DateTime(2008, 4, 11, 3, 0, 0, DateTimeKind.Local) };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_FromW3CDateTimeString_Local))]
  public void FromW3CDateTimeString_Local(string s, DateTime expected)
  {
    var actual = DateTimeFormat.FromW3CDateTimeString(s);

    Assert.AreEqual(
      expected + TimeZoneInfo.Local.GetUtcOffset(actual),
      actual,
      s
    );
  }

  private static IEnumerable YieldTestCases_FromW3CDateTimeOffsetString()
  {
    foreach (var T in new[] { "T", " "}) {
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333-0400", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(-4)).AddTicks(3333) }; // invalid but acceptable
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333 -0400", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(-4)).AddTicks(3333) }; // invalid but acceptable
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333-04:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(-4)).AddTicks(3333) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333 -04:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(-4)).AddTicks(3333) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333 +12:45", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(12) + TimeSpan.FromMinutes(45)).AddTicks(3333) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7893333 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)).AddTicks(3333) };
      yield return new object[] { $"2008-04-11{T}12:34:56.789333 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)).AddTicks(3330) };
      yield return new object[] { $"2008-04-11{T}12:34:56.78933 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)).AddTicks(3300) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7893 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)).AddTicks(3000) };
      yield return new object[] { $"2008-04-11{T}12:34:56.789 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 789, TimeSpan.FromHours(+9)) };
      yield return new object[] { $"2008-04-11{T}12:34:56.78 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 780, TimeSpan.FromHours(+9)) };
      yield return new object[] { $"2008-04-11{T}12:34:56.7 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, 700, TimeSpan.FromHours(+9)) };
      yield return new object[] { $"2008-04-11{T}12:34:56 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 56, TimeSpan.FromHours(+9)) };
      yield return new object[] { $"2008-04-11{T}12:34:05 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 5, TimeSpan.FromHours(+9)) };
      yield return new object[] { $"2008-04-11{T}12:34 +09:00", new DateTimeOffset(2008, 4, 11, 12, 34, 0, TimeSpan.FromHours(+9)) };
      yield return new object[] { $"2008-04-11{T}12 +09:00", new DateTimeOffset(2008, 4, 11, 12, 0, 0, TimeSpan.FromHours(+9)) };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_FromW3CDateTimeOffsetString))]
  public void FromW3CDateTimeOffsetString(string s, DateTimeOffset expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromW3CDateTimeOffsetString(s),
      s
    );

  [SetCulture("it-IT")] // '.' is used instead of ':'
  [TestCaseSource(nameof(YieldTestCases_FromW3CDateTimeOffsetString))]
  public void FromW3CDateTimeOffsetString_TimeSeparator(string s, DateTimeOffset expected)
    => Assert.AreEqual(
      expected,
      DateTimeFormat.FromW3CDateTimeOffsetString(s),
      s
    );
}
