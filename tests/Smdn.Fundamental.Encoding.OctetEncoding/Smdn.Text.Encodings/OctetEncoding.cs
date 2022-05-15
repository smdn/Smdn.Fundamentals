// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;

using NUnit.Framework;

namespace Smdn.Text.Encodings;

[TestFixture]
public class OctetEncodingTests {
  [Test]
  public void GetBytes_SevenBits_ValidChar()
  {
    OctetEncoding.SevenBits.GetBytes("0004 append \"INBOX\" (\\Seen) {33}\r\n\x00\x20\x40\x60\x7f");
  }

  [Test]
  public void GetBytes_EightBits_ValidChar()
  {
    OctetEncoding.EightBits.GetBytes("0004 append \"INBOX\" (\\Seen) {33}\r\n\x00\x20\x40\x60\x80\xa0\xc0\xe0\xff");
  }

  [Test]
  public void GetBytes_SevenBits_InvalidChar()
  {
    Assert.Throws<EncoderFallbackException>(() => OctetEncoding.SevenBits.GetBytes("\x20\x40\x60\x80"));
  }

  [Test]
  public void GetBytes_EightBits_InvalidChar()
  {
    Assert.Throws<EncoderFallbackException>(() => OctetEncoding.EightBits.GetBytes("INBOX.日本語"));
  }
}
