using System;
using NUnit.Framework;

namespace Smdn.Mathematics {
  [TestFixture]
  public class OperationsTest {
    [Test]
    public void TestHypot()
    {
      Assert.AreEqual(5.0f, Operations.Hypot(4.0f, 3.0f));
      Assert.AreEqual(5.0f, Operations.Hypot(3.0f, 4.0f));

      Assert.AreEqual(Math.Sqrt(2.0), Operations.Hypot(1.0, 1.0));
    }

    [Test]
    public void TestGCD()
    {
      Assert.AreEqual(3, Operations.GCD(3, 0));
      Assert.AreEqual(4, Operations.GCD(8, 4));
      Assert.AreEqual(3, Operations.GCD(12, 9));
      Assert.AreEqual(8, Operations.GCD(128, 72));
    }

    [Test]
    public void TestLCM()
    {
      Assert.AreEqual(0, Operations.LCM(3, 0));
      Assert.AreEqual(36, Operations.LCM(12, 18));
      Assert.AreEqual(187, Operations.LCM(17, 11));
    }
  }
}