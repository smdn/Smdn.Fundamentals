// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;
partial class UInt24nTests {
  [Test]
  public void INumber_Sign_UInt24()
  {
    Assert.That(Sign(UInt24.Zero), Is.EqualTo(0), $"{typeof(UInt24).Name}.{nameof(UInt24.Zero)}");
    Assert.That(Sign(UInt24.One), Is.EqualTo(1), $"{typeof(UInt24).Name}.{nameof(UInt24.One)}");
    Assert.That(Sign(UInt24.MaxValue), Is.EqualTo(1), $"{typeof(UInt24).Name}.{nameof(UInt24.MaxValue)}");

#if FEATURE_GENERIC_MATH
    static int Sign<TUInt24n>(TUInt24n value) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Sign(value);
#else
    static int Sign(UInt24 value)
      => UInt24.Sign(value);
#endif
  }

  [Test]
  public void INumber_Sign_UInt48()
  {
    Assert.That(Sign(UInt48.Zero), Is.EqualTo(0), $"{typeof(UInt48).Name}.{nameof(UInt48.Zero)}");
    Assert.That(Sign(UInt48.One), Is.EqualTo(1), $"{typeof(UInt48).Name}.{nameof(UInt48.One)}");
    Assert.That(Sign(UInt48.MaxValue), Is.EqualTo(1), $"{typeof(UInt48).Name}.{nameof(UInt48.MaxValue)}");

#if FEATURE_GENERIC_MATH
    static int Sign<TUInt24n>(TUInt24n value) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Sign(value);
#else
    static int Sign(UInt48 value)
      => UInt48.Sign(value);
#endif
  }

