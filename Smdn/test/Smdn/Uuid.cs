using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class UuidTests {
    [Test]
    public void TestSizeOfUuid()
    {
      Assert.AreEqual(Marshal.SizeOf(typeof(Guid)),
                      Marshal.SizeOf(typeof(Uuid)));
    }

    [Test]
    public void TestConstruct1()
    {
      var uuid = new Uuid("f81d4fae-7dec-11d0-a765-00a0c91e6bf6");

      Assert.AreEqual(0xf81d4fae, uuid.TimeLow);
      Assert.AreEqual(0x7dec, uuid.TimeMid);
      Assert.AreEqual(0x11d0, uuid.TimeHighAndVersion);
      Assert.AreEqual(0xa7, uuid.ClockSeqHighAndReserved);
      Assert.AreEqual(0x65, uuid.ClockSeqLow);
      Assert.AreEqual(new byte[] {0x00, 0xa0, 0xc9, 0x1e, 0x6b, 0xf6}, uuid.Node);
      Assert.AreEqual(UuidVersion.Version1, uuid.Version);
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField);

      Assert.AreEqual((new DateTime(1997, 2, 3, 17, 43, 12, 216, DateTimeKind.Utc)).AddTicks(8750), uuid.Timestamp);
      Assert.AreEqual(10085, uuid.Clock);
      Assert.AreEqual("00:a0:c9:1e:6b:f6", uuid.IEEE802MacAddress);
    }

    [Test]
    public void TestConstruct2()
    {
      var uuid = new Uuid("01c47915-4777-11d8-bc70-0090272ff725");

      Assert.AreEqual(0x01c47915, uuid.TimeLow);
      Assert.AreEqual(0x4777, uuid.TimeMid);
      Assert.AreEqual(0x11d8, uuid.TimeHighAndVersion);
      Assert.AreEqual(0xbc, uuid.ClockSeqHighAndReserved);
      Assert.AreEqual(0x70, uuid.ClockSeqLow);
      Assert.AreEqual(new byte[] {0x00, 0x90, 0x27, 0x2f, 0xf7, 0x25}, uuid.Node);
      Assert.AreEqual(UuidVersion.Version1, uuid.Version);
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField);

      Assert.AreEqual((new DateTime(2004, 1, 15, 16, 22, 26, 376, DateTimeKind.Utc)).AddTicks(3221), uuid.Timestamp);
      Assert.AreEqual(15472, uuid.Clock);
      Assert.AreEqual("00:90:27:2f:f7:25", uuid.IEEE802MacAddress);
#if NETFRAMEWORK || NETSTANDARD2_0
      Assert.AreEqual(PhysicalAddress.Parse("00-90-27-2F-F7-25"), uuid.PhysicalAddress);
#endif
    }

    [Test]
    public void TestConstruct3()
    {
      var uuid = new Uuid("01c47915-4777-11d8-bc70-0090272ff725");
      var guid = new Guid("01c47915-4777-11d8-bc70-0090272ff725");

      Assert.IsTrue(uuid.Equals(guid), "construct from string");

      uuid = new Uuid(new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8});
      guid = new Guid(new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8});

      Assert.IsTrue(uuid.Equals(guid), "construct from byte array");
    }

    [Test]
    public void TestConstructFromGuid()
    {
      Assert.AreEqual(Uuid.Nil, new Uuid(Guid.Empty));
      Assert.AreEqual(Uuid.RFC4122NamespaceDns, new Uuid(new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8")));
    }

    [Test]
    public void TestCreateTimeBased1()
    {
      var uuid = Uuid.CreateTimeBased((new DateTime(1997, 2, 3, 17, 43, 12, 216, DateTimeKind.Utc)).AddTicks(8750), 10085, new byte[] {0x00, 0xa0, 0xc9, 0x1e, 0x6b, 0xf6});

      Assert.AreEqual(new Uuid("f81d4fae-7dec-11d0-a765-00a0c91e6bf6"), uuid);
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField);
      Assert.AreEqual(UuidVersion.Version1, uuid.Version);
    }

#if NETFRAMEWORK || NETSTANDARD2_0
    [Test]
    public void TestCreateTimeBased2()
    {
      var node = new PhysicalAddress(new byte[] {0x00, 0x90, 0x27, 0x2f, 0xf7, 0x25});
      var uuid = Uuid.CreateTimeBased((new DateTime(2004, 1, 15, 16, 22, 26, 376, DateTimeKind.Utc)).AddTicks(3221), 15472, node);

      Assert.AreEqual(new Uuid("01c47915-4777-11d8-bc70-0090272ff725"), uuid);
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField);
      Assert.AreEqual(UuidVersion.Version1, uuid.Version);
    }

    [Test]
    public void TestCreateTimeBased3()
    {
      var uuid = Uuid.CreateTimeBased((new DateTime(2009, 3, 4, 1, 3, 25, DateTimeKind.Utc)), 0);

      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField);
      Assert.AreEqual(UuidVersion.Version1, uuid.Version);
      Assert.AreNotEqual(PhysicalAddress.None, uuid.PhysicalAddress);
    }
