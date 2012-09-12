using System;
using NUnit.Framework;

namespace Smdn.Mathematics {
  [TestFixture()]
  public class PolarTests {
    [Test]
    public void TestConstruct()
    {
      Assert.AreEqual(0.0f, Polar.Zero.Radius);
      Assert.AreEqual((Radian)0.0f, Polar.Zero.Angle);

      var pol1 = new Polar(1.0f, 2.0f);
      var pol2 = new Polar(3.0f, new Radian(4.0f));

      Assert.AreEqual(1.0f, pol1.Radius);
      Assert.AreEqual((Radian)2.0f, pol1.Angle);
      Assert.AreEqual(3.0f, pol2.Radius);
      Assert.AreEqual(4.0f, (float)pol2.Angle);
    }

    [Test]
    public void TestFromCartesian()
    {
      var pol = Polar.FromCartecian(1.0f, 1.0f);

      Assert.AreEqual((float)Math.Sqrt(2.0), pol.Radius);
      Assert.AreEqual(Radian.RightAngle * 0.5f, pol.Angle);
    }

    [Test]
    public void TestConversionToCartesian()
    {
      var pol = new Polar(5.0f, (float)Math.Acos(0.8));

      Assert.AreEqual(4.0f, pol.X);
      Assert.AreEqual(3.0f, pol.Y);
    }

    [Test]
    public void TestRegularized()
    {
      var pol = new Polar(1.0f, 0.0f);

      Assert.AreEqual(new Polar(1.0f, Radian.PI), (new Polar(-1.0f, 0.0f)).Regularized);
      Assert.AreEqual(new Polar(1.0f, Radian.PI), (pol * -1.0f).Regularized);
      Assert.AreEqual(new Polar(1.0f, Radian.PI), (-pol).Regularized);
      Assert.AreEqual(new Polar(1.0f, Radian.PI), (new Polar(1.0f, -Radian.PI)).Regularized);
    }

    [Test]
    public void TestEquatable()
    {
      var pol = new Polar(1.0f, 1.0f);

      Assert.IsTrue(pol.Equals(pol));
      Assert.IsFalse(pol.Equals(null));
    }

    [Test]
    public void TestOperatorSign()
    {
      Assert.AreEqual(new Polar(+1.0f, 0.0f), +(new Polar(+1.0f, 0.0f)));
      Assert.AreEqual(new Polar(-1.0f, 0.0f), +(new Polar(-1.0f, 0.0f)));
      Assert.AreEqual(new Polar(-1.0f, 0.0f), -(new Polar(+1.0f, 0.0f)));
      Assert.AreEqual(new Polar(+1.0f, 0.0f), -(new Polar(-1.0f, 0.0f)));
    }

    [Test]
    public void TestOperatorEquality()
    {
      var x = new Polar(1.0f, 0.0f);
      var y = new Polar(1.0f, 1.0f);

#pragma warning disable 1718
      Assert.IsTrue(x == x);
#pragma warning restore 1718
      Assert.IsTrue(x == (new Polar(1.0f, 0.0f)));
      Assert.IsFalse(x == y);
      Assert.IsFalse(x == Polar.Zero);
    }

    [Test]
    public void TestOperatorInequality()
    {
      var x = new Polar(1.0f, 0.0f);
      var y = new Polar(1.0f, 1.0f);

#pragma warning disable 1718
      Assert.IsFalse(x != x);
#pragma warning restore 1718
      Assert.IsFalse(x != (new Polar(1.0f, 0.0f)));
      Assert.IsTrue(x != y);
      Assert.IsTrue(x != Polar.Zero);
    }

    [Test]
    public void TestOperatorMultiply()
    {
      var pol = new Polar(1.0f, 0.0f);

      Assert.AreEqual(new Polar(2.0f, 0.0f), pol * 2.0f);
      Assert.AreEqual(new Polar(2.0f, 0.0f), 2.0f * pol);
      Assert.AreEqual(new Polar(1.0f, Radian.PI), pol * (new Polar(1.0f, Radian.PI)));
      Assert.AreEqual(new Polar(2.0f, Radian.PI), pol * (new Polar(2.0f, Radian.PI)));
    }

    [Test]
    public void TestOperatorDivide()
    {
      var pol = new Polar(1.0f, 0.0f);

      Assert.AreEqual(new Polar(0.5f, 0.0f), pol / 2.0f);
      Assert.AreEqual(new Polar(2.0f, 0.0f), 2.0f / pol);
      Assert.AreEqual(new Polar(1.0f, -Radian.PI), pol / (new Polar(1.0f, Radian.PI)));
      Assert.AreEqual(new Polar(0.5f, -Radian.PI), pol / (new Polar(2.0f, Radian.PI)));
    }

    /*
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
    public void TestExplicitOperator()
    {
      var rad = new Radian(1.0f);

      Assert.AreEqual((Radian)1.0f, rad);
      Assert.AreEqual(1.0f, (float)rad);
    }
    */
  }
}
