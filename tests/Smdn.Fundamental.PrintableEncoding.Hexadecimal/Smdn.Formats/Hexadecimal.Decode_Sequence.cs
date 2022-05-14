// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

using Is = Smdn.Test.NUnit.Constraints.Buffers.Is;

namespace Smdn.Formats;

partial class HexadecimalTests {
#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfCharSequence_IncorrectForm()
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

  [Test]
  public void TryDecode_OfCharSequence_UpperCase()
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

  [Test]
  public void TryDecode_OfCharSequence_LowerCase()
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

  [Test]
  public void TryDecode_OfCharSequence_Empty()
  {
    Assert.IsTrue(Hexadecimal.TryDecode(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength0), nameof(Hexadecimal.TryDecode));
    Assert.AreEqual(0, decodedLength0, nameof(decodedLength0));

    Assert.IsTrue(Hexadecimal.TryDecodeUpperCase(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength1), nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.AreEqual(0, decodedLength1, nameof(decodedLength1));

    Assert.IsTrue(Hexadecimal.TryDecodeLowerCase(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength2), nameof(Hexadecimal.TryDecodeLowerCase));
    Assert.AreEqual(0, decodedLength2, nameof(decodedLength1));
  }

  [Test]
  public void TryDecode_OfCharSequence_DestinationTooShort()
  {
    const string dataSequence = "0123456789abcdefABCDEF";

    Assert.IsFalse(Hexadecimal.TryDecode(dataSequence.AsSpan(), Span<byte>.Empty, out _), nameof(Hexadecimal.TryDecode));
    Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(dataSequence.AsSpan(), Span<byte>.Empty, out _), nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(dataSequence.AsSpan(), Span<byte>.Empty, out _), nameof(Hexadecimal.TryDecodeLowerCase));
  }
#endif
}
