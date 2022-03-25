// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void TestOpLessThan()
  {
    Assert.IsFalse(UInt24.Zero < (UInt24)0, "#1");
    Assert.IsTrue(UInt24.Zero < (UInt24)1, "#2");
    Assert.IsFalse((UInt24)1 < UInt24.Zero, "#3");

    Assert.IsFalse((UInt24)0xffffff < UInt24.MaxValue, "#4");
    Assert.IsFalse(UInt24.MaxValue < (UInt24)0xfffffe, "#5");
    Assert.IsTrue((UInt24)0xfffffe < UInt24.MaxValue, "#6");
  }

  [Test]
  public void TestOpLessThanOrEqual()
  {
    Assert.IsTrue(UInt24.Zero <= (UInt24)0, "#1");
    Assert.IsTrue(UInt24.Zero <= (UInt24)1, "#2");
    Assert.IsFalse((UInt24)1 <= UInt24.Zero, "#3");

    Assert.IsTrue((UInt24)0xffffff <= UInt24.MaxValue, "#4");
    Assert.IsFalse(UInt24.MaxValue <= (UInt24)0xfffffe, "#5");
    Assert.IsTrue((UInt24)0xfffffe <= UInt24.MaxValue, "#6");
  }

  [Test]
  public void TestOpGreaterThan()
  {
    Assert.IsFalse(UInt24.Zero > (UInt24)0, "#1");
    Assert.IsFalse(UInt24.Zero > (UInt24)1, "#2");
    Assert.IsTrue((UInt24)1 > UInt24.Zero, "#3");

    Assert.IsFalse((UInt24)0xffffff > UInt24.MaxValue, "#4");
    Assert.IsTrue(UInt24.MaxValue > (UInt24)0xfffffe, "#5");
    Assert.IsFalse((UInt24)0xfffffe > UInt24.MaxValue, "#6");
  }

  [Test]
  public void TestOpGreaterThanOrEqual()
  {
    Assert.IsTrue(UInt24.Zero >= (UInt24)0, "#1");
    Assert.IsFalse(UInt24.Zero >= (UInt24)1, "#2");
    Assert.IsTrue((UInt24)1 >= UInt24.Zero, "#3");

    Assert.IsTrue((UInt24)0xffffff >= UInt24.MaxValue, "#4");
    Assert.IsTrue(UInt24.MaxValue >= (UInt24)0xfffffe, "#5");
    Assert.IsFalse((UInt24)0xfffffe >= UInt24.MaxValue, "#6");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpLessThan()
  {
    Assert.IsFalse(UInt48.Zero < (UInt48)0, "#1");
    Assert.IsTrue(UInt48.Zero < (UInt48)1, "#2");
    Assert.IsFalse((UInt48)1 < UInt48.Zero, "#3");

    Assert.IsFalse((UInt48)0xffffffffffff < UInt48.MaxValue, "#4");
    Assert.IsFalse(UInt48.MaxValue < (UInt48)0xfffffffffffe, "#5");
    Assert.IsTrue((UInt48)0xfffffffffffe < UInt48.MaxValue, "#6");
  }

  [Test]
  public void TestOpLessThanOrEqual()
  {
    Assert.IsTrue(UInt48.Zero <= (UInt48)0, "#1");
    Assert.IsTrue(UInt48.Zero <= (UInt48)1, "#2");
    Assert.IsFalse((UInt48)1 <= UInt48.Zero, "#3");

    Assert.IsTrue((UInt48)0xffffffffffff <= UInt48.MaxValue, "#4");
    Assert.IsFalse(UInt48.MaxValue <= (UInt48)0xfffffffffffe, "#5");
    Assert.IsTrue((UInt48)0xfffffffffffe <= UInt48.MaxValue, "#6");
  }

  [Test]
  public void TestOpGreaterThan()
  {
    Assert.IsFalse(UInt48.Zero > (UInt48)0, "#1");
    Assert.IsFalse(UInt48.Zero > (UInt48)1, "#2");
    Assert.IsTrue((UInt48)1 > UInt48.Zero, "#3");

    Assert.IsFalse((UInt48)0xffffffffffff > UInt48.MaxValue, "#4");
    Assert.IsTrue(UInt48.MaxValue > (UInt48)0xfffffffffffe, "#5");
    Assert.IsFalse((UInt48)0xfffffffffffe > UInt48.MaxValue, "#6");
  }

  [Test]
  public void TestOpGreaterThanOrEqual()
  {
    Assert.IsTrue(UInt48.Zero >= (UInt48)0, "#1");
    Assert.IsFalse(UInt48.Zero >= (UInt48)1, "#2");
    Assert.IsTrue((UInt48)1 >= UInt48.Zero, "#3");

    Assert.IsTrue((UInt48)0xffffffffffff >= UInt48.MaxValue, "#4");
    Assert.IsTrue(UInt48.MaxValue >= (UInt48)0xfffffffffffe, "#5");
    Assert.IsFalse((UInt48)0xfffffffffffe >= UInt48.MaxValue, "#6");
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IComparisonOperators_GreaterThan()
  {
    Assert.IsFalse(GreaterThan(UInt24.Zero, UInt24.Zero), "UInt24.Zero > UInt24.Zero");
    Assert.IsTrue(GreaterThan(UInt24.One, UInt24.Zero), "UInt24.One > UInt24.Zero");
    Assert.IsFalse(GreaterThan(UInt24.Zero, UInt24.One), "UInt24.Zero > UInt24.One");
    Assert.IsFalse(GreaterThan(UInt24.One, UInt24.One), "UInt24.One > UInt24.One");

    Assert.IsFalse(GreaterThan(UInt48.Zero, UInt48.Zero), "UInt48.Zero > UInt48.Zero");
    Assert.IsTrue(GreaterThan(UInt48.One, UInt48.Zero), "UInt48.One > UInt48.Zero");
    Assert.IsFalse(GreaterThan(UInt48.Zero, UInt48.One), "UInt48.Zero > UInt48.One");
    Assert.IsFalse(GreaterThan(UInt48.One, UInt48.One), "UInt48.One > UInt48.One");

    static bool GreaterThan<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IComparisonOperators<TUInt24n, TUInt24n>
      => x > y;
  }

  [Test]
  public void IComparisonOperators_GreaterThanOrEqual()
  {
    Assert.IsTrue(GreaterThanOrEqual(UInt24.Zero, UInt24.Zero), "UInt24.Zero >= UInt24.Zero");
    Assert.IsTrue(GreaterThanOrEqual(UInt24.One, UInt24.Zero), "UInt24.One >= UInt24.Zero");
    Assert.IsFalse(GreaterThanOrEqual(UInt24.Zero, UInt24.One), "UInt24.Zero >= UInt24.One");
    Assert.IsTrue(GreaterThanOrEqual(UInt24.One, UInt24.One), "UInt24.One >= UInt24.One");

    Assert.IsTrue(GreaterThanOrEqual(UInt48.Zero, UInt48.Zero), "UInt48.Zero >= UInt48.Zero");
    Assert.IsTrue(GreaterThanOrEqual(UInt48.One, UInt48.Zero), "UInt48.One >= UInt48.Zero");
    Assert.IsFalse(GreaterThanOrEqual(UInt48.Zero, UInt48.One), "UInt48.Zero >= UInt48.One");
    Assert.IsTrue(GreaterThanOrEqual(UInt48.One, UInt48.One), "UInt48.One >= UInt48.One");

    static bool GreaterThanOrEqual<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IComparisonOperators<TUInt24n, TUInt24n>
      => x >= y;
  }

  [Test]
  public void IComparisonOperators_LessThan()
  {
    Assert.IsFalse(LessThan(UInt24.Zero, UInt24.Zero), "UInt24.Zero < UInt24.Zero");
    Assert.IsFalse(LessThan(UInt24.One, UInt24.Zero), "UInt24.One < UInt24.Zero");
    Assert.IsTrue(LessThan(UInt24.Zero, UInt24.One), "UInt24.Zero < UInt24.One");
    Assert.IsFalse(LessThan(UInt24.One, UInt24.One), "UInt24.One < UInt24.One");

    Assert.IsFalse(LessThan(UInt48.Zero, UInt48.Zero), "UInt48.Zero < UInt48.Zero");
    Assert.IsFalse(LessThan(UInt48.One, UInt48.Zero), "UInt48.One < UInt48.Zero");
    Assert.IsTrue(LessThan(UInt48.Zero, UInt48.One), "UInt48.Zero < UInt48.One");
    Assert.IsFalse(LessThan(UInt48.One, UInt48.One), "UInt48.One < UInt48.One");

    static bool LessThan<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IComparisonOperators<TUInt24n, TUInt24n>
      => x < y;
  }

  [Test]
  public void IComparisonOperators_LessThanOrEqual()
  {
    Assert.IsTrue(LessThanOrEqual(UInt24.Zero, UInt24.Zero), "UInt24.Zero <= UInt24.Zero");
    Assert.IsFalse(LessThanOrEqual(UInt24.One, UInt24.Zero), "UInt24.One <= UInt24.Zero");
    Assert.IsTrue(LessThanOrEqual(UInt24.Zero, UInt24.One), "UInt24.Zero <= UInt24.One");
    Assert.IsTrue(LessThanOrEqual(UInt24.One, UInt24.One), "UInt24.One <= UInt24.One");

    Assert.IsTrue(LessThanOrEqual(UInt48.Zero, UInt48.Zero), "UInt48.Zero <= UInt48.Zero");
    Assert.IsFalse(LessThanOrEqual(UInt48.One, UInt48.Zero), "UInt48.One <= UInt48.Zero");
    Assert.IsTrue(LessThanOrEqual(UInt48.Zero, UInt48.One), "UInt48.Zero <= UInt48.One");
    Assert.IsTrue(LessThanOrEqual(UInt48.One, UInt48.One), "UInt48.One <= UInt48.One");

    static bool LessThanOrEqual<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IComparisonOperators<TUInt24n, TUInt24n>
      => x <= y;
  }
#endif
}
