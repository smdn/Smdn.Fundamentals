using System;
using System.Text;
using NUnit.Framework;

namespace Smdn.Formats.PercentEncodings {
  [TestFixture]
  public class PercentEncodingTests {

    [Test]
    public void TestGetEncodedStringRfc2396Uri()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.Rfc2396Uri,
                                                    Encoding.ASCII);

      Assert.AreEqual("%20!%22#$%25&'()*+,-./0123456789:;%3C=%3E?@ABCDEFGHIJKLMNOPQRSTUVWXYZ%5B%5C%5D%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~",
                      actual);
    }

    [Test]
    public void TestGetEncodedStringRfc3986Uri()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.Rfc3986Uri,
                                                    Encoding.ASCII);

      Assert.AreEqual("%20!%22#$%25&'()*+,-./0123456789:;%3C=%3E?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[%5C]%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~",
                      actual);
    }

    [Test]
    public void TestGetEncodedStringRfc2396Data()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.Rfc2396Data,
                                                    Encoding.ASCII);

      Assert.AreEqual("%20!%22%23%24%25%26'()*%2B%2C-.%2F0123456789%3A%3B%3C%3D%3E%3F%40ABCDEFGHIJKLMNOPQRSTUVWXYZ%5B%5C%5D%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~",
                      actual);
    }

    [Test]
    public void TestGetEncodedStringRfc3986Data()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.Rfc3986Data,
                                                    Encoding.ASCII);

      Assert.AreEqual("%20%21%22%23%24%25%26%27%28%29%2A%2B%2C-.%2F0123456789%3A%3B%3C%3D%3E%3F%40ABCDEFGHIJKLMNOPQRSTUVWXYZ%5B%5C%5D%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~",
                      actual);
    }

    [Test]
    public void TestGetEncodedStringUriEscapeUriString()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.UriEscapeUriString,
                                                    Encoding.ASCII);

      Assert.AreEqual(ToPercentEncodedTransformMode.Rfc3986Uri,
                      ToPercentEncodedTransformMode.UriEscapeUriString);

      Assert.AreEqual(Uri.EscapeUriString(text),
                      actual,
                      "same as Uri.EscapeDataString");
    }

    [Test]
    public void TestGetEncodedStringUriEscapeDataString()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.UriEscapeDataString,
                                                    Encoding.ASCII);

      Assert.AreEqual(ToPercentEncodedTransformMode.Rfc3986Data,
                      ToPercentEncodedTransformMode.UriEscapeDataString);

      Assert.AreEqual(Uri.EscapeDataString(text),
                      actual,
                      "same as Uri.EscapeDataString");
    }

    [Test]
    public void TestGetEncodedStringRfc5092Uri()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.Rfc5092Uri,
                                                    Encoding.ASCII);

      Assert.AreEqual("%20!%22%23$%25&'()*+,-.%2F0123456789%3A%3B%3C=%3E%3F%40ABCDEFGHIJKLMNOPQRSTUVWXYZ%5B%5C%5D%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~",
                      actual);
    }

    [Test]
    public void TestGetEncodedStringRfc5092Path()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.Rfc5092Path,
                                                    Encoding.ASCII);

      Assert.AreEqual("%20!%22%23$%25&'()*+,-./0123456789:%3B%3C=%3E%3F@ABCDEFGHIJKLMNOPQRSTUVWXYZ%5B%5C%5D%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~",
                      actual);
    }

    [Test]
    public void TestGetEncodedStringNonAsciiString()
    {
      foreach (var mode in new[] {
        ToPercentEncodedTransformMode.Rfc2396Uri,
        ToPercentEncodedTransformMode.Rfc2396Data,
        ToPercentEncodedTransformMode.Rfc3986Uri,
        ToPercentEncodedTransformMode.Rfc3986Data,
        ToPercentEncodedTransformMode.Rfc5092Uri,
        ToPercentEncodedTransformMode.Rfc5092Path,
      }) {
        Assert.AreEqual("%93%FA%96%7B%8C%EA", PercentEncoding.GetEncodedString("日本語", mode, TestUtils.Encodings.ShiftJis), "mode: {0}", mode);
        Assert.AreEqual("%C6%FC%CB%DC%B8%EC", PercentEncoding.GetEncodedString("日本語", mode, TestUtils.Encodings.EucJP), "mode: {0}", mode);
      }
    }

    [Test]
    public void TestGetEncodedStringEscapeSpaceToPlus()
    {
      foreach (var mode in new[] {
        ToPercentEncodedTransformMode.Rfc2396Uri,
        ToPercentEncodedTransformMode.Rfc2396Data,
        ToPercentEncodedTransformMode.Rfc3986Uri,
        ToPercentEncodedTransformMode.Rfc3986Data,
      }) {
        Assert.AreEqual("abc%20def", PercentEncoding.GetEncodedString("abc def", mode, Encoding.ASCII), "mode: {0}", mode);

        var mode2 = mode | ToPercentEncodedTransformMode.EscapeSpaceToPlus;

        Assert.AreEqual("abc+def", PercentEncoding.GetEncodedString("abc def", mode2, Encoding.ASCII), "mode: {0}", mode2);
      }
    }

    [Test]
    public void TestGetDecodedString()
    {
      Assert.AreEqual("012abcABC-._~!\"#$日本語",
                      PercentEncoding.GetDecodedString("012abcABC-._~%21%22%23%24%e6%97%a5%e6%9c%ac%e8%aa%9e", Encoding.UTF8));
      Assert.AreEqual("日本語", PercentEncoding.GetDecodedString("%93%fa%96%7B%8C%EA", TestUtils.Encodings.ShiftJis));
      Assert.AreEqual("日本語", PercentEncoding.GetDecodedString("%c6%Fc%cb%Dc%b8%eC", TestUtils.Encodings.EucJP));
    }

    [Test]
    public void TestGetDecodedStringDecodePlusToSpace()
    {
      Assert.AreEqual("ABC+DEF", PercentEncoding.GetDecodedString("ABC+DEF", false));
      Assert.AreEqual("ABC DEF", PercentEncoding.GetDecodedString("ABC+DEF", true));
    }
  }
}