  [Test]
  public void INumber_Min_UInt24()
  {
    Assert.That(Min(UInt24.Zero, UInt24.Zero), Is.EqualTo(UInt24.Zero), $"{typeof(UInt24).Name} min(Zero, Zero)");
    Assert.That(Min(UInt24.Zero, UInt24.One), Is.EqualTo(UInt24.Zero), $"{typeof(UInt24).Name} min(Zero, One)");
    Assert.That(Min(UInt24.One, UInt24.Zero), Is.EqualTo(UInt24.Zero), $"{typeof(UInt24).Name} min(One, Zero)");
    Assert.That(Min(UInt24.One, UInt24.One), Is.EqualTo(UInt24.One), $"{typeof(UInt24).Name} min(One, One)");
    Assert.That(Min(UInt24.One, UInt24.MaxValue), Is.EqualTo(UInt24.One), $"{typeof(UInt24).Name} min(One, MaxValue)");
    Assert.That(Min(UInt24.MaxValue, UInt24.One), Is.EqualTo(UInt24.One), $"{typeof(UInt24).Name} min(MaxValue, One)");
    Assert.That(Min(UInt24.MaxValue, UInt24.MaxValue), Is.EqualTo(UInt24.MaxValue), $"{typeof(UInt24).Name} min(MaxValue, MaxValue)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Min<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Min(x, y);
#else
    static UInt24 Min(UInt24 x, UInt24 y)
      => UInt24.Min(x, y);
#endif
  }

  [Test]
  public void INumber_Min_UInt48()
  {
    Assert.That(Min(UInt48.Zero, UInt48.Zero), Is.EqualTo(UInt48.Zero), $"{typeof(UInt48).Name} min(Zero, Zero)");
    Assert.That(Min(UInt48.Zero, UInt48.One), Is.EqualTo(UInt48.Zero), $"{typeof(UInt48).Name} min(Zero, One)");
    Assert.That(Min(UInt48.One, UInt48.Zero), Is.EqualTo(UInt48.Zero), $"{typeof(UInt48).Name} min(One, Zero)");
    Assert.That(Min(UInt48.One, UInt48.One), Is.EqualTo(UInt48.One), $"{typeof(UInt48).Name} min(One, One)");
    Assert.That(Min(UInt48.One, UInt48.MaxValue), Is.EqualTo(UInt48.One), $"{typeof(UInt48).Name} min(One, MaxValue)");
    Assert.That(Min(UInt48.MaxValue, UInt48.One), Is.EqualTo(UInt48.One), $"{typeof(UInt48).Name} min(MaxValue, One)");
    Assert.That(Min(UInt48.MaxValue, UInt48.MaxValue), Is.EqualTo(UInt48.MaxValue), $"{typeof(UInt48).Name} min(MaxValue, MaxValue)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Min<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Min(x, y);
#else
    static UInt48 Min(UInt48 x, UInt48 y)
      => UInt48.Min(x, y);
#endif
  }

  [Test]
  public void INumber_Max_UInt24()
  {
    Assert.That(Max(UInt24.Zero, UInt24.Zero), Is.EqualTo(UInt24.Zero), $"{typeof(UInt24).Name} max(Zero, Zero)");
    Assert.That(Max(UInt24.Zero, UInt24.One), Is.EqualTo(UInt24.One), $"{typeof(UInt24).Name} max(Zero, One)");
    Assert.That(Max(UInt24.One, UInt24.Zero), Is.EqualTo(UInt24.One), $"{typeof(UInt24).Name} max(One, Zero)");
    Assert.That(Max(UInt24.One, UInt24.One), Is.EqualTo(UInt24.One), $"{typeof(UInt24).Name} max(One, One)");
    Assert.That(Max(UInt24.One, UInt24.MaxValue), Is.EqualTo(UInt24.MaxValue), $"{typeof(UInt24).Name} max(One, MaxValue)");
    Assert.That(Max(UInt24.MaxValue, UInt24.One), Is.EqualTo(UInt24.MaxValue), $"{typeof(UInt24).Name} max(MaxValue, One)");
    Assert.That(Max(UInt24.MaxValue, UInt24.MaxValue), Is.EqualTo(UInt24.MaxValue), $"{typeof(UInt24).Name} max(MaxValue, MaxValue)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Max<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Max(x, y);
#else
    static UInt24 Max(UInt24 x, UInt24 y)
      => UInt24.Max(x, y);
#endif
  }

  [Test]
  public void INumber_Max_UInt48()
  {
    Assert.That(Max(UInt48.Zero, UInt48.Zero), Is.EqualTo(UInt48.Zero), $"{typeof(UInt48).Name} max(Zero, Zero)");
    Assert.That(Max(UInt48.Zero, UInt48.One), Is.EqualTo(UInt48.One), $"{typeof(UInt48).Name} max(Zero, One)");
    Assert.That(Max(UInt48.One, UInt48.Zero), Is.EqualTo(UInt48.One), $"{typeof(UInt48).Name} max(One, Zero)");
    Assert.That(Max(UInt48.One, UInt48.One), Is.EqualTo(UInt48.One), $"{typeof(UInt48).Name} max(One, One)");
    Assert.That(Max(UInt48.One, UInt48.MaxValue), Is.EqualTo(UInt48.MaxValue), $"{typeof(UInt48).Name} max(One, MaxValue)");
    Assert.That(Max(UInt48.MaxValue, UInt48.One), Is.EqualTo(UInt48.MaxValue), $"{typeof(UInt48).Name} max(MaxValue, One)");
    Assert.That(Max(UInt48.MaxValue, UInt48.MaxValue), Is.EqualTo(UInt48.MaxValue), $"{typeof(UInt48).Name} max(MaxValue, MaxValue)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Max<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Max(x, y);
#else
    static UInt48 Max(UInt48 x, UInt48 y)
      => UInt48.Max(x, y);
#endif
  }

  [Test]
  public void INumber_Clamp_UInt24()
  {
    Assert.Throws<ArgumentException>(() => Clamp(UInt24.One, min: UInt24.One, max: UInt24.Zero), $"{typeof(UInt24).Name} min > max");

    Assert.That(Clamp(UInt24.Zero, min: UInt24.Zero, max: UInt24.Zero), Is.EqualTo(UInt24.Zero), $"{typeof(UInt24).Name} clamp(Zero, Zero, Zero)");
    Assert.That(Clamp(UInt24.One, min: UInt24.Zero, max: UInt24.Zero), Is.EqualTo(UInt24.Zero), $"{typeof(UInt24).Name} clamp(One, Zero, Zero)");
    Assert.That(Clamp(UInt24.Zero, min: UInt24.Zero, max: UInt24.One), Is.EqualTo(UInt24.Zero), $"{typeof(UInt24).Name} clamp(Zero, Zero, One)");
    Assert.That(Clamp(UInt24.One, min: UInt24.Zero, max: UInt24.One), Is.EqualTo(UInt24.One), $"{typeof(UInt24).Name} clamp(Zero, Zero, One)");
    Assert.That(Clamp(UInt24.MaxValue, min: UInt24.Zero, max: UInt24.One), Is.EqualTo(UInt24.One), $"{typeof(UInt24).Name} clamp(MaxValue, Zero, One)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Clamp<TUInt24n>(TUInt24n value, TUInt24n min, TUInt24n max) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Clamp(value, min, max);
#else
    static UInt24 Clamp(UInt24 value, UInt24 min, UInt24 max)
      => UInt24.Clamp(value, min, max);
#endif
  }

  [Test]
  public void INumber_Clamp_UInt48()
  {
    Assert.Throws<ArgumentException>(() => Clamp(UInt48.One, min: UInt48.One, max: UInt48.Zero), $"{typeof(UInt48).Name} min > max");

    Assert.That(Clamp(UInt48.Zero, min: UInt48.Zero, max: UInt48.Zero), Is.EqualTo(UInt48.Zero), $"{typeof(UInt48).Name} clamp(Zero, Zero, Zero)");
    Assert.That(Clamp(UInt48.One, min: UInt48.Zero, max: UInt48.Zero), Is.EqualTo(UInt48.Zero), $"{typeof(UInt48).Name} clamp(One, Zero, Zero)");
    Assert.That(Clamp(UInt48.Zero, min: UInt48.Zero, max: UInt48.One), Is.EqualTo(UInt48.Zero), $"{typeof(UInt48).Name} clamp(Zero, Zero, One)");
    Assert.That(Clamp(UInt48.One, min: UInt48.Zero, max: UInt48.One), Is.EqualTo(UInt48.One), $"{typeof(UInt48).Name} clamp(Zero, Zero, One)");
    Assert.That(Clamp(UInt48.MaxValue, min: UInt48.Zero, max: UInt48.One), Is.EqualTo(UInt48.One), $"{typeof(UInt48).Name} clamp(MaxValue, Zero, One)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Clamp<TUInt24n>(TUInt24n value, TUInt24n min, TUInt24n max) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Clamp(value, min, max);
#else
    static UInt48 Clamp(UInt48 value, UInt48 min, UInt48 max)
      => UInt48.Clamp(value, min, max);
#endif
  }
}
