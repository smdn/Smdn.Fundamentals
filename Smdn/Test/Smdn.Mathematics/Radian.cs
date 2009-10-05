using System;
using NUnit.Framework;

namespace Smdn.Mathematics {
  [TestFixture()]
  public class RadianTests {
    [Test]
    public void TestConstruct()
    {
      Assert.AreEqual((new Radian(12.34f)).Value, 12.34f);
      Assert.AreEqual((new Radian(float.PositiveInfinity)).Value, float.PositiveInfinity);
      Assert.AreEqual((new Radian(float.NegativeInfinity)).Value, float.NegativeInfinity);
      Assert.AreEqual((new Radian(float.NaN)).Value, float.NaN);
    }

    [Test]
    public void TestQuadrant()
    {
      var r = new Radian(1.0f);

      Assert.AreEqual(4, (Radian.Zero - r).Quadrant);
      Assert.AreEqual(1, (Radian.Zero + r).Quadrant);

      Assert.AreEqual(1, (Radian.RightAngle - r).Quadrant);
      Assert.AreEqual(2, (Radian.RightAngle + r).Quadrant);

      Assert.AreEqual(2, (Radian.StraightAngle - r).Quadrant);
      Assert.AreEqual(3, (Radian.StraightAngle + r).Quadrant);

      Assert.AreEqual(3, (Radian.StraightAngle + Radian.RightAngle - r).Quadrant);
      Assert.AreEqual(4, (Radian.StraightAngle + Radian.RightAngle + r).Quadrant);

      Assert.AreEqual(4, (Radian.FullAngle - r).Quadrant);
      Assert.AreEqual(1, (Radian.FullAngle + r).Quadrant);

      Assert.AreEqual(4, (2.0f * Radian.FullAngle - r).Quadrant);
      Assert.AreEqual(1, (2.0f * Radian.FullAngle + r).Quadrant);
    }

    [Test]
    public void TestRegularized()
    {
      Assert.AreEqual(Radian.Zero, Radian.Zero.Regularized);
      Assert.AreEqual(Radian.One, Radian.One.Regularized);
      Assert.AreEqual(Radian.Zero, Radian.FullAngle.Regularized);
      Assert.AreEqual(Radian.Zero, (Radian.Zero + Radian.FullAngle).Regularized, "0 + 2π");
      Assert.AreEqual(Radian.One, (Radian.One + Radian.FullAngle).Regularized, "1 + 2π");
      Assert.AreEqual(Radian.Zero, (Radian.Zero - Radian.FullAngle).Regularized, "0 - 2π");
      Assert.AreEqual(Radian.One, (Radian.One - Radian.FullAngle).Regularized, "1 - 2π");
      Assert.AreEqual(Radian.FullAngle - Radian.One, (-Radian.One - Radian.FullAngle).Regularized, "-1 - 2π");

      for (var r = -10.0f; r < 10.0f; r += 0.1f) {
        var reg = (new Radian(r)).Regularized;

        Assert.IsTrue((Radian.Zero <= reg && reg < Radian.FullAngle));
      }
    }

    [Test]
    public void TestFromDegree()
    {
      Assert.AreEqual(Radian.Zero, Radian.FromDegree(0.0f));
      Assert.AreEqual(Radian.RightAngle, Radian.FromDegree(90.0f));
      Assert.AreEqual(Radian.StraightAngle, Radian.FromDegree(180.0f));
      Assert.AreEqual(Radian.FullAngle, Radian.FromDegree(360.0f));
      Assert.AreEqual(Radian.FullAngle * 2.0f, Radian.FromDegree(720.0f));
    }

    [Test]
    public void TestFourArithmeticOperators()
    {
      Assert.AreEqual(new Radian(+1.0f), +(new Radian(+1.0f)));
      Assert.AreEqual(new Radian(-1.0f), +(new Radian(-1.0f)));
      Assert.AreEqual(new Radian(-1.0f), -(new Radian(+1.0f)));
      Assert.AreEqual(new Radian(+1.0f), -(new Radian(-1.0f)));

      Assert.AreEqual(Radian.StraightAngle, Radian.RightAngle + Radian.RightAngle);
      Assert.AreEqual(Radian.Zero, Radian.RightAngle - Radian.RightAngle);

      Assert.AreEqual(Radian.StraightAngle, Radian.RightAngle * 2.0f);
      Assert.AreEqual(Radian.StraightAngle, 2.0f * Radian.RightAngle);
      Assert.AreEqual(Radian.RightAngle, Radian.StraightAngle / 2.0f);
      Assert.AreEqual(2.0f, Radian.StraightAngle / Radian.RightAngle);
    }

    [Test]
    public void TestComparisonOperators()
    {
      var r = Radian.FromDegree(90.0f);
      var r1 = Radian.FromDegree(90.0f);
      var r2 = Radian.FromDegree(180.0f);

      Assert.IsFalse(r < r1);
      Assert.IsTrue(r < r2);

      Assert.IsTrue(r <= r1);
      Assert.IsTrue(r <= r2);

      Assert.IsFalse(r1 > r);
      Assert.IsTrue(r2 > r);

      Assert.IsTrue(r1 >= r);
      Assert.IsTrue(r2 >= r);
    }

    [Test]
    public void TestEquatable()
    {
      var rad = new Radian(1.0f);
      var f = 1.0f;

      Assert.IsTrue(rad.Equals(rad));
      Assert.IsTrue(rad.Equals(f));
      Assert.IsFalse(rad.Equals(null));
    }

    [Test]
    public void TestOperatorEquality()
    {
      var x = new Radian(0.0f);
      var y = new Radian(1.0f);

      Assert.IsTrue(x == x);
      Assert.IsTrue(x == (new Radian(0.0f)));
      Assert.IsFalse(x == y);
      Assert.IsTrue(x == Radian.Zero);
      Assert.IsFalse(x == Radian.One);
    }

    [Test]
    public void TestOperatorInequality()
    {
      var x = new Radian(0.0f);
      var y = new Radian(1.0f);

      Assert.IsFalse(x != x);
      Assert.IsFalse(x != (new Radian(0.0f)));
      Assert.IsTrue(x != y);
      Assert.IsFalse(x != Radian.Zero);
      Assert.IsTrue(x != Radian.One);
    }

    [Test]
    public void TestExplicitOperator()
    {
      var rad = new Radian(1.0f);

      Assert.AreEqual((Radian)1.0f, rad);
      Assert.AreEqual(1.0f, (float)rad);
    }
  }
}
