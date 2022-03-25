// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void TestOpDecrement()
  {
    static UInt24 MaxMinusOne() => UInt24.MaxValue - UInt24.One;
    var zero = UInt24.Zero;
    var one = UInt24.One;
    var max = UInt24.MaxValue;

    Assert.AreEqual(UInt24.MaxValue, --zero, "--0");
    Assert.AreEqual(UInt24.MaxValue, zero, "(--0) value");

    Assert.AreEqual(UInt24.Zero, --one, "--1");
    Assert.AreEqual(UInt24.Zero, one, "(--1) value");

    Assert.AreEqual(MaxMinusOne(), --max, "--max");
    Assert.AreEqual(MaxMinusOne(), max, "(--max) value");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpDecrement()
  {
    static UInt48 MaxMinusOne() => UInt48.MaxValue - UInt48.One;
    var zero = UInt48.Zero;
    var one = UInt48.One;
    var max = UInt48.MaxValue;

    Assert.AreEqual(UInt48.MaxValue, --zero, "--0");
    Assert.AreEqual(UInt48.MaxValue, zero, "(--0) value");

    Assert.AreEqual(UInt48.Zero, --one, "--1");
    Assert.AreEqual(UInt48.Zero, one, "(--1) value");

    Assert.AreEqual(MaxMinusOne(), --max, "--max");
    Assert.AreEqual(MaxMinusOne(), max, "(--max) value");
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IDecrementOperators_OpIncrement()
  {
    static TUInt24n MaxMinusOne<TUInt24n>() where TUInt24n : INumber<TUInt24n>, IMinMaxValue<TUInt24n> => TUInt24n.MaxValue - TUInt24n.One;

    Decrement(UInt24.Zero, UInt24.MaxValue);
    Decrement(UInt24.One, UInt24.Zero);
    Decrement(UInt24.MaxValue, MaxMinusOne<UInt24>());

    Decrement(UInt48.Zero, UInt48.MaxValue);
    Decrement(UInt48.One, UInt48.Zero);
    Decrement(UInt48.MaxValue, MaxMinusOne<UInt48>());

    static void Decrement<TUInt24n>(TUInt24n value, TUInt24n expected) where TUInt24n : IDecrementOperators<TUInt24n>
    {
      Assert.AreEqual(expected, --value, $"{typeof(TUInt24n)} --{value}");
      Assert.AreEqual(expected, value, "{typeof(TUInt24n)} value");
    }
  }
#endif
}
