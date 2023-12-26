// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_INUMBERBASE
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test] public void Zero() => Assert.That(UInt24.Zero, Is.EqualTo(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x00 }, isBigEndian: true)));
  [Test] public void One() => Assert.That(UInt24.One, Is.EqualTo(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x01 }, isBigEndian: true)));
}

partial class UInt48Tests {
  [Test] public void Zero() => Assert.That(UInt48.Zero, Is.EqualTo(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isBigEndian: true)));
  [Test] public void One() => Assert.That(UInt48.One, Is.EqualTo(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isBigEndian: true)));
}

partial class UInt24nTests {
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

  [Test]
  public void INumberBase_Create_Zero_UInt24()
  {
#pragma warning disable CS0618
    Assert.That(UInt24.Create((byte)0), Is.EqualTo(UInt24.Zero), "byte 0");
    Assert.That(UInt24.Create((sbyte)0), Is.EqualTo(UInt24.Zero), "sbyte 0");
    Assert.That(UInt24.Create((char)0), Is.EqualTo(UInt24.Zero), "char 0");
    Assert.That(UInt24.Create((ushort)0), Is.EqualTo(UInt24.Zero), "ushort 0");
    Assert.That(UInt24.Create((short)0), Is.EqualTo(UInt24.Zero), "short 0");
    Assert.That(UInt24.Create((uint)0), Is.EqualTo(UInt24.Zero), "uint 0");
    Assert.That(UInt24.Create((int)0), Is.EqualTo(UInt24.Zero), "int 0");
    Assert.That(UInt24.Create((ulong)0), Is.EqualTo(UInt24.Zero), "ulong 0");
    Assert.That(UInt24.Create((long)0), Is.EqualTo(UInt24.Zero), "long 0");
    Assert.That(UInt24.Create((nuint)0), Is.EqualTo(UInt24.Zero), "nuint 0");
    Assert.That(UInt24.Create((nint)0), Is.EqualTo(UInt24.Zero), "nint 0");
    Assert.That(UInt24.Create((Half)0), Is.EqualTo(UInt24.Zero), "Half 0");
    Assert.That(UInt24.Create((float)0), Is.EqualTo(UInt24.Zero), "float 0");
    Assert.That(UInt24.Create((double)0), Is.EqualTo(UInt24.Zero), "double 0");
    Assert.That(UInt24.Create((decimal)0), Is.EqualTo(UInt24.Zero), "decimal 0");
#pragma warning restore CS0618
  }

  [Test]
  public void INumberBase_CreateTruncating_Zero_UInt24()
  {
    Assert.That(UInt24.CreateTruncating((byte)0), Is.EqualTo(UInt24.Zero), "byte 0");
    Assert.That(UInt24.CreateTruncating((sbyte)0), Is.EqualTo(UInt24.Zero), "sbyte 0");
    Assert.That(UInt24.CreateTruncating((char)0), Is.EqualTo(UInt24.Zero), "char 0");
    Assert.That(UInt24.CreateTruncating((ushort)0), Is.EqualTo(UInt24.Zero), "ushort 0");
    Assert.That(UInt24.CreateTruncating((short)0), Is.EqualTo(UInt24.Zero), "short 0");
    Assert.That(UInt24.CreateTruncating((uint)0), Is.EqualTo(UInt24.Zero), "uint 0");
    Assert.That(UInt24.CreateTruncating((int)0), Is.EqualTo(UInt24.Zero), "int 0");
    Assert.That(UInt24.CreateTruncating((ulong)0), Is.EqualTo(UInt24.Zero), "ulong 0");
    Assert.That(UInt24.CreateTruncating((long)0), Is.EqualTo(UInt24.Zero), "long 0");
    Assert.That(UInt24.CreateTruncating((nuint)0), Is.EqualTo(UInt24.Zero), "nuint 0");
    Assert.That(UInt24.CreateTruncating((nint)0), Is.EqualTo(UInt24.Zero), "nint 0");
    Assert.That(UInt24.CreateTruncating((Half)0), Is.EqualTo(UInt24.Zero), "Half 0");
    Assert.That(UInt24.CreateTruncating((float)0), Is.EqualTo(UInt24.Zero), "float 0");
    Assert.That(UInt24.CreateTruncating((double)0), Is.EqualTo(UInt24.Zero), "double 0");
    Assert.That(UInt24.CreateTruncating((decimal)0), Is.EqualTo(UInt24.Zero), "decimal 0");
  }

  [Test]
  public void INumberBase_CreateSaturating_Zero_UInt24()
  {
    Assert.That(UInt24.CreateSaturating((byte)0), Is.EqualTo(UInt24.Zero), "byte 0");
    Assert.That(UInt24.CreateSaturating((sbyte)0), Is.EqualTo(UInt24.Zero), "sbyte 0");
    Assert.That(UInt24.CreateSaturating((char)0), Is.EqualTo(UInt24.Zero), "char 0");
    Assert.That(UInt24.CreateSaturating((ushort)0), Is.EqualTo(UInt24.Zero), "ushort 0");
    Assert.That(UInt24.CreateSaturating((short)0), Is.EqualTo(UInt24.Zero), "short 0");
    Assert.That(UInt24.CreateSaturating((uint)0), Is.EqualTo(UInt24.Zero), "uint 0");
    Assert.That(UInt24.CreateSaturating((int)0), Is.EqualTo(UInt24.Zero), "int 0");
    Assert.That(UInt24.CreateSaturating((ulong)0), Is.EqualTo(UInt24.Zero), "ulong 0");
    Assert.That(UInt24.CreateSaturating((long)0), Is.EqualTo(UInt24.Zero), "long 0");
    Assert.That(UInt24.CreateSaturating((nuint)0), Is.EqualTo(UInt24.Zero), "nuint 0");
    Assert.That(UInt24.CreateSaturating((nint)0), Is.EqualTo(UInt24.Zero), "nint 0");
    Assert.That(UInt24.CreateSaturating((Half)0), Is.EqualTo(UInt24.Zero), "Half 0");
    Assert.That(UInt24.CreateSaturating((float)0), Is.EqualTo(UInt24.Zero), "float 0");
    Assert.That(UInt24.CreateSaturating((double)0), Is.EqualTo(UInt24.Zero), "double 0");
    Assert.That(UInt24.CreateSaturating((decimal)0), Is.EqualTo(UInt24.Zero), "decimal 0");
  }

  [Test]
  public void INumberBase_Create_Zero_UInt48()
  {
#pragma warning disable CS0618
    Assert.That(UInt48.Create((byte)0), Is.EqualTo(UInt48.Zero), "byte 0");
    Assert.That(UInt48.Create((sbyte)0), Is.EqualTo(UInt48.Zero), "sbyte 0");
    Assert.That(UInt48.Create((char)0), Is.EqualTo(UInt48.Zero), "char 0");
    Assert.That(UInt48.Create((ushort)0), Is.EqualTo(UInt48.Zero), "ushort 0");
    Assert.That(UInt48.Create((short)0), Is.EqualTo(UInt48.Zero), "short 0");
    Assert.That(UInt48.Create((uint)0), Is.EqualTo(UInt48.Zero), "uint 0");
    Assert.That(UInt48.Create((int)0), Is.EqualTo(UInt48.Zero), "int 0");
    Assert.That(UInt48.Create((ulong)0), Is.EqualTo(UInt48.Zero), "ulong 0");
    Assert.That(UInt48.Create((long)0), Is.EqualTo(UInt48.Zero), "long 0");
    Assert.That(UInt48.Create((nuint)0), Is.EqualTo(UInt48.Zero), "nuint 0");
    Assert.That(UInt48.Create((nint)0), Is.EqualTo(UInt48.Zero), "nint 0");
    Assert.That(UInt48.Create((Half)0), Is.EqualTo(UInt48.Zero), "Half 0");
    Assert.That(UInt48.Create((float)0), Is.EqualTo(UInt48.Zero), "float 0");
    Assert.That(UInt48.Create((double)0), Is.EqualTo(UInt48.Zero), "double 0");
    Assert.That(UInt48.Create((decimal)0), Is.EqualTo(UInt48.Zero), "decimal 0");
#pragma warning restore CS0618
  }

