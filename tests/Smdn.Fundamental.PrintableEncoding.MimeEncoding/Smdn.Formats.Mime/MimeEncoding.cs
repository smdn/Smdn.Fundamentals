// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;

using NUnit.Framework;

using Smdn.Test.NUnit;
using Smdn.Text.Encodings;

namespace Smdn.Formats.Mime;

[TestFixture]
public class MimeEncodingTests {
  [SetUp]
  public void SetUp()
  {
#if !NETFRAMEWORK
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
  }

  [TestCase("漢字abcかな123カナ", MimeEncodingMethod.Base64, "utf-8", "=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?=")]
  [TestCase("漢字abcかな123カナ", MimeEncodingMethod.QuotedPrintable, "utf-8", "=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=")]
  public void Encode_NoFolding(string input, MimeEncodingMethod encoding, string charset, string expected)
    => Assert.AreEqual(
      expected,
      MimeEncoding.Encode(input, encoding, Encoding.GetEncoding(charset))
    );

  [TestCase(
    "漢字abcかな123カナ漢字abcかな123カナ漢字abcかな123カナ",
    MimeEncodingMethod.Base64,
    "utf-8",
    76,
    "=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?=\r\n\t=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?="
  )]
  [TestCase(
    "漢字abcかな123カナ漢字abcかな123カナ",
    MimeEncodingMethod.QuotedPrintable,
    "utf-8",
    76,
    "=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=\r\n\t=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB?=\r\n\t=?utf-8?q?=E3=83=8A?="
  )]
  public void Encode_WithFolding(
    string input,
    MimeEncodingMethod encoding,
    string charset,
    int foldingLimit,
    string expected
  )
    => Assert.AreEqual(
      expected,
      MimeEncoding.Encode(input, encoding, Encoding.GetEncoding(charset), foldingLimit, 0)
    );

  [TestCase(
    "漢字abcかな123カナ漢字abcかな123カナ漢字abcかな123カナ",
    MimeEncodingMethod.Base64,
    "utf-8",
    64,
    9,
    "\n ",
    "=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK5ryi5a2X?=\n =?utf-8?b?YWJj44GL44GqMTIz44Kr44OK5ryi5a2XYWJj44GL44GqMTIz44Kr?=\n =?utf-8?b?44OK?="
  )]
  [TestCase(
    "漢字abcかな123カナ漢字abcかな123カナ漢字abcかな123カナ",
    MimeEncodingMethod.QuotedPrintable,
    "utf-8",
    64,
    9,
    "\n ",
    "=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123?=\n =?utf-8?q?=E3=82=AB=E3=83=8A=E6=BC=A2=E5=AD=97abc=E3=81=8B?=\n =?utf-8?q?=E3=81=AA123=E3=82=AB=E3=83=8A=E6=BC=A2=E5=AD=97abc?=\n =?utf-8?q?=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?="
  )]
  public void Encode_SpecifiedFormat(
    string input,
    MimeEncodingMethod encoding,
    string charset,
    int foldingLimit,
    int foldingOffset,
    string foldingString,
    string expected
  )
    => Assert.AreEqual(
      expected,
      MimeEncoding.Encode(input, encoding, Encoding.GetEncoding(charset), foldingLimit, foldingOffset, foldingString)
    );

  [Test]
  public void Encode_InvalidCharset()
    => Assert.Throws<ArgumentNullException>(() => MimeEncoding.Encode("漢字abcかな123カナ", MimeEncodingMethod.Base64, null!));

  [TestCase(0x7fffffff)]
  [TestCase(MimeEncodingMethod.None)]
  public void Encode_InvalidEncoding(MimeEncodingMethod encoding)
    => Assert.Throws<ArgumentException>(() => MimeEncoding.Encode("漢字abcかな123カナ", encoding, Encoding.UTF8));

  [Test]
  public void Decode_ContainsLanguageSpecification()
  {
    Assert.AreEqual(
      "Keith Moore",
      MimeEncoding.Decode("=?US-ASCII*EN?Q?Keith_Moore?=", out var encoding, out var charset)
    );

    Assert.AreEqual(MimeEncodingMethod.QEncoding, encoding);
    Assert.AreEqual(Encoding.ASCII, charset);
  }

