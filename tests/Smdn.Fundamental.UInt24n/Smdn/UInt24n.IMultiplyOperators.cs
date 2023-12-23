// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void OpMultiply()
  {
    Assert.That(UInt24.Zero * UInt24.Zero, Is.EqualTo(UInt24.Zero), "0 * 0");
    Assert.That(UInt24.Zero * UInt24.One, Is.EqualTo(UInt24.Zero), "0 * 1");
    Assert.That(UInt24.One * UInt24.Zero, Is.EqualTo(UInt24.Zero), "1 * 0");
    Assert.That(UInt24.One * UInt24.One, Is.EqualTo(UInt24.One), "1 * 1");
    Assert.That(unchecked(UInt24.MaxValue * UInt24.MaxValue), Is.EqualTo(UInt24.One), "Max * Max");
  }

  [Test]
  public void OpCheckedMultiply()
  {
    UInt24 result;

    Assert.Throws<OverflowException>(() => result = checked(UInt24.MaxValue * (UInt24)2), "Max * 2");
    Assert.Throws<OverflowException>(() => result = checked(UInt24.MaxValue * UInt24.MaxValue), "Max * Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void OpMultiply()
  {
    Assert.That(UInt48.Zero * UInt48.Zero, Is.EqualTo(UInt48.Zero), "0 * 0");
    Assert.That(UInt48.Zero * UInt48.One, Is.EqualTo(UInt48.Zero), "0 * 1");
    Assert.That(UInt48.One * UInt48.Zero, Is.EqualTo(UInt48.Zero), "1 * 0");
    Assert.That(UInt48.One * UInt48.One, Is.EqualTo(UInt48.One), "1 * 1");
    Assert.That(unchecked(UInt48.MaxValue * UInt48.MaxValue), Is.EqualTo(UInt48.One), "Max * Max");
  }

  [Test]
  public void OpCheckedMultiply()
  {
    UInt48 result;

    Assert.Throws<OverflowException>(() => result = checked(UInt48.MaxValue * (UInt48)2), "Max * 2");
    Assert.Throws<OverflowException>(() => result = checked(UInt48.MaxValue * UInt48.MaxValue), "Max * Max");
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IMultiplyOperators_OpMultiply()
  {
    Assert.That(Multiply(UInt24.Zero, UInt24.Zero), Is.EqualTo(UInt24.Zero), "0 * 0");
    Assert.That(Multiply(UInt24.Zero, UInt24.One), Is.EqualTo(UInt24.Zero), "0 * 1");
    Assert.That(Multiply(UInt24.One, UInt24.Zero), Is.EqualTo(UInt24.Zero), "1 * 0");
    Assert.That(Multiply(UInt24.One, UInt24.One), Is.EqualTo(UInt24.One), "1 * 1");
    Assert.That(Multiply(UInt24.MaxValue, UInt24.MaxValue), Is.EqualTo(UInt24.One), "Max * Max");

    Assert.That(Multiply(UInt48.Zero, UInt48.Zero), Is.EqualTo(UInt48.Zero), "0 * 0");
    Assert.That(Multiply(UInt48.Zero, UInt48.One), Is.EqualTo(UInt48.Zero), "0 * 1");
    Assert.That(Multiply(UInt48.One, UInt48.Zero), Is.EqualTo(UInt48.Zero), "1 * 0");
    Assert.That(Multiply(UInt48.One, UInt48.One), Is.EqualTo(UInt48.One), "1 * 1");
    Assert.That(Multiply(UInt48.MaxValue, UInt48.MaxValue), Is.EqualTo(UInt48.One), "Max * Max");

    static TUInt24n Multiply<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IMultiplyOperators<TUInt24n, TUInt24n, TUInt24n>
      => x * y;
  }

  [Test]
  public void IMultiplyOperators_OpCheckedMultiply()
  {
    Assert.Throws<OverflowException>(() => Multiply(UInt24.MaxValue, (UInt24)2), "Max * 2");
    Assert.Throws<OverflowException>(() => Multiply(UInt24.MaxValue, UInt24.MaxValue), "Max * Max");

    Assert.Throws<OverflowException>(() => Multiply(UInt48.MaxValue, (UInt48)2), "Max * 2");
    Assert.Throws<OverflowException>(() => Multiply(UInt48.MaxValue, UInt48.MaxValue), "Max * Max");

    static TUInt24n Multiply<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IMultiplyOperators<TUInt24n, TUInt24n, TUInt24n>
      => checked(x * y);
  }
#endif
}
