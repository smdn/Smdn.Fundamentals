// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using NUnit.Framework;

namespace Smdn.Test.NUnit;

[TestFixture]
public class EncodingsTests {
  [Test]
  public void Latin1()
  {
    Assert.That(Encodings.Latin1, Is.Not.Null);
    Assert.That(Encodings.Latin1.WebName, Is.EqualTo("iso-8859-1"));
    Assert.That(Encodings.Latin1.GetString(new byte[] { 0xA0, 0xFF }), Is.EqualTo("\u00A0\u00FF"));
  }

  [Test]
  public void Jis()
  {
    Assert.That(Encodings.Jis, Is.Not.Null);
    Assert.That(Encodings.Jis.WebName, Is.AnyOf("iso-2022-jp", "csISO2022JP"));
    Assert.That(Encodings.Jis.GetString(new byte[] { 0x1B, 0x24, 0x42, 0x46, 0x7C, 0x4B, 0x5C, 0x38, 0x6C, 0x1B, 0x28, 0x4A }), Is.EqualTo("日本語"));
  }

  [Test]
  public void ShiftJis()
  {
    Assert.That(Encodings.ShiftJis, Is.Not.Null);
    Assert.That(Encodings.ShiftJis.WebName, Is.EqualTo("shift_jis"));
    Assert.That(Encodings.ShiftJis.GetString(new byte[] { 0x93, 0xFA, 0x96, 0x7B, 0x8C, 0xEA }), Is.EqualTo("日本語"));
  }

  [Test]
  public void EucJP()
  {
    Assert.That(Encodings.EucJP, Is.Not.Null);
    Assert.That(Encodings.EucJP.WebName, Is.EqualTo("euc-jp"));
    Assert.That(Encodings.EucJP.GetString(new byte[] { 0xC6, 0xFC, 0xCB, 0xDC, 0xB8, 0xEC }), Is.EqualTo("日本語"));
  }
}