  // http://tools.ietf.org/html/rfc2047#section-8
  // 8. Examples
  [TestCase("=?ISO-8859-1?Q?a?=", "a")]
  //[TestCase("=?ISO-8859-1?Q?a?= b", "a b")] // TODO
  [TestCase("=?ISO-8859-1?Q?a?= =?ISO-8859-1?Q?b?=", "ab")]
  [TestCase("=?ISO-8859-1?Q?a?=  =?ISO-8859-1?Q?b?=", "ab")]
  [TestCase("=?ISO-8859-1?Q?a?=\n =?ISO-8859-1?Q?b?=", "ab")]
  [TestCase("=?ISO-8859-1?Q?a_b?=", "a b")]
  [TestCase("=?ISO-8859-1?Q?a?= =?ISO-8859-2?Q?_b?=", "a b")]
  public void Decode_QEncoding(string input, string expected)
    => Assert.AreEqual(expected, MimeEncoding.Decode(input));

  [TestCase("漢 字\tかな", "utf-8", "=?utf-8?q?=E6=BC=A2=20=E5=AD=97=09=E3=81=8B=E3=81=AA?=")]
  public void Encode_QEncoding_Whitespaces(string input, string charset, string expected)
    => Assert.AreEqual(expected, MimeEncoding.Encode(input, MimeEncodingMethod.QEncoding, Encoding.GetEncoding(charset)));

  [TestCase("=?utf-8?q?=E6=BC=A2=20=E5=AD=97=09=E3=81=8B=E3=81=AA?=", "漢 字\tかな")]
  [TestCase("=?utf-8?q?=E6=BC=A2_=E5=AD=97=09=E3=81=8B=E3=81=AA?=", "漢 字\tかな")]
  public void Decode_QEncoding_Whitespaces(string input, string expected)
    => Assert.AreEqual(expected, MimeEncoding.Decode(input));

  [TestCase("a?b=c_d", "utf-8", "=?utf-8?q?a=3Fb=3Dc=5Fd?=")]
  public void Encode_QEncoding_SpecialChars(string input, string charset, string expected)
    => Assert.AreEqual(expected, MimeEncoding.Encode(input, MimeEncodingMethod.QEncoding, Encoding.GetEncoding(charset)));

  [TestCase("=?utf-8?q?a=3Fb=3Dc=5Fd?=", "a?b=c_d")]
  [TestCase("=?utf-8?q?a=3Fb=3Dc_d?=", "a?b=c d")]
  public void Decode_QEncoding_SpecialChars(string input, string expected)
    => Assert.AreEqual(expected, MimeEncoding.Decode(input));

  [TestCase("=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=", "漢字abcかな123カナ", "UTF-8")]
  [TestCase("=?iso-2022-jp?q?=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B?=", "漢字abcかな123カナ", "iso-2022-jp")]
  [TestCase("=?shift_jis?Q?=8A=BF=8E=9Aabc=82=A9=82=C8123=83J=83i?=", "漢字abcかな123カナ", "shift_jis")]
  [TestCase("=?euc-jp?q?=B4=C1=BB=FAabc=A4=AB=A4=CA123=A5=AB=A5=CA?=", "漢字abcかな123カナ", "euc-jp")]
#if !NET5_0_OR_GREATER // SYSLIB0001
  [TestCase("=?utf-7?Q?+byJbVw-abc+MEswag-123+MKswyg-?=", "漢字abcかな123カナ", "UTF-7")]
#endif
  public void Decode_QEncoding_Charsets(string input, string expectedResult, string expectedCharset)
  {
    Assert.AreEqual(
      expectedResult,
      MimeEncoding.Decode(input, out var encoding, out var charset)
    );
    Assert.AreEqual(MimeEncodingMethod.QuotedPrintable, encoding, nameof(encoding));
    Assert.AreEqual(Encoding.GetEncoding(expectedCharset), charset, nameof(charset));
  }

