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
  public void OpDecrement()
  {
    static UInt24 MaxMinusOne() => UInt24.MaxValue - UInt24.One;
    var zero = UInt24.Zero;
    var one = UInt24.One;
    var max = UInt24.MaxValue;

    Assert.That(--zero, Is.EqualTo(UInt24.MaxValue), "--0");
    Assert.That(zero, Is.EqualTo(UInt24.MaxValue), "(--0) value");

    Assert.That(--one, Is.EqualTo(UInt24.Zero), "--1");
    Assert.That(one, Is.EqualTo(UInt24.Zero), "(--1) value");

    Assert.That(--max, Is.EqualTo(MaxMinusOne()), "--max");
    Assert.That(max, Is.EqualTo(MaxMinusOne()), "(--max) value");
  }

  [Test]
  public void OpCheckedDecrement()
  {
    var zero = UInt24.Zero;

    checked {
      Assert.Throws<OverflowException>(() => --zero, "--zero");
    }
  }
}

partial class UInt48Tests {
  [Test]
  public void OpDecrement()
  {
    static UInt48 MaxMinusOne() => UInt48.MaxValue - UInt48.One;
    var zero = UInt48.Zero;
    var one = UInt48.One;
    var max = UInt48.MaxValue;

    Assert.That(--zero, Is.EqualTo(UInt48.MaxValue), "--0");
    Assert.That(zero, Is.EqualTo(UInt48.MaxValue), "(--0) value");

    Assert.That(--one, Is.EqualTo(UInt48.Zero), "--1");
    Assert.That(one, Is.EqualTo(UInt48.Zero), "(--1) value");

    Assert.That(--max, Is.EqualTo(MaxMinusOne()), "--max");
    Assert.That(max, Is.EqualTo(MaxMinusOne()), "(--max) value");
  }

  [Test]
  public void OpCheckedDecrement()
  {
    var zero = UInt48.Zero;

    checked {
      Assert.Throws<OverflowException>(() => --zero, "--zero");
    }
  }
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IDecrementOperators_OpDecrement()
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
      Assert.That(--value, Is.EqualTo(expected), $"{typeof(TUInt24n)} --{value}");
      Assert.That(value, Is.EqualTo(expected), "{typeof(TUInt24n)} value");
    }
  }

  [Test]
  public void IDecrementOperators_OpCheckedDecrement()
  {
    Decrement(UInt24.Zero);

    Decrement(UInt48.Zero);

    static void Decrement<TUInt24n>(TUInt24n value) where TUInt24n : IDecrementOperators<TUInt24n>
    {
      checked {
        Assert.Throws<OverflowException>(() => --value);
      }
    }
  }
#endif
}
