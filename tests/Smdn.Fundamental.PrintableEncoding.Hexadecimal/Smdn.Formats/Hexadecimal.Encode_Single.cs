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
  public void TryEncodeUpperCase_OfData_ToByteSpan_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new byte[0], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new byte[1], out _), Is.False);
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfData_ToByteArray_InvalidArguments()
  {
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, (byte[])null!, 0, out _), Is.False); // null
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new byte[0], 0, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new byte[1], 0, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new byte[2], 1, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new byte[2], -1, out _), Is.False); // out of range
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfData_ToByteSpan_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new byte[0], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new byte[1], out _), Is.False);
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfData_ToByteArray_InvalidArguments()
  {
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, (byte[])null!, 0, out _), Is.False); // null
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new byte[0], 0, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new byte[1], 0, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new byte[2], 1, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new byte[2], -1, out _), Is.False); // out of range
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_OfData_ToCharSpan_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new char[0], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new char[1], out _), Is.False);
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfData_ToCharArray_InvalidArguments()
  {
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, (char[])null!, 0, out _), Is.False); // null
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new char[0], 0, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new char[1], 0, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new char[2], 1, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeUpperCase(0x00, new char[2], -1, out _), Is.False); // out of range
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfData_ToCharSpan_DestinationTooShort()
  {
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new char[0], out _), Is.False);
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new char[1], out _), Is.False);
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfData_ToCharArray_InvalidArguments()
  {
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, (char[])null!, 0, out _), Is.False); // null
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new char[0], 0, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new char[1], 0, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new char[2], 1, out _), Is.False); // destination too short
    Assert.That(Hexadecimal.TryEncodeLowerCase(0x00, new char[2], -1, out _), Is.False); // out of range
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_DestinationSpan()
  {
    var dest = new byte[4];

    Assert.That(Hexadecimal.TryEncodeUpperCase(0x01, dest.AsSpan(0), out var bytesEncoded), Is.True, $"#0 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#0 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x30, 0x31, 0x00, 0x00 }, Is.EqualTo(dest).AsCollection, $"#0 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.That(Hexadecimal.TryEncodeUpperCase(0x23, dest.AsSpan(1), out bytesEncoded), Is.True, $"#1 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#1 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x00, 0x32, 0x33, 0x00 }, Is.EqualTo(dest).AsCollection, $"#1 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.That(Hexadecimal.TryEncodeUpperCase(0xAB, dest.AsSpan(2), out bytesEncoded), Is.True, $"#2 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#2 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x00, 0x00, 0x41, 0x42 }, Is.EqualTo(dest).AsCollection, $"#2 {dest}");
  }
#endif

  [Test]
  public void TryEncodeUpperCase_DestinationArray()
  {
    var dest = new byte[4];

    Assert.That(Hexadecimal.TryEncodeUpperCase(0x01, dest, 0, out var bytesEncoded), Is.True, $"#0 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#0 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x30, 0x31, 0x00, 0x00 }, Is.EqualTo(dest).AsCollection, $"#0 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.That(Hexadecimal.TryEncodeUpperCase(0x23, dest, 1, out bytesEncoded), Is.True, $"#1 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#1 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x00, 0x32, 0x33, 0x00 }, Is.EqualTo(dest).AsCollection, $"#1 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.That(Hexadecimal.TryEncodeUpperCase(0xAB, dest, 2, out bytesEncoded), Is.True, $"#2 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#2 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x00, 0x00, 0x41, 0x42 }, Is.EqualTo(dest).AsCollection, $"#2 {dest}");
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_DestinationSpan()
  {
    var dest = new byte[4];

    Assert.That(Hexadecimal.TryEncodeLowerCase(0x01, dest.AsSpan(0), out var bytesEncoded), Is.True, $"#0 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#0 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x30, 0x31, 0x00, 0x00 }, Is.EqualTo(dest).AsCollection, $"#0 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.That(Hexadecimal.TryEncodeLowerCase(0x23, dest.AsSpan(1), out bytesEncoded), Is.True, $"#1 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#1 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x00, 0x32, 0x33, 0x00 }, Is.EqualTo(dest).AsCollection, $"#1 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.That(Hexadecimal.TryEncodeLowerCase(0xAB, dest.AsSpan(2), out bytesEncoded), Is.True, $"#2 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#2 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x00, 0x00, 0x61, 0x62 }, Is.EqualTo(dest).AsCollection, $"#2 {dest}");
  }
#endif

  [Test]
  public void TryEncodeLowerCase_DestinationArray()
  {
    var dest = new byte[4];

    Assert.That(Hexadecimal.TryEncodeLowerCase(0x01, dest, 0, out var bytesEncoded), Is.True, $"#0 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#0 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x30, 0x31, 0x00, 0x00 }, Is.EqualTo(dest).AsCollection, $"#0 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.That(Hexadecimal.TryEncodeLowerCase(0x23, dest, 1, out bytesEncoded), Is.True, $"#1 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#1 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x00, 0x32, 0x33, 0x00 }, Is.EqualTo(dest).AsCollection, $"#1 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.That(Hexadecimal.TryEncodeLowerCase(0xAB, dest, 2, out bytesEncoded), Is.True, $"#2 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"#2 {nameof(bytesEncoded)}");
    Assert.That(new byte[] { 0x00, 0x00, 0x61, 0x62 }, Is.EqualTo(dest).AsCollection, $"#2 {dest}");
  }

#if SYSTEM_SPAN
  [TestCase(0x01, '0', '1')]
  [TestCase(0x23, '2', '3')]
  [TestCase(0x45, '4', '5')]
  [TestCase(0x67, '6', '7')]
  [TestCase(0x89, '8', '9')]
  [TestCase(0xAB, 'A', 'B')]
  [TestCase(0xCD, 'C', 'D')]
  [TestCase(0xEF, 'E', 'F')]
  public void TryEncodeUpperCase_OfData_ToSpan(byte data, char expectedHigh, char expectedLow)
  {
    var bytes = new byte[2];

    Assert.That(Hexadecimal.TryEncodeUpperCase(data, bytes.AsSpan(), out var bytesEncoded), Is.True, $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<byte>");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"{nameof(bytesEncoded)}");
    Assert.That((byte)expectedHigh, Is.EqualTo(bytes[0]), "bytes[0]");
    Assert.That((byte)expectedLow, Is.EqualTo(bytes[1]), "bytes[1]");

    var chars = new char[2];

    Assert.That(Hexadecimal.TryEncodeUpperCase(data, chars.AsSpan(), out var charsEncoded), Is.True, $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<char>");
    Assert.That(charsEncoded, Is.EqualTo(2), $"{nameof(bytesEncoded)}");
    Assert.That(chars[0], Is.EqualTo(expectedHigh), "chars[0]");
    Assert.That(chars[1], Is.EqualTo(expectedLow), "chars[1]");
  }
#endif

  [TestCase(0x01, '0', '1')]
  [TestCase(0x23, '2', '3')]
  [TestCase(0x45, '4', '5')]
  [TestCase(0x67, '6', '7')]
  [TestCase(0x89, '8', '9')]
  [TestCase(0xAB, 'A', 'B')]
  [TestCase(0xCD, 'C', 'D')]
  [TestCase(0xEF, 'E', 'F')]
  public void TryEncodeUpperCase_OfData_ToArray(byte data, char expectedHigh, char expectedLow)
  {
    var bytes = new byte[2];

    Assert.That(Hexadecimal.TryEncodeUpperCase(data, bytes, 0, out var bytesEncoded), Is.True, $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<byte>");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"{nameof(bytesEncoded)}");
    Assert.That((byte)expectedHigh, Is.EqualTo(bytes[0]), "bytes[0]");
    Assert.That((byte)expectedLow, Is.EqualTo(bytes[1]), "bytes[1]");

    var chars = new char[2];

    Assert.That(Hexadecimal.TryEncodeUpperCase(data, chars, 0, out var charsEncoded), Is.True, $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<char>");
    Assert.That(charsEncoded, Is.EqualTo(2), $"{nameof(bytesEncoded)}");
    Assert.That(chars[0], Is.EqualTo(expectedHigh), "chars[0]");
    Assert.That(chars[1], Is.EqualTo(expectedLow), "chars[1]");
  }

#if SYSTEM_SPAN
  [TestCase(0x01, '0', '1')]
  [TestCase(0x23, '2', '3')]
  [TestCase(0x45, '4', '5')]
  [TestCase(0x67, '6', '7')]
  [TestCase(0x89, '8', '9')]
  [TestCase(0xAB, 'a', 'b')]
  [TestCase(0xCD, 'c', 'd')]
  [TestCase(0xEF, 'e', 'f')]
  public void TryEncodeLowerCase_OfData_ToSpan(byte data, char expectedHigh, char expectedLow)
  {
    var bytes = new byte[2];

    Assert.That(Hexadecimal.TryEncodeLowerCase(data, bytes.AsSpan(), out var bytesEncoded), Is.True, $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<byte>");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"{nameof(bytesEncoded)}");
    Assert.That((byte)expectedHigh, Is.EqualTo(bytes[0]), "bytes[0]");
    Assert.That((byte)expectedLow, Is.EqualTo(bytes[1]), "bytes[1]");

    var chars = new char[2];

    Assert.That(Hexadecimal.TryEncodeLowerCase(data, chars.AsSpan(), out var charsEncoded), Is.True, $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<char>");
    Assert.That(charsEncoded, Is.EqualTo(2), $"{nameof(bytesEncoded)}");
    Assert.That(chars[0], Is.EqualTo(expectedHigh), "chars[0]");
    Assert.That(chars[1], Is.EqualTo(expectedLow), "chars[1]");
  }
#endif

  [TestCase(0x01, '0', '1')]
  [TestCase(0x23, '2', '3')]
  [TestCase(0x45, '4', '5')]
  [TestCase(0x67, '6', '7')]
  [TestCase(0x89, '8', '9')]
  [TestCase(0xAB, 'a', 'b')]
  [TestCase(0xCD, 'c', 'd')]
  [TestCase(0xEF, 'e', 'f')]
  public void TryEncodeLowerCase_OfData_ToArray(byte data, char expectedHigh, char expectedLow)
  {
    var bytes = new byte[2];

    Assert.That(Hexadecimal.TryEncodeLowerCase(data, bytes, 0, out var bytesEncoded), Is.True, $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<byte>");
    Assert.That(bytesEncoded, Is.EqualTo(2), $"{nameof(bytesEncoded)}");
    Assert.That((byte)expectedHigh, Is.EqualTo(bytes[0]), "bytes[0]");
    Assert.That((byte)expectedLow, Is.EqualTo(bytes[1]), "bytes[1]");

    var chars = new char[2];

    Assert.That(Hexadecimal.TryEncodeLowerCase(data, chars, 0, out var charsEncoded), Is.True, $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<char>");
    Assert.That(charsEncoded, Is.EqualTo(2), $"{nameof(bytesEncoded)}");
    Assert.That(chars[0], Is.EqualTo(expectedHigh), "chars[0]");
    Assert.That(chars[1], Is.EqualTo(expectedLow), "chars[1]");
  }
}
