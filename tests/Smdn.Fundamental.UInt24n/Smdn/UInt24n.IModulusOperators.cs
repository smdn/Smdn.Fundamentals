// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void TestOpModulus()
  {
    Assert.Throws<DivideByZeroException>(() => Assert.AreEqual(UInt24.One, UInt24.Zero % UInt24.Zero), "0 % 0");
    Assert.Throws<DivideByZeroException>(() => Assert.AreEqual(UInt24.One, UInt24.One % UInt24.Zero), "1 % 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Zero % UInt24.One, "0 % 1");
    Assert.AreEqual(UInt24.Zero, UInt24.One % UInt24.One, "1 % 1");
    Assert.AreEqual(UInt24.One, UInt24.One % UInt24.MaxValue, "1 % Max");
    Assert.AreEqual(UInt24.Zero, UInt24.MaxValue % UInt24.One, "Max % 1");
    Assert.AreEqual(UInt24.Zero, UInt24.MaxValue % UInt24.MaxValue, "Max % Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpModulus()
  {
    Assert.Throws<DivideByZeroException>(() => Assert.AreEqual(UInt48.One, UInt48.Zero % UInt48.Zero), "0 % 0");
    Assert.Throws<DivideByZeroException>(() => Assert.AreEqual(UInt48.One, UInt48.One % UInt48.Zero), "1 % 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Zero % UInt48.One, "0 % 1");
    Assert.AreEqual(UInt48.Zero, UInt48.One % UInt48.One, "1 % 1");
    Assert.AreEqual(UInt48.One, UInt48.One % UInt48.MaxValue, "1 % Max");
    Assert.AreEqual(UInt48.Zero, UInt48.MaxValue % UInt48.One, "Max % 1");
    Assert.AreEqual(UInt48.Zero, UInt48.MaxValue % UInt48.MaxValue, "Max % Max");
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IModulusOperators_OpMultiply()
  {
    Assert.Throws<DivideByZeroException>(() => Assert.AreEqual(UInt24.One, Modulo(UInt24.Zero, UInt24.Zero)), "0 % 0");
    Assert.Throws<DivideByZeroException>(() => Assert.AreEqual(UInt24.One, Modulo(UInt24.One, UInt24.Zero)), "1 % 0");
    Assert.AreEqual(UInt24.Zero, Modulo(UInt24.Zero, UInt24.One), "0 % 1");
    Assert.AreEqual(UInt24.Zero, Modulo(UInt24.One, UInt24.One), "1 % 1");
    Assert.AreEqual(UInt24.One, Modulo(UInt24.One, UInt24.MaxValue), "1 % Max");
    Assert.AreEqual(UInt24.Zero, Modulo(UInt24.MaxValue, UInt24.One), "Max % 1");
    Assert.AreEqual(UInt24.Zero, Modulo(UInt24.MaxValue, UInt24.MaxValue), "Max % Max");

    Assert.Throws<DivideByZeroException>(() => Assert.AreEqual(UInt48.One, Modulo(UInt48.Zero, UInt48.Zero)), "0 % 0");
    Assert.Throws<DivideByZeroException>(() => Assert.AreEqual(UInt48.One, Modulo(UInt48.One, UInt48.Zero)), "1 % 0");
    Assert.AreEqual(UInt48.Zero, Modulo(UInt48.Zero, UInt48.One), "0 % 1");
    Assert.AreEqual(UInt48.Zero, Modulo(UInt48.One, UInt48.One), "1 % 1");
    Assert.AreEqual(UInt48.One, Modulo(UInt48.One, UInt48.MaxValue), "1 % Max");
    Assert.AreEqual(UInt48.Zero, Modulo(UInt48.MaxValue, UInt48.One), "Max % 1");
    Assert.AreEqual(UInt48.Zero, Modulo(UInt48.MaxValue, UInt48.MaxValue), "Max % Max");

    static TUInt24n Modulo<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IModulusOperators<TUInt24n, TUInt24n, TUInt24n>
      => x % y;
  }
#endif
}
