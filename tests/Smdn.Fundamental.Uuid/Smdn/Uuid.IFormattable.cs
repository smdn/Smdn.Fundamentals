// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

partial class UuidTests {
  [Test]
  public void TestToString1()
  {
    Assert.AreEqual("00000000-0000-0000-0000-000000000000", Uuid.Nil.ToString());
    Assert.AreEqual("6ba7b810-9dad-11d1-80b4-00c04fd430c8", Uuid.RFC4122NamespaceDns.ToString());
    Assert.AreEqual("6ba7b811-9dad-11d1-80b4-00c04fd430c8", Uuid.RFC4122NamespaceUrl.ToString());
    Assert.AreEqual("6ba7b812-9dad-11d1-80b4-00c04fd430c8", Uuid.RFC4122NamespaceIsoOid.ToString());
    Assert.AreEqual("6ba7b814-9dad-11d1-80b4-00c04fd430c8", Uuid.RFC4122NamespaceX500.ToString());

    Assert.AreEqual("00000000-0000-0000-0000-000000000000", Uuid.Nil.ToString(null, null));
    Assert.AreEqual("00000000-0000-0000-0000-000000000000", Uuid.Nil.ToString(string.Empty, null));
    Assert.AreEqual("00000000000000000000000000000000", Uuid.Nil.ToString("N", null));
    Assert.AreEqual("00000000-0000-0000-0000-000000000000", Uuid.Nil.ToString("D", null));
    Assert.AreEqual("{00000000-0000-0000-0000-000000000000}", Uuid.Nil.ToString("B", null));
    Assert.AreEqual("(00000000-0000-0000-0000-000000000000)", Uuid.Nil.ToString("P", null));
    Assert.AreEqual("{0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}", Uuid.Nil.ToString("X", null));

    Assert.Throws<FormatException>(() => Uuid.Nil.ToString("Z", null));
  }

  [Test]
  public void TestToString2()
  {
    var guid = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
    var uuid = new Uuid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");

    Assert.AreEqual(guid.ToString(), uuid.ToString());
    Assert.AreEqual(guid.ToString("N"), uuid.ToString("N"), "format = N");
    Assert.AreEqual(guid.ToString("D"), uuid.ToString("D"), "format = D");
    Assert.AreEqual(guid.ToString("B"), uuid.ToString("B"), "format = B");
    Assert.AreEqual(guid.ToString("P"), uuid.ToString("P"), "format = P");
    Assert.AreEqual(guid.ToString("X"), uuid.ToString("X"), "format = X");
  }

  [Test]
  public void TestToString3()
  {
    var guid = new Guid(new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8});
    var uuid = new Uuid(new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8});

    Assert.AreEqual(guid.ToString(), uuid.ToString());
    Assert.AreEqual(guid.ToString("N"), uuid.ToString("N"), "format = N");
    Assert.AreEqual(guid.ToString("D"), uuid.ToString("D"), "format = D");
    Assert.AreEqual(guid.ToString("B"), uuid.ToString("B"), "format = B");
    Assert.AreEqual(guid.ToString("P"), uuid.ToString("P"), "format = P");
    Assert.AreEqual(guid.ToString("X"), uuid.ToString("X"), "format = X");
  }
}
