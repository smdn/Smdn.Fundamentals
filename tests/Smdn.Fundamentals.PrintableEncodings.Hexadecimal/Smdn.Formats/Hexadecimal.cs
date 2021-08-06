// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;
using Smdn.Test.NUnit;

namespace Smdn.Formats {
  [TestFixture]
  public class HexadecimalTests {
    [Test]
    public void TryEncodeUpperCase_ToByteSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[1], out _));
    }

    [Test]
    public void TryEncodeLowerCase_ToByteSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[1], out _));
    }

    [Test]
    public void TryEncodeUpperCase_ToCharSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[1], out _));
    }

    [Test]
    public void TryEncodeLowerCase_ToCharSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[1], out _));
    }

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

    [TestCase(0x01, '0', '1')]
    [TestCase(0x23, '2', '3')]
    [TestCase(0x45, '4', '5')]
    [TestCase(0x67, '6', '7')]
    [TestCase(0x89, '8', '9')]
    [TestCase(0xAB, 'A', 'B')]
    [TestCase(0xCD, 'C', 'D')]
    [TestCase(0xEF, 'E', 'F')]
    public void TryEncodeUpperCase(byte data, char expectedHigh, char expectedLow)
    {
      var bytes = new byte[2];

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(data, bytes, out var bytesEncoded), $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<byte>");
      Assert.AreEqual(bytesEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(bytes[0], (byte)expectedHigh, "bytes[0]");
      Assert.AreEqual(bytes[1], (byte)expectedLow, "bytes[1]");

      var chars = new char[2];

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(data, chars, out var charsEncoded), $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<char>");
      Assert.AreEqual(charsEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(chars[0], expectedHigh, "chars[0]");
      Assert.AreEqual(chars[1], expectedLow, "chars[1]");
    }

    [TestCase(0x01, '0', '1')]
    [TestCase(0x23, '2', '3')]
    [TestCase(0x45, '4', '5')]
    [TestCase(0x67, '6', '7')]
    [TestCase(0x89, '8', '9')]
    [TestCase(0xAB, 'a', 'b')]
    [TestCase(0xCD, 'c', 'd')]
    [TestCase(0xEF, 'e', 'f')]
    public void TryEncodeLowerCase(byte data, char expectedHigh, char expectedLow)
    {
      var bytes = new byte[2];

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(data, bytes, out var bytesEncoded), $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<byte>");
      Assert.AreEqual(bytesEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(bytes[0], (byte)expectedHigh, "bytes[0]");
      Assert.AreEqual(bytes[1], (byte)expectedLow, "bytes[1]");

      var chars = new char[2];

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(data, chars, out var charsEncoded), $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<char>");
      Assert.AreEqual(charsEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(chars[0], expectedHigh, "chars[0]");
      Assert.AreEqual(chars[1], expectedLow, "chars[1]");
    }

    [Test]
    public void TryDecode_DataTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryDecode(new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryDecode(new byte[1], out _));

      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new byte[1], out _));

      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new byte[1], out _));
    }

    [TestCase(0x30, 0x31, 0x01)]
    [TestCase(0x32, 0x33, 0x23)]
    [TestCase(0x34, 0x35, 0x45)]
    [TestCase(0x36, 0x37, 0x67)]
    [TestCase(0x38, 0x39, 0x89)]
    [TestCase(0x41, 0x42, 0xAB)]
    [TestCase(0x43, 0x44, 0xCD)]
    [TestCase(0x45, 0x46, 0xEF)]
    public void TryDecode_UpperCase(byte high, byte low, byte expected)
    {
      Assert.IsTrue(Hexadecimal.TryDecode(new byte[] {high, low}, out var decoded0), nameof(Hexadecimal.TryDecode));
      Assert.AreEqual(decoded0, expected, nameof(decoded0));

      Assert.IsTrue(Hexadecimal.TryDecodeUpperCase(new byte[] {high, low}, out var decoded1), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.AreEqual(decoded1, expected, nameof(decoded1));
    }

    [TestCase(0x30, 0x31, 0x01)]
    [TestCase(0x32, 0x33, 0x23)]
    [TestCase(0x34, 0x35, 0x45)]
    [TestCase(0x36, 0x37, 0x67)]
    [TestCase(0x38, 0x39, 0x89)]
    [TestCase(0x61, 0x62, 0xAB)]
    [TestCase(0x63, 0x64, 0xCD)]
    [TestCase(0x65, 0x66, 0xEF)]
    public void TryDecode_LowerCase(byte high, byte low, byte expected)
    {
      Assert.IsTrue(Hexadecimal.TryDecode(new byte[] {high, low}, out var decoded0), nameof(Hexadecimal.TryDecode));
      Assert.AreEqual(decoded0, expected, nameof(decoded0));

      Assert.IsTrue(Hexadecimal.TryDecodeLowerCase(new byte[] {high, low}, out var decoded1), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.AreEqual(decoded1, expected, nameof(decoded1));
    }

    [TestCase(0x00, 0x00)]
    [TestCase(0x00, (byte)('0' - 1))]
    [TestCase((byte)('0' - 1), 0x00)]
    [TestCase(0x00, (byte)('9' + 1))]
    [TestCase((byte)('9' + 1), 0x00)]
    [TestCase(0x00, (byte)('A' - 1))]
    [TestCase((byte)('A' - 1), 0x00)]
    [TestCase(0x00, (byte)('F' + 1))]
    [TestCase((byte)('F' + 1), 0x00)]
    [TestCase(0x00, (byte)('a' - 1))]
    [TestCase((byte)('a' - 1), 0x00)]
    [TestCase(0x00, (byte)('f' + 1))]
    [TestCase((byte)('f' + 1), 0x00)]
    public void TryDecode_InvalidOctet(byte high, byte low)
    {
      Assert.IsFalse(Hexadecimal.TryDecode(new byte[] {high, low}, out _), nameof(Hexadecimal.TryDecode));
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new byte[] {high, low}, out _), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new byte[] {high, low}, out _), nameof(Hexadecimal.TryDecodeLowerCase));
    }

    [TestCase(0x00, (byte)('a'))]
    [TestCase((byte)('a'), 0x00)]
    [TestCase(0x00, (byte)('f'))]
    [TestCase((byte)('f'), 0x00)]
    public void TryDecodeUpperCase_InvalidOctet(byte high, byte low)
    {
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new byte[] {high, low}, out _), nameof(Hexadecimal.TryDecodeUpperCase));
    }

    [TestCase(0x00, (byte)('A'))]
    [TestCase((byte)('A'), 0x00)]
    [TestCase(0x00, (byte)('F'))]
    [TestCase((byte)('F'), 0x00)]
    public void TryDecodeLowerCase_InvalidOctet(byte high, byte low)
    {
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new byte[] {high, low}, out _), nameof(Hexadecimal.TryDecodeLowerCase));
    }
  }
}
