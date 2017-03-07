using System;
using System.Text;
using NUnit.Framework;

namespace Smdn.Formats.Mime {
  [TestFixture]
  public class MimeEncodingTests {
    [Test]
    public void TestEncodeNoFolding()
    {
      Assert.AreEqual("=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?=",
                      MimeEncoding.Encode("漢字abcかな123カナ", MimeEncodingMethod.Base64, Encoding.UTF8),
                      "base64");
      Assert.AreEqual("=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
                      MimeEncoding.Encode("漢字abcかな123カナ", MimeEncodingMethod.QuotedPrintable, Encoding.UTF8),
                      "quoted-printable");
    }

    [Test]
    public void TestEncodeWithFolding()
    {
      Assert.AreEqual("=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?=\r\n\t=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?=",
                      MimeEncoding.Encode("漢字abcかな123カナ漢字abcかな123カナ漢字abcかな123カナ", MimeEncodingMethod.Base64, Encoding.UTF8, 76, 0),
                      "base64");
      Assert.AreEqual("=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=\r\n\t=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB?=\r\n\t=?utf-8?q?=E3=83=8A?=",
                      MimeEncoding.Encode("漢字abcかな123カナ漢字abcかな123カナ", MimeEncodingMethod.QuotedPrintable, Encoding.UTF8, 76, 0),
                      "quoted-printable");
    }

    [Test]
    public void TestEncodeSpecifiedFormat()
    {
      Assert.AreEqual("=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK5ryi5a2X?=\n =?utf-8?b?YWJj44GL44GqMTIz44Kr44OK5ryi5a2XYWJj44GL44GqMTIz44Kr?=\n =?utf-8?b?44OK?=",
                      MimeEncoding.Encode("漢字abcかな123カナ漢字abcかな123カナ漢字abcかな123カナ", MimeEncodingMethod.Base64, Encoding.UTF8, 64, 9, "\n "),
                      "base64");
      Assert.AreEqual("=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123?=\n =?utf-8?q?=E3=82=AB=E3=83=8A=E6=BC=A2=E5=AD=97abc=E3=81=8B?=\n =?utf-8?q?=E3=81=AA123=E3=82=AB=E3=83=8A=E6=BC=A2=E5=AD=97abc?=\n =?utf-8?q?=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
                      MimeEncoding.Encode("漢字abcかな123カナ漢字abcかな123カナ漢字abcかな123カナ", MimeEncodingMethod.QuotedPrintable, Encoding.UTF8, 64, 9, "\n "),
                      "quoted-printable");
    }

    [Test]
    public void TestEncodeInvalidCharset()
    {
      try {
        MimeEncoding.Encode("漢字abcかな123カナ", MimeEncodingMethod.Base64, null);
        Assert.Fail("no exceptions thrown");
      }
      catch (ArgumentException) {
      }
    }

    [Test]
    public void TestEncodeInvalidEncoding()
    {
      try {
        MimeEncoding.Encode("漢字abcかな123カナ", (MimeEncodingMethod)0x7fffffff, Encoding.UTF8);
        Assert.Fail("no exceptions thrown");
      }
      catch (ArgumentException) {
      }
    }

    [Test]
    public void TestDecodeContainsLanguageSpecification()
    {
      Encoding charset;
      MimeEncodingMethod encoding;

      Assert.AreEqual("Keith Moore",
                      MimeEncoding.Decode("=?US-ASCII*EN?Q?Keith_Moore?=", out encoding, out charset));
      Assert.AreEqual(MimeEncodingMethod.QEncoding, encoding);
      Assert.AreEqual(Encoding.ASCII, charset);
    }

    [Test]
    public void TestDecodeQEncoding()
    {
      // http://tools.ietf.org/html/rfc2047#section-8
      // 8. Examples
      Assert.AreEqual("a", MimeEncoding.Decode("=?ISO-8859-1?Q?a?="), "case1");
      // TODO
      //Assert.AreEqual("a b", MimeEncoding.Decode("=?ISO-8859-1?Q?a?= b"), "case2");
      Assert.AreEqual("ab", MimeEncoding.Decode("=?ISO-8859-1?Q?a?= =?ISO-8859-1?Q?b?="), "case3");
      Assert.AreEqual("ab", MimeEncoding.Decode("=?ISO-8859-1?Q?a?=  =?ISO-8859-1?Q?b?="), "case4");
      Assert.AreEqual("ab", MimeEncoding.Decode("=?ISO-8859-1?Q?a?=\n =?ISO-8859-1?Q?b?="), "case5");
      Assert.AreEqual("a b", MimeEncoding.Decode("=?ISO-8859-1?Q?a_b?="), "case6");
      Assert.AreEqual("a b", MimeEncoding.Decode("=?ISO-8859-1?Q?a?= =?ISO-8859-2?Q?_b?="), "case7");
    }

