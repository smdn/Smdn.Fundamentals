// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class UuidTests {
#pragma warning restore IDE0040
  [Test]
  public void TestParse()
  {
#pragma warning disable CA1305
    const string s = "f81d4fae-7dec-11d0-a765-00a0c91e6bf6";

    static void TestParsed(Uuid uuid, string testCaseLabel)
    {
      Assert.That(uuid.TimeLow, Is.EqualTo(0xf81d4fae), $"{testCaseLabel} {nameof(Uuid.TimeLow)}");
      Assert.That(uuid.TimeMid, Is.EqualTo(0x7dec), $"{testCaseLabel} {nameof(Uuid.TimeMid)}");
      Assert.That(uuid.TimeHighAndVersion, Is.EqualTo(0x11d0), $"{testCaseLabel} {nameof(Uuid.TimeHighAndVersion)}");
      Assert.That(uuid.ClockSeqHighAndReserved, Is.EqualTo(0xa7), $"{testCaseLabel} {nameof(Uuid.ClockSeqHighAndReserved)}");
      Assert.That(uuid.ClockSeqLow, Is.EqualTo(0x65), $"{testCaseLabel} {nameof(Uuid.ClockSeqLow)}");
      Assert.That(uuid.Node, Is.EqualTo(new byte[] {0x00, 0xa0, 0xc9, 0x1e, 0x6b, 0xf6}), $"{testCaseLabel} {nameof(Uuid.Node)}");
      Assert.That(uuid.Version, Is.EqualTo(UuidVersion.Version1), $"{testCaseLabel} {nameof(Uuid.Version)}");
      Assert.That(uuid.VariantField, Is.EqualTo(Uuid.Variant.RFC4122), $"{testCaseLabel} {nameof(Uuid.VariantField)}");
    }

    TestParsed(Uuid.Parse(s), "Parse<string>");
    TestParsed(Uuid.Parse(s.AsSpan()), "Parse<ReadOnlySpan<char>>");

#if SYSTEM_IPARSABLE
    TestParsed(ParseString<Uuid>(s), "IParsable.Parse<string>");

    static T ParseString<T>(string s) where T : IParsable<T> => T.Parse(s, provider: null);
#endif

#if SYSTEM_ISPANPARSABLE
    TestParsed(ParseReadOnlySpanOfChar<Uuid>(s.AsSpan()), "ISpanParsable.Parse<ReadOnlySpan<char>>");

    static T ParseReadOnlySpanOfChar<T>(ReadOnlySpan<char> s) where T : ISpanParsable<T> => T.Parse(s, provider: null);
#endif
#pragma warning restore CA1305
  }

  [Test]
  public void TestTryParse()
  {
#pragma warning disable CA1305
    const string s = "f81d4fae-7dec-11d0-a765-00a0c91e6bf6";

    static void TestParsed(Uuid uuid, string testCaseLabel)
    {
      Assert.That(uuid.TimeLow, Is.EqualTo(0xf81d4fae), $"{testCaseLabel} {nameof(Uuid.TimeLow)}");
      Assert.That(uuid.TimeMid, Is.EqualTo(0x7dec), $"{testCaseLabel} {nameof(Uuid.TimeMid)}");
      Assert.That(uuid.TimeHighAndVersion, Is.EqualTo(0x11d0), $"{testCaseLabel} {nameof(Uuid.TimeHighAndVersion)}");
      Assert.That(uuid.ClockSeqHighAndReserved, Is.EqualTo(0xa7), $"{testCaseLabel} {nameof(Uuid.ClockSeqHighAndReserved)}");
      Assert.That(uuid.ClockSeqLow, Is.EqualTo(0x65), $"{testCaseLabel} {nameof(Uuid.ClockSeqLow)}");
      Assert.That(uuid.Node, Is.EqualTo(new byte[] {0x00, 0xa0, 0xc9, 0x1e, 0x6b, 0xf6}), $"{testCaseLabel} {nameof(Uuid.Node)}");
      Assert.That(uuid.Version, Is.EqualTo(UuidVersion.Version1), $"{testCaseLabel} {nameof(Uuid.Version)}");
      Assert.That(uuid.VariantField, Is.EqualTo(Uuid.Variant.RFC4122), $"{testCaseLabel} {nameof(Uuid.VariantField)}");
    }

    Assert.That(Uuid.TryParse(s, provider: null, out var uuid0), Is.True, "TryParse<string>");
    Assert.That(Uuid.TryParse(s.AsSpan(), provider: null, out var uuid1), Is.True, "TryParse<ReadOnlySpan<char>>");

    TestParsed(uuid0, "TryParse<string>");
    TestParsed(uuid1, "TryParse<ReadOnlySpan<char>>");

#if SYSTEM_IPARSABLE
    Assert.That(TryParseString<Uuid>(s, out var uuid2), Is.True, "IParsable.TryParse<string>");

    TestParsed(uuid2, "IParsable.TryParse<string>");

    static bool TryParseString<T>(string s, out T result) where T : struct, IParsable<T> => T.TryParse(s, provider: null, out result);
#endif

#if SYSTEM_ISPANPARSABLE
    Assert.That(TryParseReadOnlySpanOfChar<Uuid>(s.AsSpan(), out var uuid3), Is.True, "ISpanParsable.TryParse<ReadOnlySpan<char>>");

    TestParsed(uuid3, "ISpanParsable.TryParse<ReadOnlySpan<char>>");

    static bool TryParseReadOnlySpanOfChar<T>(ReadOnlySpan<char> s, out T result) where T : struct, ISpanParsable<T> => T.TryParse(s, provider: null, out result);
#endif
#pragma warning restore CA1305
  }

  private static System.Collections.IEnumerable YieldTestCases_Parse_FormatException()
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

  [TestCaseSource(nameof(YieldTestCases_Parse_FormatException))]
  public void TestParse_FormatException(string uuid, string expectedExceptionMessage)
  {
#pragma warning disable CA1305
    var exString = Assert.Throws<FormatException>(() => Uuid.Parse(uuid), "Parse<string>");
    Assert.That(exString!.Message, Does.Contain(expectedExceptionMessage), "Parse<string> exception message");

    var exReadOnlySpanOfChar = Assert.Throws<FormatException>(() => Uuid.Parse(uuid.AsSpan()), "Parse<ReadOnlySpan<char>>");
    Assert.That(exReadOnlySpanOfChar!.Message, Does.Contain(expectedExceptionMessage), "Parse<<ReadOnlySpan<char>> exception message");

#if SYSTEM_IPARSABLE
    var exStringGM = Assert.Throws<FormatException>(() => ParseString<Uuid>(uuid), "IParsable.Parse<string>");
    Assert.That(exStringGM.Message, Does.Contain(expectedExceptionMessage), "IParsable.Parse<string> exception message");

    static T ParseString<T>(string s) where T : IParsable<T> => T.Parse(s, provider: null);
#endif

#if SYSTEM_ISPANPARSABLE
    var exReadOnlySpanOfCharGM = Assert.Throws<FormatException>(() => ParseReadOnlySpanOfChar<Uuid>(uuid.AsSpan()), "IParsable.Parse<ReadOnlySpan<char>>");
    Assert.That(exReadOnlySpanOfCharGM.Message, Does.Contain(expectedExceptionMessage), "IParsable.Parse<<ReadOnlySpan<char>> exception message");

    static T ParseReadOnlySpanOfChar<T>(ReadOnlySpan<char> s) where T : ISpanParsable<T> => T.Parse(s, provider: null);
#endif
#pragma warning restore CA1305
  }

  [TestCaseSource(nameof(YieldTestCases_Parse_FormatException))]
  public void TestTryParse_FormatError(string uuid, string discard)
  {
    Assert.That(Uuid.TryParse(uuid, provider: null, out _), Is.False, "TryParse<string>");
    Assert.That(Uuid.TryParse(uuid.AsSpan(), provider: null, out _), Is.False, "TryParse<ReadOnlySpan<char>>");

#if SYSTEM_IPARSABLE
    Assert.That(TryParseString<Uuid>(uuid), Is.False, "IParsable.TryParse<string>");

    static bool TryParseString<T>(string s) where T : IParsable<T> => T.TryParse(s, provider: null, out var _);
#endif

#if SYSTEM_ISPANPARSABLE
    Assert.That(TryParseReadOnlySpanOfChar<Uuid>(uuid.AsSpan()), Is.False, "IParsable.TryParse<ReadOnlySpan<char>>");

    static bool TryParseReadOnlySpanOfChar<T>(ReadOnlySpan<char> s) where T : ISpanParsable<T> => T.TryParse(s, provider: null, out var _);
#endif
  }
}
