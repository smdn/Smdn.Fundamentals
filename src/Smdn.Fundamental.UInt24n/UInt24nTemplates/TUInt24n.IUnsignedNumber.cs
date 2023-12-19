// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n
#if FEATURE_GENERIC_MATH
  : IUnsignedNumber<TUInt24n>
#endif
{
#pragma warning restore IDE0040
}