    [Test]
    public void TestEncodeQEncodingWhitespaces()
    {
      Assert.AreEqual("=?utf-8?q?=E6=BC=A2=20=E5=AD=97=09=E3=81=8B=E3=81=AA?=",
                      MimeEncoding.Encode("漢 字\tかな", MimeEncodingMethod.QEncoding, Encoding.UTF8));
    }

    [Test]
    public void TestDecodeQEncodingWhitespaces()
    {
      Assert.AreEqual("漢 字\tかな",
                      MimeEncoding.Decode("=?utf-8?q?=E6=BC=A2=20=E5=AD=97=09=E3=81=8B=E3=81=AA?="));
      Assert.AreEqual("漢 字\tかな",
                      MimeEncoding.Decode("=?utf-8?q?=E6=BC=A2_=E5=AD=97=09=E3=81=8B=E3=81=AA?="));
    }

    [Test]
    public void TestEncodeQEncodingSpecialChars()
    {
      Assert.AreEqual("=?utf-8?q?a=3Fb=3Dc=5Fd?=",
                      MimeEncoding.Encode("a?b=c_d", MimeEncodingMethod.QEncoding, Encoding.UTF8));
    }

    [Test]
    public void TestDecodeQEncodingSpecialChars()
    {
      Assert.AreEqual("a?b=c_d", MimeEncoding.Decode("=?utf-8?q?a=3Fb=3Dc=5Fd?="));
      Assert.AreEqual("a?b=c d", MimeEncoding.Decode("=?utf-8?q?a=3Fb=3Dc_d?="));
    }

    [Test]
    public void TestDecodeQEncodingCharsets()
    {
      Encoding charset;
      MimeEncodingMethod encoding;

      Assert.AreEqual("漢字abcかな123カナ",
                      MimeEncoding.Decode("=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=", out encoding, out charset),
                      "utf8");
      Assert.AreEqual(MimeEncodingMethod.QuotedPrintable, encoding, "utf8");
      Assert.AreEqual(Encoding.UTF8, charset, "utf8");

      Assert.AreEqual("漢字abcかな123カナ",
                      MimeEncoding.Decode("=?utf-7?Q?+byJbVw-abc+MEswag-123+MKswyg-?=", out encoding, out charset),
                      "utf7");
      Assert.AreEqual(MimeEncodingMethod.QuotedPrintable, encoding, "utf7");
      Assert.AreEqual(Encoding.UTF7, charset, "utf7");

      Assert.AreEqual("漢字abcかな123カナ",
                      MimeEncoding.Decode("=?iso-2022-jp?q?=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B?=", out encoding, out charset),
                      "iso-2022-jp");
      Assert.AreEqual(MimeEncodingMethod.QuotedPrintable, encoding, "iso-2022-jp");
      Assert.AreEqual(TestUtils.Encodings.Jis, charset, "iso-2022-jp");

      Assert.AreEqual("漢字abcかな123カナ",
                      MimeEncoding.Decode("=?shift_jis?Q?=8A=BF=8E=9Aabc=82=A9=82=C8123=83J=83i?=", out encoding, out charset),
                      "shift_jis");
      Assert.AreEqual(MimeEncodingMethod.QuotedPrintable, encoding, "shift_jis");
      Assert.AreEqual(TestUtils.Encodings.ShiftJis, charset, "shift_jis");

      Assert.AreEqual("漢字abcかな123カナ",
                      MimeEncoding.Decode("=?euc-jp?q?=B4=C1=BB=FAabc=A4=AB=A4=CA123=A5=AB=A5=CA?=", out encoding, out charset),
                      "euc-jp");
      Assert.AreEqual(MimeEncodingMethod.QuotedPrintable, encoding, "euc-jp");
      Assert.AreEqual(TestUtils.Encodings.EucJP, charset, "euc-jp");
    }

