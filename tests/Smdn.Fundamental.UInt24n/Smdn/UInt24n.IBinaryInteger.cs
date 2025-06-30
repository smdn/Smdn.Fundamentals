// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IBINARYINTEGER
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,   0, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,   1, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  11, 0b_0000_0000_0000__1100_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  12, 0b_0000_0000_0001__1000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  13, 0b_0000_0000_0011__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  23, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  24, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  25, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  -1, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -23, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -24, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -25, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,  -1, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   0, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   1, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,  25, 0b_1111_1111_1111__1111_1111_1111u)]
  public void RotateLeft(uint value, int rotateAmount, uint expected)
    => Assert.That(UInt24.RotateLeft((UInt24)value, rotateAmount), Is.EqualTo((UInt24)expected), $"RotateLeft({((UInt24)value).ToBinaryString()}, {rotateAmount})");

  [TestCase(0b_1000_0000_0000__0000_0000_0001u,   0, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,   1, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  11, 0b_0000_0000_0011__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  12, 0b_0000_0000_0001__1000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  13, 0b_0000_0000_0000__1100_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  23, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  24, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  25, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  -1, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -23, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -24, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -25, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,  -1, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   0, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   1, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,  25, 0b_1111_1111_1111__1111_1111_1111u)]
  public void RotateRight(uint value, int rotateAmount, uint expected)
    => Assert.That(UInt24.RotateRight((UInt24)value, rotateAmount), Is.EqualTo((UInt24)expected), $"RotateRight({((UInt24)value).ToBinaryString()}, {rotateAmount})");
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   0, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   1, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  11, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__1100_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  12, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0001__1000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  13, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0011__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  23, 0b_0000_0000_0000__0000_0000_0000__1100_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  24, 0b_0000_0000_0000__0000_0000_0001__1000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  25, 0b_0000_0000_0000__0000_0000_0011__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  35, 0b_0000_0000_0000__1100_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  36, 0b_0000_0000_0001__1000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  37, 0b_0000_0000_0011__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  47, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  48, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  49, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  -1, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -47, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -48, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -49, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,  -1, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   0, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   1, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,  49, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  public void RotateLeft(ulong value, int rotateAmount, ulong expected)
    => Assert.That(UInt48.RotateLeft((UInt48)value, rotateAmount), Is.EqualTo((UInt48)expected), $"RotateLeft({((UInt48)value).ToBinaryString()}, {rotateAmount})");

  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   0, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   1, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  11, 0b_0000_0000_0011__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  12, 0b_0000_0000_0001__1000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  13, 0b_0000_0000_0000__1100_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  23, 0b_0000_0000_0000__0000_0000_0011__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  24, 0b_0000_0000_0000__0000_0000_0001__1000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  25, 0b_0000_0000_0000__0000_0000_0000__1100_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  35, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0011__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  36, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0001__1000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  37, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__1100_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  47, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  48, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  49, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  -1, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -47, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -48, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -49, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,  -1, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   0, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   1, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,  49, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  public void RotateRight(ulong value, int rotateAmount, ulong expected)
    => Assert.That(UInt48.RotateRight((UInt48)value, rotateAmount), Is.EqualTo((UInt48)expected), $"RotateRight({((UInt48)value).ToBinaryString()}, {rotateAmount})");
}

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040

  [Test]
  public void DivRem()
  {
    Assert.Throws<DivideByZeroException>(() => DivRem(UInt24.Zero, UInt24.Zero), $"{typeof(UInt24).Name} DivRem(Zero, Zero)");
    Assert.Throws<DivideByZeroException>(() => DivRem(UInt24.One, UInt24.Zero), $"{typeof(UInt24).Name} DivRem(One, Zero)");

    Assert.That(DivRem(UInt24.Zero, UInt24.One), Is.EqualTo((UInt24.Zero, UInt24.Zero)), $"{typeof(UInt24).Name} DivRem(Zero, One)");
    Assert.That(DivRem(UInt24.One, UInt24.One), Is.EqualTo((UInt24.One, UInt24.Zero)), $"{typeof(UInt24).Name} DivRem(One, One)");
    Assert.That(DivRem(UInt24.One, UInt24.MaxValue), Is.EqualTo((UInt24.Zero, UInt24.One)), $"{typeof(UInt24).Name} DivRem(One, MaxValue)");
    Assert.That(DivRem(UInt24.MaxValue, UInt24.One), Is.EqualTo((UInt24.MaxValue, UInt24.Zero)), $"{typeof(UInt24).Name} DivRem(MaxValue, One)");
    Assert.That(DivRem(UInt24.MaxValue, UInt24.MaxValue), Is.EqualTo((UInt24.One, UInt24.Zero)), $"{typeof(UInt24).Name} DivRem(MaxValue, MaxValue)");

#if SYSTEM_NUMERICS_IBINARYINTEGER
    static (TUInt24n Quotient, TUInt24n Remainder) DivRem<TUInt24n>(TUInt24n left, TUInt24n right) where TUInt24n : IBinaryInteger<TUInt24n>
      => TUInt24n.DivRem(left, right);
#else
    static (UInt24 Quotient, UInt24 Remainder) DivRem(UInt24 left, UInt24 right)
      => UInt24.DivRem(left, right);
#endif
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test]
  public void DivRem()
  {
    Assert.Throws<DivideByZeroException>(() => DivRem(UInt48.Zero, UInt48.Zero), $"{typeof(UInt48).Name} DivRem(Zero, Zero)");
    Assert.Throws<DivideByZeroException>(() => DivRem(UInt48.One, UInt48.Zero), $"{typeof(UInt48).Name} DivRem(One, Zero)");

    Assert.That(DivRem(UInt48.Zero, UInt48.One), Is.EqualTo((UInt48.Zero, UInt48.Zero)), $"{typeof(UInt48).Name} DivRem(Zero, One)");
    Assert.That(DivRem(UInt48.One, UInt48.One), Is.EqualTo((UInt48.One, UInt48.Zero)), $"{typeof(UInt48).Name} DivRem(One, One)");
    Assert.That(DivRem(UInt48.One, UInt48.MaxValue), Is.EqualTo((UInt48.Zero, UInt48.One)), $"{typeof(UInt48).Name} DivRem(One, MaxValue)");
    Assert.That(DivRem(UInt48.MaxValue, UInt48.One), Is.EqualTo((UInt48.MaxValue, UInt48.Zero)), $"{typeof(UInt48).Name} DivRem(MaxValue, One)");
    Assert.That(DivRem(UInt48.MaxValue, UInt48.MaxValue), Is.EqualTo((UInt48.One, UInt48.Zero)), $"{typeof(UInt48).Name} DivRem(MaxValue, MaxValue)");

#if SYSTEM_NUMERICS_IBINARYINTEGER
    static (TUInt24n Quotient, TUInt24n Remainder) DivRem<TUInt24n>(TUInt24n left, TUInt24n right) where TUInt24n : IBinaryInteger<TUInt24n>
      => TUInt24n.DivRem(left, right);
#else
    static (UInt48 Quotient, UInt48 Remainder) DivRem(UInt48 left, UInt48 right)
      => UInt48.DivRem(left, right);
#endif
  }
}

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040

  private static System.Collections.IEnumerable YieldTestCases_TryRead()
  {
    const bool asUnsigned = true;
    const bool asSigned = false;

    yield return new object?[] { new byte[] { 0x00, 0x00, 0x00 }, asUnsigned, (UInt24)0x000000u, (UInt24)0x000000u };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0x00 }, asSigned,   (UInt24)0x000000u, (UInt24)0x000000u };
    yield return new object?[] { new byte[] { 0xFF, 0x00, 0x00 }, asUnsigned, (UInt24)0xFF0000u, (UInt24)0x0000FFu };
    yield return new object?[] { new byte[] { 0xFF, 0x00, 0x00 }, asSigned,   (UInt24)0xFF0000u, (UInt24)0x0000FFu };
    yield return new object?[] { new byte[] { 0x00, 0xFF, 0x00 }, asUnsigned, (UInt24)0x00FF00u, (UInt24)0x00FF00u };
    yield return new object?[] { new byte[] { 0x00, 0xFF, 0x00 }, asSigned,   (UInt24)0x00FF00u, (UInt24)0x00FF00u };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0xFF }, asUnsigned, (UInt24)0x0000FFu, (UInt24)0xFF0000u };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0xFF }, asSigned,   (UInt24)0x0000FFu, (UInt24)0xFF0000u };
    yield return new object?[] { new byte[] { 0x01, 0x23, 0x45 }, asUnsigned, (UInt24)0x012345u, (UInt24)0x452301u };
  }

  [TestCaseSource(nameof(YieldTestCases_TryRead))]
  public void TryReadBigEndian(byte[] source, bool isUnsigned, UInt24 expectedResultBigEndian, UInt24 _)
  {
    Assert.That(UInt24.TryReadBigEndian(source, isUnsigned, out var result), Is.True);
    Assert.That(result, Is.EqualTo(expectedResultBigEndian));
  }

  [TestCaseSource(nameof(YieldTestCases_TryRead))]
  public void TryReadLittleEndian(byte[] source, bool isUnsigned, UInt24 _, UInt24 expectedResultLittleEndian)
  {
    Assert.That(UInt24.TryReadLittleEndian(source, isUnsigned, out var result), Is.True);
    Assert.That(result, Is.EqualTo(expectedResultLittleEndian));
  }

  [Test]
  public void TryReadBigEndian_SourceTooShort([Values] bool isUnsigned, [Values(0, 1, 2)] int length)
  {
    Assert.That(UInt24.TryReadBigEndian(new byte[length], isUnsigned, out _), Is.False);
  }

  [Test]
  public void TryReadLittleEndian_SourceTooShort([Values] bool isUnsigned, [Values(0, 1, 2)] int length)
  {
    Assert.That(UInt24.TryReadLittleEndian(new byte[length], isUnsigned, out _), Is.False);
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  private static System.Collections.IEnumerable YieldTestCases_TryRead()
  {
    const bool asUnsigned = true;
    const bool asSigned = false;

    yield return new object?[] { new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, asUnsigned, (UInt48)0x000000_000000uL, (UInt48)0x000000_000000uL };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, asSigned,   (UInt48)0x000000_000000uL, (UInt48)0x000000_000000uL };
    yield return new object?[] { new byte[] { 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00 }, asUnsigned, (UInt48)0xFF0000_000000uL, (UInt48)0x000000_0000FFuL };
    yield return new object?[] { new byte[] { 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00 }, asSigned,   (UInt48)0xFF0000_000000uL, (UInt48)0x000000_0000FFuL };
    yield return new object?[] { new byte[] { 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00 }, asUnsigned, (UInt48)0x00FF00_000000uL, (UInt48)0x000000_00FF00uL };
    yield return new object?[] { new byte[] { 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00 }, asSigned,   (UInt48)0x00FF00_000000uL, (UInt48)0x000000_00FF00uL };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00 }, asUnsigned, (UInt48)0x0000FF_000000uL, (UInt48)0x000000_FF0000uL };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00 }, asSigned,   (UInt48)0x0000FF_000000uL, (UInt48)0x000000_FF0000uL };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00 }, asUnsigned, (UInt48)0x000000_FF0000uL, (UInt48)0x0000FF_000000uL };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00 }, asSigned,   (UInt48)0x000000_FF0000uL, (UInt48)0x0000FF_000000uL };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00 }, asUnsigned, (UInt48)0x000000_00FF00uL, (UInt48)0x00FF00_000000uL };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00 }, asSigned,   (UInt48)0x000000_00FF00uL, (UInt48)0x00FF00_000000uL };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF }, asUnsigned, (UInt48)0x000000_0000FFuL, (UInt48)0xFF0000_000000uL };
    yield return new object?[] { new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF }, asSigned,   (UInt48)0x000000_0000FFuL, (UInt48)0xFF0000_000000uL };
    yield return new object?[] { new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB }, asUnsigned, (UInt48)0x012345_6789ABuL, (UInt48)0xAB8967_452301uL };
  }

  [TestCaseSource(nameof(YieldTestCases_TryRead))]
  public void TryReadBigEndian(byte[] source, bool isUnsigned, UInt48 expectedResultBigEndian, UInt48 _)
  {
    Assert.That(UInt48.TryReadBigEndian(source, isUnsigned, out var result), Is.True);
    Assert.That(result, Is.EqualTo(expectedResultBigEndian));
  }

  [TestCaseSource(nameof(YieldTestCases_TryRead))]
  public void TryReadLittleEndian(byte[] source, bool isUnsigned, UInt48 _, UInt48 expectedResultLittleEndian)
  {
    Assert.That(UInt48.TryReadLittleEndian(source, isUnsigned, out var result), Is.True);
    Assert.That(result, Is.EqualTo(expectedResultLittleEndian));
  }

  [Test]
  public void TryReadBigEndian_SourceTooShort([Values] bool isUnsigned, [Values(0, 1, 2, 3, 4, 5)] int length)
  {
    Assert.That(UInt48.TryReadBigEndian(new byte[length], isUnsigned, out _), Is.False);
  }

  [Test]
  public void TryReadLittleEndian_SourceTooShort([Values] bool isUnsigned, [Values(0, 1, 2, 3, 4, 5)] int length)
  {
    Assert.That(UInt48.TryReadLittleEndian(new byte[length], isUnsigned, out _), Is.False);
  }
}

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  private static System.Collections.IEnumerable YieldTestCases_TryWrite()
  {
    yield return new object?[] { (UInt24)0x000000u, new byte[] { 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00, 0x00 } };
    yield return new object?[] { (UInt24)0xFF0000u, new byte[] { 0xFF, 0x00, 0x00 }, new byte[] { 0x00, 0x00, 0xFF } };
    yield return new object?[] { (UInt24)0x00FF00u, new byte[] { 0x00, 0xFF, 0x00 }, new byte[] { 0x00, 0xFF, 0x00 } };
    yield return new object?[] { (UInt24)0x0000FFu, new byte[] { 0x00, 0x00, 0xFF }, new byte[] { 0xFF, 0x00, 0x00 } };
    yield return new object?[] { (UInt24)0x012345u, new byte[] { 0x01, 0x23, 0x45 }, new byte[] { 0x45, 0x23, 0x01 } };
  }

  [TestCaseSource(nameof(YieldTestCases_TryWrite))]
  public void TryWriteBigEndian(UInt24 value, byte[] expectedResultBigEndian, byte[] _)
  {
    var destination = new byte[3];

    Assert.That(value.TryWriteBigEndian(destination, out var bytesWritten), Is.True);
    Assert.That(bytesWritten, Is.EqualTo(3));
    Assert.That(destination, Is.EqualTo(expectedResultBigEndian).AsCollection);
  }

  [TestCaseSource(nameof(YieldTestCases_TryWrite))]
  public void TryWriteLittleEndian(UInt24 value, byte[] _, byte[] expectedResultLittleEndian)
  {
    var destination = new byte[3];

    Assert.That(value.TryWriteLittleEndian(destination, out var bytesWritten), Is.True);
    Assert.That(bytesWritten, Is.EqualTo(3));
    Assert.That(destination, Is.EqualTo(expectedResultLittleEndian).AsCollection);
  }

  [Test]
  public void TryWriteBigEndian_SourceTooShort([Values(0, 1, 2)] int length)
  {
    Assert.That(UInt24.One.TryWriteBigEndian(new byte[length], out var bytesWritten), Is.False);
    Assert.That(bytesWritten, Is.Zero);
  }

  [Test]
  public void TryWriteLittleEndian_SourceTooShort([Values(0, 1, 2)] int length)
  {
    Assert.That(UInt24.One.TryWriteLittleEndian(new byte[length], out var bytesWritten), Is.False);
    Assert.That(bytesWritten, Is.Zero);
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  private static System.Collections.IEnumerable YieldTestCases_TryWrite()
  {
    yield return new object?[] { (UInt48)0x000000_000000uL, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } };
    yield return new object?[] { (UInt48)0xFF0000_000000uL, new byte[] { 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF } };
    yield return new object?[] { (UInt48)0x00FF00_000000uL, new byte[] { 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00 } };
    yield return new object?[] { (UInt48)0x0000FF_000000uL, new byte[] { 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00 }, new byte[] { 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00 } };
    yield return new object?[] { (UInt48)0x000000_FF0000uL, new byte[] { 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00 }, new byte[] { 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00 } };
    yield return new object?[] { (UInt48)0x000000_00FF00uL, new byte[] { 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00 }, new byte[] { 0x00, 0xFF, 0x00, 0x00, 0x00, 0x00 } };
    yield return new object?[] { (UInt48)0x000000_0000FFuL, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF }, new byte[] { 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00 } };
    yield return new object?[] { (UInt48)0x012345_6789ABuL, new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB }, new byte[] { 0xAB, 0x89, 0x67, 0x45, 0x23, 0x01 } };
  }

  [TestCaseSource(nameof(YieldTestCases_TryWrite))]
  public void TryWriteBigEndian(UInt48 value, byte[] expectedResultBigEndian, byte[] _)
  {
    var destination = new byte[6];

    Assert.That(value.TryWriteBigEndian(destination, out var bytesWritten), Is.True);
    Assert.That(bytesWritten, Is.EqualTo(6));
    Assert.That(destination, Is.EqualTo(expectedResultBigEndian).AsCollection);
  }

  [TestCaseSource(nameof(YieldTestCases_TryWrite))]
  public void TryWriteLittleEndian(UInt48 value, byte[] _, byte[] expectedResultLittleEndian)
  {
    var destination = new byte[6];

    Assert.That(value.TryWriteLittleEndian(destination, out var bytesWritten), Is.True);
    Assert.That(bytesWritten, Is.EqualTo(6));
    Assert.That(destination, Is.EqualTo(expectedResultLittleEndian).AsCollection);
  }

  [Test]
  public void TryWriteBigEndian_SourceTooShort([Values(0, 1, 2, 3, 4, 5)] int length)
  {
    Assert.That(UInt48.One.TryWriteBigEndian(new byte[length], out var bytesWritten), Is.False);
    Assert.That(bytesWritten, Is.Zero);
  }

  [Test]
  public void TryWriteLittleEndian_SourceTooShort([Values(0, 1, 2, 3, 4, 5)] int length)
  {
    Assert.That(UInt48.One.TryWriteLittleEndian(new byte[length], out var bytesWritten), Is.False);
    Assert.That(bytesWritten, Is.Zero);
  }
}

