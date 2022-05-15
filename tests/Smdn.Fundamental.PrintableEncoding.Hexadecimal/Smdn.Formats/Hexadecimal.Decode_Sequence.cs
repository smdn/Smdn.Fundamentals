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

    Assert.IsFalse(Hexadecimal.TryDecode(ToArraySegment("0"), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(ToArraySegment("0"), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(ToArraySegment("0"), destination, out _));

    Assert.IsFalse(Hexadecimal.TryDecode(ToArraySegment("012"), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(ToArraySegment("012"), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(ToArraySegment("012"), destination, out _));

    Assert.IsFalse(Hexadecimal.TryDecode(ToArraySegment("XX"), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(ToArraySegment("XX"), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(ToArraySegment("XX"), destination, out _));

    Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(ToArraySegment("ab"), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(ToArraySegment("AB"), destination, out _));
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfReadOnlyCharSpanSequence_IncorrectForm()
  {
    var destination = new byte[16];

    Assert.IsFalse(Hexadecimal.TryDecode("0".AsSpan(), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeUpperCase("0".AsSpan(), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeLowerCase("0".AsSpan(), destination, out _));

    Assert.IsFalse(Hexadecimal.TryDecode("012".AsSpan(), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeUpperCase("012".AsSpan(), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeLowerCase("012".AsSpan(), destination, out _));

    Assert.IsFalse(Hexadecimal.TryDecode("XX".AsSpan(), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeUpperCase("XX".AsSpan(), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeLowerCase("XX".AsSpan(), destination, out _));

    Assert.IsFalse(Hexadecimal.TryDecodeUpperCase("ab".AsSpan(), destination, out _));
    Assert.IsFalse(Hexadecimal.TryDecodeLowerCase("AB".AsSpan(), destination, out _));
  }
#endif

  [Test]
  public void TryDecode_OfCharArraySegmentSequence_UpperCase()
  {
    const string dataSequence = "0123456789ABCDEF";
    var expected = new byte[] { 0x00, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0x00 };
    var destination = new byte[expected.Length];

    Assert.IsTrue(Hexadecimal.TryDecode(ToArraySegment(dataSequence), new ArraySegment<byte>(destination, 1, 8), out var decodedLength0), nameof(Hexadecimal.TryDecode));
    Assert.AreEqual(8, decodedLength0, nameof(decodedLength0));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));

    Assert.IsTrue(Hexadecimal.TryDecodeUpperCase(ToArraySegment(dataSequence), new ArraySegment<byte>(destination, 1, 8), out var decodedLength1), nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.AreEqual(8, decodedLength1, nameof(decodedLength1));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfReadOnlyCharSpanSequence_UpperCase()
  {
    const string dataSequence = "0123456789ABCDEF";
    var expected = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destination = new byte[expected.Length];

    Assert.IsTrue(Hexadecimal.TryDecode(dataSequence.AsSpan(), destination, out var decodedLength0), nameof(Hexadecimal.TryDecode));
    Assert.AreEqual(expected.Length, decodedLength0, nameof(decodedLength0));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));

    Assert.IsTrue(Hexadecimal.TryDecodeUpperCase(dataSequence.AsSpan(), destination, out var decodedLength1), nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.AreEqual(expected.Length, decodedLength1, nameof(decodedLength1));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));
  }
#endif

  [Test]
  public void TryDecode_OfCharArraySegmentSequence_LowerCase()
  {
    const string dataSequence = "0123456789abcdef";
    var expected = new byte[] { 0x00, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0x00 };
    var destination = new byte[expected.Length];

    Assert.IsTrue(Hexadecimal.TryDecode(ToArraySegment(dataSequence), new ArraySegment<byte>(destination, 1, 8), out var decodedLength0), nameof(Hexadecimal.TryDecode));
    Assert.AreEqual(8, decodedLength0, nameof(decodedLength0));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));

    Assert.IsTrue(Hexadecimal.TryDecodeLowerCase(ToArraySegment(dataSequence), new ArraySegment<byte>(destination, 1, 8), out var decodedLength1), nameof(Hexadecimal.TryDecodeLowerCase));
    Assert.AreEqual(8, decodedLength1, nameof(decodedLength1));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfReadOnlyCharSpanSequence_LowerCase()
  {
    const string dataSequence = "0123456789abcdef";
    var expected = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destination = new byte[expected.Length];

    Assert.IsTrue(Hexadecimal.TryDecode(dataSequence.AsSpan(), destination, out var decodedLength0), nameof(Hexadecimal.TryDecode));
    Assert.AreEqual(expected.Length, decodedLength0, nameof(decodedLength0));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));

    Assert.IsTrue(Hexadecimal.TryDecodeLowerCase(dataSequence.AsSpan(), destination, out var decodedLength1), nameof(Hexadecimal.TryDecodeLowerCase));
    Assert.AreEqual(expected.Length, decodedLength1, nameof(decodedLength1));
    Assert.That(destination, Is.EqualTo(expected), nameof(destination));
  }
#endif

  [Test]
  public void TryDecode_OfCharArraySegmentSequence_Empty()
  {
    Assert.IsTrue(Hexadecimal.TryDecode(CreateEmptyArraySegment<char>(), CreateEmptyArraySegment<byte>(), out var decodedLength0), nameof(Hexadecimal.TryDecode));
    Assert.AreEqual(0, decodedLength0, nameof(decodedLength0));

    Assert.IsTrue(Hexadecimal.TryDecodeUpperCase(CreateEmptyArraySegment<char>(), CreateEmptyArraySegment<byte>(), out var decodedLength1), nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.AreEqual(0, decodedLength1, nameof(decodedLength1));

    Assert.IsTrue(Hexadecimal.TryDecodeLowerCase(CreateEmptyArraySegment<char>(), CreateEmptyArraySegment<byte>(), out var decodedLength2), nameof(Hexadecimal.TryDecodeLowerCase));
    Assert.AreEqual(0, decodedLength2, nameof(decodedLength1));
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfReadOnlyCharSpanSequence_Empty()
  {
    Assert.IsTrue(Hexadecimal.TryDecode(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength0), nameof(Hexadecimal.TryDecode));
    Assert.AreEqual(0, decodedLength0, nameof(decodedLength0));

    Assert.IsTrue(Hexadecimal.TryDecodeUpperCase(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength1), nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.AreEqual(0, decodedLength1, nameof(decodedLength1));

    Assert.IsTrue(Hexadecimal.TryDecodeLowerCase(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength2), nameof(Hexadecimal.TryDecodeLowerCase));
    Assert.AreEqual(0, decodedLength2, nameof(decodedLength1));
  }
#endif

  [Test]
  public void TryDecode_OfCharArraySegmentSequence_DestinationTooShort()
  {
    const string dataSequence = "0123456789abcdefABCDEF";

    Assert.IsFalse(Hexadecimal.TryDecode(ToArraySegment(dataSequence), CreateEmptyArraySegment<byte>(), out _), nameof(Hexadecimal.TryDecode));
    Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(ToArraySegment(dataSequence), CreateEmptyArraySegment<byte>(), out _), nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(ToArraySegment(dataSequence), CreateEmptyArraySegment<byte>(), out _), nameof(Hexadecimal.TryDecodeLowerCase));
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfReadOnlyCharSpanSequence_DestinationTooShort()
  {
    const string dataSequence = "0123456789abcdefABCDEF";

    Assert.IsFalse(Hexadecimal.TryDecode(dataSequence.AsSpan(), Span<byte>.Empty, out _), nameof(Hexadecimal.TryDecode));
    Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(dataSequence.AsSpan(), Span<byte>.Empty, out _), nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(dataSequence.AsSpan(), Span<byte>.Empty, out _), nameof(Hexadecimal.TryDecodeLowerCase));
  }
#endif
}
