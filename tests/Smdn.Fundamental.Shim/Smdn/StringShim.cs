// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public class StringShimTests {
#pragma warning disable NUnit2007
  [Test]
  public void ShimType_StartsWith_Char()
    => Assert.That(
#if SYSTEM_STRING_STARTSWITH_CHAR
      typeof(System.String)
#else
      typeof(Smdn.StringShim)
#endif
      ,
      Is.EqualTo(typeof(ShimTypeSystemStringStartsWithChar))
    );
#pragma warning restore NUnit2007

  [Test]
  public void TestStartsWith()
  {
#if !SYSTEM_STRING_STARTSWITH_CHAR
    string nullString = null!;

    Assert.Throws<ArgumentNullException>(() => nullString.StartsWith('a'));
#endif

    Assert.That(string.Empty.StartsWith('a'), Is.False);
    Assert.That("a".StartsWith('a'), Is.True);
    Assert.That("abc".StartsWith('a'), Is.True);
    Assert.That("abc".StartsWith('c'), Is.False);

    Assert.That(string.Empty.StartsWith('a'), Is.EqualTo(string.Empty.StartsWith("a", StringComparison.Ordinal)),
                    "same as StartsWith(string) #1");

    Assert.That("a".StartsWith('a'), Is.EqualTo("a".StartsWith("a", StringComparison.Ordinal)),
                    "same as StartsWith(string) #2");

    Assert.That("abc".StartsWith('a'), Is.EqualTo("abc".StartsWith("a", StringComparison.Ordinal)),
                    "same as StartsWith(string) #3");

    Assert.That("abc".StartsWith('c'), Is.EqualTo("abc".StartsWith("c", StringComparison.Ordinal)),
                    "same as StartsWith(string) #4");
  }

#pragma warning disable NUnit2007
  [Test]
  public void ShimType_EndsWith_Char()
    => Assert.That(
#if SYSTEM_STRING_ENDSWITH_CHAR
      typeof(System.String)
#else
      typeof(Smdn.StringShim)
#endif
      ,
      Is.EqualTo(typeof(ShimTypeSystemStringEndsWithChar))
    );
#pragma warning restore NUnit2007

  [Test]
  public void TestEndsWith()
  {
#if !SYSTEM_STRING_ENDSWITH_CHAR
    string nullString = null!;

    Assert.Throws<ArgumentNullException>(() => nullString.EndsWith('a'));
#endif

    Assert.That(string.Empty.EndsWith('a'), Is.False);
    Assert.That("a".EndsWith('a'), Is.True);
    Assert.That("abc".EndsWith('c'), Is.True);
    Assert.That("abc".EndsWith('a'), Is.False);

    Assert.That(string.Empty.EndsWith('a'), Is.EqualTo(string.Empty.EndsWith("a", StringComparison.Ordinal)),
                    "same as EndsWith(string) #1");

    Assert.That("a".EndsWith('a'), Is.EqualTo("a".EndsWith("a", StringComparison.Ordinal)),
                    "same as EndsWith(string) #2");

    Assert.That("abc".EndsWith('c'), Is.EqualTo("abc".EndsWith("c", StringComparison.Ordinal)),
                    "same as EndsWith(string) #3");

    Assert.That("abc".EndsWith('a'), Is.EqualTo("abc".EndsWith("a", StringComparison.Ordinal)),
                    "same as EndsWith(string) #4");
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TestConstruct()
  {
    Assert.That(StringShim.Construct(default), Is.EqualTo(string.Empty), "#1");
    Assert.That(StringShim.Construct(stackalloc char[] { 'A' } ), Is.EqualTo("A"), "#2");
    Assert.That(StringShim.Construct(stackalloc char[] { 'A', 'B', 'C' } ), Is.EqualTo("ABC"), "#3");
    Assert.That(StringShim.Construct(stackalloc char[] { 'A', '\u0000', 'C' } ), Is.EqualTo("A\u0000C"), "#4");
    Assert.That(StringShim.Construct(stackalloc char[] { '\uD83C', '\uDF1F' } ), Is.EqualTo("🌟"), "#5");
  }
#endif
}
