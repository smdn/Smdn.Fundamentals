// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if FEATURE_GENERIC_MATH
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn;

partial class UInt24nTests {
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
