// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_TEXT_ENCODING_GETCHARCOUNT_READONLYSPAN_OF_BYTE
#endif

using System;
using System.Collections;
using System.Linq;

using NUnit.Framework;

namespace Smdn.Text.Encodings;

partial class OctetEncodingTests {
  [Test]
  public void GetCharCount_ArgumentNull()
  {
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetCharCount((byte[])null));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetCharCount((byte[])null, 0, 0));
  }

  [Test]
  public void GetChars_ArgumentNull()
  {
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetChars((byte[])null));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetChars((byte[])null, 0, 0));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetChars((byte[])null, 0, 0, new char[0], 0));
    Assert.Throws<ArgumentNullException>(() => OctetEncoding.SevenBits.GetChars(new byte[0], 0, 0, (char[])null, 0));
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
