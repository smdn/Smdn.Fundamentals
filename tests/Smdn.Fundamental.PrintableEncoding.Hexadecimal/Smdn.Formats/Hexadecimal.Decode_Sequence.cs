// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

using Is = Smdn.Test.NUnit.Constraints.Buffers.Is;

namespace Smdn.Formats;

partial class HexadecimalTests {
  private static ArraySegment<char> ToArraySegment(string input)
    => new(input.ToCharArray());

  [Test]
  public void TryDecode_OfCharArraySegmentSequence_IncorrectForm()
  {
    var destination = new ArraySegment<byte>(new byte[16]);

    Assert.That(Hexadecimal.TryDecode(ToArraySegment("0"), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeUpperCase(ToArraySegment("0"), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase(ToArraySegment("0"), destination, out _), Is.False);

    Assert.That(Hexadecimal.TryDecode(ToArraySegment("012"), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeUpperCase(ToArraySegment("012"), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase(ToArraySegment("012"), destination, out _), Is.False);

    Assert.That(Hexadecimal.TryDecode(ToArraySegment("XX"), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeUpperCase(ToArraySegment("XX"), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase(ToArraySegment("XX"), destination, out _), Is.False);

    Assert.That(Hexadecimal.TryDecodeUpperCase(ToArraySegment("ab"), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase(ToArraySegment("AB"), destination, out _), Is.False);
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfReadOnlyCharSpanSequence_IncorrectForm()
  {
    var destination = new byte[16];

    Assert.That(Hexadecimal.TryDecode("0".AsSpan(), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeUpperCase("0".AsSpan(), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase("0".AsSpan(), destination, out _), Is.False);

    Assert.That(Hexadecimal.TryDecode("012".AsSpan(), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeUpperCase("012".AsSpan(), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase("012".AsSpan(), destination, out _), Is.False);

    Assert.That(Hexadecimal.TryDecode("XX".AsSpan(), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeUpperCase("XX".AsSpan(), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase("XX".AsSpan(), destination, out _), Is.False);

    Assert.That(Hexadecimal.TryDecodeUpperCase("ab".AsSpan(), destination, out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase("AB".AsSpan(), destination, out _), Is.False);
  }
#endif

  [Test]
  public void TryDecode_OfCharArraySegmentSequence_UpperCase()
  {
    const string dataSequence = "0123456789ABCDEF";
    var expected = new byte[] { 0x00, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0x00 };
    var destination = new byte[expected.Length];

    Assert.That(Hexadecimal.TryDecode(ToArraySegment(dataSequence), new ArraySegment<byte>(destination, 1, 8), out var decodedLength0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decodedLength0, Is.EqualTo(8), nameof(decodedLength0));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));

    Assert.That(Hexadecimal.TryDecodeUpperCase(ToArraySegment(dataSequence), new ArraySegment<byte>(destination, 1, 8), out var decodedLength1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decodedLength1, Is.EqualTo(8), nameof(decodedLength1));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfReadOnlyCharSpanSequence_UpperCase()
  {
    const string dataSequence = "0123456789ABCDEF";
    var expected = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destination = new byte[expected.Length];

    Assert.That(Hexadecimal.TryDecode(dataSequence.AsSpan(), destination, out var decodedLength0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decodedLength0, Is.EqualTo(expected.Length), nameof(decodedLength0));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));

    Assert.That(Hexadecimal.TryDecodeUpperCase(dataSequence.AsSpan(), destination, out var decodedLength1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decodedLength1, Is.EqualTo(expected.Length), nameof(decodedLength1));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));
  }
#endif

  [Test]
  public void TryDecode_OfCharArraySegmentSequence_LowerCase()
  {
    const string dataSequence = "0123456789abcdef";
    var expected = new byte[] { 0x00, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0x00 };
    var destination = new byte[expected.Length];

    Assert.That(Hexadecimal.TryDecode(ToArraySegment(dataSequence), new ArraySegment<byte>(destination, 1, 8), out var decodedLength0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decodedLength0, Is.EqualTo(8), nameof(decodedLength0));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));

    Assert.That(Hexadecimal.TryDecodeLowerCase(ToArraySegment(dataSequence), new ArraySegment<byte>(destination, 1, 8), out var decodedLength1), Is.True, nameof(Hexadecimal.TryDecodeLowerCase));
    Assert.That(decodedLength1, Is.EqualTo(8), nameof(decodedLength1));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfReadOnlyCharSpanSequence_LowerCase()
  {
    const string dataSequence = "0123456789abcdef";
    var expected = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destination = new byte[expected.Length];

    Assert.That(Hexadecimal.TryDecode(dataSequence.AsSpan(), destination, out var decodedLength0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decodedLength0, Is.EqualTo(expected.Length), nameof(decodedLength0));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));

    Assert.That(Hexadecimal.TryDecodeLowerCase(dataSequence.AsSpan(), destination, out var decodedLength1), Is.True, nameof(Hexadecimal.TryDecodeLowerCase));
    Assert.That(decodedLength1, Is.EqualTo(expected.Length), nameof(decodedLength1));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));
  }
#endif

  [Test]
  public void TryDecode_OfCharArraySegmentSequence_Empty()
  {
    Assert.That(Hexadecimal.TryDecode(CreateEmptyArraySegment<char>(), CreateEmptyArraySegment<byte>(), out var decodedLength0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decodedLength0, Is.Zero, nameof(decodedLength0));

    Assert.That(Hexadecimal.TryDecodeUpperCase(CreateEmptyArraySegment<char>(), CreateEmptyArraySegment<byte>(), out var decodedLength1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decodedLength1, Is.Zero, nameof(decodedLength1));

    Assert.That(Hexadecimal.TryDecodeLowerCase(CreateEmptyArraySegment<char>(), CreateEmptyArraySegment<byte>(), out var decodedLength2), Is.True, nameof(Hexadecimal.TryDecodeLowerCase));
    Assert.That(decodedLength2, Is.Zero, nameof(decodedLength1));
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfReadOnlyCharSpanSequence_Empty()
  {
    Assert.That(Hexadecimal.TryDecode(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decodedLength0, Is.Zero, nameof(decodedLength0));

    Assert.That(Hexadecimal.TryDecodeUpperCase(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decodedLength1, Is.Zero, nameof(decodedLength1));

    Assert.That(Hexadecimal.TryDecodeLowerCase(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength2), Is.True, nameof(Hexadecimal.TryDecodeLowerCase));
    Assert.That(decodedLength2, Is.Zero, nameof(decodedLength1));
  }
#endif

  [Test]
  public void TryDecode_OfCharArraySegmentSequence_DestinationTooShort()
  {
    const string dataSequence = "0123456789abcdefABCDEF";

    Assert.That(Hexadecimal.TryDecode(ToArraySegment(dataSequence), CreateEmptyArraySegment<byte>(), out _), Is.False, nameof(Hexadecimal.TryDecode));
    Assert.That(Hexadecimal.TryDecodeUpperCase(ToArraySegment(dataSequence), CreateEmptyArraySegment<byte>(), out _), Is.False, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(Hexadecimal.TryDecodeLowerCase(ToArraySegment(dataSequence), CreateEmptyArraySegment<byte>(), out _), Is.False, nameof(Hexadecimal.TryDecodeLowerCase));
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfReadOnlyCharSpanSequence_DestinationTooShort()
  {
    const string dataSequence = "0123456789abcdefABCDEF";

    Assert.That(Hexadecimal.TryDecode(dataSequence.AsSpan(), Span<byte>.Empty, out _), Is.False, nameof(Hexadecimal.TryDecode));
    Assert.That(Hexadecimal.TryDecodeUpperCase(dataSequence.AsSpan(), Span<byte>.Empty, out _), Is.False, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(Hexadecimal.TryDecodeLowerCase(dataSequence.AsSpan(), Span<byte>.Empty, out _), Is.False, nameof(Hexadecimal.TryDecodeLowerCase));
  }
#endif
}
