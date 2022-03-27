// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

#if SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2
using ShimSystemNumericsBitOperationsIsPow2 = System.Numerics.BitOperations;
#else
using ShimSystemNumericsBitOperationsIsPow2 = Smdn.BitOperationsShim;
#endif

#if SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
using ShimSystemNumericsBitOperationsPopCount = System.Numerics.BitOperations;
#else
using ShimSystemNumericsBitOperationsPopCount = Smdn.BitOperationsShim;
#endif

namespace Smdn;

[TestFixture()]
public class BitOperationsShimTests {
  [TestCase(0b_00000000, false)]
  [TestCase(0b_00000001, true)]
  [TestCase(0b_00000010, true)]
  [TestCase(0b_00000100, true)]
  [TestCase(0b_00001000, true)]
  [TestCase(0b_00010000, true)]
  [TestCase(0b_00100000, true)]
  [TestCase(0b_01000000, true)]
  [TestCase(0b_10000000, true)]
  [TestCase(0b_00000011, false)]
  [TestCase(0b_00000110, false)]
  [TestCase(0b_11000000, false)]
  [TestCase(0b_10000001, false)]
  public void IsPow2_32(int value, bool expected)
  {
    Assert.AreEqual(expected, ShimSystemNumericsBitOperationsIsPow2.IsPow2(value), $"IsPow2<int>({0:X4})");
    Assert.AreEqual(expected, ShimSystemNumericsBitOperationsIsPow2.IsPow2(unchecked((uint)value)), $"IsPow2<uint>({0:X4})");
  }

  [TestCase(0b_00000000_00000000, false)]
  [TestCase(0b_00000000_00000001, true)]
  [TestCase(0b_00000000_00000010, true)]
  [TestCase(0b_00000000_00000100, true)]
  [TestCase(0b_00000000_00001000, true)]
  [TestCase(0b_00000000_00010000, true)]
  [TestCase(0b_00000000_00100000, true)]
  [TestCase(0b_00000000_01000000, true)]
  [TestCase(0b_00000000_10000000, true)]
  [TestCase(0b_00000001_00000000, true)]
  [TestCase(0b_00000010_00000000, true)]
  [TestCase(0b_00000100_00000000, true)]
  [TestCase(0b_00001000_00000000, true)]
  [TestCase(0b_00010000_00000000, true)]
  [TestCase(0b_00100000_00000000, true)]
  [TestCase(0b_01000000_00000000, true)]
  [TestCase(0b_10000000_00000000, true)]
  [TestCase(0b_00000000_00000011, false)]
  [TestCase(0b_00000000_00000110, false)]
  [TestCase(0b_00000000_11000000, false)]
  [TestCase(0b_00000001_10000000, false)]
  [TestCase(0b_00000011_00000000, false)]
  [TestCase(0b_11000000_00000000, false)]
  [TestCase(0b_10000000_00000001, false)]
  public void IsPow2_64(long value, bool expected)
  {
    Assert.AreEqual(expected, ShimSystemNumericsBitOperationsIsPow2.IsPow2(value), $"IsPow2<long>({0:X8})");
    Assert.AreEqual(expected, ShimSystemNumericsBitOperationsIsPow2.IsPow2(unchecked((ulong)value)), $"IsPow2<ulong>({0:X8})");
  }

  [TestCase(0b_00000000u, 0)]
  [TestCase(0b_00000001u, 1)]
  [TestCase(0b_00000010u, 1)]
  [TestCase(0b_00000100u, 1)]
  [TestCase(0b_00001000u, 1)]
  [TestCase(0b_00010000u, 1)]
  [TestCase(0b_00100000u, 1)]
  [TestCase(0b_01000000u, 1)]
  [TestCase(0b_10000000u, 1)]
  [TestCase(0b_00000011u, 2)]
  [TestCase(0b_00000111u, 3)]
  [TestCase(0b_00001111u, 4)]
  [TestCase(0b_00011111u, 5)]
  [TestCase(0b_00111111u, 6)]
  [TestCase(0b_01111111u, 7)]
  [TestCase(0b_11000000u, 2)]
  [TestCase(0b_11100000u, 3)]
  [TestCase(0b_11110000u, 4)]
  [TestCase(0b_11111000u, 5)]
  [TestCase(0b_11111100u, 6)]
  [TestCase(0b_11111110u, 7)]
  [TestCase(0b_11111111u, 8)]
  public void PopCount_UInt32(uint value, int expectedValue)
    => Assert.AreEqual(
      expectedValue,
      ShimSystemNumericsBitOperationsPopCount.PopCount(value),
      $"PopCount<uint>({value:X4})"
    );

  [TestCase(0b_00000000_00000000u,  0)]
  [TestCase(0b_00000000_00000001u,  1)]
  [TestCase(0b_00000000_00000010u,  1)]
  [TestCase(0b_00000000_00000100u,  1)]
  [TestCase(0b_00000000_00001000u,  1)]
  [TestCase(0b_00000000_00010000u,  1)]
  [TestCase(0b_00000000_00100000u,  1)]
  [TestCase(0b_00000000_01000000u,  1)]
  [TestCase(0b_00000000_10000000u,  1)]
  [TestCase(0b_00000001_00000000u,  1)]
  [TestCase(0b_00000010_00000000u,  1)]
  [TestCase(0b_00000100_00000000u,  1)]
  [TestCase(0b_00001000_00000000u,  1)]
  [TestCase(0b_00010000_00000000u,  1)]
  [TestCase(0b_00100000_00000000u,  1)]
  [TestCase(0b_01000000_00000000u,  1)]
  [TestCase(0b_10000000_00000000u,  1)]
  [TestCase(0b_00000000_00000011u,  2)]
  [TestCase(0b_00000000_00000111u,  3)]
  [TestCase(0b_00000000_00001111u,  4)]
  [TestCase(0b_00000000_00011111u,  5)]
  [TestCase(0b_00000000_00111111u,  6)]
  [TestCase(0b_00000000_01111111u,  7)]
  [TestCase(0b_00000000_11111111u,  8)]
  [TestCase(0b_00000001_11111111u,  9)]
  [TestCase(0b_00000011_11111111u, 10)]
  [TestCase(0b_00000111_11111111u, 11)]
  [TestCase(0b_00001111_11111111u, 12)]
  [TestCase(0b_00011111_11111111u, 13)]
  [TestCase(0b_00111111_11111111u, 14)]
  [TestCase(0b_01111111_11111111u, 15)]
  [TestCase(0b_11000000_00000000u,  2)]
  [TestCase(0b_11100000_00000000u,  3)]
  [TestCase(0b_11110000_00000000u,  4)]
  [TestCase(0b_11111000_00000000u,  5)]
  [TestCase(0b_11111100_00000000u,  6)]
  [TestCase(0b_11111110_00000000u,  7)]
  [TestCase(0b_11111111_00000000u,  8)]
  [TestCase(0b_11111111_10000000u,  9)]
  [TestCase(0b_11111111_11000000u, 10)]
  [TestCase(0b_11111111_11100000u, 11)]
  [TestCase(0b_11111111_11110000u, 12)]
  [TestCase(0b_11111111_11111000u, 13)]
  [TestCase(0b_11111111_11111100u, 14)]
  [TestCase(0b_11111111_11111110u, 15)]
  [TestCase(0b_11111111_11111111u, 16)]
  public void PopCount_UInt64(ulong value, int expectedValue)
    => Assert.AreEqual(
      expectedValue,
      ShimSystemNumericsBitOperationsPopCount.PopCount(value),
      $"PopCount<ulong>({value:X8})"
    );
}
