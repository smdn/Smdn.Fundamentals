// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Smdn.Test.NUnit;

namespace Smdn.Formats.Mime {
  [TestFixture]
  public class ContentTransferEncodingTests {
    [Test]
    public void TestGetEncodingMethod()
    {
      foreach (var test in new[] {
        new {Expected = ContentTransferEncodingMethod.SevenBit, Name = "7bit"},
        new {Expected = ContentTransferEncodingMethod.SevenBit, Name = "7BIT"},

        new {Expected = ContentTransferEncodingMethod.EightBit, Name = "8bit"},

        new {Expected = ContentTransferEncodingMethod.Binary, Name = "binary"},

        new {Expected = ContentTransferEncodingMethod.Base64, Name = "base64"},
        new {Expected = ContentTransferEncodingMethod.Base64, Name = "Base64"},
        new {Expected = ContentTransferEncodingMethod.Base64, Name = "BASE64"},

        new {Expected = ContentTransferEncodingMethod.QuotedPrintable, Name = "quoted-printable"},

        new {Expected = ContentTransferEncodingMethod.GZip64, Name = "x-gzip64"},
        new {Expected = ContentTransferEncodingMethod.GZip64, Name = "gzip64"},

        new {Expected = ContentTransferEncodingMethod.UUEncode, Name = "x-uuencode"},
        new {Expected = ContentTransferEncodingMethod.UUEncode, Name = "x-uuencoded"},
        new {Expected = ContentTransferEncodingMethod.UUEncode, Name = "uuencode"},

        new {Expected = ContentTransferEncodingMethod.Unknown, Name = "x-unknown"},
        new {Expected = ContentTransferEncodingMethod.Unknown, Name = "unknown"},
      }) {
        Assert.AreEqual(test.Expected, ContentTransferEncoding.GetEncodingMethod(test.Name), "name: {0}", test.Name);
      }
    }

    [Test]
    public void TestGetEncodingMethodThrowException()
    {
      foreach (var name in new[] {
        "x-unkwnon",
        "unknown",
        "base32",
      }) {
        Assert.Throws<NotSupportedException>(() => ContentTransferEncoding.GetEncodingMethodThrowException(name), name);
      }
    }

    [TestCase(ContentTransferEncodingMethod.SevenBit)]
    [TestCase(ContentTransferEncodingMethod.EightBit)]
    [TestCase(ContentTransferEncodingMethod.Binary)]
    [TestCase(ContentTransferEncodingMethod.Base64)]
    [TestCase(ContentTransferEncodingMethod.QuotedPrintable)]
    [TestCase(ContentTransferEncodingMethod.UUEncode)]
    public void TestCreateDecodingStreamCloseInnerStream(ContentTransferEncodingMethod method)
    {
      using (var inputStream = new MemoryStream(new byte[] {0, 1, 2, 3, 4, 5, 6, 7})) {
        using (var decodingStream = ContentTransferEncoding.CreateDecodingStream(inputStream,
                                                                                 method,
                                                                                 false)) {
        }

        Assert.Throws<ObjectDisposedException>(() => inputStream.ReadByte());
      }
    }

    [TestCase(ContentTransferEncodingMethod.SevenBit)]
    [TestCase(ContentTransferEncodingMethod.EightBit)]
    [TestCase(ContentTransferEncodingMethod.Binary)]
    [TestCase(ContentTransferEncodingMethod.Base64)]
    [TestCase(ContentTransferEncodingMethod.QuotedPrintable)]
    [TestCase(ContentTransferEncodingMethod.UUEncode)]
    public void TestCreateDecodingStreamLeaveInnerStreamOpen(ContentTransferEncodingMethod method)
    {
      using (var inputStream = new MemoryStream(new byte[] {0, 1, 2, 3, 4, 5, 6, 7})) {
        using (var decodingStream = ContentTransferEncoding.CreateDecodingStream(inputStream,
                                                                                 method,
                                                                                 true)) {
        }

        Assert.DoesNotThrow(() => inputStream.ReadByte());
      }
    }

