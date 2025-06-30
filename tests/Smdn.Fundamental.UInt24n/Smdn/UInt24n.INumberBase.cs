// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_INUMBERBASE
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [Test] public void Zero() => Assert.That(UInt24.Zero, Is.EqualTo(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x00 }, isBigEndian: true)));
  [Test] public void One() => Assert.That(UInt24.One, Is.EqualTo(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x01 }, isBigEndian: true)));
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test] public void Zero() => Assert.That(UInt48.Zero, Is.EqualTo(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isBigEndian: true)));
  [Test] public void One() => Assert.That(UInt48.One, Is.EqualTo(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isBigEndian: true)));
}

#pragma warning disable IDE0040
partial class UInt24nTests {
#pragma warning restore IDE0040
#if SYSTEM_NUMERICS_INUMBERBASE
  [Test]
  public void INumberBase_Zero()
  {
    Assert.That(GetZero<UInt24>(), Is.EqualTo(UInt24.Zero), nameof(UInt24));
    Assert.That(GetZero<UInt48>(), Is.EqualTo(UInt48.Zero), nameof(UInt48));

    static TUInt24n GetZero<TUInt24n>() where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.Zero;
  }

  [Test]
  public void INumberBase_One()
  {
    Assert.That(GetOne<UInt24>(), Is.EqualTo(UInt24.One), nameof(UInt24));
    Assert.That(GetOne<UInt48>(), Is.EqualTo(UInt48.One), nameof(UInt48));

    static TUInt24n GetOne<TUInt24n>() where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.One;
  }

  [Test]
  public void INumberBase_Radix()
  {
    Assert.That(GetRadix<UInt16>(), Is.EqualTo(2), nameof(UInt16));
    Assert.That(GetRadix<UInt24>(), Is.EqualTo(2), nameof(UInt24));
    Assert.That(GetRadix<UInt32>(), Is.EqualTo(2), nameof(UInt32));
    Assert.That(GetRadix<UInt48>(), Is.EqualTo(2), nameof(UInt48));
    Assert.That(GetRadix<UInt64>(), Is.EqualTo(2), nameof(UInt64));

    static int GetRadix<TUInt>() where TUInt : INumberBase<TUInt>
      => TUInt.Radix;
  }

  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsCanonical(int n)
  {
    Assert.That(IsCanonical((UInt16)n), Is.True, nameof(UInt16));
    Assert.That(IsCanonical((UInt24)n), Is.True, nameof(UInt24));
    Assert.That(IsCanonical((UInt32)n), Is.True, nameof(UInt32));
    Assert.That(IsCanonical((UInt48)n), Is.True, nameof(UInt48));
    Assert.That(IsCanonical((UInt64)n), Is.True, nameof(UInt64));

    static bool IsCanonical<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsCanonical(number);
  }

  [Test]
  public void INumberBase_IsCanonical_MaxValue()
  {
    Assert.That(IsMaxValueCanonical<UInt16>(), Is.True, nameof(UInt16));
    Assert.That(IsMaxValueCanonical<UInt24>(), Is.True, nameof(UInt24));
    Assert.That(IsMaxValueCanonical<UInt32>(), Is.True, nameof(UInt32));
    Assert.That(IsMaxValueCanonical<UInt48>(), Is.True, nameof(UInt48));
    Assert.That(IsMaxValueCanonical<UInt64>(), Is.True, nameof(UInt64));

    static bool IsMaxValueCanonical<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsCanonical(TUInt.MaxValue);
  }

  [TestCase(0, false)]
  [TestCase(1, true)]
  [TestCase(2, true)]
  public void INumberBase_IsNormal(int n, bool isNormal)
  {
    Assert.That(IsNormal((UInt16)n), Is.EqualTo(isNormal), nameof(UInt16));
    Assert.That(IsNormal((UInt24)n), Is.EqualTo(isNormal), nameof(UInt24));
    Assert.That(IsNormal((UInt32)n), Is.EqualTo(isNormal), nameof(UInt32));
    Assert.That(IsNormal((UInt48)n), Is.EqualTo(isNormal), nameof(UInt48));
    Assert.That(IsNormal((UInt64)n), Is.EqualTo(isNormal), nameof(UInt64));

    static bool IsNormal<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsNormal(number);
  }

