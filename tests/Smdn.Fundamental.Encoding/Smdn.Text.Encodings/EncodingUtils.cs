// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;
using NUnit.Framework;
using EncodingProvider = Smdn.Test.NUnit.Encodings;

namespace Smdn.Text.Encodings;

[TestFixture]
public partial class EncodingUtilsTests {
  [SetUp]
  public void SetUp()
  {
#if !NETFRAMEWORK
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
  }

  [Test]
  public void TestGetEncodingNameNull()
    => Assert.Throws<ArgumentNullException>(() => EncodingUtils.GetEncoding(name: null!));

  [Test]
  public void TestGetEncodingNameEmpty()
    => Assert.That(EncodingUtils.GetEncoding(name: string.Empty), Is.Null);

  [TestCase("utf16")]
  [TestCase("UTF16")]
  [TestCase("utf-16")]
  [TestCase("UTF-16")]
  [TestCase("utf_16")]
  [TestCase("UTF_16")]
  [TestCase("utf 16")]
  [TestCase("UTF 16")]
  public void TestGetEncodingUTF16(string name)
    => Assert.That(EncodingUtils.GetEncoding(name), Is.EqualTo(Encoding.Unicode), name);

  [TestCase("utf8")]
  [TestCase("UTF8")]
  [TestCase("utf-8")]
  [TestCase("UTF-8")]
  [TestCase("utf_8")]
  [TestCase("UTF_8")]
  [TestCase("utf 8")]
  [TestCase("UTF 8")]
  public void TestGetEncodingUTF8(string name)
    => Assert.That(EncodingUtils.GetEncoding(name), Is.EqualTo(Encoding.UTF8), name);

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
    => Assert.That(EncodingUtils.GetEncoding(name), Is.EqualTo(EncodingProvider.ShiftJis), name);

  [TestCase("iso-2022-jp")]
  [TestCase("ISO-2022-JP")]
  [TestCase("iso_2022_jp")]
  [TestCase("ISO_2022_JP")]
  [TestCase("iso2022jp")]
  [TestCase("ISO2022JP")]
  public void TestGetEncodingISO2022JP(string name)
    => Assert.That(EncodingUtils.GetEncoding(name), Is.EqualTo(EncodingProvider.Jis), name);

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
    => Assert.That(EncodingUtils.GetEncoding(name), Is.EqualTo(EncodingProvider.EucJP), name);

  [TestCase("utf8 ")]
  [TestCase(" utf8")]
  [TestCase(" utf8 ")]
  [TestCase("utf8\n")]
  [TestCase("\tutf8")]
  [TestCase("\tutf8\n")]
  public void TestGetEncodingContainsWhitespaces(string name)
    => Assert.That(EncodingUtils.GetEncoding(name), Is.EqualTo(Encoding.UTF8), name);

  [Test]
  public void TestGetEncodingUnsupported()
    => Assert.That(EncodingUtils.GetEncoding("x-unkwnown-encoding"), Is.Null);

  [Test]
  public void TestGetEncodingSelectFallback()
  {
    var ret = EncodingUtils.GetEncoding("x-unkwnown-encoding", name => {
      Assert.That(name, Is.EqualTo("x-unkwnown-encoding"), "callback arg");

      return Encoding.UTF8;
    });

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret, Is.EqualTo(Encoding.UTF8));
  }

  [Test]
  public void TestGetEncodingSelectFallbackReturnNull()
  {
    var ret = EncodingUtils.GetEncoding("x-unkwnown-encoding", name => {
      Assert.That(name, Is.EqualTo("x-unkwnown-encoding"), "callback arg");

      return null;
    });

    Assert.That(ret, Is.Null);
  }

  [Test]
  public void TestGetEncodingThrowException()
  {
    var ex = Assert.Throws<EncodingNotSupportedException>(
      () => EncodingUtils.GetEncodingThrowException("x-unkwnown-encoding")
    );

    Assert.That(ex!.EncodingName, Is.EqualTo("x-unkwnown-encoding"));
    Assert.That(ex.Message, Is.Not.Null);
    Assert.That(ex.InnerException, Is.Null);
  }

  [Test]
  public void TestGetEncodingThrowExceptionSelectFallback()
  {
    var ret = EncodingUtils.GetEncodingThrowException("x-unkwnown-encoding", name => {
      Assert.That(name, Is.EqualTo("x-unkwnown-encoding"));
      return Encoding.UTF8;
    });

    Assert.That(ret, Is.Not.Null);
    Assert.That(ret, Is.EqualTo(Encoding.UTF8));
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

    Assert.That(encodingName, Is.EqualTo("x-unkwnown-encoding"));
    Assert.That(ex!.EncodingName, Is.EqualTo("x-unkwnown-encoding"));
    Assert.That(ex.Message, Is.Not.Null);
    Assert.That(ex.InnerException, Is.Null);
  }
}