  [TestCase("=?ISO-2022-JP?Q?Amazon.co.jp_=1B$B$4CmJ8$NH/Aw=1B(B_(XXX-YYYYYYY-ZZZZZZZ)?=", "Amazon.co.jp ご注文の発送 (XXX-YYYYYYY-ZZZZZZZ)", "iso-2022-jp")]
  [TestCase("=?UTF-8?Q?Amazon.co?= =?UTF-8?Q?.jp_=E3=81=94=E6=B3=A8=E6=96=87=E3=81=AE=E7=A2=BA=E8=AA=8D?=", "Amazon.co.jp ご注文の確認", "UTF-8")]
  public void Decode_QEncoding_EscapedWhitespace(string input, string expectedResult, string expectedCharset)
  {
    Assert.AreEqual(
      expectedResult,
      MimeEncoding.Decode(input, out var encoding, out var charset)
    );
    Assert.AreEqual(MimeEncodingMethod.QuotedPrintable, encoding, nameof(encoding));
    Assert.AreEqual(Encoding.GetEncoding(expectedCharset), charset, nameof(charset));
  }

  [TestCase("=?utf-8?B?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?=", "漢字abcかな123カナ", "UTF-8")]
  [TestCase("=?iso-2022-jp?B?GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC?=", "漢字abcかな123カナ", "iso-2022-jp")]
  [TestCase("=?shift_jis?b?ir+OmmFiY4KpgsgxMjODSoNp?=", "漢字abcかな123カナ", "shift_jis")]
  [TestCase("=?euc-jp?B?tMG7+mFiY6SrpMoxMjOlq6XK?=", "漢字abcかな123カナ", "euc-jp")]
#if !NET5_0_OR_GREATER // SYSLIB0001
  [TestCase("=?utf-7?b?K2J5SmJWdy1hYmMrTUVzd2FnLTEyMytNS3N3eWct?=", "漢字abcかな123カナ", "UTF-7")]
#endif
  public void Decode_BEncoding(string input, string expectedResult, string expectedCharset)
  {
    Assert.AreEqual(
      expectedResult,
      MimeEncoding.Decode(input, out var encoding, out var charset)
    );
    Assert.AreEqual(MimeEncodingMethod.Base64, encoding, "utf8");
    Assert.AreEqual(Encoding.GetEncoding(expectedCharset), charset, "utf8");
  }

  [TestCase(
    "=?iso-2022-jp?B?GyRCIVobKEJNaWNyb3NvZnQbJEIhWzZWJEEkYyRzNWVDRBsoQg==?="
    + " =?iso-2022-jp?B?GyRCJE5KUjIsMEJNNEh+JDUkcyQsGyhCIE9mZmljZSAbJEIkSyVBGyhC?="
    + " =?iso-2022-jp?B?GyRCJWMlbCVzJTgbKEIhIBskQiVvJS8lbyUvJE49VRsoQiA=?="
    + " =?iso-2022-jp?B?GyRCMytLaxsoQiE=?=",
    "【Microsoft】欽ちゃん球団の片岡安祐美さんが Office にチャレンジ! ワクワクの春 開幕!"
  )]
  [TestCase(
    "=?ISO-2022-JP?B?c2FudGFtYXJ0YRskQiQ1JHMkTkZ8NS0kSyUzJWElcyVIJCxFUE8/GyhC?= =?ISO-2022-JP?B?GyRCJDUkbCReJDckPxsoQg==?=",
    "santamartaさんの日記にコメントが登録されました"
  )]
  [TestCase(
    "=?iso-2022-jp?B?GyRCPWkkYSReJDckRiEjRk1BMyROJWEhPCVrJEs/PCQvJCobKEI=?=",
    "初めまして。突然のメールに深くお"
  )]
  [TestCase(
    "=?ISO-2022-JP?B?GyRCJSYlJCVrJTklUCU5JT8hPCUvJWklViVLJWUhPCU5GyhC?=3=?ISO-2022-JP?B?GyRCN24bKEI=?="
    + " =?ISO-2022-JP?B?GyRCOWYiIxsoQg==?=3=?ISO-2022-JP?B?GyRCRy9KLCRHGyhC?="
    + " =?ISO-2022-JP?B?GyRCJVclaSU5GyhC?=3=?ISO-2022-JP?B?GyRCJSs3biEjGyhC?="
    + " =?ISO-2022-JP?B?GyRCQmc5JUk+JS0lYyVzJVohPCVzJCw0ViRiJEokLz0qTjsbKEI=?="
    + " /=?ISO-2022-JP?B?GyRCPzdAODNoJTklPyE8JUghI0VQTz9GYk1GJE5KUTk5GyhC?="
    + " =?ISO-2022-JP?B?GyRCJHIhKhsoQg==?=",
    "ウイルスバスタークラブニュース3月号■3年分でプラス3カ月。大好評キャンペーンが間もなく終了/新生活スタート。登録内容の変更を！"
  )]
  public void Decode_BEncoding_Bug(string input, string expected)
    => Assert.AreEqual(expected, MimeEncoding.Decode(input));