  [Test]
  public void INumberBase_IsNormal_MaxValue()
  {
    Assert.That(IsMaxValueNormal<UInt16>(), Is.True, nameof(UInt16));
    Assert.That(IsMaxValueNormal<UInt24>(), Is.True, nameof(UInt24));
    Assert.That(IsMaxValueNormal<UInt32>(), Is.True, nameof(UInt32));
    Assert.That(IsMaxValueNormal<UInt48>(), Is.True, nameof(UInt48));
    Assert.That(IsMaxValueNormal<UInt64>(), Is.True, nameof(UInt64));

    static bool IsMaxValueNormal<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsNormal(TUInt.MaxValue);
  }

  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsSubnormal(int n)
  {
    Assert.That(IsSubnormal((UInt16)n), Is.False, nameof(UInt16));
    Assert.That(IsSubnormal((UInt24)n), Is.False, nameof(UInt24));
    Assert.That(IsSubnormal((UInt32)n), Is.False, nameof(UInt32));
    Assert.That(IsSubnormal((UInt48)n), Is.False, nameof(UInt48));
    Assert.That(IsSubnormal((UInt64)n), Is.False, nameof(UInt64));

    static bool IsSubnormal<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsSubnormal(number);
  }

  [Test]
  public void INumberBase_IsSubnormal_MaxValue()
  {
    Assert.That(IsMaxValueSubnormal<UInt16>(), Is.False, nameof(UInt16));
    Assert.That(IsMaxValueSubnormal<UInt24>(), Is.False, nameof(UInt24));
    Assert.That(IsMaxValueSubnormal<UInt32>(), Is.False, nameof(UInt32));
    Assert.That(IsMaxValueSubnormal<UInt48>(), Is.False, nameof(UInt48));
    Assert.That(IsMaxValueSubnormal<UInt64>(), Is.False, nameof(UInt64));

    static bool IsMaxValueSubnormal<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsSubnormal(TUInt.MaxValue);
  }


  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsComplexNumber(int n)
  {
    Assert.That(IsComplexNumber((UInt16)n), Is.False, nameof(UInt16));
    Assert.That(IsComplexNumber((UInt24)n), Is.False, nameof(UInt24));
    Assert.That(IsComplexNumber((UInt32)n), Is.False, nameof(UInt32));
    Assert.That(IsComplexNumber((UInt48)n), Is.False, nameof(UInt48));
    Assert.That(IsComplexNumber((UInt64)n), Is.False, nameof(UInt64));

    static bool IsComplexNumber<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsComplexNumber(number);
  }

  [Test]
  public void INumberBase_IsComplexNumber_MaxValue()
  {
    Assert.That(IsMaxValueComplexNumber<UInt16>(), Is.False, nameof(UInt16));
    Assert.That(IsMaxValueComplexNumber<UInt24>(), Is.False, nameof(UInt24));
    Assert.That(IsMaxValueComplexNumber<UInt32>(), Is.False, nameof(UInt32));
    Assert.That(IsMaxValueComplexNumber<UInt48>(), Is.False, nameof(UInt48));
    Assert.That(IsMaxValueComplexNumber<UInt64>(), Is.False, nameof(UInt64));

    static bool IsMaxValueComplexNumber<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsComplexNumber(TUInt.MaxValue);
  }


  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsImaginaryNumber(int n)
  {
    Assert.That(IsImaginaryNumber((UInt16)n), Is.False, nameof(UInt16));
    Assert.That(IsImaginaryNumber((UInt24)n), Is.False, nameof(UInt24));
    Assert.That(IsImaginaryNumber((UInt32)n), Is.False, nameof(UInt32));
    Assert.That(IsImaginaryNumber((UInt48)n), Is.False, nameof(UInt48));
    Assert.That(IsImaginaryNumber((UInt64)n), Is.False, nameof(UInt64));

    static bool IsImaginaryNumber<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsImaginaryNumber(number);
  }

