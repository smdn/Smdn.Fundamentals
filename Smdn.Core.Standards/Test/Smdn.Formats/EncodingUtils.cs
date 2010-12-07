using System;
using System.Text;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture]
  public class EncodingUtilsTests {
    [Test, ExpectedException(typeof(ArgumentNullException))]
    public void TestGetEncodingNameNull()
    {
      string name = null;

      Assert.IsNull(EncodingUtils.GetEncoding(name));
    }

    [Test]
    public void TestGetEncodingNameEmpty()
    {
      string name = string.Empty;

      Assert.IsNull(EncodingUtils.GetEncoding(name));
    }

    [Test]
    public void TestGetEncodingUTF16()
    {
      foreach (var name in new[] {
        "utf16",
        "UTF16",
        "utf-16",
        "UTF-16",
        "utf_16",
        "UTF_16",
        "utf 16",
        "UTF 16",
      }) {
        Assert.AreEqual(Encoding.Unicode, EncodingUtils.GetEncoding(name), name);
      }
    }

    [Test]
    public void TestGetEncodingUTF8()
    {
      foreach (var name in new[] {
        "utf8",
        "UTF8",
        "utf-8",
        "UTF-8",
        "utf_8",
        "UTF_8",
        "utf 8",
        "UTF 8",
      }) {
        Assert.AreEqual(Encoding.UTF8, EncodingUtils.GetEncoding(name), name);
      }
    }

    [Test]
    public void TestGetEncodingShiftJIS()
    {
      var expected = Encoding.GetEncoding("shift_jis");

      foreach (var name in new[] {
        "shiftjis",
        "SHIFTJIS",
        "shift_jis",
        "SHIFT_JIS",
        "shift-jis",
        "SHIFT-JIS",
        "shift jis",
        "SHIFT JIS",
        "x-sjis",
        "X-SJIS",
        "x_sjis",
        "X_SJIS",
      }) {
        Assert.AreEqual(expected, EncodingUtils.GetEncoding(name), name);
      }
    }

    [Test]
    public void TestGetEncodingISO2022JP()
    {
      var expected = Encoding.GetEncoding("iso-2022-jp");

      foreach (var name in new[] {
        "iso-2022-jp",
        "ISO-2022-JP",
        "iso_2022_jp",
        "ISO_2022_JP",
        "iso2022jp",
        "ISO2022JP",
      }) {
        Assert.AreEqual(expected, EncodingUtils.GetEncoding(name), name);
      }
    }

    [Test]
    public void TestGetEncodingEUCJP()
    {
      var expected = Encoding.GetEncoding("euc-jp");

      foreach (var name in new[] {
        "euc-jp",
        "EUC-JP",
        "euc_jp",
        "EUC_JP",
        "euc jp",
        "EUC JP",
        "x-euc-jp",
        "X-EUC-JP",
        "x_euc_jp",
        "X_EUC_JP",
      }) {
        Assert.AreEqual(expected, EncodingUtils.GetEncoding(name), name);
      }
    }

    [Test]
    public void TestGetEncodingContainsWhitespaces()
    {
      foreach (var name in new[] {
        "utf8 ",
        " utf8",
        " utf8 ",
        "utf8\n",
        "\tutf8",
        "\tutf8\n",
      }) {
        Assert.AreEqual(Encoding.UTF8, EncodingUtils.GetEncoding(name), name);
      }
    }

    [Test]
    public void TestGetEncodingUnsupported()
    {
      Assert.IsNull(EncodingUtils.GetEncoding("x-unkwnown-encoding"));
    }

    [Test]
    public void TestGetEncodingThrowException()
    {
      try {
        EncodingUtils.GetEncodingThrowException("x-unkwnown-encoding");
        Assert.Fail("EncodingNotSupportedException not thrown");
      }
      catch (EncodingNotSupportedException ex) {
        Assert.AreEqual("x-unkwnown-encoding", ex.EncodingName);
        Assert.IsNotNull(ex.Message);
        Assert.IsNull(ex.InnerException);
      }
    }
  }
}
