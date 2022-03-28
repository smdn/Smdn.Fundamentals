// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

public static class BitOperationsShim {
#if !SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2
  public static bool IsPow2(int value) => 0 <= value && ShimTypeSystemNumericsBitOperationsPopCount.PopCount(unchecked((uint)value)) == 1;
  public static bool IsPow2(long value) => 0L <= value && ShimTypeSystemNumericsBitOperationsPopCount.PopCount(unchecked((ulong)value)) == 1;
  [CLSCompliant(false)] public static bool IsPow2(uint value) => ShimTypeSystemNumericsBitOperationsPopCount.PopCount(value) == 1;
  [CLSCompliant(false)] public static bool IsPow2(ulong value) => ShimTypeSystemNumericsBitOperationsPopCount.PopCount(value) == 1;
#endif

#if !SYSTEM_NUMERICS_BITOPERATIONS_LOG2
  [CLSCompliant(false)] public static int Log2(uint value) => value == 0u ? 0 : 31 - ShimTypeSystemNumericsBitOperationsLeadingZeroCount.LeadingZeroCount(value);
  [CLSCompliant(false)] public static int Log2(ulong value) => value == 0uL ? 0 : 63 - ShimTypeSystemNumericsBitOperationsLeadingZeroCount.LeadingZeroCount(value);
#endif

#if !SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
  [CLSCompliant(false)]
  public static int PopCount(uint value)
  {
    var count = 0;

    for (uint mask = 1u; mask != 0u; mask <<= 1) {
      if ((value & mask) != 0u)
        count++;
    }

    return count;
  }

  [CLSCompliant(false)]
  public static int PopCount(ulong value)
  {
    var count = 0;

    for (ulong mask = 1uL; mask != 0uL; mask <<= 1) {
      if ((value & mask) != 0uL)
        count++;
    }

    return count;
  }
#endif

#if !SYSTEM_NUMERICS_BITOPERATIONS_LEADINGZEROCOUNT
  [CLSCompliant(false)]
  public static int LeadingZeroCount(uint value)
  {
    var count = 0;

    for (uint mask = 0x8000_0000u; mask != 0u; mask >>= 1, count++) {
      if ((value & mask) != 0u)
        break;
    }

    return count;
  }

  [CLSCompliant(false)]
  public static int LeadingZeroCount(ulong value)
  {
    var count = 0;

    for (ulong mask = 0x8000_0000_0000_0000uL; mask != 0uL; mask >>= 1, count++) {
      if ((value & mask) != 0uL)
        break;
    }

    return count;
  }
#endif

#if !SYSTEM_NUMERICS_BITOPERATIONS_TRAILINGZEROCOUNT
  [CLSCompliant(false)]
  public static int TrailingZeroCount(uint value)
  {
    if (value == 0u)
      return 32;

    var count = 0;

    for (uint mask = 0x0000_0001u; mask != 0u; mask <<= 1, count++) {
      if ((value & mask) != 0u)
        break;
    }

    return count;
  }

  [CLSCompliant(false)]
  public static int TrailingZeroCount(ulong value)
  {
    if (value == 0uL)
      return 64;

    var count = 0;

    for (ulong mask = 0x0000_0000_0000_0001uL; mask != 0uL; mask <<= 1, count++) {
      if ((value & mask) != 0uL)
        break;
    }

    return count;
  }
#endif
}
