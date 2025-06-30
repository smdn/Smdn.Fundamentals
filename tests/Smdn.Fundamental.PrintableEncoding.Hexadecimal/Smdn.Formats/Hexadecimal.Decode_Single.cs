// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn.Formats;

#pragma warning disable IDE0040
partial class HexadecimalTests {
#pragma warning restore IDE0040
  [Test]
  public void TryDecode_OfDataArraySegment_DataTooShort()
  {
    Assert.That(Hexadecimal.TryDecode(new ArraySegment<byte>(new byte[0]), out _), Is.False);
    Assert.That(Hexadecimal.TryDecode(new ArraySegment<byte>(new byte[1]), out _), Is.False);

    Assert.That(Hexadecimal.TryDecodeUpperCase(new ArraySegment<byte>(new byte[0]), out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeUpperCase(new ArraySegment<byte>(new byte[1]), out _), Is.False);

    Assert.That(Hexadecimal.TryDecodeLowerCase(new ArraySegment<byte>(new byte[0]), out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase(new ArraySegment<byte>(new byte[1]), out _), Is.False);

    Assert.That(Hexadecimal.TryDecode(new ArraySegment<char>(new char[0]), out _), Is.False);
    Assert.That(Hexadecimal.TryDecode(new ArraySegment<char>(new char[1]), out _), Is.False);

    Assert.That(Hexadecimal.TryDecodeUpperCase(new ArraySegment<char>(new char[0]), out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeUpperCase(new ArraySegment<char>(new char[1]), out _), Is.False);

    Assert.That(Hexadecimal.TryDecodeLowerCase(new ArraySegment<char>(new char[0]), out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase(new ArraySegment<char>(new char[1]), out _), Is.False);
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void TryDecode_OfDataSpan_DataTooShort()
  {
    Assert.That(Hexadecimal.TryDecode(new byte[0], out _), Is.False);
    Assert.That(Hexadecimal.TryDecode(new byte[1], out _), Is.False);

    Assert.That(Hexadecimal.TryDecodeUpperCase(new byte[0], out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeUpperCase(new byte[1], out _), Is.False);

    Assert.That(Hexadecimal.TryDecodeLowerCase(new byte[0], out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase(new byte[1], out _), Is.False);

    Assert.That(Hexadecimal.TryDecode(new char[0], out _), Is.False);
    Assert.That(Hexadecimal.TryDecode(new char[1], out _), Is.False);

    Assert.That(Hexadecimal.TryDecodeUpperCase(new char[0], out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeUpperCase(new char[1], out _), Is.False);

    Assert.That(Hexadecimal.TryDecodeLowerCase(new char[0], out _), Is.False);
    Assert.That(Hexadecimal.TryDecodeLowerCase(new char[1], out _), Is.False);
  }
#endif

  [TestCase(0x30, 0x31, 0x01)]
  [TestCase(0x32, 0x33, 0x23)]
  [TestCase(0x34, 0x35, 0x45)]
  [TestCase(0x36, 0x37, 0x67)]
  [TestCase(0x38, 0x39, 0x89)]
  [TestCase(0x41, 0x42, 0xAB)]
  [TestCase(0x43, 0x44, 0xCD)]
  [TestCase(0x45, 0x46, 0xEF)]
  public void TryDecode_OfByteArraySegment_UpperCase(byte high, byte low, byte expected)
  {
    Assert.That(Hexadecimal.TryDecode(new ArraySegment<byte>(new[] { high, low }), out var decoded0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decoded0, Is.EqualTo(expected), nameof(decoded0));

    Assert.That(Hexadecimal.TryDecodeUpperCase(new ArraySegment<byte>(new[] { high, low }), out var decoded1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decoded1, Is.EqualTo(expected), nameof(decoded1));
  }

#if SYSTEM_READONLYSPAN
  [TestCase(0x30, 0x31, 0x01)]
  [TestCase(0x32, 0x33, 0x23)]
  [TestCase(0x34, 0x35, 0x45)]
  [TestCase(0x36, 0x37, 0x67)]
  [TestCase(0x38, 0x39, 0x89)]
  [TestCase(0x41, 0x42, 0xAB)]
  [TestCase(0x43, 0x44, 0xCD)]
  [TestCase(0x45, 0x46, 0xEF)]
  public void TryDecode_OfByteSpan_UpperCase(byte high, byte low, byte expected)
  {
    Assert.That(Hexadecimal.TryDecode(new[] { high, low }, out var decoded0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decoded0, Is.EqualTo(expected), nameof(decoded0));

    Assert.That(Hexadecimal.TryDecodeUpperCase(new[] { high, low }, out var decoded1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decoded1, Is.EqualTo(expected), nameof(decoded1));
  }
#endif

  [TestCase('0', '1', 0x01)]
  [TestCase('2', '3', 0x23)]
  [TestCase('4', '5', 0x45)]
  [TestCase('6', '7', 0x67)]
  [TestCase('8', '9', 0x89)]
  [TestCase('A', 'B', 0xAB)]
  [TestCase('C', 'D', 0xCD)]
  [TestCase('E', 'F', 0xEF)]
  public void TryDecode_OfCharArraySegment_UpperCase(char high, char low, byte expected)
  {
    Assert.That(Hexadecimal.TryDecode(new ArraySegment<char>(new[] { high, low }), out var decoded0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decoded0, Is.EqualTo(expected), nameof(decoded0));

    Assert.That(Hexadecimal.TryDecodeUpperCase(new ArraySegment<char>(new[] { high, low }), out var decoded1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decoded1, Is.EqualTo(expected), nameof(decoded1));
  }

#if SYSTEM_READONLYSPAN
  [TestCase('0', '1', 0x01)]
  [TestCase('2', '3', 0x23)]
  [TestCase('4', '5', 0x45)]
  [TestCase('6', '7', 0x67)]
  [TestCase('8', '9', 0x89)]
  [TestCase('A', 'B', 0xAB)]
  [TestCase('C', 'D', 0xCD)]
  [TestCase('E', 'F', 0xEF)]
  public void TryDecode_OfCharSpan_UpperCase(char high, char low, byte expected)
  {
    Assert.That(Hexadecimal.TryDecode(new[] { high, low }, out var decoded0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decoded0, Is.EqualTo(expected), nameof(decoded0));

    Assert.That(Hexadecimal.TryDecodeUpperCase(new[] { high, low }, out var decoded1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decoded1, Is.EqualTo(expected), nameof(decoded1));
  }
#endif

  [TestCase(0x30, 0x31, 0x01)]
  [TestCase(0x32, 0x33, 0x23)]
  [TestCase(0x34, 0x35, 0x45)]
  [TestCase(0x36, 0x37, 0x67)]
  [TestCase(0x38, 0x39, 0x89)]
  [TestCase(0x61, 0x62, 0xAB)]
  [TestCase(0x63, 0x64, 0xCD)]
  [TestCase(0x65, 0x66, 0xEF)]
  public void TryDecode_OfByteArraySegment_LowerCase(byte high, byte low, byte expected)
  {
    Assert.That(Hexadecimal.TryDecode(new ArraySegment<byte>(new[] { high, low }), out var decoded0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decoded0, Is.EqualTo(expected), nameof(decoded0));

    Assert.That(Hexadecimal.TryDecodeLowerCase(new ArraySegment<byte>(new[] { high, low }), out var decoded1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decoded1, Is.EqualTo(expected), nameof(decoded1));
  }

#if SYSTEM_READONLYSPAN
  [TestCase(0x30, 0x31, 0x01)]
  [TestCase(0x32, 0x33, 0x23)]
  [TestCase(0x34, 0x35, 0x45)]
  [TestCase(0x36, 0x37, 0x67)]
  [TestCase(0x38, 0x39, 0x89)]
  [TestCase(0x61, 0x62, 0xAB)]
  [TestCase(0x63, 0x64, 0xCD)]
  [TestCase(0x65, 0x66, 0xEF)]
  public void TryDecode_OfByteSpan_LowerCase(byte high, byte low, byte expected)
  {
    Assert.That(Hexadecimal.TryDecode(new[] { high, low }, out var decoded0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decoded0, Is.EqualTo(expected), nameof(decoded0));

    Assert.That(Hexadecimal.TryDecodeLowerCase(new[] { high, low }, out var decoded1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decoded1, Is.EqualTo(expected), nameof(decoded1));
  }
#endif

  [TestCase('0', '1', 0x01)]
  [TestCase('2', '3', 0x23)]
  [TestCase('4', '5', 0x45)]
  [TestCase('6', '7', 0x67)]
  [TestCase('8', '9', 0x89)]
  [TestCase('a', 'b', 0xAB)]
  [TestCase('c', 'd', 0xCD)]
  [TestCase('e', 'f', 0xEF)]
  public void TryDecode_OfCharArraySegment_LowerCase(char high, char low, byte expected)
  {
    Assert.That(Hexadecimal.TryDecode(new ArraySegment<char>(new[] { high, low }), out var decoded0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decoded0, Is.EqualTo(expected), nameof(decoded0));

    Assert.That(Hexadecimal.TryDecodeLowerCase(new ArraySegment<char>(new[] { high, low }), out var decoded1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decoded1, Is.EqualTo(expected), nameof(decoded1));
  }

#if SYSTEM_READONLYSPAN
  [TestCase('0', '1', 0x01)]
  [TestCase('2', '3', 0x23)]
  [TestCase('4', '5', 0x45)]
  [TestCase('6', '7', 0x67)]
  [TestCase('8', '9', 0x89)]
  [TestCase('a', 'b', 0xAB)]
  [TestCase('c', 'd', 0xCD)]
  [TestCase('e', 'f', 0xEF)]
  public void TryDecode_OfCharSpan_LowerCase(char high, char low, byte expected)
  {
    Assert.That(Hexadecimal.TryDecode(new[] { high, low }, out var decoded0), Is.True, nameof(Hexadecimal.TryDecode));
    Assert.That(decoded0, Is.EqualTo(expected), nameof(decoded0));

    Assert.That(Hexadecimal.TryDecodeLowerCase(new[] { high, low }, out var decoded1), Is.True, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(decoded1, Is.EqualTo(expected), nameof(decoded1));
  }
#endif

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
  public void TryDecode_OfByteArraySegment_InvalidOctet(byte high, byte low)
  {
    Assert.That(Hexadecimal.TryDecode(new ArraySegment<byte>(new[] { high, low }), out _), Is.False, nameof(Hexadecimal.TryDecode));
    Assert.That(Hexadecimal.TryDecodeUpperCase(new ArraySegment<byte>(new[] { high, low }), out _), Is.False, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(Hexadecimal.TryDecodeLowerCase(new ArraySegment<byte>(new[] { high, low }), out _), Is.False, nameof(Hexadecimal.TryDecodeLowerCase));
  }

#if SYSTEM_READONLYSPAN
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
  public void TryDecode_OfByteSpan_InvalidOctet(byte high, byte low)
  {
    Assert.That(Hexadecimal.TryDecode(new[] { high, low }, out _), Is.False, nameof(Hexadecimal.TryDecode));
    Assert.That(Hexadecimal.TryDecodeUpperCase(new[] { high, low }, out _), Is.False, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(Hexadecimal.TryDecodeLowerCase(new[] { high, low }, out _), Is.False, nameof(Hexadecimal.TryDecodeLowerCase));
  }
#endif

  [TestCase('\0', '\0')]
  [TestCase('\0', (char)('0' - 1))]
  [TestCase((char)('0' - 1), '\0')]
  [TestCase('\0', (char)('9' + 1))]
  [TestCase((char)('9' + 1), '\0')]
  [TestCase('\0', (char)('A' - 1))]
  [TestCase((char)('A' - 1), '\0')]
  [TestCase('\0', (char)('F' + 1))]
  [TestCase((char)('F' + 1), '\0')]
  [TestCase('\0', (char)('a' - 1))]
  [TestCase((char)('a' - 1), '\0')]
  [TestCase('\0', (char)('f' + 1))]
  [TestCase((char)('f' + 1), '\0')]
  public void TryDecode_OfCharArraySegment_InvalidOctet(char high, char low)
  {
    Assert.That(Hexadecimal.TryDecode(new ArraySegment<char>(new[] { high, low }), out _), Is.False, nameof(Hexadecimal.TryDecode));
    Assert.That(Hexadecimal.TryDecodeUpperCase(new ArraySegment<char>(new[] { high, low }), out _), Is.False, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(Hexadecimal.TryDecodeLowerCase(new ArraySegment<char>(new[] { high, low }), out _), Is.False, nameof(Hexadecimal.TryDecodeLowerCase));
  }

#if SYSTEM_READONLYSPAN
  [TestCase('\0', '\0')]
  [TestCase('\0', (char)('0' - 1))]
  [TestCase((char)('0' - 1), '\0')]
  [TestCase('\0', (char)('9' + 1))]
  [TestCase((char)('9' + 1), '\0')]
  [TestCase('\0', (char)('A' - 1))]
  [TestCase((char)('A' - 1), '\0')]
  [TestCase('\0', (char)('F' + 1))]
  [TestCase((char)('F' + 1), '\0')]
  [TestCase('\0', (char)('a' - 1))]
  [TestCase((char)('a' - 1), '\0')]
  [TestCase('\0', (char)('f' + 1))]
  [TestCase((char)('f' + 1), '\0')]
  public void TryDecode_OfCharSpan_InvalidOctet(char high, char low)
  {
    Assert.That(Hexadecimal.TryDecode(new[] { high, low }, out _), Is.False, nameof(Hexadecimal.TryDecode));
    Assert.That(Hexadecimal.TryDecodeUpperCase(new[] { high, low }, out _), Is.False, nameof(Hexadecimal.TryDecodeUpperCase));
    Assert.That(Hexadecimal.TryDecodeLowerCase(new[] { high, low }, out _), Is.False, nameof(Hexadecimal.TryDecodeLowerCase));
  }
#endif

  [TestCase(0x00, (byte)'a')]
  [TestCase((byte)'a', 0x00)]
  [TestCase(0x00, (byte)'f')]
  [TestCase((byte)'f', 0x00)]
  public void TryDecodeUpperCase_OfByteArraySegment_InvalidOctet(byte high, byte low)
  {
    Assert.That(Hexadecimal.TryDecodeUpperCase(new ArraySegment<byte>(new[] { high, low }), out _), Is.False, nameof(Hexadecimal.TryDecodeUpperCase));
  }

#if SYSTEM_READONLYSPAN
  [TestCase(0x00, (byte)'a')]
  [TestCase((byte)'a', 0x00)]
  [TestCase(0x00, (byte)'f')]
  [TestCase((byte)'f', 0x00)]
  public void TryDecodeUpperCase_OfByteSpan_InvalidOctet(byte high, byte low)
  {
    Assert.That(Hexadecimal.TryDecodeUpperCase(new[] { high, low }, out _), Is.False, nameof(Hexadecimal.TryDecodeUpperCase));
  }
#endif

  [TestCase('\0', 'a')]
  [TestCase('a', '\0')]
  [TestCase('\0', 'f')]
  [TestCase('f', '\0')]
  public void TryDecodeUpperCase_OfCharArraySegment_InvalidOctet(char high, char low)
  {
    Assert.That(Hexadecimal.TryDecodeUpperCase(new ArraySegment<char>(new[] { high, low }), out _), Is.False, nameof(Hexadecimal.TryDecodeUpperCase));
  }

#if SYSTEM_READONLYSPAN
  [TestCase('\0', 'a')]
  [TestCase('a', '\0')]
  [TestCase('\0', 'f')]
  [TestCase('f', '\0')]
  public void TryDecodeUpperCase_OfCharSpan_InvalidOctet(char high, char low)
  {
    Assert.That(Hexadecimal.TryDecodeUpperCase(new[] { high, low }, out _), Is.False, nameof(Hexadecimal.TryDecodeUpperCase));
  }
#endif

  [TestCase(0x00, (byte)'A')]
  [TestCase((byte)'A', 0x00)]
  [TestCase(0x00, (byte)'F')]
  [TestCase((byte)'F', 0x00)]
  public void TryDecodeLowerCase_OfByteArraySegment_InvalidOctet(byte high, byte low)
  {
    Assert.That(Hexadecimal.TryDecodeLowerCase(new ArraySegment<byte>(new[] { high, low }), out _), Is.False, nameof(Hexadecimal.TryDecodeLowerCase));
  }

#if SYSTEM_READONLYSPAN
  [TestCase(0x00, (byte)'A')]
  [TestCase((byte)'A', 0x00)]
  [TestCase(0x00, (byte)'F')]
  [TestCase((byte)'F', 0x00)]
  public void TryDecodeLowerCase_OfByteSpan_InvalidOctet(byte high, byte low)
  {
    Assert.That(Hexadecimal.TryDecodeLowerCase(new[] { high, low }, out _), Is.False, nameof(Hexadecimal.TryDecodeLowerCase));
  }
#endif

  [TestCase('\0', 'A')]
  [TestCase('A', '\0')]
  [TestCase('\0', 'F')]
  [TestCase('F', '\0')]
  public void TryDecodeLowerCase_OfCharArraySegment_InvalidOctet(char high, char low)
  {
    Assert.That(Hexadecimal.TryDecodeLowerCase(new ArraySegment<char>(new[] { high, low }), out _), Is.False, nameof(Hexadecimal.TryDecodeLowerCase));
  }

#if SYSTEM_READONLYSPAN
  [TestCase('\0', 'A')]
  [TestCase('A', '\0')]
  [TestCase('\0', 'F')]
  [TestCase('F', '\0')]
  public void TryDecodeLowerCase_OfCharSpan_InvalidOctet(char high, char low)
  {
    Assert.That(Hexadecimal.TryDecodeLowerCase(new[] { high, low }, out _), Is.False, nameof(Hexadecimal.TryDecodeLowerCase));
  }
#endif

  [TestCase((byte)'0', true, 0x0)]
  [TestCase((byte)'1', true, 0x1)]
  [TestCase((byte)'2', true, 0x2)]
  [TestCase((byte)'3', true, 0x3)]
  [TestCase((byte)'4', true, 0x4)]
  [TestCase((byte)'5', true, 0x5)]
  [TestCase((byte)'6', true, 0x6)]
  [TestCase((byte)'7', true, 0x7)]
  [TestCase((byte)'8', true, 0x8)]
  [TestCase((byte)'9', true, 0x9)]
  [TestCase((byte)'A', true, 0xA)]
  [TestCase((byte)'B', true, 0xB)]
  [TestCase((byte)'C', true, 0xC)]
  [TestCase((byte)'D', true, 0xD)]
  [TestCase((byte)'E', true, 0xE)]
  [TestCase((byte)'F', true, 0xF)]
  [TestCase((byte)('0' - 1), false, 0x0)]
  [TestCase((byte)('9' + 1), false, 0x0)]
  [TestCase((byte)('A' - 1), false, 0x0)]
  [TestCase((byte)('F' + 1), false, 0x0)]
  [TestCase((byte)'a', false, 0x0)]
  [TestCase((byte)'f', false, 0x0)]
  public void TryDecodeUpperCaseValue_OfByte(byte data, bool canDecode, byte expectedDecodedValue)
  {
    Assert.That(Hexadecimal.TryDecodeUpperCaseValue(data, out var decodedValue), Is.EqualTo(canDecode), nameof(canDecode));

    if (canDecode)
      Assert.That(decodedValue, Is.EqualTo(expectedDecodedValue), nameof(decodedValue));
  }

  [TestCase('0', true, 0x0)]
  [TestCase('1', true, 0x1)]
  [TestCase('2', true, 0x2)]
  [TestCase('3', true, 0x3)]
  [TestCase('4', true, 0x4)]
  [TestCase('5', true, 0x5)]
  [TestCase('6', true, 0x6)]
  [TestCase('7', true, 0x7)]
  [TestCase('8', true, 0x8)]
  [TestCase('9', true, 0x9)]
  [TestCase('A', true, 0xA)]
  [TestCase('B', true, 0xB)]
  [TestCase('C', true, 0xC)]
  [TestCase('D', true, 0xD)]
  [TestCase('E', true, 0xE)]
  [TestCase('F', true, 0xF)]
  [TestCase((char)('0' - 1), false, 0x0)]
  [TestCase((char)('9' + 1), false, 0x0)]
  [TestCase((char)('A' - 1), false, 0x0)]
  [TestCase((char)('F' + 1), false, 0x0)]
  [TestCase('a', false, 0x0)]
  [TestCase('f', false, 0x0)]
  public void TryDecodeUpperCaseValue_OfChar(char data, bool canDecode, byte expectedDecodedValue)
  {
    Assert.That(Hexadecimal.TryDecodeUpperCaseValue(data, out var decodedValue), Is.EqualTo(canDecode), nameof(canDecode));

    if (canDecode)
      Assert.That(decodedValue, Is.EqualTo(expectedDecodedValue), nameof(decodedValue));
  }

  [TestCase((byte)'0', true, 0x0)]
  [TestCase((byte)'1', true, 0x1)]
  [TestCase((byte)'2', true, 0x2)]
  [TestCase((byte)'3', true, 0x3)]
  [TestCase((byte)'4', true, 0x4)]
  [TestCase((byte)'5', true, 0x5)]
  [TestCase((byte)'6', true, 0x6)]
  [TestCase((byte)'7', true, 0x7)]
  [TestCase((byte)'8', true, 0x8)]
  [TestCase((byte)'9', true, 0x9)]
  [TestCase((byte)'a', true, 0xA)]
  [TestCase((byte)'b', true, 0xB)]
  [TestCase((byte)'c', true, 0xC)]
  [TestCase((byte)'d', true, 0xD)]
  [TestCase((byte)'e', true, 0xE)]
  [TestCase((byte)'f', true, 0xF)]
  [TestCase((byte)('0' - 1), false, 0x0)]
  [TestCase((byte)('9' + 1), false, 0x0)]
  [TestCase((byte)('a' - 1), false, 0x0)]
  [TestCase((byte)('f' + 1), false, 0x0)]
  [TestCase((byte)'A', false, 0x0)]
  [TestCase((byte)'F', false, 0x0)]
  public void TryDecodeLowerCaseValue_OfByte(byte data, bool canDecode, byte expectedDecodedValue)
  {
    Assert.That(Hexadecimal.TryDecodeLowerCaseValue(data, out var decodedValue), Is.EqualTo(canDecode), nameof(canDecode));

    if (canDecode)
      Assert.That(decodedValue, Is.EqualTo(expectedDecodedValue), nameof(decodedValue));
  }

  [TestCase('0', true, 0x0)]
  [TestCase('1', true, 0x1)]
  [TestCase('2', true, 0x2)]
  [TestCase('3', true, 0x3)]
  [TestCase('4', true, 0x4)]
  [TestCase('5', true, 0x5)]
  [TestCase('6', true, 0x6)]
  [TestCase('7', true, 0x7)]
  [TestCase('8', true, 0x8)]
  [TestCase('9', true, 0x9)]
  [TestCase('a', true, 0xA)]
  [TestCase('b', true, 0xB)]
  [TestCase('c', true, 0xC)]
  [TestCase('d', true, 0xD)]
  [TestCase('e', true, 0xE)]
  [TestCase('f', true, 0xF)]
  [TestCase((char)('0' - 1), false, 0x0)]
  [TestCase((char)('9' + 1), false, 0x0)]
  [TestCase((char)('a' - 1), false, 0x0)]
  [TestCase((char)('f' + 1), false, 0x0)]
  [TestCase('A', false, 0x0)]
  [TestCase('F', false, 0x0)]
  public void TryDecodeLowerCaseValue_OfChar(char data, bool canDecode, byte expectedDecodedValue)
  {
    Assert.That(Hexadecimal.TryDecodeLowerCaseValue(data, out var decodedValue), Is.EqualTo(canDecode), nameof(canDecode));

    if (canDecode)
      Assert.That(decodedValue, Is.EqualTo(expectedDecodedValue), nameof(decodedValue));
  }

  [TestCase((byte)'0', true, 0x0)]
  [TestCase((byte)'1', true, 0x1)]
  [TestCase((byte)'2', true, 0x2)]
  [TestCase((byte)'3', true, 0x3)]
  [TestCase((byte)'4', true, 0x4)]
  [TestCase((byte)'5', true, 0x5)]
  [TestCase((byte)'6', true, 0x6)]
  [TestCase((byte)'7', true, 0x7)]
  [TestCase((byte)'8', true, 0x8)]
  [TestCase((byte)'9', true, 0x9)]
  [TestCase((byte)'a', true, 0xA)]
  [TestCase((byte)'b', true, 0xB)]
  [TestCase((byte)'c', true, 0xC)]
  [TestCase((byte)'d', true, 0xD)]
  [TestCase((byte)'e', true, 0xE)]
  [TestCase((byte)'f', true, 0xF)]
  [TestCase((byte)'A', true, 0xA)]
  [TestCase((byte)'B', true, 0xB)]
  [TestCase((byte)'C', true, 0xC)]
  [TestCase((byte)'D', true, 0xD)]
  [TestCase((byte)'E', true, 0xE)]
  [TestCase((byte)'F', true, 0xF)]
  [TestCase((byte)('0' - 1), false, 0x0)]
  [TestCase((byte)('9' + 1), false, 0x0)]
  [TestCase((byte)('a' - 1), false, 0x0)]
  [TestCase((byte)('f' + 1), false, 0x0)]
  [TestCase((byte)('A' - 1), false, 0x0)]
  [TestCase((byte)('F' + 1), false, 0x0)]
  public void TryDecodeValue_OfByte(byte data, bool canDecode, byte expectedDecodedValue)
  {
    Assert.That(Hexadecimal.TryDecodeValue(data, out var decodedValue), Is.EqualTo(canDecode), nameof(canDecode));

    if (canDecode)
      Assert.That(decodedValue, Is.EqualTo(expectedDecodedValue), nameof(decodedValue));
  }

  [TestCase('0', true, 0x0)]
  [TestCase('1', true, 0x1)]
  [TestCase('2', true, 0x2)]
  [TestCase('3', true, 0x3)]
  [TestCase('4', true, 0x4)]
  [TestCase('5', true, 0x5)]
  [TestCase('6', true, 0x6)]
  [TestCase('7', true, 0x7)]
  [TestCase('8', true, 0x8)]
  [TestCase('9', true, 0x9)]
  [TestCase('a', true, 0xA)]
  [TestCase('b', true, 0xB)]
  [TestCase('c', true, 0xC)]
  [TestCase('d', true, 0xD)]
  [TestCase('e', true, 0xE)]
  [TestCase('f', true, 0xF)]
  [TestCase('A', true, 0xA)]
  [TestCase('B', true, 0xB)]
  [TestCase('C', true, 0xC)]
  [TestCase('D', true, 0xD)]
  [TestCase('E', true, 0xE)]
  [TestCase('F', true, 0xF)]
  [TestCase((char)('0' - 1), false, 0x0)]
  [TestCase((char)('9' + 1), false, 0x0)]
  [TestCase((char)('a' - 1), false, 0x0)]
  [TestCase((char)('f' + 1), false, 0x0)]
  [TestCase((char)('A' - 1), false, 0x0)]
  [TestCase((char)('F' + 1), false, 0x0)]
  public void TryDecodeValue_OfChar(char data, bool canDecode, byte expectedDecodedValue)
  {
    Assert.That(Hexadecimal.TryDecodeValue(data, out var decodedValue), Is.EqualTo(canDecode), nameof(canDecode));

    if (canDecode)
      Assert.That(decodedValue, Is.EqualTo(expectedDecodedValue), nameof(decodedValue));
  }
}