    [Test]
    public void TestDecodeQEncodingEscapedWhitespace()
    {
      Encoding charset;
      MimeEncodingMethod encoding;

      Assert.AreEqual("Amazon.co.jp ご注文の発送 (XXX-YYYYYYY-ZZZZZZZ)",
                      MimeEncoding.Decode("=?ISO-2022-JP?Q?Amazon.co.jp_=1B$B$4CmJ8$NH/Aw=1B(B_(XXX-YYYYYYY-ZZZZZZZ)?=", out encoding, out charset), "case1");
      Assert.AreEqual(MimeEncodingMethod.QuotedPrintable, encoding, "case1");
      Assert.AreEqual(TestUtils.Encodings.Jis, charset, "case1");

      Assert.AreEqual("Amazon.co.jp ご注文の確認",
                      MimeEncoding.Decode("=?UTF-8?Q?Amazon.co?= =?UTF-8?Q?.jp_=E3=81=94=E6=B3=A8=E6=96=87=E3=81=AE=E7=A2=BA=E8=AA=8D?=", out encoding, out charset), "case2");
      Assert.AreEqual(MimeEncodingMethod.QuotedPrintable, encoding, "case2");
      Assert.AreEqual(Encoding.UTF8, charset, "case2");
    }

    [Test]
    public void TestDecodeBEncoding()
    {
      Encoding charset;
      MimeEncodingMethod encoding;

      Assert.AreEqual("漢字abcかな123カナ",
                      MimeEncoding.Decode("=?utf-8?B?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?=", out encoding, out charset),
                      "utf8");
      Assert.AreEqual(MimeEncodingMethod.Base64, encoding, "utf8");
      Assert.AreEqual(Encoding.UTF8, charset, "utf8");

      Assert.AreEqual("漢字abcかな123カナ",
                      MimeEncoding.Decode("=?utf-7?b?K2J5SmJWdy1hYmMrTUVzd2FnLTEyMytNS3N3eWct?=", out encoding, out charset),
                      "utf7");
      Assert.AreEqual(MimeEncodingMethod.Base64, encoding, "utf7");
      Assert.AreEqual(Encoding.UTF7, charset, "utf7");

      Assert.AreEqual("漢字abcかな123カナ",
                      MimeEncoding.Decode("=?iso-2022-jp?B?GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC?=", out encoding, out charset),
                      "iso-2022-jp");
      Assert.AreEqual(MimeEncodingMethod.Base64, encoding, "iso-2022-jp");
      Assert.AreEqual(TestUtils.Encodings.Jis, charset, "iso-2022-jp");

      Assert.AreEqual("漢字abcかな123カナ",
                      MimeEncoding.Decode("=?shift_jis?b?ir+OmmFiY4KpgsgxMjODSoNp?=", out encoding, out charset),
                      "shift_jis");
      Assert.AreEqual(MimeEncodingMethod.Base64, encoding, "shift_jis");
      Assert.AreEqual(TestUtils.Encodings.ShiftJis, charset, "shift_jis");

      Assert.AreEqual("漢字abcかな123カナ",
                      MimeEncoding.Decode("=?euc-jp?B?tMG7+mFiY6SrpMoxMjOlq6XK?=", out encoding, out charset),
                      "euc-jp");
      Assert.AreEqual(MimeEncodingMethod.Base64, encoding, "euc-jp");
      Assert.AreEqual(TestUtils.Encodings.EucJP, charset, "euc-jp");
    }

