using System;
using System.Globalization;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class UnixTimeStampTests {
    [Test]
    public void TestToUtcDateTime()
    {
      Assert.AreEqual(DateTime.Parse("1970-01-01T00:00:00+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      UnixTimeStamp.ToUtcDateTime(0L));
      Assert.AreEqual(DateTime.Parse("2001-09-09T01:46:40+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      UnixTimeStamp.ToUtcDateTime(1000000000L));
      Assert.AreEqual(DateTime.Parse("2038-01-19T03:14:07+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      UnixTimeStamp.ToUtcDateTime((long)0x7FFFFFFF));
    }

    [Test]
    public void TestToInt64()
    {
      Assert.AreEqual(0L,
                      UnixTimeStamp.ToInt64(DateTime.Parse("1970-01-01T00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)));
      Assert.AreEqual(1000000000L,
                      UnixTimeStamp.ToInt64(DateTime.Parse("2001-09-09T01:46:40", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)));
      Assert.AreEqual((long)0x7FFFFFFF,
                      UnixTimeStamp.ToInt64(DateTime.Parse("2038-01-19T03:14:07", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)));
    }
  }
}