  [Test]
  public void INumberBase_CreateTruncating_Zero_UInt48()
  {
    Assert.That(UInt48.CreateTruncating((byte)0), Is.EqualTo(UInt48.Zero), "byte 0");
    Assert.That(UInt48.CreateTruncating((sbyte)0), Is.EqualTo(UInt48.Zero), "sbyte 0");
    Assert.That(UInt48.CreateTruncating((char)0), Is.EqualTo(UInt48.Zero), "char 0");
    Assert.That(UInt48.CreateTruncating((ushort)0), Is.EqualTo(UInt48.Zero), "ushort 0");
    Assert.That(UInt48.CreateTruncating((short)0), Is.EqualTo(UInt48.Zero), "short 0");
    Assert.That(UInt48.CreateTruncating((uint)0), Is.EqualTo(UInt48.Zero), "uint 0");
    Assert.That(UInt48.CreateTruncating((int)0), Is.EqualTo(UInt48.Zero), "int 0");
    Assert.That(UInt48.CreateTruncating((ulong)0), Is.EqualTo(UInt48.Zero), "ulong 0");
    Assert.That(UInt48.CreateTruncating((long)0), Is.EqualTo(UInt48.Zero), "long 0");
    Assert.That(UInt48.CreateTruncating((nuint)0), Is.EqualTo(UInt48.Zero), "nuint 0");
    Assert.That(UInt48.CreateTruncating((nint)0), Is.EqualTo(UInt48.Zero), "nint 0");
    Assert.That(UInt48.CreateTruncating((Half)0), Is.EqualTo(UInt48.Zero), "Half 0");
    Assert.That(UInt48.CreateTruncating((float)0), Is.EqualTo(UInt48.Zero), "float 0");
    Assert.That(UInt48.CreateTruncating((double)0), Is.EqualTo(UInt48.Zero), "double 0");
    Assert.That(UInt48.CreateTruncating((decimal)0), Is.EqualTo(UInt48.Zero), "decimal 0");
  }

  [Test]
  public void INumberBase_CreateSaturating_Zero_UInt48()
  {
    Assert.That(UInt48.CreateSaturating((byte)0), Is.EqualTo(UInt48.Zero), "byte 0");
    Assert.That(UInt48.CreateSaturating((sbyte)0), Is.EqualTo(UInt48.Zero), "sbyte 0");
    Assert.That(UInt48.CreateSaturating((char)0), Is.EqualTo(UInt48.Zero), "char 0");
    Assert.That(UInt48.CreateSaturating((ushort)0), Is.EqualTo(UInt48.Zero), "ushort 0");
    Assert.That(UInt48.CreateSaturating((short)0), Is.EqualTo(UInt48.Zero), "short 0");
    Assert.That(UInt48.CreateSaturating((uint)0), Is.EqualTo(UInt48.Zero), "uint 0");
    Assert.That(UInt48.CreateSaturating((int)0), Is.EqualTo(UInt48.Zero), "int 0");
    Assert.That(UInt48.CreateSaturating((ulong)0), Is.EqualTo(UInt48.Zero), "ulong 0");
    Assert.That(UInt48.CreateSaturating((long)0), Is.EqualTo(UInt48.Zero), "long 0");
    Assert.That(UInt48.CreateSaturating((nuint)0), Is.EqualTo(UInt48.Zero), "nuint 0");
    Assert.That(UInt48.CreateSaturating((nint)0), Is.EqualTo(UInt48.Zero), "nint 0");
    Assert.That(UInt48.CreateSaturating((Half)0), Is.EqualTo(UInt48.Zero), "Half 0");
    Assert.That(UInt48.CreateSaturating((float)0), Is.EqualTo(UInt48.Zero), "float 0");
    Assert.That(UInt48.CreateSaturating((double)0), Is.EqualTo(UInt48.Zero), "double 0");
    Assert.That(UInt48.CreateSaturating((decimal)0), Is.EqualTo(UInt48.Zero), "decimal 0");
  }

  [Test]
  public void TryCreate_Zero_UInt24()
  {
#pragma warning disable CS0618
    Assert.That(UInt24.TryCreate((byte)0, out var r_byte), Is.True, "byte 0"); Assert.That(r_byte, Is.EqualTo(UInt24.Zero), "result byte 0");
    Assert.That(UInt24.TryCreate((sbyte)0, out var r_sbyte), Is.True, "sbyte 0"); Assert.That(r_sbyte, Is.EqualTo(UInt24.Zero), "result sbyte 0");
    Assert.That(UInt24.TryCreate((char)0, out var r_char), Is.True, "char 0"); Assert.That(r_char, Is.EqualTo(UInt24.Zero), "result char 0");
    Assert.That(UInt24.TryCreate((ushort)0, out var r_ushort), Is.True, "ushort 0"); Assert.That(r_ushort, Is.EqualTo(UInt24.Zero), "result ushort 0");
    Assert.That(UInt24.TryCreate((short)0, out var r_short), Is.True, "short 0"); Assert.That(r_short, Is.EqualTo(UInt24.Zero), "result short 0");
    Assert.That(UInt24.TryCreate((uint)0, out var r_uint), Is.True, "uint 0"); Assert.That(r_uint, Is.EqualTo(UInt24.Zero), "result uint 0");
    Assert.That(UInt24.TryCreate((int)0, out var r_int), Is.True, "int 0"); Assert.That(r_int, Is.EqualTo(UInt24.Zero), "result int 0");
    Assert.That(UInt24.TryCreate((ulong)0, out var r_ulong), Is.True, "ulong 0"); Assert.That(r_ulong, Is.EqualTo(UInt24.Zero), "result ulong 0");
    Assert.That(UInt24.TryCreate((long)0, out var r_long), Is.True, "long 0"); Assert.That(r_long, Is.EqualTo(UInt24.Zero), "result long 0");
    Assert.That(UInt24.TryCreate((nuint)0, out var r_nuint), Is.True, "nuint 0"); Assert.That(r_nuint, Is.EqualTo(UInt24.Zero), "result nuint 0");
    Assert.That(UInt24.TryCreate((nint)0, out var r_nint), Is.True, "nint 0"); Assert.That(r_nint, Is.EqualTo(UInt24.Zero), "result nint 0");
    Assert.That(UInt24.TryCreate((Half)0, out var r_half), Is.True, "Half 0"); Assert.That(r_half, Is.EqualTo(UInt24.Zero), "result Half 0");
    Assert.That(UInt24.TryCreate((float)0, out var r_float), Is.True, "float 0"); Assert.That(r_float, Is.EqualTo(UInt24.Zero), "result float 0");
    Assert.That(UInt24.TryCreate((double)0, out var r_double), Is.True, "double 0"); Assert.That(r_double, Is.EqualTo(UInt24.Zero), "result double 0");
    Assert.That(UInt24.TryCreate((decimal)0, out var r_decimal), Is.True, "decimal 0"); Assert.That(r_decimal, Is.EqualTo(UInt24.Zero), "result decimal 0");
#pragma warning restore CS0618
  }

  [Test]
  public void TryCreate_Zero_UInt48()
  {
#pragma warning disable CS0618
    Assert.That(UInt48.TryCreate((byte)0, out var r_byte), Is.True, "byte 0"); Assert.That(r_byte, Is.EqualTo(UInt48.Zero), "result byte 0");
    Assert.That(UInt48.TryCreate((sbyte)0, out var r_sbyte), Is.True, "sbyte 0"); Assert.That(r_sbyte, Is.EqualTo(UInt48.Zero), "result sbyte 0");
    Assert.That(UInt48.TryCreate((char)0, out var r_char), Is.True, "char 0"); Assert.That(r_char, Is.EqualTo(UInt48.Zero), "result char 0");
    Assert.That(UInt48.TryCreate((ushort)0, out var r_ushort), Is.True, "ushort 0"); Assert.That(r_ushort, Is.EqualTo(UInt48.Zero), "result ushort 0");
    Assert.That(UInt48.TryCreate((short)0, out var r_short), Is.True, "short 0"); Assert.That(r_short, Is.EqualTo(UInt48.Zero), "result short 0");
    Assert.That(UInt48.TryCreate((uint)0, out var r_uint), Is.True, "uint 0"); Assert.That(r_uint, Is.EqualTo(UInt48.Zero), "result uint 0");
    Assert.That(UInt48.TryCreate((int)0, out var r_int), Is.True, "int 0"); Assert.That(r_int, Is.EqualTo(UInt48.Zero), "result int 0");
    Assert.That(UInt48.TryCreate((ulong)0, out var r_ulong), Is.True, "ulong 0"); Assert.That(r_ulong, Is.EqualTo(UInt48.Zero), "result ulong 0");
    Assert.That(UInt48.TryCreate((long)0, out var r_long), Is.True, "long 0"); Assert.That(r_long, Is.EqualTo(UInt48.Zero), "result long 0");
    Assert.That(UInt48.TryCreate((nuint)0, out var r_nuint), Is.True, "nuint 0"); Assert.That(r_nuint, Is.EqualTo(UInt48.Zero), "result nuint 0");
    Assert.That(UInt48.TryCreate((nint)0, out var r_nint), Is.True, "nint 0"); Assert.That(r_nint, Is.EqualTo(UInt48.Zero), "result nint 0");
    Assert.That(UInt48.TryCreate((Half)0, out var r_half), Is.True, "Half 0"); Assert.That(r_half, Is.EqualTo(UInt48.Zero), "result Half 0");
    Assert.That(UInt48.TryCreate((float)0, out var r_float), Is.True, "float 0"); Assert.That(r_float, Is.EqualTo(UInt48.Zero), "result float 0");
    Assert.That(UInt48.TryCreate((double)0, out var r_double), Is.True, "double 0"); Assert.That(r_double, Is.EqualTo(UInt48.Zero), "result double 0");
    Assert.That(UInt48.TryCreate((decimal)0, out var r_decimal), Is.True, "decimal 0"); Assert.That(r_decimal, Is.EqualTo(UInt48.Zero), "result decimal 0");
#pragma warning restore CS0618
  }

