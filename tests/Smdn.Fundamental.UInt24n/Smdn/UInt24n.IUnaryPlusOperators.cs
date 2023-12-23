// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IUNARYPLUSOPERATORS
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void OpUnaryPlus()
  {
    Assert.That(+UInt24.Zero, Is.EqualTo(UInt24.Zero), "+0");
    Assert.That(+UInt24.One, Is.EqualTo(UInt24.One), "+1");
    Assert.That(+UInt24.MaxValue, Is.EqualTo(UInt24.MaxValue), "+Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void OpUnaryPlus()
  {
    Assert.That(+UInt48.Zero, Is.EqualTo(UInt48.Zero), "+0");
    Assert.That(+UInt48.One, Is.EqualTo(UInt48.One), "+1");
    Assert.That(+UInt48.MaxValue, Is.EqualTo(UInt48.MaxValue), "+Max");
  }
}

partial class UInt24nTests {
#if SYSTEM_NUMERICS_IUNARYPLUSOPERATORS
  [Test]
  public void IUnaryPlusOperators_OpUnaryPlus()
  {
    Assert.That(UnaryPlus(UInt24.Zero), Is.EqualTo(UInt24.Zero), "+0");
    Assert.That(UnaryPlus(UInt24.One), Is.EqualTo(UInt24.One), "+1");
    Assert.That(UnaryPlus(UInt24.MaxValue), Is.EqualTo(UInt24.MaxValue), "+Max");

    Assert.That(UnaryPlus(UInt48.Zero), Is.EqualTo(UInt48.Zero), "+0");
    Assert.That(UnaryPlus(UInt48.One), Is.EqualTo(UInt48.One), "+1");
    Assert.That(UnaryPlus(UInt48.MaxValue), Is.EqualTo(UInt48.MaxValue), "+Max");

    static TUInt24n UnaryPlus<TUInt24n>(TUInt24n value) where TUInt24n : IUnaryPlusOperators<TUInt24n, TUInt24n>
      => +value;
  }
#endif
}