  [Test]
  public void INumberBase_IsImaginaryNumber_MaxValue()
  {
    Assert.That(IsMaxValueImaginaryNumber<UInt16>(), Is.False, nameof(UInt16));
    Assert.That(IsMaxValueImaginaryNumber<UInt24>(), Is.False, nameof(UInt24));
    Assert.That(IsMaxValueImaginaryNumber<UInt32>(), Is.False, nameof(UInt32));
    Assert.That(IsMaxValueImaginaryNumber<UInt48>(), Is.False, nameof(UInt48));
    Assert.That(IsMaxValueImaginaryNumber<UInt64>(), Is.False, nameof(UInt64));

    static bool IsMaxValueImaginaryNumber<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsImaginaryNumber(TUInt.MaxValue);
  }


  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsRealNumber(int n)
  {
    Assert.That(IsRealNumber((UInt16)n), Is.True, nameof(UInt16));
    Assert.That(IsRealNumber((UInt24)n), Is.True, nameof(UInt24));
    Assert.That(IsRealNumber((UInt32)n), Is.True, nameof(UInt32));
    Assert.That(IsRealNumber((UInt48)n), Is.True, nameof(UInt48));
    Assert.That(IsRealNumber((UInt64)n), Is.True, nameof(UInt64));

    static bool IsRealNumber<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsRealNumber(number);
  }

  [Test]
  public void INumberBase_IsRealNumber_MaxValue()
  {
    Assert.That(IsMaxValueRealNumber<UInt16>(), Is.True, nameof(UInt16));
    Assert.That(IsMaxValueRealNumber<UInt24>(), Is.True, nameof(UInt24));
    Assert.That(IsMaxValueRealNumber<UInt32>(), Is.True, nameof(UInt32));
    Assert.That(IsMaxValueRealNumber<UInt48>(), Is.True, nameof(UInt48));
    Assert.That(IsMaxValueRealNumber<UInt64>(), Is.True, nameof(UInt64));

    static bool IsMaxValueRealNumber<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsRealNumber(TUInt.MaxValue);
  }

  [TestCase(0, true)]
  [TestCase(1, false)]
  [TestCase(2, true)]
  public void INumberBase_IsEvenInteger(int n, bool isEven)
  {
    Assert.That(IsEvenInteger((UInt16)n), Is.EqualTo(isEven), nameof(UInt16));
    Assert.That(IsEvenInteger((UInt24)n), Is.EqualTo(isEven), nameof(UInt24));
    Assert.That(IsEvenInteger((UInt32)n), Is.EqualTo(isEven), nameof(UInt32));
    Assert.That(IsEvenInteger((UInt48)n), Is.EqualTo(isEven), nameof(UInt48));
    Assert.That(IsEvenInteger((UInt64)n), Is.EqualTo(isEven), nameof(UInt64));

    static bool IsEvenInteger<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsEvenInteger(number);
  }

  [Test]
  public void INumberBase_IsEvenInteger_MaxValue()
  {
    Assert.That(IsMaxValueEvenInteger<UInt16>(), Is.False, nameof(UInt16));
    Assert.That(IsMaxValueEvenInteger<UInt24>(), Is.False, nameof(UInt24));
    Assert.That(IsMaxValueEvenInteger<UInt32>(), Is.False, nameof(UInt32));
    Assert.That(IsMaxValueEvenInteger<UInt48>(), Is.False, nameof(UInt48));
    Assert.That(IsMaxValueEvenInteger<UInt64>(), Is.False, nameof(UInt64));

    static bool IsMaxValueEvenInteger<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsEvenInteger(TUInt.MaxValue);
  }

  [TestCase(0, false)]
  [TestCase(1, true)]
  [TestCase(2, false)]
  public void INumberBase_IsOddInteger(int n, bool isOdd)
  {
    Assert.That(IsOddInteger((UInt16)n), Is.EqualTo(isOdd), nameof(UInt16));
    Assert.That(IsOddInteger((UInt24)n), Is.EqualTo(isOdd), nameof(UInt24));
    Assert.That(IsOddInteger((UInt32)n), Is.EqualTo(isOdd), nameof(UInt32));
    Assert.That(IsOddInteger((UInt48)n), Is.EqualTo(isOdd), nameof(UInt48));
    Assert.That(IsOddInteger((UInt64)n), Is.EqualTo(isOdd), nameof(UInt64));

    static bool IsOddInteger<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsOddInteger(number);
  }

  [Test]
  public void INumberBase_IsOddInteger_MaxValue()
  {
    Assert.That(IsMaxValueEvenInteger<UInt16>(), Is.True, nameof(UInt16));
    Assert.That(IsMaxValueEvenInteger<UInt24>(), Is.True, nameof(UInt24));
    Assert.That(IsMaxValueEvenInteger<UInt32>(), Is.True, nameof(UInt32));
    Assert.That(IsMaxValueEvenInteger<UInt48>(), Is.True, nameof(UInt48));
    Assert.That(IsMaxValueEvenInteger<UInt64>(), Is.True, nameof(UInt64));

    static bool IsMaxValueEvenInteger<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsOddInteger(TUInt.MaxValue);
  }

  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsFinite(int n)
  {
    Assert.That(IsFinite((UInt16)n), Is.True, nameof(UInt16));
    Assert.That(IsFinite((UInt24)n), Is.True, nameof(UInt24));
    Assert.That(IsFinite((UInt32)n), Is.True, nameof(UInt32));
    Assert.That(IsFinite((UInt48)n), Is.True, nameof(UInt48));
    Assert.That(IsFinite((UInt64)n), Is.True, nameof(UInt64));

    static bool IsFinite<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsFinite(number);
  }

