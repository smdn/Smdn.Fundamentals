// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

#if SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
using ShimBitOperationsPopCount = System.Numerics.BitOperations;
#else
using ShimBitOperationsPopCount = Smdn.BitOperationsShim;
#endif

namespace Smdn;

public static class BitOperationsShim {
#if !SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2
  public static bool IsPow2(int value) => ShimBitOperationsPopCount.PopCount(unchecked((uint)value)) == 1;
  public static bool IsPow2(long value) => ShimBitOperationsPopCount.PopCount(unchecked((ulong)value)) == 1;
  [CLSCompliant(false)] public static bool IsPow2(uint value) => ShimBitOperationsPopCount.PopCount(value) == 1;
  [CLSCompliant(false)] public static bool IsPow2(ulong value) => ShimBitOperationsPopCount.PopCount(value) == 1;
#endif

#if !SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
  [CLSCompliant(false)]
  public static int PopCount(uint value)
  {
    var count = 0;

    for (uint mask = 1; mask != 0; mask <<= 1) {
      if ((value & mask) != 0u)
        count++;
    }

    return count;
  }

  [CLSCompliant(false)]
  public static int PopCount(ulong value)
  {
    var count = 0;

    for (ulong mask = 1; mask != 0; mask <<= 1) {
      if ((value & mask) != 0uL)
        count++;
    }

    return count;
  }
#endif
}
