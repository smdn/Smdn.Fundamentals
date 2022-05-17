// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_TEXT_ENCODING_GETBYTECOUNT_READONLYSPAN_OF_CHAR
#endif

using System;
using System.Collections;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace Smdn.Text.Encodings;

partial class OctetEncodingTests {
  [Test]
  public void GetBytes_OfArray_SevenBits()
  {
    var input = new string(Enumerable.Range(0x00, 0x80).Select(static i => (char)i).ToArray());
    var expected = Enumerable.Range(0x00, 0x80).Select(static i => (byte)i).ToArray();

    CollectionAssert.AreEqual(expected, OctetEncoding.SevenBits.GetBytes(input));
  }

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
  [Test]
  public void GetBytes_OfReadOnlySpan_SevenBits()
  {
    var input = new string(Enumerable.Range(0x00, 0x80).Select(static i => (char)i).ToArray());
    var expectedLength = 0x80;
    var buffer = new byte[expectedLength];
    var expected = Enumerable.Range(0x00, 0x80).Select(static i => (byte)i).ToArray();

    Assert.AreEqual(expectedLength, OctetEncoding.SevenBits.GetBytes(input.AsSpan(), buffer.AsSpan()));
    CollectionAssert.AreEqual(expected, buffer);
  }
#endif

  [Test]
  public void GetBytes_OfArray_EightBits()
  {
    var input = new string(Enumerable.Range(0x00, 0x100).Select(static i => (char)i).ToArray());
    var expected = Enumerable.Range(0x00, 0x100).Select(static i => (byte)i).ToArray();

    CollectionAssert.AreEqual(expected, OctetEncoding.EightBits.GetBytes(input));
  }

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
  [Test]
  public void GetBytes_OfReadOnlySpan_EightBits()
  {
    var input = new string(Enumerable.Range(0x00, 0x100).Select(static i => (char)i).ToArray());
    var expectedLength = 0x100;
    var buffer = new byte[expectedLength];
    var expected = Enumerable.Range(0x00, 0x100).Select(static i => (byte)i).ToArray();

    Assert.AreEqual(expectedLength, OctetEncoding.EightBits.GetBytes(input.AsSpan(), buffer.AsSpan()));
    CollectionAssert.AreEqual(expected, buffer);
  }
#endif

  private static IEnumerable YieldTestCases_GetBytes_ValidChar_SevenBits()
  {
    yield return new object[] { "\u0000", new byte[] { 0x00 } };
    yield return new object[] { "\u0020", new byte[] { 0x20 } };
    yield return new object[] { "\u007f", new byte[] { 0x7f } };
  }

  private static IEnumerable YieldTestCases_GetBytes_ValidChar_EightBits()
  {
    yield return new object[] { "\u0000", new byte[] { 0x00 } };
    yield return new object[] { "\u0020", new byte[] { 0x20 } };
    yield return new object[] { "\u007f", new byte[] { 0x7f } };
    yield return new object[] { "\u0080", new byte[] { 0x80 } };
    yield return new object[] { "\u00ff", new byte[] { 0xff } };
  }

  [TestCaseSource(nameof(YieldTestCases_GetBytes_ValidChar_SevenBits))]
  public void GetBytes_OfString_ValidChar_SevenBits(string input, byte[] expected)
    => CollectionAssert.AreEqual(expected, OctetEncoding.SevenBits.GetBytes(input));

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
  [TestCaseSource(nameof(YieldTestCases_GetBytes_ValidChar_SevenBits))]
  public void GetBytes_OfReadOnlySpan_ValidChar_SevenBits(string input, byte[] expected)
  {
    var buffer = new byte[expected.Length];

    Assert.AreEqual(expected.Length, OctetEncoding.SevenBits.GetBytes(input.AsSpan(), buffer.AsSpan()));
    CollectionAssert.AreEqual(expected, buffer);
  }
#endif

