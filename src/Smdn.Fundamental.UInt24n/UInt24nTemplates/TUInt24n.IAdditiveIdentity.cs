// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif

namespace Smdn;

#if FEATURE_GENERIC_MATH
#pragma warning disable IDE0040
partial struct TUInt24n
#if false
  :
  IAdditiveIdentity<TUInt24n, TIntWide>,
  IAdditiveIdentity<TUInt24n, TUIntWide>
#endif
{
#pragma warning restore IDE0040
  static TUInt24n IAdditiveIdentity<TUInt24n, TUInt24n>.AdditiveIdentity => Zero;
#if false
  static TIntWide IAdditiveIdentity<TUInt24n, TIntWide>.AdditiveIdentity => (TIntWide)0;
  static TUIntWide IAdditiveIdentity<TUInt24n, TUIntWide>.AdditiveIdentity => (TUIntWide)0u;
#endif
}
#endif