  [Test]
  public void INumberBase_IsFinite_MaxValue()
  {
    Assert.That(IsMaxValueFinite<UInt16>(), Is.True, nameof(UInt16));
    Assert.That(IsMaxValueFinite<UInt24>(), Is.True, nameof(UInt24));
    Assert.That(IsMaxValueFinite<UInt32>(), Is.True, nameof(UInt32));
    Assert.That(IsMaxValueFinite<UInt48>(), Is.True, nameof(UInt48));
    Assert.That(IsMaxValueFinite<UInt64>(), Is.True, nameof(UInt64));

    static bool IsMaxValueFinite<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsFinite(TUInt.MaxValue);
  }

  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsInfinity(int n)
  {
    Assert.That(IsInfinity((UInt16)n), Is.False, nameof(UInt16));
    Assert.That(IsInfinity((UInt24)n), Is.False, nameof(UInt24));
    Assert.That(IsInfinity((UInt32)n), Is.False, nameof(UInt32));
    Assert.That(IsInfinity((UInt48)n), Is.False, nameof(UInt48));
    Assert.That(IsInfinity((UInt64)n), Is.False, nameof(UInt64));

    static bool IsInfinity<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsInfinity(number);
  }

  [Test]
  public void INumberBase_IsInfinity_MaxValue()
  {
    Assert.That(IsMaxValueInfinity<UInt16>(), Is.False, nameof(UInt16));
    Assert.That(IsMaxValueInfinity<UInt24>(), Is.False, nameof(UInt24));
    Assert.That(IsMaxValueInfinity<UInt32>(), Is.False, nameof(UInt32));
    Assert.That(IsMaxValueInfinity<UInt48>(), Is.False, nameof(UInt48));
    Assert.That(IsMaxValueInfinity<UInt64>(), Is.False, nameof(UInt64));

    static bool IsMaxValueInfinity<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsInfinity(TUInt.MaxValue);
  }

  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsPositiveInfinity(int n)
  {
    Assert.That(IsPositiveInfinity((UInt16)n), Is.False, nameof(UInt16));
    Assert.That(IsPositiveInfinity((UInt24)n), Is.False, nameof(UInt24));
    Assert.That(IsPositiveInfinity((UInt32)n), Is.False, nameof(UInt32));
    Assert.That(IsPositiveInfinity((UInt48)n), Is.False, nameof(UInt48));
    Assert.That(IsPositiveInfinity((UInt64)n), Is.False, nameof(UInt64));

    static bool IsPositiveInfinity<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsPositiveInfinity(number);
  }

  [Test]
  public void INumberBase_IsPositiveInfinity_MaxValue()
  {
    Assert.That(IsMaxValuePositiveInfinity<UInt16>(), Is.False, nameof(UInt16));
    Assert.That(IsMaxValuePositiveInfinity<UInt24>(), Is.False, nameof(UInt24));
    Assert.That(IsMaxValuePositiveInfinity<UInt32>(), Is.False, nameof(UInt32));
    Assert.That(IsMaxValuePositiveInfinity<UInt48>(), Is.False, nameof(UInt48));
    Assert.That(IsMaxValuePositiveInfinity<UInt64>(), Is.False, nameof(UInt64));

    static bool IsMaxValuePositiveInfinity<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsPositiveInfinity(TUInt.MaxValue);
  }

  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsNegativeInfinity(int n)
  {
    Assert.That(IsNegativeInfinity((UInt16)n), Is.False, nameof(UInt16));
    Assert.That(IsNegativeInfinity((UInt24)n), Is.False, nameof(UInt24));
    Assert.That(IsNegativeInfinity((UInt32)n), Is.False, nameof(UInt32));
    Assert.That(IsNegativeInfinity((UInt48)n), Is.False, nameof(UInt48));
    Assert.That(IsNegativeInfinity((UInt64)n), Is.False, nameof(UInt64));

    static bool IsNegativeInfinity<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsNegativeInfinity(number);
  }

