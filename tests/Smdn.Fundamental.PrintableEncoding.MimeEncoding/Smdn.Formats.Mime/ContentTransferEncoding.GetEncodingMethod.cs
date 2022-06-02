// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn.Formats.Mime;

public partial class ContentTransferEncodingTests {
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
}
