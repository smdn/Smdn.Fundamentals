// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Formats;

partial class HexadecimalTests {
#if SYSTEM_READONLYSPAN
  [Test]
  public void ToUpperCaseString_OfReadOnlySpan()
  {
    Assert.That(Hexadecimal.ToUpperCaseString(new byte[0]), Is.EqualTo(string.Empty));
    Assert.That(Hexadecimal.ToUpperCaseString(new byte[] { 0x00 }), Is.EqualTo("00"));
    Assert.That(Hexadecimal.ToUpperCaseString(new byte[] { 0xFF }), Is.EqualTo("FF"));
    Assert.That(Hexadecimal.ToUpperCaseString(new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF }), Is.EqualTo("0123456789ABCDEF"));
  }
#endif

  [Test]
  public void ToUpperCaseString_OfArraySegment()
  {
    Assert.That(Hexadecimal.ToUpperCaseString(CreateEmptyArraySegment<byte>()), Is.EqualTo(string.Empty));
    Assert.That(Hexadecimal.ToUpperCaseString(new ArraySegment<byte>(new byte[] { 0x00 })), Is.EqualTo("00"));
    Assert.That(Hexadecimal.ToUpperCaseString(new ArraySegment<byte>(new byte[] { 0xFF })), Is.EqualTo("FF"));
    Assert.That(Hexadecimal.ToUpperCaseString(new ArraySegment<byte>(new byte[] { 0xFF, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xFF }, 1, 8)), Is.EqualTo("0123456789ABCDEF"));
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void ToLowerCaseString_OfReadOnlySpan()
  {
    Assert.That(Hexadecimal.ToLowerCaseString(new byte[0]), Is.EqualTo(string.Empty));
    Assert.That(Hexadecimal.ToLowerCaseString(new byte[] { 0x00 }), Is.EqualTo("00"));
    Assert.That(Hexadecimal.ToLowerCaseString(new byte[] { 0xFF }), Is.EqualTo("ff"));
    Assert.That(Hexadecimal.ToLowerCaseString(new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF }), Is.EqualTo("0123456789abcdef"));
  }
#endif

  [Test]
  public void ToLowerCaseString_OfArraySegment()
  {
    Assert.That(Hexadecimal.ToLowerCaseString(CreateEmptyArraySegment<byte>()), Is.EqualTo(string.Empty));
    Assert.That(Hexadecimal.ToLowerCaseString(new ArraySegment<byte>(new byte[] { 0x00 })), Is.EqualTo("00"));
    Assert.That(Hexadecimal.ToLowerCaseString(new ArraySegment<byte>(new byte[] { 0xFF })), Is.EqualTo("ff"));
    Assert.That(Hexadecimal.ToLowerCaseString(new ArraySegment<byte>(new byte[] { 0xFF, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xFF }, 1, 8)), Is.EqualTo("0123456789abcdef"));
  }
}
