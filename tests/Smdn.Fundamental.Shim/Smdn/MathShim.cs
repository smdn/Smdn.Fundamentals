// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

#if SYSTEM_MATH_CLAMP
using ShimSystemMathClamp = System.Math;
#else
using ShimSystemMathClamp = Smdn.MathShim;
#endif

#if SYSTEM_MATH_DIVREM
using ShimSystemMathDivRem = System.Math;
#else
using ShimSystemMathDivRem = Smdn.MathShim;
#endif

#if SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
using ShimSystemMathDivRemReturnValueTuple2 = System.Math;
#else
using ShimSystemMathDivRemReturnValueTuple2 = Smdn.MathShim;
#endif

namespace Smdn;

[TestFixture()]
public class MathShimTests {
  [TestCase(0, 0, 0, 0)]
  [TestCase(0, 0, 1, 0)]
  [TestCase(1, 0, 1, 1)]
  [TestCase(1, 1, 1, 1)]
  [TestCase(2, 0, 1, 1)]
  [TestCase(1, 0, 2, 1)]
  public void Clamp(int value, int min, int max, int expected)
  {
    Assert.AreEqual((byte)expected, ShimSystemMathClamp.Clamp((byte)value, (byte)min, (byte)max), $"Clamp<byte>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((ushort)expected, ShimSystemMathClamp.Clamp((ushort)value, (ushort)min, (ushort)max), $"Clamp<ushort>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((uint)expected, ShimSystemMathClamp.Clamp((uint)value, (uint)min, (uint)max), $"Clamp<uint>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((ulong)expected, ShimSystemMathClamp.Clamp((ulong)value, (ulong)min, (ulong)max), $"Clamp<ulong>(value: {value}, min: {min}, max: {max})");

    Assert.AreEqual((sbyte)expected, ShimSystemMathClamp.Clamp((sbyte)value, (sbyte)min, (sbyte)max), $"Clamp<sbyte>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((short)expected, ShimSystemMathClamp.Clamp((short)value, (short)min, (short)max), $"Clamp<short>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((int)expected, ShimSystemMathClamp.Clamp((int)value, (int)min, (int)max), $"Clamp<int>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((long)expected, ShimSystemMathClamp.Clamp((long)value, (long)min, (long)max), $"Clamp<long>(value: {value}, min: {min}, max: {max})");

    Assert.AreEqual((float)expected, ShimSystemMathClamp.Clamp((float)value, (float)min, (float)max), $"Clamp<float>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((double)expected, ShimSystemMathClamp.Clamp((double)value, (double)min, (double)max), $"Clamp<double>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((decimal)expected, ShimSystemMathClamp.Clamp((decimal)value, (decimal)min, (decimal)max), $"Clamp<decimal>(value: {value}, min: {min}, max: {max})");
  }

  [TestCase(-2, -1, +1, -1)]
  [TestCase(-1, -1, +1, -1)]
  [TestCase( 0, -1, +1,  0)]
  [TestCase(+1, -1, +1, +1)]
  [TestCase(+2, -1, +1, +1)]
  public void Clamp_MinusValue(int value, int min, int max, int expected)
  {
    Assert.AreEqual((sbyte)expected, ShimSystemMathClamp.Clamp((sbyte)value, (sbyte)min, (sbyte)max), $"Clamp<sbyte>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((short)expected, ShimSystemMathClamp.Clamp((short)value, (short)min, (short)max), $"Clamp<short>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((int)expected, ShimSystemMathClamp.Clamp((int)value, (int)min, (int)max), $"Clamp<int>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((long)expected, ShimSystemMathClamp.Clamp((long)value, (long)min, (long)max), $"Clamp<long>(value: {value}, min: {min}, max: {max})");

    Assert.AreEqual((float)expected, ShimSystemMathClamp.Clamp((float)value, (float)min, (float)max), $"Clamp<float>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((double)expected, ShimSystemMathClamp.Clamp((double)value, (double)min, (double)max), $"Clamp<double>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((decimal)expected, ShimSystemMathClamp.Clamp((decimal)value, (decimal)min, (decimal)max), $"Clamp<decimal>(value: {value}, min: {min}, max: {max})");
  }

  [TestCase(0, 1, 0)]
  [TestCase(1, 1, 0)]
  [TestCase(0, 2, 1)]
  public void Clamp_MaxMustBeGreaterThanMin(int value, int min, int max)
  {
    Assert.Throws<ArgumentException>(() => ShimSystemMathClamp.Clamp((byte)value, (byte)min, (byte)max), $"Clamp<byte>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimSystemMathClamp.Clamp((ushort)value, (ushort)min, (ushort)max), $"Clamp<ushort>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimSystemMathClamp.Clamp((uint)value, (uint)min, (uint)max), $"Clamp<uint>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimSystemMathClamp.Clamp((ulong)value, (ulong)min, (ulong)max), $"Clamp<ulong>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimSystemMathClamp.Clamp((sbyte)value, (sbyte)min, (sbyte)max), $"Clamp<sbyte>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimSystemMathClamp.Clamp((short)value, (short)min, (short)max), $"Clamp<short>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimSystemMathClamp.Clamp((int)value, (int)min, (int)max), $"Clamp<int>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimSystemMathClamp.Clamp((long)value, (long)min, (long)max), $"Clamp<long>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimSystemMathClamp.Clamp((float)value, (float)min, (float)max), $"Clamp<float>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimSystemMathClamp.Clamp((double)value, (double)min, (double)max), $"Clamp<double>(value: {value}, min: {min}, max: {max})");
    Assert.Throws<ArgumentException>(() => ShimSystemMathClamp.Clamp((decimal)value, (decimal)min, (decimal)max), $"Clamp<decimal>(value: {value}, min: {min}, max: {max})");
  }

  [TestCase(0, 1, 0, 0)]
  [TestCase(1, 1, 1, 0)]
  [TestCase(1, 2, 0, 1)]
  [TestCase(2, 2, 1, 0)]
  [TestCase(2, 1, 2, 0)]
  public void DivRem(int left, int right, int expectedQuotient, int expectedRemainder)
  {
    Assert.AreEqual(expectedQuotient, ShimSystemMathDivRem.DivRem(left, right, out var remainderInt), $"DivRem<int>({left}, {right}) quotient");
    Assert.AreEqual(expectedRemainder, remainderInt, $"DivRem<int>({left}, {right}) remainder");

    Assert.AreEqual((long)expectedQuotient, ShimSystemMathDivRem.DivRem((long)left, (long)right, out long remainderLong), $"DivRem<long>({left}, {right}) quotient");
    Assert.AreEqual((long)expectedRemainder, remainderLong, $"DivRem<long>({left}, {right}) remainder");
  }

  [TestCase(0, 0)]
  [TestCase(1, 0)]
  public void DivRem_DivideByZero(int left, int right)
  {
    Assert.Throws<DivideByZeroException>(() => ShimSystemMathDivRem.DivRem(left, right, out _), $"DivRem<int>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimSystemMathDivRem.DivRem((long)left, (long)right, out _), $"DivRem<long>({left}, {right})");
  }

  [TestCase(0, 1, 0, 0)]
  [TestCase(1, 1, 1, 0)]
  [TestCase(1, 2, 0, 1)]
  [TestCase(2, 2, 1, 0)]
  [TestCase(2, 1, 2, 0)]
  public void DivRemReturnValueTuple2(int left, int right, int expectedQuotient, int expectedRemainder)
  {
    Assert.AreEqual(((sbyte)expectedQuotient, (sbyte)expectedRemainder), ShimSystemMathDivRemReturnValueTuple2.DivRem((sbyte)left, (sbyte)right), $"DivRem<sbyte>({left}, {right})");
    Assert.AreEqual(((short)expectedQuotient, (short)expectedRemainder), ShimSystemMathDivRemReturnValueTuple2.DivRem((short)left, (short)right), $"DivRem<long>({left}, {right})");
    Assert.AreEqual((expectedQuotient, expectedRemainder), ShimSystemMathDivRemReturnValueTuple2.DivRem(left, right), $"DivRem<int>({left}, {right})");
    Assert.AreEqual(((long)expectedQuotient, (long)expectedRemainder), ShimSystemMathDivRemReturnValueTuple2.DivRem((long)left, (long)right), $"DivRem<long>({left}, {right})");

    Assert.AreEqual(((byte)expectedQuotient, (byte)expectedRemainder), ShimSystemMathDivRemReturnValueTuple2.DivRem((byte)left, (byte)right), $"DivRem<byte>({left}, {right})");
    Assert.AreEqual(((ushort)expectedQuotient, (ushort)expectedRemainder), ShimSystemMathDivRemReturnValueTuple2.DivRem((ushort)left, (ushort)right), $"DivRem<ushort>({left}, {right})");
    Assert.AreEqual(((uint)expectedQuotient, (uint)expectedRemainder), ShimSystemMathDivRemReturnValueTuple2.DivRem((uint)left, (uint)right), $"DivRem<uint>({left}, {right})");
    Assert.AreEqual(((ulong)expectedQuotient, (ulong)expectedRemainder), ShimSystemMathDivRemReturnValueTuple2.DivRem((ulong)left, (ulong)right), $"DivRem<ulong>({left}, {right})");
  }

  [TestCase(0, 0)]
  [TestCase(1, 0)]
  public void DivRemReturnValueTuple2_DivideByZero(int left, int right)
  {
    Assert.Throws<DivideByZeroException>(() => ShimSystemMathDivRemReturnValueTuple2.DivRem((sbyte)left, (sbyte)right), $"DivRem<sbyte>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimSystemMathDivRemReturnValueTuple2.DivRem((short)left, (short)right), $"DivRem<short>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimSystemMathDivRemReturnValueTuple2.DivRem(left, right), $"DivRem<int>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimSystemMathDivRemReturnValueTuple2.DivRem((long)left, (long)right), $"DivRem<long>({left}, {right})");

    Assert.Throws<DivideByZeroException>(() => ShimSystemMathDivRemReturnValueTuple2.DivRem((byte)left, (byte)right), $"DivRem<byte>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimSystemMathDivRemReturnValueTuple2.DivRem((ushort)left, (ushort)right), $"DivRem<ushort>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimSystemMathDivRemReturnValueTuple2.DivRem((uint)left, (uint)right), $"DivRem<uint>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimSystemMathDivRemReturnValueTuple2.DivRem((ulong)left, (ulong)right), $"DivRem<ulong>({left}, {right})");
  }
}