#endif

    [Test]
    public void TestCreateNameBasedMD5()
    {
      var uuid = Uuid.CreateNameBasedMD5("www.widgets.com", Uuid.Namespace.RFC4122Dns);

      Assert.AreEqual(new Uuid("3d813cbb-47fb-32ba-91df-831e1593ac29"), uuid);
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField);
      Assert.AreEqual(UuidVersion.Version3, uuid.Version);

      uuid = Uuid.CreateNameBasedMD5("python.org", Uuid.Namespace.RFC4122Dns);

      Assert.AreEqual(new Uuid("6fa459ea-ee8a-3ca4-894e-db77e160355e"), uuid, "Python uuid; http://pythonjp.sourceforge.jp/dev/library/uuid.html");
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField);
      Assert.AreEqual(UuidVersion.Version3, uuid.Version);
    }

    [Test]
    public void TestCreateNameBasedSHA1()
    {
      var uuid = Uuid.CreateNameBasedSHA1("python.org", Uuid.Namespace.RFC4122Dns);

      Assert.AreEqual(new Uuid("886313e1-3b8a-5372-9b90-0c9aee199e5d"), uuid, "Python uuid; http://pythonjp.sourceforge.jp/dev/library/uuid.html");
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField);
      Assert.AreEqual(UuidVersion.Version5, uuid.Version);
    }

    [Test]
    public void TestCreateRandom()
    {
      var uuids = new List<Uuid>();

      uuids.Add(Uuid.CreateFromRandomNumber());

      for (var i = 0; i < 10; i++) {
        var uuid = Uuid.CreateFromRandomNumber();

        Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField);
        Assert.AreEqual(UuidVersion.Version4, uuid.Version);

        foreach (var created in uuids) {
          Assert.AreNotEqual(created, uuid);
        }

        uuids.Add(uuid);
      }
    }

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

    [Test]
    public void TestToByteArray1()
    {
      var expectedBigEndian     = new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8};
      var expectedLittleEndian  = new byte[] {0x10, 0xb8, 0xa7, 0x6b, 0xad, 0x9d, 0xd1, 0x11, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8};

      Assert.AreEqual(BitConverter.ToString(BitConverter.IsLittleEndian ? expectedLittleEndian : expectedBigEndian),
                      BitConverter.ToString(Uuid.RFC4122NamespaceDns.ToByteArray()));

      Assert.AreEqual(BitConverter.ToString(expectedBigEndian),
                      BitConverter.ToString(Uuid.RFC4122NamespaceDns.ToByteArray(Endianness.BigEndian)));
      Assert.AreEqual(BitConverter.ToString(expectedLittleEndian),
                      BitConverter.ToString(Uuid.RFC4122NamespaceDns.ToByteArray(Endianness.LittleEndian)));
    }

    [Test]
    public void TestToByteArray2()
    {
      var guid = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
      var uuid = new Uuid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");

      Assert.AreEqual(BitConverter.ToString(guid.ToByteArray()),
                      BitConverter.ToString(uuid.ToByteArray()));
    }

    [Test]
    public void TestGetBytes()
    {
      byte[] buffer = new byte[18] {
        0xcc,
        0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00,
        0xcc
      };

      Uuid.RFC4122NamespaceDns.GetBytes(buffer, 1, Endianness.BigEndian);

      CollectionAssert.AreEqual(new[] {0xcc, 0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8, 0xcc},
                                buffer);

      Uuid.RFC4122NamespaceDns.GetBytes(buffer, 1, Endianness.LittleEndian);

      CollectionAssert.AreEqual(new[] {0xcc, 0x10, 0xb8, 0xa7, 0x6b, 0xad, 0x9d, 0xd1, 0x11, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8, 0xcc},
                                buffer);

      Assert.Throws<ArgumentNullException>(() => Uuid.RFC4122NamespaceDns.GetBytes(null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => Uuid.RFC4122NamespaceDns.GetBytes(new byte[15], -1));
      Assert.Throws<ArgumentException>(() => Uuid.RFC4122NamespaceDns.GetBytes(new byte[15], 0));
      Assert.Throws<ArgumentException>(() => Uuid.RFC4122NamespaceDns.GetBytes(new byte[16], 1));
    }

    [Test]
    public void TestToGuid()
    {
      Assert.AreEqual(Guid.Empty, Uuid.Nil.ToGuid());
      Assert.AreEqual(new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"), Uuid.RFC4122NamespaceDns.ToGuid());
    }

    [Test]
    public void TestExplicitConversionOperator()
    {
      Guid guid = (Guid)Uuid.Nil;

      Assert.AreEqual(guid, Guid.Empty);

      Uuid uuid = (Uuid)Guid.Empty;

      Assert.AreEqual(uuid, Uuid.Nil);
    }

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

    [Test]
    public void TestCompareTo()
    {
      Assert.AreEqual(0, Uuid.RFC4122NamespaceDns.CompareTo(Uuid.RFC4122NamespaceDns));
      Assert.AreEqual(1, Uuid.RFC4122NamespaceDns.CompareTo(null));
      Assert.AreNotEqual(0, Uuid.RFC4122NamespaceDns.CompareTo(Guid.Empty));

      Assert.Throws<ArgumentException>(() => Uuid.RFC4122NamespaceDns.CompareTo(1));

      var ux = new Uuid("00000000-0000-0000-0000-000000000000");
      var uy = new Uuid("00000001-0000-0000-0000-000000000000");
      var gx = new Guid("00000000-0000-0000-0000-000000000000");
      var gy = new Guid("00000001-0000-0000-0000-000000000000");

      Assert.That(ux.CompareTo(uy) < 0);
      Assert.That(ux.CompareTo(gy) < 0);
      Assert.That(0 < uy.CompareTo(ux));
      Assert.That(0 < uy.CompareTo(gx));

      ux = new Uuid("00000000-0000-0000-0000-000000000000");
      uy = new Uuid("00000000-0000-0000-0000-000000000001");
      gx = new Guid("00000000-0000-0000-0000-000000000000");
      gy = new Guid("00000000-0000-0000-0000-000000000001");

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
}
