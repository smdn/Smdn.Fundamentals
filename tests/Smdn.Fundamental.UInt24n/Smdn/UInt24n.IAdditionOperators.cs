// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IADDITIONOPERATORS
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [Test]
  public void OpAddition()
  {
    Assert.That(UInt24.Zero + UInt24.Zero, Is.EqualTo(UInt24.Zero), "0 + 0");
    Assert.That(UInt24.Zero + UInt24.One, Is.EqualTo(UInt24.One), "0 + 1");
    Assert.That(UInt24.One + UInt24.Zero, Is.EqualTo(UInt24.One), "1 + 0");
    Assert.That(UInt24.One + UInt24.One, Is.EqualTo((UInt24)2), "1 + 1");
    Assert.That(unchecked(UInt24.MaxValue + UInt24.One), Is.EqualTo(UInt24.Zero), "Max + 1");
  }

  [Test]
  public void OpCheckedAddition()
  {
    UInt24 result;

    Assert.Throws<OverflowException>(() => result = checked(UInt24.MaxValue + UInt24.One), "Max + 1");
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test]
  public void OpAddition()
  {
    Assert.That(UInt48.Zero + UInt48.Zero, Is.EqualTo(UInt48.Zero), "0 + 0");
    Assert.That(UInt48.Zero + UInt48.One, Is.EqualTo(UInt48.One), "0 + 1");
    Assert.That(UInt48.One + UInt48.Zero, Is.EqualTo(UInt48.One), "1 + 0");
    Assert.That(UInt48.One + UInt48.One, Is.EqualTo((UInt48)2), "1 + 1");
    Assert.That(unchecked(UInt48.MaxValue + UInt48.One), Is.EqualTo(UInt48.Zero), "Max + 1");
  }

  [Test]
  public void OpCheckedAddition()
  {
    UInt48 result;

    Assert.Throws<OverflowException>(() => result = checked(UInt48.MaxValue + UInt48.One), "Max + 1");
  }
}

#pragma warning disable IDE0040
partial class UInt24nTests {
#pragma warning restore IDE0040
#if SYSTEM_NUMERICS_IADDITIONOPERATORS
  [Test]
  public void IAdditionOperators_OpAddition()
  {
    Assert.That(Add(UInt24.Zero, UInt24.Zero), Is.EqualTo(UInt24.Zero), "0 + 0");
    Assert.That(Add(UInt24.Zero, UInt24.One), Is.EqualTo(UInt24.One), "0 + 1");
    Assert.That(Add(UInt24.One, UInt24.Zero), Is.EqualTo(UInt24.One), "1 + 0");
    Assert.That(Add(UInt24.One, UInt24.One), Is.EqualTo((UInt24)2), "1 + 1");
    Assert.That(Add(UInt24.MaxValue, UInt24.One), Is.EqualTo(UInt24.Zero), "Max + 1");

    Assert.That(Add(UInt48.Zero, UInt48.Zero), Is.EqualTo(UInt48.Zero), "0 + 0");
    Assert.That(Add(UInt48.Zero, UInt48.One), Is.EqualTo(UInt48.One), "0 + 1");
    Assert.That(Add(UInt48.One, UInt48.Zero), Is.EqualTo(UInt48.One), "1 + 0");
    Assert.That(Add(UInt48.One, UInt48.One), Is.EqualTo((UInt48)2), "1 + 1");
    Assert.That(Add(UInt48.MaxValue, UInt48.One), Is.EqualTo(UInt48.Zero), "Max + 1");

    static TUInt24n Add<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IAdditionOperators<TUInt24n, TUInt24n, TUInt24n>
      => x + y;
  }

  [Test]
  public void IAdditionOperators_OpCheckedAddition()
  {
    Assert.Throws<OverflowException>(() => Add(UInt24.MaxValue, UInt24.One), "Max + 1");
    Assert.Throws<OverflowException>(() => Add(UInt48.MaxValue, UInt48.One), "Max + 1");

    static TUInt24n Add<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IAdditionOperators<TUInt24n, TUInt24n, TUInt24n>
      => checked(x + y);
  }
#endif
}
