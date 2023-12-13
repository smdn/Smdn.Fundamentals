// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public class MathShimTests {
  [Test]
  public void ShimType_Clamp()
    => Assert.That(
#if SYSTEM_MATH_CLAMP
      typeof(System.Math)
#else
      typeof(Smdn.MathShim)
#endif
      ,
      Is.EqualTo(typeof(ShimTypeSystemMathClamp))
    );

  [TestCase(0, 0, 0, 0)]
  [TestCase(0, 0, 1, 0)]
  [TestCase(1, 0, 1, 1)]
  [TestCase(1, 1, 1, 1)]
  [TestCase(2, 0, 1, 1)]
  [TestCase(1, 0, 2, 1)]
  public void Clamp(int value, int min, int max, int expected)
  {
    Assert.That(ShimTypeSystemMathClamp.Clamp((byte)value, (byte)min, (byte)max), Is.EqualTo((byte)expected), $"Clamp<byte>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((ushort)value, (ushort)min, (ushort)max), Is.EqualTo((ushort)expected), $"Clamp<ushort>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((uint)value, (uint)min, (uint)max), Is.EqualTo((uint)expected), $"Clamp<uint>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((ulong)value, (ulong)min, (ulong)max), Is.EqualTo((ulong)expected), $"Clamp<ulong>(value: {value}, min: {min}, max: {max})");

    Assert.That(ShimTypeSystemMathClamp.Clamp((sbyte)value, (sbyte)min, (sbyte)max), Is.EqualTo((sbyte)expected), $"Clamp<sbyte>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((short)value, (short)min, (short)max), Is.EqualTo((short)expected), $"Clamp<short>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((int)value, (int)min, (int)max), Is.EqualTo((int)expected), $"Clamp<int>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((long)value, (long)min, (long)max), Is.EqualTo((long)expected), $"Clamp<long>(value: {value}, min: {min}, max: {max})");

    Assert.That(ShimTypeSystemMathClamp.Clamp((float)value, (float)min, (float)max), Is.EqualTo((float)expected), $"Clamp<float>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((double)value, (double)min, (double)max), Is.EqualTo((double)expected), $"Clamp<double>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((decimal)value, (decimal)min, (decimal)max), Is.EqualTo((decimal)expected), $"Clamp<decimal>(value: {value}, min: {min}, max: {max})");
  }

  [TestCase(-2, -1, +1, -1)]
  [TestCase(-1, -1, +1, -1)]
  [TestCase( 0, -1, +1,  0)]
  [TestCase(+1, -1, +1, +1)]
  [TestCase(+2, -1, +1, +1)]
  public void Clamp_MinusValue(int value, int min, int max, int expected)
  {
    Assert.That(ShimTypeSystemMathClamp.Clamp((sbyte)value, (sbyte)min, (sbyte)max), Is.EqualTo((sbyte)expected), $"Clamp<sbyte>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((short)value, (short)min, (short)max), Is.EqualTo((short)expected), $"Clamp<short>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((int)value, (int)min, (int)max), Is.EqualTo((int)expected), $"Clamp<int>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((long)value, (long)min, (long)max), Is.EqualTo((long)expected), $"Clamp<long>(value: {value}, min: {min}, max: {max})");

    Assert.That(ShimTypeSystemMathClamp.Clamp((float)value, (float)min, (float)max), Is.EqualTo((float)expected), $"Clamp<float>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((double)value, (double)min, (double)max), Is.EqualTo((double)expected), $"Clamp<double>(value: {value}, min: {min}, max: {max})");
    Assert.That(ShimTypeSystemMathClamp.Clamp((decimal)value, (decimal)min, (decimal)max), Is.EqualTo((decimal)expected), $"Clamp<decimal>(value: {value}, min: {min}, max: {max})");
  }

  [TestCase(0, 1, 0)]
  [TestCase(1, 1, 0)]
  [TestCase(0, 2, 1)]
  public void Clamp_MaxMustBeGreaterThanMin(int value, int min, int max)
  {
    Assert.Throws<ArgumentException>(() => ShimTypeSystemMathClamp.Clamp((byte)value, (byte)min, (byte)max), $"Clamp<byte>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimTypeSystemMathClamp.Clamp((ushort)value, (ushort)min, (ushort)max), $"Clamp<ushort>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimTypeSystemMathClamp.Clamp((uint)value, (uint)min, (uint)max), $"Clamp<uint>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimTypeSystemMathClamp.Clamp((ulong)value, (ulong)min, (ulong)max), $"Clamp<ulong>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimTypeSystemMathClamp.Clamp((sbyte)value, (sbyte)min, (sbyte)max), $"Clamp<sbyte>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimTypeSystemMathClamp.Clamp((short)value, (short)min, (short)max), $"Clamp<short>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimTypeSystemMathClamp.Clamp((int)value, (int)min, (int)max), $"Clamp<int>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimTypeSystemMathClamp.Clamp((long)value, (long)min, (long)max), $"Clamp<long>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimTypeSystemMathClamp.Clamp((float)value, (float)min, (float)max), $"Clamp<float>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimTypeSystemMathClamp.Clamp((double)value, (double)min, (double)max), $"Clamp<double>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimTypeSystemMathClamp.Clamp((decimal)value, (decimal)min, (decimal)max), $"Clamp<decimal>(value: {value}, min: {min}, max: {max})");
  }

  [Test]
  public void ShimType_DivRem()
    => Assert.That(
#if SYSTEM_MATH_DIVREM
      typeof(System.Math)
#else
      typeof(Smdn.MathShim)
#endif
      ,
      Is.EqualTo(typeof(ShimTypeSystemMathDivRem))
    );

  [TestCase(0, 1, 0, 0)]
  [TestCase(1, 1, 1, 0)]
  [TestCase(1, 2, 0, 1)]
  [TestCase(2, 2, 1, 0)]
  [TestCase(2, 1, 2, 0)]
  public void DivRem(int left, int right, int expectedQuotient, int expectedRemainder)
  {
    Assert.That(ShimTypeSystemMathDivRem.DivRem(left, right, out var remainderInt), Is.EqualTo(expectedQuotient), $"DivRem<int>({left}, {right}) quotient");
    Assert.That(remainderInt, Is.EqualTo(expectedRemainder), $"DivRem<int>({left}, {right}) remainder");

    Assert.That(ShimTypeSystemMathDivRem.DivRem((long)left, (long)right, out long remainderLong), Is.EqualTo((long)expectedQuotient), $"DivRem<long>({left}, {right}) quotient");
    Assert.That(remainderLong, Is.EqualTo((long)expectedRemainder), $"DivRem<long>({left}, {right}) remainder");
  }

  [TestCase(0, 0)]
  [TestCase(1, 0)]
  public void DivRem_DivideByZero(int left, int right)
  {
    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRem.DivRem(left, right, out _), $"DivRem<int>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRem.DivRem((long)left, (long)right, out _), $"DivRem<long>({left}, {right})");
  }

  [Test]
  public void ShimType_ReturnValueTuple2()
    => Assert.That(
#if SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
      typeof(System.Math)
#else
      typeof(Smdn.MathShim)
#endif
      ,
      Is.EqualTo(typeof(ShimTypeSystemMathDivRemReturnValueTuple2))
    );

  [TestCase(0, 1, 0, 0)]
  [TestCase(1, 1, 1, 0)]
  [TestCase(1, 2, 0, 1)]
  [TestCase(2, 2, 1, 0)]
  [TestCase(2, 1, 2, 0)]
  public void DivRemReturnValueTuple2(int left, int right, int expectedQuotient, int expectedRemainder)
  {
    Assert.That(ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((sbyte)left, (sbyte)right), Is.EqualTo(((sbyte)expectedQuotient, (sbyte)expectedRemainder)), $"DivRem<sbyte>({left}, {right})");
    Assert.That(ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((short)left, (short)right), Is.EqualTo(((short)expectedQuotient, (short)expectedRemainder)), $"DivRem<long>({left}, {right})");
    Assert.That(ShimTypeSystemMathDivRemReturnValueTuple2.DivRem(left, right), Is.EqualTo((expectedQuotient, expectedRemainder)), $"DivRem<int>({left}, {right})");
    Assert.That(ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((long)left, (long)right), Is.EqualTo(((long)expectedQuotient, (long)expectedRemainder)), $"DivRem<long>({left}, {right})");

    Assert.That(ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((byte)left, (byte)right), Is.EqualTo(((byte)expectedQuotient, (byte)expectedRemainder)), $"DivRem<byte>({left}, {right})");
    Assert.That(ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((ushort)left, (ushort)right), Is.EqualTo(((ushort)expectedQuotient, (ushort)expectedRemainder)), $"DivRem<ushort>({left}, {right})");
    Assert.That(ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((uint)left, (uint)right), Is.EqualTo(((uint)expectedQuotient, (uint)expectedRemainder)), $"DivRem<uint>({left}, {right})");
    Assert.That(ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((ulong)left, (ulong)right), Is.EqualTo(((ulong)expectedQuotient, (ulong)expectedRemainder)), $"DivRem<ulong>({left}, {right})");
  }

  [TestCase(0, 0)]
  [TestCase(1, 0)]
  public void DivRemReturnValueTuple2_DivideByZero(int left, int right)
  {
    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((sbyte)left, (sbyte)right), $"DivRem<sbyte>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((short)left, (short)right), $"DivRem<short>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRemReturnValueTuple2.DivRem(left, right), $"DivRem<int>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((long)left, (long)right), $"DivRem<long>({left}, {right})");

    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((byte)left, (byte)right), $"DivRem<byte>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((ushort)left, (ushort)right), $"DivRem<ushort>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((uint)left, (uint)right), $"DivRem<uint>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((ulong)left, (ulong)right), $"DivRem<ulong>({left}, {right})");
  }
}
