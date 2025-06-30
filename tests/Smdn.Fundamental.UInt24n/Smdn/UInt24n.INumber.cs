// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_INUMBER
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class UInt24nTests {
#pragma warning restore IDE0040
  private static System.Collections.IEnumerable YieldTestCases_INumber_Sign_UInt24()
  {
    yield return new object?[] { UInt24.Zero, 0 };
    yield return new object?[] { UInt24.One, 1 };
    yield return new object?[] { UInt24.MaxValue, 1 };
  }

  [TestCaseSource(nameof(YieldTestCases_INumber_Sign_UInt24))]
  public void INumber_Sign_UInt24(UInt24 value, int expected)
  {
    Assert.That(UInt24.Sign(value), Is.EqualTo(expected), nameof(UInt24.Sign));

#if SYSTEM_NUMERICS_INUMBER
    Assert.That(INumber_Sign(value), Is.EqualTo(expected), nameof(INumber_Sign));

    static int INumber_Sign<TUInt24n>(TUInt24n value) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Sign(value);
#endif
  }

  private static System.Collections.IEnumerable YieldTestCases_INumber_Sign_UInt48()
  {
    yield return new object?[] { UInt48.Zero, 0 };
    yield return new object?[] { UInt48.One, 1 };
    yield return new object?[] { UInt48.MaxValue, 1 };
  }

  [TestCaseSource(nameof(YieldTestCases_INumber_Sign_UInt48))]
  public void INumber_Sign_UInt48(UInt48 value, int expected)
  {
    Assert.That(UInt48.Sign(value), Is.EqualTo(expected), nameof(UInt48.Sign));

#if SYSTEM_NUMERICS_INUMBER
    Assert.That(INumber_Sign(value), Is.EqualTo(expected), nameof(INumber_Sign));

    static int INumber_Sign<TUInt24n>(TUInt24n value) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Sign(value);
#endif
  }

  private static System.Collections.IEnumerable YieldTestCases_Min_UInt24()
  {
    yield return new object?[] { UInt24.Zero, UInt24.Zero, UInt24.Zero };
    yield return new object?[] { UInt24.Zero, UInt24.One, UInt24.Zero };
    yield return new object?[] { UInt24.One, UInt24.Zero, UInt24.Zero };
    yield return new object?[] { UInt24.One, UInt24.One, UInt24.One };
    yield return new object?[] { UInt24.One, UInt24.MaxValue, UInt24.One };
    yield return new object?[] { UInt24.MaxValue, UInt24.One, UInt24.One };
    yield return new object?[] { UInt24.MaxValue, UInt24.MaxValue, UInt24.MaxValue };
  }

  [TestCaseSource(nameof(YieldTestCases_Min_UInt24))]
  public void INumber_Min_UInt24(UInt24 x, UInt24 y, UInt24 expected)
  {
    Assert.That(UInt24.Min(x, y), Is.EqualTo(expected), nameof(UInt24.Min));

#if SYSTEM_NUMERICS_INUMBER
    Assert.That(INumber_Min(x, y), Is.EqualTo(expected), nameof(INumber_Min));

    static TUInt24n INumber_Min<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Min(x, y);
#endif
  }

  private static System.Collections.IEnumerable YieldTestCases_Min_UInt48()
  {
    yield return new object?[] { UInt48.Zero, UInt48.Zero, UInt48.Zero };
    yield return new object?[] { UInt48.Zero, UInt48.One, UInt48.Zero };
    yield return new object?[] { UInt48.One, UInt48.Zero, UInt48.Zero };
    yield return new object?[] { UInt48.One, UInt48.One, UInt48.One };
    yield return new object?[] { UInt48.One, UInt48.MaxValue, UInt48.One };
    yield return new object?[] { UInt48.MaxValue, UInt48.One, UInt48.One };
    yield return new object?[] { UInt48.MaxValue, UInt48.MaxValue, UInt48.MaxValue };
  }

  [TestCaseSource(nameof(YieldTestCases_Min_UInt48))]
  public void INumber_Min_UInt48(UInt48 x, UInt48 y, UInt48 expected)
  {
    Assert.That(UInt48.Min(x, y), Is.EqualTo(expected), nameof(UInt48.Min));

#if SYSTEM_NUMERICS_INUMBER
    Assert.That(INumber_Min(x, y), Is.EqualTo(expected), nameof(INumber_Min));

    static TUInt24n INumber_Min<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Min(x, y);
#endif
  }

  private static System.Collections.IEnumerable YieldTestCases_Max_UInt24()
  {
    yield return new object?[] { UInt24.Zero, UInt24.Zero, UInt24.Zero };
    yield return new object?[] { UInt24.Zero, UInt24.One, UInt24.One };
    yield return new object?[] { UInt24.One, UInt24.Zero, UInt24.One };
    yield return new object?[] { UInt24.One, UInt24.One, UInt24.One };
    yield return new object?[] { UInt24.One, UInt24.MaxValue, UInt24.MaxValue };
    yield return new object?[] { UInt24.MaxValue, UInt24.One, UInt24.MaxValue };
    yield return new object?[] { UInt24.MaxValue, UInt24.MaxValue, UInt24.MaxValue };
  }

  [TestCaseSource(nameof(YieldTestCases_Max_UInt24))]
  public void INumber_Max_UInt24(UInt24 x, UInt24 y, UInt24 expected)
  {
    Assert.That(UInt24.Max(x, y), Is.EqualTo(expected), nameof(UInt24.Max));

#if SYSTEM_NUMERICS_INUMBER
    Assert.That(INumber_Max(x, y), Is.EqualTo(expected), nameof(INumber_Max));

    static TUInt24n INumber_Max<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Max(x, y);
#endif
  }

  private static System.Collections.IEnumerable YieldTestCases_Max_UInt48()
  {
    yield return new object?[] { UInt48.Zero, UInt48.Zero, UInt48.Zero };
    yield return new object?[] { UInt48.Zero, UInt48.One, UInt48.One };
    yield return new object?[] { UInt48.One, UInt48.Zero, UInt48.One };
    yield return new object?[] { UInt48.One, UInt48.One, UInt48.One };
    yield return new object?[] { UInt48.One, UInt48.MaxValue, UInt48.MaxValue };
    yield return new object?[] { UInt48.MaxValue, UInt48.One, UInt48.MaxValue };
    yield return new object?[] { UInt48.MaxValue, UInt48.MaxValue, UInt48.MaxValue };
  }

  [TestCaseSource(nameof(YieldTestCases_Max_UInt48))]
  public void INumber_Max_UInt48(UInt48 x, UInt48 y, UInt48 expected)
  {
    Assert.That(UInt48.Max(x, y), Is.EqualTo(expected), nameof(UInt48.Max));

#if SYSTEM_NUMERICS_INUMBER
    Assert.That(INumber_Max(x, y), Is.EqualTo(expected), nameof(INumber_Max));

    static TUInt24n INumber_Max<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Max(x, y);
#endif
  }

  private static System.Collections.IEnumerable YieldTestCases_INumber_Clamp_UInt24()
  {
    yield return new object?[] { (Value: UInt24.Zero, Min: UInt24.Zero, Max: UInt24.Zero), UInt24.Zero };
    yield return new object?[] { (Value: UInt24.One, Min: UInt24.Zero, Max: UInt24.Zero), UInt24.Zero };
    yield return new object?[] { (Value: UInt24.Zero, Min: UInt24.Zero, Max: UInt24.One), UInt24.Zero };
    yield return new object?[] { (Value: UInt24.One, Min: UInt24.Zero, Max: UInt24.One), UInt24.One };
    yield return new object?[] { (Value: UInt24.Zero, Min: UInt24.One, Max: UInt24.One), UInt24.One };
    yield return new object?[] { (Value: UInt24.One, Min: UInt24.One, Max: UInt24.One), UInt24.One };
    yield return new object?[] { (Value: UInt24.MaxValue, Min: UInt24.Zero, Max: UInt24.One), UInt24.One };
  }

  [TestCaseSource(nameof(YieldTestCases_INumber_Clamp_UInt24))]
  public void INumber_Clamp_UInt24((UInt24 value, UInt24 min, UInt24 max) input, UInt24 expected)
  {
    var (value, min, max) = input;

    Assert.That(UInt24.Clamp(value, min, max), Is.EqualTo(expected), nameof(UInt24.Clamp));

#if SYSTEM_NUMERICS_INUMBER
    Assert.That(INumber_Clamp(value, min, max), Is.EqualTo(expected), nameof(INumber_Clamp));

    static TUInt24n INumber_Clamp<TUInt24n>(TUInt24n value, TUInt24n min, TUInt24n max) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Clamp(value, min, max);
#endif
  }

  private static System.Collections.IEnumerable YieldTestCases_INumber_Clamp_UInt48()
  {
    yield return new object?[] { (Value: UInt48.Zero, Min: UInt48.Zero, Max: UInt48.Zero), UInt48.Zero };
    yield return new object?[] { (Value: UInt48.One, Min: UInt48.Zero, Max: UInt48.Zero), UInt48.Zero };
    yield return new object?[] { (Value: UInt48.Zero, Min: UInt48.Zero, Max: UInt48.One), UInt48.Zero };
    yield return new object?[] { (Value: UInt48.One, Min: UInt48.Zero, Max: UInt48.One), UInt48.One };
    yield return new object?[] { (Value: UInt48.Zero, Min: UInt48.One, Max: UInt48.One), UInt48.One };
    yield return new object?[] { (Value: UInt48.One, Min: UInt48.One, Max: UInt48.One), UInt48.One };
    yield return new object?[] { (Value: UInt48.MaxValue, Min: UInt48.Zero, Max: UInt48.One), UInt48.One };
  }

  [TestCaseSource(nameof(YieldTestCases_INumber_Clamp_UInt48))]
  public void INumber_Clamp_UInt48((UInt48 value, UInt48 min, UInt48 max) input, UInt48 expected)
  {
    var (value, min, max) = input;

    Assert.That(UInt48.Clamp(value, min, max), Is.EqualTo(expected), nameof(UInt48.Clamp));

#if SYSTEM_NUMERICS_INUMBER
    Assert.That(INumber_Clamp(value, min, max), Is.EqualTo(expected), nameof(INumber_Clamp));

    static TUInt24n INumber_Clamp<TUInt24n>(TUInt24n value, TUInt24n min, TUInt24n max) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Clamp(value, min, max);
#endif
  }

  private static System.Collections.IEnumerable YieldTestCases_INumber_Clamp_UInt24_MinMustBeLessThanMax()
  {
    yield return new object?[] { (Value: UInt24.Zero, Min: UInt24.One, Max: UInt24.Zero) };
    yield return new object?[] { (Value: UInt24.One, Min: UInt24.One, Max: UInt24.Zero) };
    yield return new object?[] { (Value: UInt24.Zero, Min: UInt24.MaxValue, Max: UInt24.Zero) };
    yield return new object?[] { (Value: UInt24.One, Min: UInt24.MaxValue, Max: UInt24.Zero) };
  }

  [TestCaseSource(nameof(YieldTestCases_INumber_Clamp_UInt24_MinMustBeLessThanMax))]
  public void INumber_Clamp_UInt24_MinMustBeLessThanMax((UInt24 value, UInt24 min, UInt24 max) input)
  {
    var (value, min, max) = input;

    Assert.Throws<ArgumentException>(() => UInt24.Clamp(value, min, max), nameof(UInt24.Clamp));

#if SYSTEM_NUMERICS_INUMBER
    Assert.Throws<ArgumentException>(() => INumber_Clamp(value, min, max), nameof(INumber_Clamp));

    static TUInt24n INumber_Clamp<TUInt24n>(TUInt24n value, TUInt24n min, TUInt24n max) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Clamp(value, min, max);
#endif
  }

  private static System.Collections.IEnumerable YieldTestCases_INumber_Clamp_UInt48_MinMustBeLessThanMax()
  {
    yield return new object?[] { (Value: UInt48.Zero, Min: UInt48.One, Max: UInt48.Zero) };
    yield return new object?[] { (Value: UInt48.One, Min: UInt48.One, Max: UInt48.Zero) };
    yield return new object?[] { (Value: UInt48.Zero, Min: UInt48.MaxValue, Max: UInt48.Zero) };
    yield return new object?[] { (Value: UInt48.One, Min: UInt48.MaxValue, Max: UInt48.Zero) };
  }

  [TestCaseSource(nameof(YieldTestCases_INumber_Clamp_UInt48_MinMustBeLessThanMax))]
  public void INumber_Clamp_UInt48_MinMustBeLessThanMax((UInt48 value, UInt48 min, UInt48 max) input)
  {
    var (value, min, max) = input;

    Assert.Throws<ArgumentException>(() => UInt48.Clamp(value, min, max), nameof(UInt48.Clamp));

#if SYSTEM_NUMERICS_INUMBER
    Assert.Throws<ArgumentException>(() => INumber_Clamp(value, min, max), nameof(INumber_Clamp));

    static TUInt24n INumber_Clamp<TUInt24n>(TUInt24n value, TUInt24n min, TUInt24n max) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Clamp(value, min, max);
#endif
  }
}
