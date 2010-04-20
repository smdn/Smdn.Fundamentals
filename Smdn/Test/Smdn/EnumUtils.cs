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
      Assert.AreEqual(DayOfWeek.Sunday, EnumUtils.ParseIgnoreCase<DayOfWeek>("sUndaY"));

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

#if NET_4_0
      Assert.IsTrue(Enum.TryParse<DayOfWeek>("Friday", out result));
      Assert.AreEqual(DayOfWeek.Friday, result);

      Assert.IsTrue(Enum.TryParse<DayOfWeek>("fRiDay", true, out result));
      Assert.AreEqual(DayOfWeek.Friday, result);

      Assert.IsTrue(EnumUtils.TryParseIgnoreCase<DayOfWeek>("fRiDay", out result));
      Assert.AreEqual(DayOfWeek.Friday, result);

      Assert.IsFalse(Enum.TryParse<DayOfWeek>("fRiDay", false, out result));
#else
      Assert.IsTrue(EnumUtils.TryParse<DayOfWeek>("Friday", out result));
      Assert.AreEqual(DayOfWeek.Friday, result);

      Assert.IsTrue(EnumUtils.TryParse<DayOfWeek>("fRiDay", true, out result));
      Assert.AreEqual(DayOfWeek.Friday, result);

      Assert.IsTrue(EnumUtils.TryParseIgnoreCase<DayOfWeek>("fRiDay", out result));
      Assert.AreEqual(DayOfWeek.Friday, result);

      Assert.IsFalse(EnumUtils.TryParse<DayOfWeek>("fRiDay", false, out result));
#endif
    }

    [Test, ExpectedException(typeof(ArgumentException))]
    public void TestTryParseNonEnumType()
    {
      Guid result;

#if NET_4_0
      Enum.TryParse<Guid>("test", out result);
#else
      EnumUtils.TryParse<Guid>("test", out result);
#endif
    }

    [Flags] enum Colors { None=0, Red = 1, Green = 2, Blue = 4 };

    [Test]
    public void TestTryParseFlagsEnum()
    {
      Colors colorValue;

#if NET_4_0
      Assert.IsTrue(Enum.TryParse("Red, Green", true, out colorValue));
#else
      Assert.IsTrue(EnumUtils.TryParse("Red, Green", true, out colorValue));
#endif

      Assert.IsTrue((int)(colorValue & Colors.Red) != 0);
      Assert.IsTrue((int)(colorValue & Colors.Green) != 0);
    }
  }
}