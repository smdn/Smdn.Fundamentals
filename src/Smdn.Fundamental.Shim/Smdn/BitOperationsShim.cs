// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

public static class BitOperationsShim {
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
