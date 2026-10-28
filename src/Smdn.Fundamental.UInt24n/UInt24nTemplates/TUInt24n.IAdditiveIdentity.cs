// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_NUMERICS_IADDITIVEIDENTITY
using System;
using System.Numerics;

namespace Smdn;

partial struct TUInt24n : IAdditiveIdentity<TUInt24n, TUInt24n> {
  static TUInt24n IAdditiveIdentity<TUInt24n, TUInt24n>.AdditiveIdentity => Zero;
}
#endif
