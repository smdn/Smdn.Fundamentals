// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn.Formats.ModifiedBase64 {
  [TestFixture]
  public class ModifiedUTF7Tests {
    private struct TestCase {
      public string PlainText;
      public string ModifiedUTF7Text;
      public string Description;
    }

    private IEnumerable<TestCase> GenerateTestCases()
    {
      yield return new TestCase() { PlainText = string.Empty, ModifiedUTF7Text = string.Empty, Description = "(empty string)" };

      yield return new TestCase() { PlainText = "INBOX.日本語", ModifiedUTF7Text = "INBOX.&ZeVnLIqe-", Description = "日本語1" };
      yield return new TestCase() { PlainText = "INBOX.日本語.child", ModifiedUTF7Text = "INBOX.&ZeVnLIqe-.child", Description = "日本語2" };
      yield return new TestCase() { PlainText = "&日&-本-&語-", ModifiedUTF7Text = "&-&ZeU-&--&Zyw--&-&ip4--", Description = "日本語3" };
      yield return new TestCase() { PlainText = "~peter/mail/台北/日本語", ModifiedUTF7Text = "~peter/mail/&U,BTFw-/&ZeVnLIqe-", Description = "日本語4" };
      yield return new TestCase() { PlainText = "☺!", ModifiedUTF7Text = "&Jjo-!", Description = "☺" };
      yield return new TestCase() { PlainText = "📧", ModifiedUTF7Text = "&2D3c5w-", Description = "U+1F4E7 'E-MAIL SYMBOL'" };
      yield return new TestCase() { PlainText = "\U0001F4E7", ModifiedUTF7Text = "&2D3c5w-", Description = "U+1F4E7 'E-MAIL SYMBOL' (escape sequence)" };
      yield return new TestCase() { PlainText = "mail📧mail", ModifiedUTF7Text = "mail&2D3c5w-mail", Description = "mail U+1F4E7 'E-MAIL SYMBOL' mail" };

      yield return new TestCase() { PlainText = "下書き", ModifiedUTF7Text = "&Tgtm+DBN-", Description = "padding: 0" };
      yield return new TestCase() { PlainText = "サポート", ModifiedUTF7Text = "&MLUw3TD8MMg-", Description = "padding: 1" };
      yield return new TestCase() { PlainText = "迷惑メール", ModifiedUTF7Text = "&j,dg0TDhMPww6w-", Description = "padding: 2" };
    }

    [Test]
    public void TestDecode()
    {
      foreach (var testCase in GenerateTestCases()) {
        Assert.AreEqual(testCase.PlainText, ModifiedUTF7.Decode(testCase.ModifiedUTF7Text), testCase.Description);
      }
    }

    [Test]
    public void TestEncode()
    {
      foreach (var testCase in GenerateTestCases()) {
        Assert.AreEqual(testCase.ModifiedUTF7Text, ModifiedUTF7.Encode(testCase.PlainText), testCase.Description);
      }
    }

    [Test]
    public void TestDecodeArgumentNull()
    {
      Assert.Throws<ArgumentNullException>(() => ModifiedUTF7.Decode(null));
    }

    [Test]
    public void TestEncodeArgumentNull()
    {
      Assert.Throws<ArgumentNullException>(() => ModifiedUTF7.Encode(null));
    }

    [Test]
    public void TestDecodeIncorrectForm()
    {
      Assert.Throws<FormatException>(() => ModifiedUTF7.Decode("&Tgtm+DBN-&"));
    }

    [Test]
    public void TestDecodeBroken()
    {
      Assert.AreEqual("下書き", ModifiedUTF7.Decode("&Tgtm+DBN"));
      Assert.AreEqual("Tgtm+DBN-", ModifiedUTF7.Decode("Tgtm+DBN-"));
    }
  }
}
