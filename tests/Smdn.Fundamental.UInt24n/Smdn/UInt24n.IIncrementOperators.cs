// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void TestOpIncrement()
  {
    static UInt24 Two() => UInt24.One + UInt24.One;
    var zero = UInt24.Zero;
    var one = UInt24.One;
    var max = UInt24.MaxValue;

    Assert.AreEqual(UInt24.One, ++zero, "++0");
    Assert.AreEqual(UInt24.One, zero, "(++0) value");

    Assert.AreEqual(Two(), ++one, "++1");
    Assert.AreEqual(Two(), one, "(++1) value");

    Assert.AreEqual(UInt24.Zero, ++max, "++max");
    Assert.AreEqual(UInt24.Zero, max, "(++max) value");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpIncrement()
  {
    static UInt48 Two() => UInt48.One + UInt48.One;
    var zero = UInt48.Zero;
    var one = UInt48.One;
    var max = UInt48.MaxValue;

    Assert.AreEqual(UInt48.One, ++zero, "++0");
    Assert.AreEqual(UInt48.One, zero, "(++0) value");

    Assert.AreEqual(Two(), ++one, "++1");
    Assert.AreEqual(Two(), one, "(++1) value");

    Assert.AreEqual(UInt48.Zero, ++max, "++max");
    Assert.AreEqual(UInt48.Zero, max, "(++max) value");
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IIncrementOperators_OpIncrement()
  {
    static TUInt24n Two<TUInt24n>() where TUInt24n : INumber<TUInt24n> => TUInt24n.One + TUInt24n.One;

    Increment(UInt24.Zero, UInt24.One);
    Increment(UInt24.One, Two<UInt24>());
    Increment(UInt24.MaxValue, UInt24.Zero);

    Increment(UInt48.Zero, UInt48.One);
    Increment(UInt48.One, Two<UInt48>());
    Increment(UInt48.MaxValue, UInt48.Zero);

    static void Increment<TUInt24n>(TUInt24n value, TUInt24n expected) where TUInt24n : IIncrementOperators<TUInt24n>
    {
      Assert.AreEqual(expected, ++value, $"{typeof(TUInt24n)} ++{value}");
      Assert.AreEqual(expected, value, "{typeof(TUInt24n)} value");
    }
  }
#endif
}