  [TestCase("=?cp932?B?gsSCt4LGh0A=?=", "てすと①")]
  public void Decode_CharsetMightNotBeSupported(string input, string expected)
  {
    try {
      Assert.AreEqual(expected, MimeEncoding.Decode(input));
    }
    catch (EncodingNotSupportedException ex) {
      Assert.Inconclusive($"{nameof(EncodingNotSupportedException)}: {ex.Message}");
    }
  }

  [Test]
  public void Decode_InvalidEncoding()
    => Assert.Throws<FormatException>(() => MimeEncoding.Decode("=?utf-8?x?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?="));

  [Test]
  public void Decode_InvalidEncoding_ConvertToAlternativeText()
  {
    bool called = false;

    var ret = MimeEncoding.Decode(
      "foo =?utf-8?x?baz?= bar",
      null,
      (Encoding c, string m, string t) => {
        called = true;
        Assert.AreEqual(Encoding.UTF8, c);
        Assert.AreEqual("x", m);
        Assert.AreEqual("baz", t);
        return "baz";
      }
    );

    Assert.IsTrue(called, "called");
    Assert.AreEqual("foobazbar", ret);

    Assert.AreEqual(
      "<alternative text>",
      MimeEncoding.Decode(
        "=?utf-8?x?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
        null,
        (c, m, t) => "<alternative text>",
        out var encoding,
        out var charset
      )
    );
    Assert.AreEqual(Encoding.UTF8, charset);
    Assert.AreEqual(MimeEncodingMethod.None, encoding);
  }

  [Test]
  public void Decode_CharsetNotBeSupported()
  {
    var ex = Assert.Throws<EncodingNotSupportedException>(
      () => MimeEncoding.Decode("=?invalid?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=")
    );

    Assert.AreEqual("invalid", ex!.EncodingName, "EncodingName");
  }

  [Test]
  public void Decode_ValidCharset_SelectFallback()
  {
    string ret = default;
    MimeEncodingMethod encoding = default;
    Encoding charset = default;

    Assert.DoesNotThrow(() => {
      ret = MimeEncoding.Decode(
        "=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
        name => throw new InvalidOperationException("callback must not be called"),
        out encoding,
        out charset
      );
    });

    Assert.AreEqual("漢字abcかな123カナ", ret);
    Assert.AreEqual(MimeEncodingMethod.QEncoding, encoding);
    Assert.AreEqual(Encoding.UTF8, charset);
  }

  [Test]
  public void Decode_InvalidCharset_SelectFallback()
  {
    var called = false;
    EncodingSelectionCallback callback = (string name) => {
      called = true;
      return Encoding.UTF8;
    };
    string ret = default;
    MimeEncodingMethod encoding = default;
    Encoding charset = default;

    Assert.DoesNotThrow(() => {
      ret = MimeEncoding.Decode(
        "=?x-invalid?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
        callback,
        out encoding,
        out charset
      );
    });

    Assert.AreEqual("漢字abcかな123カナ", ret);
    Assert.IsTrue(called);
    Assert.AreEqual(MimeEncodingMethod.QEncoding, encoding);
    Assert.AreEqual(Encoding.UTF8, charset);
  }

