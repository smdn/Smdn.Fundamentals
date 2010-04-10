using System;
using NUnit.Framework;

namespace Smdn.Formats.Mime {
  [TestFixture]
  public class ContentTransferEncodingTests {
    [Test]
    public void GetEncodingMethod()
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
    public void GetEncodingMethodThrowException()
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
  }
}