    [Test]
    public void TestDecodeBEncodingBug()
    {
      Assert.AreEqual("【Microsoft】欽ちゃん球団の片岡安祐美さんが Office にチャレンジ! ワクワクの春 開幕!",
                      MimeEncoding.Decode("=?iso-2022-jp?B?GyRCIVobKEJNaWNyb3NvZnQbJEIhWzZWJEEkYyRzNWVDRBsoQg==?=" + 
                                          " =?iso-2022-jp?B?GyRCJE5KUjIsMEJNNEh+JDUkcyQsGyhCIE9mZmljZSAbJEIkSyVBGyhC?=" + 
                                          " =?iso-2022-jp?B?GyRCJWMlbCVzJTgbKEIhIBskQiVvJS8lbyUvJE49VRsoQiA=?=" + 
                                          " =?iso-2022-jp?B?GyRCMytLaxsoQiE=?="));

      Assert.AreEqual("santamartaさんの日記にコメントが登録されました",
                      MimeEncoding.Decode("=?ISO-2022-JP?B?c2FudGFtYXJ0YRskQiQ1JHMkTkZ8NS0kSyUzJWElcyVIJCxFUE8/GyhC?= =?ISO-2022-JP?B?GyRCJDUkbCReJDckPxsoQg==?="));

      Assert.AreEqual("初めまして。突然のメールに深くお",
                      MimeEncoding.Decode("=?iso-2022-jp?B?GyRCPWkkYSReJDckRiEjRk1BMyROJWEhPCVrJEs/PCQvJCobKEI=?="));

      Assert.AreEqual("ウイルスバスタークラブニュース3月号■3年分でプラス3カ月。大好評キャンペーンが間もなく終了/新生活スタート。登録内容の変更を！",
                      MimeEncoding.Decode("=?ISO-2022-JP?B?GyRCJSYlJCVrJTklUCU5JT8hPCUvJWklViVLJWUhPCU5GyhC?=3=?ISO-2022-JP?B?GyRCN24bKEI=?=" +
                                          " =?ISO-2022-JP?B?GyRCOWYiIxsoQg==?=3=?ISO-2022-JP?B?GyRCRy9KLCRHGyhC?=" +
                                          " =?ISO-2022-JP?B?GyRCJVclaSU5GyhC?=3=?ISO-2022-JP?B?GyRCJSs3biEjGyhC?=" +
                                          " =?ISO-2022-JP?B?GyRCQmc5JUk+JS0lYyVzJVohPCVzJCw0ViRiJEokLz0qTjsbKEI=?=" +
                                          " /=?ISO-2022-JP?B?GyRCPzdAODNoJTklPyE8JUghI0VQTz9GYk1GJE5KUTk5GyhC?=" +
                                          " =?ISO-2022-JP?B?GyRCJHIhKhsoQg==?="));

      if (Runtime.IsRunningOnMono)
        Assert.AreEqual("てすと①",
                        MimeEncoding.Decode("=?cp932?B?gsSCt4LGh0A=?="));
    }

    [Test]
    public void TestDecodeInvalidEncoding()
    {
      Assert.Throws<FormatException>(() => MimeEncoding.Decode("=?utf-8?x?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?="));
    }

    [Test]
    public void TestDecodeInvalidEncodingConvertToAlternativeText()
    {
      bool called = false;

      var ret = MimeEncoding.Decode("foo =?utf-8?x?baz?= bar", null, delegate(Encoding c, string m, string t) {
        called = true;
        Assert.AreEqual(Encoding.UTF8, c);
        Assert.AreEqual("x", m);
        Assert.AreEqual("baz", t);
        return "baz";
      });

      Assert.IsTrue(called, "called");
      Assert.AreEqual(ret, "foobazbar");

      Encoding charset;
      MimeEncodingMethod encoding;

      Assert.AreEqual("<alternative text>",
                      MimeEncoding.Decode("=?utf-8?x?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
                                          null,
                                          (e, m, t) => "<alternative text>",
                                          out encoding,
                                          out charset));

      Assert.AreEqual(Encoding.UTF8, charset);
      Assert.AreEqual(MimeEncodingMethod.None, encoding);
    }

    [Test]
    public void TestDecodeInvalidCharset()
    {
      try {
        Assert.AreEqual("漢字abcかな123カナ",
                        MimeEncoding.Decode("=?invalid?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?="),
                        "utf8");

        Assert.Fail("EncodingNotSupportedException not thrown");
      }
      catch (EncodingNotSupportedException ex) {
        Assert.AreEqual("invalid", ex.EncodingName, "EncodingName");
      }
    }

    [Test]
    public void TestDecodeValidCharsetSelectFallback()
    {
      bool called = false;
      EncodingSelectionCallback callback = delegate(string name) {
        called = true;
        return Encoding.ASCII;
      };
      MimeEncodingMethod encoding;
      Encoding charset;

      var ret = MimeEncoding.Decode("=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
                                    callback,
                                    out encoding,
                                    out charset);

      Assert.IsNotNull(ret);
      Assert.AreEqual("漢字abcかな123カナ", ret);
      Assert.IsFalse(called);
      Assert.AreEqual(MimeEncodingMethod.QEncoding, encoding);
      Assert.AreEqual(Encoding.UTF8, charset);
    }

