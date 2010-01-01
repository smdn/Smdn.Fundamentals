using System;
using System.Globalization;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class Iso14496TimeStampTests {
    [Test]
    public void TestFromIso14496DateTime()
    {
      Assert.AreEqual(DateTime.Parse("1904-01-01T00:00:00+00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                      Iso14496TimeStamp.ToDateTime(0UL));
    }

    [Test]
    public void TestToIso14496DateTime64()
    {
      Assert.AreEqual(0UL,
                      Iso14496TimeStamp.ToUInt64(DateTime.Parse("1904-01-01T00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)));
    }
  }
}
