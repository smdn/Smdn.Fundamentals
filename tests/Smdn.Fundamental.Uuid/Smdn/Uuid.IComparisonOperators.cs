// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

partial class UuidTests {
  [Test]
  public void TestCompareTo()
  {
    Assert.AreEqual(0, Uuid.RFC4122NamespaceDns.CompareTo(Uuid.RFC4122NamespaceDns));
    Assert.AreEqual(0, Uuid.RFC4122NamespaceDns.CompareTo((Guid)Uuid.RFC4122NamespaceDns));
    Assert.AreEqual(0, Uuid.RFC4122NamespaceDns.CompareTo((object)Uuid.RFC4122NamespaceDns));
    Assert.AreEqual(1, Uuid.RFC4122NamespaceDns.CompareTo(null));
    Assert.AreNotEqual(0, Uuid.RFC4122NamespaceDns.CompareTo(Guid.Empty));

    Assert.Throws<ArgumentException>(() => Uuid.RFC4122NamespaceDns.CompareTo(1));

    var ux = new Uuid("00000000-0000-0000-0000-000000000000");
    var uy = new Uuid("00000001-0000-0000-0000-000000000000");
    var gx = new Guid("00000000-0000-0000-0000-000000000000");
    var gy = new Guid("00000001-0000-0000-0000-000000000000");

    Assert.That(ux.CompareTo(ux) == 0);
    Assert.That(ux.CompareTo(gx) == 0);
    Assert.That(uy.CompareTo(uy) == 0);
    Assert.That(gy.CompareTo(gy) == 0);

    Assert.That(ux.CompareTo(uy) < 0);
    Assert.That(ux.CompareTo(gy) < 0);
    Assert.That(0 < uy.CompareTo(ux));
    Assert.That(0 < uy.CompareTo(gx));

    ux = new Uuid("00000000-0000-0000-0000-000000000000");
    uy = new Uuid("00000000-0000-0000-0000-000000000001");
    gx = new Guid("00000000-0000-0000-0000-000000000000");
    gy = new Guid("00000000-0000-0000-0000-000000000001");

    Assert.That(ux.CompareTo(ux) == 0);
    Assert.That(ux.CompareTo(gx) == 0);
    Assert.That(uy.CompareTo(uy) == 0);
    Assert.That(gy.CompareTo(gy) == 0);

    Assert.That(ux.CompareTo(uy) < 0);
    Assert.That(ux.CompareTo(gy) < 0);
    Assert.That(0 < uy.CompareTo(ux));
    Assert.That(0 < uy.CompareTo(gx));
  }

  [Test]
  public void TestGreaterThanLessThanOperator()
  {
    var x = new Uuid("00000000-0000-0000-0000-000000000000");
    var y = new Uuid("00000001-0000-0000-0000-000000000000");

    Assert.IsTrue(x < y, "x < y");
    Assert.IsFalse(x > y, "x > y");

    x = new Uuid("00000000-0000-0000-0000-000000000000");
    y = new Uuid("00000000-0000-0000-0000-000000000001");

    Assert.IsTrue(x < y, "x < y");
    Assert.IsFalse(x > y, "x > y");
  }
}
