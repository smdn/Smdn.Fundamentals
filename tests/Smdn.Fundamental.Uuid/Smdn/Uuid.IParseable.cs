// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

partial class UuidTests {
  [Test]
  public void TestParse()
  {
    const string s = "f81d4fae-7dec-11d0-a765-00a0c91e6bf6";

    static void TestParsed(Uuid uuid, string testCaseLabel)
    {
      Assert.AreEqual(0xf81d4fae, uuid.TimeLow, $"{testCaseLabel} {nameof(Uuid.TimeLow)}");
      Assert.AreEqual(0x7dec, uuid.TimeMid, $"{testCaseLabel} {nameof(Uuid.TimeMid)}");
      Assert.AreEqual(0x11d0, uuid.TimeHighAndVersion, $"{testCaseLabel} {nameof(Uuid.TimeHighAndVersion)}");
      Assert.AreEqual(0xa7, uuid.ClockSeqHighAndReserved, $"{testCaseLabel} {nameof(Uuid.ClockSeqHighAndReserved)}");
      Assert.AreEqual(0x65, uuid.ClockSeqLow, $"{testCaseLabel} {nameof(Uuid.ClockSeqLow)}");
      Assert.AreEqual(new byte[] {0x00, 0xa0, 0xc9, 0x1e, 0x6b, 0xf6}, uuid.Node, $"{testCaseLabel} {nameof(Uuid.Node)}");
      Assert.AreEqual(UuidVersion.Version1, uuid.Version, $"{testCaseLabel} {nameof(Uuid.Version)}");
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField, $"{testCaseLabel} {nameof(Uuid.VariantField)}");
    }

    TestParsed(Uuid.Parse(s), "Parse<string>");
    TestParsed(Uuid.Parse(s.AsSpan()), "Parse<ReadOnlySpan<char>>");

#if FEATURE_GENERIC_MATH
    TestParsed(ParseString<Uuid>(s), "IParseable.Parse<string>");
    TestParsed(ParseReadOnlySpanOfChar<Uuid>(s.AsSpan()), "ISpanParseable.Parse<ReadOnlySpan<char>>");

    static T ParseString<T>(string s) where T : IParseable<T> => T.Parse(s, provider: null);
    static T ParseReadOnlySpanOfChar<T>(ReadOnlySpan<char> s) where T : ISpanParseable<T> => T.Parse(s, provider: null);
#endif
  }

  [Test]
  public void TestTryParse()
  {
    const string s = "f81d4fae-7dec-11d0-a765-00a0c91e6bf6";

    static void TestParsed(Uuid uuid, string testCaseLabel)
    {
      Assert.AreEqual(0xf81d4fae, uuid.TimeLow, $"{testCaseLabel} {nameof(Uuid.TimeLow)}");
      Assert.AreEqual(0x7dec, uuid.TimeMid, $"{testCaseLabel} {nameof(Uuid.TimeMid)}");
      Assert.AreEqual(0x11d0, uuid.TimeHighAndVersion, $"{testCaseLabel} {nameof(Uuid.TimeHighAndVersion)}");
      Assert.AreEqual(0xa7, uuid.ClockSeqHighAndReserved, $"{testCaseLabel} {nameof(Uuid.ClockSeqHighAndReserved)}");
      Assert.AreEqual(0x65, uuid.ClockSeqLow, $"{testCaseLabel} {nameof(Uuid.ClockSeqLow)}");
      Assert.AreEqual(new byte[] {0x00, 0xa0, 0xc9, 0x1e, 0x6b, 0xf6}, uuid.Node, $"{testCaseLabel} {nameof(Uuid.Node)}");
      Assert.AreEqual(UuidVersion.Version1, uuid.Version, $"{testCaseLabel} {nameof(Uuid.Version)}");
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField, $"{testCaseLabel} {nameof(Uuid.VariantField)}");
    }

    Assert.IsTrue(Uuid.TryParse(s, provider: null, out var uuid0), "TryParse<string>");
    Assert.IsTrue(Uuid.TryParse(s.AsSpan(), provider: null, out var uuid1), "TryParse<ReadOnlySpan<char>>");

    TestParsed(uuid0, "TryParse<string>");
    TestParsed(uuid1, "TryParse<ReadOnlySpan<char>>");