#if SYSTEM_NUMERICS_IBINARYINTEGER
#pragma warning disable IDE0040
partial class UInt24nTests {
#pragma warning restore IDE0040
  static int IBinaryInteger_GetByteCount<TUInt24n>(TUInt24n value) where TUInt24n : IBinaryInteger<TUInt24n>
    => value.GetByteCount();

  [TestCase(0x000000u)]
  [TestCase(0x000001u)]
  [TestCase(0x100000u)]
  [TestCase(0xFFFFFFu)]
  public void IBinaryInteger_GetByteCount_OfUInt24(uint value)
    => Assert.That(IBinaryInteger_GetByteCount((UInt24)value), Is.EqualTo(3), $"GetByteCount({((UInt24)value).ToBinaryString()})");

  [TestCase(0x000000_000000u)]
  [TestCase(0x000000_000001u)]
  [TestCase(0x100000_000000u)]
  [TestCase(0xFFFFFF_FFFFFFu)]
  public void IBinaryInteger_GetByteCount_OfUInt48(ulong value)
    => Assert.That(IBinaryInteger_GetByteCount((UInt48)value), Is.EqualTo(6), $"GetByteCount({((UInt48)value).ToBinaryString()})");

  static int IBinaryInteger_GetShortestBitLength<TUInt24n>(TUInt24n value) where TUInt24n : IBinaryInteger<TUInt24n>
    => value.GetShortestBitLength();

