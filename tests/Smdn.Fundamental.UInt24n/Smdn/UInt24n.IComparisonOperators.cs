// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void TestOpLessThan()
  {
    Assert.That(UInt24.Zero < (UInt24)0, Is.False, "#1");
    Assert.That(UInt24.Zero < (UInt24)1, Is.True, "#2");
    Assert.That((UInt24)1 < UInt24.Zero, Is.False, "#3");

    Assert.That((UInt24)0xffffff < UInt24.MaxValue, Is.False, "#4");
    Assert.That(UInt24.MaxValue < (UInt24)0xfffffe, Is.False, "#5");
    Assert.That((UInt24)0xfffffe < UInt24.MaxValue, Is.True, "#6");
  }

  [Test]
  public void TestOpLessThanOrEqual()
  {
    Assert.That(UInt24.Zero <= (UInt24)0, Is.True, "#1");
    Assert.That(UInt24.Zero <= (UInt24)1, Is.True, "#2");
    Assert.That((UInt24)1 <= UInt24.Zero, Is.False, "#3");

    Assert.That((UInt24)0xffffff <= UInt24.MaxValue, Is.True, "#4");
    Assert.That(UInt24.MaxValue <= (UInt24)0xfffffe, Is.False, "#5");
    Assert.That((UInt24)0xfffffe <= UInt24.MaxValue, Is.True, "#6");
  }

  [Test]
  public void TestOpGreaterThan()
  {
    Assert.That(UInt24.Zero > (UInt24)0, Is.False, "#1");
    Assert.That(UInt24.Zero > (UInt24)1, Is.False, "#2");
    Assert.That((UInt24)1 > UInt24.Zero, Is.True, "#3");

    Assert.That((UInt24)0xffffff > UInt24.MaxValue, Is.False, "#4");
    Assert.That(UInt24.MaxValue > (UInt24)0xfffffe, Is.True, "#5");
    Assert.That((UInt24)0xfffffe > UInt24.MaxValue, Is.False, "#6");
  }

  [Test]
  public void TestOpGreaterThanOrEqual()
  {
    Assert.That(UInt24.Zero >= (UInt24)0, Is.True, "#1");
    Assert.That(UInt24.Zero >= (UInt24)1, Is.False, "#2");
    Assert.That((UInt24)1 >= UInt24.Zero, Is.True, "#3");

    Assert.That((UInt24)0xffffff >= UInt24.MaxValue, Is.True, "#4");
    Assert.That(UInt24.MaxValue >= (UInt24)0xfffffe, Is.True, "#5");
    Assert.That((UInt24)0xfffffe >= UInt24.MaxValue, Is.False, "#6");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpLessThan()
  {
    Assert.That(UInt48.Zero < (UInt48)0, Is.False, "#1");
    Assert.That(UInt48.Zero < (UInt48)1, Is.True, "#2");
    Assert.That((UInt48)1 < UInt48.Zero, Is.False, "#3");

    Assert.That((UInt48)0xffffffffffff < UInt48.MaxValue, Is.False, "#4");
    Assert.That(UInt48.MaxValue < (UInt48)0xfffffffffffe, Is.False, "#5");
    Assert.That((UInt48)0xfffffffffffe < UInt48.MaxValue, Is.True, "#6");
  }

  [Test]
  public void TestOpLessThanOrEqual()
  {
    Assert.That(UInt48.Zero <= (UInt48)0, Is.True, "#1");
    Assert.That(UInt48.Zero <= (UInt48)1, Is.True, "#2");
    Assert.That((UInt48)1 <= UInt48.Zero, Is.False, "#3");

    Assert.That((UInt48)0xffffffffffff <= UInt48.MaxValue, Is.True, "#4");
    Assert.That(UInt48.MaxValue <= (UInt48)0xfffffffffffe, Is.False, "#5");
    Assert.That((UInt48)0xfffffffffffe <= UInt48.MaxValue, Is.True, "#6");
  }

  [Test]
  public void TestOpGreaterThan()
  {
    Assert.That(UInt48.Zero > (UInt48)0, Is.False, "#1");
    Assert.That(UInt48.Zero > (UInt48)1, Is.False, "#2");
    Assert.That((UInt48)1 > UInt48.Zero, Is.True, "#3");

    Assert.That((UInt48)0xffffffffffff > UInt48.MaxValue, Is.False, "#4");
    Assert.That(UInt48.MaxValue > (UInt48)0xfffffffffffe, Is.True, "#5");
    Assert.That((UInt48)0xfffffffffffe > UInt48.MaxValue, Is.False, "#6");
  }

  [Test]
  public void TestOpGreaterThanOrEqual()
  {
    Assert.That(UInt48.Zero >= (UInt48)0, Is.True, "#1");
    Assert.That(UInt48.Zero >= (UInt48)1, Is.False, "#2");
    Assert.That((UInt48)1 >= UInt48.Zero, Is.True, "#3");

    Assert.That((UInt48)0xffffffffffff >= UInt48.MaxValue, Is.True, "#4");
    Assert.That(UInt48.MaxValue >= (UInt48)0xfffffffffffe, Is.True, "#5");
    Assert.That((UInt48)0xfffffffffffe >= UInt48.MaxValue, Is.False, "#6");
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
