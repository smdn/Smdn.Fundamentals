using System;
using NUnit.Framework;

namespace Smdn.Mathematics {
  [TestFixture]
  public class FractionTest {
    [Test]
    public void TestToDouble()
    {
      Assert.AreEqual(5994, (int)((new Fraction(60000, 1001)).ToDouble() * 100.0));
      Assert.AreEqual(2997, (int)((new Fraction(30000, 1001)).ToDouble() * 100.0));
    }

    [Test]
    public void TestMulDiv()
    {
      var f1 = new Fraction(48000, 1);
      var f2 = new Fraction(30000, 1001);

      Assert.AreEqual(0000, 0 * f1 / f2);
      Assert.AreEqual(1601, 1 * f1 / f2);
      Assert.AreEqual(3203, 2 * f1 / f2);
      Assert.AreEqual(4804, 3 * f1 / f2);
      Assert.AreEqual(6406, 4 * f1 / f2);
      Assert.AreEqual(8008, 5 * f1 / f2);
      Assert.AreEqual(48048, 30 * f1 / f2);

      Assert.AreEqual(1280, 720 * new Fraction(16, 9));
    }
  }
}