  [Test]
  public void INumberBase_IsNegativeInfinity_MaxValue()
  {
    Assert.That(IsMaxValueNegativeInfinity<UInt16>(), Is.False, nameof(UInt16));
    Assert.That(IsMaxValueNegativeInfinity<UInt24>(), Is.False, nameof(UInt24));
    Assert.That(IsMaxValueNegativeInfinity<UInt32>(), Is.False, nameof(UInt32));
    Assert.That(IsMaxValueNegativeInfinity<UInt48>(), Is.False, nameof(UInt48));
    Assert.That(IsMaxValueNegativeInfinity<UInt64>(), Is.False, nameof(UInt64));

    static bool IsMaxValueNegativeInfinity<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsNegativeInfinity(TUInt.MaxValue);
  }

  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsNaN(int n)
  {
    Assert.That(IsNaN((UInt16)n), Is.False, nameof(UInt16));
    Assert.That(IsNaN((UInt24)n), Is.False, nameof(UInt24));
    Assert.That(IsNaN((UInt32)n), Is.False, nameof(UInt32));
    Assert.That(IsNaN((UInt48)n), Is.False, nameof(UInt48));
    Assert.That(IsNaN((UInt64)n), Is.False, nameof(UInt64));

    static bool IsNaN<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsNaN(number);
  }

  [Test]
  public void INumberBase_IsNaN_MaxValue()
  {
    Assert.That(IsMaxValueNaN<UInt16>(), Is.False, nameof(UInt16));
    Assert.That(IsMaxValueNaN<UInt24>(), Is.False, nameof(UInt24));
    Assert.That(IsMaxValueNaN<UInt32>(), Is.False, nameof(UInt32));
    Assert.That(IsMaxValueNaN<UInt48>(), Is.False, nameof(UInt48));
    Assert.That(IsMaxValueNaN<UInt64>(), Is.False, nameof(UInt64));

    static bool IsMaxValueNaN<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsNaN(TUInt.MaxValue);
  }

  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsInteger(int n)
  {
    Assert.That(IsInteger((UInt16)n), Is.True, nameof(UInt16));
    Assert.That(IsInteger((UInt24)n), Is.True, nameof(UInt24));
    Assert.That(IsInteger((UInt32)n), Is.True, nameof(UInt32));
    Assert.That(IsInteger((UInt48)n), Is.True, nameof(UInt48));
    Assert.That(IsInteger((UInt64)n), Is.True, nameof(UInt64));

    static bool IsInteger<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsInteger(number);
  }

  [Test]
  public void INumberBase_IsInteger_MaxValue()
  {
    Assert.That(IsMaxValueInteger<UInt16>(), Is.True, nameof(UInt16));
    Assert.That(IsMaxValueInteger<UInt24>(), Is.True, nameof(UInt24));
    Assert.That(IsMaxValueInteger<UInt32>(), Is.True, nameof(UInt32));
    Assert.That(IsMaxValueInteger<UInt48>(), Is.True, nameof(UInt48));
    Assert.That(IsMaxValueInteger<UInt64>(), Is.True, nameof(UInt64));

    static bool IsMaxValueInteger<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsInteger(TUInt.MaxValue);
  }

  [TestCase(0, true)]
  [TestCase(1, false)]
  [TestCase(2, false)]
  public void INumberBase_IsZero(int n, bool IsOdd)
  {
    Assert.That(IsZero((UInt16)n), Is.EqualTo(IsOdd), nameof(UInt16));
    Assert.That(IsZero((UInt24)n), Is.EqualTo(IsOdd), nameof(UInt24));
    Assert.That(IsZero((UInt32)n), Is.EqualTo(IsOdd), nameof(UInt32));
    Assert.That(IsZero((UInt48)n), Is.EqualTo(IsOdd), nameof(UInt48));
    Assert.That(IsZero((UInt64)n), Is.EqualTo(IsOdd), nameof(UInt64));

    static bool IsZero<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsZero(number);
  }

