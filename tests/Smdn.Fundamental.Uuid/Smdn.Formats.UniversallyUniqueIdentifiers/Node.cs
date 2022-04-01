// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
using System.Net.NetworkInformation;
#endif
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Smdn.Formats.UniversallyUniqueIdentifiers {
  [TestFixture]
  public partial class NodeTests {
    [Test]
    public void TestCreateRandom()
    {
      var regexRandomNode = new Regex("^[0-9A-F][13579BDF]:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}$");

      for (var n = 0; n < 1000; n++) {
        var node = Node.CreateRandom();

        Assert.IsTrue(regexRandomNode.IsMatch(node.ToString()), node.ToString());
      }
    }

#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
    [Test]
    public void TestConstruct_Default()
      => Assert.AreEqual(PhysicalAddress.Parse("00-00-00-00-00-00"), new Node().ToPhysicalAddress());

    [Test]
    public void TestConstruct_FromPhysicalAddress()
    {
      var addr = PhysicalAddress.Parse("01-23-45-67-89-AB");
      var node = new Node(addr);

      Assert.AreEqual(addr, node.ToPhysicalAddress());
    }
#endif

    [Test]
    public void WriteBytes()
    {
      var node = Node.CreateRandom();
      var dest = new byte[7] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

      Assert.DoesNotThrow(() => node.WriteBytes((Span<byte>)dest), "length 7");
      CollectionAssert.AreNotEquivalent(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, dest);

      Assert.DoesNotThrow(() => node.WriteBytes(((Span<byte>)dest).Slice(0, 6)), "length 6");
      CollectionAssert.AreNotEquivalent(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, dest);
    }

    [Test]
    public void TryWriteBytes()
    {
      var node = Node.CreateRandom();
      var dest = new byte[7] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

      Assert.IsTrue(node.TryWriteBytes((Span<byte>)dest), "length 7");
      CollectionAssert.AreNotEquivalent(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, dest);

      Assert.IsTrue(node.TryWriteBytes(((Span<byte>)dest).Slice(0, 6)), "length 6");
      CollectionAssert.AreNotEquivalent(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, dest);
    }

    [Test]
    public void WriteBytes_DestinationLengthTooShort()
    {
      var node = Node.CreateRandom();

      Assert.Throws<ArgumentException>(() => node.WriteBytes(stackalloc byte[5]), "length 5");
      Assert.Throws<ArgumentException>(() => node.WriteBytes(stackalloc byte[1]), "length 1");
      Assert.Throws<ArgumentException>(() => node.WriteBytes(stackalloc byte[0]), "length 0");
    }

    [Test]
    public void TryWriteBytes_DestinationLengthTooShort()
    {
      var node = Node.CreateRandom();

      Assert.IsFalse(node.TryWriteBytes(stackalloc byte[5]), "length 5");
      Assert.IsFalse(node.TryWriteBytes(stackalloc byte[1]), "length 1");
      Assert.IsFalse(node.TryWriteBytes(stackalloc byte[0]), "length 0");
    }
  }
}