  [TestCase(0b_0000_0000_0000__0000_0000_0000u, 0)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001u, 1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0010u, 2)]
  [TestCase(0b_0000_0000_0000__0000_0000_0011u, 2)]
  [TestCase(0b_0100_0000_0000__0000_0000_0000u, 23)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000u, 24)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u, 24)]
  public void IBinaryInteger_GetShortestBitLength_OfUInt24(uint value, int expected)
    => Assert.That(IBinaryInteger_GetShortestBitLength((UInt24)value), Is.EqualTo(expected), $"GetShortestBitLength({((UInt24)value).ToBinaryString()})");

  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL, 0)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, 1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0010uL, 2)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL, 2)]
  [TestCase(0b_0100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL, 47)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL, 48)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL, 48)]
  public void IBinaryInteger_GetShortestBitLength_OfUInt48(ulong value, int expected)
    => Assert.That(IBinaryInteger_GetShortestBitLength((UInt48)value), Is.EqualTo(expected), $"GetShortestBitLength({((UInt48)value).ToBinaryString()})");
}
#endif

#if SYSTEM_NUMERICS_IBINARYINTEGER
#pragma warning disable IDE0040
partial class UInt24nTests {
#pragma warning restore IDE0040
  static TUInt24n IBinaryInteger_RotateLeft<TUInt24n>(TUInt24n value, int rotateAmount) where TUInt24n : IBinaryInteger<TUInt24n>
    => TUInt24n.RotateLeft(value, rotateAmount);

