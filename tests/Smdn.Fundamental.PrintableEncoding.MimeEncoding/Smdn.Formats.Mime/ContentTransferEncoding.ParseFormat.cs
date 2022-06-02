// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
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
  [TestCase("x-uu", ContentTransferEncodingMethod.UUEncode)]
  [TestCase("x-uue", ContentTransferEncodingMethod.UUEncode)]
  [TestCase("uuencode", ContentTransferEncodingMethod.UUEncode)]
  public void Parse(string input, ContentTransferEncodingMethod expected)
    => Assert.AreEqual(
      expected,
      ContentTransferEncoding.Parse(input),
      input
    );

  [TestCase(null, typeof(ArgumentNullException))]
  [TestCase("", typeof(ArgumentException))]
  [TestCase("base32", typeof(FormatException))]
  [TestCase("x-unknown", typeof(FormatException))]
  [TestCase("unknown", typeof(FormatException))]
  public void Parse_Invalid(string input, Type typeOfExpectedException)
    => Assert.Throws(typeOfExpectedException, () => ContentTransferEncoding.Parse(input));

  [TestCase("7bit", true, ContentTransferEncodingMethod.SevenBit)]
  [TestCase("7BIT", true, ContentTransferEncodingMethod.SevenBit)]
  [TestCase("8bit", true, ContentTransferEncodingMethod.EightBit)]
  [TestCase("binary", true, ContentTransferEncodingMethod.Binary)]
  [TestCase("base64", true, ContentTransferEncodingMethod.Base64)]
  [TestCase("Base64", true, ContentTransferEncodingMethod.Base64)]
  [TestCase("BASE64", true, ContentTransferEncodingMethod.Base64)]
  [TestCase("quoted-printable", true, ContentTransferEncodingMethod.QuotedPrintable)]
  [TestCase("x-gzip64", true, ContentTransferEncodingMethod.GZip64)]
  [TestCase("gzip64", true, ContentTransferEncodingMethod.GZip64)]
  [TestCase("x-uuencode", true, ContentTransferEncodingMethod.UUEncode)]
  [TestCase("x-uuencoded", true, ContentTransferEncodingMethod.UUEncode)]
  [TestCase("x-uu", true, ContentTransferEncodingMethod.UUEncode)]
  [TestCase("x-uue", true, ContentTransferEncodingMethod.UUEncode)]
  [TestCase("uuencode", true, ContentTransferEncodingMethod.UUEncode)]
  [TestCase(null, false, ContentTransferEncodingMethod.Unknown)]
  [TestCase("", false, ContentTransferEncodingMethod.Unknown)]
  [TestCase("base32", false, ContentTransferEncodingMethod.Unknown)]
  [TestCase("x-unknown", false, ContentTransferEncodingMethod.Unknown)]
  [TestCase("unknown", false, ContentTransferEncodingMethod.Unknown)]
  public void TryParse(string input, bool expectedResult, ContentTransferEncodingMethod expectedEncoding)
  {
    Assert.AreEqual(expectedResult, ContentTransferEncoding.TryParse(input, out var actualEncoding), nameof(expectedResult));

    if (expectedResult)
      Assert.AreEqual(expectedEncoding, actualEncoding, nameof(expectedEncoding));
  }

  [TestCase(ContentTransferEncodingMethod.SevenBit, true, "7bit")]
  [TestCase(ContentTransferEncodingMethod.EightBit, true, "8bit")]
  [TestCase(ContentTransferEncodingMethod.Binary, true, "binary")]
  [TestCase(ContentTransferEncodingMethod.Base64, true, "base64")]
  [TestCase(ContentTransferEncodingMethod.QuotedPrintable, true, "quoted-printable")]
  [TestCase(ContentTransferEncodingMethod.UUEncode, true, "x-uuencode")]
  [TestCase(ContentTransferEncodingMethod.GZip64, true, "x-gzip64")]
  [TestCase(ContentTransferEncodingMethod.Unknown, false, null)]
  [TestCase(-1, false, null)]
  public void TryFormat(ContentTransferEncodingMethod encoding, bool expectedResult, string expected)
  {
    Span<char> destination = stackalloc char[0x10];

    Assert.AreEqual(expectedResult, ContentTransferEncoding.TryFormat(encoding, destination, out var charsWritten), nameof(expectedResult));

    if (expectedResult) {
      Assert.AreEqual(expected.Length, charsWritten, nameof(charsWritten));
      Assert.IsTrue(destination.Slice(0, charsWritten).SequenceEqual(expected.AsSpan()));
    }
  }

  [TestCase(ContentTransferEncodingMethod.SevenBit, "7bit")]
  [TestCase(ContentTransferEncodingMethod.EightBit, "8bit")]
  [TestCase(ContentTransferEncodingMethod.Binary, "binary")]
  [TestCase(ContentTransferEncodingMethod.Base64, "base64")]
  [TestCase(ContentTransferEncodingMethod.QuotedPrintable, "quoted-printable")]
  [TestCase(ContentTransferEncodingMethod.UUEncode, "x-uuencode")]
  [TestCase(ContentTransferEncodingMethod.GZip64, "x-gzip64")]
  public void TryFormat_DestinationTooShort(ContentTransferEncodingMethod encoding, string expected)
  {
    Span<char> destination = stackalloc char[expected.Length - 1];

    Assert.IsFalse(
      ContentTransferEncoding.TryFormat(encoding, destination, out _)
    );
  }
}
