// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace Smdn.Text.Encodings;

[TestFixture]
public class OctetEncodingTests {
  [Test]
  public void GetBytes_SevenBits()
  {
    var input = new string(Enumerable.Range(0x00, 0x80).Select(static i => (char)i).ToArray());
    var expected = Enumerable.Range(0x00, 0x80).Select(static i => (byte)i).ToArray();

    CollectionAssert.AreEqual(expected, OctetEncoding.SevenBits.GetBytes(input));
  }

  [Test]
  public void GetBytes_EightBits()
  {
    var input = new string(Enumerable.Range(0x00, 0x100).Select(static i => (char)i).ToArray());
    var expected = Enumerable.Range(0x00, 0x100).Select(static i => (byte)i).ToArray();

    CollectionAssert.AreEqual(expected, OctetEncoding.EightBits.GetBytes(input));
  }

  [TestCase("\u0000", 0x00)]
  [TestCase("\u0020", 0x20)]
  [TestCase("\u007f", 0x7f)]
  public void GetBytes_SevenBits_ValidChar(string input, byte expected)
  {
    var bytes = OctetEncoding.SevenBits.GetBytes(input);

    CollectionAssert.AreEqual(new[] { expected }, bytes);
  }

  [Test]
  public void GetBytes_SevenBits_ValidChar()
  {
    OctetEncoding.SevenBits.GetBytes("0004 append \"INBOX\" (\\Seen) {33}\r\n\x00\x20\x40\x60\x7f");
  }

  [TestCase("\u0000", 0x00)]
  [TestCase("\u0020", 0x20)]
  [TestCase("\u007f", 0x7f)]
  [TestCase("\u0080", 0x80)]
  [TestCase("\u00ff", 0xff)]
  public void GetBytes_EightBits_ValidChar(string input, byte expected)
  {
    var bytes = OctetEncoding.EightBits.GetBytes(input);

    CollectionAssert.AreEqual(new[] { expected }, bytes);
  }

  [Test]
  public void GetBytes_EightBits_ValidChar()
  {
    OctetEncoding.EightBits.GetBytes("0004 append \"INBOX\" (\\Seen) {33}\r\n\x00\x20\x40\x60\x80\xa0\xc0\xe0\xff");
  }

  [TestCase("\u0080")]
  [TestCase("\u00ff")]
  [TestCase("\uffff")]
  [TestCase("日本語")]
  [TestCase("😩")]
  public void GetBytes_SevenBits_InvalidChar(string input)
    => Assert.Throws<EncoderFallbackException>(() => OctetEncoding.SevenBits.GetBytes(input));

  [Test]
  public void GetBytes_SevenBits_InvalidChar()
  {
    Assert.Throws<EncoderFallbackException>(() => OctetEncoding.SevenBits.GetBytes("\x20\x40\x60\x80"));
  }

  [TestCase("\u0100")]
  [TestCase("\uffff")]
  [TestCase("日本語")]
  [TestCase("😩")]
  public void GetBytes_EightBits_InvalidChar(string input)
    => Assert.Throws<EncoderFallbackException>(() => OctetEncoding.EightBits.GetBytes(input));

  [Test]
  public void GetBytes_EightBits_InvalidChar()
  {
    Assert.Throws<EncoderFallbackException>(() => OctetEncoding.EightBits.GetBytes("INBOX.日本語"));
  }
}
