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
  public void TestOpUnaryNegation()
  {
    Assert.That(-UInt24.Zero, Is.EqualTo(UInt24.Zero), "-0");
    Assert.That(-UInt24.One, Is.EqualTo(UInt24.MaxValue), "-1");
    Assert.That(-UInt24.MaxValue, Is.EqualTo(UInt24.One), "-Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpUnaryNegation()
  {
    Assert.That(-UInt48.Zero, Is.EqualTo(UInt48.Zero), "-0");
    Assert.That(-UInt48.One, Is.EqualTo(UInt48.MaxValue), "-1");
    Assert.That(-UInt48.MaxValue, Is.EqualTo(UInt48.One), "-Max");
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IUnaryNegationOperators_OpUnaryNegation()
  {
    Assert.That(UnaryNegation(UInt24.Zero), Is.EqualTo(UInt24.Zero), "-0");
    Assert.That(UnaryNegation(UInt24.One), Is.EqualTo(UInt24.MaxValue), "-1");
    Assert.That(UnaryNegation(UInt24.MaxValue), Is.EqualTo(UInt24.One), "-Max");

    Assert.That(UnaryNegation(UInt48.Zero), Is.EqualTo(UInt48.Zero), "-0");
    Assert.That(UnaryNegation(UInt48.One), Is.EqualTo(UInt48.MaxValue), "-1");
    Assert.That(UnaryNegation(UInt48.MaxValue), Is.EqualTo(UInt48.One), "-Max");

    static TUInt24n UnaryNegation<TUInt24n>(TUInt24n value) where TUInt24n : IUnaryNegationOperators<TUInt24n, TUInt24n>
      => -value;
  }
#endif
}
