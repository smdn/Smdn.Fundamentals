// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Globalization;
using NUnit.Framework;

namespace Smdn.Formats {
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
    public void TestGetCurrentTimeZoneOffsetString()
    {
      Assert.AreEqual(timezoneOffset, DateTimeFormat.GetCurrentTimeZoneOffsetString(true));
      Assert.AreEqual(timezoneOffsetNoDelim, DateTimeFormat.GetCurrentTimeZoneOffsetString(false));
    }

    [Test]
    public void TestToRFC822DateTimeStringUtc()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

      Assert.AreEqual("Mon, 25 Feb 2008 15:01:12 GMT",
                      DateTimeFormat.ToRFC822DateTimeString(dtm));
    }

    [Test]
    public void TestToRFC822DateTimeStringLocal()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Local);

      Assert.AreEqual("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
                      DateTimeFormat.ToRFC822DateTimeString(dtm));
    }

    [Test]
    public void TestToRFC822DateTimeStringUnspecifiedKind()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Unspecified);

      Assert.AreEqual("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
                      DateTimeFormat.ToRFC822DateTimeString(dtm));
    }

    [Test]
    public void TestToRFC822DateTimeStringDateTimeOffset()
    {
      var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, DateTimeOffset.Now.Offset);

      Assert.AreEqual("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
                      DateTimeFormat.ToRFC822DateTimeString(dto));
    }

    [Test]
    public void TestFromRFC822DateTimeStringUtc()
    {
      var dtm = DateTimeFormat.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41:01.1234567 GMT");
      var c = "case1";

      Assert.AreEqual(DayOfWeek.Tuesday, dtm.DayOfWeek, c);
      Assert.AreEqual(10, dtm.Day, c);
      Assert.AreEqual(6, dtm.Month, c);
      Assert.AreEqual(2003, dtm.Year, c);
      Assert.AreEqual(9, dtm.Hour, c);
      Assert.AreEqual(41, dtm.Minute, c);
      Assert.AreEqual(1, dtm.Second, c);
      Assert.AreEqual(123, dtm.Millisecond, c);
      Assert.AreEqual(4567, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Utc, dtm.Kind, c);

      dtm = DateTimeFormat.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41:01.123 GMT");
      c = "case2";

      Assert.AreEqual(DayOfWeek.Tuesday, dtm.DayOfWeek, c);
      Assert.AreEqual(10, dtm.Day, c);
      Assert.AreEqual(6, dtm.Month, c);
      Assert.AreEqual(2003, dtm.Year, c);
      Assert.AreEqual(9, dtm.Hour, c);
      Assert.AreEqual(41, dtm.Minute, c);
      Assert.AreEqual(1, dtm.Second, c);
      Assert.AreEqual(123, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Utc, dtm.Kind, c);

      dtm = DateTimeFormat.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41:01 GMT");
      c = "case3";

      Assert.AreEqual(DayOfWeek.Tuesday, dtm.DayOfWeek, c);
      Assert.AreEqual(10, dtm.Day, c);
      Assert.AreEqual(6, dtm.Month, c);
      Assert.AreEqual(2003, dtm.Year, c);
      Assert.AreEqual(9, dtm.Hour, c);
      Assert.AreEqual(41, dtm.Minute, c);
      Assert.AreEqual(1, dtm.Second, c);
      Assert.AreEqual(0, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Utc, dtm.Kind, c);

      dtm = DateTimeFormat.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41 GMT");

      Assert.AreEqual(DayOfWeek.Tuesday, dtm.DayOfWeek, c);
      Assert.AreEqual(10, dtm.Day, c);
      Assert.AreEqual(6, dtm.Month, c);
      Assert.AreEqual(2003, dtm.Year, c);
      Assert.AreEqual(9, dtm.Hour, c);
      Assert.AreEqual(41, dtm.Minute, c);
      Assert.AreEqual(0, dtm.Second, c);
      Assert.AreEqual(0, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Utc, dtm.Kind, c);
    }

    [Test]
    public void TestFromRFC822DateTimeStringLocal()
    {
      var localTimeZone = TimeZoneInfo.Local;
      var dtm = DateTimeFormat.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41:01.1234567 +0900");
      var c = "case1";

      Assert.AreEqual(DayOfWeek.Tuesday, dtm.DayOfWeek, c);
      Assert.AreEqual(10, dtm.Day, c);
      Assert.AreEqual(6, dtm.Month, c);
      Assert.AreEqual(2003, dtm.Year, c);
      Assert.AreEqual(0 + localTimeZone.GetUtcOffset(dtm).Hours, dtm.Hour, c);
      Assert.AreEqual(41 + localTimeZone.GetUtcOffset(dtm).Minutes, dtm.Minute, c);
      Assert.AreEqual(1, dtm.Second, c);
      Assert.AreEqual(123, dtm.Millisecond, c);
      Assert.AreEqual(4567, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind, c);

      dtm = DateTimeFormat.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41:01.123 +0900");
      c = "case2";

      Assert.AreEqual(DayOfWeek.Tuesday, dtm.DayOfWeek, c);
      Assert.AreEqual(10, dtm.Day, c);
      Assert.AreEqual(6, dtm.Month, c);
      Assert.AreEqual(2003, dtm.Year, c);
      Assert.AreEqual(0 + localTimeZone.GetUtcOffset(dtm).Hours, dtm.Hour, c);
      Assert.AreEqual(41 + localTimeZone.GetUtcOffset(dtm).Minutes, dtm.Minute, c);
      Assert.AreEqual(1, dtm.Second, c);
      Assert.AreEqual(123, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind, c);

      dtm = DateTimeFormat.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41:01 +0900");
      c = "case3";

      Assert.AreEqual(DayOfWeek.Tuesday, dtm.DayOfWeek, c);
      Assert.AreEqual(10, dtm.Day, c);
      Assert.AreEqual(6, dtm.Month, c);
      Assert.AreEqual(2003, dtm.Year, c);
      Assert.AreEqual(0 + localTimeZone.GetUtcOffset(dtm).Hours, dtm.Hour, c);
      Assert.AreEqual(41 + localTimeZone.GetUtcOffset(dtm).Minutes, dtm.Minute, c);
      Assert.AreEqual(1, dtm.Second, c);
      Assert.AreEqual(0, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind, c);

      dtm = DateTimeFormat.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41 +0900");
      c = "case4";

      Assert.AreEqual(DayOfWeek.Tuesday, dtm.DayOfWeek, c);
      Assert.AreEqual(10, dtm.Day, c);
      Assert.AreEqual(6, dtm.Month, c);
      Assert.AreEqual(2003, dtm.Year, c);
      Assert.AreEqual(0 + localTimeZone.GetUtcOffset(dtm).Hours, dtm.Hour, c);
      Assert.AreEqual(41 + localTimeZone.GetUtcOffset(dtm).Minutes, dtm.Minute, c);
      Assert.AreEqual(0, dtm.Second, c);
      Assert.AreEqual(0, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind, c);
    }

    [Test/*, Ignore("Mono Bug #547675")*/]
    public void TestFromRFC822DateTimeOffsetString()
    {
      var dto = DateTimeFormat.FromRFC822DateTimeOffsetString("Tue, 10 Jun 2003 09:41:01.1234567 +0900");
      var c = "case1";

      Assert.AreEqual(DayOfWeek.Tuesday, dto.DayOfWeek, c);
      Assert.AreEqual(10, dto.Day, c);
      Assert.AreEqual(6, dto.Month, c);
      Assert.AreEqual(2003, dto.Year, c);
      Assert.AreEqual(9, dto.Hour, c);
      Assert.AreEqual(41, dto.Minute, c);
      Assert.AreEqual(1, dto.Second, c);
      Assert.AreEqual(123, dto.Millisecond, c);
      Assert.AreEqual(4567, dto.Ticks % 10000, c);
      Assert.AreEqual(9, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);

      dto = DateTimeFormat.FromRFC822DateTimeOffsetString("Tue, 10 Jun 2003 09:41:01.123 +0900");
      c = "case2";

      Assert.AreEqual(DayOfWeek.Tuesday, dto.DayOfWeek, c);
      Assert.AreEqual(10, dto.Day, c);
      Assert.AreEqual(6, dto.Month, c);
      Assert.AreEqual(2003, dto.Year, c);
      Assert.AreEqual(9, dto.Hour, c);
      Assert.AreEqual(41, dto.Minute, c);
      Assert.AreEqual(1, dto.Second, c);
      Assert.AreEqual(123, dto.Millisecond, c);
      Assert.AreEqual(0, dto.Ticks % 10000, c);
      Assert.AreEqual(9, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);

      dto = DateTimeFormat.FromRFC822DateTimeOffsetString("Tue, 10 Jun 2003 09:41:01 +0900");
      c = "case3";

      Assert.AreEqual(DayOfWeek.Tuesday, dto.DayOfWeek, c);
      Assert.AreEqual(10, dto.Day, c);
      Assert.AreEqual(6, dto.Month, c);
      Assert.AreEqual(2003, dto.Year, c);
      Assert.AreEqual(9, dto.Hour, c);
      Assert.AreEqual(41, dto.Minute, c);
      Assert.AreEqual(1, dto.Second, c);
      Assert.AreEqual(0, dto.Millisecond, c);
      Assert.AreEqual(0, dto.Ticks % 10000, c);
      Assert.AreEqual(9, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);

      dto = DateTimeFormat.FromRFC822DateTimeOffsetString("Tue, 10 Jun 2003 09:41 +0900");
      c = "case4";

      Assert.AreEqual(DayOfWeek.Tuesday, dto.DayOfWeek, c);
      Assert.AreEqual(10, dto.Day, c);
      Assert.AreEqual(6, dto.Month, c);
      Assert.AreEqual(2003, dto.Year, c);
      Assert.AreEqual(9, dto.Hour, c);
      Assert.AreEqual(41, dto.Minute, c);
      Assert.AreEqual(0, dto.Second, c);
      Assert.AreEqual(0, dto.Millisecond, c);
      Assert.AreEqual(0, dto.Ticks % 10000, c);
      Assert.AreEqual(9, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);
    }

    [Test]
    public void TestFromRFC822DateTimeOffsetStringGmt()
    {
      var dto = DateTimeFormat.FromRFC822DateTimeOffsetString("Fri, 13 Apr 2001 19:23:02.1234567 GMT");
      var c = "case1";

      Assert.AreEqual(DayOfWeek.Friday, dto.DayOfWeek, c);
      Assert.AreEqual(13, dto.Day, c);
      Assert.AreEqual(4, dto.Month, c);
      Assert.AreEqual(2001, dto.Year, c);
      Assert.AreEqual(19, dto.Hour, c);
      Assert.AreEqual(23, dto.Minute, c);
      Assert.AreEqual(2, dto.Second, c);
      Assert.AreEqual(123, dto.Millisecond, c);
      Assert.AreEqual(4567, dto.Ticks % 10000, c);
      Assert.AreEqual(0, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);

      dto = DateTimeFormat.FromRFC822DateTimeOffsetString("Fri, 13 Apr 2001 19:23:02.123 GMT");
      c = "case2";

      Assert.AreEqual(DayOfWeek.Friday, dto.DayOfWeek, c);
      Assert.AreEqual(13, dto.Day, c);
      Assert.AreEqual(4, dto.Month, c);
      Assert.AreEqual(2001, dto.Year, c);
      Assert.AreEqual(19, dto.Hour, c);
      Assert.AreEqual(23, dto.Minute, c);
      Assert.AreEqual(2, dto.Second, c);
      Assert.AreEqual(123, dto.Millisecond, c);
      Assert.AreEqual(0, dto.Ticks % 10000, c);
      Assert.AreEqual(0, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);

      dto = DateTimeFormat.FromRFC822DateTimeOffsetString("Fri, 13 Apr 2001 19:23:02 GMT");
      c = "case3";

      Assert.AreEqual(DayOfWeek.Friday, dto.DayOfWeek, c);
      Assert.AreEqual(13, dto.Day, c);
      Assert.AreEqual(4, dto.Month, c);
      Assert.AreEqual(2001, dto.Year, c);
      Assert.AreEqual(19, dto.Hour, c);
      Assert.AreEqual(23, dto.Minute, c);
      Assert.AreEqual(2, dto.Second, c);
      Assert.AreEqual(0, dto.Millisecond, c);
      Assert.AreEqual(0, dto.Ticks % 10000, c);
      Assert.AreEqual(0, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);

      dto = DateTimeFormat.FromRFC822DateTimeOffsetString("Fri, 13 Apr 2001 19:23 GMT");
      c = "case4";

      Assert.AreEqual(DayOfWeek.Friday, dto.DayOfWeek, c);
      Assert.AreEqual(13, dto.Day, c);
      Assert.AreEqual(4, dto.Month, c);
      Assert.AreEqual(2001, dto.Year, c);
      Assert.AreEqual(19, dto.Hour, c);
      Assert.AreEqual(23, dto.Minute, c);
      Assert.AreEqual(0, dto.Second, c);
      Assert.AreEqual(0, dto.Millisecond, c);
      Assert.AreEqual(0, dto.Ticks % 10000, c);
      Assert.AreEqual(0, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);
    }


    [Test]
    public void TestToISO8601DateTimeStringUtc()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

      Assert.AreEqual(DateTimeFormat.ToW3CDateTimeString(dtm),
                      DateTimeFormat.ToISO8601DateTimeString(dtm));
    }

    [Test]
    public void TestToW3CDateTimeStringUtc()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, 456, DateTimeKind.Utc);

      Assert.AreEqual("2008-02-25T15:01:12.4560000Z",
                      DateTimeFormat.ToW3CDateTimeString(dtm));
    }

    [Test]
    public void TestToW3CDateTimeStringLocal()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, 456, DateTimeKind.Local);

      Assert.AreEqual("2008-02-25T15:01:12.4560000" + timezoneOffset,
                      DateTimeFormat.ToW3CDateTimeString(dtm));
    }

    [Test]
    public void TestToW3CDateTimeStringUnspecifiedKind()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, 456, DateTimeKind.Unspecified);

      Assert.AreEqual("2008-02-25T15:01:12.4560000",
                      DateTimeFormat.ToW3CDateTimeString(dtm));
    }

    [Test]
    public void TestToW3CDateTimeStringDateTimeOffset()
    {
      var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, 456, DateTimeOffset.Now.Offset);

      Assert.AreEqual("2008-02-25T15:01:12.4560000" + timezoneOffset,
                      DateTimeFormat.ToW3CDateTimeString(dto));
    }

    [Test]
    public void TestFromISO8601DateTimeString()
    {
      var dtm = "2008-04-11T12:34:56.7893333Z";

      Assert.AreEqual(DateTimeFormat.FromW3CDateTimeString(dtm),
                      DateTimeFormat.FromISO8601DateTimeString(dtm));

      dtm = "2008-04-11T12:34:56.789Z";

      Assert.AreEqual(DateTimeFormat.FromW3CDateTimeString(dtm),
                      DateTimeFormat.FromISO8601DateTimeString(dtm));

      dtm = "2008-04-11T12:34:56Z";

      Assert.AreEqual(DateTimeFormat.FromW3CDateTimeString(dtm),
                      DateTimeFormat.FromISO8601DateTimeString(dtm));

      dtm = "2008-04-11T12:34Z";

      Assert.AreEqual(DateTimeFormat.FromW3CDateTimeString(dtm),
                      DateTimeFormat.FromISO8601DateTimeString(dtm));
    }

    [Test]
    public void TestFromW3CDateTimeStringUtc()
    {
      var dtm = DateTimeFormat.FromW3CDateTimeString("2008-04-11T12:34:56.7893333Z");
      var c = "case1";

      Assert.AreEqual(2008, dtm.Year, c);
      Assert.AreEqual(04, dtm.Month, c);
      Assert.AreEqual(11, dtm.Day, c);
      Assert.AreEqual(12, dtm.Hour, c);
      Assert.AreEqual(34, dtm.Minute, c);
      Assert.AreEqual(56, dtm.Second, c);
      Assert.AreEqual(789, dtm.Millisecond, c);
      Assert.AreEqual(3333, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Utc, dtm.Kind, c);

      dtm = DateTimeFormat.FromW3CDateTimeString("2008-04-11T12:34:56.789Z");
      c = "case2";

      Assert.AreEqual(2008, dtm.Year, c);
      Assert.AreEqual(04, dtm.Month, c);
      Assert.AreEqual(11, dtm.Day, c);
      Assert.AreEqual(12, dtm.Hour, c);
      Assert.AreEqual(34, dtm.Minute, c);
      Assert.AreEqual(56, dtm.Second, c);
      Assert.AreEqual(789, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Utc, dtm.Kind, c);

      dtm = DateTimeFormat.FromW3CDateTimeString("2008-04-11T12:34:56Z");
      c = "case3";

      Assert.AreEqual(2008, dtm.Year, c);
      Assert.AreEqual(04, dtm.Month, c);
      Assert.AreEqual(11, dtm.Day, c);
      Assert.AreEqual(12, dtm.Hour, c);
      Assert.AreEqual(34, dtm.Minute, c);
      Assert.AreEqual(56, dtm.Second, c);
      Assert.AreEqual(0, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Utc, dtm.Kind, c);

      dtm = DateTimeFormat.FromW3CDateTimeString("2008-04-11T12:34Z");
      c = "case4";

      Assert.AreEqual(2008, dtm.Year, c);
      Assert.AreEqual(04, dtm.Month, c);
      Assert.AreEqual(11, dtm.Day, c);
      Assert.AreEqual(12, dtm.Hour, c);
      Assert.AreEqual(34, dtm.Minute, c);
      Assert.AreEqual(0, dtm.Second, c);
      Assert.AreEqual(0, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Utc, dtm.Kind, c);
    }

    [Test]
    public void TestFromW3CDateTimeStringLocal()
    {
      var localTimeZone = TimeZoneInfo.Local;
      var dtm = DateTimeFormat.FromW3CDateTimeString("2008-04-11T12:34:56.7893333 +09:00");
      var c = "case1";

      Assert.AreEqual(2008, dtm.Year, c);
      Assert.AreEqual(04, dtm.Month, c);
      Assert.AreEqual(11, dtm.Day, c);
      Assert.AreEqual(3 + localTimeZone.GetUtcOffset(dtm).Hours, dtm.Hour, c);
      Assert.AreEqual(34 + localTimeZone.GetUtcOffset(dtm).Minutes, dtm.Minute, c);
      Assert.AreEqual(56, dtm.Second, c);
      Assert.AreEqual(789, dtm.Millisecond, c);
      Assert.AreEqual(3333, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind, c);

      dtm = DateTimeFormat.FromW3CDateTimeString("2008-04-11T12:34:56.789 +09:00");
      c = "case2";

      Assert.AreEqual(2008, dtm.Year, c);
      Assert.AreEqual(04, dtm.Month, c);
      Assert.AreEqual(11, dtm.Day, c);
      Assert.AreEqual(3 + localTimeZone.GetUtcOffset(dtm).Hours, dtm.Hour, c);
      Assert.AreEqual(34 + localTimeZone.GetUtcOffset(dtm).Minutes, dtm.Minute, c);
      Assert.AreEqual(56, dtm.Second, c);
      Assert.AreEqual(789, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind, c);

      dtm = DateTimeFormat.FromW3CDateTimeString("2008-04-11T12:34:56 +09:00");
      c = "case3";

      Assert.AreEqual(2008, dtm.Year, c);
      Assert.AreEqual(04, dtm.Month, c);
      Assert.AreEqual(11, dtm.Day, c);
      Assert.AreEqual(3 + localTimeZone.GetUtcOffset(dtm).Hours, dtm.Hour, c);
      Assert.AreEqual(34 + localTimeZone.GetUtcOffset(dtm).Minutes, dtm.Minute, c);
      Assert.AreEqual(56, dtm.Second, c);
      Assert.AreEqual(0, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind, c);

      dtm = DateTimeFormat.FromW3CDateTimeString("2008-04-11T12:34 +09:00");
      c = "case4";

      Assert.AreEqual(2008, dtm.Year, c);
      Assert.AreEqual(04, dtm.Month, c);
      Assert.AreEqual(11, dtm.Day, c);
      Assert.AreEqual(3 + localTimeZone.GetUtcOffset(dtm).Hours, dtm.Hour, c);
      Assert.AreEqual(34 + localTimeZone.GetUtcOffset(dtm).Minutes, dtm.Minute, c);
      Assert.AreEqual(0, dtm.Second, c);
      Assert.AreEqual(0, dtm.Millisecond, c);
      Assert.AreEqual(0, dtm.Ticks % 10000, c);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind, c);
    }

    [Test]
    public void TestFromW3CDateTimeOffsetString()
    {
      var dto = DateTimeFormat.FromW3CDateTimeOffsetString("2008-04-11T12:34:56.7893333 +09:00");
      var c = "case1";

      Assert.AreEqual(2008, dto.Year, c);
      Assert.AreEqual(04, dto.Month, c);
      Assert.AreEqual(11, dto.Day, c);
      Assert.AreEqual(12, dto.Hour, c);
      Assert.AreEqual(34, dto.Minute, c);
      Assert.AreEqual(56, dto.Second, c);
      Assert.AreEqual(789, dto.Millisecond, c);
      Assert.AreEqual(3333, dto.Ticks % 10000, c);
      Assert.AreEqual(9, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);

      dto = DateTimeFormat.FromW3CDateTimeOffsetString("2008-04-11T12:34:56.789 +09:00");
      c = "case2";

      Assert.AreEqual(2008, dto.Year, c);
      Assert.AreEqual(04, dto.Month, c);
      Assert.AreEqual(11, dto.Day, c);
      Assert.AreEqual(12, dto.Hour, c);
      Assert.AreEqual(34, dto.Minute, c);
      Assert.AreEqual(56, dto.Second, c);
      Assert.AreEqual(789, dto.Millisecond, c);
      Assert.AreEqual(0, dto.Ticks % 10000, c);
      Assert.AreEqual(9, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);

      dto = DateTimeFormat.FromW3CDateTimeOffsetString("2008-04-11T12:34:56 +09:00");
      c = "case3";

      Assert.AreEqual(2008, dto.Year, c);
      Assert.AreEqual(04, dto.Month, c);
      Assert.AreEqual(11, dto.Day, c);
      Assert.AreEqual(12, dto.Hour, c);
      Assert.AreEqual(34, dto.Minute, c);
      Assert.AreEqual(56, dto.Second, c);
      Assert.AreEqual(0, dto.Millisecond, c);
      Assert.AreEqual(0, dto.Ticks % 10000, c);
      Assert.AreEqual(9, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);

      dto = DateTimeFormat.FromW3CDateTimeOffsetString("2008-04-11T12:34 +09:00");
      c = "case4";

      Assert.AreEqual(2008, dto.Year, c);
      Assert.AreEqual(04, dto.Month, c);
      Assert.AreEqual(11, dto.Day, c);
      Assert.AreEqual(12, dto.Hour, c);
      Assert.AreEqual(34, dto.Minute, c);
      Assert.AreEqual(0, dto.Second, c);
      Assert.AreEqual(0, dto.Millisecond, c);
      Assert.AreEqual(0, dto.Ticks % 10000, c);
      Assert.AreEqual(9, dto.Offset.Hours, c);
      Assert.AreEqual(0, dto.Offset.Minutes, c);
    }
  }
}