  [TestCase(0b_1000_0000_0000__0000_0000_0001u,   0, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,   1, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  11, 0b_0000_0000_0000__1100_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  12, 0b_0000_0000_0001__1000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  13, 0b_0000_0000_0011__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  23, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  24, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  25, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  -1, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -23, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -24, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -25, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,  -1, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   0, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   1, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,  25, 0b_1111_1111_1111__1111_1111_1111u)]
  public void IBinaryInteger_RotateLeft_OfUInt24(uint value, int rotateAmount, uint expected)
    => Assert.That(IBinaryInteger_RotateLeft((UInt24)value, rotateAmount), Is.EqualTo((UInt24)expected), $"RotateLeft({((UInt24)value).ToBinaryString()}, {rotateAmount})");

  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   0, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   1, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  11, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__1100_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  12, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0001__1000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  13, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0011__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  23, 0b_0000_0000_0000__0000_0000_0000__1100_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  24, 0b_0000_0000_0000__0000_0000_0001__1000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  25, 0b_0000_0000_0000__0000_0000_0011__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  35, 0b_0000_0000_0000__1100_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  36, 0b_0000_0000_0001__1000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  37, 0b_0000_0000_0011__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  47, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  48, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  49, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  -1, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -47, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -48, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -49, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,  -1, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   0, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   1, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,  49, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  public void IBinaryInteger_RotateLeft_OfUInt48(ulong value, int rotateAmount, ulong expected)
    => Assert.That(IBinaryInteger_RotateLeft((UInt48)value, rotateAmount), Is.EqualTo((UInt48)expected), $"RotateLeft({((UInt48)value).ToBinaryString()}, {rotateAmount})");

