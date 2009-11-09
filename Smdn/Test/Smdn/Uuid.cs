using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class UuidTests {
    [Test]
    public void TestConstruct()
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
    public void TestConstructFromGuid()
    {
      Assert.AreEqual(Uuid.Nil, new Uuid(Guid.Empty));
      Assert.AreEqual(Uuid.RFC4122NamespaceDns, new Uuid(new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8")));
    }

    [Test]
    public void TestConstructFromUrn()
    {
      Assert.AreEqual(new Uuid("f81d4fae-7dec-11d0-a765-00a0c91e6bf6"), new Uuid(new Uri("urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6")));
      Assert.AreEqual(new Uuid("f81d4fae-7dec-11d0-a765-00a0c91e6bf6"), new Uuid(new Urn("urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6")));
    }

    [Test]
    public void TestCreateTimeBased()
    {
      var uuid = Uuid.CreateTimeBased((new DateTime(1997, 2, 3, 17, 43, 12, 216, DateTimeKind.Utc)).AddTicks(8750), 10085, new byte[] {0x00, 0xa0, 0xc9, 0x1e, 0x6b, 0xf6});

      Assert.AreEqual(new Uuid("f81d4fae-7dec-11d0-a765-00a0c91e6bf6"), uuid);
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField);
      Assert.AreEqual(UuidVersion.Version1, uuid.Version);
    }

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
    public void TestToString()
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

      try {
        Uuid.Nil.ToString("X", null);
        Assert.Fail("FormatException not thrown");
      }
      catch (FormatException) {
      }
    }

    [Test]
    public void TestToByteArray()
    {
      Assert.AreEqual(BitConverter.ToString(new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8}),
                      BitConverter.ToString(Uuid.RFC4122NamespaceDns.ToByteArray()));
    }

    [Test]
    public void TestToUrn()
    {
      Assert.AreEqual(new Urn("urn:uuid:00000000-0000-0000-0000-000000000000"), Uuid.Nil.ToUrn());
      Assert.AreEqual(new Urn("urn:uuid:6ba7b810-9dad-11d1-80b4-00c04fd430c8"), Uuid.RFC4122NamespaceDns.ToUrn());
    }

    [Test]
    public void TestToGuid()
    {
      Assert.AreEqual(Guid.Empty, Uuid.Nil.ToGuid());
      Assert.AreEqual(new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"), Uuid.RFC4122NamespaceDns.ToGuid());
    }

    [Test]
    public void TestImplicitConversionOperator()
    {
      Guid guid = Uuid.Nil;

      Assert.AreEqual(guid, Guid.Empty);

      Uuid uuid = Guid.Empty;

      Assert.AreEqual(uuid, Uuid.Nil);
    }

    [Test]
    public void TestEqualityOperator()
    {
      Assert.IsTrue(Uuid.Nil == Uuid.Nil);
      Assert.IsFalse(Uuid.Nil != Uuid.Nil);
      Assert.IsFalse(Uuid.Nil == Uuid.RFC4122NamespaceDns);
      Assert.IsTrue(Uuid.Nil != Uuid.RFC4122NamespaceDns);
    }
  }
}