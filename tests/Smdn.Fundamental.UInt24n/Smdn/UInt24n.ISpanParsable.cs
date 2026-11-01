// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_ISPANPARSABLE
using System;

using NUnit.Framework;

namespace Smdn;

partial class UInt24nTests {
  private static TUInt24n Parse<TUInt24n>(string s) where TUInt24n : ISpanParsable<TUInt24n>
    => TUInt24n.Parse(s, provider: null);

  private static bool TryParse<TUInt24n>(string s, out TUInt24n result) where TUInt24n : struct, ISpanParsable<TUInt24n>
    => TUInt24n.TryParse(s, provider: null, out result);

  [Test]
  public void IParsable_Parse()
  {
    Assert.That(
      Parse<UInt24>("74565"),
      Is.EqualTo((UInt24)0x012345),
      nameof(UInt24)
    );

    Assert.That(
      Parse<UInt48>("1250999896491"),
      Is.EqualTo((UInt48)0x0123456789AB),
      nameof(UInt48)
    );
  }

  [Test]
  public void IParsable_Parse_FormatException()
  {
    Assert.That(() => Parse<UInt24>("ABCDEF"), Throws.TypeOf<FormatException>(), nameof(UInt24));

    Assert.That(() => Parse<UInt48>("456789ABCDEF"), Throws.TypeOf<FormatException>(), nameof(UInt48));
  }

  [Test]
  public void IParsable_Parse_OverflowException()
  {
    Assert.That(() => Parse<UInt24>("-1"), Throws.TypeOf<OverflowException>(), nameof(UInt24));
    Assert.That(() => Parse<UInt24>("16777216"), Throws.TypeOf<OverflowException>(), nameof(UInt24));

    Assert.That(() => Parse<UInt48>("-1"), Throws.TypeOf<OverflowException>(), nameof(UInt48));
    Assert.That(() => Parse<UInt48>("281474976710656"), Throws.TypeOf<OverflowException>(), nameof(UInt48));
  }

  [Test]
  public void IParsable_TryParse()
  {
    Assert.That(TryParse<UInt24>("74565", out var uint24), Is.True, nameof(UInt24));
    Assert.That(uint24, Is.EqualTo((UInt24)0x012345), nameof(UInt24));

    Assert.That(TryParse<UInt48>("1250999896491", out var uint48), Is.True, nameof(UInt48));
    Assert.That(uint48, Is.EqualTo((UInt48)0x0123456789AB), nameof(UInt48));
  }

  [Test]
  public void IParsable_TryParse_FormatException()
  {
    Assert.That(TryParse<UInt24>("ABCDEF", out var uint24), Is.False, nameof(UInt24));

    Assert.That(TryParse<UInt48>("456789ABCDEF", out var uint48), Is.False, nameof(UInt48));
  }

  [Test]
  public void IParsable_TryParse_OverflowException()
  {
    Assert.That(TryParse<UInt24>("-1", out var uint24), Is.False, nameof(UInt24));
    Assert.That(TryParse<UInt24>("16777216", out uint24), Is.False, nameof(UInt24));

    Assert.That(TryParse<UInt48>("-1", out var uint48), Is.False, nameof(UInt48));
    Assert.That(TryParse<UInt48>("281474976710656", out uint48), Is.False, nameof(UInt48));
  }

  private static TUInt24n Parse<TUInt24n>(ReadOnlySpan<char> s) where TUInt24n : ISpanParsable<TUInt24n>
    => TUInt24n.Parse(s, provider: null);

  private static bool TryParse<TUInt24n>(ReadOnlySpan<char> s, out TUInt24n result) where TUInt24n : struct, ISpanParsable<TUInt24n>
    => TUInt24n.TryParse(s, provider: null, out result);

  [Test]
  public void ISpanParsable_Parse()
  {
    Assert.That(
      Parse<UInt24>("74565".AsSpan()),
      Is.EqualTo((UInt24)0x012345),
      nameof(UInt24)
    );

    Assert.That(
      Parse<UInt48>("1250999896491".AsSpan()),
      Is.EqualTo((UInt48)0x0123456789AB),
      nameof(UInt48)
    );
  }

  [Test]
  public void ISpanParsable_Parse_FormatException()
  {
    Assert.That(() => Parse<UInt24>("ABCDEF".AsSpan()), Throws.TypeOf<FormatException>(), nameof(UInt24));

    Assert.That(() => Parse<UInt48>("456789ABCDEF".AsSpan()), Throws.TypeOf<FormatException>(), nameof(UInt48));
  }

  [Test]
  public void ISpanParsable_Parse_OverflowException()
  {
    Assert.That(() => Parse<UInt24>("-1".AsSpan()), Throws.TypeOf<OverflowException>(), nameof(UInt24));
    Assert.That(() => Parse<UInt24>("16777216".AsSpan()), Throws.TypeOf<OverflowException>(), nameof(UInt24));

    Assert.That(() => Parse<UInt48>("-1".AsSpan()), Throws.TypeOf<OverflowException>(), nameof(UInt48));
    Assert.That(() => Parse<UInt48>("281474976710656".AsSpan()), Throws.TypeOf<OverflowException>(), nameof(UInt48));
  }

  [Test]
  public void ISpanParsable_TryParse()
  {
    Assert.That(TryParse<UInt24>("74565".AsSpan(), out var uint24), Is.True, nameof(UInt24));
    Assert.That(uint24, Is.EqualTo((UInt24)0x012345), nameof(UInt24));

    Assert.That(TryParse<UInt48>("1250999896491".AsSpan(), out var uint48), Is.True, nameof(UInt48));
    Assert.That(uint48, Is.EqualTo((UInt48)0x0123456789AB), nameof(UInt48));
  }

  [Test]
  public void ISpanParsable_TryParse_FormatException()
  {
    Assert.That(TryParse<UInt24>("ABCDEF".AsSpan(), out var uint24), Is.False, nameof(UInt24));

    Assert.That(TryParse<UInt48>("456789ABCDEF".AsSpan(), out var uint48), Is.False, nameof(UInt48));
  }

  [Test]
  public void ISpanParsable_TryParse_OverflowException()
  {
    Assert.That(TryParse<UInt24>("-1".AsSpan(), out var uint24), Is.False, nameof(UInt24));
    Assert.That(TryParse<UInt24>("16777216".AsSpan(), out uint24), Is.False, nameof(UInt24));

    Assert.That(TryParse<UInt48>("-1".AsSpan(), out var uint48), Is.False, nameof(UInt48));
    Assert.That(TryParse<UInt48>("281474976710656".AsSpan(), out uint48), Is.False, nameof(UInt48));
  }
}
#endif
