// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Formats;

partial class HexadecimalTests {
#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_OfData_ToByteSpan_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[0], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[1], out _));
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfData_ToByteArray_InvalidArguments()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, (byte[])null, 0, out _)); // null
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[0], 0, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[1], 0, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[2], 1, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[2], -1, out _)); // out of range
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfData_ToByteSpan_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[0], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[1], out _));
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfData_ToByteArray_InvalidArguments()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, (byte[])null, 0, out _)); // null
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[0], 0, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[1], 0, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[2], 1, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[2], -1, out _)); // out of range
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_OfData_ToCharSpan_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[0], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[1], out _));
  }
#endif

  [Test]
  public void TryEncodeUpperCase_OfData_ToCharArray_InvalidArguments()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, (char[])null, 0, out _)); // null
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[0], 0, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[1], 0, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[2], 1, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[2], -1, out _)); // out of range
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_OfData_ToCharSpan_DestinatioTooShort()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[0], out _));
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[1], out _));
  }
#endif

  [Test]
  public void TryEncodeLowerCase_OfData_ToCharArray_InvalidArguments()
  {
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, (char[])null, 0, out _)); // null
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[0], 0, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[1], 0, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[2], 1, out _)); // destination too short
    Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[2], -1, out _)); // out of range
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeUpperCase_DestinationSpan()
  {
    var dest = new byte[4];

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0x01, dest.AsSpan(0), out var bytesEncoded), $"#0 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#0 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x30, 0x31, 0x00, 0x00}, $"#0 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0x23, dest.AsSpan(1), out bytesEncoded), $"#1 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#1 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x32, 0x33, 0x00}, $"#1 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0xAB, dest.AsSpan(2), out bytesEncoded), $"#2 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#2 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x00, 0x41, 0x42}, $"#2 {dest}");
  }
#endif

  [Test]
  public void TryEncodeUpperCase_DestinationArray()
  {
    var dest = new byte[4];

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0x01, dest, 0 , out var bytesEncoded), $"#0 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#0 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x30, 0x31, 0x00, 0x00}, $"#0 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0x23, dest, 1, out bytesEncoded), $"#1 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#1 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x32, 0x33, 0x00}, $"#1 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0xAB, dest, 2, out bytesEncoded), $"#2 {nameof(Hexadecimal.TryEncodeUpperCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#2 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x00, 0x41, 0x42}, $"#2 {dest}");
  }

#if SYSTEM_SPAN
  [Test]
  public void TryEncodeLowerCase_DestinationSpan()
  {
    var dest = new byte[4];

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0x01, dest.AsSpan(0), out var bytesEncoded), $"#0 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#0 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x30, 0x31, 0x00, 0x00}, $"#0 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0x23, dest.AsSpan(1), out bytesEncoded), $"#1 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#1 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x32, 0x33, 0x00}, $"#1 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0xAB, dest.AsSpan(2), out bytesEncoded), $"#2 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#2 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x00, 0x61, 0x62}, $"#2 {dest}");
  }
#endif

  [Test]
  public void TryEncodeLowerCase_DestinationArray()
  {
    var dest = new byte[4];

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0x01, dest, 0, out var bytesEncoded), $"#0 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#0 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x30, 0x31, 0x00, 0x00}, $"#0 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0x23, dest, 1, out bytesEncoded), $"#1 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#1 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x32, 0x33, 0x00}, $"#1 {dest}");

    Array.Clear(dest, 0, dest.Length);

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0xAB, dest, 2, out bytesEncoded), $"#2 {nameof(Hexadecimal.TryEncodeLowerCase)}");
    Assert.AreEqual(bytesEncoded, 2, $"#2 {nameof(bytesEncoded)}");
    CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x00, 0x61, 0x62}, $"#2 {dest}");
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

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(data, bytes.AsSpan(), out var bytesEncoded), $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<byte>");
    Assert.AreEqual(bytesEncoded, 2, $"{nameof(bytesEncoded)}");
    Assert.AreEqual(bytes[0], (byte)expectedHigh, "bytes[0]");
    Assert.AreEqual(bytes[1], (byte)expectedLow, "bytes[1]");

    var chars = new char[2];

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(data, chars.AsSpan(), out var charsEncoded), $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<char>");
    Assert.AreEqual(charsEncoded, 2, $"{nameof(bytesEncoded)}");
    Assert.AreEqual(chars[0], expectedHigh, "chars[0]");
    Assert.AreEqual(chars[1], expectedLow, "chars[1]");
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

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(data, bytes, 0, out var bytesEncoded), $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<byte>");
    Assert.AreEqual(bytesEncoded, 2, $"{nameof(bytesEncoded)}");
    Assert.AreEqual(bytes[0], (byte)expectedHigh, "bytes[0]");
    Assert.AreEqual(bytes[1], (byte)expectedLow, "bytes[1]");

    var chars = new char[2];

    Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(data, chars, 0, out var charsEncoded), $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<char>");
    Assert.AreEqual(charsEncoded, 2, $"{nameof(bytesEncoded)}");
    Assert.AreEqual(chars[0], expectedHigh, "chars[0]");
    Assert.AreEqual(chars[1], expectedLow, "chars[1]");
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

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(data, bytes.AsSpan(), out var bytesEncoded), $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<byte>");
    Assert.AreEqual(bytesEncoded, 2, $"{nameof(bytesEncoded)}");
    Assert.AreEqual(bytes[0], (byte)expectedHigh, "bytes[0]");
    Assert.AreEqual(bytes[1], (byte)expectedLow, "bytes[1]");

    var chars = new char[2];

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(data, chars.AsSpan(), out var charsEncoded), $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<char>");
    Assert.AreEqual(charsEncoded, 2, $"{nameof(bytesEncoded)}");
    Assert.AreEqual(chars[0], expectedHigh, "chars[0]");
    Assert.AreEqual(chars[1], expectedLow, "chars[1]");
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

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(data, bytes, 0, out var bytesEncoded), $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<byte>");
    Assert.AreEqual(bytesEncoded, 2, $"{nameof(bytesEncoded)}");
    Assert.AreEqual(bytes[0], (byte)expectedHigh, "bytes[0]");
    Assert.AreEqual(bytes[1], (byte)expectedLow, "bytes[1]");

    var chars = new char[2];

    Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(data, chars, 0, out var charsEncoded), $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<char>");
    Assert.AreEqual(charsEncoded, 2, $"{nameof(bytesEncoded)}");
    Assert.AreEqual(chars[0], expectedHigh, "chars[0]");
    Assert.AreEqual(chars[1], expectedLow, "chars[1]");
  }
}