  [Test]
  public void INumberBase_IsZero_MaxValue()
  {
    Assert.That(IsMaxValueEvenInteger<UInt16>(), Is.False, nameof(UInt16));
    Assert.That(IsMaxValueEvenInteger<UInt24>(), Is.False, nameof(UInt24));
    Assert.That(IsMaxValueEvenInteger<UInt32>(), Is.False, nameof(UInt32));
    Assert.That(IsMaxValueEvenInteger<UInt48>(), Is.False, nameof(UInt48));
    Assert.That(IsMaxValueEvenInteger<UInt64>(), Is.False, nameof(UInt64));

    static bool IsMaxValueEvenInteger<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsZero(TUInt.MaxValue);
  }

  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsPositive(int n)
  {
    Assert.That(IsPositive((UInt16)n), Is.True, nameof(UInt16));
    Assert.That(IsPositive((UInt24)n), Is.True, nameof(UInt24));
    Assert.That(IsPositive((UInt32)n), Is.True, nameof(UInt32));
    Assert.That(IsPositive((UInt48)n), Is.True, nameof(UInt48));
    Assert.That(IsPositive((UInt64)n), Is.True, nameof(UInt64));

    static bool IsPositive<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsPositive(number);
  }

  [Test]
  public void INumberBase_IsPositive_MaxValue()
  {
    Assert.That(IsMaxValuePositive<UInt16>(), Is.True, nameof(UInt16));
    Assert.That(IsMaxValuePositive<UInt24>(), Is.True, nameof(UInt24));
    Assert.That(IsMaxValuePositive<UInt32>(), Is.True, nameof(UInt32));
    Assert.That(IsMaxValuePositive<UInt48>(), Is.True, nameof(UInt48));
    Assert.That(IsMaxValuePositive<UInt64>(), Is.True, nameof(UInt64));

    static bool IsMaxValuePositive<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsPositive(TUInt.MaxValue);
  }

  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  public void INumberBase_IsNegative(int n)
  {
    Assert.That(IsNegative((UInt16)n), Is.False, nameof(UInt16));
    Assert.That(IsNegative((UInt24)n), Is.False, nameof(UInt24));
    Assert.That(IsNegative((UInt32)n), Is.False, nameof(UInt32));
    Assert.That(IsNegative((UInt48)n), Is.False, nameof(UInt48));
    Assert.That(IsNegative((UInt64)n), Is.False, nameof(UInt64));

    static bool IsNegative<TUInt>(TUInt number) where TUInt : INumberBase<TUInt>
      => TUInt.IsNegative(number);
  }

  [Test]
  public void INumberBase_IsNegative_MaxValue()
  {
    Assert.That(IsMaxValueNegative<UInt16>(), Is.False, nameof(UInt16));
    Assert.That(IsMaxValueNegative<UInt24>(), Is.False, nameof(UInt24));
    Assert.That(IsMaxValueNegative<UInt32>(), Is.False, nameof(UInt32));
    Assert.That(IsMaxValueNegative<UInt48>(), Is.False, nameof(UInt48));
    Assert.That(IsMaxValueNegative<UInt64>(), Is.False, nameof(UInt64));

    static bool IsMaxValueNegative<TUInt>() where TUInt : INumberBase<TUInt>, IMinMaxValue<TUInt>
      => TUInt.IsNegative(TUInt.MaxValue);
  }
#endif

  private static System.Collections.IEnumerable YieldTestCases_INumberBase_Abs_UInt24()
  {
    yield return new object?[] { UInt24.Zero, UInt24.Zero };
    yield return new object?[] { UInt24.One, UInt24.One };
    yield return new object?[] { UInt24.MaxValue, UInt24.MaxValue };
  }

  [TestCaseSource(nameof(YieldTestCases_INumberBase_Abs_UInt24))]
  public void INumberBase_Abs_UInt24(UInt24 value, UInt24 expected)
  {
    Assert.That(UInt24.Abs(value), Is.EqualTo(expected), nameof(UInt24.Abs));

#if SYSTEM_NUMERICS_INUMBERBASE
    Assert.That(INumberBase_Abs(value), Is.EqualTo(expected), nameof(INumberBase_Abs));

    static TUInt24n INumberBase_Abs<TUInt24n>(TUInt24n value) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.Abs(value);
#endif
  }

  private static System.Collections.IEnumerable YieldTestCases_INumberBase_Abs_UInt48()
  {
    yield return new object?[] { UInt48.Zero, UInt48.Zero };
    yield return new object?[] { UInt48.One, UInt48.One };
    yield return new object?[] { UInt48.MaxValue, UInt48.MaxValue };
  }

