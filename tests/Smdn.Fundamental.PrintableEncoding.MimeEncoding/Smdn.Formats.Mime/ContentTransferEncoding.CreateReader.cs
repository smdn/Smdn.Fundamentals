// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text;

using NUnit.Framework;

using Smdn.Test.NUnit;

namespace Smdn.Formats.Mime;

partial class ContentTransferEncodingTests {
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

    Assert.That(reader.CurrentEncoding, Is.EqualTo(Encodings.Jis));
    Assert.That(reader.ReadToEnd(), Is.EqualTo("漢字abcかな123カナ"));

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

    Assert.That(reader1.ReadToEnd(), Is.EqualTo("漢字abcかな123カナ"));

    stream.Position = 0L;

    reader1.Dispose();

    var reader2 = ContentTransferEncoding.CreateTextReader(
      stream,
      cte,
      Encodings.Jis
    );

    Assert.That(reader2.ReadToEnd(), Is.EqualTo("漢字abcかな123カナ"), "read again");
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

    Assert.That(
      reader.ReadBytes((int)stream.Length),
      Is.EqualTo(Encodings.Jis.GetBytes("漢字abcかな123カナ"))
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

    Assert.That(
      reader1.ReadBytes((int)stream.Length),
      Is.EqualTo(Encodings.Jis.GetBytes("漢字abcかな123カナ"))
    );

    stream.Position = 0L;

    reader1.Dispose();

    var reader2 = ContentTransferEncoding.CreateBinaryReader(
      stream,
      cte,
      Encodings.Jis
    );

    Assert.That(
      reader2.ReadBytes((int)stream.Length), Is.EqualTo(Encodings.Jis.GetBytes("漢字abcかな123カナ")),
      "read again"
    );
  }
}
