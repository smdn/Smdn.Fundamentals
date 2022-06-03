// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace Smdn.Text.Encodings;

partial class OctetEncodingTests {
  [Test]
  public void GetByteCount()
  {
    Assert.AreEqual(3, OctetEncoding.SevenBits.GetByteCount("abc"), "#1");
#if SYSTEM_TEXT_ENCODING_GETBYTECOUNT_STRING_INT_INT
    Assert.AreEqual(1, OctetEncoding.SevenBits.GetByteCount("abc", 1, 1), "#2");
#endif
    Assert.AreEqual(3, OctetEncoding.SevenBits.GetByteCount("abc".ToCharArray()), "#3");
    Assert.AreEqual(1, OctetEncoding.SevenBits.GetByteCount("abc".ToCharArray(), 1, 1), "#4");
#if SYSTEM_TEXT_ENCODING_GETBYTECOUNT_READONLYSPAN_OF_CHAR
    Assert.AreEqual(3, OctetEncoding.SevenBits.GetByteCount("abc".AsSpan()), "#5");
#endif
  }

  [Test]
  public void GetByteCount_Empty()
  {
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetByteCount(string.Empty), "#1");
#if SYSTEM_TEXT_ENCODING_GETBYTECOUNT_STRING_INT_INT
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetByteCount("abc", 1, 0), "#2");
#endif
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetByteCount(string.Empty.ToCharArray()), "#3");
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetByteCount("abc".ToCharArray(), 1, 0), "#4");
#if SYSTEM_TEXT_ENCODING_GETBYTECOUNT_READONLYSPAN_OF_CHAR
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetByteCount(string.Empty.AsSpan()), "#5");
#endif
  }

  [Test]
  public void GetBytes()
  {
    CollectionAssert.AreEqual(new[] { (byte)'a', (byte)'b', (byte)'c' }, OctetEncoding.SevenBits.GetBytes("abc"), "#1");
#if SYSTEM_TEXT_ENCODING_GETBYTES_STRING_INT_INT
    CollectionAssert.AreEqual(new[] { (byte)'b' }, OctetEncoding.SevenBits.GetBytes("abc", 1, 1), "#2");
#endif
    var bytes_3 = new byte[3];
    Assert.AreEqual(1, OctetEncoding.SevenBits.GetBytes("abc", 1, 1, bytes_3, 1), "#3");
    CollectionAssert.AreEqual(new[] { (byte)'\0', (byte)'b', (byte)'\0' }, bytes_3, nameof(bytes_3));

    CollectionAssert.AreEqual(new[] { (byte)'a', (byte)'b', (byte)'c' }, OctetEncoding.SevenBits.GetBytes("abc".ToCharArray()), "#4");
    CollectionAssert.AreEqual(new[] { (byte)'b' }, OctetEncoding.SevenBits.GetBytes("abc".ToCharArray(), 1, 1), "#5");

    var bytes_6 = new byte[3];
    Assert.AreEqual(1, OctetEncoding.SevenBits.GetBytes("abc".ToCharArray(), 1, 1, bytes_6, 1), "#6");
    CollectionAssert.AreEqual(new[] { (byte)'\0', (byte)'b', (byte)'\0' }, bytes_6, nameof(bytes_6));

#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
    var bytes_7 = new byte[3];
    Assert.AreEqual(3, OctetEncoding.SevenBits.GetBytes("abc".AsSpan(), bytes_7.AsSpan()), "#7");
    CollectionAssert.AreEqual(new[] { (byte)'a', (byte)'b', (byte)'c' }, bytes_7, nameof(bytes_7));
#endif
  }

  [Test]
  public void GetBytes_Empty()
  {
    CollectionAssert.AreEqual(new byte[0], OctetEncoding.SevenBits.GetBytes(string.Empty), "#1");
#if SYSTEM_TEXT_ENCODING_GETBYTES_STRING_INT_INT
    CollectionAssert.AreEqual(new byte[0], OctetEncoding.SevenBits.GetBytes("abc", 1, 0), "#2");
#endif
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetBytes("abc", 1, 0, new byte[0], 0), "#3");
    CollectionAssert.AreEqual(new byte[0], OctetEncoding.SevenBits.GetBytes("abc".ToCharArray(1, 0)), "#4");
    CollectionAssert.AreEqual(new byte[0], OctetEncoding.SevenBits.GetBytes("abc".ToCharArray(), 1, 0), "#5");
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetBytes("abc".ToCharArray(), 1, 0, new byte[0], 0), "#6");
#if SYSTEM_TEXT_ENCODING_GETBYTES_READONLYSPAN_OF_CHAR
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetBytes(string.Empty.AsSpan(), new byte[0].AsSpan()), "#7");
#endif
  }

  [Test]
  public void GetByteCount_ArgumentNull()
  {
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetByteCount((string)null));
#if SYSTEM_TEXT_ENCODING_GETBYTECOUNT_STRING_INT_INT
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetByteCount((string)null, 0, 0));
#endif
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetByteCount((char[])null));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetByteCount((char[])null, 0, 0));
  }

  [Test]
  public void GetBytes_ArgumentNull()
  {
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetBytes((string)null));
#if SYSTEM_TEXT_ENCODING_GETBYTES_STRING_INT_INT
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetBytes((string)null, 0, 0));
#endif
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetBytes((string)null, 0, 0, new byte[0], 0));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetBytes(string.Empty, 0, 0, (byte[])null, 0));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetBytes((char[])null));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetBytes((char[])null, 0, 0));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetBytes((char[])null, 0, 0, new byte[0], 0));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetBytes(new char[0], 0, 0, (byte[])null, 0));
  }

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
