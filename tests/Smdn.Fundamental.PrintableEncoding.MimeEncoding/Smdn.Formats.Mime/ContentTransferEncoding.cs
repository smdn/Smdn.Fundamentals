// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text;

using NUnit.Framework;

using Smdn.Test.NUnit;

namespace Smdn.Formats.Mime;

[TestFixture]
public class ContentTransferEncodingTests {
  [TestCase("7bit", ContentTransferEncodingMethod.SevenBit)]
  [TestCase("7BIT", ContentTransferEncodingMethod.SevenBit)]
  [TestCase("8bit", ContentTransferEncodingMethod.EightBit)]
  [TestCase("binary", ContentTransferEncodingMethod.Binary)]
  [TestCase("base64", ContentTransferEncodingMethod.Base64)]
  [TestCase("Base64", ContentTransferEncodingMethod.Base64)]
  [TestCase("BASE64", ContentTransferEncodingMethod.Base64)]
  [TestCase("quoted-printable", ContentTransferEncodingMethod.QuotedPrintable)]
  [TestCase("x-gzip64", ContentTransferEncodingMethod.GZip64)]
  [TestCase("gzip64", ContentTransferEncodingMethod.GZip64)]
  [TestCase("x-uuencode", ContentTransferEncodingMethod.UUEncode)]
  [TestCase("x-uuencoded", ContentTransferEncodingMethod.UUEncode)]
  [TestCase("uuencode", ContentTransferEncodingMethod.UUEncode)]
  [TestCase("x-unknown", ContentTransferEncodingMethod.Unknown)]
  [TestCase("unknown", ContentTransferEncodingMethod.Unknown)]
  public void GetEncodingMethod(string name, ContentTransferEncodingMethod expected)
    => Assert.AreEqual(
      expected,
      ContentTransferEncoding.GetEncodingMethod(name),
      name
    );

  [TestCase("x-unknown")]
  [TestCase("unknown")]
  [TestCase("base32")]
  public void GetEncodingMethod_ThrowException(string name)
    => Assert.Throws<NotSupportedException>(
      () => ContentTransferEncoding.GetEncodingMethodThrowException(name),
      name
    );

  [TestCase(ContentTransferEncodingMethod.SevenBit)]
  [TestCase(ContentTransferEncodingMethod.EightBit)]
  [TestCase(ContentTransferEncodingMethod.Binary)]
  [TestCase(ContentTransferEncodingMethod.Base64)]
  [TestCase(ContentTransferEncodingMethod.QuotedPrintable)]
  [TestCase(ContentTransferEncodingMethod.UUEncode)]
  public void CreateDecodingStream_CloseInnerStream(ContentTransferEncodingMethod method)
  {
    using var inputStream = new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
    using var decodingStream = ContentTransferEncoding.CreateDecodingStream(
      inputStream,
      method,
      false
    );

    decodingStream.Dispose();

    Assert.Throws<ObjectDisposedException>(() => inputStream.ReadByte());
  }

  [TestCase(ContentTransferEncodingMethod.SevenBit)]
  [TestCase(ContentTransferEncodingMethod.EightBit)]
  [TestCase(ContentTransferEncodingMethod.Binary)]
  [TestCase(ContentTransferEncodingMethod.Base64)]
  [TestCase(ContentTransferEncodingMethod.QuotedPrintable)]
  [TestCase(ContentTransferEncodingMethod.UUEncode)]
  public void CreateDecodingStream_LeaveInnerStreamOpen(ContentTransferEncodingMethod method)
  {
    using var inputStream = new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
    using var decodingStream = ContentTransferEncoding.CreateDecodingStream(
      inputStream,
      method,
      true
    );

    decodingStream.Dispose();

    Assert.DoesNotThrow(() => inputStream.ReadByte());
  }

