// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void TestOpEquality()
  {
    Assert.IsTrue(UInt24.Zero == (UInt24)0);
    Assert.IsFalse(UInt24.Zero == (UInt24)0x000010);
    Assert.IsFalse(UInt24.Zero == (UInt24)0x001000);
    Assert.IsFalse(UInt24.Zero == (UInt24)0x100000);
  }

  [Test]
  public void TestOpIneqality()
  {
    Assert.IsFalse(UInt24.Zero != (UInt24)0);
    Assert.IsTrue(UInt24.Zero != (UInt24)0x000010);
    Assert.IsTrue(UInt24.Zero != (UInt24)0x001000);
    Assert.IsTrue(UInt24.Zero != (UInt24)0x100000);
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpEquality()
  {
    Assert.IsTrue(UInt48.Zero == (UInt48)0);
    Assert.IsFalse(UInt48.Zero == (UInt48)0x000000000010);
    Assert.IsFalse(UInt48.Zero == (UInt48)0x000000001000);
    Assert.IsFalse(UInt48.Zero == (UInt48)0x000000100000);
    Assert.IsFalse(UInt48.Zero == (UInt48)0x000010000000);
    Assert.IsFalse(UInt48.Zero == (UInt48)0x001000000000);
    Assert.IsFalse(UInt48.Zero == (UInt48)0x100000000000);
  }

  [Test]
  public void TestOpIneqality()
  {
    Assert.IsFalse(UInt48.Zero != (UInt48)0);
    Assert.IsTrue(UInt48.Zero != (UInt48)0x000000000010);
    Assert.IsTrue(UInt48.Zero != (UInt48)0x000000001000);
    Assert.IsTrue(UInt48.Zero != (UInt48)0x000000100000);
    Assert.IsTrue(UInt48.Zero != (UInt48)0x000010000000);
    Assert.IsTrue(UInt48.Zero != (UInt48)0x001000000000);
    Assert.IsTrue(UInt48.Zero != (UInt48)0x100000000000);
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IEqualityOperators_Equality()
  {
    Assert.IsTrue(Equals(UInt24.Zero, UInt24.Zero), "UInt24.Zero == UInt24.Zero");
    Assert.IsFalse(Equals(UInt24.One, UInt24.Zero), "UInt24.One == UInt24.Zero");
    Assert.IsFalse(Equals(UInt24.Zero, UInt24.One), "UInt24.Zero == UInt24.One");
    Assert.IsTrue(Equals(UInt24.One, UInt24.One), "UInt24.One == UInt24.One");

    Assert.IsTrue(Equals(UInt48.Zero, UInt48.Zero), "UInt48.Zero == UInt48.Zero");
    Assert.IsFalse(Equals(UInt48.One, UInt48.Zero), "UInt48.One == UInt48.Zero");
    Assert.IsFalse(Equals(UInt48.Zero, UInt48.One), "UInt48.Zero == UInt48.One");
    Assert.IsTrue(Equals(UInt48.One, UInt48.One), "UInt48.One == UInt48.One");

    static bool Equals<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IComparisonOperators<TUInt24n, TUInt24n>
      => x == y;
  }

  [Test]
  public void IEqualityOperators_Inequality()
  {
    Assert.IsFalse(NotEqual(UInt24.Zero, UInt24.Zero), "UInt24.Zero != UInt24.Zero");
    Assert.IsTrue(NotEqual(UInt24.One, UInt24.Zero), "UInt24.One != UInt24.Zero");
    Assert.IsTrue(NotEqual(UInt24.Zero, UInt24.One), "UInt24.Zero != UInt24.One");
    Assert.IsFalse(NotEqual(UInt24.One, UInt24.One), "UInt24.One != UInt24.One");

    Assert.IsFalse(NotEqual(UInt48.Zero, UInt48.Zero), "UInt48.Zero != UInt48.Zero");
    Assert.IsTrue(NotEqual(UInt48.One, UInt48.Zero), "UInt48.One != UInt48.Zero");
    Assert.IsTrue(NotEqual(UInt48.Zero, UInt48.One), "UInt48.Zero != UInt48.One");
    Assert.IsFalse(NotEqual(UInt48.One, UInt48.One), "UInt48.One != UInt48.One");

    static bool NotEqual<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IComparisonOperators<TUInt24n, TUInt24n>
      => x != y;
  }
#endif
}
