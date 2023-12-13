// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Smdn.Test.NUnit;

namespace Smdn.Formats {
  [TestFixture]
  public class Base64Tests {
    [Test]
    public void TestEncode1()
    {
      foreach (var test in new[] {
        new {Data = new byte[] {0xfb},              ExpectedString = "+w==", ExpectedBytes = new byte[] {0x2b, 0x77, 0x3d, 0x3d}},
        new {Data = new byte[] {0xfb, 0xf0},        ExpectedString = "+/A=", ExpectedBytes = new byte[] {0x2b, 0x2f, 0x41, 0x3d}},
        new {Data = new byte[] {0xfb, 0xf0, 0x00},  ExpectedString = "+/AA", ExpectedBytes = new byte[] {0x2b, 0x2f, 0x41, 0x41}},
      }) {
        Assert.That(Base64.GetEncodedString(test.Data), Is.EqualTo(test.ExpectedString));
        Assert.That(Base64.Encode(test.Data), Is.EqualTo(test.ExpectedBytes));
      }
    }

    [Test]
    public void TestDecode1()
    {
      foreach (var test in new[] {
        new {ExpectedString = "\xfb",         ExpectedBytes = new byte[] {0xfb},              Data = "+w=="},
        new {ExpectedString = "\xfb\xf0",     ExpectedBytes = new byte[] {0xfb, 0xf0},        Data = "+/A="},
        new {ExpectedString = "\xfb\xf0\x00", ExpectedBytes = new byte[] {0xfb, 0xf0, 0x00},  Data = "+/AA"},
      }) {
        Assert.That(Base64.GetDecodedString(test.Data, Encodings.Latin1), Is.EqualTo(test.ExpectedString));
        Assert.That(Base64.Decode(test.Data), Is.EqualTo(test.ExpectedBytes));
      }
    }

    [Test]
    public void TestEncode2()
    {
      Assert.That(Base64.Encode(new byte[] {0x62, 0x61, 0x73, 0x65, 0x36, 0x34}), Is.EqualTo(new byte[] {0x59, 0x6d, 0x46, 0x7a, 0x5a, 0x54, 0x59, 0x30}));
      Assert.That(Base64.Encode(new byte[] {0xff, 0x62, 0x61, 0x73, 0x65, 0x36, 0x34, 0xff}, 1, 6), Is.EqualTo(new byte[] {0x59, 0x6d, 0x46, 0x7a, 0x5a, 0x54, 0x59, 0x30}));
    }

    [Test]
    public void TestDecode2()
    {
      Assert.That(Base64.Decode(new byte[] {0x59, 0x6d, 0x46, 0x7a, 0x5a, 0x54, 0x59, 0x30}), Is.EqualTo(new byte[] {0x62, 0x61, 0x73, 0x65, 0x36, 0x34}));
      Assert.That(Base64.Decode(new byte[] {0xff, 0x59, 0x6d, 0x46, 0x7a, 0x5a, 0x54, 0x59, 0x30, 0xff}, 1, 8), Is.EqualTo(new byte[] {0x62, 0x61, 0x73, 0x65, 0x36, 0x34}));
    }

    [Test]
    public void TestEncodeDecodeWithSpecificEncoding()
    {
      foreach (var test in new[] {
        new{PlainText = "漢字abcかな123カナ", Base64Text = "5ryi5a2XYWJj44GL44GqMTIz44Kr44OK", Encoding = (Encoding)Encoding.UTF8},
        new{PlainText = "漢字abcかな123カナ", Base64Text = "tMG7+mFiY6SrpMoxMjOlq6XK", Encoding = Encodings.EucJP},
        new{PlainText = "漢字abcかな123カナ", Base64Text = "GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC", Encoding = Encodings.Jis},
        new{PlainText = "漢字abcかな123カナ", Base64Text = "ir+OmmFiY4KpgsgxMjODSoNp", Encoding = Encodings.ShiftJis},
      }) {
        Assert.That(Base64.GetDecodedString(test.Base64Text, test.Encoding), Is.EqualTo(test.PlainText), test.Encoding.WebName);
        Assert.That(Base64.GetEncodedString(test.PlainText, test.Encoding), Is.EqualTo(test.Base64Text), test.Encoding.WebName);
      }
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestCreateEncodingStream_LeaveStreamOpen(bool leaveStreamOpen)
    {
      using (var stream = new MemoryStream()) {
        using (var s = Base64.CreateEncodingStream(stream, leaveStreamOpen)) {
        }

        if (leaveStreamOpen)
          Assert.DoesNotThrow(() => stream.WriteByte(0));
        else
          Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0));
      }
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestCreateDecodingStream_LeaveStreamOpen(bool leaveStreamOpen)
    {
      using (var stream = new MemoryStream()) {
        using (var s = Base64.CreateDecodingStream(stream, leaveStreamOpen)) {
        }

        if (leaveStreamOpen)
          Assert.DoesNotThrow(() => stream.ReadByte());
        else
          Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
      }
    }
  }
}
