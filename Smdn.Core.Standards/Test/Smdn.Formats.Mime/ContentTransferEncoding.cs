using System;
using System.IO;
using System.Text;
using NUnit.Framework;

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
        try {
          ContentTransferEncoding.GetEncodingMethodThrowException(name);
          Assert.Fail("NotSupportedException not thrown");
        }
        catch (NotSupportedException) {
        }
      }
    }

    [Test]
    public void TestCreateTextReader()
    {
      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes("=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B"))) {
        var reader = ContentTransferEncoding.CreateTextReader(stream,
                                                              ContentTransferEncodingMethod.QuotedPrintable,
                                                              TestUtils.Encodings.Jis);

        Assert.AreEqual(TestUtils.Encodings.Jis, reader.CurrentEncoding);
        Assert.AreEqual("漢字abcかな123カナ", reader.ReadToEnd());

        stream.Position = 0L;

        reader.Close();

        try {
          stream.ReadByte();

          Assert.Fail("InvalidOperationException not thrown");
        }
        catch (InvalidOperationException) {
        }
      }
    }

    [Test]
    public void TestCreateTextReaderLeaveInnerStreamOpen()
    {
      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes("=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B"))) {
        var reader1 = ContentTransferEncoding.CreateTextReader(stream,
                                                               ContentTransferEncodingMethod.QuotedPrintable,
                                                               TestUtils.Encodings.Jis,
                                                               true);

        Assert.AreEqual("漢字abcかな123カナ", reader1.ReadToEnd());

        stream.Position = 0L;

        reader1.Close();

        var reader2 = ContentTransferEncoding.CreateTextReader(stream,
                                                               ContentTransferEncodingMethod.QuotedPrintable,
                                                               TestUtils.Encodings.Jis);

        Assert.AreEqual("漢字abcかな123カナ", reader2.ReadToEnd(), "read again");
      }
    }

    [Test]
    public void TestCreateBinaryReader()
    {
      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes("=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B"))) {
        var reader = ContentTransferEncoding.CreateBinaryReader(stream,
                                                                ContentTransferEncodingMethod.QuotedPrintable,
                                                                TestUtils.Encodings.Jis);

        Assert.AreEqual(TestUtils.Encodings.Jis.GetBytes("漢字abcかな123カナ"),
                        reader.ReadBytes((int)stream.Length));

        stream.Position = 0L;

        reader.Close();

        try {
          stream.ReadByte();
          
          Assert.Fail("InvalidOperationException not thrown");
        }
        catch (InvalidOperationException) {
        }
      }
    }

    [Test]
    public void TestCreateBinaryReaderLeaveInnerStreamOpen()
    {
      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes("=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B"))) {
        var reader1 = ContentTransferEncoding.CreateBinaryReader(stream,
                                                                 ContentTransferEncodingMethod.QuotedPrintable,
                                                                 TestUtils.Encodings.Jis,
                                                                 true);

        Assert.AreEqual(TestUtils.Encodings.Jis.GetBytes("漢字abcかな123カナ"),
                        reader1.ReadBytes((int)stream.Length));

        stream.Position = 0L;

        reader1.Close();

        var reader2 = ContentTransferEncoding.CreateBinaryReader(stream,
                                                                 ContentTransferEncodingMethod.QuotedPrintable,
                                                                 TestUtils.Encodings.Jis);

        Assert.AreEqual(TestUtils.Encodings.Jis.GetBytes("漢字abcかな123カナ"),
                        reader2.ReadBytes((int)stream.Length),
                        "read again");
      }
    }
  }
}