  [TestCaseSource(nameof(YieldTestCases_INumberBase_Abs_UInt48))]
  public void INumberBase_Abs_UInt48(UInt48 value, UInt48 expected)
  {
    Assert.That(UInt48.Abs(value), Is.EqualTo(expected), nameof(UInt48.Abs));

#if SYSTEM_NUMERICS_INUMBERBASE
    Assert.That(INumberBase_Abs(value), Is.EqualTo(expected), nameof(INumberBase_Abs));

    static TUInt24n INumberBase_Abs<TUInt24n>(TUInt24n value) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.Abs(value);
#endif
  }

#if SYSTEM_NUMERICS_INUMBERBASE
  [TestCaseSource(nameof(YieldTestCases_Max_UInt24))]
  public void INumberBase_MaxMagnitude_UInt24(UInt24 x, UInt24 y, UInt24 expected)
  {
    Assert.That(INumberBase_MaxMagnitude(x, y), Is.EqualTo(expected));

    static TUInt24n INumberBase_MaxMagnitude<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.MaxMagnitude(x, y);
  }

  [TestCaseSource(nameof(YieldTestCases_Max_UInt48))]
  public void INumberBase_MaxMagnitude_UInt48(UInt48 x, UInt48 y, UInt48 expected)
  {
    Assert.That(INumberBase_MaxMagnitude(x, y), Is.EqualTo(expected));

    static TUInt24n INumberBase_MaxMagnitude<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.MaxMagnitude(x, y);
  }

  [TestCaseSource(nameof(YieldTestCases_Max_UInt24))]
  public void INumberBase_MaxMagnitudeNumber_UInt24(UInt24 x, UInt24 y, UInt24 expected)
  {
    Assert.That(INumberBase_MaxMagnitudeNumber(x, y), Is.EqualTo(expected));

    static TUInt24n INumberBase_MaxMagnitudeNumber<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.MaxMagnitudeNumber(x, y);
  }

  [TestCaseSource(nameof(YieldTestCases_Max_UInt48))]
  public void INumberBase_MaxMagnitudeNumber_UInt48(UInt48 x, UInt48 y, UInt48 expected)
  {
    Assert.That(INumberBase_MaxMagnitudeNumber(x, y), Is.EqualTo(expected));

    static TUInt24n INumberBase_MaxMagnitudeNumber<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.MaxMagnitudeNumber(x, y);
  }

  [TestCaseSource(nameof(YieldTestCases_Min_UInt24))]
  public void INumberBase_MinMagnitude_UInt24(UInt24 x, UInt24 y, UInt24 expected)
  {
    Assert.That(INumberBase_MinMagnitude(x, y), Is.EqualTo(expected));

    static TUInt24n INumberBase_MinMagnitude<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.MinMagnitude(x, y);
  }

  [TestCaseSource(nameof(YieldTestCases_Min_UInt48))]
  public void INumberBase_MinMagnitude_UInt48(UInt48 x, UInt48 y, UInt48 expected)
  {
    Assert.That(INumberBase_MinMagnitude(x, y), Is.EqualTo(expected));

    static TUInt24n INumberBase_MinMagnitude<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.MinMagnitude(x, y);
  }

  [TestCaseSource(nameof(YieldTestCases_Min_UInt24))]
  public void INumberBase_MinMagnitudeNumber_UInt24(UInt24 x, UInt24 y, UInt24 expected)
  {
    Assert.That(INumberBase_MinMagnitudeNumber(x, y), Is.EqualTo(expected));

    static TUInt24n INumberBase_MinMagnitudeNumber<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.MinMagnitudeNumber(x, y);
  }

