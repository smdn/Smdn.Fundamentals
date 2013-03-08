using System;
using System.Globalization;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class UnixTimeStampTests {
    [Test]
    public void TestToUtcDateTime()
    {
      Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                      UnixTimeStamp.ToUtcDateTime(0L));
      Assert.AreEqual(new DateTime(2001, 9, 9, 1, 46, 40, DateTimeKind.Utc),
                      UnixTimeStamp.ToUtcDateTime(1000000000L));
      Assert.AreEqual(new DateTime(2038, 1, 19, 3, 14, 7, DateTimeKind.Utc),
                      UnixTimeStamp.ToUtcDateTime((long)0x7FFFFFFF));

      Assert.AreEqual(UnixTimeStamp.ToUtcDateTime(0L),
                      UnixTimeStamp.ToUtcDateTime(0));
      Assert.AreEqual(UnixTimeStamp.ToUtcDateTime((long)0x7FFFFFFF),
                      UnixTimeStamp.ToUtcDateTime((int)0x7FFFFFFF));
    }

    [Test]
    public void TestToLocalDateTime()
    {
      Assert.AreEqual(new DateTime(1970, 1, 1, 9, 0, 0, DateTimeKind.Local),
                      UnixTimeStamp.ToLocalDateTime(0L));
      Assert.AreEqual(new DateTime(2001, 9, 9, 10, 46, 40, DateTimeKind.Local),
                      UnixTimeStamp.ToLocalDateTime(1000000000L));
      Assert.AreEqual(new DateTime(2038, 1, 19, 12, 14, 7, DateTimeKind.Local),
                      UnixTimeStamp.ToLocalDateTime((long)0x7FFFFFFF));
    }

    [Test]
    public void TestToInt64()
    {
      Assert.AreEqual(0L,
                      UnixTimeStamp.ToInt64(      DateTime.Parse("1970-01-01T00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)));
      Assert.AreEqual(0L,
                      UnixTimeStamp.ToInt64(DateTimeOffset.Parse("1970-01-01T00:00:00+00:00", CultureInfo.InvariantCulture, DateTimeStyles.None)));
      Assert.AreEqual(1000000000L,
                      UnixTimeStamp.ToInt64(      DateTime.Parse("2001-09-09T01:46:40", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)));
      Assert.AreEqual(1000000000L,
                      UnixTimeStamp.ToInt64(DateTimeOffset.Parse("2001-09-09T01:46:40+00:00", CultureInfo.InvariantCulture, DateTimeStyles.None)));
      Assert.AreEqual(1000000000L,
                      UnixTimeStamp.ToInt64(DateTimeOffset.Parse("2001-09-09T10:46:40+09:00", CultureInfo.InvariantCulture, DateTimeStyles.None)));
      Assert.AreEqual((long)0x7FFFFFFF,
                      UnixTimeStamp.ToInt64(      DateTime.Parse("2038-01-19T03:14:07", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)));
      Assert.AreEqual((long)0x7FFFFFFF,
                      UnixTimeStamp.ToInt64(DateTimeOffset.Parse("2038-01-19T03:14:07+00:00", CultureInfo.InvariantCulture, DateTimeStyles.None)));
      Assert.AreEqual((long)0x7FFFFFFF,
                      UnixTimeStamp.ToInt64(DateTimeOffset.Parse("2038-01-19T12:14:07+09:00", CultureInfo.InvariantCulture, DateTimeStyles.None)));
    }

    [Test]
    public void TestToInt32ToInt64()
    {
      foreach (var dateTime in new[] {
        DateTime.Parse("1970-01-01T00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
        DateTime.Parse("2001-09-09T01:46:40", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
        DateTime.Parse("2038-01-19T03:14:07", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
      }) {
        Assert.AreEqual(UnixTimeStamp.ToInt64(dateTime),
                        UnixTimeStamp.ToInt32(dateTime),
                        "dateTime = {0}",
                        dateTime);
      }

      foreach (var dateTimeOffset in new[] {
        DateTimeOffset.Parse("1970-01-01T00:00:00+00:00", CultureInfo.InvariantCulture, DateTimeStyles.None),
        DateTimeOffset.Parse("2001-09-09T01:46:40+00:00", CultureInfo.InvariantCulture, DateTimeStyles.None),
        DateTimeOffset.Parse("2001-09-09T10:46:40+09:00", CultureInfo.InvariantCulture, DateTimeStyles.None),
        DateTimeOffset.Parse("2038-01-19T03:14:07+00:00", CultureInfo.InvariantCulture, DateTimeStyles.None),
        DateTimeOffset.Parse("2038-01-19T12:14:07+09:00", CultureInfo.InvariantCulture, DateTimeStyles.None),
      }) {
        Assert.AreEqual(UnixTimeStamp.ToInt64(dateTimeOffset),
                        UnixTimeStamp.ToInt32(dateTimeOffset),
                        "dateTimeOffset = {0}",
                        dateTimeOffset);
      }
    }
  }
}
