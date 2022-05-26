// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;
using NUnit.Framework;
using EncodingProvider = Smdn.Test.NUnit.Encodings;

namespace Smdn.Text.Encodings {
  [TestFixture]
  public class EncodingUtilsTests {
    [SetUp]
    public void SetUp()
    {
#if !NETFRAMEWORK
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
    }

    [Test]
    public void TestGetEncodingNameNull()
      => Assert.Throws<ArgumentNullException>(() => EncodingUtils.GetEncoding(name: null));

    [Test]
    public void TestGetEncodingNameEmpty()
      => Assert.IsNull(EncodingUtils.GetEncoding(name: string.Empty));

    [TestCase("utf16")]
    [TestCase("UTF16")]
    [TestCase("utf-16")]
    [TestCase("UTF-16")]
    [TestCase("utf_16")]
    [TestCase("UTF_16")]
    [TestCase("utf 16")]
    [TestCase("UTF 16")]
    public void TestGetEncodingUTF16(string name)
      => Assert.AreEqual(Encoding.Unicode, EncodingUtils.GetEncoding(name), name);

    [TestCase("utf8")]
    [TestCase("UTF8")]
    [TestCase("utf-8")]
    [TestCase("UTF-8")]
    [TestCase("utf_8")]
    [TestCase("UTF_8")]
    [TestCase("utf 8")]
    [TestCase("UTF 8")]
    public void TestGetEncodingUTF8(string name)
      => Assert.AreEqual(Encoding.UTF8, EncodingUtils.GetEncoding(name), name);

    [TestCase("shiftjis")]
    [TestCase("SHIFTJIS")]
    [TestCase("shift_jis")]
    [TestCase("SHIFT_JIS")]
    [TestCase("shift-jis")]
    [TestCase("SHIFT-JIS")]
    [TestCase("shift jis")]
    [TestCase("SHIFT JIS")]
    [TestCase("x-sjis")]
    [TestCase("X-SJIS")]
    [TestCase("x_sjis")]
    [TestCase("X_SJIS")]
    public void TestGetEncodingShiftJIS(string name)
      => Assert.AreEqual(EncodingProvider.ShiftJis, EncodingUtils.GetEncoding(name), name);

    [TestCase("iso-2022-jp")]
    [TestCase("ISO-2022-JP")]
    [TestCase("iso_2022_jp")]
    [TestCase("ISO_2022_JP")]
    [TestCase("iso2022jp")]
    [TestCase("ISO2022JP")]
    public void TestGetEncodingISO2022JP(string name)
      => Assert.AreEqual(EncodingProvider.Jis, EncodingUtils.GetEncoding(name), name);

    [TestCase("euc-jp")]
    [TestCase("EUC-JP")]
    [TestCase("euc_jp")]
    [TestCase("EUC_JP")]
    [TestCase("euc jp")]
    [TestCase("EUC JP")]
    [TestCase("x-euc-jp")]
    [TestCase("X-EUC-JP")]
    [TestCase("x_euc_jp")]
    [TestCase("X_EUC_JP")]
    public void TestGetEncodingEUCJP(string name)
      => Assert.AreEqual(EncodingProvider.EucJP, EncodingUtils.GetEncoding(name), name);

    [TestCase("utf8 ")]
    [TestCase(" utf8")]
    [TestCase(" utf8 ")]
    [TestCase("utf8\n")]
    [TestCase("\tutf8")]
    [TestCase("\tutf8\n")]
    public void TestGetEncodingContainsWhitespaces(string name)
      => Assert.AreEqual(Encoding.UTF8, EncodingUtils.GetEncoding(name), name);

    [Test]
    public void TestGetEncodingUnsupported()
      => Assert.IsNull(EncodingUtils.GetEncoding("x-unkwnown-encoding"));

    [Test]
    public void TestGetEncodingSelectFallback()
    {
      var ret = EncodingUtils.GetEncoding("x-unkwnown-encoding", name => {
        Assert.AreEqual("x-unkwnown-encoding", name, "callback arg");

        return Encoding.UTF8;
      });

      Assert.IsNotNull(ret);
      Assert.AreEqual(Encoding.UTF8, ret);
    }

    [Test]
    public void TestGetEncodingSelectFallbackReturnNull()
    {
      var ret = EncodingUtils.GetEncoding("x-unkwnown-encoding", name => {
        Assert.AreEqual("x-unkwnown-encoding", name, "callback arg");

        return null;
      });

      Assert.IsNull(ret);
    }

    [Test]
    public void TestGetEncodingThrowException()
    {
      var ex = Assert.Throws<EncodingNotSupportedException>(
        () => EncodingUtils.GetEncodingThrowException("x-unkwnown-encoding")
      );

      Assert.AreEqual("x-unkwnown-encoding", ex.EncodingName);
      Assert.IsNotNull(ex.Message);
      Assert.IsNull(ex.InnerException);
    }

    [Test]
    public void TestGetEncodingThrowExceptionSelectFallback()
    {
      var ret = EncodingUtils.GetEncodingThrowException("x-unkwnown-encoding", name => {
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

      var ex = Assert.Throws<EncodingNotSupportedException>(
        () => EncodingUtils.GetEncodingThrowException("x-unkwnown-encoding", name => {
          encodingName = name;
          return null;
        })
      );

      Assert.AreEqual("x-unkwnown-encoding", encodingName);
      Assert.AreEqual("x-unkwnown-encoding", ex.EncodingName);
      Assert.IsNotNull(ex.Message);
      Assert.IsNull(ex.InnerException);
    }
  }
}
