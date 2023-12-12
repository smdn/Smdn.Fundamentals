// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Formats;

partial class HexadecimalTests {
#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_OfReadOnlySpanDataSequence_ToByteSpan_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new byte[0], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new byte[1], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new byte[2], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new byte[3], out _));
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfArraySegmentDataSequence_ToByteArraySegment_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[0]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[1]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[2]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[3]), out _));
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_OfReadOnlySpanDataSequence_ToCharSpan_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new char[0], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new char[1], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new char[2], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new char[3], out _));
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfArraySegmentDataSequence_ToCharArraySegment_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[0]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[1]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[2]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[3]), out _));
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfReadOnlySpanDataSequence_ToByteSpan_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new byte[0], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new byte[1], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new byte[2], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new byte[3], out _));
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfArraySegmentDataSequence_ToByteArraySegment_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[0]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[1]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[2]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[3]), out _));
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfReadOnlySpanDataSequence_ToCharSpan_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new char[0], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new char[1], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new char[2], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new char[3], out _));
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfArraySegmentDataSequence_ToCharArraySegment_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[0]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[1]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[2]), out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[3]), out _));
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_OfReadOnlySpanDataSequence_Empty()
  {
    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(ReadOnlySpan<byte>.Empty, Span<byte>.Empty, out _));
    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(ReadOnlySpan<byte>.Empty, Span<char>.Empty, out _));
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfArraySegmentDataSequence_Empty()
  {
    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(CreateEmptyArraySegment<byte>(), CreateEmptyArraySegment<byte>(), out _));
    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(CreateEmptyArraySegment<byte>(), CreateEmptyArraySegment<char>(), out _));
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfReadOnlySpanDataSequence_Empty()
  {
    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(ReadOnlySpan<byte>.Empty, Span<byte>.Empty, out _));
    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(ReadOnlySpan<byte>.Empty, Span<char>.Empty, out _));
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfArraySegmentDataSequence_Empty()
  {
    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(CreateEmptyArraySegment<byte>(), CreateEmptyArraySegment<byte>(), out _));
    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(CreateEmptyArraySegment<byte>(), CreateEmptyArraySegment<char>(), out _));
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_OfReadOnlySpanDataSequence()
  {
    var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destinationBytes = new byte[18];
    var destinationChars = new byte[18];

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(input.AsSpan(), destinationBytes.AsSpan(1, 16), out var bytesWritten));
    Assert.AreEqual(16, bytesWritten, nameof(bytesWritten));
    CollectionAssert.AreEqual(destinationBytes, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, nameof(destinationBytes));

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(input.AsSpan(), destinationChars.AsSpan(1, 16), out var charsWritten));
    Assert.AreEqual(16, charsWritten, nameof(charsWritten));
    CollectionAssert.AreEqual(destinationChars, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, nameof(destinationChars));
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfArraySegmentDataSequence()
  {
    var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destinationBytes = new byte[18];
    var destinationChars = new byte[18];

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationBytes, 1, 16), out var bytesWritten));
    Assert.AreEqual(16, bytesWritten, nameof(bytesWritten));
    CollectionAssert.AreEqual(destinationBytes, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, nameof(destinationBytes));

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationChars, 1, 16), out var charsWritten));
    Assert.AreEqual(16, charsWritten, nameof(charsWritten));
    CollectionAssert.AreEqual(destinationChars, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, nameof(destinationChars));
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfReadOnlySpanDataSequence()
  {
    var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destinationBytes = new byte[18];
    var destinationChars = new byte[18];

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(input.AsSpan(), destinationBytes.AsSpan(1, 16), out var bytesWritten));
    Assert.AreEqual(16, bytesWritten, nameof(bytesWritten));
    CollectionAssert.AreEqual(destinationBytes, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, nameof(destinationBytes));

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(input.AsSpan(), destinationChars.AsSpan(1, 16), out var charsWritten));
    Assert.AreEqual(16, charsWritten, nameof(charsWritten));
    CollectionAssert.AreEqual(destinationChars, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, nameof(destinationChars));
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfArraySegmentDataSequence()
  {
    var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destinationBytes = new byte[18];
    var destinationChars = new byte[18];

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationBytes, 1, 16), out var bytesWritten));
    Assert.AreEqual(16, bytesWritten, nameof(bytesWritten));
    CollectionAssert.AreEqual(destinationBytes, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, nameof(destinationBytes));

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationChars, 1, 16), out var charsWritten));
    Assert.AreEqual(16, charsWritten, nameof(charsWritten));
    CollectionAssert.AreEqual(destinationChars, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, nameof(destinationChars));
  }
}
