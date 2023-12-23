// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_ICOMPARISONOPERATORS
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void OpLessThan()
  {
    Assert.That(UInt24.Zero < (UInt24)0, Is.False, "#1");
    Assert.That(UInt24.Zero < (UInt24)1, Is.True, "#2");
    Assert.That((UInt24)1 < UInt24.Zero, Is.False, "#3");

    Assert.That((UInt24)0xffffff < UInt24.MaxValue, Is.False, "#4");
    Assert.That(UInt24.MaxValue < (UInt24)0xfffffe, Is.False, "#5");
    Assert.That((UInt24)0xfffffe < UInt24.MaxValue, Is.True, "#6");
  }

  [Test]
  public void OpLessThanOrEqual()
  {
    Assert.That(UInt24.Zero <= (UInt24)0, Is.True, "#1");
    Assert.That(UInt24.Zero <= (UInt24)1, Is.True, "#2");
    Assert.That((UInt24)1 <= UInt24.Zero, Is.False, "#3");

    Assert.That((UInt24)0xffffff <= UInt24.MaxValue, Is.True, "#4");
    Assert.That(UInt24.MaxValue <= (UInt24)0xfffffe, Is.False, "#5");
    Assert.That((UInt24)0xfffffe <= UInt24.MaxValue, Is.True, "#6");
  }

  [Test]
  public void OpGreaterThan()
  {
    Assert.That(UInt24.Zero > (UInt24)0, Is.False, "#1");
    Assert.That(UInt24.Zero > (UInt24)1, Is.False, "#2");
    Assert.That((UInt24)1 > UInt24.Zero, Is.True, "#3");

    Assert.That((UInt24)0xffffff > UInt24.MaxValue, Is.False, "#4");
    Assert.That(UInt24.MaxValue > (UInt24)0xfffffe, Is.True, "#5");
    Assert.That((UInt24)0xfffffe > UInt24.MaxValue, Is.False, "#6");
  }

  [Test]
  public void OpGreaterThanOrEqual()
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
  public void OpLessThan()
  {
    Assert.That(UInt48.Zero < (UInt48)0, Is.False, "#1");
    Assert.That(UInt48.Zero < (UInt48)1, Is.True, "#2");
    Assert.That((UInt48)1 < UInt48.Zero, Is.False, "#3");

    Assert.That((UInt48)0xffffffffffff < UInt48.MaxValue, Is.False, "#4");
    Assert.That(UInt48.MaxValue < (UInt48)0xfffffffffffe, Is.False, "#5");
    Assert.That((UInt48)0xfffffffffffe < UInt48.MaxValue, Is.True, "#6");
  }

  [Test]
  public void OpLessThanOrEqual()
  {
    Assert.That(UInt48.Zero <= (UInt48)0, Is.True, "#1");
    Assert.That(UInt48.Zero <= (UInt48)1, Is.True, "#2");
    Assert.That((UInt48)1 <= UInt48.Zero, Is.False, "#3");

    Assert.That((UInt48)0xffffffffffff <= UInt48.MaxValue, Is.True, "#4");
    Assert.That(UInt48.MaxValue <= (UInt48)0xfffffffffffe, Is.False, "#5");
    Assert.That((UInt48)0xfffffffffffe <= UInt48.MaxValue, Is.True, "#6");
  }

  [Test]
  public void OpGreaterThan()
  {
    Assert.That(UInt48.Zero > (UInt48)0, Is.False, "#1");
    Assert.That(UInt48.Zero > (UInt48)1, Is.False, "#2");
    Assert.That((UInt48)1 > UInt48.Zero, Is.True, "#3");

    Assert.That((UInt48)0xffffffffffff > UInt48.MaxValue, Is.False, "#4");
    Assert.That(UInt48.MaxValue > (UInt48)0xfffffffffffe, Is.True, "#5");
    Assert.That((UInt48)0xfffffffffffe > UInt48.MaxValue, Is.False, "#6");
  }

  [Test]
  public void OpGreaterThanOrEqual()
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
#if SYSTEM_NUMERICS_ICOMPARISONOPERATORS
  [Test]
  public void IComparisonOperators_GreaterThan()
  {
    Assert.That(GreaterThan(UInt24.Zero, UInt24.Zero), Is.False, "UInt24.Zero > UInt24.Zero");
    Assert.That(GreaterThan(UInt24.One, UInt24.Zero), Is.True, "UInt24.One > UInt24.Zero");
    Assert.That(GreaterThan(UInt24.Zero, UInt24.One), Is.False, "UInt24.Zero > UInt24.One");
    Assert.That(GreaterThan(UInt24.One, UInt24.One), Is.False, "UInt24.One > UInt24.One");

    Assert.That(GreaterThan(UInt48.Zero, UInt48.Zero), Is.False, "UInt48.Zero > UInt48.Zero");
    Assert.That(GreaterThan(UInt48.One, UInt48.Zero), Is.True, "UInt48.One > UInt48.Zero");
    Assert.That(GreaterThan(UInt48.Zero, UInt48.One), Is.False, "UInt48.Zero > UInt48.One");
    Assert.That(GreaterThan(UInt48.One, UInt48.One), Is.False, "UInt48.One > UInt48.One");

    static bool GreaterThan<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IComparisonOperators<TUInt24n, TUInt24n, bool>
      => x > y;
  }

  [Test]
  public void IComparisonOperators_GreaterThanOrEqual()
  {
    Assert.That(GreaterThanOrEqual(UInt24.Zero, UInt24.Zero), Is.True, "UInt24.Zero >= UInt24.Zero");
    Assert.That(GreaterThanOrEqual(UInt24.One, UInt24.Zero), Is.True, "UInt24.One >= UInt24.Zero");
    Assert.That(GreaterThanOrEqual(UInt24.Zero, UInt24.One), Is.False, "UInt24.Zero >= UInt24.One");
    Assert.That(GreaterThanOrEqual(UInt24.One, UInt24.One), Is.True, "UInt24.One >= UInt24.One");

    Assert.That(GreaterThanOrEqual(UInt48.Zero, UInt48.Zero), Is.True, "UInt48.Zero >= UInt48.Zero");
    Assert.That(GreaterThanOrEqual(UInt48.One, UInt48.Zero), Is.True, "UInt48.One >= UInt48.Zero");
    Assert.That(GreaterThanOrEqual(UInt48.Zero, UInt48.One), Is.False, "UInt48.Zero >= UInt48.One");
    Assert.That(GreaterThanOrEqual(UInt48.One, UInt48.One), Is.True, "UInt48.One >= UInt48.One");

    static bool GreaterThanOrEqual<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IComparisonOperators<TUInt24n, TUInt24n, bool>
      => x >= y;
  }

  [Test]
  public void IComparisonOperators_LessThan()
  {
    Assert.That(LessThan(UInt24.Zero, UInt24.Zero), Is.False, "UInt24.Zero < UInt24.Zero");
    Assert.That(LessThan(UInt24.One, UInt24.Zero), Is.False, "UInt24.One < UInt24.Zero");
    Assert.That(LessThan(UInt24.Zero, UInt24.One), Is.True, "UInt24.Zero < UInt24.One");
    Assert.That(LessThan(UInt24.One, UInt24.One), Is.False, "UInt24.One < UInt24.One");

    Assert.That(LessThan(UInt48.Zero, UInt48.Zero), Is.False, "UInt48.Zero < UInt48.Zero");
    Assert.That(LessThan(UInt48.One, UInt48.Zero), Is.False, "UInt48.One < UInt48.Zero");
    Assert.That(LessThan(UInt48.Zero, UInt48.One), Is.True, "UInt48.Zero < UInt48.One");
    Assert.That(LessThan(UInt48.One, UInt48.One), Is.False, "UInt48.One < UInt48.One");

    static bool LessThan<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IComparisonOperators<TUInt24n, TUInt24n, bool>
      => x < y;
  }

  [Test]
  public void IComparisonOperators_LessThanOrEqual()
  {
    Assert.That(LessThanOrEqual(UInt24.Zero, UInt24.Zero), Is.True, "UInt24.Zero <= UInt24.Zero");
    Assert.That(LessThanOrEqual(UInt24.One, UInt24.Zero), Is.False, "UInt24.One <= UInt24.Zero");
    Assert.That(LessThanOrEqual(UInt24.Zero, UInt24.One), Is.True, "UInt24.Zero <= UInt24.One");
    Assert.That(LessThanOrEqual(UInt24.One, UInt24.One), Is.True, "UInt24.One <= UInt24.One");

    Assert.That(LessThanOrEqual(UInt48.Zero, UInt48.Zero), Is.True, "UInt48.Zero <= UInt48.Zero");
    Assert.That(LessThanOrEqual(UInt48.One, UInt48.Zero), Is.False, "UInt48.One <= UInt48.Zero");
    Assert.That(LessThanOrEqual(UInt48.Zero, UInt48.One), Is.True, "UInt48.Zero <= UInt48.One");
    Assert.That(LessThanOrEqual(UInt48.One, UInt48.One), Is.True, "UInt48.One <= UInt48.One");

    static bool LessThanOrEqual<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IComparisonOperators<TUInt24n, TUInt24n, bool>
      => x <= y;
  }
#endif
}
