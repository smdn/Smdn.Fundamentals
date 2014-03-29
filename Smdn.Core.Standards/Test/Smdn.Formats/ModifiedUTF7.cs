using System;
using NUnit.Framework;

namespace Smdn.Formats {
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

      Assert.AreEqual(string.Empty, ModifiedUTF7.Decode(string.Empty), "(empty string)");

      // padding: 0
      Assert.AreEqual("下書き", ModifiedUTF7.Decode("&Tgtm+DBN-"));
      // padding: 1
      Assert.AreEqual("サポート", ModifiedUTF7.Decode("&MLUw3TD8MMg-"));
      // padding: 2
      Assert.AreEqual("迷惑メール", ModifiedUTF7.Decode("&j,dg0TDhMPww6w-"));
    }

    [Test, ExpectedException(typeof(FormatException))]
    public void TestDecodeIncorrectForm()
    {
      ModifiedUTF7.Decode("&Tgtm+DBN-&");
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