  [Test]
  public void Decode_InvalidCharset_SelectFallback_ReturnNull()
  {
    var called = false;
    EncodingSelectionCallback callback = (string name) => {
      called = true;
      return null;
    };
    MimeEncodingMethod encoding = default;
    Encoding charset = default;

    var ex = Assert.Throws<EncodingNotSupportedException>(
      () => MimeEncoding.Decode(
        "=?x-invalid?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
        callback,
        out encoding,
        out charset
      )
    );

    Assert.AreEqual("x-invalid", ex!.EncodingName, "EncodingName");
    Assert.IsTrue(called);
    Assert.AreEqual(MimeEncodingMethod.None, encoding);
    Assert.IsNull(charset);
  }

  [Test]
  public void Decode_InvalidFormat_ConvertToAlternativeText()
  {
    string ret = default;
    Encoding charset = default;
    MimeEncodingMethod encoding = default;

    Assert.DoesNotThrow(() => {
      ret = MimeEncoding.Decode(
        "foo =?utf-8?q?===?= bar",
        null,
        (Encoding c, string m, string t) => {
          Assert.AreEqual(Encoding.UTF8, c);
          Assert.AreEqual("q", m);
          Assert.AreEqual("===", t);
          return "baz";
        },
        out encoding,
        out charset
      );
    });

    Assert.AreEqual("foobazbar", ret);
    Assert.AreEqual(MimeEncodingMethod.QEncoding, encoding);
    Assert.AreEqual(Encoding.UTF8, charset);
  }

  [TestCase("=?utf-8?q?===?=")]
  public void Decode_QEncoding_InvalidFormat(string input)
    => Assert.Throws<FormatException>(() => MimeEncoding.Decode(input));

  [TestCase("=?utf-8?q?===?=", null, "=?utf-8?q?===?=")]
  [TestCase("=?utf-8?q?===?=", "", "")]
  [TestCase("=?utf-8?q?===?=", "<alt>", "<alt>")]
  [TestCase("foo =?utf-8?q?===?= bar", null, "foo =?utf-8?q?===?= bar")]
  [TestCase("foo =?utf-8?q?===?= bar", "", "foobar")]
  [TestCase("foo =?utf-8?q?===?= bar", "<alt>", "foo<alt>bar")]
  public void Decode_QEncoding_InvalidFormat_ConvertToAlternativeText(string input, string replacement, string expected)
    => Assert.AreEqual(
      expected,
      MimeEncoding.Decode(input, null, (c, m, t) => replacement)
    );

  [TestCase("=?utf-8?q?===?=", "===")]
  [TestCase("foo =?utf-8?q?===?= bar", "foo===bar")]
  public void Decode_QEncoding_InvalidFormat_ConvertToInputText(string input, string expected)
    => Assert.AreEqual(
      expected,
      MimeEncoding.Decode(input, null, static (c, m, t) => t)
    );

  [TestCase("=?utf-8?b?****?=")]
  public void Decode_BEncoding_InvalidFormat(string input)
    => Assert.Throws<FormatException>(() => MimeEncoding.Decode(input));


  [TestCase("=?utf-8?b?****?=", null, "=?utf-8?b?****?=")]
  [TestCase("=?utf-8?b?****?=", "", "")]
  [TestCase("=?utf-8?b?****?=", "<alt>", "<alt>")]
  [TestCase("foo =?utf-8?b?****?= bar", null, "foo =?utf-8?b?****?= bar")]
  [TestCase("foo =?utf-8?b?****?= bar", "", "foobar")]
  [TestCase("foo =?utf-8?b?****?= bar", "<alt>", "foo<alt>bar")]
  public void Decode_BEncoding_InvalidFormat_ConvertToAlternativeText(string input, string replacement, string expected)
    => Assert.AreEqual(
      expected,
      MimeEncoding.Decode(input, null, (c, m, t) => replacement)
    );

  [TestCase("=?utf-8?b?****?=", "****")]
  [TestCase("foo =?utf-8?b?****?= bar", "foo****bar")]
  public void Decode_BEncoding_InvalidFormat_ConvertToInputText(string input, string expected)
    => Assert.AreEqual(
      expected,
      MimeEncoding.Decode(input, null, static (c, m, t) => t)
    );
}
