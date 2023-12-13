// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void TestOpUnaryPlus()
  {
    Assert.That(+UInt24.Zero, Is.EqualTo(UInt24.Zero), "+0");
    Assert.That(+UInt24.One, Is.EqualTo(UInt24.One), "+1");
    Assert.That(+UInt24.MaxValue, Is.EqualTo(UInt24.MaxValue), "+Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpUnaryPlus()
  {
    Assert.That(+UInt48.Zero, Is.EqualTo(UInt48.Zero), "+0");
    Assert.That(+UInt48.One, Is.EqualTo(UInt48.One), "+1");
    Assert.That(+UInt48.MaxValue, Is.EqualTo(UInt48.MaxValue), "+Max");
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IUnaryPlusOperators_OpUnaryPlus()
  {
    Assert.AreEqual(UInt24.Zero, UnaryPlus(UInt24.Zero), "+0");
    Assert.AreEqual(UInt24.One, UnaryPlus(UInt24.One), "+1");
    Assert.AreEqual(UInt24.MaxValue, UnaryPlus(UInt24.MaxValue), "+Max");

    Assert.AreEqual(UInt48.Zero, UnaryPlus(UInt48.Zero), "+0");
    Assert.AreEqual(UInt48.One, UnaryPlus(UInt48.One), "+1");
    Assert.AreEqual(UInt48.MaxValue, UnaryPlus(UInt48.MaxValue), "+Max");

    static TUInt24n UnaryPlus<TUInt24n>(TUInt24n value) where TUInt24n : IUnaryPlusOperators<TUInt24n, TUInt24n>
      => +value;
  }
#endif
}