    [Test]
    public void TestDecodeInvalidCharsetSelectFallback()
    {
      bool called = false;
      EncodingSelectionCallback callback = delegate(string name) {
        called = true;
        return Encoding.UTF8;
      };
      MimeEncodingMethod encoding;
      Encoding charset;

      var ret = MimeEncoding.Decode("=?invalid?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
                                    callback,
                                    out encoding,
                                    out charset);

      Assert.IsNotNull(ret);
      Assert.AreEqual("漢字abcかな123カナ", ret);
      Assert.IsTrue(called);
      Assert.AreEqual(MimeEncodingMethod.QEncoding, encoding);
      Assert.AreEqual(Encoding.UTF8, charset);
    }

    [Test]
    public void TestDecodeInvalidCharsetSelectFallbackReturnNull()
    {
      bool called = false;
      EncodingSelectionCallback callback = delegate(string name) {
        called = true;
        return null;
      };
      MimeEncodingMethod encoding = MimeEncodingMethod.None;
      Encoding charset = null;

      try {
        MimeEncoding.Decode("=?invalid?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
                            callback,
                            out encoding,
                            out charset);

        Assert.Fail("EncodingNotSupportedException not thrown");
      }
      catch (EncodingNotSupportedException ex) {
        Assert.AreEqual("invalid", ex.EncodingName, "EncodingName");
      }

      Assert.IsTrue(called);
      Assert.AreEqual(MimeEncodingMethod.None, encoding);
      Assert.IsNull(charset);
    }

    [Test]
    public void TestDecodeInvalidFormatConvertToAlternativeText()
    {
      Encoding charset;
      MimeEncodingMethod encoding;
      bool called = false;

      var ret = MimeEncoding.Decode("foo =?utf-8?q?===?= bar", null, delegate(Encoding c, string m, string t) {
        called = true;
        Assert.AreEqual(Encoding.UTF8, c);
        Assert.AreEqual("q", m);
        Assert.AreEqual("===", t);
        return "baz";
      }, out encoding, out charset);

      Assert.IsTrue(called, "called");
      Assert.AreEqual(ret, "foobazbar");
      Assert.AreEqual(MimeEncodingMethod.QEncoding, encoding);
      Assert.AreEqual(Encoding.UTF8, charset);
    }

    [Test]
    public void TestDecodeQEncodingInvalidFormat()
    {
      Assert.Throws<FormatException>(() => MimeEncoding.Decode("=?utf-8?q?===?="));
    }

    [Test]
    public void TestDecodeQEncodingInvalidFormatConvertToAlternativeText()
    {
      Assert.AreEqual("<alt>",
                      MimeEncoding.Decode("=?utf-8?q?===?=", null, (c, m, t) => "<alt>"));

      Assert.AreEqual("foo<alt>bar",
                      MimeEncoding.Decode("foo =?utf-8?q?===?= bar", null, (c, m, t) => "<alt>"));

      Assert.AreEqual("foo===bar",
                      MimeEncoding.Decode("foo =?utf-8?q?===?= bar", null, (c, m, t) => t));

      Assert.AreEqual("foobar",
                      MimeEncoding.Decode("foo =?utf-8?q?===?= bar", null, (c, m, t) => string.Empty));

      Assert.AreEqual("foo =?utf-8?q?===?= bar",
                      MimeEncoding.Decode("foo =?utf-8?q?===?= bar", null, (c, m, t) => null));
    }

    [Test]
    public void TestDecodeBEncodingInvalidFormat()
    {
      Assert.Throws<FormatException>(() => MimeEncoding.Decode("=?utf-8?b?****?="));
    }

    [Test]
    public void TestDecodeBEncodingInvalidFormatConvertToAlternativeText()
    {
      Assert.AreEqual("<alt>",
                      MimeEncoding.Decode("=?utf-8?b?****?=", null, (c, m, t) => "<alt>"));

      Assert.AreEqual("foo<alt>bar",
                      MimeEncoding.Decode("foo =?utf-8?b?****?= bar", null, (c, m, t) => "<alt>"));

      Assert.AreEqual("foo****bar",
                      MimeEncoding.Decode("foo =?utf-8?b?****?= bar", null, (c, m, t) => t));

      Assert.AreEqual("foobar",
                      MimeEncoding.Decode("foo =?utf-8?b?****?= bar", null, (c, m, t) => string.Empty));

      Assert.AreEqual("foo =?utf-8?b?****?= bar",
                      MimeEncoding.Decode("foo =?utf-8?b?****?= bar", null, (c, m, t) => null));
    }
  }
}
