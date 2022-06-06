// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.Linq;

using NUnit.Framework;

namespace Smdn.Text.Encodings;

partial class OctetEncodingTests {
  [Test]
  public void GetCharCount()
  {
    var bytes = new byte[] { (byte)'a', (byte)'b', (byte)'c' };

    Assert.AreEqual(3, OctetEncoding.SevenBits.GetCharCount(bytes), "#1");
    Assert.AreEqual(1, OctetEncoding.SevenBits.GetCharCount(bytes, 1, 1), "#2");
#if SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
    Assert.AreEqual(3, OctetEncoding.SevenBits.GetCharCount(bytes.AsSpan()), "#3");
#endif
  }

  [Test]
  public void GetCharChars()
  {
    var bytes = new byte[] { (byte)'a', (byte)'b', (byte)'c' };

    CollectionAssert.AreEqual(new[] { 'a', 'b', 'c' }, OctetEncoding.SevenBits.GetChars(bytes), "#1");
    CollectionAssert.AreEqual(new[] { 'b' }, OctetEncoding.SevenBits.GetChars(bytes, 1, 1), "#2");

    var chars_3 = new char[3];
    Assert.AreEqual(1, OctetEncoding.SevenBits.GetChars(bytes, 1, 1, chars_3, 1), "#3");
    CollectionAssert.AreEqual(new[] { '\0', 'b', '\0' }, chars_3, nameof(chars_3));

#if SYSTEM_TEXT_ENCODING_GETCHARS_READONLYSPAN_OF_BYTE
    var chars_4 = new char[3];
    Assert.AreEqual(1, OctetEncoding.SevenBits.GetChars(bytes.AsSpan(1, 1), chars_4.AsSpan(1)), "#3");
    CollectionAssert.AreEqual(new[] { '\0', 'b', '\0' }, chars_4, nameof(chars_4));
#endif
  }

  [Test]
  public void GetString()
  {
    var bytes = new byte[] { (byte)'a', (byte)'b', (byte)'c' };

    Assert.AreEqual("abc", OctetEncoding.SevenBits.GetString(bytes), "#1");
    Assert.AreEqual("b", OctetEncoding.SevenBits.GetString(bytes, 1, 1), "#2");
#if SYSTEM_TEXT_ENCODING_GETSTRING_READONLYSPAN_OF_BYTE
    Assert.AreEqual("b", OctetEncoding.SevenBits.GetString(bytes.AsSpan(1, 1)), "#3");
#endif
  }

  [Test]
  public void GetCharCount_Empty()
  {
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetCharCount(new byte[0]), "#1");
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetCharCount(new byte[1], 1, 0), "#2");
#if SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetCharCount(new byte[0].AsSpan()), "#3");
#endif
  }

  [Test]
  public void GetCharChars_Empty()
  {
    CollectionAssert.AreEqual(new byte[0], OctetEncoding.SevenBits.GetChars(new byte[0]), "#1");
    CollectionAssert.AreEqual(new byte[0], OctetEncoding.SevenBits.GetChars(new byte[1], 1, 0), "#2");
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetChars(new byte[1], 1, 0, new char[0], 0), "#3");
#if SYSTEM_TEXT_ENCODING_GETCHARS_READONLYSPAN_OF_BYTE
    Assert.AreEqual(0, OctetEncoding.SevenBits.GetChars(new byte[0].AsSpan(), new char[0].AsSpan()), "#4");
#endif
  }

  [Test]
  public void GetString_Empty()
  {
    Assert.AreEqual(string.Empty, OctetEncoding.SevenBits.GetString(new byte[0]), "#1");
    Assert.AreEqual(string.Empty, OctetEncoding.SevenBits.GetString(new byte[1], 1, 0), "#2");
#if SYSTEM_TEXT_ENCODING_GETSTRING_READONLYSPAN_OF_BYTE
    Assert.AreEqual(string.Empty, OctetEncoding.SevenBits.GetString(new byte[0].AsSpan()), "#3");
#endif
  }

  [Test]
  public void GetCharCount_ArgumentNull()
  {
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetCharCount((byte[])null!));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetCharCount((byte[])null!, 0, 0));
  }

  [Test]
  public void GetChars_ArgumentNull()
  {
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetChars((byte[])null!));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetChars((byte[])null!, 0, 0));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetChars((byte[])null!, 0, 0, new char[0], 0));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetChars(new byte[0], 0, 0, (char[])null!, 0));
  }

  private static IEnumerable YieldTestCases_GetCharCount()
  {
    yield return new object[] { new byte[0], 0 };
    yield return new object[] { new byte[] { 0x00 }, 1 };
    yield return new object[] { Enumerable.Range(0x00, 0x80).Select(static i => (byte)i).ToArray(), 0x80 };
    yield return new object[] { Enumerable.Range(0x00, 0x100).Select(static i => (byte)i).ToArray(), 0x100 };
  }

  [TestCaseSource(nameof(YieldTestCases_GetCharCount))]
  public void GetCharCount_OfArray_SevenBits(byte[] bytes, int expectedCount)
    => Assert.AreEqual(expectedCount, OctetEncoding.SevenBits.GetCharCount(bytes, 0, bytes.Length));

