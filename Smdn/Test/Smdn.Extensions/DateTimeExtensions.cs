using System;
using System.Globalization;
using NUnit.Framework;

namespace Smdn.Extensions {
  [TestFixture()]
  public class DateTimeExtensionsTests {
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
      Assert.AreEqual(timezoneOffset, DateTimeExtensions.GetCurrentTimeZoneOffsetString(true));
      Assert.AreEqual(timezoneOffsetNoDelim, DateTimeExtensions.GetCurrentTimeZoneOffsetString(false));
    }

    [Test]
    public void TestToRFC822DateTimeStringUtc()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

      Assert.AreEqual("Mon, 25 Feb 2008 15:01:12 GMT",
                      DateTimeExtensions.ToRFC822DateTimeString(dtm));
    }

    [Test]
    public void TestToRFC822DateTimeStringLocal()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Local);

      Assert.AreEqual("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
                      DateTimeExtensions.ToRFC822DateTimeString(dtm));
    }

    [Test]
    public void TestToRFC822DateTimeStringUnspecifiedKind()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Unspecified);

      Assert.AreEqual("Mon, 25 Feb 2008 15:01:12 " + timezoneOffsetNoDelim,
                      DateTimeExtensions.ToRFC822DateTimeString(dtm));
    }

    [Test]
    public void FromRFC822DateTimeStringUtc()
    {
      var dtm = DateTimeExtensions.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41:01 GMT");

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
      var dtm = DateTimeExtensions.FromRFC822DateTimeString("Tue, 10 Jun 2003 09:41:01 +09:00");

      Assert.AreEqual(DayOfWeek.Tuesday, dtm.DayOfWeek);
      Assert.AreEqual(10, dtm.Day);
      Assert.AreEqual(6, dtm.Month);
      Assert.AreEqual(2003, dtm.Year);
      Assert.AreEqual(9, dtm.Hour);
      Assert.AreEqual(41, dtm.Minute);
      Assert.AreEqual(1, dtm.Second);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind);
    }

    [Test]
    public void TestToISO8601DateTimeStringUtc()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

      Assert.AreEqual(DateTimeExtensions.ToW3CDateTimeString(dtm),
                      DateTimeExtensions.ToISO8601DateTimeString(dtm));
    }

    [Test]
    public void TestToW3CDateTimeStringUtc()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Utc);

      Assert.AreEqual("2008-02-25T15:01:12Z",
                      DateTimeExtensions.ToW3CDateTimeString(dtm));
    }

    [Test]
    public void TestToW3CDateTimeStringLocal()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Local);

      Assert.AreEqual("2008-02-25T15:01:12" + timezoneOffset,
                      DateTimeExtensions.ToW3CDateTimeString(dtm));
    }

    [Test]
    public void TestToW3CDateTimeStringFUnspecifiedKind()
    {
      var dtm = new DateTime(2008, 2, 25, 15, 1, 12, DateTimeKind.Unspecified);

      Assert.AreEqual("2008-02-25T15:01:12" + timezoneOffset,
                      DateTimeExtensions.ToW3CDateTimeString(dtm));
    }

    [Test]
    public void TestFromISO8601DateTimeString()
    {
      var dtm = "2008-04-11T12:34:56Z";

      Assert.AreEqual(DateTimeExtensions.FromW3CDateTimeString(dtm),
                      DateTimeExtensions.FromISO8601DateTimeString(dtm));
    }

    [Test]
    public void TestFromW3CDateTimeStringUtc()
    {
      var dtm = DateTimeExtensions.FromW3CDateTimeString("2008-04-11T12:34:56Z");

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
      var dtm = DateTimeExtensions.FromW3CDateTimeString("2008-04-11T12:34:56 +09:00");

      Assert.AreEqual(2008, dtm.Year);
      Assert.AreEqual(04, dtm.Month);
      Assert.AreEqual(11, dtm.Day);
      Assert.AreEqual(12, dtm.Hour);
      Assert.AreEqual(34, dtm.Minute);
      Assert.AreEqual(56, dtm.Second);
      Assert.AreEqual(DateTimeKind.Local, dtm.Kind);
    }

    [Test]
    public void FromUnixTimeUtcTest()
    {
      Assert.AreEqual(DateTime.Parse("1970-01-01T00:00:00+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      DateTimeExtensions.FromUnixTimeUtc(0L));
      Assert.AreEqual(DateTime.Parse("2001-09-09T01:46:40+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      DateTimeExtensions.FromUnixTimeUtc(1000000000L));
      Assert.AreEqual(DateTime.Parse("2038-01-19T03:14:07+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      DateTimeExtensions.FromUnixTimeUtc((long)0x7FFFFFFF));
    }

    [Test]
    public void ToUnixTime64Test()
    {
      Assert.AreEqual(0L,
                      DateTime.Parse("1970-01-01T00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUnixTime64());
      Assert.AreEqual(1000000000L,
                      DateTime.Parse("2001-09-09T01:46:40", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUnixTime64());
      Assert.AreEqual((long)0x7FFFFFFF,
                      DateTime.Parse("2038-01-19T03:14:07", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUnixTime64());
    }

    [Test]
    public void FromIso14496DateTimeTest()
    {
      Assert.AreEqual(DateTime.Parse("1904-01-01T00:00:00+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      DateTimeExtensions.FromISO14496DateTime(0UL));
    }

    [Test]
    public void ToIso14496DateTime64Test()
    {
      Assert.AreEqual(0UL,
                      DateTime.Parse("1904-01-01T00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToISO14496DateTime64());
    }
  }
}
