// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_NUMERICS_IADDITIVEIDENTITY
using System;
using System.Collections.Generic;
using System.Numerics;

using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class UInt24nTests {
#pragma warning restore IDE0040

  [Test]
  public void IAdditiveIdentity_AdditiveIdentity()
  {
    AssertAdditiveIdentityIsEqualToZero<UInt24>();
    AssertAdditiveIdentityIsEqualToZero<UInt48>();

    static void AssertAdditiveIdentityIsEqualToZero<TUInt24n>()
      where TUInt24n : IAdditiveIdentity<TUInt24n, TUInt24n>, INumber<TUInt24n>
      => Assert.That(
        TUInt24n.AdditiveIdentity,
        Is.EqualTo(TUInt24n.Zero).Using((IComparer<TUInt24n>)Comparer<TUInt24n>.Create((x, y) => x.CompareTo(y))),
        typeof(TUInt24n).FullName
      );
  }
}
#endif
