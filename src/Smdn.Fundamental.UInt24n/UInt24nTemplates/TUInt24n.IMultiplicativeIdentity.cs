// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_NUMERICS_IMULTIPLICATIVEIDENTITY
using System;
using System.Numerics;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n : IMultiplicativeIdentity<TUInt24n, TUInt24n> {
#pragma warning restore IDE0040
  static TUInt24n IMultiplicativeIdentity<TUInt24n, TUInt24n>.MultiplicativeIdentity => One;
}
#endif
