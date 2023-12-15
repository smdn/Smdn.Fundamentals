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

        Assert.That(regexRandomNode.IsMatch(node.ToString()), Is.True, node.ToString());
      }
    }

#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
    [Test]
    public void TestConstruct_Default()
      => Assert.That(new Node().ToPhysicalAddress(), Is.EqualTo(PhysicalAddress.Parse("00-00-00-00-00-00")));

    [Test]
    public void TestConstruct_FromPhysicalAddress()
    {
      var addr = PhysicalAddress.Parse("01-23-45-67-89-AB");
      var node = new Node(addr);

      Assert.That(node.ToPhysicalAddress(), Is.EqualTo(addr));
    }
#endif

    [Test]
    public void WriteBytes()
    {
      var node = Node.CreateRandom();
      var dest = new byte[7] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

      Assert.DoesNotThrow(() => node.WriteBytes((Span<byte>)dest), "length 7");
      Assert.That(dest, Is.Not.EquivalentTo(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));

      Assert.DoesNotThrow(() => node.WriteBytes(((Span<byte>)dest).Slice(0, 6)), "length 6");
      Assert.That(dest, Is.Not.EquivalentTo(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
    }

    [Test]
    public void TryWriteBytes()
    {
      var node = Node.CreateRandom();
      var dest = new byte[7] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

      Assert.That(node.TryWriteBytes((Span<byte>)dest), Is.True, "length 7");
      Assert.That(dest, Is.Not.EquivalentTo(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));

      Assert.That(node.TryWriteBytes(((Span<byte>)dest).Slice(0, 6)), Is.True, "length 6");
      Assert.That(dest, Is.Not.EquivalentTo(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }));
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

      Assert.That(node.TryWriteBytes(stackalloc byte[5]), Is.False, "length 5");
      Assert.That(node.TryWriteBytes(stackalloc byte[1]), Is.False, "length 1");
      Assert.That(node.TryWriteBytes(stackalloc byte[0]), Is.False, "length 0");
    }
  }
}