  [TestCaseSource(nameof(YieldTestCases_GetBytes_ValidChar_EightBits))]
  public void GetBytes_OfString_ValidChar_EightBits(string input, byte[] expected)
    => CollectionAssert.AreEqual(expected, OctetEncoding.EightBits.GetBytes(input));

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
  [TestCaseSource(nameof(YieldTestCases_GetBytes_ValidChar_EightBits))]
  public void GetBytes_OfReadOnlySpan_ValidChar_EightBits(string input, byte[] expected)
  {
    var buffer = new byte[expected.Length];

    Assert.AreEqual(expected.Length, OctetEncoding.EightBits.GetBytes(input.AsSpan(), buffer.AsSpan()));
    CollectionAssert.AreEqual(expected, buffer);
  }
#endif

  [Test]
  public void GetBytes_ValidChar_SevenBits()
    => OctetEncoding.SevenBits.GetBytes("0004 append \"INBOX\" (\\Seen) {33}\r\n\x00\x20\x40\x60\x7f");

  [Test]
  public void GetBytes_ValidChar_EightBits()
    => OctetEncoding.EightBits.GetBytes("0004 append \"INBOX\" (\\Seen) {33}\r\n\x00\x20\x40\x60\x80\xa0\xc0\xe0\xff");

  private static IEnumerable YieldTestCases_GetBytes_InvalidChar_SevenBits()
  {
    yield return new object[] { "\u0080" };
    yield return new object[] { "\u00ff" };
    yield return new object[] { "\uffff" };
    yield return new object[] { "日本語" };
    yield return new object[] { "😩" };
  }

  private static IEnumerable YieldTestCases_GetBytes_InvalidChar_EightBits()
  {
    yield return new object[] { "\u0100" };
    yield return new object[] { "\uffff" };
    yield return new object[] { "日本語" };
    yield return new object[] { "😩" };
  }

  [TestCaseSource(nameof(YieldTestCases_GetBytes_InvalidChar_SevenBits))]
  public void GetBytes_OfString_InvalidChar_SevenBits(string input)
    => Assert.Throws<EncoderFallbackException>(() => OctetEncoding.SevenBits.GetBytes(input));

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
  [TestCaseSource(nameof(YieldTestCases_GetBytes_InvalidChar_SevenBits))]
  public void GetBytes_OfReadOnlySpan_InvalidChar_SevenBits(string input)
    => Assert.Throws<EncoderFallbackException>(() => OctetEncoding.SevenBits.GetBytes(input.AsSpan(), new byte[0x10].AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_GetBytes_InvalidChar_EightBits))]
  public void GetBytes_OfString_InvalidChar_EightBits(string input)
    => Assert.Throws<EncoderFallbackException>(() => OctetEncoding.EightBits.GetBytes(input));

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
  [TestCaseSource(nameof(YieldTestCases_GetBytes_InvalidChar_EightBits))]
  public void GetBytes_OfReadOnlySpan_InvalidChar_EightBits(string input)
    => Assert.Throws<EncoderFallbackException>(() => OctetEncoding.EightBits.GetBytes(input.AsSpan(), new byte[0x10].AsSpan()));
#endif

  [Test]
  public void GetBytes_InvalidChar_SevenBits()
    => Assert.Throws<EncoderFallbackException>(() => OctetEncoding.SevenBits.GetBytes("\x20\x40\x60\x80"));

  [Test]
  public void GetBytes_InvalidChar_EightBits()
    => Assert.Throws<EncoderFallbackException>(() => OctetEncoding.EightBits.GetBytes("INBOX.日本語"));

#if SYSTEM_TEXT_ENCODING_CTOR_ENCODERFALLBACK_DECODERFALLBACK
  [Test]
  public void GetByteCount_OfString_EncoderFallback_Replacement()
    => Assert.AreEqual(
      10,
      CreateEncoding(bits: 7, new EncoderReplacementFallback("*")).GetByteCount(" aA\u0080あ😩💥?")
    );

#if SYSTEM_TEXT_ENCODING_GETBYTECOUNT_READONLYSPAN_OF_CHAR
  [Test]
  public void GetByteCount_OfReadOnlySpan_EncoderFallback_Replacement()
    => Assert.AreEqual(
      10,
      CreateEncoding(bits: 7, new EncoderReplacementFallback("*")).GetByteCount(" aA\u0080あ😩💥?".AsSpan())
    );
