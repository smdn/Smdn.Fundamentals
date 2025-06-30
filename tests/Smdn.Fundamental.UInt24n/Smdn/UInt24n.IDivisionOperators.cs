// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IDIVISIONOPERATORS
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [Test]
  public void OpDivision()
  {
    Assert.Throws<DivideByZeroException>(() => Assert.That(UInt24.Zero / UInt24.Zero, Is.EqualTo(UInt24.One)), "0 / 0");
    Assert.Throws<DivideByZeroException>(() => Assert.That(UInt24.One / UInt24.Zero, Is.EqualTo(UInt24.One)), "1 / 0");
    Assert.That(UInt24.Zero / UInt24.One, Is.EqualTo(UInt24.Zero), "0 / 1");
    Assert.That(UInt24.One / UInt24.One, Is.EqualTo(UInt24.One), "1 / 1");
    Assert.That(UInt24.MaxValue / UInt24.MaxValue, Is.EqualTo(UInt24.One), "Max / Max");
  }

  [Test]
  public void OpCheckedDivision()
  {
    Assert.That(checked(UInt24.Zero / UInt24.One), Is.EqualTo(UInt24.Zero), "0 / 1");
    Assert.That(checked(UInt24.One / UInt24.One), Is.EqualTo(UInt24.One), "1 / 1");
  }

  [Test]
  public void OpCheckedDivision_Overflow()
  {
    Assert.Ignore("no test cases");
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test]
  public void OpDivision()
  {
    Assert.Throws<DivideByZeroException>(() => Assert.That(UInt48.Zero / UInt48.Zero, Is.EqualTo(UInt48.One)), "0 / 0");
    Assert.Throws<DivideByZeroException>(() => Assert.That(UInt48.One / UInt48.Zero, Is.EqualTo(UInt48.One)), "1 / 0");
    Assert.That(UInt48.Zero / UInt48.One, Is.EqualTo(UInt48.Zero), "0 / 1");
    Assert.That(UInt48.One / UInt48.One, Is.EqualTo(UInt48.One), "1 / 1");
    Assert.That(UInt48.MaxValue / UInt48.MaxValue, Is.EqualTo(UInt48.One), "Max / Max");
  }

  [Test]
  public void OpCheckedDivision()
  {
    Assert.That(checked(UInt48.Zero / UInt48.One), Is.EqualTo(UInt48.Zero), "0 / 1");
    Assert.That(checked(UInt48.One / UInt48.One), Is.EqualTo(UInt48.One), "1 / 1");
  }

  [Test]
  public void OpCheckedDivision_Overflow()
  {
    Assert.Ignore("no test cases");
  }
}

#pragma warning disable IDE0040
partial class UInt24nTests {
#pragma warning restore IDE0040
#if SYSTEM_NUMERICS_IDIVISIONOPERATORS
  [Test]
  public void IDivisionOperators_OpDivision()
  {
    Assert.Throws<DivideByZeroException>(() => Assert.That(Divide(UInt24.Zero, UInt24.Zero), Is.EqualTo(UInt24.One)), "0 / 0");
    Assert.Throws<DivideByZeroException>(() => Assert.That(Divide(UInt24.One, UInt24.Zero), Is.EqualTo(UInt24.One)), "1 / 0");
    Assert.That(Divide(UInt24.Zero, UInt24.One), Is.EqualTo(UInt24.Zero), "0 / 1");
    Assert.That(Divide(UInt24.One, UInt24.One), Is.EqualTo(UInt24.One), "1 / 1");
    Assert.That(Divide(UInt24.MaxValue, UInt24.MaxValue), Is.EqualTo(UInt24.One), "Max / Max");

    Assert.Throws<DivideByZeroException>(() => Assert.That(Divide(UInt48.Zero, UInt48.Zero), Is.EqualTo(UInt48.One)), "0 / 0");
    Assert.Throws<DivideByZeroException>(() => Assert.That(Divide(UInt48.One, UInt48.Zero), Is.EqualTo(UInt48.One)), "1 / 0");
    Assert.That(Divide(UInt48.Zero, UInt48.One), Is.EqualTo(UInt48.Zero), "0 / 1");
    Assert.That(Divide(UInt48.One, UInt48.One), Is.EqualTo(UInt48.One), "1 / 1");
    Assert.That(Divide(UInt48.MaxValue, UInt48.MaxValue), Is.EqualTo(UInt48.One), "Max / Max");

    static TUInt24n Divide<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IDivisionOperators<TUInt24n, TUInt24n, TUInt24n>
      => x / y;
  }

  [Test]
  public void IDivisionOperators_OpCheckedDivision()
  {
    Assert.That(Divide(UInt24.Zero, UInt24.One), Is.EqualTo(UInt24.Zero), "0 / 1");
    Assert.That(Divide(UInt24.One, UInt24.One), Is.EqualTo(UInt24.One), "1 / 1");

    Assert.That(Divide(UInt48.Zero, UInt48.One), Is.EqualTo(UInt48.Zero), "0 / 1");
    Assert.That(Divide(UInt48.One, UInt48.One), Is.EqualTo(UInt48.One), "1 / 1");

    static TUInt24n Divide<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IDivisionOperators<TUInt24n, TUInt24n, TUInt24n>
      => checked(x / y);
  }

  [Test]
  public void IDivisionOperators_OpCheckedDivision_Overflow()
  {
    Assert.Ignore("no test cases");
  }
#endif
}
