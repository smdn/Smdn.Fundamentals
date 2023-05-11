// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public partial class UuidTests {
    [Test]
    public void TestSizeOfUuid()
    {
      Assert.AreEqual(Marshal.SizeOf(typeof(Guid)),
                      Marshal.SizeOf(typeof(Uuid)));
    }

    private static System.Collections.IEnumerable YieldUuid()
    {
      yield return new object[] { "#1", new Uuid(0xF81D4FAE, 0x7DEC, 0x11D0, 0xA7, 0x65, 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6) };
      yield return new object[] { "#2", new Uuid(0xF81D4FAE, 0x7DEC, 0x11D0, 0xA7, 0x65, new byte[] { 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6 }) };
      yield return new object[] { "#3", new Uuid(0xF81D4FAE, 0x7DEC, 0x11D0, 0xA7, 0x65, stackalloc byte[] { 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6 }) };
#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
      yield return new object[] { "#4", new Uuid(0xF81D4FAE, 0x7DEC, 0x11D0, 0xA7, 0x65, PhysicalAddress.Parse("00-A0-C9-1E-6B-F6")) };
#endif
      yield return new object[] { "#5", new Uuid(new Guid("f81d4fae-7dec-11d0-a765-00a0c91e6bf6")) };
      yield return new object[] { "#6", new Uuid(new byte[] { 0xAE, 0x4F, 0x1D, 0xF8, 0xEC, 0x7D, 0xD0, 0x11, 0xA7, 0x65, 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6 }, isBigEndian: false) };
      yield return new object[] { "#7", new Uuid(new byte[] { 0xF8, 0x1D, 0x4F, 0xAE, 0x7D, 0xEC, 0x11, 0xD0, 0xA7, 0x65, 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6 }, isBigEndian: true) };
      yield return new object[] { "#8", new Uuid(
        BitConverter.IsLittleEndian
          ? new byte[] { 0xAE, 0x4F, 0x1D, 0xF8, 0xEC, 0x7D, 0xD0, 0x11, 0xA7, 0x65, 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6 }
          : new byte[] { 0xF8, 0x1D, 0x4F, 0xAE, 0x7D, 0xEC, 0x11, 0xD0, 0xA7, 0x65, 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6 }
      )};
      yield return new object[] { "#9", new Uuid(new byte[] { 0x00, 0xF8, 0x1D, 0x4F, 0xAE, 0x7D, 0xEC, 0x11, 0xD0, 0xA7, 0x65, 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6 }, index: 1, isBigEndian: true) };
      yield return new object[] { "#10", new Uuid(stackalloc byte[] { 0xAE, 0x4F, 0x1D, 0xF8, 0xEC, 0x7D, 0xD0, 0x11, 0xA7, 0x65, 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6 }, isBigEndian: false) };
      yield return new object[] { "#11", new Uuid(stackalloc byte[] { 0xF8, 0x1D, 0x4F, 0xAE, 0x7D, 0xEC, 0x11, 0xD0, 0xA7, 0x65, 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6 }, isBigEndian: true) };
      yield return new object[] { "#12", new Uuid(
        BitConverter.IsLittleEndian
          ? new byte[] { 0xAE, 0x4F, 0x1D, 0xF8, 0xEC, 0x7D, 0xD0, 0x11, 0xA7, 0x65, 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6 }
          : new byte[] { 0xF8, 0x1D, 0x4F, 0xAE, 0x7D, 0xEC, 0x11, 0xD0, 0xA7, 0x65, 0x00, 0xA0, 0xC9, 0x1E, 0x6B, 0xF6 }
      )};
    }

    [TestCaseSource(nameof(YieldUuid))]
    public void TestConstruct(string testCaseLabel, Uuid uuid)
      => Assert.AreEqual("f81d4fae-7dec-11d0-a765-00a0c91e6bf6", uuid.ToString("D", formatProvider: null), testCaseLabel);

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
#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
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