  [Test]
  public void INumberBase_TryConvertFromSaturating_Zero_UInt24()
  {
    Assert.That(UInt24.TryConvertFromSaturating((byte)0, out var r_byte), Is.True, "byte 0"); Assert.That(r_byte, Is.EqualTo(UInt24.Zero), "result byte 0");
    Assert.That(UInt24.TryConvertFromSaturating((sbyte)0, out var r_sbyte), Is.True, "sbyte 0"); Assert.That(r_sbyte, Is.EqualTo(UInt24.Zero), "result sbyte 0");
    Assert.That(UInt24.TryConvertFromSaturating((char)0, out var r_char), Is.True, "char 0"); Assert.That(r_char, Is.EqualTo(UInt24.Zero), "result char 0");
    Assert.That(UInt24.TryConvertFromSaturating((ushort)0, out var r_ushort), Is.True, "ushort 0"); Assert.That(r_ushort, Is.EqualTo(UInt24.Zero), "result ushort 0");
    Assert.That(UInt24.TryConvertFromSaturating((short)0, out var r_short), Is.True, "short 0"); Assert.That(r_short, Is.EqualTo(UInt24.Zero), "result short 0");
    Assert.That(UInt24.TryConvertFromSaturating((uint)0, out var r_uint), Is.True, "uint 0"); Assert.That(r_uint, Is.EqualTo(UInt24.Zero), "result uint 0");
    Assert.That(UInt24.TryConvertFromSaturating((int)0, out var r_int), Is.True, "int 0"); Assert.That(r_int, Is.EqualTo(UInt24.Zero), "result int 0");
    Assert.That(UInt24.TryConvertFromSaturating((ulong)0, out var r_ulong), Is.True, "ulong 0"); Assert.That(r_ulong, Is.EqualTo(UInt24.Zero), "result ulong 0");
    Assert.That(UInt24.TryConvertFromSaturating((long)0, out var r_long), Is.True, "long 0"); Assert.That(r_long, Is.EqualTo(UInt24.Zero), "result long 0");
    Assert.That(UInt24.TryConvertFromSaturating((nuint)0, out var r_nuint), Is.True, "nuint 0"); Assert.That(r_nuint, Is.EqualTo(UInt24.Zero), "result nuint 0");
    Assert.That(UInt24.TryConvertFromSaturating((nint)0, out var r_nint), Is.True, "nint 0"); Assert.That(r_nint, Is.EqualTo(UInt24.Zero), "result nint 0");
    Assert.That(UInt24.TryConvertFromSaturating((Half)0, out var r_half), Is.True, "Half 0"); Assert.That(r_half, Is.EqualTo(UInt24.Zero), "result Half 0");
    Assert.That(UInt24.TryConvertFromSaturating((float)0, out var r_float), Is.True, "float 0"); Assert.That(r_float, Is.EqualTo(UInt24.Zero), "result float 0");
    Assert.That(UInt24.TryConvertFromSaturating((double)0, out var r_double), Is.True, "double 0"); Assert.That(r_double, Is.EqualTo(UInt24.Zero), "result double 0");
    Assert.That(UInt24.TryConvertFromSaturating((decimal)0, out var r_decimal), Is.True, "decimal 0"); Assert.That(r_decimal, Is.EqualTo(UInt24.Zero), "result decimal 0");
  }

  [Test]
  public void INumberBase_TryConvertFromSaturating_Zero_UInt48()
  {
    Assert.That(UInt48.TryConvertFromSaturating((byte)0, out var r_byte), Is.True, "byte 0"); Assert.That(r_byte, Is.EqualTo(UInt48.Zero), "result byte 0");
    Assert.That(UInt48.TryConvertFromSaturating((sbyte)0, out var r_sbyte), Is.True, "sbyte 0"); Assert.That(r_sbyte, Is.EqualTo(UInt48.Zero), "result sbyte 0");
    Assert.That(UInt48.TryConvertFromSaturating((char)0, out var r_char), Is.True, "char 0"); Assert.That(r_char, Is.EqualTo(UInt48.Zero), "result char 0");
    Assert.That(UInt48.TryConvertFromSaturating((ushort)0, out var r_ushort), Is.True, "ushort 0"); Assert.That(r_ushort, Is.EqualTo(UInt48.Zero), "result ushort 0");
    Assert.That(UInt48.TryConvertFromSaturating((short)0, out var r_short), Is.True, "short 0"); Assert.That(r_short, Is.EqualTo(UInt48.Zero), "result short 0");
    Assert.That(UInt48.TryConvertFromSaturating((uint)0, out var r_uint), Is.True, "uint 0"); Assert.That(r_uint, Is.EqualTo(UInt48.Zero), "result uint 0");
    Assert.That(UInt48.TryConvertFromSaturating((int)0, out var r_int), Is.True, "int 0"); Assert.That(r_int, Is.EqualTo(UInt48.Zero), "result int 0");
    Assert.That(UInt48.TryConvertFromSaturating((ulong)0, out var r_ulong), Is.True, "ulong 0"); Assert.That(r_ulong, Is.EqualTo(UInt48.Zero), "result ulong 0");
    Assert.That(UInt48.TryConvertFromSaturating((long)0, out var r_long), Is.True, "long 0"); Assert.That(r_long, Is.EqualTo(UInt48.Zero), "result long 0");
    Assert.That(UInt48.TryConvertFromSaturating((nuint)0, out var r_nuint), Is.True, "nuint 0"); Assert.That(r_nuint, Is.EqualTo(UInt48.Zero), "result nuint 0");
    Assert.That(UInt48.TryConvertFromSaturating((nint)0, out var r_nint), Is.True, "nint 0"); Assert.That(r_nint, Is.EqualTo(UInt48.Zero), "result nint 0");
    Assert.That(UInt48.TryConvertFromSaturating((Half)0, out var r_half), Is.True, "Half 0"); Assert.That(r_half, Is.EqualTo(UInt48.Zero), "result Half 0");
    Assert.That(UInt48.TryConvertFromSaturating((float)0, out var r_float), Is.True, "float 0"); Assert.That(r_float, Is.EqualTo(UInt48.Zero), "result float 0");
    Assert.That(UInt48.TryConvertFromSaturating((double)0, out var r_double), Is.True, "double 0"); Assert.That(r_double, Is.EqualTo(UInt48.Zero), "result double 0");
    Assert.That(UInt48.TryConvertFromSaturating((decimal)0, out var r_decimal), Is.True, "decimal 0"); Assert.That(r_decimal, Is.EqualTo(UInt48.Zero), "result decimal 0");
  }

