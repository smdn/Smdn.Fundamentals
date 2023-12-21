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
  public void TestOpAddition()
  {
    Assert.That(UInt24.Zero + UInt24.Zero, Is.EqualTo(UInt24.Zero), "0 + 0");
    Assert.That(UInt24.Zero + UInt24.One, Is.EqualTo(UInt24.One), "0 + 1");
    Assert.That(UInt24.One + UInt24.Zero, Is.EqualTo(UInt24.One), "1 + 0");
    Assert.That(UInt24.One + UInt24.One, Is.EqualTo((UInt24)2), "1 + 1");
    Assert.That(UInt24.MaxValue + UInt24.One, Is.EqualTo(UInt24.Zero), "Max + 1");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpAddition()
  {
    Assert.That(UInt48.Zero + UInt48.Zero, Is.EqualTo(UInt48.Zero), "0 + 0");
    Assert.That(UInt48.Zero + UInt48.One, Is.EqualTo(UInt48.One), "0 + 1");
    Assert.That(UInt48.One + UInt48.Zero, Is.EqualTo(UInt48.One), "1 + 0");
    Assert.That(UInt48.One + UInt48.One, Is.EqualTo((UInt48)2), "1 + 1");
    Assert.That(UInt48.MaxValue + UInt48.One, Is.EqualTo(UInt48.Zero), "Max + 1");
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
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
#endif
}
