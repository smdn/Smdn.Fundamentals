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
  public class NodeTests {
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

    [Test]
    public void TestParse_ArgumentNull() => Assert.Throws<ArgumentNullException>(() => Node.Parse(null));

    [TestCase("00:00:00:00:00:00", true, "00:00:00:00:00:00")]
    [TestCase("01:23:45:67:89:AB", true, "01:23:45:67:89:AB")]
    [TestCase("FF:FF:FF:FF:FF:FF", true, "FF:FF:FF:FF:FF:FF")]
    [TestCase("ab:cd:ef:AB:CD:EF", true, "ab:cd:ef:AB:CD:EF")]
    [TestCase("0:1:2:3:4:F", true, "00:01:02:03:04:0F")]
    [TestCase("000:001:002:003:004:00F", true, "00:01:02:03:04:0F")]
    [TestCase("00:00:00:00:00", false, null)]
    [TestCase("00", false, null)]
    [TestCase("", false, null)]
    [TestCase("100:00:00:00:00:00", false, null)]
    [TestCase("00:00:00:00:00:100", false, null)]
    [TestCase("00:00:00:00:00:0X", false, null)]
    [TestCase("00-00-00-00-00-00", false, null)]
    public void TestParse(string s, bool expectValid, string expectedString)
    {
      Node n = default;

      if (expectValid)
        Assert.DoesNotThrow(() => n = Node.Parse(s));
      else
        Assert.Throws<FormatException>(() => n = Node.Parse(s));

      if (expectValid) {
        Assert.AreEqual(expectedString.ToUpperInvariant(), n.ToString("X"));
        Assert.AreEqual(expectedString.ToLowerInvariant(), n.ToString("x"));
      }
    }

    [TestCase("00:00:00:00:00:00", true, "00:00:00:00:00:00")]
    [TestCase("01:23:45:67:89:AB", true, "01:23:45:67:89:AB")]
    [TestCase("FF:FF:FF:FF:FF:FF", true, "FF:FF:FF:FF:FF:FF")]
    [TestCase("ab:cd:ef:AB:CD:EF", true, "ab:cd:ef:AB:CD:EF")]
    [TestCase("0:1:2:3:4:F", true, "00:01:02:03:04:0F")]
    [TestCase("000:001:002:003:004:00F", true, "00:01:02:03:04:0F")]
    [TestCase("00:00:00:00:00", false, null)]
    [TestCase("00", false, null)]
    [TestCase("", false, null)]
    [TestCase(null, false, null)]
    [TestCase("100:00:00:00:00:00", false, null)]
    [TestCase("00:00:00:00:00:100", false, null)]
    [TestCase("00:00:00:00:00:0X", false, null)]
    [TestCase("00-00-00-00-00-00", false, null)]
    public void TestTryParse(string s, bool expectValid, string expectedString)
    {
      Assert.AreEqual(expectValid, Node.TryParse(s, out var node));

      if (expectValid) {
        Assert.AreEqual(expectedString.ToUpperInvariant(), node.ToString("X"));
        Assert.AreEqual(expectedString.ToLowerInvariant(), node.ToString("x"));
      }
    }

    [Test]
    public void TestToString()
    {
      var regexFormat_X = new Regex("^[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}$");
      var regexFormat_x = new Regex("^[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}:[0-9a-f]{2}$");
      var regexFormat_Default = regexFormat_X;

      for (var n = 0; n < 1000; n++) {
        var node = Node.CreateRandom();

        Assert.IsTrue(regexFormat_Default.IsMatch(node.ToString()), node.ToString());
        Assert.IsTrue(regexFormat_Default.IsMatch(node.ToString(null)), node.ToString(null));
        Assert.IsTrue(regexFormat_Default.IsMatch(node.ToString(string.Empty)), node.ToString(string.Empty));
        Assert.IsTrue(regexFormat_Default.IsMatch(node.ToString(null, formatProvider: null)), node.ToString(null, formatProvider: null));
        Assert.IsTrue(regexFormat_X.IsMatch(node.ToString("X")), node.ToString("X"));
        Assert.IsTrue(regexFormat_x.IsMatch(node.ToString("x")), node.ToString("x"));
      }
    }

    [Test]
    public void TestToString_InvalidFormat()
    {
      var node = Node.CreateRandom();

      Assert.Throws<FormatException>(() => node.ToString("n"));
      Assert.Throws<FormatException>(() => node.ToString("xx"));
      Assert.Throws<FormatException>(() => node.ToString("XX"));
    }
  }
}
