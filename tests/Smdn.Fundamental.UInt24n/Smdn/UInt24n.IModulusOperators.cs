// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IMODULUSOPERATORS
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void OpModulus()
  {
    Assert.Throws<DivideByZeroException>(() => Assert.That(UInt24.Zero % UInt24.Zero, Is.EqualTo(UInt24.One)), "0 % 0");
    Assert.Throws<DivideByZeroException>(() => Assert.That(UInt24.One % UInt24.Zero, Is.EqualTo(UInt24.One)), "1 % 0");
    Assert.That(UInt24.Zero % UInt24.One, Is.EqualTo(UInt24.Zero), "0 % 1");
    Assert.That(UInt24.One % UInt24.One, Is.EqualTo(UInt24.Zero), "1 % 1");
    Assert.That(UInt24.One % UInt24.MaxValue, Is.EqualTo(UInt24.One), "1 % Max");
    Assert.That(UInt24.MaxValue % UInt24.One, Is.EqualTo(UInt24.Zero), "Max % 1");
    Assert.That(UInt24.MaxValue % UInt24.MaxValue, Is.EqualTo(UInt24.Zero), "Max % Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void OpModulus()
  {
    Assert.Throws<DivideByZeroException>(() => Assert.That(UInt48.Zero % UInt48.Zero, Is.EqualTo(UInt48.One)), "0 % 0");
    Assert.Throws<DivideByZeroException>(() => Assert.That(UInt48.One % UInt48.Zero, Is.EqualTo(UInt48.One)), "1 % 0");
    Assert.That(UInt48.Zero % UInt48.One, Is.EqualTo(UInt48.Zero), "0 % 1");
    Assert.That(UInt48.One % UInt48.One, Is.EqualTo(UInt48.Zero), "1 % 1");
    Assert.That(UInt48.One % UInt48.MaxValue, Is.EqualTo(UInt48.One), "1 % Max");
    Assert.That(UInt48.MaxValue % UInt48.One, Is.EqualTo(UInt48.Zero), "Max % 1");
    Assert.That(UInt48.MaxValue % UInt48.MaxValue, Is.EqualTo(UInt48.Zero), "Max % Max");
  }
}

partial class UInt24nTests {
#if SYSTEM_NUMERICS_IMODULUSOPERATORS
  [Test]
  public void IModulusOperators_OpMultiply()
  {
    Assert.Throws<DivideByZeroException>(() => Assert.That(Modulo(UInt24.Zero, UInt24.Zero), Is.EqualTo(UInt24.One)), "0 % 0");
    Assert.Throws<DivideByZeroException>(() => Assert.That(Modulo(UInt24.One, UInt24.Zero), Is.EqualTo(UInt24.One)), "1 % 0");
    Assert.That(Modulo(UInt24.Zero, UInt24.One), Is.EqualTo(UInt24.Zero), "0 % 1");
    Assert.That(Modulo(UInt24.One, UInt24.One), Is.EqualTo(UInt24.Zero), "1 % 1");
    Assert.That(Modulo(UInt24.One, UInt24.MaxValue), Is.EqualTo(UInt24.One), "1 % Max");
    Assert.That(Modulo(UInt24.MaxValue, UInt24.One), Is.EqualTo(UInt24.Zero), "Max % 1");
    Assert.That(Modulo(UInt24.MaxValue, UInt24.MaxValue), Is.EqualTo(UInt24.Zero), "Max % Max");

    Assert.Throws<DivideByZeroException>(() => Assert.That(Modulo(UInt48.Zero, UInt48.Zero), Is.EqualTo(UInt48.One)), "0 % 0");
    Assert.Throws<DivideByZeroException>(() => Assert.That(Modulo(UInt48.One, UInt48.Zero), Is.EqualTo(UInt48.One)), "1 % 0");
    Assert.That(Modulo(UInt48.Zero, UInt48.One), Is.EqualTo(UInt48.Zero), "0 % 1");
    Assert.That(Modulo(UInt48.One, UInt48.One), Is.EqualTo(UInt48.Zero), "1 % 1");
    Assert.That(Modulo(UInt48.One, UInt48.MaxValue), Is.EqualTo(UInt48.One), "1 % Max");
    Assert.That(Modulo(UInt48.MaxValue, UInt48.One), Is.EqualTo(UInt48.Zero), "Max % 1");
    Assert.That(Modulo(UInt48.MaxValue, UInt48.MaxValue), Is.EqualTo(UInt48.Zero), "Max % Max");

    static TUInt24n Modulo<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IModulusOperators<TUInt24n, TUInt24n, TUInt24n>
      => x % y;
  }
#endif
}