  static TUInt24n IBinaryInteger_RotateRight<TUInt24n>(TUInt24n value, int rotateAmount) where TUInt24n : IBinaryInteger<TUInt24n>
    => TUInt24n.RotateRight(value, rotateAmount);

  [TestCase(0b_1000_0000_0000__0000_0000_0001u,   0, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,   1, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  11, 0b_0000_0000_0011__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  12, 0b_0000_0000_0001__1000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  13, 0b_0000_0000_0000__1100_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  23, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  24, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  25, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u,  -1, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -23, 0b_1100_0000_0000__0000_0000_0000u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -24, 0b_1000_0000_0000__0000_0000_0001u)]
  [TestCase(0b_1000_0000_0000__0000_0000_0001u, -25, 0b_0000_0000_0000__0000_0000_0011u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,  -1, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   0, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   1, 0b_1111_1111_1111__1111_1111_1111u)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,  25, 0b_1111_1111_1111__1111_1111_1111u)]
  public void IBinaryInteger_RotateRight_OfUInt24(uint value, int rotateAmount, uint expected)
    => Assert.That(IBinaryInteger_RotateRight((UInt24)value, rotateAmount), Is.EqualTo((UInt24)expected), $"RotateRight({((UInt24)value).ToBinaryString()}, {rotateAmount})");

  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   0, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   1, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  11, 0b_0000_0000_0011__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  12, 0b_0000_0000_0001__1000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  13, 0b_0000_0000_0000__1100_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  23, 0b_0000_0000_0000__0000_0000_0011__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  24, 0b_0000_0000_0000__0000_0000_0001__1000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  25, 0b_0000_0000_0000__0000_0000_0000__1100_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  35, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0011__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  36, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0001__1000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  37, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__1100_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  47, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  48, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  49, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  -1, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -47, 0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -48, 0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL, -49, 0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,  -1, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   0, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   1, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,  49, 0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL)]
  public void IBinaryInteger_RotateRight_OfUInt48(ulong value, int rotateAmount, ulong expected)
    => Assert.That(IBinaryInteger_RotateRight((UInt48)value, rotateAmount), Is.EqualTo((UInt48)expected), $"RotateRight({((UInt48)value).ToBinaryString()}, {rotateAmount})");
}
#endif

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [TestCase(0b_0000_0000_0000__0000_0000_0000u,  24)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001u,  23)]
  [TestCase(0b_0000_0000_0000__0000_0000_0010u,  22)]
  [TestCase(0b_0000_0000_0000__0000_0000_0011u,  22)]
  [TestCase(0b_0000_0000_0000__0000_1000_0000u,  16)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000u,  15)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000u,   8)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000u,   7)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000u,   0)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   0)]
  public void LeadingZeroCount(uint value, int expected)
    => Assert.That(UInt24.LeadingZeroCount((UInt24)value), Is.EqualTo(expected), $"LeadingZeroCount({((UInt24)value).ToBinaryString()})");

  [TestCase(0b_0000_0000_0000__0000_0000_0000u,  24)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000u,  23)]
  [TestCase(0b_0100_0000_0000__0000_0000_0000u,  22)]
  [TestCase(0b_1100_0000_0000__0000_0000_0000u,  22)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000u,  16)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000u,  15)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000u,   8)]
  [TestCase(0b_0000_0001_0000__0000_1000_0000u,   7)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001u,   0)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   0)]
  public void TrailingZeroCount(uint value, int expected)
    => Assert.That(UInt24.TrailingZeroCount((UInt24)value), Is.EqualTo(expected), $"TrailingZeroCount({((UInt24)value).ToBinaryString()})");

  [TestCase(0b_0000_0000_0000__0000_0000_0000u,   0)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000u,   1)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000u,   1)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000u,   1)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000u,   1)]
  [TestCase(0b_0000_0000_0000__0000_1000_0000u,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001u,   1)]
  [TestCase(0b_0000_0000_0000__0000_1111_1111u,   8)]
  [TestCase(0b_0000_0000_1111__1111_0000_0000u,   8)]
  [TestCase(0b_1111_1111_0000__0000_0000_0000u,   8)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,  24)]
  public void PopCount(uint value, int expected)
    => Assert.That(UInt24.PopCount((UInt24)value), Is.EqualTo(expected), $"PopCount({((UInt24)value).ToBinaryString()})");
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  48)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  47)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0010uL,  46)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL,  46)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_1000_0000uL,  40)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0001_0000_0000uL,  39)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_1000__0000_0000_0000uL,  32)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0001_0000__0000_0000_0000uL,  31)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__1000_0000_0000__0000_0000_0000uL,  24)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001__0000_0000_0000__0000_0000_0000uL,  23)]
  [TestCase(0b_0000_0000_0000__0000_1000_0000__0000_0000_0000__0000_0000_0000uL,  16)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000__0000_0000_0000__0000_0000_0000uL,  15)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   8)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   7)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   0)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   0)]
  public void LeadingZeroCount(ulong value, int expected)
    => Assert.That(UInt48.LeadingZeroCount((UInt48)value), Is.EqualTo(expected), $"LeadingZeroCount({((UInt48)value).ToBinaryString()})");

  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  48)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  47)]
  [TestCase(0b_0100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  46)]
  [TestCase(0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  46)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  40)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  39)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000__0000_0000_0000__0000_0000_0000uL,  32)]
  [TestCase(0b_0000_0001_0000__0000_1000_0000__0000_0000_0000__0000_0000_0000uL,  31)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001__0000_0000_0000__0000_0000_0000uL,  24)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__1000_0000_0000__0000_0000_0000uL,  23)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0001_0000__0000_0000_0000uL,  16)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_1000__0000_0000_0000uL,  15)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0001_0000_0000uL,   8)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_1000_0000uL,   7)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   0)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   0)]
  public void TrailingZeroCount(ulong value, int expected)
    => Assert.That(UInt48.TrailingZeroCount((UInt48)value), Is.EqualTo(expected), $"TrailingZeroCount({((UInt48)value).ToBinaryString()})");

  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000u,    0)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_1000_0000__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__1000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0001_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_1000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0001_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_1000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_1111_1111uL,   8)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_1111__1111_0000_0000uL,   8)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__1111_1111_0000__0000_0000_0000uL,   8)]
  [TestCase(0b_0000_0000_0000__0000_1111_1111__0000_0000_0000__0000_0000_0000uL,   8)]
  [TestCase(0b_0000_0000_1111__1111_0000_0000__0000_0000_0000__0000_0000_0000uL,   8)]
  [TestCase(0b_1111_1111_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   8)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,  48)]
  public void PopCount(ulong value, int expected)
    => Assert.That(UInt48.PopCount((UInt48)value), Is.EqualTo(expected), $"PopCount({((UInt48)value).ToBinaryString()})");
}

