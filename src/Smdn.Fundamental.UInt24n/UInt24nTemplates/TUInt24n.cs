// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.CompilerServices;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n {
#pragma warning restore IDE0040

  private const int BitsOfSelf = 8 * SizeOfSelf;
  private const TIntWide maxValue = ((TIntWide)1 << BitsOfSelf) - 1;
  private const TIntWide minValue = (TIntWide)0;
  private const TUIntWide UnusedBitMask = ~unchecked((TUIntWide)maxValue);

  public static readonly TUInt24n MaxValue = new(maxValue, check: false);
  public static readonly TUInt24n MinValue = new(minValue, check: false);
  public static readonly TUInt24n Zero     = new(0, check: false);
  public static readonly TUInt24n One      = new(1, check: false);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public TIntWide ToTIntWide() => unchecked((TIntWide)Widen());

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  [CLSCompliant(false)]
  public TUIntWide ToTUIntWide() => Widen();
}