#if SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
  [TestCaseSource(nameof(YieldTestCases_GetCharCount))]
  public void GetCharCount_OfReadOnlySpan_SevenBits(byte[] bytes, int expectedCount)
    => Assert.AreEqual(expectedCount, OctetEncoding.SevenBits.GetCharCount(bytes.AsSpan()));
#endif

  [TestCaseSource(nameof(YieldTestCases_GetCharCount))]
  public void GetCharCount_OfArray_EightBits(byte[] bytes, int expectedCount)
    => Assert.AreEqual(expectedCount, OctetEncoding.EightBits.GetCharCount(bytes, 0, bytes.Length));

#if SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
  [TestCaseSource(nameof(YieldTestCases_GetCharCount))]
  public void GetCharCount_OfReadOnlySpan_EightBits(byte[] bytes, int expectedCount)
    => Assert.AreEqual(expectedCount, OctetEncoding.EightBits.GetCharCount(bytes.AsSpan()));
#endif

  private static IEnumerable YieldTestCases_GetChars()
  {
    yield return new object[] { new byte[0], new char[0] };
    yield return new object[] { new byte[] { 0x00 }, new char[] { '\u0000' } };
    yield return new object[] {
      Enumerable.Range(0x00, 0x80).Select(static i => (byte)i).ToArray(),
      Enumerable.Range(0x00, 0x80).Select(static i => (char)i).ToArray()
    };
    yield return new object[] {
      Enumerable.Range(0x00, 0x100).Select(static i => (byte)i).ToArray(),
      Enumerable.Range(0x00, 0x100).Select(static i => (char)i).ToArray()
    };
  }

  [TestCaseSource(nameof(YieldTestCases_GetChars))]
  public void GetChars_OfArray_SevenBits(byte[] bytes, char[] expectedChars)
  {
    var actualChars = new char[expectedChars.Length];

    Assert.AreEqual(expectedChars.Length, OctetEncoding.SevenBits.GetChars(bytes, 0, bytes.Length, actualChars, 0));
    CollectionAssert.AreEqual(expectedChars, actualChars);
  }

#if SYSTEM_TEXT_ENCODING_GETCHARS_READONLYSPAN_OF_BYTE
  [TestCaseSource(nameof(YieldTestCases_GetChars))]
  public void GetChars_OfReadOnlySpan_SevenBits(byte[] bytes, char[] expectedChars)
  {
    var actualChars = new char[expectedChars.Length];

    Assert.AreEqual(expectedChars.Length, OctetEncoding.SevenBits.GetChars(bytes.AsSpan(), actualChars.AsSpan()));
    CollectionAssert.AreEqual(expectedChars, actualChars);
  }
#endif

  [TestCaseSource(nameof(YieldTestCases_GetChars))]
  public void GetChars_OfArray_EightBits(byte[] bytes, char[] expectedChars)
  {
    var actualChars = new char[expectedChars.Length];

    Assert.AreEqual(expectedChars.Length, OctetEncoding.EightBits.GetChars(bytes, 0, bytes.Length, actualChars, 0));
    CollectionAssert.AreEqual(expectedChars, actualChars);
  }

#if SYSTEM_TEXT_ENCODING_GETCHARS_READONLYSPAN_OF_BYTE
  [TestCaseSource(nameof(YieldTestCases_GetChars))]
  public void GetChars_OfReadOnlySpan_EightBits(byte[] bytes, char[] expectedChars)
  {
    var actualChars = new char[expectedChars.Length];

    Assert.AreEqual(expectedChars.Length, OctetEncoding.EightBits.GetChars(bytes.AsSpan(), actualChars.AsSpan()));
    CollectionAssert.AreEqual(expectedChars, actualChars);
  }
#endif
}
