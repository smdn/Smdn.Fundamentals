// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Formats;

partial class HexadecimalTests {
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