    [TestCase(ContentTransferEncodingMethod.SevenBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
    [TestCase(ContentTransferEncodingMethod.EightBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
    [TestCase(ContentTransferEncodingMethod.Base64, "GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC")]
    [TestCase(ContentTransferEncodingMethod.QuotedPrintable, "=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B")]
    [TestCase(ContentTransferEncodingMethod.UUEncode, "begin 664 test.txt\nD&R1\"-$$[>ALH0F%B8QLD0B0K)$H;*$(Q,C,;)$(E*R5*&RA\"\n`\nend")]
    public void TestCreateTextReader(ContentTransferEncodingMethod cte, string content)
    {
      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(content))) {
        var reader = ContentTransferEncoding.CreateTextReader(stream,
                                                              cte,
                                                              TestUtils.Encodings.Jis);

        Assert.AreEqual(TestUtils.Encodings.Jis, reader.CurrentEncoding);
        Assert.AreEqual("漢字abcかな123カナ", reader.ReadToEnd());

        stream.Position = 0L;

        reader.Dispose();

        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
      }
    }

    [TestCase(ContentTransferEncodingMethod.SevenBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
    [TestCase(ContentTransferEncodingMethod.EightBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
    [TestCase(ContentTransferEncodingMethod.Base64, "GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC")]
    [TestCase(ContentTransferEncodingMethod.QuotedPrintable, "=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B")]
    [TestCase(ContentTransferEncodingMethod.UUEncode, "begin 664 test.txt\nD&R1\"-$$[>ALH0F%B8QLD0B0K)$H;*$(Q,C,;)$(E*R5*&RA\"\n`\nend")]
    public void TestCreateTextReaderLeaveInnerStreamOpen(ContentTransferEncodingMethod cte, string content)
    {
      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(content))) {
        var reader1 = ContentTransferEncoding.CreateTextReader(stream,
                                                               cte,
                                                               TestUtils.Encodings.Jis,
                                                               true);

        Assert.AreEqual("漢字abcかな123カナ", reader1.ReadToEnd());

        stream.Position = 0L;

        reader1.Dispose();

        var reader2 = ContentTransferEncoding.CreateTextReader(stream,
                                                               cte,
                                                               TestUtils.Encodings.Jis);

        Assert.AreEqual("漢字abcかな123カナ", reader2.ReadToEnd(), "read again");
      }
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestCreateTextReaderFromBinaryContentTransferEncoding(bool leaveStreamOpen)
    {
      var content = "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42";

      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(content))) {
        Assert.Throws<InvalidOperationException>(delegate {
          ContentTransferEncoding.CreateTextReader(stream,
                                                   ContentTransferEncodingMethod.Binary,
                                                   TestUtils.Encodings.Jis,
                                                   leaveStreamOpen);
        });
      }
    }

    [TestCase(ContentTransferEncodingMethod.SevenBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
    [TestCase(ContentTransferEncodingMethod.EightBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
    [TestCase(ContentTransferEncodingMethod.Binary, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
    [TestCase(ContentTransferEncodingMethod.Base64, "GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC")]
    [TestCase(ContentTransferEncodingMethod.QuotedPrintable, "=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B")]
    [TestCase(ContentTransferEncodingMethod.UUEncode, "begin 664 test.txt\nD&R1\"-$$[>ALH0F%B8QLD0B0K)$H;*$(Q,C,;)$(E*R5*&RA\"\n`\nend")]
    public void TestCreateBinaryReader(ContentTransferEncodingMethod cte, string content)
    {
      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(content))) {
        var reader = ContentTransferEncoding.CreateBinaryReader(stream,
                                                                cte,
                                                                TestUtils.Encodings.Jis);

        Assert.AreEqual(TestUtils.Encodings.Jis.GetBytes("漢字abcかな123カナ"),
                        reader.ReadBytes((int)stream.Length));

        stream.Position = 0L;

        reader.Dispose();

        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
      }
    }

    [TestCase(ContentTransferEncodingMethod.SevenBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
    [TestCase(ContentTransferEncodingMethod.EightBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
    [TestCase(ContentTransferEncodingMethod.Binary, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
    [TestCase(ContentTransferEncodingMethod.Base64, "GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC")]
    [TestCase(ContentTransferEncodingMethod.QuotedPrintable, "=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B")]
    [TestCase(ContentTransferEncodingMethod.UUEncode, "begin 664 test.txt\nD&R1\"-$$[>ALH0F%B8QLD0B0K)$H;*$(Q,C,;)$(E*R5*&RA\"\n`\nend")]
    public void TestCreateBinaryReaderLeaveInnerStreamOpen(ContentTransferEncodingMethod cte, string content)
    {
      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(content))) {
        var reader1 = ContentTransferEncoding.CreateBinaryReader(stream,
                                                                 cte,
                                                                 TestUtils.Encodings.Jis,
                                                                 true);

        Assert.AreEqual(TestUtils.Encodings.Jis.GetBytes("漢字abcかな123カナ"),
                        reader1.ReadBytes((int)stream.Length));

        stream.Position = 0L;

        reader1.Dispose();

        var reader2 = ContentTransferEncoding.CreateBinaryReader(stream,
                                                                 cte,
                                                                 TestUtils.Encodings.Jis);

        Assert.AreEqual(TestUtils.Encodings.Jis.GetBytes("漢字abcかな123カナ"),
                        reader2.ReadBytes((int)stream.Length),
                        "read again");
      }
    }
  }
}
