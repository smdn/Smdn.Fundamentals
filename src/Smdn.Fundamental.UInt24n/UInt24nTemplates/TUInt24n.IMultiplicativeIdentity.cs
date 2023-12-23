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
  IMultiplicativeIdentity<TUInt24n, TIntWide>,
  IMultiplicativeIdentity<TUInt24n, TUIntWide>
#endif
{
#pragma warning restore IDE0040
  static TUInt24n IMultiplicativeIdentity<TUInt24n, TUInt24n>.MultiplicativeIdentity => One;
#if false
  static TIntWide IMultiplicativeIdentity<TUInt24n, TIntWide>.MultiplicativeIdentity => (TIntWide)1;
  static TUIntWide IMultiplicativeIdentity<TUInt24n, TUIntWide>.MultiplicativeIdentity => (TUIntWide)1u;
#endif
}
#endif
