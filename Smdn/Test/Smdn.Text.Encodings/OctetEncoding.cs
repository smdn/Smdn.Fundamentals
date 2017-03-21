using System;
using System.Text;

using NUnit.Framework;

namespace Smdn.Net {
  [TestFixture]
  public class NetworkTransferEncodingTests {
    [Test]
    public void TestEncodeTransfer7BitValidChar()
    {
      NetworkTransferEncoding.Transfer7Bit.GetBytes("0004 append \"INBOX\" (\\Seen) {33}\r\n\x00\x20\x40\x60\x7f");
    }

    [Test]
    public void TestEncodeTransfer8BitValidChar()
    {
      NetworkTransferEncoding.Transfer8Bit.GetBytes("0004 append \"INBOX\" (\\Seen) {33}\r\n\x00\x20\x40\x60\x80\xa0\xc0\xe0\xff");
    }

    [Test]
    public void TestEncodeTransfer7BitInvalidChar()
    {
      Assert.Throws<EncoderFallbackException>(() => NetworkTransferEncoding.Transfer7Bit.GetBytes("\x20\x40\x60\x80"));
    }

    [Test]
    public void TestEncodeTransfer8BitInvalidChar()
    {
      Assert.Throws<EncoderFallbackException>(() => NetworkTransferEncoding.Transfer8Bit.GetBytes("INBOX.日本語"));
    }
  }
}