  [TestCase(ContentTransferEncodingMethod.SevenBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
  [TestCase(ContentTransferEncodingMethod.EightBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
  [TestCase(ContentTransferEncodingMethod.Base64, "GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC")]
  [TestCase(ContentTransferEncodingMethod.QuotedPrintable, "=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B")]
  [TestCase(ContentTransferEncodingMethod.UUEncode, "begin 664 test.txt\nD&R1\"-$$[>ALH0F%B8QLD0B0K)$H;*$(Q,C,;)$(E*R5*&RA\"\n`\nend")]
  public void CreateTextReader(ContentTransferEncodingMethod cte, string content)
  {
    using var stream = new MemoryStream(Encoding.ASCII.GetBytes(content));
    var reader = ContentTransferEncoding.CreateTextReader(
      stream,
      cte,
      Encodings.Jis
    );

    Assert.AreEqual(Encodings.Jis, reader.CurrentEncoding);
    Assert.AreEqual("漢字abcかな123カナ", reader.ReadToEnd());

    stream.Position = 0L;

    reader.Dispose();

    Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
  }

  [TestCase(ContentTransferEncodingMethod.SevenBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
  [TestCase(ContentTransferEncodingMethod.EightBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
  [TestCase(ContentTransferEncodingMethod.Base64, "GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC")]
  [TestCase(ContentTransferEncodingMethod.QuotedPrintable, "=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B")]
  [TestCase(ContentTransferEncodingMethod.UUEncode, "begin 664 test.txt\nD&R1\"-$$[>ALH0F%B8QLD0B0K)$H;*$(Q,C,;)$(E*R5*&RA\"\n`\nend")]
  public void CreateTextReader_LeaveInnerStreamOpen(ContentTransferEncodingMethod cte, string content)
  {
    using var stream = new MemoryStream(Encoding.ASCII.GetBytes(content));
    var reader1 = ContentTransferEncoding.CreateTextReader(
      stream,
      cte,
      Encodings.Jis,
      true
    );

    Assert.AreEqual("漢字abcかな123カナ", reader1.ReadToEnd());

    stream.Position = 0L;

    reader1.Dispose();

    var reader2 = ContentTransferEncoding.CreateTextReader(
      stream,
      cte,
      Encodings.Jis
    );

    Assert.AreEqual("漢字abcかな123カナ", reader2.ReadToEnd(), "read again");
  }

  [TestCase(true)]
  [TestCase(false)]
  public void CreateTextReader_FromBinaryContentTransferEncoding(bool leaveStreamOpen)
  {
    var content = "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42";

    using var stream = new MemoryStream(Encoding.ASCII.GetBytes(content));

    Assert.Throws<InvalidOperationException>(
      () => ContentTransferEncoding.CreateTextReader(
        stream,
        ContentTransferEncodingMethod.Binary,
        Encodings.Jis,
        leaveStreamOpen
      )
    );
  }

  [TestCase(ContentTransferEncodingMethod.SevenBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
  [TestCase(ContentTransferEncodingMethod.EightBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
  [TestCase(ContentTransferEncodingMethod.Binary, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
  [TestCase(ContentTransferEncodingMethod.Base64, "GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC")]
  [TestCase(ContentTransferEncodingMethod.QuotedPrintable, "=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B")]
  [TestCase(ContentTransferEncodingMethod.UUEncode, "begin 664 test.txt\nD&R1\"-$$[>ALH0F%B8QLD0B0K)$H;*$(Q,C,;)$(E*R5*&RA\"\n`\nend")]
  public void CreateBinaryReader(ContentTransferEncodingMethod cte, string content)
  {
    using var stream = new MemoryStream(Encoding.ASCII.GetBytes(content));

    var reader = ContentTransferEncoding.CreateBinaryReader(
      stream,
      cte,
      Encodings.Jis
    );

    Assert.AreEqual(
      Encodings.Jis.GetBytes("漢字abcかな123カナ"),
      reader.ReadBytes((int)stream.Length)
    );

    stream.Position = 0L;

    reader.Dispose();

    Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
  }

  [TestCase(ContentTransferEncodingMethod.SevenBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
  [TestCase(ContentTransferEncodingMethod.EightBit, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
  [TestCase(ContentTransferEncodingMethod.Binary, "\x1b\x24\x42\x34\x41\x3b\x7a\x1b\x28\x42\x61\x62\x63\x1b\x24\x42\x24\x2b\x24\x4a\x1b\x28\x42\x31\x32\x33\x1b\x24\x42\x25\x2b\x25\x4a\x1b\x28\x42")]
  [TestCase(ContentTransferEncodingMethod.Base64, "GyRCNEE7ehsoQmFiYxskQiQrJEobKEIxMjMbJEIlKyVKGyhC")]
  [TestCase(ContentTransferEncodingMethod.QuotedPrintable, "=1B$B4A;z=1B(Babc=1B$B$+$J=1B(B123=1B$B%+%J=1B(B")]
  [TestCase(ContentTransferEncodingMethod.UUEncode, "begin 664 test.txt\nD&R1\"-$$[>ALH0F%B8QLD0B0K)$H;*$(Q,C,;)$(E*R5*&RA\"\n`\nend")]
  public void CreateBinaryReader_LeaveInnerStreamOpen(ContentTransferEncodingMethod cte, string content)
  {
    using var stream = new MemoryStream(Encoding.ASCII.GetBytes(content));

    var reader1 = ContentTransferEncoding.CreateBinaryReader(
      stream,
      cte,
      Encodings.Jis,
      true
    );

    Assert.AreEqual(
      Encodings.Jis.GetBytes("漢字abcかな123カナ"),
      reader1.ReadBytes((int)stream.Length)
    );

    stream.Position = 0L;

    reader1.Dispose();

    var reader2 = ContentTransferEncoding.CreateBinaryReader(
      stream,
      cte,
      Encodings.Jis
    );

    Assert.AreEqual(
      Encodings.Jis.GetBytes("漢字abcかな123カナ"),
      reader2.ReadBytes((int)stream.Length),
      "read again"
    );
  }
}
