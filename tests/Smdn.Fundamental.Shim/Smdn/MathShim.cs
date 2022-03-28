// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

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
    Assert.AreEqual((byte)expected, ShimTypeSystemMathClamp.Clamp((byte)value, (byte)min, (byte)max), $"Clamp<byte>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((ushort)expected, ShimTypeSystemMathClamp.Clamp((ushort)value, (ushort)min, (ushort)max), $"Clamp<ushort>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((uint)expected, ShimTypeSystemMathClamp.Clamp((uint)value, (uint)min, (uint)max), $"Clamp<uint>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((ulong)expected, ShimTypeSystemMathClamp.Clamp((ulong)value, (ulong)min, (ulong)max), $"Clamp<ulong>(value: {value}, min: {min}, max: {max})");

    Assert.AreEqual((sbyte)expected, ShimTypeSystemMathClamp.Clamp((sbyte)value, (sbyte)min, (sbyte)max), $"Clamp<sbyte>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((short)expected, ShimTypeSystemMathClamp.Clamp((short)value, (short)min, (short)max), $"Clamp<short>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((int)expected, ShimTypeSystemMathClamp.Clamp((int)value, (int)min, (int)max), $"Clamp<int>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((long)expected, ShimTypeSystemMathClamp.Clamp((long)value, (long)min, (long)max), $"Clamp<long>(value: {value}, min: {min}, max: {max})");

    Assert.AreEqual((float)expected, ShimTypeSystemMathClamp.Clamp((float)value, (float)min, (float)max), $"Clamp<float>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((double)expected, ShimTypeSystemMathClamp.Clamp((double)value, (double)min, (double)max), $"Clamp<double>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((decimal)expected, ShimTypeSystemMathClamp.Clamp((decimal)value, (decimal)min, (decimal)max), $"Clamp<decimal>(value: {value}, min: {min}, max: {max})");
  }

  [TestCase(-2, -1, +1, -1)]
  [TestCase(-1, -1, +1, -1)]
  [TestCase( 0, -1, +1,  0)]
  [TestCase(+1, -1, +1, +1)]
  [TestCase(+2, -1, +1, +1)]
  public void Clamp_MinusValue(int value, int min, int max, int expected)
  {
    Assert.AreEqual((sbyte)expected, ShimTypeSystemMathClamp.Clamp((sbyte)value, (sbyte)min, (sbyte)max), $"Clamp<sbyte>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((short)expected, ShimTypeSystemMathClamp.Clamp((short)value, (short)min, (short)max), $"Clamp<short>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((int)expected, ShimTypeSystemMathClamp.Clamp((int)value, (int)min, (int)max), $"Clamp<int>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((long)expected, ShimTypeSystemMathClamp.Clamp((long)value, (long)min, (long)max), $"Clamp<long>(value: {value}, min: {min}, max: {max})");

    Assert.AreEqual((float)expected, ShimTypeSystemMathClamp.Clamp((float)value, (float)min, (float)max), $"Clamp<float>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((double)expected, ShimTypeSystemMathClamp.Clamp((double)value, (double)min, (double)max), $"Clamp<double>(value: {value}, min: {min}, max: {max})");
    Assert.AreEqual((decimal)expected, ShimTypeSystemMathClamp.Clamp((decimal)value, (decimal)min, (decimal)max), $"Clamp<decimal>(value: {value}, min: {min}, max: {max})");
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

  [TestCase(0, 1, 0, 0)]
  [TestCase(1, 1, 1, 0)]
  [TestCase(1, 2, 0, 1)]
  [TestCase(2, 2, 1, 0)]
  [TestCase(2, 1, 2, 0)]
  public void DivRem(int left, int right, int expectedQuotient, int expectedRemainder)
  {
    Assert.AreEqual(expectedQuotient, ShimTypeSystemMathDivRem.DivRem(left, right, out var remainderInt), $"DivRem<int>({left}, {right}) quotient");
    Assert.AreEqual(expectedRemainder, remainderInt, $"DivRem<int>({left}, {right}) remainder");

    Assert.AreEqual((long)expectedQuotient, ShimTypeSystemMathDivRem.DivRem((long)left, (long)right, out long remainderLong), $"DivRem<long>({left}, {right}) quotient");
    Assert.AreEqual((long)expectedRemainder, remainderLong, $"DivRem<long>({left}, {right}) remainder");
  }

  [TestCase(0, 0)]
  [TestCase(1, 0)]
  public void DivRem_DivideByZero(int left, int right)
  {
    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRem.DivRem(left, right, out _), $"DivRem<int>({left}, {right})");
    Assert.Throws<DivideByZeroException>(() => ShimTypeSystemMathDivRem.DivRem((long)left, (long)right, out _), $"DivRem<long>({left}, {right})");
  }

  [TestCase(0, 1, 0, 0)]
  [TestCase(1, 1, 1, 0)]
  [TestCase(1, 2, 0, 1)]
  [TestCase(2, 2, 1, 0)]
  [TestCase(2, 1, 2, 0)]
  public void DivRemReturnValueTuple2(int left, int right, int expectedQuotient, int expectedRemainder)
  {
    Assert.AreEqual(((sbyte)expectedQuotient, (sbyte)expectedRemainder), ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((sbyte)left, (sbyte)right), $"DivRem<sbyte>({left}, {right})");
    Assert.AreEqual(((short)expectedQuotient, (short)expectedRemainder), ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((short)left, (short)right), $"DivRem<long>({left}, {right})");
    Assert.AreEqual((expectedQuotient, expectedRemainder), ShimTypeSystemMathDivRemReturnValueTuple2.DivRem(left, right), $"DivRem<int>({left}, {right})");
    Assert.AreEqual(((long)expectedQuotient, (long)expectedRemainder), ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((long)left, (long)right), $"DivRem<long>({left}, {right})");

    Assert.AreEqual(((byte)expectedQuotient, (byte)expectedRemainder), ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((byte)left, (byte)right), $"DivRem<byte>({left}, {right})");
    Assert.AreEqual(((ushort)expectedQuotient, (ushort)expectedRemainder), ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((ushort)left, (ushort)right), $"DivRem<ushort>({left}, {right})");
    Assert.AreEqual(((uint)expectedQuotient, (uint)expectedRemainder), ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((uint)left, (uint)right), $"DivRem<uint>({left}, {right})");
    Assert.AreEqual(((ulong)expectedQuotient, (ulong)expectedRemainder), ShimTypeSystemMathDivRemReturnValueTuple2.DivRem((ulong)left, (ulong)right), $"DivRem<ulong>({left}, {right})");
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
