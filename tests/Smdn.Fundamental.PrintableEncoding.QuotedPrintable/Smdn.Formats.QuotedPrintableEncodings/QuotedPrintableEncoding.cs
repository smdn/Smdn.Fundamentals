// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Smdn.Test.NUnit;

namespace Smdn.Formats.QuotedPrintableEncodings {
  [TestFixture]
  public class QuotedPrintableEncodingTests {
    [Test]
    public void TestConvertQuotedPrintableDecodableJapanese()
    {
      foreach (var encoding in new[] {
        Encodings.Jis,
        Encodings.ShiftJis,
        Encodings.EucJP,
        Encoding.BigEndianUnicode,
#pragma warning disable SYSLIB0001
        Encoding.UTF7,
#pragma warning restore SYSLIB0001
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
        var unquoted = QuotedPrintableEncoding.GetDecodedString((QuotedPrintableEncoding.GetEncodedString(text, encoding)), encoding);

        Assert.That(unquoted, Is.EqualTo(text), "with " + encoding.EncodingName);
      }
      catch (FormatException ex) {
        Assert.Fail($"failed encoding:{encoding.EncodingName} text:{text} exception:{ex}");
      }
    }

    [Test]
    public void TestGetDecodedString()
    {
      Assert.That(QuotedPrintableEncoding.GetDecodedString("Now's the time =\r\nfor all folk to come=\r\n to the aid of their country."), Is.EqualTo("Now's the time for all folk to come to the aid of their country."));

      Assert.That(QuotedPrintableEncoding.GetDecodedString("=E6=BC=A2=E5=AD=97abc=E3=81=8B=E3=81=AA123=E3=82=AB=E3=83=8A", Encoding.UTF8), Is.EqualTo("漢字abcかな123カナ"),
                      "utf8");

      Assert.That(QuotedPrintableEncoding.GetDecodedString("=B4=C1=BB=FAabc=A4=AB=A4=CA123=A5=AB=A5=CA", Encodings.EucJP), Is.EqualTo("漢字abcかな123カナ"),
                      "eucjp");

      Assert.That(QuotedPrintableEncoding.GetDecodedString("=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B", Encodings.Jis), Is.EqualTo("漢字abcかな123カナ"),
                      "jis");

      Assert.That(QuotedPrintableEncoding.GetDecodedString("=8A=BF=8E=9Aabc=82=A9=82=C8123=83J=83i", Encodings.ShiftJis), Is.EqualTo("漢字abcかな123カナ"),
                      "shift-jis");
    }

    [Test]
    public void TestGetDecodedStringWithSoftNewline()
    {
      Assert.That(QuotedPrintableEncoding.GetDecodedString("Now's the=\n time =\rfor all folk to come =\r\nto the aid=\r=\n of their country."), Is.EqualTo("Now's the time for all folk to come to the aid of their country."));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestCreateEncodingStream_LeaveStreamOpen(bool leaveStreamOpen)
    {
      using (var stream = new MemoryStream()) {
        Assert.Throws<NotImplementedException>(() => QuotedPrintableEncoding.CreateEncodingStream(stream, leaveStreamOpen));
#if false
        using (var s = QuotedPrintableEncoding.CreateEncodingStream(stream, leaveStreamOpen)) {
        }

        if (leaveStreamOpen)
          Assert.DoesNotThrow(() => stream.WriteByte(0));
        else
          Assert.Throws<ObjectDisposedException>(() => stream.WriteByte(0));
#endif
      }
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestCreateDecodingStream_LeaveStreamOpen(bool leaveStreamOpen)
    {
      using (var stream = new MemoryStream()) {
        using (var s = QuotedPrintableEncoding.CreateDecodingStream(stream, leaveStreamOpen)) {
        }

        if (leaveStreamOpen)
          Assert.DoesNotThrow(() => stream.ReadByte());
        else
          Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
      }
    }
  }
}
