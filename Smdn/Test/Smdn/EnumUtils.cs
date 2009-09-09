using System;
using System.IO;
using NUnit.Framework;

namespace Smdn {
  [TestFixture]
  public class EnumUtilsTests {
    [Test]
    public void TestParse()
    {
      Assert.AreEqual(DayOfWeek.Sunday, EnumUtils.Parse<DayOfWeek>("Sunday"));
      Assert.AreEqual(DayOfWeek.Monday, EnumUtils.Parse<DayOfWeek>("Monday"));
      Assert.AreEqual(DayOfWeek.Tuesday, EnumUtils.Parse<DayOfWeek>("Tuesday"));
      Assert.AreEqual(DayOfWeek.Wednesday, EnumUtils.Parse<DayOfWeek>("Wednesday"));

      Assert.AreEqual(DayOfWeek.Sunday, EnumUtils.Parse<DayOfWeek>("sUndaY", true));

      try {
        Assert.AreEqual(DayOfWeek.Sunday, EnumUtils.Parse<DayOfWeek>("sUndaY"));
        Assert.Fail("exception not thrown");
      }
      catch {
      }

      try {
        Assert.AreEqual(DayOfWeek.Sunday, EnumUtils.Parse<DayOfWeek>("sUndaY", false));
        Assert.Fail("exception not thrown");
      }
      catch {
      }
    }

    [Test]
    public void TestTryParse()
    {
      DayOfWeek result;

      Assert.IsTrue(EnumUtils.TryParse<DayOfWeek>("Friday", out result));
      Assert.AreEqual(DayOfWeek.Friday, result);

      Assert.IsTrue(EnumUtils.TryParse<DayOfWeek>("fRiDay", true, out result));
      Assert.AreEqual(DayOfWeek.Friday, result);

      Assert.IsFalse(EnumUtils.TryParse<DayOfWeek>("fRiDay", false, out result));
    }
  }
}