#if FEATURE_GENERIC_MATH
    Assert.IsTrue(TryParseString<Uuid>(s, out var uuid2), "IParseable.TryParse<string>");
    Assert.IsTrue(TryParseReadOnlySpanOfChar<Uuid>(s.AsSpan(), out var uuid3), "ISpanParseable.TryParse<ReadOnlySpan<char>>");

    TestParsed(uuid2, "IParseable.TryParse<string>");
    TestParsed(uuid3, "ISpanParseable.TryParse<ReadOnlySpan<char>>");

    static bool TryParseString<T>(string s, out T result) where T : IParseable<T> => T.TryParse(s, provider: null, out result);
    static bool TryParseReadOnlySpanOfChar<T>(ReadOnlySpan<char> s, out T result) where T : ISpanParseable<T> => T.TryParse(s, provider: null, out result);
#endif
  }

  private static System.Collections.IEnumerable YieldTestCase_Parse_FormatException()
  {
    yield return new object[] { "", "invalid UUID" };
    yield return new object[] { "6", "invalid UUID" };
    yield return new object[] { "6ba7b810", "invalid UUID" };
    yield return new object[] { "6ba7b810-", "invalid UUID" };
    yield return new object[] { "6ba7b810-9", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad-", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad-1", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad-11d1", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad-11d1-", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad-11d1-8", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c04fd430c8-", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c04fd430c8FF", "invalid UUID" };

    yield return new object[] { "6ba7b810*9dad*11d1*80b4*00c04fd430c8", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4*00c04fd430c8", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad-11d1*80b4*00c04fd430c8", "invalid UUID" };
    yield return new object[] { "6ba7b810-9dad*11d1*80b4*00c04fd430c8", "invalid UUID" };
    yield return new object[] { "6ba7b810*9dad-11d1-80b4-00c04fd430c8", "invalid time_low" };
    yield return new object[] { "6ba7b810*9dad*11d1-80b4-00c04fd430c8", "invalid time_low" };
    yield return new object[] { "6ba7b810*9dad*11d1*80b4-00c04fd430c8", "invalid time_low" };
    yield return new object[] { "6ba7b810-9dad*11d1-80b4-00c04fd430c8", "invalid time_mid" };
    yield return new object[] { "6ba7b810-9dad-11d1*80b4-00c04fd430c8", "invalid time_hi_and_version" };

    yield return new object[] { "-ba7b810-9dad-11d1-80b4-00c04fd430c8", "invalid time_low" };
    yield return new object[] { "*ba7b810-9dad-11d1-80b4-00c04fd430c8", "invalid time_low" };
    yield return new object[] { "6ba7b81--9dad-11d1-80b4-00c04fd430c8", "invalid time_low" };
    yield return new object[] { "6ba7b81*-9dad-11d1-80b4-00c04fd430c8", "invalid time_low" };
    yield return new object[] { "6ba7b810--dad-11d1-80b4-00c04fd430c8", "invalid time_mid" };
    yield return new object[] { "6ba7b810-*dad-11d1-80b4-00c04fd430c8", "invalid time_mid" };
    yield return new object[] { "6ba7b810-9da--11d1-80b4-00c04fd430c8", "invalid time_mid" };
    yield return new object[] { "6ba7b810-9da*-11d1-80b4-00c04fd430c8", "invalid time_mid" };
    yield return new object[] { "6ba7b810-9dad--1d1-80b4-00c04fd430c8", "invalid time_hi_and_version" };
    yield return new object[] { "6ba7b810-9dad-*1d1-80b4-00c04fd430c8", "invalid time_hi_and_version" };
    yield return new object[] { "6ba7b810-9dad-11d--80b4-00c04fd430c8", "invalid time_hi_and_version" };
    yield return new object[] { "6ba7b810-9dad-11d*-80b4-00c04fd430c8", "invalid time_hi_and_version" };
    yield return new object[] { "6ba7b810-9dad-11d1--0b4-00c04fd430c8", "invalid clock_seq_hi_and_reserved" };
    yield return new object[] { "6ba7b810-9dad-11d1-*0b4-00c04fd430c8", "invalid clock_seq_hi_and_reserved" };
    yield return new object[] { "6ba7b810-9dad-11d1-8-b4-00c04fd430c8", "invalid clock_seq_hi_and_reserved" };
    yield return new object[] { "6ba7b810-9dad-11d1-8*b4-00c04fd430c8", "invalid clock_seq_hi_and_reserved" };
    yield return new object[] { "6ba7b810-9dad-11d1-80-4-00c04fd430c8", "invalid clock_seq_hi_and_reserved" };
    yield return new object[] { "6ba7b810-9dad-11d1-80*4-00c04fd430c8", "invalid clock_seq_low" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b--00c04fd430c8", "invalid clock_seq_hi_and_reserved" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b*-00c04fd430c8", "invalid clock_seq_low" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-0-c04fd430c8", "invalid node" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-0*c04fd430c8", "invalid node" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c-4fd430c8", "invalid node" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c*4fd430c8", "invalid node" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c04-d430c8", "invalid node" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c04*d430c8", "invalid node" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c04fd-30c8", "invalid node" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c04fd*30c8", "invalid node" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c04fd43-c8", "invalid node" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c04fd43*c8", "invalid node" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c04fd430c-", "invalid node" };
    yield return new object[] { "6ba7b810-9dad-11d1-80b4-00c04fd430c*", "invalid node" };
  }

  [TestCaseSource(nameof(YieldTestCase_Parse_FormatException))]
  public void TestParse_FormatException(string uuid, string expectedExceptionMessage)
  {
    var exString = Assert.Throws<FormatException>(() => Uuid.Parse(uuid), "Parse<string>");
    StringAssert.Contains(expectedExceptionMessage, exString!.Message, "Parse<string> exception message");

    var exReadOnlySpanOfChar = Assert.Throws<FormatException>(() => Uuid.Parse(uuid.AsSpan()), "Parse<ReadOnlySpan<char>>");
    StringAssert.Contains(expectedExceptionMessage, exReadOnlySpanOfChar!.Message, "Parse<<ReadOnlySpan<char>> exception message");

#if FEATURE_GENERIC_MATH
    var exStringGM = Assert.Throws<FormatException>(() => ParseString<Uuid>(uuid), "IParseable.Parse<string>");
    StringAssert.Contains(expectedExceptionMessage, exStringGM.Message, "IParseable.Parse<string> exception message");

    var exReadOnlySpanOfCharGM = Assert.Throws<FormatException>(() => ParseReadOnlySpanOfChar<Uuid>(uuid.AsSpan()), "IParseable.Parse<ReadOnlySpan<char>>");
    StringAssert.Contains(expectedExceptionMessage, exReadOnlySpanOfCharGM.Message, "IParseable.Parse<<ReadOnlySpan<char>> exception message");

    static T ParseString<T>(string s) where T : IParseable<T> => T.Parse(s, provider: null);
    static T ParseReadOnlySpanOfChar<T>(ReadOnlySpan<char> s) where T : ISpanParseable<T> => T.Parse(s, provider: null);
#endif
  }

  [TestCaseSource(nameof(YieldTestCase_Parse_FormatException))]
  public void TestTryParse_FormatError(string uuid, string discard)
  {
    Assert.IsFalse(Uuid.TryParse(uuid, provider: null, out _), "TryParse<string>");
    Assert.IsFalse(Uuid.TryParse(uuid.AsSpan(), provider: null, out _), "TryParse<ReadOnlySpan<char>>");

#if FEATURE_GENERIC_MATH
    Assert.IsFalse(TryParseString<Uuid>(uuid), "IParseable.TryParse<string>");
    Assert.IsFalse(TryParseReadOnlySpanOfChar<Uuid>(uuid.AsSpan()), "IParseable.TryParse<ReadOnlySpan<char>>");

    static bool TryParseString<T>(string s) where T : IParseable<T> => T.TryParse(s, provider: null, out var _);
    static bool TryParseReadOnlySpanOfChar<T>(ReadOnlySpan<char> s) where T : ISpanParseable<T> => T.TryParse(s, provider: null, out var _);
#endif
  }
}
