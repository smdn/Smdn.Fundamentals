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

    Assert.That(OctetEncoding.SevenBits.GetCharCount(bytes), Is.EqualTo(3), "#1");
    Assert.That(OctetEncoding.SevenBits.GetCharCount(bytes, 1, 1), Is.EqualTo(1), "#2");
#if SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
    Assert.That(OctetEncoding.SevenBits.GetCharCount(bytes.AsSpan()), Is.EqualTo(3), "#3");
#endif
  }

  [Test]
  public void GetCharChars()
  {
    var bytes = new byte[] { (byte)'a', (byte)'b', (byte)'c' };

    Assert.That(OctetEncoding.SevenBits.GetChars(bytes), Is.EqualTo(new[] { 'a', 'b', 'c' }).AsCollection, "#1");
    Assert.That(OctetEncoding.SevenBits.GetChars(bytes, 1, 1), Is.EqualTo(new[] { 'b' }).AsCollection, "#2");

    var chars_3 = new char[3];
    Assert.That(OctetEncoding.SevenBits.GetChars(bytes, 1, 1, chars_3, 1), Is.EqualTo(1), "#3");
    Assert.That(chars_3, Is.EqualTo(new[] { '\0', 'b', '\0' }).AsCollection, nameof(chars_3));

#if SYSTEM_TEXT_ENCODING_GETCHARS_READONLYSPAN_OF_BYTE
    var chars_4 = new char[3];
    Assert.That(OctetEncoding.SevenBits.GetChars(bytes.AsSpan(1, 1), chars_4.AsSpan(1)), Is.EqualTo(1), "#3");
    Assert.That(chars_4, Is.EqualTo(new[] { '\0', 'b', '\0' }).AsCollection, nameof(chars_4));
#endif
  }

  [Test]
  public void GetString()
  {
    var bytes = new byte[] { (byte)'a', (byte)'b', (byte)'c' };

    Assert.That(OctetEncoding.SevenBits.GetString(bytes), Is.EqualTo("abc"), "#1");
    Assert.That(OctetEncoding.SevenBits.GetString(bytes, 1, 1), Is.EqualTo("b"), "#2");
#if SYSTEM_TEXT_ENCODING_GETSTRING_READONLYSPAN_OF_BYTE
    Assert.That(OctetEncoding.SevenBits.GetString(bytes.AsSpan(1, 1)), Is.EqualTo("b"), "#3");
#endif
  }

  [Test]
  public void GetCharCount_Empty()
  {
    Assert.That(OctetEncoding.SevenBits.GetCharCount(new byte[0]), Is.EqualTo(0), "#1");
    Assert.That(OctetEncoding.SevenBits.GetCharCount(new byte[1], 1, 0), Is.EqualTo(0), "#2");
#if SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
    Assert.That(OctetEncoding.SevenBits.GetCharCount(new byte[0].AsSpan()), Is.EqualTo(0), "#3");
#endif
  }

  [Test]
  public void GetCharChars_Empty()
  {
    Assert.That(OctetEncoding.SevenBits.GetChars(new byte[0]), Is.EqualTo(new byte[0]).AsCollection, "#1");
    Assert.That(OctetEncoding.SevenBits.GetChars(new byte[1], 1, 0), Is.EqualTo(new byte[0]).AsCollection, "#2");
    Assert.That(OctetEncoding.SevenBits.GetChars(new byte[1], 1, 0, new char[0], 0), Is.EqualTo(0), "#3");
#if SYSTEM_TEXT_ENCODING_GETCHARS_READONLYSPAN_OF_BYTE
    Assert.That(OctetEncoding.SevenBits.GetChars(new byte[0].AsSpan(), new char[0].AsSpan()), Is.EqualTo(0), "#4");
#endif
  }

  [Test]
  public void GetString_Empty()
  {
    Assert.That(OctetEncoding.SevenBits.GetString(new byte[0]), Is.EqualTo(string.Empty), "#1");
    Assert.That(OctetEncoding.SevenBits.GetString(new byte[1], 1, 0), Is.EqualTo(string.Empty), "#2");
#if SYSTEM_TEXT_ENCODING_GETSTRING_READONLYSPAN_OF_BYTE
    Assert.That(OctetEncoding.SevenBits.GetString(new byte[0].AsSpan()), Is.EqualTo(string.Empty), "#3");
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
    => Assert.That(OctetEncoding.SevenBits.GetCharCount(bytes, 0, bytes.Length), Is.EqualTo(expectedCount));

#if SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
  [TestCaseSource(nameof(YieldTestCases_GetCharCount))]
  public void GetCharCount_OfReadOnlySpan_SevenBits(byte[] bytes, int expectedCount)
    => Assert.That(OctetEncoding.SevenBits.GetCharCount(bytes.AsSpan()), Is.EqualTo(expectedCount));
#endif

  [TestCaseSource(nameof(YieldTestCases_GetCharCount))]
  public void GetCharCount_OfArray_EightBits(byte[] bytes, int expectedCount)
    => Assert.That(OctetEncoding.EightBits.GetCharCount(bytes, 0, bytes.Length), Is.EqualTo(expectedCount));

#if SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
  [TestCaseSource(nameof(YieldTestCases_GetCharCount))]
  public void GetCharCount_OfReadOnlySpan_EightBits(byte[] bytes, int expectedCount)
    => Assert.That(OctetEncoding.EightBits.GetCharCount(bytes.AsSpan()), Is.EqualTo(expectedCount));
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

    Assert.That(OctetEncoding.SevenBits.GetChars(bytes, 0, bytes.Length, actualChars, 0), Is.EqualTo(expectedChars.Length));
    Assert.That(actualChars, Is.EqualTo(expectedChars).AsCollection);
  }

#if SYSTEM_TEXT_ENCODING_GETCHARS_READONLYSPAN_OF_BYTE
  [TestCaseSource(nameof(YieldTestCases_GetChars))]
  public void GetChars_OfReadOnlySpan_SevenBits(byte[] bytes, char[] expectedChars)
  {
    var actualChars = new char[expectedChars.Length];

    Assert.That(OctetEncoding.SevenBits.GetChars(bytes.AsSpan(), actualChars.AsSpan()), Is.EqualTo(expectedChars.Length));
    Assert.That(actualChars, Is.EqualTo(expectedChars).AsCollection);
  }
#endif

  [TestCaseSource(nameof(YieldTestCases_GetChars))]
  public void GetChars_OfArray_EightBits(byte[] bytes, char[] expectedChars)
  {
    var actualChars = new char[expectedChars.Length];

    Assert.That(OctetEncoding.EightBits.GetChars(bytes, 0, bytes.Length, actualChars, 0), Is.EqualTo(expectedChars.Length));
    Assert.That(actualChars, Is.EqualTo(expectedChars).AsCollection);
  }

#if SYSTEM_TEXT_ENCODING_GETCHARS_READONLYSPAN_OF_BYTE
  [TestCaseSource(nameof(YieldTestCases_GetChars))]
  public void GetChars_OfReadOnlySpan_EightBits(byte[] bytes, char[] expectedChars)
  {
    var actualChars = new char[expectedChars.Length];

    Assert.That(OctetEncoding.EightBits.GetChars(bytes.AsSpan(), actualChars.AsSpan()), Is.EqualTo(expectedChars.Length));
    Assert.That(actualChars, Is.EqualTo(expectedChars).AsCollection);
  }
#endif
}
