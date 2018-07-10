using System;
using System.IO;
using System.Text;
using NUnit.Framework;

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
        Assert.AreEqual(test.ExpectedString, Base64.GetEncodedString(test.Data));
        Assert.AreEqual(test.ExpectedBytes,  Base64.Encode(test.Data));
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
        Assert.AreEqual(test.ExpectedString, Base64.GetDecodedString(test.Data, TestUtils.Encodings.Latin1));
        Assert.AreEqual(test.ExpectedBytes,  Base64.Decode(test.Data));
      }
    }

    [Test]
    public void TestEncode2()
    {
      Assert.AreEqual(new byte[] {0x59, 0x6d, 0x46, 0x7a, 0x5a, 0x54, 0x59, 0x30},
                      Base64.Encode(new byte[] {0x62, 0x61, 0x73, 0x65, 0x36, 0x34}));
      Assert.AreEqual(new byte[] {0x59, 0x6d, 0x46, 0x7a, 0x5a, 0x54, 0x59, 0x30},
                      Base64.Encode(new byte[] {0xff, 0x62, 0x61, 0x73, 0x65, 0x36, 0x34, 0xff}, 1, 6));
    }

    [Test]
    public void TestDecode2()
    {
      Assert.AreEqual(new byte[] {0x62, 0x61, 0x73, 0x65, 0x36, 0x34},
                      Base64.Decode(new byte[] {0x59, 0x6d, 0x46, 0x7a, 0x5a, 0x54, 0x59, 0x30}));
      Assert.AreEqual(new byte[] {0x62, 0x61, 0x73, 0x65, 0x36, 0x34},
                      Base64.Decode(new byte[] {0xff, 0x59, 0x6d, 0x46, 0x7a, 0x5a, 0x54, 0x59, 0x30, 0xff}, 1, 8));
    }

    [Test]
    public void TestEncodeDecodeWithSpecificEncoding()
    {
      foreach (var test in new[] {
        new{PlainText = "漢字abcかな123カナ", Base64Text = "5ryi5a2XYWJj44GL44GqMTIz44Kr44OK", Encoding = (Encoding)Encoding.UTF8},
        new{PlainText = "漢字abcかな123カナ", Base64Text = "tMG7+mFiY6SrpMoxMjOlq6XK", Encoding = TestUtils.Encodings.EucJP},
        new{PlainText = "漢字abcかな123カナ", Base64Text = "GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC", Encoding = TestUtils.Encodings.Jis},
        new{PlainText = "漢字abcかな123カナ", Base64Text = "ir+OmmFiY4KpgsgxMjODSoNp", Encoding = TestUtils.Encodings.ShiftJis},
      }) {
        Assert.AreEqual(test.PlainText, Base64.GetDecodedString(test.Base64Text, test.Encoding), test.Encoding.WebName);
        Assert.AreEqual(test.Base64Text, Base64.GetEncodedString(test.PlainText, test.Encoding), test.Encoding.WebName);
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
