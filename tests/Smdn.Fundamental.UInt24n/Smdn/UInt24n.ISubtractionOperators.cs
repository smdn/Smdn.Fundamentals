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
  public void TestOpSubtraction()
  {
    Assert.That(UInt24.Zero - UInt24.Zero, Is.EqualTo(UInt24.Zero), "0 - 0");
    Assert.That(UInt24.Zero - UInt24.One, Is.EqualTo(UInt24.MaxValue), "0 - 1");
    Assert.That(UInt24.One - UInt24.Zero, Is.EqualTo(UInt24.One), "1 - 0");
    Assert.That(UInt24.One - UInt24.One, Is.EqualTo(UInt24.Zero), "1 - 1");
    Assert.That(UInt24.Zero - UInt24.MaxValue, Is.EqualTo(UInt24.One), "0 - Max");
  }

}

partial class UInt48Tests {
  [Test]
  public void TestOpSubtraction()
  {
    Assert.That(UInt48.Zero - UInt48.Zero, Is.EqualTo(UInt48.Zero), "0 - 0");
    Assert.That(UInt48.Zero - UInt48.One, Is.EqualTo(UInt48.MaxValue), "0 - 1");
    Assert.That(UInt48.One - UInt48.Zero, Is.EqualTo(UInt48.One), "1 - 0");
    Assert.That(UInt48.One - UInt48.One, Is.EqualTo(UInt48.Zero), "1 - 1");
    Assert.That(UInt48.Zero - UInt48.MaxValue, Is.EqualTo(UInt48.One), "0 - Max");
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void ISubtractionOperators_OpSubtraction()
  {
    Assert.That(Subtract(UInt24.Zero, UInt24.Zero), Is.EqualTo(UInt24.Zero), "0 - 0");
    Assert.That(Subtract(UInt24.Zero, UInt24.One), Is.EqualTo(UInt24.MaxValue), "0 - 1");
    Assert.That(Subtract(UInt24.One, UInt24.Zero), Is.EqualTo(UInt24.One), "1 - 0");
    Assert.That(Subtract(UInt24.One, UInt24.One), Is.EqualTo(UInt24.Zero), "1 - 1");
    Assert.That(Subtract(UInt24.Zero, UInt24.MaxValue), Is.EqualTo(UInt24.One), "0 - Max");

    Assert.That(Subtract(UInt48.Zero, UInt48.Zero), Is.EqualTo(UInt48.Zero), "0 - 0");
    Assert.That(Subtract(UInt48.Zero, UInt48.One), Is.EqualTo(UInt48.MaxValue), "0 - 1");
    Assert.That(Subtract(UInt48.One, UInt48.Zero), Is.EqualTo(UInt48.One), "1 - 0");
    Assert.That(Subtract(UInt48.One, UInt48.One), Is.EqualTo(UInt48.Zero), "1 - 1");
    Assert.That(Subtract(UInt48.Zero, UInt48.MaxValue), Is.EqualTo(UInt48.One), "0 - Max");

    static TUInt24n Subtract<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : ISubtractionOperators<TUInt24n, TUInt24n, TUInt24n>
      => x - y;
  }
#endif
}
