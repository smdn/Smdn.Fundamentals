using System;
using System.Text;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture]
  public class TextConvertTest {
    [Test]
    public void TestToLowerCaseHexString()
    {
      Assert.AreEqual("", TextConvert.ToLowerCaseHexString(new byte[] {}), "empty");
      Assert.AreEqual("0123456789abcdef",
                      TextConvert.ToLowerCaseHexString(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}));
    }

    [Test]
    public void TestToUpperCaseHexString()
    {
      Assert.AreEqual("", TextConvert.ToLowerCaseHexString(new byte[] {}), "empty");
      Assert.AreEqual("0123456789ABCDEF",
                      TextConvert.ToUpperCaseHexString(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}));
    }

    [Test]
    public void TestToLowerCaseHexByteArray()
    {
      Assert.AreEqual(new byte[] {}, TextConvert.ToLowerCaseHexByteArray(new byte[] {}), "empty");
      Assert.AreEqual(new byte[] {0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66},
                      TextConvert.ToLowerCaseHexByteArray(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}));
    }

    [Test]
    public void TestToUpperCaseHexByteArray()
    {
      Assert.AreEqual(new byte[] {}, TextConvert.ToUpperCaseHexByteArray(new byte[] {}), "empty");
      Assert.AreEqual(new byte[] {0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46},
                      TextConvert.ToUpperCaseHexByteArray(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}));
    }

    [Test]
    public void TestFromLowerCaseHexString()
    {
      Assert.AreEqual(new byte[] {}, TextConvert.FromLowerCaseHexString(""), "empty");
      Assert.AreEqual(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}, TextConvert.FromLowerCaseHexString("0123456789abcdef"));

      try {
        TextConvert.FromLowerCaseHexString("0123456789abcde");
        Assert.Fail("invalid length, FormatException not thrown");
      }
      catch (FormatException) {
      }

      try {
        TextConvert.FromLowerCaseHexString("0123456789abcdeg");
        Assert.Fail("FormatException not thrown");
      }
      catch (FormatException) {
      }

      try {
        TextConvert.FromLowerCaseHexString("0123456789abcdeF");
        Assert.Fail("FormatException not thrown");
      }
      catch (FormatException) {
      }
    }

    [Test]
    public void TestFromUpperCaseHexString()
    {
      Assert.AreEqual(new byte[] {}, TextConvert.FromUpperCaseHexString(""), "empty");
      Assert.AreEqual(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}, TextConvert.FromUpperCaseHexString("0123456789ABCDEF"));

      try {
        TextConvert.FromUpperCaseHexString("0123456789ABCDE");
        Assert.Fail("FormatException not thrown");
      }
      catch (FormatException) {
      }

      try {
        TextConvert.FromUpperCaseHexString("0123456789ABCDEG");
        Assert.Fail("FormatException not thrown");
      }
      catch (FormatException) {
      }

      try {
        TextConvert.FromUpperCaseHexString("0123456789ABCDEf");
        Assert.Fail("FormatException not thrown");
      }
      catch (FormatException) {
      }
    }

    [Test]
    public void TestFromHexString()
    {
      Assert.AreEqual(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}, TextConvert.FromHexString("0123456789AbcDef"));
    }

    [Test]
    public void TestToCSVString()
    {
      Assert.AreEqual("a,b,c", TextConvert.ToCSVString(new[] {"a", "b", "c"}));
      Assert.AreEqual("abc,\"d\"\"e\"\"f\",g'h'i", TextConvert.ToCSVString(new[] {"abc", "d\"e\"f", "g'h'i"}));
    }

    [Test]
    public void TestFromCSVString()
    {
      Assert.AreEqual(new[] {"a", "b", "c"}, TextConvert.FromCSVString("a,b,c"));
      Assert.AreEqual(new[] {"abc", "d\"e\"f", "g'h'i"}, TextConvert.FromCSVString("abc,\"d\"\"e\"\"f\",g'h'i"));
    }

    [Test]
    public void TestToBase64ByteArray()
    {
      Assert.AreEqual(new byte[] {0x59, 0x6d, 0x46, 0x7a, 0x5a, 0x54, 0x59, 0x30},
                      TextConvert.ToBase64ByteArray(new byte[] {0x62, 0x61, 0x73, 0x65, 0x36, 0x34}));
    }

    [Test]
    public void TestFromBase64ByteArray()
    {
      Assert.AreEqual(new byte[] {0x62, 0x61, 0x73, 0x65, 0x36, 0x34},
                      TextConvert.FromBase64ByteArray(new byte[] {0x59, 0x6d, 0x46, 0x7a, 0x5a, 0x54, 0x59, 0x30}));
    }

    [Test]
    public void TestTransformToBase64()
    {
      foreach (var test in new[] {
        new {Data = new byte[] {0xfb},              ExpectedBase64 = "+w==", Expected2152Base64 = "+w",   Expected3501Base64 = "+w"},
        new {Data = new byte[] {0xfb, 0xf0},        ExpectedBase64 = "+/A=", Expected2152Base64 = "+/A",  Expected3501Base64 = "+,A"},
        new {Data = new byte[] {0xfb, 0xf0, 0x00},  ExpectedBase64 = "+/AA", Expected2152Base64 = "+/AA", Expected3501Base64 = "+,AA"},
      }) {
        Assert.AreEqual(test.ExpectedBase64, TextConvert.ToBase64String(test.Data), "Base64");
        Assert.AreEqual(test.Expected2152Base64, TextConvert.ToRFC2152ModifiedBase64String(test.Data), "RFC2152 Base64");
        Assert.AreEqual(test.Expected3501Base64, TextConvert.ToRFC3501ModifiedBase64String(test.Data), "RFC3501 Base64");
      }
    }

    [Test]
    public void TestTransformFromBase64()
    {
      foreach (var test in new[] {
        new {Expected = new byte[] {0xfb},              DataBase64 = "+w==", Data2152Base64 = "+w",   Data3501Base64 = "+w"},
        new {Expected = new byte[] {0xfb, 0xf0},        DataBase64 = "+/A=", Data2152Base64 = "+/A",  Data3501Base64 = "+,A"},
        new {Expected = new byte[] {0xfb, 0xf0, 0x00},  DataBase64 = "+/AA", Data2152Base64 = "+/AA", Data3501Base64 = "+,AA"},
      }) {
        Assert.AreEqual(test.Expected, TextConvert.FromBase64StringToByteArray(test.DataBase64), "Base64");
        Assert.AreEqual(test.Expected, TextConvert.FromRFC2152ModifiedBase64StringToByteArray(test.Data2152Base64), "RFC2152 Base64");
        Assert.AreEqual(test.Expected, TextConvert.FromRFC3501ModifiedBase64StringToByteArray(test.Data3501Base64), "RFC3501 Base64");
      }
    }

    [Test]
    public void TestFromModifiedUTF7String()
    {
      Assert.AreEqual("INBOX.日本語", TextConvert.FromModifiedUTF7String("INBOX.&ZeVnLIqe-"));

      Assert.AreEqual("&日&-本-&語-", TextConvert.FromModifiedUTF7String("&-&ZeU-&--&Zyw--&-&ip4--"));

      Assert.AreEqual("~peter/mail/台北/日本語", TextConvert.FromModifiedUTF7String("~peter/mail/&U,BTFw-/&ZeVnLIqe-"));

      Assert.AreEqual("☺!", TextConvert.FromModifiedUTF7String("&Jjo-!"), "☺");

      // padding: 0
      Assert.AreEqual("下書き", TextConvert.FromModifiedUTF7String("&Tgtm+DBN-"));
      // padding: 1
      Assert.AreEqual("サポート", TextConvert.FromModifiedUTF7String("&MLUw3TD8MMg-"));
      // padding: 2
      Assert.AreEqual("迷惑メール", TextConvert.FromModifiedUTF7String("&j,dg0TDhMPww6w-"));
    }

    [Test, ExpectedException(typeof(FormatException))]
    public void TestFromModifiedUTF7StringIncorrectForm()
    {
      TextConvert.FromModifiedUTF7String("&Tgtm+DBN-&");
    }

    [Test]
    public void TestFromModifiedUTF7StringBroken()
    {
      Assert.AreEqual("下書き", TextConvert.FromModifiedUTF7String("&Tgtm+DBN"));
      Assert.AreEqual("Tgtm+DBN-", TextConvert.FromModifiedUTF7String("Tgtm+DBN-"));
    }

    [Test]
    public void TestToModifiedUTF7String()
    {
      Assert.AreEqual("INBOX.&ZeVnLIqe-", TextConvert.ToModifiedUTF7String("INBOX.日本語"));

      Assert.AreEqual("&-&ZeU-&--&Zyw--&-&ip4--", TextConvert.ToModifiedUTF7String("&日&-本-&語-"));

      Assert.AreEqual("~peter/mail/&U,BTFw-/&ZeVnLIqe-", TextConvert.ToModifiedUTF7String("~peter/mail/台北/日本語"));

      Assert.AreEqual("&Jjo-!", TextConvert.ToModifiedUTF7String("☺!"), "☺");

      // padding: 0
      Assert.AreEqual("&Tgtm+DBN-", TextConvert.ToModifiedUTF7String("下書き"));
      // padding: 1
      Assert.AreEqual("&MLUw3TD8MMg-", TextConvert.ToModifiedUTF7String("サポート"));
      // padding: 2
      Assert.AreEqual("&j,dg0TDhMPww6w-", TextConvert.ToModifiedUTF7String("迷惑メール"));
    }

    [Test]
    public void TestToPercentEncodedString()
    {
      Assert.AreEqual("012abcABC-._~%21%22%23%24%E6%97%A5%E6%9C%AC%E8%AA%9E", TextConvert.ToPercentEncodedString("012abcABC-._~!\"#$日本語", Encoding.UTF8));
      Assert.AreEqual("%93%FA%96%7B%8C%EA", TextConvert.ToPercentEncodedString("日本語", sjis));
      Assert.AreEqual("%C6%FC%CB%DC%B8%EC", TextConvert.ToPercentEncodedString("日本語", eucjp));
    }

    [Test]
    public void TestFromPercentEncodedString()
    {
      Assert.AreEqual("012abcABC-._~!\"#$日本語", TextConvert.FromPercentEncodedString("012abcABC-._~%21%22%23%24%e6%97%a5%e6%9c%ac%e8%aa%9e", Encoding.UTF8));
      Assert.AreEqual("日本語", TextConvert.FromPercentEncodedString("%93%fa%96%7B%8C%EA", sjis));
      Assert.AreEqual("日本語", TextConvert.FromPercentEncodedString("%c6%Fc%cb%Dc%b8%eC", eucjp));
    }

    [Test]
    public void TestFromPercentEncodedStringDecodePlusToSpace()
    {
      Assert.AreEqual("ABC+DEF", TextConvert.FromPercentEncodedString("ABC+DEF", false));
      Assert.AreEqual("ABC DEF", TextConvert.FromPercentEncodedString("ABC+DEF", true));
    }

    [Test]
    public void TestConvertQuotedPrintableDecodableJapanese()
    {
      foreach (var encoding in new[] {
        jis,
        sjis,
        eucjp,
        Encoding.BigEndianUnicode,
        Encoding.UTF7,
        Encoding.UTF8}) {

        AssertUnquotable(encoding, "ascii-text");
        AssertUnquotable(encoding, "非ASCII文字");
        AssertUnquotable(encoding, "日本語");
        AssertUnquotable(encoding, new string('漢', 10));
        AssertUnquotable(encoding, new string('漢', 40));
        AssertUnquotable(encoding, "途中で\r\n改行と\tイ ン デ ン ト\tした\r\n");
        AssertUnquotable(encoding, "*123456789*123456789*123456789*123456789*123456789*123456789*123456789*123456789");
        AssertUnquotable(encoding, " 1 3 5 7 9 1 3 5 7 9 1 3 5 7 9 1 3 5 7 9 1 3 5 7 9 1 3 5 7 9 1 3 5 7 9 1 3 5 7 9");
        AssertUnquotable(encoding, "0 2 4 6 8 0 2 4 6 8 0 2 4 6 8 0 2 4 6 8 0 2 4 6 8 0 2 4 6 8 0 2 4 6 8 0 2 4 6 8 ");
        AssertUnquotable(encoding, "\t1\t3\t5\t7\t9\t1\t3\t5\t7\t9\t1\t3\t5\t7\t9\t1\t3\t5\t7\t9\t1\t3\t5\t7\t9\t1\t3\t5\t7\t9\t1\t3\t5\t7\t9\t1\t3\t5\t7\t9");
        AssertUnquotable(encoding, "0\t2\t4\t6\t8\t0\t2\t4\t6\t8\t0\t2\t4\t6\t8\t0\t2\t4\t6\t8\t0\t2\t4\t6\t8\t0\t2\t4\t6\t8\t0\t2\t4\t6\t8\t0\t2\t4\t6\t8\t");
        AssertUnquotable(encoding, "\r\n1\r\n3\r\n5\r\n7\r\n9\r\n1\r\n3\r\n5\r\n7\r\n9\r\n1\r\n3\r\n5\r\n7\r\n9\r\n1\r\n3\r\n5\r\n7\r\n9\r\n1\r\n3\r\n5\r\n7\r\n9\r\n1\r\n3\r\n5\r\n7\r\n9\r\n1\r\n3\r\n5\r\n7\r\n9\r\n1\r\n3\r\n5\r\n7\r\n9");
        AssertUnquotable(encoding, "0\r\n2\r\n4\r\n6\r\n8\r\n0\r\n2\r\n4\r\n6\r\n8\r\n0\r\n2\r\n4\r\n6\r\n8\r\n0\r\n2\r\n4\r\n6\r\n8\r\n0\r\n2\r\n4\r\n6\r\n8\r\n0\r\n2\r\n4\r\n6\r\n8\r\n0\r\n2\r\n4\r\n6\r\n8\r\n0\r\n2\r\n4\r\n6\r\n8\r\n");
        AssertUnquotable(encoding, "漢字かなカナ１２３漢字かなカナ１２３漢字かなカナ１２３漢字かなカナ１２３漢字かなカナ１２３");
        AssertUnquotable(encoding, "漢\t字\rか\nな\tカ\rナ\n１\t２\r３\n漢\t字\rか\nな\tカ\rナ\n１\t２\r３\n漢\t字\rか\nな\tカ\rナ\n１\t２\r３\n");
      }
    }

    private void AssertUnquotable(Encoding encoding, string text)
    {
      try {
        var unquoted = TextConvert.FromQuotedPrintableString((TextConvert.ToQuotedPrintableString(text, encoding)), encoding);

        Assert.AreEqual(text, unquoted, "with " + encoding.EncodingName);
      }
      catch (FormatException ex) {
        Assert.Fail("failed encoding:{0} text:{1} exception:{2}", encoding.EncodingName, text, ex);
      }
    }

    [Test]
    public void TestFromQuotedPrintableString()
    {
      Assert.AreEqual("Now's the time for all folk to come to the aid of their country.",
                      TextConvert.FromQuotedPrintableString("Now's the time =\r\nfor all folk to come=\r\n to the aid of their country."));

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromQuotedPrintableString("=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A", Encoding.UTF8),
                      "utf8");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromQuotedPrintableString("=B4=C1=BB=FAabc=A4=AB=A4=CA123=A5=AB=A5=CA", eucjp),
                      "eucjp");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromQuotedPrintableString("=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B", jis),
                      "jis");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromQuotedPrintableString("=8A=BF=8E=9Aabc=82=A9=82=C8123=83J=83i", sjis),
                      "shift-jis");
    }

    [Test]
    public void TestFromQuotedPrintableStringWithSoftNewline()
    {
      Assert.AreEqual("Now's the time for all folk to come to the aid of their country.",
                      TextConvert.FromQuotedPrintableString("Now's the=\n time =\rfor all folk to come =\r\nto the aid=\r=\n of their country."));
    }

    [Test]
    public void TestFromBase64String()
    {
      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromBase64String("5ryi5a2XYWJj44GL44GqMTIz44Kr44OK", Encoding.UTF8),
                      "utf-8");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromBase64String("tMG7+mFiY6SrpMoxMjOlq6XK", eucjp),
                      "euc-jp");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromBase64String("GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC", jis),
                      "jis");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromBase64String("ir+OmmFiY4KpgsgxMjODSoNp", sjis),
                      "shift-jis");
    }

    [Test]
    public void TestToMimeEncodedStringNoFolding()
    {
      Assert.AreEqual("=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?=",
                      TextConvert.ToMimeEncodedString("漢字abcかな123カナ", MimeEncoding.Base64, Encoding.UTF8),
                      "base64");
      Assert.AreEqual("=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
                      TextConvert.ToMimeEncodedString("漢字abcかな123カナ", MimeEncoding.QuotedPrintable, Encoding.UTF8),
                      "quoted-printable");
    }

    [Test]
    public void TestToMimeEncodedStringWithFolding()
    {
      Assert.AreEqual("=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?=\r\n\t=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?=",
                      TextConvert.ToMimeEncodedString("漢字abcかな123カナ漢字abcかな123カナ漢字abcかな123カナ", MimeEncoding.Base64, Encoding.UTF8, 76, 0),
                      "base64");
      Assert.AreEqual("=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=\r\n\t=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB?=\r\n\t=?utf-8?q?=E3=83=8A?=",
                      TextConvert.ToMimeEncodedString("漢字abcかな123カナ漢字abcかな123カナ", MimeEncoding.QuotedPrintable, Encoding.UTF8, 76, 0),
                      "quoted-printable");
    }

    [Test]
    public void TestToMimeEncodedStringSpecifiedFormat()
    {
      Assert.AreEqual("=?utf-8?b?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK5ryi5a2X?=\n =?utf-8?b?YWJj44GL44GqMTIz44Kr44OK5ryi5a2XYWJj44GL44GqMTIz44Kr?=\n =?utf-8?b?44OK?=",
                      TextConvert.ToMimeEncodedString("漢字abcかな123カナ漢字abcかな123カナ漢字abcかな123カナ", MimeEncoding.Base64, Encoding.UTF8, 64, 9, "\n "),
                      "base64");
      Assert.AreEqual("=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123?=\n =?utf-8?q?=E3=82=AB=E3=83=8A=E6=BC=A2=E5=AD=97abc=E3=81=8B?=\n =?utf-8?q?=E3=81=AA123=E3=82=AB=E3=83=8A=E6=BC=A2=E5=AD=97abc?=\n =?utf-8?q?=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=",
                      TextConvert.ToMimeEncodedString("漢字abcかな123カナ漢字abcかな123カナ漢字abcかな123カナ", MimeEncoding.QuotedPrintable, Encoding.UTF8, 64, 9, "\n "),
                      "quoted-printable");
    }

    [Test]
    public void TestToMimeEncodedStringInvalidCharset()
    {
      try {
        TextConvert.ToMimeEncodedString("漢字abcかな123カナ", MimeEncoding.Base64, null);
        Assert.Fail("no exceptions thrown");
      }
      catch (ArgumentException) {
      }
    }

    [Test]
    public void TestToMimeEncodedStringInvalidEncoding()
    {
      try {
        TextConvert.ToMimeEncodedString("漢字abcかな123カナ", (MimeEncoding)0x7fffffff, Encoding.UTF8);
        Assert.Fail("no exceptions thrown");
      }
      catch (ArgumentException) {
      }
    }

    [Test]
    public void TestFromMimeEncodedStringQEncoded()
    {
      Encoding charset;
      MimeEncoding encoding;

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?utf-8?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?=", out encoding, out charset),
                      "utf8");
      Assert.AreEqual(MimeEncoding.QuotedPrintable, encoding, "utf8");
      Assert.AreEqual(Encoding.UTF8, charset, "utf8");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?utf-7?Q?+byJbVw-abc+MEswag-123+MKswyg-?=", out encoding, out charset),
                      "utf7");
      Assert.AreEqual(MimeEncoding.QuotedPrintable, encoding, "utf7");
      Assert.AreEqual(Encoding.UTF7, charset, "utf7");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?iso-2022-jp?q?=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B?=", out encoding, out charset),
                      "iso-2022-jp");
      Assert.AreEqual(MimeEncoding.QuotedPrintable, encoding, "iso-2022-jp");
      Assert.AreEqual(jis, charset, "iso-2022-jp");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?shift_jis?Q?=8A=BF=8E=9Aabc=82=A9=82=C8123=83J=83i?=", out encoding, out charset),
                      "shift_jis");
      Assert.AreEqual(MimeEncoding.QuotedPrintable, encoding, "shift_jis");
      Assert.AreEqual(sjis, charset, "shift_jis");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?euc-jp?q?=B4=C1=BB=FAabc=A4=AB=A4=CA123=A5=AB=A5=CA?=", out encoding, out charset),
                      "euc-jp");
      Assert.AreEqual(MimeEncoding.QuotedPrintable, encoding, "euc-jp");
      Assert.AreEqual(eucjp, charset, "euc-jp");
    }

    [Test]
    public void FromMimeEncodedStringBEncoded()
    {
      Encoding charset;
      MimeEncoding encoding;

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?utf-8?B?5ryi5a2XYWJj44GL44GqMTIz44Kr44OK?=", out encoding, out charset),
                      "utf8");
      Assert.AreEqual(MimeEncoding.Base64, encoding, "utf8");
      Assert.AreEqual(Encoding.UTF8, charset, "utf8");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?utf-7?b?K2J5SmJWdy1hYmMrTUVzd2FnLTEyMytNS3N3eWct?=", out encoding, out charset),
                      "utf7");
      Assert.AreEqual(MimeEncoding.Base64, encoding, "utf7");
      Assert.AreEqual(Encoding.UTF7, charset, "utf7");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?iso-2022-jp?B?GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC?=", out encoding, out charset),
                      "iso-2022-jp");
      Assert.AreEqual(MimeEncoding.Base64, encoding, "iso-2022-jp");
      Assert.AreEqual(jis, charset, "iso-2022-jp");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?shift_jis?b?ir+OmmFiY4KpgsgxMjODSoNp?=", out encoding, out charset),
                      "shift_jis");
      Assert.AreEqual(MimeEncoding.Base64, encoding, "shift_jis");
      Assert.AreEqual(sjis, charset, "shift_jis");

      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?euc-jp?B?tMG7+mFiY6SrpMoxMjOlq6XK?=", out encoding, out charset),
                      "euc-jp");
      Assert.AreEqual(MimeEncoding.Base64, encoding, "euc-jp");
      Assert.AreEqual(eucjp, charset, "euc-jp");
    }

    [Test]
    public void FromMimeEncodedStringBEncodedBug()
    {
      Assert.AreEqual("【Microsoft】欽ちゃん球団の片岡安祐美さんが Office にチャレンジ! ワクワクの春 開幕!",
                      TextConvert.FromMimeEncodedString(
                                                      "=?iso-2022-jp?B?GyRCIVobKEJNaWNyb3NvZnQbJEIhWzZWJEEkYyRzNWVDRBsoQg==?=" + 
                                                      " =?iso-2022-jp?B?GyRCJE5KUjIsMEJNNEh+JDUkcyQsGyhCIE9mZmljZSAbJEIkSyVBGyhC?=" + 
                                                      " =?iso-2022-jp?B?GyRCJWMlbCVzJTgbKEIhIBskQiVvJS8lbyUvJE49VRsoQiA=?=" + 
                                                      " =?iso-2022-jp?B?GyRCMytLaxsoQiE=?="));

      Assert.AreEqual("santamartaさんの日記にコメントが登録されました",
                      TextConvert.FromMimeEncodedString("=?ISO-2022-JP?B?c2FudGFtYXJ0YRskQiQ1JHMkTkZ8NS0kSyUzJWElcyVIJCxFUE8/GyhC?= =?ISO-2022-JP?B?GyRCJDUkbCReJDckPxsoQg==?="));

      Assert.AreEqual("初めまして。突然のメールに深くお",
                      TextConvert.FromMimeEncodedString("=?iso-2022-jp?B?GyRCPWkkYSReJDckRiEjRk1BMyROJWEhPCVrJEs/PCQvJCobKEI=?="));

      Assert.AreEqual("ウイルスバスタークラブニュース3月号■3年分でプラス3カ月。大好評キャンペーンが間もなく終了/新生活スタート。登録内容の変更を！",
                      TextConvert.FromMimeEncodedString("=?ISO-2022-JP?B?GyRCJSYlJCVrJTklUCU5JT8hPCUvJWklViVLJWUhPCU5GyhC?=3=?ISO-2022-JP?B?GyRCN24bKEI=?=" +
                                                      " =?ISO-2022-JP?B?GyRCOWYiIxsoQg==?=3=?ISO-2022-JP?B?GyRCRy9KLCRHGyhC?=" +
                                                      " =?ISO-2022-JP?B?GyRCJVclaSU5GyhC?=3=?ISO-2022-JP?B?GyRCJSs3biEjGyhC?=" +
                                                      " =?ISO-2022-JP?B?GyRCQmc5JUk+JS0lYyVzJVohPCVzJCw0ViRiJEokLz0qTjsbKEI=?=" +
                                                      " /=?ISO-2022-JP?B?GyRCPzdAODNoJTklPyE8JUghI0VQTz9GYk1GJE5KUTk5GyhC?=" +
                                                      " =?ISO-2022-JP?B?GyRCJHIhKhsoQg==?="));
    }

    [Test, ExpectedException(typeof(FormatException))]
    public void TestFromMimeEncodedStringInvalidEncoding()
    {
      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?utf-8?x?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?="),
                      "utf8");
    }

    [Test, ExpectedException(typeof(FormatException))]
    public void TestFromMimeEncodedStringInvalidCharset()
    {
      Assert.AreEqual("漢字abcかな123カナ",
                      TextConvert.FromMimeEncodedString("=?invalid?q?=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A?="),
                      "utf8");
    }

    private readonly Encoding jis = Encoding.GetEncoding("iso-2022-jp");
    private readonly Encoding sjis = Encoding.GetEncoding("shift_jis");
    private readonly Encoding eucjp = Encoding.GetEncoding("euc-jp");
  }
}
