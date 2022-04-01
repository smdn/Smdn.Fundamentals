// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

partial class UuidTests {

  [Test]
  public void TestEqualityOperator()
  {
#pragma warning disable 1718
    Assert.IsTrue(Uuid.Nil == Uuid.Nil);
    Assert.IsFalse(Uuid.Nil != Uuid.Nil);
#pragma warning restore 1718
    Assert.IsFalse(Uuid.Nil == Uuid.RFC4122NamespaceDns);
    Assert.IsTrue(Uuid.Nil != Uuid.RFC4122NamespaceDns);
  }

  [Test]
  public void TestEquals()
  {
    Assert.IsTrue(Uuid.RFC4122NamespaceDns.Equals(Uuid.RFC4122NamespaceDns));
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
}