  [Test]
  public void INumberBase_Create_UInt24()
  {
#pragma warning disable CS0618
    Assert.That(UInt24.Create((byte)0xFF), Is.EqualTo((UInt24)0xFF), "byte 0xFF");
    Assert.That(UInt24.Create((sbyte)0x7F), Is.EqualTo((UInt24)0x7F), "sbyte 0x7F");
    Assert.That(UInt24.Create((char)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "char 0xFFFF");
    Assert.That(UInt24.Create((ushort)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt24.Create((short)0x7FFF), Is.EqualTo((UInt24)0x7FFF), "short 0x7FFF");
    Assert.That(UInt24.Create((uint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "uint 0xFFFFFF");
    Assert.That(UInt24.Create((int)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "int 0xFFFFFF");
    Assert.That(UInt24.Create((ulong)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "ulong 0xFFFFFF");
    Assert.That(UInt24.Create((long)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "long 0xFFFFFF");
    Assert.That(UInt24.Create((nuint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nuint 0xFFFFFF");
    Assert.That(UInt24.Create((nint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nint 0xFFFFFF");
    Assert.That(UInt24.Create((Half)0xFFE0), Is.EqualTo((UInt24)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt24.Create((float)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "float 0xFFFFFF");
    Assert.That(UInt24.Create((double)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "double 0xFFFFFF");
    Assert.That(UInt24.Create((decimal)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "decimal 0xFFFFFF");
#pragma warning restore CS0618
  }

  [Test]
  public void INumberBase_CreateTruncating_UInt24()
  {
    Assert.That(UInt24.CreateTruncating((byte)0xFF), Is.EqualTo((UInt24)0xFF), "byte 0xFF");
    Assert.That(UInt24.CreateTruncating((sbyte)0x7F), Is.EqualTo((UInt24)0x7F), "sbyte 0x7F");
    Assert.That(UInt24.CreateTruncating((char)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "char 0xFFFF");
    Assert.That(UInt24.CreateTruncating((ushort)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt24.CreateTruncating((short)0x7FFF), Is.EqualTo((UInt24)0x7FFF), "short 0x7FFF");
    Assert.That(UInt24.CreateTruncating((uint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "uint 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((int)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "int 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((ulong)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "ulong 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((long)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "long 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((nuint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nuint 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((nint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nint 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((Half)0xFFE0), Is.EqualTo((UInt24)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt24.CreateTruncating((float)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "float 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((double)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "double 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((decimal)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "decimal 0xFFFFFF");
  }

  [Test]
  public void INumberBase_CreateSaturating_UInt24()
  {
    Assert.That(UInt24.CreateSaturating((byte)0xFF), Is.EqualTo((UInt24)0xFF), "byte 0xFF");
    Assert.That(UInt24.CreateSaturating((sbyte)0x7F), Is.EqualTo((UInt24)0x7F), "sbyte 0x7F");
    Assert.That(UInt24.CreateSaturating((char)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "char 0xFFFF");
    Assert.That(UInt24.CreateSaturating((ushort)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt24.CreateSaturating((short)0x7FFF), Is.EqualTo((UInt24)0x7FFF), "short 0x7FFF");
    Assert.That(UInt24.CreateSaturating((uint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "uint 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((int)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "int 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((ulong)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "ulong 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((long)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "long 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((nuint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nuint 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((nint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nint 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((Half)0xFFE0), Is.EqualTo((UInt24)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt24.CreateSaturating((float)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "float 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((double)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "double 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((decimal)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "decimal 0xFFFFFF");
  }

  [Test]
  public void INumberBase_Create_UInt48()
  {
#pragma warning disable CS0618
    Assert.That(UInt48.Create((byte)0xFF), Is.EqualTo((UInt48)0xFF), "byte 0xFF");
    Assert.That(UInt48.Create((sbyte)0x7F), Is.EqualTo((UInt48)0x7F), "sbyte 0x7F");
    Assert.That(UInt48.Create((char)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "char 0xFFFF");
    Assert.That(UInt48.Create((ushort)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt48.Create((short)0x7FFF), Is.EqualTo((UInt48)0x7FFF), "short 0x7FFF");
    Assert.That(UInt48.Create((uint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "uint 0xFFFFFFFF");
    Assert.That(UInt48.Create((int)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "int 0x7FFFFFFF");
    Assert.That(UInt48.Create((ulong)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.Create((long)0x7FFF_FFFFFFFF), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.Create(unchecked((nuint)0xFFFF_FFFFFFFF)), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.Create(unchecked((nint)0x7FFF_FFFFFFFF)), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.Create((nuint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "nuint 0xFFFFFFFF");
      Assert.That(UInt48.Create((nint)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.Create((Half)0xFFE0), Is.EqualTo((UInt48)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt48.Create((float)2.8147496E+014), Is.EqualTo((UInt48)0xFFFF_FF000000), "float 2.8147496E+014");
    Assert.That(UInt48.Create((double)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.Create((decimal)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "decimal 0xFFFF_FFFFFFFF");
#pragma warning restore CS0618
  }

  [Test]
  public void INumberBase_CreateTruncating_UInt48()
  {
    Assert.That(UInt48.CreateTruncating((byte)0xFF), Is.EqualTo((UInt48)0xFF), "byte 0xFF");
    Assert.That(UInt48.CreateTruncating((sbyte)0x7F), Is.EqualTo((UInt48)0x7F), "sbyte 0x7F");
    Assert.That(UInt48.CreateTruncating((char)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "char 0xFFFF");
    Assert.That(UInt48.CreateTruncating((ushort)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt48.CreateTruncating((short)0x7FFF), Is.EqualTo((UInt48)0x7FFF), "short 0x7FFF");
    Assert.That(UInt48.CreateTruncating((uint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "uint 0xFFFFFFFF");
    Assert.That(UInt48.CreateTruncating((int)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "int 0x7FFFFFFF");
    Assert.That(UInt48.CreateTruncating((ulong)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.CreateTruncating((long)0x7FFF_FFFFFFFF), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.CreateTruncating(unchecked((nuint)0xFFFF_FFFFFFFF)), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.CreateTruncating(unchecked((nint)0x7FFF_FFFFFFFF)), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.CreateTruncating((nuint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "nuint 0xFFFFFFFF");
      Assert.That(UInt48.CreateTruncating((nint)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.CreateTruncating((Half)0xFFE0), Is.EqualTo((UInt48)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt48.CreateTruncating((float)2.8147496E+014), Is.EqualTo((UInt48)0xFFFF_FF000000), "float 2.8147496E+014");
    Assert.That(UInt48.CreateTruncating((double)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.CreateTruncating((decimal)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "decimal 0xFFFF_FFFFFFFF");
  }

  [Test]
  public void INumberBase_CreateSaturating_UInt48()
  {
    Assert.That(UInt48.CreateSaturating((byte)0xFF), Is.EqualTo((UInt48)0xFF), "byte 0xFF");
    Assert.That(UInt48.CreateSaturating((sbyte)0x7F), Is.EqualTo((UInt48)0x7F), "sbyte 0x7F");
    Assert.That(UInt48.CreateSaturating((char)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "char 0xFFFF");
    Assert.That(UInt48.CreateSaturating((ushort)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt48.CreateSaturating((short)0x7FFF), Is.EqualTo((UInt48)0x7FFF), "short 0x7FFF");
    Assert.That(UInt48.CreateSaturating((uint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "uint 0xFFFFFFFF");
    Assert.That(UInt48.CreateSaturating((int)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "int 0x7FFFFFFF");
    Assert.That(UInt48.CreateSaturating((ulong)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.CreateSaturating((long)0x7FFF_FFFFFFFF), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.CreateSaturating(unchecked((nuint)0xFFFF_FFFFFFFF)), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.CreateSaturating(unchecked((nint)0x7FFF_FFFFFFFF)), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.CreateSaturating((nuint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "nuint 0xFFFFFFFF");
      Assert.That(UInt48.CreateSaturating((nint)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.CreateSaturating((Half)0xFFE0), Is.EqualTo((UInt48)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt48.CreateSaturating((float)2.8147496E+014), Is.EqualTo((UInt48)0xFFFF_FF000000), "float 2.8147496E+014");
    Assert.That(UInt48.CreateSaturating((double)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.CreateSaturating((decimal)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "decimal 0xFFFF_FFFFFFFF");
  }

  [Test]
  public void TryCreate_UInt24()
  {
#pragma warning disable CS0618
    Assert.That(UInt24.TryCreate((byte)0xFF, out var r_byte), Is.True, "byte 0xFF"); Assert.That(r_byte, Is.EqualTo((UInt24)0xFF), "result byte 0xFF");
    Assert.That(UInt24.TryCreate((sbyte)0x7F, out var r_sbyte), Is.True, "sbyte 0x7F"); Assert.That(r_sbyte, Is.EqualTo((UInt24)0x7F), "result sbyte 0x7F");
    Assert.That(UInt24.TryCreate((char)0xFFFF, out var r_char), Is.True, "char 0xFFFF"); Assert.That(r_char, Is.EqualTo((UInt24)0xFFFF), "result char 0xFFFF");
    Assert.That(UInt24.TryCreate((ushort)0xFFFF, out var r_ushort), Is.True, "ushort 0xFFFF"); Assert.That(r_ushort, Is.EqualTo((UInt24)0xFFFF), "result ushort 0xFFFF");
    Assert.That(UInt24.TryCreate((short)0x7FFF, out var r_short), Is.True, "short 0x7FFF"); Assert.That(r_short, Is.EqualTo((UInt24)0x7FFF), "result short 0x7FFF");
    Assert.That(UInt24.TryCreate((uint)0xFFFFFF, out var r_uint), Is.True, "uint 0xFFFFFF"); Assert.That(r_uint, Is.EqualTo((UInt24)0xFFFFFF), "result uint 0xFFFFFF");
    Assert.That(UInt24.TryCreate((int)0xFFFFFF, out var r_int), Is.True, "int 0xFFFFFF"); Assert.That(r_int, Is.EqualTo((UInt24)0xFFFFFF), "result int 0xFFFFFF");
    Assert.That(UInt24.TryCreate((ulong)0xFFFFFF, out var r_ulong), Is.True, "ulong 0xFFFFFF"); Assert.That(r_ulong, Is.EqualTo((UInt24)0xFFFFFF), "result ulong 0xFFFFFF");
    Assert.That(UInt24.TryCreate((long)0xFFFFFF, out var r_long), Is.True, "long 0xFFFFFF"); Assert.That(r_long, Is.EqualTo((UInt24)0xFFFFFF), "result long 0xFFFFFF");
    Assert.That(UInt24.TryCreate((nuint)0xFFFFFF, out var r_nuint), Is.True, "nuint 0xFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt24)0xFFFFFF), "result nuint 0xFFFFFF");
    Assert.That(UInt24.TryCreate((nint)0xFFFFFF, out var r_nint), Is.True, "nint 0xFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt24)0xFFFFFF), "result nint 0xFFFFFF");
    Assert.That(UInt24.TryCreate((Half)0xFFE0, out var r_half), Is.True, "Half 0xFFE0"); Assert.That(r_half, Is.EqualTo((UInt24)0xFFE0), "result Half 0xFFE0");
    Assert.That(UInt24.TryCreate((float)0xFFFFFF, out var r_float), Is.True, "float 0xFFFFFF"); Assert.That(r_float, Is.EqualTo((UInt24)0xFFFFFF), "result float 0xFFFFFF");
    Assert.That(UInt24.TryCreate((double)0xFFFFFF, out var r_double), Is.True, "double 0xFFFFFF"); Assert.That(r_double, Is.EqualTo((UInt24)0xFFFFFF), "result double 0xFFFFFF");
    Assert.That(UInt24.TryCreate((decimal)0xFFFFFF, out var r_decimal), Is.True, "decimal 0xFFFFFF"); Assert.That(r_decimal, Is.EqualTo((UInt24)0xFFFFFF), "result decimal 0xFFFFFF");
#pragma warning restore CS0618
  }

  [Test]
  public void TryCreate_UInt48()
  {
#pragma warning disable CS0618
    Assert.That(UInt48.TryCreate((byte)0xFF, out var r_byte), Is.True, "byte 0xFF"); Assert.That(r_byte, Is.EqualTo((UInt48)0xFF), "result byte 0xFF");
    Assert.That(UInt48.TryCreate((sbyte)0x7F, out var r_sbyte), Is.True, "sbyte 0x7F"); Assert.That(r_sbyte, Is.EqualTo((UInt48)0x7F), "result sbyte 0x7F");
    Assert.That(UInt48.TryCreate((char)0xFFFF, out var r_char), Is.True, "char 0xFFFF"); Assert.That(r_char, Is.EqualTo((UInt48)0xFFFF), "result char 0xFFFF");
    Assert.That(UInt48.TryCreate((ushort)0xFFFF, out var r_ushort), Is.True, "ushort 0xFFFF"); Assert.That(r_ushort, Is.EqualTo((UInt48)0xFFFF), "result ushort 0xFFFF");
    Assert.That(UInt48.TryCreate((short)0x7FFF, out var r_short), Is.True, "short 0x7FFF"); Assert.That(r_short, Is.EqualTo((UInt48)0x7FFF), "result short 0x7FFF");
    Assert.That(UInt48.TryCreate((uint)0xFFFFFFFF, out var r_uint), Is.True, "uint 0xFFFFFFFF"); Assert.That(r_uint, Is.EqualTo((UInt48)0xFFFFFFFF), "result uint 0xFFFFFFFF");
    Assert.That(UInt48.TryCreate((int)0x7FFFFFFF, out var r_int), Is.True, "int 0x7FFFFFFF"); Assert.That(r_int, Is.EqualTo((UInt48)0x7FFFFFFF), "result int 0x7FFFFFFF");
    Assert.That(UInt48.TryCreate((ulong)0xFFFF_FFFFFFFF, out var r_ulong), Is.True, "ulong 0xFFFF_FFFFFFFF"); Assert.That(r_ulong, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.TryCreate((long)0x7FFF_FFFFFFFF, out var r_long), Is.True, "long 0x7FFF_FFFFFFFF"); Assert.That(r_long, Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "result long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.TryCreate(unchecked((nuint)0xFFFF_FFFFFFFF), out var r_nuint), Is.True, "nuint 0xFFFF_FFFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.TryCreate(unchecked((nint)0x7FFF_FFFFFFFF), out var r_nint), Is.True, "nint 0x7FFF_FFFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "result nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.TryCreate((nuint)0xFFFFFFFF, out var r_nuint), Is.True, "nuint 0xFFFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt48)0xFFFFFFFF), "result nuint 0xFFFFFFFF");
      Assert.That(UInt48.TryCreate((nint)0x7FFFFFFF, out var r_nint), Is.True, "nint 0x7FFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt48)0x7FFFFFFF), "result nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.TryCreate((Half)0xFFE0, out var r_half), Is.True, "Half 0xFFE0"); Assert.That(r_half, Is.EqualTo((UInt48)0xFFE0), "result Half 0xFFE0");
    Assert.That(UInt48.TryCreate((float)2.8147496E+014, out var r_float), Is.True, "float 2.8147496E+014"); Assert.That(r_float, Is.EqualTo((UInt48)0xFFFF_FF000000), "result float 2.8147496E+014");
    Assert.That(UInt48.TryCreate((double)0xFFFF_FFFFFFFF, out var r_double), Is.True, "double 0xFFFF_FFFFFFFF"); Assert.That(r_double, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.TryCreate((decimal)0xFFFF_FFFFFFFF, out var r_decimal), Is.True, "decimal 0xFFFF_FFFFFFFF"); Assert.That(r_decimal, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result decimal 0xFFFF_FFFFFFFF");
#pragma warning restore CS0618
  }

  [Test]
  public void INumberBase_TryConvertFromSaturating_UInt24()
  {
    Assert.That(UInt24.TryConvertFromSaturating((byte)0xFF, out var r_byte), Is.True, "byte 0xFF"); Assert.That(r_byte, Is.EqualTo((UInt24)0xFF), "result byte 0xFF");
    Assert.That(UInt24.TryConvertFromSaturating((sbyte)0x7F, out var r_sbyte), Is.True, "sbyte 0x7F"); Assert.That(r_sbyte, Is.EqualTo((UInt24)0x7F), "result sbyte 0x7F");
    Assert.That(UInt24.TryConvertFromSaturating((char)0xFFFF, out var r_char), Is.True, "char 0xFFFF"); Assert.That(r_char, Is.EqualTo((UInt24)0xFFFF), "result char 0xFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((ushort)0xFFFF, out var r_ushort), Is.True, "ushort 0xFFFF"); Assert.That(r_ushort, Is.EqualTo((UInt24)0xFFFF), "result ushort 0xFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((short)0x7FFF, out var r_short), Is.True, "short 0x7FFF"); Assert.That(r_short, Is.EqualTo((UInt24)0x7FFF), "result short 0x7FFF");
    Assert.That(UInt24.TryConvertFromSaturating((uint)0xFFFFFF, out var r_uint), Is.True, "uint 0xFFFFFF"); Assert.That(r_uint, Is.EqualTo((UInt24)0xFFFFFF), "result uint 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((int)0xFFFFFF, out var r_int), Is.True, "int 0xFFFFFF"); Assert.That(r_int, Is.EqualTo((UInt24)0xFFFFFF), "result int 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((ulong)0xFFFFFF, out var r_ulong), Is.True, "ulong 0xFFFFFF"); Assert.That(r_ulong, Is.EqualTo((UInt24)0xFFFFFF), "result ulong 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((long)0xFFFFFF, out var r_long), Is.True, "long 0xFFFFFF"); Assert.That(r_long, Is.EqualTo((UInt24)0xFFFFFF), "result long 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((nuint)0xFFFFFF, out var r_nuint), Is.True, "nuint 0xFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt24)0xFFFFFF), "result nuint 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((nint)0xFFFFFF, out var r_nint), Is.True, "nint 0xFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt24)0xFFFFFF), "result nint 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((Half)0xFFE0, out var r_half), Is.True, "Half 0xFFE0"); Assert.That(r_half, Is.EqualTo((UInt24)0xFFE0), "result Half 0xFFE0");
    Assert.That(UInt24.TryConvertFromSaturating((float)0xFFFFFF, out var r_float), Is.True, "float 0xFFFFFF"); Assert.That(r_float, Is.EqualTo((UInt24)0xFFFFFF), "result float 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((double)0xFFFFFF, out var r_double), Is.True, "double 0xFFFFFF"); Assert.That(r_double, Is.EqualTo((UInt24)0xFFFFFF), "result double 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((decimal)0xFFFFFF, out var r_decimal), Is.True, "decimal 0xFFFFFF"); Assert.That(r_decimal, Is.EqualTo((UInt24)0xFFFFFF), "result decimal 0xFFFFFF");
  }

  [Test]
  public void INumberBase_TryConvertFromSaturating_UInt48()
  {
    Assert.That(UInt48.TryConvertFromSaturating((byte)0xFF, out var r_byte), Is.True, "byte 0xFF"); Assert.That(r_byte, Is.EqualTo((UInt48)0xFF), "result byte 0xFF");
    Assert.That(UInt48.TryConvertFromSaturating((sbyte)0x7F, out var r_sbyte), Is.True, "sbyte 0x7F"); Assert.That(r_sbyte, Is.EqualTo((UInt48)0x7F), "result sbyte 0x7F");
    Assert.That(UInt48.TryConvertFromSaturating((char)0xFFFF, out var r_char), Is.True, "char 0xFFFF"); Assert.That(r_char, Is.EqualTo((UInt48)0xFFFF), "result char 0xFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((ushort)0xFFFF, out var r_ushort), Is.True, "ushort 0xFFFF"); Assert.That(r_ushort, Is.EqualTo((UInt48)0xFFFF), "result ushort 0xFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((short)0x7FFF, out var r_short), Is.True, "short 0x7FFF"); Assert.That(r_short, Is.EqualTo((UInt48)0x7FFF), "result short 0x7FFF");
    Assert.That(UInt48.TryConvertFromSaturating((uint)0xFFFFFFFF, out var r_uint), Is.True, "uint 0xFFFFFFFF"); Assert.That(r_uint, Is.EqualTo((UInt48)0xFFFFFFFF), "result uint 0xFFFFFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((int)0x7FFFFFFF, out var r_int), Is.True, "int 0x7FFFFFFF"); Assert.That(r_int, Is.EqualTo((UInt48)0x7FFFFFFF), "result int 0x7FFFFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((ulong)0xFFFF_FFFFFFFF, out var r_ulong), Is.True, "ulong 0xFFFF_FFFFFFFF"); Assert.That(r_ulong, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((long)0x7FFF_FFFFFFFF, out var r_long), Is.True, "long 0x7FFF_FFFFFFFF"); Assert.That(r_long, Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "result long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.TryConvertFromSaturating(unchecked((nuint)0xFFFF_FFFFFFFF), out var r_nuint), Is.True, "nuint 0xFFFF_FFFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.TryConvertFromSaturating(unchecked((nint)0x7FFF_FFFFFFFF), out var r_nint), Is.True, "nint 0x7FFF_FFFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "result nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.TryConvertFromSaturating((nuint)0xFFFFFFFF, out var r_nuint), Is.True, "nuint 0xFFFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt48)0xFFFFFFFF), "result nuint 0xFFFFFFFF");
      Assert.That(UInt48.TryConvertFromSaturating((nint)0x7FFFFFFF, out var r_nint), Is.True, "nint 0x7FFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt48)0x7FFFFFFF), "result nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.TryConvertFromSaturating((Half)0xFFE0, out var r_half), Is.True, "Half 0xFFE0"); Assert.That(r_half, Is.EqualTo((UInt48)0xFFE0), "result Half 0xFFE0");
    Assert.That(UInt48.TryConvertFromSaturating((float)2.8147496E+014, out var r_float), Is.True, "float 2.8147496E+014"); Assert.That(r_float, Is.EqualTo((UInt48)0xFFFF_FF000000), "result float 2.8147496E+014");
    Assert.That(UInt48.TryConvertFromSaturating((double)0xFFFF_FFFFFFFF, out var r_double), Is.True, "double 0xFFFF_FFFFFFFF"); Assert.That(r_double, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((decimal)0xFFFF_FFFFFFFF, out var r_decimal), Is.True, "decimal 0xFFFF_FFFFFFFF"); Assert.That(r_decimal, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result decimal 0xFFFF_FFFFFFFF");
  }

  [Test]
  public void INumberBase_Create_OverflowException_UInt24()
  {
#pragma warning disable CS0618
    Assert.Throws<OverflowException>(() => UInt24.Create((sbyte)-1), "sbyte -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((short)-1), "short -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((uint)0x1_000000), "uint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((int)-1), "int -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((int)0x1_000000), "int 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((ulong)0x1_000000), "ulong 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((long)-1), "long -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((long)0x1_000000), "long 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((nuint)0x1_000000), "nuint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((nint)(-1)), "nint -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((nint)0x1_000000), "nint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((Half)(-1)), "Half -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((float)-1), "float -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((float)0x1_000000), "float 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((double)-1), "double -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((double)0x1_000000), "double 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((decimal)-1), "decimal -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((decimal)0x1_000000), "decimal 0x1_000000");
#pragma warning restore CS0618
  }

  [Test]
  public void INumberBase_CreateTruncating_OverflowException_UInt24()
  {
    Assert.That(UInt24.CreateTruncating((sbyte)-1), Is.EqualTo(UInt24.MaxValue), "sbyte -1");
    Assert.That(UInt24.CreateTruncating((short)-1), Is.EqualTo(UInt24.MaxValue), "short -1");
    Assert.That(UInt24.CreateTruncating((uint)0x1_000000), Is.EqualTo(UInt24.Zero), "uint 0x1_000000");
    Assert.That(UInt24.CreateTruncating((int)-1), Is.EqualTo(UInt24.MaxValue), "int -1");
    Assert.That(UInt24.CreateTruncating((int)0x1_000000), Is.EqualTo(UInt24.Zero), "int 0x1_000000");
    Assert.That(UInt24.CreateTruncating((ulong)0x1_000000), Is.EqualTo(UInt24.Zero), "ulong 0x1_000000");
    Assert.That(UInt24.CreateTruncating((long)-1), Is.EqualTo(UInt24.MaxValue), "long -1");
    Assert.That(UInt24.CreateTruncating((long)0x1_000000), Is.EqualTo(UInt24.Zero), "long 0x1_000000");
    Assert.That(UInt24.CreateTruncating((nuint)0x1_000000), Is.EqualTo(UInt24.Zero), "nuint 0x1_000000");
    Assert.That(UInt24.CreateTruncating((nint)(-1)), Is.EqualTo(UInt24.MaxValue), "nint -1");
    Assert.That(UInt24.CreateTruncating((nint)0x1_000000), Is.EqualTo(UInt24.Zero), "nint 0x1_000000");
    Assert.That(UInt24.CreateTruncating((Half)(-1)), Is.EqualTo(UInt24.MaxValue), "Half -1");
    Assert.That(UInt24.CreateTruncating((float)-1), Is.EqualTo(UInt24.MaxValue), "float -1");
    Assert.That(UInt24.CreateTruncating((float)0x1_000000), Is.EqualTo(UInt24.Zero), "float 0x1_000000");
    Assert.That(UInt24.CreateTruncating((double)-1), Is.EqualTo(UInt24.MaxValue), "double -1");
    Assert.That(UInt24.CreateTruncating((double)0x1_000000), Is.EqualTo(UInt24.Zero), "double 0x1_000000");
    Assert.That(UInt24.CreateTruncating((decimal)-1), Is.EqualTo(UInt24.MaxValue), "decimal -1");
    Assert.That(UInt24.CreateTruncating((decimal)0x1_000000), Is.EqualTo(UInt24.Zero), "decimal 0x1_000000");
  }

  [Test]
  public void INumberBase_CreateSaturating_Overflow_UInt24()
  {
    Assert.That(UInt24.CreateSaturating((sbyte)-1), Is.EqualTo(UInt24.MinValue), "sbyte -1");
    Assert.That(UInt24.CreateSaturating((short)-1), Is.EqualTo(UInt24.MinValue), "short -1");
    Assert.That(UInt24.CreateSaturating((int)-1), Is.EqualTo(UInt24.MinValue), "int -1");
    Assert.That(UInt24.CreateSaturating((long)-1), Is.EqualTo(UInt24.MinValue), "long -1");
    Assert.That(UInt24.CreateSaturating((nint)(-1)), Is.EqualTo(UInt24.MinValue), "nint -1");
    Assert.That(UInt24.CreateSaturating((Half)(-1)), Is.EqualTo(UInt24.MinValue), "Half -1");
    Assert.That(UInt24.CreateSaturating((float)-1), Is.EqualTo(UInt24.MinValue), "float -1");
    Assert.That(UInt24.CreateSaturating((double)-1), Is.EqualTo(UInt24.MinValue), "double -1");
    Assert.That(UInt24.CreateSaturating((decimal)-1), Is.EqualTo(UInt24.MinValue), "decimal -1");

    Assert.That(UInt24.CreateSaturating((uint)0x1_000000), Is.EqualTo(UInt24.MaxValue), "uint 0x1_000000");
    Assert.That(UInt24.CreateSaturating((int)0x1_000000), Is.EqualTo(UInt24.MaxValue), "int 0x1_000000");
    Assert.That(UInt24.CreateSaturating((ulong)0x1_000000), Is.EqualTo(UInt24.MaxValue), "ulong 0x1_000000");
    Assert.That(UInt24.CreateSaturating((long)0x1_000000), Is.EqualTo(UInt24.MaxValue), "long 0x1_000000");
    Assert.That(UInt24.CreateSaturating((nuint)0x1_000000), Is.EqualTo(UInt24.MaxValue), "nuint 0x1_000000");
    Assert.That(UInt24.CreateSaturating((nint)0x1_000000), Is.EqualTo(UInt24.MaxValue), "nint 0x1_000000");
    Assert.That(UInt24.CreateSaturating((float)0x1_000000), Is.EqualTo(UInt24.MaxValue), "float 0x1_000000");
    Assert.That(UInt24.CreateSaturating((double)0x1_000000), Is.EqualTo(UInt24.MaxValue), "double 0x1_000000");
    Assert.That(UInt24.CreateSaturating((decimal)0x1_000000), Is.EqualTo(UInt24.MaxValue), "decimal 0x1_000000");
  }

  [Test]
  public void INumberBase_Create_OverflowException_UInt48()
  {
#pragma warning disable CS0618
    Assert.Throws<OverflowException>(() => UInt48.Create((sbyte)-1), "sbyte -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((short)-1), "short -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((int)-1), "int -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((ulong)0x1_000000_000000), "ulong 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.Create((long)-1), "long -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((long)0x1_000000_000000), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.Throws<OverflowException>(() => UInt48.Create(unchecked((nuint)0x1_000000_000000)), "nuint 0x1_000000_000000");
      Assert.Throws<OverflowException>(() => UInt48.Create(unchecked((nint)0x1_000000_000000)), "nint 0x1_000000_000000");
    }
    Assert.Throws<OverflowException>(() => UInt48.Create((Half)(-1)), "Half -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((float)-1), "float -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((float)0x1_000000_000000), "float 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.Create((double)-1), "double -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((double)0x1_000000_000000), "double 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.Create((decimal)-1), "decimal -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((decimal)0x1_000000_000000), "decimal 0x1_000000_000000");
#pragma warning restore CS0618
  }

  [Test]
  public void INumberBase_CreateTruncating_OverflowException_UInt48()
  {
    Assert.That(UInt48.CreateTruncating((sbyte)-1), Is.EqualTo(UInt48.MaxValue), "sbyte -1");
    Assert.That(UInt48.CreateTruncating((short)-1), Is.EqualTo(UInt48.MaxValue), "short -1");
    Assert.That(UInt48.CreateTruncating((int)-1), Is.EqualTo(UInt48.MaxValue), "int -1");
    Assert.That(UInt48.CreateTruncating((ulong)0x1_000000_000000), Is.EqualTo(UInt48.Zero), "ulong 0x1_000000_000000");
    Assert.That(UInt48.CreateTruncating((long)-1), Is.EqualTo(UInt48.MaxValue), "long -1");
    Assert.That(UInt48.CreateTruncating((long)0x1_000000_000000), Is.EqualTo(UInt48.Zero), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.CreateTruncating(unchecked((nuint)0x1_000000_000000)), Is.EqualTo(UInt48.Zero) , "nuint 0x1_000000_000000");
      Assert.That(UInt48.CreateTruncating(unchecked((nint)0x1_000000_000000)), Is.EqualTo(UInt48.Zero) , "nint 0x1_000000_000000");
    }
    Assert.That(UInt48.CreateTruncating((Half)(-1)), Is.EqualTo(UInt48.MaxValue), "Half -1");
    Assert.That(UInt48.CreateTruncating((float)-1), Is.EqualTo(UInt48.MaxValue), "float -1");
    Assert.That(UInt48.CreateTruncating((float)0x1_000000_000000), Is.EqualTo(UInt48.Zero), "float 0x1_000000_000000");
    Assert.That(UInt48.CreateTruncating((double)-1), Is.EqualTo(UInt48.MaxValue), "double -1");
    Assert.That(UInt48.CreateTruncating((double)0x1_000000_000000), Is.EqualTo(UInt48.Zero), "double 0x1_000000_000000");
    Assert.That(UInt48.CreateTruncating((decimal)-1), Is.EqualTo(UInt48.MaxValue), "decimal -1");
    Assert.That(UInt48.CreateTruncating((decimal)0x1_000000_000000), Is.EqualTo(UInt48.Zero), "decimal 0x1_000000_000000");
  }

  [Test]
  public void INumberBase_CreateSaturating_Overflow_UInt48()
  {
    Assert.That(UInt48.CreateSaturating((sbyte)-1), Is.EqualTo(UInt48.MinValue), "sbyte -1");
    Assert.That(UInt48.CreateSaturating((short)-1), Is.EqualTo(UInt48.MinValue), "short -1");
    Assert.That(UInt48.CreateSaturating((int)-1), Is.EqualTo(UInt48.MinValue), "int -1");
    Assert.That(UInt48.CreateSaturating((long)-1), Is.EqualTo(UInt48.MinValue), "long -1");
    Assert.That(UInt48.CreateSaturating((Half)(-1)), Is.EqualTo(UInt48.MinValue), "Half -1");
    Assert.That(UInt48.CreateSaturating((float)-1), Is.EqualTo(UInt48.MinValue), "float -1");
    Assert.That(UInt48.CreateSaturating((double)-1), Is.EqualTo(UInt48.MinValue), "double -1");
    Assert.That(UInt48.CreateSaturating((decimal)-1), Is.EqualTo(UInt48.MinValue), "decimal -1");

    Assert.That(UInt48.CreateSaturating((ulong)0x1_000000_000000), Is.EqualTo(UInt48.MaxValue), "ulong 0x1_000000_000000");
    Assert.That(UInt48.CreateSaturating((long)0x1_000000_000000), Is.EqualTo(UInt48.MaxValue), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.CreateSaturating(unchecked((nuint)0x1_000000_000000)), Is.EqualTo(UInt48.MaxValue), "nuint 0x1_000000_000000");
      Assert.That(UInt48.CreateSaturating(unchecked((nint)0x1_000000_000000)), Is.EqualTo(UInt48.MaxValue), "nint 0x1_000000_000000");
    }
    Assert.That(UInt48.CreateSaturating((float)2.81474977E+014), Is.EqualTo(UInt48.MaxValue), "float 2.81474977E+014");
    Assert.That(UInt48.CreateSaturating((double)0x1_000000_000000), Is.EqualTo(UInt48.MaxValue), "double 0x1_000000_000000");
    Assert.That(UInt48.CreateSaturating((decimal)0x1_000000_000000), Is.EqualTo(UInt48.MaxValue), "decimal 0x1_000000_000000");
  }

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
  public void INumberBase_TryConvertFromSaturating_Overflow_UInt24()
  {
    Assert.That(UInt24.TryConvertFromSaturating((sbyte)-1, out var r_sbyte), Is.True, "sbyte -1"); Assert.That(r_sbyte, Is.EqualTo(UInt24.MinValue), "sbyte -1");
    Assert.That(UInt24.TryConvertFromSaturating((short)-1, out var r_short), Is.True, "short -1"); Assert.That(r_short, Is.EqualTo(UInt24.MinValue), "short -1");
    Assert.That(UInt24.TryConvertFromSaturating((int)-1, out var r_int_underflow), Is.True, "int -1"); Assert.That(r_int_underflow, Is.EqualTo(UInt24.MinValue), "int -1");
    Assert.That(UInt24.TryConvertFromSaturating((long)-1, out var r_long_underflow), Is.True, "long -1"); Assert.That(r_long_underflow, Is.EqualTo(UInt24.MinValue), "long -1");
    Assert.That(UInt24.TryConvertFromSaturating((nint)(-1), out var r_nint_underflow), Is.True, "nint -1"); Assert.That(r_nint_underflow, Is.EqualTo(UInt24.MinValue), "nint -1");
    Assert.That(UInt24.TryConvertFromSaturating((Half)(-1), out var r_half), Is.True, "Half -1"); Assert.That(r_half, Is.EqualTo(UInt24.MinValue), "Half -1");
    Assert.That(UInt24.TryConvertFromSaturating((float)-1, out var r_float_underflow), Is.True, "float -1"); Assert.That(r_float_underflow, Is.EqualTo(UInt24.MinValue), "float -1");
    Assert.That(UInt24.TryConvertFromSaturating((double)-1, out var r_double_underflow), Is.True, "double -1"); Assert.That(r_double_underflow, Is.EqualTo(UInt24.MinValue), "double -1");
    Assert.That(UInt24.TryConvertFromSaturating((decimal)-1, out var r_decimal_underflow), Is.True, "decimal -1"); Assert.That(r_decimal_underflow, Is.EqualTo(UInt24.MinValue), "decimal -1");

    Assert.That(UInt24.TryConvertFromSaturating((uint)0x1_000000, out var r_uint), Is.True, "uint 0x1_000000"); Assert.That(r_uint, Is.EqualTo(UInt24.MaxValue), "uint 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((int)0x1_000000, out var r_int_overflow), Is.True, "int 0x1_000000"); Assert.That(r_int_overflow, Is.EqualTo(UInt24.MaxValue), "int 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((ulong)0x1_000000, out var r_ulong), Is.True, "ulong 0x1_000000"); Assert.That(r_ulong, Is.EqualTo(UInt24.MaxValue), "ulong 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((long)0x1_000000, out var r_long_overflow), Is.True, "long 0x1_000000"); Assert.That(r_long_overflow, Is.EqualTo(UInt24.MaxValue), "long 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((nuint)0x1_000000, out var r_nuint), Is.True, "nuint 0x1_000000"); Assert.That(r_nuint, Is.EqualTo(UInt24.MaxValue), "nuint 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((nint)0x1_000000, out var r_nint_overflow), Is.True, "nint 0x1_000000"); Assert.That(r_nint_overflow, Is.EqualTo(UInt24.MaxValue), "nint 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((float)0x1_000000, out var r_float_overflow), Is.True, "float 0x1_000000"); Assert.That(r_float_overflow, Is.EqualTo(UInt24.MaxValue), "float 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((double)0x1_000000, out var r_double_overflow), Is.True, "double 0x1_000000"); Assert.That(r_double_overflow, Is.EqualTo(UInt24.MaxValue), "double 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((decimal)0x1_000000, out var r_decimal_overflow), Is.True, "decimal 0x1_000000"); Assert.That(r_decimal_overflow, Is.EqualTo(UInt24.MaxValue), "decimal 0x1_000000");
  }

  [Test]
  public void INumberBase_TryConvertFromSaturating_Overflow_UInt48()
  {
    Assert.That(UInt48.TryConvertFromSaturating((sbyte)-1, out var r_sbyte), Is.True, "sbyte -1"); Assert.That(r_sbyte, Is.EqualTo(UInt48.MinValue), "sbyte -1");
    Assert.That(UInt48.TryConvertFromSaturating((short)-1, out var r_short), Is.True, "short -1"); Assert.That(r_short, Is.EqualTo(UInt48.MinValue), "short -1");
    Assert.That(UInt48.TryConvertFromSaturating((int)-1, out var r_int), Is.True, "int -1"); Assert.That(r_int, Is.EqualTo(UInt48.MinValue), "int -1");
    Assert.That(UInt48.TryConvertFromSaturating((long)-1, out var r_long_underflow), Is.True, "long -1"); Assert.That(r_long_underflow, Is.EqualTo(UInt48.MinValue), "long -1");
    Assert.That(UInt48.TryConvertFromSaturating((Half)(-1), out var r_half), Is.True, "Half -1"); Assert.That(r_half, Is.EqualTo(UInt48.MinValue), "Half -1");
    Assert.That(UInt48.TryConvertFromSaturating((float)-1, out var r_float_underflow), Is.True, "float -1"); Assert.That(r_float_underflow, Is.EqualTo(UInt48.MinValue), "float -1");
    Assert.That(UInt48.TryConvertFromSaturating((double)-1, out var r_double_underflow), Is.True, "double -1"); Assert.That(r_double_underflow, Is.EqualTo(UInt48.MinValue), "double -1");
    Assert.That(UInt48.TryConvertFromSaturating((decimal)-1, out var r_decimal_underflow), Is.True, "decimal -1"); Assert.That(r_decimal_underflow, Is.EqualTo(UInt48.MinValue), "decimal -1");
    Assert.That(UInt48.TryConvertFromSaturating((ulong)0x1_000000_000000, out var r_ulong), Is.True, "ulong 0x1_000000_000000"); Assert.That(r_ulong, Is.EqualTo(UInt48.MaxValue), "ulong 0x1_000000_000000");
    Assert.That(UInt48.TryConvertFromSaturating((long)0x1_000000_000000, out var r_long_overflow), Is.True, "long 0x1_000000_000000"); Assert.That(r_long_overflow, Is.EqualTo(UInt48.MaxValue), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.TryConvertFromSaturating(unchecked((nuint)0x1_000000_000000), out var r_nuint), Is.True, "nuint 0x1_000000_000000"); Assert.That(r_nuint, Is.EqualTo(UInt48.MaxValue), "nuint 0x1_000000_000000");
      Assert.That(UInt48.TryConvertFromSaturating(unchecked((nint)0x1_000000_000000), out var r_nint), Is.True, "nint 0x1_000000_000000"); Assert.That(r_nint, Is.EqualTo(UInt48.MaxValue), "nint 0x1_000000_000000");
    }
    Assert.That(UInt48.TryConvertFromSaturating((float)2.81474977E+014, out var r_float_overflow), Is.True, "float 2.81474977E+014"); Assert.That(r_float_overflow, Is.EqualTo(UInt48.MaxValue), "float 2.81474977E+014");
    Assert.That(UInt48.TryConvertFromSaturating((double)0x1_000000_000000, out var r_double_overflow), Is.True, "double 0x1_000000_000000"); Assert.That(r_double_overflow, Is.EqualTo(UInt48.MaxValue), "double 0x1_000000_000000");
    Assert.That(UInt48.TryConvertFromSaturating((decimal)0x1_000000_000000, out var r_decimal_overflow), Is.True, "decimal 0x1_000000_000000"); Assert.That(r_decimal_overflow, Is.EqualTo(UInt48.MaxValue), "decimal 0x1_000000_000000");
  }

  [Test]
  public void INumberBase_Create_TypeNotSupportedException_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.Create(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.Create(Complex.Zero), "Complex");
  }

  [Test]
  public void INumberBase_CreateTruncating_TypeNotSupportedException_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateTruncating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateTruncating(Complex.Zero), "Complex");
  }

  [Test]
  public void INumberBase_CreateSaturating_TypeNotSupportedException_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateSaturating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateSaturating(Complex.Zero), "Complex");
  }

  [Test]
  public void INumberBase_Create_TypeNotSupportedException_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.Create(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.Create(Complex.Zero), "Complex");
  }

  [Test]
  public void INumberBase_CreateTruncating_TypeNotSupportedException_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateTruncating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateTruncating(Complex.Zero), "Complex");
  }

  [Test]
  public void INumberBase_CreateSaturating_TypeNotSupportedException_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateSaturating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateSaturating(Complex.Zero), "Complex");
  }

  [Test]
  public void TryCreate_TypeNotSupported_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.TryCreate(BigInteger.Zero, out _), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.TryCreate(Complex.Zero, out _), "Complex");
  }

  [Test]
  public void TryCreate_TypeNotSupported_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.TryCreate(BigInteger.Zero, out _), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.TryCreate(Complex.Zero, out _), "Complex");
  }

  [Test]
  public void INumberBase_TryConvertFromSaturating_TypeNotSupported_UInt24()
  {
    Assert.That(UInt24.TryConvertFromSaturating(BigInteger.Zero, out _), Is.False, "BigInteger");
    Assert.That(UInt24.TryConvertFromSaturating(Complex.Zero, out _), Is.False, "Complex");
  }

  [Test]
  public void INumberBase_TryConvertFromSaturating_TypeNotSupported_UInt48()
  {
    Assert.That(UInt48.TryConvertFromSaturating(BigInteger.Zero, out _), Is.False, "BigInteger");
    Assert.That(UInt48.TryConvertFromSaturating(Complex.Zero, out _), Is.False, "Complex");
  }
#endif
}