#if SYSTEM_NUMERICS_IBINARYINTEGER
#pragma warning disable IDE0040
partial class UInt24nTests {
#pragma warning restore IDE0040
  static TUInt24n IBinaryInteger_LeadingZeroCount<TUInt24n>(TUInt24n value) where TUInt24n : IBinaryInteger<TUInt24n>
    => TUInt24n.LeadingZeroCount(value);

  [TestCase(0b_0000_0000_0000__0000_0000_0000u,  24)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001u,  23)]
  [TestCase(0b_0000_0000_0000__0000_0000_0010u,  22)]
  [TestCase(0b_0000_0000_0000__0000_0000_0011u,  22)]
  [TestCase(0b_0000_0000_0000__0000_1000_0000u,  16)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000u,  15)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000u,   8)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000u,   7)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000u,   0)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   0)]
  public void IBinaryInteger_LeadingZeroCount_OfUInt24(uint value, int expected)
    => Assert.That(IBinaryInteger_LeadingZeroCount((UInt24)value), Is.EqualTo((UInt24)expected), $"LeadingZeroCount({((UInt24)value).ToBinaryString()})");

  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  48)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,  47)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0010uL,  46)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0011uL,  46)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_1000_0000uL,  40)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0001_0000_0000uL,  39)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_1000__0000_0000_0000uL,  32)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0001_0000__0000_0000_0000uL,  31)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__1000_0000_0000__0000_0000_0000uL,  24)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001__0000_0000_0000__0000_0000_0000uL,  23)]
  [TestCase(0b_0000_0000_0000__0000_1000_0000__0000_0000_0000__0000_0000_0000uL,  16)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000__0000_0000_0000__0000_0000_0000uL,  15)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   8)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   7)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   0)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   0)]
  public void IBinaryInteger_LeadingZeroCount_OfUInt48(ulong value, int expected)
    => Assert.That(IBinaryInteger_LeadingZeroCount((UInt48)value), Is.EqualTo((UInt48)expected), $"LeadingZeroCount({((UInt48)value).ToBinaryString()})");

  static TUInt24n IBinaryInteger_TrailingZeroCount<TUInt24n>(TUInt24n value) where TUInt24n : IBinaryInteger<TUInt24n>
    => TUInt24n.TrailingZeroCount(value);

  [TestCase(0b_0000_0000_0000__0000_0000_0000u,  24)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000u,  23)]
  [TestCase(0b_0100_0000_0000__0000_0000_0000u,  22)]
  [TestCase(0b_1100_0000_0000__0000_0000_0000u,  22)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000u,  16)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000u,  15)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000u,   8)]
  [TestCase(0b_0000_0001_0000__0000_1000_0000u,   7)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001u,   0)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,   0)]
  public void IBinaryInteger_TrailingZeroCount_OfUInt24(uint value, int expected)
    => Assert.That(IBinaryInteger_TrailingZeroCount((UInt24)value), Is.EqualTo((UInt24)expected), $"TrailingZeroCount({((UInt24)value).ToBinaryString()})");

  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  48)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  47)]
  [TestCase(0b_0100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  46)]
  [TestCase(0b_1100_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  46)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  40)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,  39)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000__0000_0000_0000__0000_0000_0000uL,  32)]
  [TestCase(0b_0000_0001_0000__0000_1000_0000__0000_0000_0000__0000_0000_0000uL,  31)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001__0000_0000_0000__0000_0000_0000uL,  24)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__1000_0000_0000__0000_0000_0000uL,  23)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0001_0000__0000_0000_0000uL,  16)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_1000__0000_0000_0000uL,  15)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0001_0000_0000uL,   8)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_1000_0000uL,   7)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   0)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,   0)]
  public void IBinaryInteger_TrailingZeroCount_OfUInt48(ulong value, int expected)
    => Assert.That(IBinaryInteger_TrailingZeroCount((UInt48)value), Is.EqualTo((UInt48)expected), $"TrailingZeroCount({((UInt48)value).ToBinaryString()})");

  static TUInt24n IBinaryInteger_PopCount<TUInt24n>(TUInt24n value) where TUInt24n : IBinaryInteger<TUInt24n>
    => TUInt24n.PopCount(value);

  [TestCase(0b_0000_0000_0000__0000_0000_0000u,   0)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000u,   1)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000u,   1)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000u,   1)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000u,   1)]
  [TestCase(0b_0000_0000_0000__0000_1000_0000u,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001u,   1)]
  [TestCase(0b_0000_0000_0000__0000_1111_1111u,   8)]
  [TestCase(0b_0000_0000_1111__1111_0000_0000u,   8)]
  [TestCase(0b_1111_1111_0000__0000_0000_0000u,   8)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111u,  24)]
  public void IBinaryInteger_PopCount_OfUInt24(uint value, int expected)
    => Assert.That(IBinaryInteger_PopCount((UInt24)value), Is.EqualTo((UInt24)expected), $"PopCount({((UInt24)value).ToBinaryString()})");

  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000u,    0)]
  [TestCase(0b_1000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0001_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_1000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0001_0000_0000__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_1000_0000__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0001__0000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__1000_0000_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0001_0000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_1000__0000_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0001_0000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_1000_0000uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_0000_0001uL,   1)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_0000__0000_1111_1111uL,   8)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__0000_0000_1111__1111_0000_0000uL,   8)]
  [TestCase(0b_0000_0000_0000__0000_0000_0000__1111_1111_0000__0000_0000_0000uL,   8)]
  [TestCase(0b_0000_0000_0000__0000_1111_1111__0000_0000_0000__0000_0000_0000uL,   8)]
  [TestCase(0b_0000_0000_1111__1111_0000_0000__0000_0000_0000__0000_0000_0000uL,   8)]
  [TestCase(0b_1111_1111_0000__0000_0000_0000__0000_0000_0000__0000_0000_0000uL,   8)]
  [TestCase(0b_1111_1111_1111__1111_1111_1111__1111_1111_1111__1111_1111_1111uL,  48)]
  public void IBinaryInteger_PopCount_OfUInt48(ulong value, int expected)
    => Assert.That(IBinaryInteger_PopCount((UInt48)value), Is.EqualTo((UInt48)expected), $"PopCount({((UInt48)value).ToBinaryString()})");
}
#endif