#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
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

    [Test]
    public void TestCreateTimeBased3_NodeNull_PhysicalAddress()
    {
      PhysicalAddress node = null;

      Assert.Throws<ArgumentNullException>(() => {
        Uuid.CreateTimeBased(new DateTime(2009, 3, 4, 1, 3, 25, DateTimeKind.Utc), 0, node);
      });
    }

    [TestCaseSource(nameof(YieldInvalidLengthOfPhysicalAddressBytes))]
    public void TestCreateTimeBased3_NodeInvalidLength_PhysicalAddress(byte[] address)
    {
      var node = new PhysicalAddress(address);

      Assert.Throws<ArgumentException>(() => {
        Uuid.CreateTimeBased(new DateTime(2009, 3, 4, 1, 3, 25, DateTimeKind.Utc), 0, node);
      });
    }
#endif

    private static System.Collections.IEnumerable YieldInvalidLengthOfPhysicalAddressBytes()
    {
      yield return new object[] { new byte[] { 0 } };
      yield return new object[] { new byte[] { 0, 1, 2, 3, 4 } };
      yield return new object[] { new byte[] { 0, 1, 2, 3, 4, 5, 6 } };
    }

    [Test]
    public void TestCreateTimeBased3_NodeNull_Bytes()
    {
      byte[] node = null;

      Assert.Throws<ArgumentNullException>(() => {
        Uuid.CreateTimeBased(new DateTime(2009, 3, 4, 1, 3, 25, DateTimeKind.Utc), 0, node);
      });
    }

    [TestCaseSource(nameof(YieldInvalidLengthOfPhysicalAddressBytes))]
    public void TestCreateTimeBased3_NodeInvalidLength_Bytes(byte[] node)
    {
      Assert.Throws<ArgumentException>(() => {
        Uuid.CreateTimeBased(new DateTime(2009, 3, 4, 1, 3, 25, DateTimeKind.Utc), 0, node);
      });
    }

    [Test]
    public void TestCreateNameBased_ArgumentNull()
    {
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBased(url: null, UuidVersion.NameBasedMD5Hash), "#1");
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBased(url: null, UuidVersion.NameBasedSHA1Hash), "#2");
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBased(name: (string)null, Uuid.Namespace.RFC4122Url, UuidVersion.NameBasedMD5Hash), "#3");
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBased(name: (string)null, Uuid.Namespace.RFC4122Url, UuidVersion.NameBasedSHA1Hash), "#4");
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBased(name: (byte[])null, Uuid.Namespace.RFC4122Url, UuidVersion.NameBasedMD5Hash), "#5");
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBased(name: (byte[])null, Uuid.Namespace.RFC4122Url, UuidVersion.NameBasedSHA1Hash), "#6");
    }

    [Test]
    public void TestCreateNameBasedMD5_ArgumentNull()
    {
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBasedMD5(url: null), "#1");
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBasedMD5(name: (string)null, Uuid.Namespace.RFC4122Url), "#2");
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBasedMD5(name: (byte[])null, Uuid.Namespace.RFC4122Url), "#3");
    }

    [Test]
    public void TestCreateNameBasedSHA1_ArgumentNull()
    {
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBasedMD5(url: null), "#1");
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBasedMD5(name: (string)null, Uuid.Namespace.RFC4122Url), "#2");
      Assert.Throws<ArgumentNullException>(() => Uuid.CreateNameBasedMD5(name: (byte[])null, Uuid.Namespace.RFC4122Url), "#3");
    }

    [TestCase("www.widgets.com", Uuid.Namespace.RFC4122Dns, "3d813cbb-47fb-32ba-91df-831e1593ac29")]
    [TestCase("python.org", Uuid.Namespace.RFC4122Dns, "6fa459ea-ee8a-3ca4-894e-db77e160355e")] // Python uuid; http://pythonjp.sourceforge.jp/dev/library/uuid.html
    [TestCase("example.com", Uuid.Namespace.RFC4122Dns, "9073926b-929f-31c2-abc9-fad77ae3e8eb")]
    [TestCase("https://example.com/", Uuid.Namespace.RFC4122Url, "b9dcdff8-af4a-365d-8043-0f8361942709")]
    public void TestCreateNameBasedMD5(string name, Uuid.Namespace ns, string expected)
    {
      var uuid = Uuid.CreateNameBasedMD5(name, ns);

      Assert.AreEqual(new Uuid(expected), uuid);
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField, nameof(Uuid.VariantField));
      Assert.AreEqual(UuidVersion.Version3, uuid.Version, nameof(Uuid.Version));
    }

    [TestCase("www.widgets.com", Uuid.Namespace.RFC4122Dns, "21f7f8de-8051-5b89-8680-0195ef798b6a")]
    [TestCase("python.org", Uuid.Namespace.RFC4122Dns, "886313e1-3b8a-5372-9b90-0c9aee199e5d")] // Python uuid; http://pythonjp.sourceforge.jp/dev/library/uuid.html
    [TestCase("example.com", Uuid.Namespace.RFC4122Dns, "cfbff0d1-9375-5685-968c-48ce8b15ae17")]
    [TestCase("https://example.com/", Uuid.Namespace.RFC4122Url, "dd2c1780-811a-5296-81c5-178a0ef488bc")]
    public void TestCreateNameBasedSHA1(string name, Uuid.Namespace ns, string expected)
    {
      var uuid = Uuid.CreateNameBasedSHA1(name, ns);

      Assert.AreEqual(new Uuid(expected), uuid);
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField, nameof(Uuid.VariantField));
      Assert.AreEqual(UuidVersion.Version5, uuid.Version, nameof(Uuid.Version));
    }

    [TestCase("example", "9073926b-929f-31c2-abc9-fad77ae3e8eb", UuidVersion.Version3, "a542de01-bff3-3fe2-b0a1-1adc92c500ee")]
    [TestCase("example", "9073926b-929f-31c2-abc9-fad77ae3e8eb", UuidVersion.Version5, "851de751-8481-5e26-82b9-251f7b6b90ee")]
    [TestCase("", "9073926b-929f-31c2-abc9-fad77ae3e8eb", UuidVersion.Version3, "00d104c4-99c9-3b9d-be62-3db5a4a311ac")]
    [TestCase("", "9073926b-929f-31c2-abc9-fad77ae3e8eb", UuidVersion.Version5, "7091bc9e-8eea-565d-9142-101b307a2d52")]
    public void TestCreateNameBased_NamespaceId(string name, string namespaceId, UuidVersion version, string expected)
    {
      var uuid = Uuid.CreateNameBased(name, new Uuid(namespaceId), version);

      Assert.AreEqual(new Uuid(expected), uuid);
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField, nameof(Uuid.VariantField));
      Assert.AreEqual(version, uuid.Version, nameof(Uuid.Version));
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
    public void TestToByteArray1()
    {
      var expectedBigEndian     = new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8};
      var expectedLittleEndian  = new byte[] {0x10, 0xb8, 0xa7, 0x6b, 0xad, 0x9d, 0xd1, 0x11, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8};

      Assert.AreEqual(BitConverter.ToString(BitConverter.IsLittleEndian ? expectedLittleEndian : expectedBigEndian),
                      BitConverter.ToString(Uuid.RFC4122NamespaceDns.ToByteArray()));

      Assert.AreEqual(BitConverter.ToString(expectedBigEndian),
                      BitConverter.ToString(Uuid.RFC4122NamespaceDns.ToByteArray(asBigEndian: true)));
      Assert.AreEqual(BitConverter.ToString(expectedLittleEndian),
                      BitConverter.ToString(Uuid.RFC4122NamespaceDns.ToByteArray(asBigEndian: false)));
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

      Uuid.RFC4122NamespaceDns.GetBytes(buffer, 1, asBigEndian: true);

      CollectionAssert.AreEqual(new[] {0xcc, 0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8, 0xcc},
                                buffer);

      Uuid.RFC4122NamespaceDns.GetBytes(buffer, 1, asBigEndian: false);

      CollectionAssert.AreEqual(new[] {0xcc, 0x10, 0xb8, 0xa7, 0x6b, 0xad, 0x9d, 0xd1, 0x11, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8, 0xcc},
                                buffer);

      Assert.Throws<ArgumentNullException>(() => Uuid.RFC4122NamespaceDns.GetBytes(null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => Uuid.RFC4122NamespaceDns.GetBytes(new byte[15], -1));
      Assert.Throws<ArgumentException>(() => Uuid.RFC4122NamespaceDns.GetBytes(new byte[15], 0));
      Assert.Throws<ArgumentException>(() => Uuid.RFC4122NamespaceDns.GetBytes(new byte[16], 1));
    }

    [Test]
    public void TestWriteBytes()
    {
      var buffer = new byte[18] {
        0xcc,
        0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00,
        0xcc
      };

      Assert.DoesNotThrow(() => Uuid.RFC4122NamespaceDns.WriteBytes(buffer.AsSpan(1), asBigEndian: true));
      CollectionAssert.AreEqual(
        new byte[] {0xcc, 0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8, 0xcc},
        buffer,
        "#1"
      );

      Assert.DoesNotThrow(() => Uuid.RFC4122NamespaceDns.WriteBytes(buffer, asBigEndian: true));
      CollectionAssert.AreEqual(
        new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8, 0xc8, 0xcc},
        buffer,
        "#2"
      );

      Assert.DoesNotThrow(() => Uuid.RFC4122NamespaceDns.WriteBytes(buffer, asBigEndian: false));
      CollectionAssert.AreEqual(
        new byte[] {0x10, 0xb8, 0xa7, 0x6b, 0xad, 0x9d, 0xd1, 0x11, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8, 0xc8, 0xcc},
        buffer,
        "#3"
      );
    }

    [Test]
    public void TestTryWriteBytes()
    {
      var buffer = new byte[18] {
        0xcc,
        0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00,
        0xcc
      };

      Assert.IsTrue(Uuid.RFC4122NamespaceDns.TryWriteBytes(buffer.AsSpan(1), asBigEndian: true), "#1");
      CollectionAssert.AreEqual(
        new byte[] {0xcc, 0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8, 0xcc},
        buffer,
        "#1"
      );

      Assert.IsTrue(Uuid.RFC4122NamespaceDns.TryWriteBytes(buffer, asBigEndian: true), "#2");
      CollectionAssert.AreEqual(
        new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8, 0xc8, 0xcc},
        buffer,
        "#2"
      );

      Assert.IsTrue(Uuid.RFC4122NamespaceDns.TryWriteBytes(buffer, asBigEndian: false), "#3");
      CollectionAssert.AreEqual(
        new byte[] {0x10, 0xb8, 0xa7, 0x6b, 0xad, 0x9d, 0xd1, 0x11, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8, 0xc8, 0xcc},
        buffer,
        "#3"
      );
    }

    [Test]
    public void TestWriteBytes_DestinationTooShort()
    {
      Assert.Throws<ArgumentException>(() => Uuid.RFC4122NamespaceDns.WriteBytes(new byte[15], asBigEndian: true), "#1");
      Assert.Throws<ArgumentException>(() => Uuid.RFC4122NamespaceDns.WriteBytes(new byte[15], asBigEndian: false), "#2");
      Assert.Throws<ArgumentException>(() => Uuid.RFC4122NamespaceDns.WriteBytes(new byte[0], asBigEndian: false), "#3");
    }

    [Test]
    public void TestTryWriteBytes_DestinationTooShort()
    {
      Assert.IsFalse(Uuid.RFC4122NamespaceDns.TryWriteBytes(new byte[15], asBigEndian: true), "#1");
      Assert.IsFalse(Uuid.RFC4122NamespaceDns.TryWriteBytes(new byte[15], asBigEndian: false), "#2");
      Assert.IsFalse(Uuid.RFC4122NamespaceDns.TryWriteBytes(new byte[0], asBigEndian: false), "#3");
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
  }
}
