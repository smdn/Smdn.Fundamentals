// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS0618 // [Obsolete]

using System;
using NUnit.Framework;

namespace Smdn.Text {
  [TestFixture()]
  public class AsciiHexadecimalsTests {
    [Test]
    public void TestToLowerString()
    {
      Assert.That(Ascii.Hexadecimals.ToLowerString(new byte[] {}), Is.EqualTo(""), "empty");
      Assert.That(Ascii.Hexadecimals.ToLowerString(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}), Is.EqualTo("0123456789abcdef"));
    }

    [Test]
    public void TestToUpperString()
    {
      Assert.That(Ascii.Hexadecimals.ToUpperString(new byte[] {}), Is.EqualTo(""), "empty");
      Assert.That(Ascii.Hexadecimals.ToUpperString(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}), Is.EqualTo("0123456789ABCDEF"));
    }

    [Test]
    public void TestToLowerByteArray()
    {
      Assert.That(Ascii.Hexadecimals.ToLowerByteArray(new byte[] {}), Is.EqualTo(new byte[] {}), "empty");
      Assert.That(Ascii.Hexadecimals.ToLowerByteArray(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}), Is.EqualTo(new byte[] {0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66}));
    }

    [Test]
    public void TestToUpperByteArray()
    {
      Assert.That(Ascii.Hexadecimals.ToUpperByteArray(new byte[] {}), Is.EqualTo(new byte[] {}), "empty");
      Assert.That(Ascii.Hexadecimals.ToUpperByteArray(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}), Is.EqualTo(new byte[] {0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46}));
    }

    [Test]
    public void TestToByteArrayFromLowerString()
    {
      Assert.That(Ascii.Hexadecimals.ToByteArrayFromLowerString(""), Is.EqualTo(new byte[] {}), "empty");
      Assert.That(Ascii.Hexadecimals.ToByteArrayFromLowerString("0123456789abcdef"), Is.EqualTo(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}));

      Assert.Throws<FormatException>(() => Ascii.Hexadecimals.ToByteArrayFromLowerString("0123456789abcde"), "#1");
      Assert.Throws<FormatException>(() => Ascii.Hexadecimals.ToByteArrayFromLowerString("0123456789abcdeg"), "#2");
      Assert.Throws<FormatException>(() => Ascii.Hexadecimals.ToByteArrayFromLowerString("0123456789abcdeF"), "#3");
    }

    [Test]
    public void TestToByteArrayFromUpperString()
    {
      Assert.That(Ascii.Hexadecimals.ToByteArrayFromUpperString(""), Is.EqualTo(new byte[] {}), "empty");
      Assert.That(Ascii.Hexadecimals.ToByteArrayFromUpperString("0123456789ABCDEF"), Is.EqualTo(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}));

      Assert.Throws<FormatException>(() => Ascii.Hexadecimals.ToByteArrayFromUpperString("0123456789ABCDE"), "#1");
      Assert.Throws<FormatException>(() => Ascii.Hexadecimals.ToByteArrayFromUpperString("0123456789ABCDEG"), "#2");
      Assert.Throws<FormatException>(() => Ascii.Hexadecimals.ToByteArrayFromUpperString("0123456789ABCDEf"), "#3");
    }

    [Test]
    public void TestToByteArray()
    {
      Assert.That(Ascii.Hexadecimals.ToByteArray("0123456789AbcDef"), Is.EqualTo(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}));
    }
  }
}
