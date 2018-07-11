using System;
using NUnit.Framework;

namespace Smdn.Formats.ModifiedBase64 {
  [TestFixture]
  public class ModifiedUTF7Tests {
    [Test]
    public void TestDecode()
    {
      Assert.AreEqual("INBOX.日本語", ModifiedUTF7.Decode("INBOX.&ZeVnLIqe-"));

      Assert.AreEqual("INBOX.日本語.child", ModifiedUTF7.Decode("INBOX.&ZeVnLIqe-.child"));

      Assert.AreEqual("&日&-本-&語-", ModifiedUTF7.Decode("&-&ZeU-&--&Zyw--&-&ip4--"));

      Assert.AreEqual("~peter/mail/台北/日本語", ModifiedUTF7.Decode("~peter/mail/&U,BTFw-/&ZeVnLIqe-"));

      Assert.AreEqual("☺!", ModifiedUTF7.Decode("&Jjo-!"), "☺");

      Assert.AreEqual("📧", ModifiedUTF7.Decode("&2D3c5w-"), "U+1F4E7 'E-MAIL SYMBOL'");　
      Assert.AreEqual("\U0001F4E7", ModifiedUTF7.Decode("&2D3c5w-"), "U+1F4E7 'E-MAIL SYMBOL' (escape sequence)");　
      Assert.AreEqual("mail📧mail", ModifiedUTF7.Decode("mail&2D3c5w-mail"), "mail U+1F4E7 'E-MAIL SYMBOL' mail");　

      Assert.AreEqual(string.Empty, ModifiedUTF7.Decode(string.Empty), "(empty string)");

      // padding: 0
      Assert.AreEqual("下書き", ModifiedUTF7.Decode("&Tgtm+DBN-"));
      // padding: 1
      Assert.AreEqual("サポート", ModifiedUTF7.Decode("&MLUw3TD8MMg-"));
      // padding: 2
      Assert.AreEqual("迷惑メール", ModifiedUTF7.Decode("&j,dg0TDhMPww6w-"));
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

    [Test]
    public void TestEncode()
    {
      Assert.AreEqual("INBOX.&ZeVnLIqe-", ModifiedUTF7.Encode("INBOX.日本語"));

      Assert.AreEqual("INBOX.&ZeVnLIqe-.child", ModifiedUTF7.Encode("INBOX.日本語.child"));

      Assert.AreEqual("&-&ZeU-&--&Zyw--&-&ip4--", ModifiedUTF7.Encode("&日&-本-&語-"));

      Assert.AreEqual("~peter/mail/&U,BTFw-/&ZeVnLIqe-", ModifiedUTF7.Encode("~peter/mail/台北/日本語"));

      Assert.AreEqual("&Jjo-!", ModifiedUTF7.Encode("☺!"), "☺");

      Assert.AreEqual("&2D3c5w-", ModifiedUTF7.Encode("📧"), "U+1F4E7 'E-MAIL SYMBOL'");　
      Assert.AreEqual("&2D3c5w-", ModifiedUTF7.Encode("\U0001F4E7"), "U+1F4E7 'E-MAIL SYMBOL' (escape sequence)");　
      Assert.AreEqual("mail&2D3c5w-mail", ModifiedUTF7.Encode("mail📧mail"), "mail U+1F4E7 'E-MAIL SYMBOL' mail");　

      Assert.AreEqual(string.Empty, ModifiedUTF7.Encode(string.Empty), "(empty string)");

      // padding: 0
      Assert.AreEqual("&Tgtm+DBN-", ModifiedUTF7.Encode("下書き"));
      // padding: 1
      Assert.AreEqual("&MLUw3TD8MMg-", ModifiedUTF7.Encode("サポート"));
      // padding: 2
      Assert.AreEqual("&j,dg0TDhMPww6w-", ModifiedUTF7.Encode("迷惑メール"));
    }
  }
}
