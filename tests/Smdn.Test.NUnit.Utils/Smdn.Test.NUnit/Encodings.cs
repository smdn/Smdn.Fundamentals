// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using NUnit.Framework;

namespace Smdn.Test.NUnit;

[TestFixture]
public class EncodingsTests {
  [Test]
  public void Latin1()
  {
    Assert.IsNotNull(Encodings.Latin1);
    Assert.AreEqual("iso-8859-1", Encodings.Latin1.WebName);
    Assert.AreEqual("\u00A0\u00FF", Encodings.Latin1.GetString(new byte[] { 0xA0, 0xFF }));
  }

  [Test]
  public void Jis()
  {
    Assert.IsNotNull(Encodings.Jis);
    Assert.That(Encodings.Jis.WebName, Is.AnyOf("iso-2022-jp", "csISO2022JP"));
    Assert.AreEqual("日本語", Encodings.Jis.GetString(new byte[] { 0x1B, 0x24, 0x42, 0x46, 0x7C, 0x4B, 0x5C, 0x38, 0x6C, 0x1B, 0x28, 0x4A }));
  }

  [Test]
  public void ShiftJis()
  {
    Assert.IsNotNull(Encodings.ShiftJis);
    Assert.AreEqual("shift_jis", Encodings.ShiftJis.WebName);
    Assert.AreEqual("日本語", Encodings.ShiftJis.GetString(new byte[] { 0x93, 0xFA, 0x96, 0x7B, 0x8C, 0xEA }));
  }

  [Test]
  public void EucJP()
  {
    Assert.IsNotNull(Encodings.EucJP);
    Assert.AreEqual("euc-jp", Encodings.EucJP.WebName);
    Assert.AreEqual("日本語", Encodings.EucJP.GetString(new byte[] { 0xC6, 0xFC, 0xCB, 0xDC, 0xB8, 0xEC }));
  }
}
