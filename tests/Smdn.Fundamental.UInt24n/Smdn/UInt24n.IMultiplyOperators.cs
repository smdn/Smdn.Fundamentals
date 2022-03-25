// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void TestOpMultiply()
  {
    Assert.AreEqual(UInt24.Zero, UInt24.Zero * UInt24.Zero, "0 * 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Zero * UInt24.One, "0 * 1");
    Assert.AreEqual(UInt24.Zero, UInt24.One * UInt24.Zero, "1 * 0");
    Assert.AreEqual(UInt24.One, UInt24.One * UInt24.One, "1 * 1");
    Assert.AreEqual(UInt24.One, UInt24.MaxValue * UInt24.MaxValue, "Max * Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpMultiply()
  {
    Assert.AreEqual(UInt48.Zero, UInt48.Zero * UInt48.Zero, "0 * 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Zero * UInt48.One, "0 * 1");
    Assert.AreEqual(UInt48.Zero, UInt48.One * UInt48.Zero, "1 * 0");
    Assert.AreEqual(UInt48.One, UInt48.One * UInt48.One, "1 * 1");
    Assert.AreEqual(UInt48.One, UInt48.MaxValue * UInt48.MaxValue, "Max * Max");
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IMultiplyOperators_OpMultiply()
  {
    Assert.AreEqual(UInt24.Zero, Multiply(UInt24.Zero, UInt24.Zero), "0 * 0");
    Assert.AreEqual(UInt24.Zero, Multiply(UInt24.Zero, UInt24.One), "0 * 1");
    Assert.AreEqual(UInt24.Zero, Multiply(UInt24.One, UInt24.Zero), "1 * 0");
    Assert.AreEqual(UInt24.One, Multiply(UInt24.One, UInt24.One), "1 * 1");
    Assert.AreEqual(UInt24.One, Multiply(UInt24.MaxValue, UInt24.MaxValue), "Max * Max");

    Assert.AreEqual(UInt48.Zero, Multiply(UInt48.Zero, UInt48.Zero), "0 * 0");
    Assert.AreEqual(UInt48.Zero, Multiply(UInt48.Zero, UInt48.One), "0 * 1");
    Assert.AreEqual(UInt48.Zero, Multiply(UInt48.One, UInt48.Zero), "1 * 0");
    Assert.AreEqual(UInt48.One, Multiply(UInt48.One, UInt48.One), "1 * 1");
    Assert.AreEqual(UInt48.One, Multiply(UInt48.MaxValue, UInt48.MaxValue), "Max * Max");

    static TUInt24n Multiply<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IMultiplyOperators<TUInt24n, TUInt24n, TUInt24n>
      => x * y;
  }
#endif
}