  [TestCaseSource(nameof(YieldTestCases_Min_UInt48))]
  public void INumberBase_MinMagnitudeNumber_UInt48(UInt48 x, UInt48 y, UInt48 expected)
  {
    Assert.That(INumberBase_MinMagnitudeNumber(x, y), Is.EqualTo(expected));

    static TUInt24n INumberBase_MinMagnitudeNumber<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.MinMagnitudeNumber(x, y);
  }
#endif

#if SYSTEM_NUMERICS_INUMBERBASE
  [Test]
  public void TryCreate_Overflow_UInt24()
  {
#pragma warning disable CS0618
    Assert.That(UInt24.TryCreate((sbyte)-1, out var _), Is.False, "sbyte -1");
    Assert.That(UInt24.TryCreate((short)-1, out var _), Is.False, "short -1");
    Assert.That(UInt24.TryCreate((uint)0x1_000000, out var _), Is.False, "uint 0x1_000000");
    Assert.That(UInt24.TryCreate((int)-1, out var _), Is.False, "int -1");
    Assert.That(UInt24.TryCreate((int)0x1_000000, out var _), Is.False, "int 0x1_000000");
    Assert.That(UInt24.TryCreate((ulong)0x1_000000, out var _), Is.False, "ulong 0x1_000000");
    Assert.That(UInt24.TryCreate((long)-1, out var _), Is.False, "long -1");
    Assert.That(UInt24.TryCreate((long)0x1_000000, out var _), Is.False, "long 0x1_000000");
    Assert.That(UInt24.TryCreate((nuint)0x1_000000, out var _), Is.False, "nuint 0x1_000000");
    Assert.That(UInt24.TryCreate((nint)(-1), out var _), Is.False, "nint -1");
    Assert.That(UInt24.TryCreate((nint)0x1_000000, out var _), Is.False, "nint 0x1_000000");
    Assert.That(UInt24.TryCreate((Half)(-1), out var _), Is.False, "Half -1");
    Assert.That(UInt24.TryCreate((float)-1, out var _), Is.False, "float -1");
    Assert.That(UInt24.TryCreate((float)0x1_000000, out var _), Is.False, "float 0x1_000000");
    Assert.That(UInt24.TryCreate((double)-1, out var _), Is.False, "double -1");
    Assert.That(UInt24.TryCreate((double)0x1_000000, out var _), Is.False, "double 0x1_000000");
    Assert.That(UInt24.TryCreate((decimal)-1, out var _), Is.False, "decimal -1");
    Assert.That(UInt24.TryCreate((decimal)0x1_000000, out var _), Is.False, "decimal 0x1_000000");
#pragma warning restore CS0618
  }

  [Test]
  public void TryCreate_Overflow_UInt48()
  {
#pragma warning disable CS0618
    Assert.That(UInt48.TryCreate((sbyte)-1, out var _), Is.False, "sbyte -1");
    Assert.That(UInt48.TryCreate((short)-1, out var _), Is.False, "short -1");
    Assert.That(UInt48.TryCreate((int)-1, out var _), Is.False, "int -1");
    Assert.That(UInt48.TryCreate((ulong)0x1_000000_000000, out var _), Is.False, "ulong 0x1_000000_000000");
    Assert.That(UInt48.TryCreate((long)-1, out var _), Is.False, "long -1");
    Assert.That(UInt48.TryCreate((long)0x1_000000_000000, out var _), Is.False, "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.TryCreate(unchecked((nuint)0x1_000000_000000), out var _), Is.False, "nuint 0x1_000000_000000");
      Assert.That(UInt48.TryCreate(unchecked((nint)0x1_000000_000000), out var _), Is.False, "nint 0x1_000000_000000");
    }
    Assert.That(UInt24.TryCreate((Half)(-1), out var _), Is.False, "Half -1");
    Assert.That(UInt48.TryCreate((float)-1, out var _), Is.False, "float -1");
    Assert.That(UInt48.TryCreate((float)0x1_000000_000000, out var _), Is.False, "float 0x1_000000_000000");
    Assert.That(UInt48.TryCreate((double)-1, out var _), Is.False, "double -1");
    Assert.That(UInt48.TryCreate((double)0x1_000000_000000, out var _), Is.False, "double 0x1_000000_000000");
    Assert.That(UInt48.TryCreate((decimal)-1, out var _), Is.False, "decimal -1");
    Assert.That(UInt48.TryCreate((decimal)0x1_000000_000000, out var _), Is.False, "decimal 0x1_000000_000000");
#pragma warning restore CS0618
  }

  [Test]
  public void TryCreate_TypeNotSupported_UInt24()
  {
#pragma warning disable CS0618
    Assert.That(UInt24.TryCreate(BigInteger.Zero, out _), Is.False, "BigInteger");
    Assert.That(UInt24.TryCreate(Complex.Zero, out _), Is.False, "Complex");
#pragma warning restore CS0618
  }

  [Test]
  public void TryCreate_TypeNotSupported_UInt48()
  {
#pragma warning disable CS0618
    Assert.That(UInt48.TryCreate(BigInteger.Zero, out _), Is.False, "BigInteger");
    Assert.That(UInt48.TryCreate(Complex.Zero, out _), Is.False, "Complex");
#pragma warning restore CS0618
  }
#endif
}
