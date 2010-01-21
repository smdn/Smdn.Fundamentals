using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture]
  public class MathUtilsTest {
    [Test]
    public void TestHypot()
    {
      Assert.AreEqual(5.0f, MathUtils.Hypot(4.0f, 3.0f));
      Assert.AreEqual(5.0f, MathUtils.Hypot(3.0f, 4.0f));

      Assert.AreEqual(Math.Sqrt(2.0), MathUtils.Hypot(1.0, 1.0));
    }

    [Test]
    public void TestGcd()
    {
      Assert.AreEqual(3, MathUtils.Gcd(3, 0));
      Assert.AreEqual(4, MathUtils.Gcd(8, 4));
      Assert.AreEqual(3, MathUtils.Gcd(12, 9));
      Assert.AreEqual(8, MathUtils.Gcd(128, 72));
    }

    [Test]
    public void TestLcm()
    {
      Assert.AreEqual(0, MathUtils.Lcm(3, 0));
      Assert.AreEqual(36, MathUtils.Lcm(12, 18));
      Assert.AreEqual(187, MathUtils.Lcm(17, 11));
    }

    [Test]
    public void TestGetRandomBytes1()
    {
      var bytes = MathUtils.GetRandomBytes(16);

      Assert.AreEqual(16, bytes.Length);
      Assert.AreNotEqual(new byte[] {0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0}, bytes);
    }

    [Test]
    public void TestGetRandomBytes2()
    {
      var bytes = new byte[16];

      MathUtils.GetRandomBytes(bytes);

      Assert.AreNotEqual(new byte[] {0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0}, bytes);
    }
  }
}