// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;
using NUnit.Framework;
using Smdn.Test.NUnit;

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

      Assert.That(actual, Is.EqualTo("%20!%22#$%25&'()*+,-./0123456789:;%3C=%3E?@ABCDEFGHIJKLMNOPQRSTUVWXYZ%5B%5C%5D%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~"));
    }

    [Test]
    public void TestGetEncodedStringRfc3986Uri()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.Rfc3986Uri,
                                                    Encoding.ASCII);

      Assert.That(actual, Is.EqualTo("%20!%22#$%25&'()*+,-./0123456789:;%3C=%3E?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[%5C]%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~"));
    }

    [Test]
    public void TestGetEncodedStringRfc2396Data()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.Rfc2396Data,
                                                    Encoding.ASCII);

      Assert.That(actual, Is.EqualTo("%20!%22%23%24%25%26'()*%2B%2C-.%2F0123456789%3A%3B%3C%3D%3E%3F%40ABCDEFGHIJKLMNOPQRSTUVWXYZ%5B%5C%5D%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~"));
    }

    [Test]
    public void TestGetEncodedStringRfc3986Data()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.Rfc3986Data,
                                                    Encoding.ASCII);

      Assert.That(actual, Is.EqualTo("%20%21%22%23%24%25%26%27%28%29%2A%2B%2C-.%2F0123456789%3A%3B%3C%3D%3E%3F%40ABCDEFGHIJKLMNOPQRSTUVWXYZ%5B%5C%5D%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~"));
    }

    [Test]
    public void TestGetEncodedStringUriEscapeUriString()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.UriEscapeUriString,
                                                    Encoding.ASCII);

      Assert.That(ToPercentEncodedTransformMode.UriEscapeUriString, Is.EqualTo(ToPercentEncodedTransformMode.Rfc3986Uri));

      Assert.That(
#pragma warning restore SYSLIB0013
        actual, Is.EqualTo(
#pragma warning disable SYSLIB0013
        Uri.EscapeUriString(text)),
        "same as Uri.EscapeDataString"
      );
    }

    [Test]
    public void TestGetEncodedStringUriEscapeDataString()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.UriEscapeDataString,
                                                    Encoding.ASCII);

      Assert.That(ToPercentEncodedTransformMode.UriEscapeDataString, Is.EqualTo(ToPercentEncodedTransformMode.Rfc3986Data));

      Assert.That(actual, Is.EqualTo(Uri.EscapeDataString(text)),
                      "same as Uri.EscapeDataString");
    }

    [Test]
    public void TestGetEncodedStringRfc5092Uri()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.Rfc5092Uri,
                                                    Encoding.ASCII);

      Assert.That(actual, Is.EqualTo("%20!%22%23$%25&'()*+,-.%2F0123456789%3A%3B%3C=%3E%3F%40ABCDEFGHIJKLMNOPQRSTUVWXYZ%5B%5C%5D%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~"));
    }

    [Test]
    public void TestGetEncodedStringRfc5092Path()
    {
      var text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
      var actual = PercentEncoding.GetEncodedString(text,
                                                    ToPercentEncodedTransformMode.Rfc5092Path,
                                                    Encoding.ASCII);

      Assert.That(actual, Is.EqualTo("%20!%22%23$%25&'()*+,-./0123456789:%3B%3C=%3E%3F@ABCDEFGHIJKLMNOPQRSTUVWXYZ%5B%5C%5D%5E_%60abcdefghijklmnopqrstuvwxyz%7B%7C%7D~"));
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
        Assert.That(PercentEncoding.GetEncodedString("日本語", mode, Encodings.ShiftJis), Is.EqualTo("%93%FA%96%7B%8C%EA"), $"mode: {mode}");
        Assert.That(PercentEncoding.GetEncodedString("日本語", mode, Encodings.EucJP), Is.EqualTo("%C6%FC%CB%DC%B8%EC"), $"mode: {mode}");
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
        Assert.That(PercentEncoding.GetEncodedString("abc def", mode, Encoding.ASCII), Is.EqualTo("abc%20def"), $"mode: {mode}");

        var mode2 = mode | ToPercentEncodedTransformMode.EscapeSpaceToPlus;

        Assert.That(PercentEncoding.GetEncodedString("abc def", mode2, Encoding.ASCII), Is.EqualTo("abc+def"), $"mode: {mode2}");
      }
    }

    [Test]
    public void TestGetDecodedString()
    {
      Assert.That(PercentEncoding.GetDecodedString("012abcABC-._~%21%22%23%24%e6%97%a5%e6%9c%ac%e8%aa%9e", Encoding.UTF8), Is.EqualTo("012abcABC-._~!\"#$日本語"));
      Assert.That(PercentEncoding.GetDecodedString("%93%fa%96%7B%8C%EA", Encodings.ShiftJis), Is.EqualTo("日本語"));
      Assert.That(PercentEncoding.GetDecodedString("%c6%Fc%cb%Dc%b8%eC", Encodings.EucJP), Is.EqualTo("日本語"));
    }

    [Test]
    public void TestGetDecodedStringDecodePlusToSpace()
    {
      Assert.That(PercentEncoding.GetDecodedString("ABC+DEF", false), Is.EqualTo("ABC+DEF"));
      Assert.That(PercentEncoding.GetDecodedString("ABC+DEF", true), Is.EqualTo("ABC DEF"));
    }
  }
}