#endif

  [Test]
  public void GetByteCount_OfString_EncoderFallback_Exception()
    => Assert.Throws<EncoderFallbackException>(
      () => CreateEncoding(bits: 7, new EncoderExceptionFallback()).GetByteCount(" aA\u0080あ😩💥?")
    );

#if SYSTEM_TEXT_ENCODING_GETBYTECOUNT_READONLYSPAN_OF_CHAR
  [Test]
  public void GetByteCount_OfReadOnlySpan_EncoderFallback_Exception()
    => Assert.Throws<EncoderFallbackException>(
      () => CreateEncoding(bits: 7, new EncoderExceptionFallback()).GetByteCount(" aA\u0080あ😩💥?".AsSpan())
    );
#endif

  [Test]
  public void GetBytes_OfString_EncoderFallback_Replacement()
    => CollectionAssert.AreEqual(
      new byte[] { (byte)' ', (byte)'a', (byte)'A', (byte)'*', (byte)'*', (byte)'*', (byte)'*', (byte)'*', (byte)'*', (byte)'?' },
      CreateEncoding(bits: 7, new EncoderReplacementFallback("*")).GetBytes(" aA\u0080あ😩💥?")
    );

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
  [Test]
  public void GetBytes_OfReadOnlySpan_EncoderFallback_Replacement()
  {
    var buffer = new byte[0x10];
    var length = CreateEncoding(bits: 7, new EncoderReplacementFallback("*")).GetBytes(" aA\u0080あ😩💥?".AsSpan(), buffer.AsSpan());

    Assert.AreEqual(length, 10);
    CollectionAssert.AreEqual(
      new byte[] { (byte)' ', (byte)'a', (byte)'A', (byte)'*', (byte)'*', (byte)'*', (byte)'*', (byte)'*', (byte)'*', (byte)'?' },
      buffer.Take(length).ToArray()
    );
  }
#endif

  [Test]
  public void GetBytes_OfString_EncoderFallback_Replacement_FallbackCharGreaterThanMaxValue_7bit()
    => Assert.Throws<EncoderFallbackException>(
      () => CreateEncoding(bits: 7, new EncoderReplacementFallback("\u0080")).GetBytes(" aA\u0080あ😩💥?")
    );

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
  [Test]
  public void GetBytes_OfReadOnlySpan_EncoderFallback_Replacement_FallbackCharGreaterThanMaxValue_7bit()
    => Assert.Throws<EncoderFallbackException>(
      () => CreateEncoding(bits: 7, new EncoderReplacementFallback("\u0080")).GetBytes(" aA\u0080あ😩💥?".AsSpan(), new byte[0x10].AsSpan())
    );
#endif

  [Test]
  public void GetBytes_OfString_EncoderFallback_Replacement_FallbackCharGreaterThanMaxValue_8bit()
    => Assert.Throws<EncoderFallbackException>(
      () => CreateEncoding(bits: 8, new EncoderReplacementFallback("\u0100")).GetBytes(" aA\u0080あ😩💥?")
    );

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR

  [Test]
  public void GetBytes_OfReadOnlySpan_EncoderFallback_Replacement_FallbackCharGreaterThanMaxValue_8bit()
    => Assert.Throws<EncoderFallbackException>(
      () => CreateEncoding(bits: 8, new EncoderReplacementFallback("\u0100")).GetBytes(" aA\u0080あ😩💥?".AsSpan(), new byte[0x10].AsSpan())
    );
#endif

  [Test]
  public void GetBytes_OfString_EncoderFallback_Exception()
    => Assert.Throws<EncoderFallbackException>(
      () => CreateEncoding(bits: 7, new EncoderExceptionFallback()).GetByteCount(" aA\u0080あ😩💥?")
    );

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
  [Test]
  public void GetBytes_OfReadOnlySpan_EncoderFallback_Exception()
    => Assert.Throws<EncoderFallbackException>(
      () => CreateEncoding(bits: 7, new EncoderExceptionFallback()).GetByteCount(" aA\u0080あ😩💥?".AsSpan())
    );
#endif

#endif
}
