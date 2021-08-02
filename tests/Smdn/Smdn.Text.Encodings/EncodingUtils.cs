using System;
using System.Text;
using NUnit.Framework;

namespace Smdn.Text.Encodings {
  [TestFixture]
  public class EncodingUtilsTests {
    [Test]
    public void TestGetEncodingNameNull()
    {
      string name = null;

      Assert.Throws<ArgumentNullException>(() => EncodingUtils.GetEncoding(name));
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
      var expected = TestUtils.Encodings.ShiftJis;

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
      var expected = TestUtils.Encodings.Jis;

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
      var expected = TestUtils.Encodings.EucJP;

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
    public void TestGetEncodingSelectFallback()
    {
      var ret = EncodingUtils.GetEncoding("x-unkwnown-encoding", delegate(string name) {
        Assert.AreEqual("x-unkwnown-encoding", name, "callback arg");

        return Encoding.UTF8;
      });

      Assert.IsNotNull(ret);
      Assert.AreEqual(Encoding.UTF8, ret);
    }

    [Test]
    public void TestGetEncodingSelectFallbackReturnNull()
    {
      var ret = EncodingUtils.GetEncoding("x-unkwnown-encoding", delegate(string name) {
        Assert.AreEqual("x-unkwnown-encoding", name, "callback arg");

        return null;
      });

      Assert.IsNull(ret);
    }

    [Test]
    public void TestGetEncodingThrowException()
    {
      var ex = Assert.Throws<EncodingNotSupportedException>(() => EncodingUtils.GetEncodingThrowException("x-unkwnown-encoding"));

      Assert.AreEqual("x-unkwnown-encoding", ex.EncodingName);
      Assert.IsNotNull(ex.Message);
      Assert.IsNull(ex.InnerException);
    }

    [Test]
    public void TestGetEncodingThrowExceptionSelectFallback()
    {
      var ret = EncodingUtils.GetEncodingThrowException("x-unkwnown-encoding", delegate(string name) {
        Assert.AreEqual("x-unkwnown-encoding", name);
        return Encoding.UTF8;
      });

      Assert.IsNotNull(ret);
      Assert.AreEqual(Encoding.UTF8, ret);
    }

    [Test]
    public void TestGetEncodingThrowExceptionSelectFallbackReturnNull()
    {
      string encodingName = null;

      var ex = Assert.Throws<EncodingNotSupportedException>(() => EncodingUtils.GetEncodingThrowException("x-unkwnown-encoding", delegate (string name) {
        encodingName = name;
        return null;
      }));

      Assert.AreEqual("x-unkwnown-encoding", encodingName);
      Assert.AreEqual("x-unkwnown-encoding", ex.EncodingName);
      Assert.IsNotNull(ex.Message);
      Assert.IsNull(ex.InnerException);
    }
  }
}
