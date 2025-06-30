// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IEQUALITYOPERATORS
using System.Numerics;

#endif
using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class UuidTests {
#pragma warning restore IDE0040
  [Test]
  public void TestEquals()
  {
    Assert.That(Uuid.RFC4122NamespaceDns.Equals(Uuid.RFC4122NamespaceDns), Is.True);
    Assert.That(Uuid.RFC4122NamespaceDns.Equals((Guid)Uuid.RFC4122NamespaceDns), Is.True);
    Assert.That(Uuid.RFC4122NamespaceDns.Equals((object)Uuid.RFC4122NamespaceDns), Is.True);
    Assert.That(Uuid.RFC4122NamespaceDns.Equals(null), Is.False);
    Assert.That(Uuid.RFC4122NamespaceDns.Equals(1), Is.False);

    var u0 = new Uuid("00000000-0000-0000-0000-000000000000");
    var g0 = new Guid("00000000-0000-0000-0000-000000000000");
    var u1 = new Uuid("00000001-0000-0000-0000-000000000000");
    var g1 = new Guid("00000001-0000-0000-0000-000000000000");
    var u2 = new Uuid("00000000-0000-0000-0000-000000000001");
    var g2 = new Guid("00000000-0000-0000-0000-000000000001");
    object o;

    Assert.That(u0.Equals(u0), Is.True);
    Assert.That(u0.Equals(g0), Is.True);

    o = u0; Assert.That(u0.Equals(o), Is.True);
    o = g0; Assert.That(u0.Equals(o), Is.True);

    Assert.That(u0.Equals(u1), Is.False);
    Assert.That(u0.Equals(g1), Is.False);

    o = u1; Assert.That(u0.Equals(o), Is.False);
    o = g1; Assert.That(u0.Equals(o), Is.False);

    Assert.That(u0.Equals(u2), Is.False);
    Assert.That(u0.Equals(g2), Is.False);

    o = u2; Assert.That(u0.Equals(o), Is.False);
    o = g2; Assert.That(u0.Equals(o), Is.False);
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
    Assert.That(new Uuid(x) == new Uuid(y), Is.EqualTo(isEqual), "op ==");
    Assert.That(new Uuid(x) != new Uuid(y), Is.EqualTo(!isEqual), "op !=");

#if SYSTEM_NUMERICS_IEQUALITYOPERATORS
    Assert.That(OpEquality(new Uuid(x), new Uuid(y)), Is.EqualTo(isEqual), "IEqualityOperators ==");
    Assert.That(OpInequality(new Uuid(x), new Uuid(y)), Is.EqualTo(!isEqual), "IEqualityOperators !=");

    static bool OpEquality<T>(T x, T y) where T : IEqualityOperators<T, T, bool> => x == y;
    static bool OpInequality<T>(T x, T y) where T : IEqualityOperators<T, T, bool> => x != y;
#endif
  }
}
