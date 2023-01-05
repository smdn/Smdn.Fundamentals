// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif
using NUnit.Framework;

namespace Smdn;

partial class UuidTests {
  [Test]
  public void TestEquals()
  {
    Assert.IsTrue(Uuid.RFC4122NamespaceDns.Equals(Uuid.RFC4122NamespaceDns));
    Assert.IsTrue(Uuid.RFC4122NamespaceDns.Equals((Guid)Uuid.RFC4122NamespaceDns));
    Assert.IsTrue(Uuid.RFC4122NamespaceDns.Equals((object)Uuid.RFC4122NamespaceDns));
    Assert.IsFalse(Uuid.RFC4122NamespaceDns.Equals(null));
    Assert.IsFalse(Uuid.RFC4122NamespaceDns.Equals(1));

    var u0 = new Uuid("00000000-0000-0000-0000-000000000000");
    var g0 = new Guid("00000000-0000-0000-0000-000000000000");
    var u1 = new Uuid("00000001-0000-0000-0000-000000000000");
    var g1 = new Guid("00000001-0000-0000-0000-000000000000");
    var u2 = new Uuid("00000000-0000-0000-0000-000000000001");
    var g2 = new Guid("00000000-0000-0000-0000-000000000001");
    object o;

    Assert.IsTrue(u0.Equals(u0));
    Assert.IsTrue(u0.Equals(g0));

    o = u0; Assert.IsTrue(u0.Equals(o));
    o = g0; Assert.IsTrue(u0.Equals(o));

    Assert.IsFalse(u0.Equals(u1));
    Assert.IsFalse(u0.Equals(g1));

    o = u1; Assert.IsFalse(u0.Equals(o));
    o = g1; Assert.IsFalse(u0.Equals(o));

    Assert.IsFalse(u0.Equals(u2));
    Assert.IsFalse(u0.Equals(g2));

    o = u2; Assert.IsFalse(u0.Equals(o));
    o = g2; Assert.IsFalse(u0.Equals(o));
  }

  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000",  true)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000001", false)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0001-000000000000", false)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0100-000000000000", false)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0001-0000-000000000000", false)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0001-0000-0000-000000000000", false)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000001-0000-0000-0000-000000000000", false)]
  [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF",  true)]
  public void TestOpEquality_OpInequality(string x, string y, bool isEqual)
  {
    Assert.AreEqual(isEqual, new Uuid(x) == new Uuid(y), "op ==");
    Assert.AreEqual(!isEqual, new Uuid(x) != new Uuid(y), "op !=");

#if FEATURE_GENERIC_MATH
    Assert.AreEqual(isEqual, OpEquality(new Uuid(x), new Uuid(y)), "IEqualityOperators ==");
    Assert.AreEqual(!isEqual, OpInequality(new Uuid(x), new Uuid(y)), "IEqualityOperators !=");

    static bool OpEquality<T>(T x, T y) where T : IEqualityOperators<T, T, bool> => x == y;
    static bool OpInequality<T>(T x, T y) where T : IEqualityOperators<T, T, bool> => x != y;
#endif
  }
}
