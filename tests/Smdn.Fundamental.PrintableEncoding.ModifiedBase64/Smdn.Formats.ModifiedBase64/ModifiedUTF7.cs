// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

using NUnit.Framework;

namespace Smdn.Formats.ModifiedBase64;

[TestFixture]
public class ModifiedUTF7Tests {
  private static IEnumerable YieldTestCases()
  {
    // plaintext, modified UTF7, description
    yield return new object[] { string.Empty, string.Empty, "(empty string)" };

    yield return new object[] { "INBOX.日本語", "INBOX.&ZeVnLIqe-", "日本語1" };
    yield return new object[] { "INBOX.日本語.child", "INBOX.&ZeVnLIqe-.child", "日本語2" };
    yield return new object[] { "&日&-本-&語-", "&-&ZeU-&--&Zyw--&-&ip4--", "日本語3" };
    yield return new object[] { "~peter/mail/台北/日本語", "~peter/mail/&U,BTFw-/&ZeVnLIqe-", "日本語4" };
    yield return new object[] { "☺!", "&Jjo-!", "☺" };
    yield return new object[] { "📧", "&2D3c5w-", "U+1F4E7 'E-MAIL SYMBOL'" };
    yield return new object[] { "\U0001F4E7", "&2D3c5w-", "U+1F4E7 'E-MAIL SYMBOL' (escape sequence)" };
    yield return new object[] { "mail📧mail", "mail&2D3c5w-mail", "mail U+1F4E7 'E-MAIL SYMBOL' mail" };

    yield return new object[] { "下書き", "&Tgtm+DBN-", "padding: 0" };
    yield return new object[] { "サポート", "&MLUw3TD8MMg-", "padding: 1" };
    yield return new object[] { "迷惑メール", "&j,dg0TDhMPww6w-", "padding: 2" };
  }

  [TestCaseSource(nameof(YieldTestCases))]
  public void TestDecode(string plainText, string modifiedUTF7Text, string description)
    => Assert.That(ModifiedUTF7.Decode(modifiedUTF7Text), Is.EqualTo(plainText), description);

  [TestCaseSource(nameof(YieldTestCases))]
  public void TestEncode(string plainText, string modifiedUTF7Text, string description)
    => Assert.That(ModifiedUTF7.Encode(plainText), Is.EqualTo(modifiedUTF7Text), description);

  [Test]
  public void TestDecodeArgumentNull()
    => Assert.That(() => ModifiedUTF7.Decode(null!), Throws.ArgumentNullException);

  [Test]
  public void TestEncodeArgumentNull()
    => Assert.That(() => ModifiedUTF7.Encode(null!), Throws.ArgumentNullException);

  [Test]
  public void TestDecodeIncorrectForm()
    => Assert.That(() => ModifiedUTF7.Decode("&Tgtm+DBN-&"), Throws.TypeOf<FormatException>());

  [Test]
  public void TestDecodeBroken()
  {
    Assert.That(ModifiedUTF7.Decode("&Tgtm+DBN"), Is.EqualTo("下書き"));
    Assert.That(ModifiedUTF7.Decode("Tgtm+DBN-"), Is.EqualTo("Tgtm+DBN-"));
  }
}
