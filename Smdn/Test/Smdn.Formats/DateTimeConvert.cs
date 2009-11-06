using System;
using System.Globalization;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture()]
  public class DateTimeConvertTests {
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
      Assert.AreEqual(timezoneOffset, DateTimeConvert.GetCurrentTimeZoneOffsetString(true));
      Assert.AreEqual(timezoneOffsetNoDelim, DateTimeConvert.GetCurrentTimeZoneOffsetString(false));
    }

    [Test]
    public void TestToRFC822DateTimeStringUtc()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

      Assert.AreEqual("Mon, 25 Feb 2008 15:01:12 GMT",
                      DateTimeConvert.ToRFC822DateTimeString(dtm));
    }

    [Test]
    public void TestToRFC822DateTimeStringLocal()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Local);

      Assert.AreEqual("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
                      DateTimeConvert.ToRFC822DateTimeString(dtm));
    }

    [Test]
    public void TestToRFC822DateTimeStringUnspecifiedKind()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Unspecified);

      Assert.AreEqual("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
                      DateTimeConvert.ToRFC822DateTimeString(dtm));
    }

    [Test]
    public void TestToRFC822DateTimeStringDateTimeOffset()
    {
      var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, DateTimeOffset.Now.Offset);

      Assert.AreEqual("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
                      DateTimeConvert.ToRFC822DateTimeString(dto));
    }

    [Test]
    public void TestFromRFC822DateTimeStringUtc()
    {
      var dtm = DateTimeConvert.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41:01 GMT");

      Assert.AreEqual(DayOfWeek.Tuesday, dtm.DayOfWeek);
      Assert.AreEqual(10, dtm.Day);
      Assert.AreEqual(6, dtm.Month);
      Assert.AreEqual(2003, dtm.Year);
      Assert.AreEqual(9, dtm.Hour);
      Assert.AreEqual(41, dtm.Minute);
      Assert.AreEqual(1, dtm.Second);
      Assert.AreEqual(DateTimeKind.Utc, dtm.Kind);
    }

    [Test]
    public void TestFromRFC822DateTimeStringLocal()
    {
      var dtm = DateTimeConvert.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41:01 +0900");

      Assert.AreEqual(DayOfWeek.Tuesday, dtm.DayOfWeek);
      Assert.AreEqual(10, dtm.Day);
      Assert.AreEqual(6, dtm.Month);
      Assert.AreEqual(2003, dtm.Year);
      Assert.AreEqual(9, dtm.Hour);
      Assert.AreEqual(41, dtm.Minute);
      Assert.AreEqual(1, dtm.Second);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind);
    }

    [Test, Ignore("Mono Bug #547675")]
    public void TestFromRFC822DateTimeOffsetString()
    {
      var dto = DateTimeConvert.FromRFC822DateTimeOffsetString("Tue, 10 Jun 2003 09:41:01 +0900");

      Assert.AreEqual(DayOfWeek.Tuesday, dto.DayOfWeek);
      Assert.AreEqual(10, dto.Day);
      Assert.AreEqual(6, dto.Month);
      Assert.AreEqual(2003, dto.Year);
      Assert.AreEqual(9, dto.Hour);
      Assert.AreEqual(41, dto.Minute);
      Assert.AreEqual(1, dto.Second);
      Assert.AreEqual(9, dto.Offset.Hours);
      Assert.AreEqual(0, dto.Offset.Minutes);
    }

    [Test]
    public void TestToISO8601DateTimeStringUtc()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

      Assert.AreEqual(DateTimeConvert.ToW3CDateTimeString(dtm),
                      DateTimeConvert.ToISO8601DateTimeString(dtm));
    }

    [Test]
    public void TestToW3CDateTimeStringUtc()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

      Assert.AreEqual("2008-02-25T15:01:12Z",
                      DateTimeConvert.ToW3CDateTimeString(dtm));
    }

    [Test]
    public void TestToW3CDateTimeStringLocal()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Local);

      Assert.AreEqual("2008-02-25T15:01:12" + timezoneOffset,
                      DateTimeConvert.ToW3CDateTimeString(dtm));
    }

    [Test]
    public void TestToW3CDateTimeStringUnspecifiedKind()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Unspecified);

      Assert.AreEqual("2008-02-25T15:01:12",
                      DateTimeConvert.ToW3CDateTimeString(dtm));
    }

    [Test]
    public void TestToW3CDateTimeStringDateTimeOffset()
    {
      var dto = new DateTimeOffset(2008, 2, 25, 15, 1, 12, DateTimeOffset.Now.Offset);

      Assert.AreEqual("2008-02-25T15:01:12" + timezoneOffset,
                      DateTimeConvert.ToW3CDateTimeString(dto));
    }

    [Test]
    public void TestFromISO8601DateTimeString()
    {
      var dtm = "2008-04-11T12:34:56Z";

      Assert.AreEqual(DateTimeConvert.FromW3CDateTimeString(dtm),
                      DateTimeConvert.FromISO8601DateTimeString(dtm));
    }

    [Test]
    public void TestFromW3CDateTimeStringUtc()
    {
      var dtm = DateTimeConvert.FromW3CDateTimeString("2008-04-11T12:34:56Z");

      Assert.AreEqual(2008, dtm.Year);
      Assert.AreEqual(04, dtm.Month);
      Assert.AreEqual(11, dtm.Day);
      Assert.AreEqual(12, dtm.Hour);
      Assert.AreEqual(34, dtm.Minute);
      Assert.AreEqual(56, dtm.Second);
      Assert.AreEqual(DateTimeKind.Utc, dtm.Kind);
    }

    [Test]
    public void TestFromW3CDateTimeStringLocal()
    {
      var dtm = DateTimeConvert.FromW3CDateTimeString("2008-04-11T12:34:56 +09:00");

      Assert.AreEqual(2008, dtm.Year);
      Assert.AreEqual(04, dtm.Month);
      Assert.AreEqual(11, dtm.Day);
      Assert.AreEqual(12, dtm.Hour);
      Assert.AreEqual(34, dtm.Minute);
      Assert.AreEqual(56, dtm.Second);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind);
    }

    [Test]
    public void TestFromW3CDateTimeOffsetString()
    {
      var dto = DateTimeConvert.FromW3CDateTimeOffsetString("2008-04-11T12:34:56 +09:00");

      Assert.AreEqual(2008, dto.Year);
      Assert.AreEqual(04, dto.Month);
      Assert.AreEqual(11, dto.Day);
      Assert.AreEqual(12, dto.Hour);
      Assert.AreEqual(34, dto.Minute);
      Assert.AreEqual(56, dto.Second);
      Assert.AreEqual(9, dto.Offset.Hours);
      Assert.AreEqual(0, dto.Offset.Minutes);
    }

    [Test]
    public void TestFromUnixTimeUtc()
    {
      Assert.AreEqual(DateTime.Parse("1970-01-01T00:00:00+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      DateTimeConvert.FromUnixTimeUtc(0L));
      Assert.AreEqual(DateTime.Parse("2001-09-09T01:46:40+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      DateTimeConvert.FromUnixTimeUtc(1000000000L));
      Assert.AreEqual(DateTime.Parse("2038-01-19T03:14:07+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      DateTimeConvert.FromUnixTimeUtc((long)0x7FFFFFFF));
    }

    [Test]
    public void TestToUnixTime64()
    {
      Assert.AreEqual(0L,
                      DateTimeConvert.ToUnixTime64(DateTime.Parse("1970-01-01T00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)));
      Assert.AreEqual(1000000000L,
                      DateTimeConvert.ToUnixTime64(DateTime.Parse("2001-09-09T01:46:40", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)));
      Assert.AreEqual((long)0x7FFFFFFF,
                      DateTimeConvert.ToUnixTime64(DateTime.Parse("2038-01-19T03:14:07", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)));
    }

    [Test]
    public void TestFromIso14496DateTime()
    {
      Assert.AreEqual(DateTime.Parse("1904-01-01T00:00:00+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      DateTimeConvert.FromISO14496DateTime(0UL));
    }

    [Test]
    public void TestToIso14496DateTime64()
    {
      Assert.AreEqual(0UL,
                      DateTimeConvert.ToISO14496DateTime64(DateTime.Parse("1904-01-01T00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)));
    }
  }
}
