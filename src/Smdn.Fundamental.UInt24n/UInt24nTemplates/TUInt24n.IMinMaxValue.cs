// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif

namespace Smdn;

#if FEATURE_GENERIC_MATH
#pragma warning disable IDE0040
partial struct TUInt24n : IMinMaxValue<TUInt24n> {
#pragma warning restore IDE0040
  static TUInt24n IMinMaxValue<TUInt24n>.MinValue => MinValue;
  static TUInt24n IMinMaxValue<TUInt24n>.MaxValue => MaxValue;
}
#endif
