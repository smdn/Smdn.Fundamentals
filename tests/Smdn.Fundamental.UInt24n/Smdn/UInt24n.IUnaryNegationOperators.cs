// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void TestOpUnaryNegation()
  {
    Assert.AreEqual(UInt24.Zero, -UInt24.Zero, "-0");
    Assert.AreEqual(UInt24.MaxValue, -UInt24.One, "-1");
    Assert.AreEqual(UInt24.One, -UInt24.MaxValue, "-Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpUnaryNegation()
  {
    Assert.AreEqual(UInt48.Zero, -UInt48.Zero, "-0");
    Assert.AreEqual(UInt48.MaxValue, -UInt48.One, "-1");
    Assert.AreEqual(UInt48.One, -UInt48.MaxValue, "-Max");
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IUnaryNegationOperators_OpUnaryNegation()
  {
    Assert.AreEqual(UInt24.Zero, UnaryNegation(UInt24.Zero), "-0");
    Assert.AreEqual(UInt24.MaxValue, UnaryNegation(UInt24.One), "-1");
    Assert.AreEqual(UInt24.One, UnaryNegation(UInt24.MaxValue), "-Max");

    Assert.AreEqual(UInt48.Zero, UnaryNegation(UInt48.Zero), "-0");
    Assert.AreEqual(UInt48.MaxValue, UnaryNegation(UInt48.One), "-1");
    Assert.AreEqual(UInt48.One, UnaryNegation(UInt48.MaxValue), "-Max");

    static TUInt24n UnaryNegation<TUInt24n>(TUInt24n value) where TUInt24n : IUnaryNegationOperators<TUInt24n, TUInt24n>
      => -value;
  }
#endif
}
