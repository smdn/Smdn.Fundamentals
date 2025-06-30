// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class HexadecimalTests {
#pragma warning restore IDE0040
#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_OfReadOnlySpanDataSequence_ToByteSpan_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new byte[0], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new byte[1], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new byte[2], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new byte[3], out _), Is.False);
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfArraySegmentDataSequence_ToByteArraySegment_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[0]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[1]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[2]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[3]), out _), Is.False);
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_OfReadOnlySpanDataSequence_ToCharSpan_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new char[0], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new char[1], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new char[2], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new char[3], out _), Is.False);
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfArraySegmentDataSequence_ToCharArraySegment_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[0]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[1]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[2]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[3]), out _), Is.False);
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfReadOnlySpanDataSequence_ToByteSpan_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new byte[0], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new byte[1], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new byte[2], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new byte[3], out _), Is.False);
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfArraySegmentDataSequence_ToByteArraySegment_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[0]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[1]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[2]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[3]), out _), Is.False);
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfReadOnlySpanDataSequence_ToCharSpan_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new char[0], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new char[1], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new char[2], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new char[3], out _), Is.False);
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfArraySegmentDataSequence_ToCharArraySegment_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[0]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[1]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[2]), out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[3]), out _), Is.False);
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_OfReadOnlySpanDataSequence_Empty()
  {
    Assert.That(Hexadecimal.TryEncodeUpperCase(ReadOnlySpan<byte>.Empty, Span<byte>.Empty, out _), Is.True);
    Assert.That(Hexadecimal.TryEncodeUpperCase(ReadOnlySpan<byte>.Empty, Span<char>.Empty, out _), Is.True);
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfArraySegmentDataSequence_Empty()
  {
    Assert.That(Hexadecimal.TryEncodeUpperCase(CreateEmptyArraySegment<byte>(), CreateEmptyArraySegment<byte>(), out _), Is.True);
    Assert.That(Hexadecimal.TryEncodeUpperCase(CreateEmptyArraySegment<byte>(), CreateEmptyArraySegment<char>(), out _), Is.True);
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfReadOnlySpanDataSequence_Empty()
  {
    Assert.That(Hexadecimal.TryEncodeLowerCase(ReadOnlySpan<byte>.Empty, Span<byte>.Empty, out _), Is.True);
    Assert.That(Hexadecimal.TryEncodeLowerCase(ReadOnlySpan<byte>.Empty, Span<char>.Empty, out _), Is.True);
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfArraySegmentDataSequence_Empty()
  {
    Assert.That(Hexadecimal.TryEncodeLowerCase(CreateEmptyArraySegment<byte>(), CreateEmptyArraySegment<byte>(), out _), Is.True);
    Assert.That(Hexadecimal.TryEncodeLowerCase(CreateEmptyArraySegment<byte>(), CreateEmptyArraySegment<char>(), out _), Is.True);
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_OfReadOnlySpanDataSequence()
  {
    var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destinationBytes = new byte[18];
    var destinationChars = new byte[18];

    Assert.That(Hexadecimal.TryEncodeUpperCase(input.AsSpan(), destinationBytes.AsSpan(1, 16), out var bytesWritten), Is.True);
    Assert.That(bytesWritten, Is.EqualTo(16), nameof(bytesWritten));
    Assert.That(new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, Is.EqualTo(destinationBytes).AsCollection, nameof(destinationBytes));

    Assert.That(Hexadecimal.TryEncodeUpperCase(input.AsSpan(), destinationChars.AsSpan(1, 16), out var charsWritten), Is.True);
    Assert.That(charsWritten, Is.EqualTo(16), nameof(charsWritten));
    Assert.That(new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, Is.EqualTo(destinationChars).AsCollection, nameof(destinationChars));
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfArraySegmentDataSequence()
  {
    var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destinationBytes = new byte[18];
    var destinationChars = new byte[18];

    Assert.That(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationBytes, 1, 16), out var bytesWritten), Is.True);
    Assert.That(bytesWritten, Is.EqualTo(16), nameof(bytesWritten));
    Assert.That(new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, Is.EqualTo(destinationBytes).AsCollection, nameof(destinationBytes));

    Assert.That(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationChars, 1, 16), out var charsWritten), Is.True);
    Assert.That(charsWritten, Is.EqualTo(16), nameof(charsWritten));
    Assert.That(new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, Is.EqualTo(destinationChars).AsCollection, nameof(destinationChars));
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfReadOnlySpanDataSequence()
  {
    var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destinationBytes = new byte[18];
    var destinationChars = new byte[18];

    Assert.That(Hexadecimal.TryEncodeLowerCase(input.AsSpan(), destinationBytes.AsSpan(1, 16), out var bytesWritten), Is.True);
    Assert.That(bytesWritten, Is.EqualTo(16), nameof(bytesWritten));
    Assert.That(new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, Is.EqualTo(destinationBytes).AsCollection, nameof(destinationBytes));

    Assert.That(Hexadecimal.TryEncodeLowerCase(input.AsSpan(), destinationChars.AsSpan(1, 16), out var charsWritten), Is.True);
    Assert.That(charsWritten, Is.EqualTo(16), nameof(charsWritten));
    Assert.That(new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, Is.EqualTo(destinationChars).AsCollection, nameof(destinationChars));
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfArraySegmentDataSequence()
  {
    var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
    var destinationBytes = new byte[18];
    var destinationChars = new byte[18];

    Assert.That(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationBytes, 1, 16), out var bytesWritten), Is.True);
    Assert.That(bytesWritten, Is.EqualTo(16), nameof(bytesWritten));
    Assert.That(new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, Is.EqualTo(destinationBytes).AsCollection, nameof(destinationBytes));

    Assert.That(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationChars, 1, 16), out var charsWritten), Is.True);
    Assert.That(charsWritten, Is.EqualTo(16), nameof(charsWritten));
    Assert.That(new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, Is.EqualTo(destinationChars).AsCollection, nameof(destinationChars));
  }
}
