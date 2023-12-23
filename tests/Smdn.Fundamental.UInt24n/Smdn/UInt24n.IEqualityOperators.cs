// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IEQUALITYOPERATORS
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void OpEquality()
  {
    Assert.That(UInt24.Zero == (UInt24)0, Is.True);
    Assert.That(UInt24.Zero == (UInt24)0x000010, Is.False);
    Assert.That(UInt24.Zero == (UInt24)0x001000, Is.False);
    Assert.That(UInt24.Zero == (UInt24)0x100000, Is.False);
  }

  [Test]
  public void OpIneqality()
  {
    Assert.That(UInt24.Zero != (UInt24)0, Is.False);
    Assert.That(UInt24.Zero != (UInt24)0x000010, Is.True);
    Assert.That(UInt24.Zero != (UInt24)0x001000, Is.True);
    Assert.That(UInt24.Zero != (UInt24)0x100000, Is.True);
  }
}

partial class UInt48Tests {
  [Test]
  public void OpEquality()
  {
    Assert.That(UInt48.Zero == (UInt48)0, Is.True);
    Assert.That(UInt48.Zero == (UInt48)0x000000000010, Is.False);
    Assert.That(UInt48.Zero == (UInt48)0x000000001000, Is.False);
    Assert.That(UInt48.Zero == (UInt48)0x000000100000, Is.False);
    Assert.That(UInt48.Zero == (UInt48)0x000010000000, Is.False);
    Assert.That(UInt48.Zero == (UInt48)0x001000000000, Is.False);
    Assert.That(UInt48.Zero == (UInt48)0x100000000000, Is.False);
  }

  [Test]
  public void OpIneqality()
  {
    Assert.That(UInt48.Zero != (UInt48)0, Is.False);
    Assert.That(UInt48.Zero != (UInt48)0x000000000010, Is.True);
    Assert.That(UInt48.Zero != (UInt48)0x000000001000, Is.True);
    Assert.That(UInt48.Zero != (UInt48)0x000000100000, Is.True);
    Assert.That(UInt48.Zero != (UInt48)0x000010000000, Is.True);
    Assert.That(UInt48.Zero != (UInt48)0x001000000000, Is.True);
    Assert.That(UInt48.Zero != (UInt48)0x100000000000, Is.True);
  }
}

partial class UInt24nTests {
#if SYSTEM_NUMERICS_IEQUALITYOPERATORS
  [Test]
  public void IEqualityOperators_Equality()
  {
    Assert.That(Equals(UInt24.Zero, UInt24.Zero), Is.True, "UInt24.Zero == UInt24.Zero");
    Assert.That(Equals(UInt24.One, UInt24.Zero), Is.False, "UInt24.One == UInt24.Zero");
    Assert.That(Equals(UInt24.Zero, UInt24.One), Is.False, "UInt24.Zero == UInt24.One");
    Assert.That(Equals(UInt24.One, UInt24.One), Is.True, "UInt24.One == UInt24.One");

    Assert.That(Equals(UInt48.Zero, UInt48.Zero), Is.True, "UInt48.Zero == UInt48.Zero");
    Assert.That(Equals(UInt48.One, UInt48.Zero), Is.False, "UInt48.One == UInt48.Zero");
    Assert.That(Equals(UInt48.Zero, UInt48.One), Is.False, "UInt48.Zero == UInt48.One");
    Assert.That(Equals(UInt48.One, UInt48.One), Is.True, "UInt48.One == UInt48.One");

    static bool Equals<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IEqualityOperators<TUInt24n, TUInt24n, bool>
      => x == y;
  }

  [Test]
  public void IEqualityOperators_Inequality()
  {
    Assert.That(NotEqual(UInt24.Zero, UInt24.Zero), Is.False, "UInt24.Zero != UInt24.Zero");
    Assert.That(NotEqual(UInt24.One, UInt24.Zero), Is.True, "UInt24.One != UInt24.Zero");
    Assert.That(NotEqual(UInt24.Zero, UInt24.One), Is.True, "UInt24.Zero != UInt24.One");
    Assert.That(NotEqual(UInt24.One, UInt24.One), Is.False, "UInt24.One != UInt24.One");

    Assert.That(NotEqual(UInt48.Zero, UInt48.Zero), Is.False, "UInt48.Zero != UInt48.Zero");
    Assert.That(NotEqual(UInt48.One, UInt48.Zero), Is.True, "UInt48.One != UInt48.Zero");
    Assert.That(NotEqual(UInt48.Zero, UInt48.One), Is.True, "UInt48.Zero != UInt48.One");
    Assert.That(NotEqual(UInt48.One, UInt48.One), Is.False, "UInt48.One != UInt48.One");

    static bool NotEqual<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IEqualityOperators<TUInt24n, TUInt24n, bool>
      => x != y;
  }
#endif
}
