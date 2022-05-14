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
    Assert.AreEqual(string.Empty, Hexadecimal.ToUpperCaseString(new byte[0]));
    Assert.AreEqual("00", Hexadecimal.ToUpperCaseString(new byte[] { 0x00 }));
    Assert.AreEqual("FF", Hexadecimal.ToUpperCaseString(new byte[] { 0xFF }));
    Assert.AreEqual("0123456789ABCDEF", Hexadecimal.ToUpperCaseString(new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF }));
  }
#endif

  [Test]
  public void ToUpperCaseString_OfArraySegment()
  {
    Assert.AreEqual(string.Empty, Hexadecimal.ToUpperCaseString(ArraySegment<byte>.Empty));
    Assert.AreEqual("00", Hexadecimal.ToUpperCaseString(new ArraySegment<byte>(new byte[] { 0x00 })));
    Assert.AreEqual("FF", Hexadecimal.ToUpperCaseString(new ArraySegment<byte>(new byte[] { 0xFF })));
    Assert.AreEqual("0123456789ABCDEF", Hexadecimal.ToUpperCaseString(new ArraySegment<byte>(new byte[] { 0xFF, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xFF }, 1, 8)));
  }

#if SYSTEM_READONLYSPAN
  [Test]
  public void ToLowerCaseString_OfReadOnlySpan()
  {
    Assert.AreEqual(string.Empty, Hexadecimal.ToLowerCaseString(new byte[0]));
    Assert.AreEqual("00", Hexadecimal.ToLowerCaseString(new byte[] { 0x00 }));
    Assert.AreEqual("ff", Hexadecimal.ToLowerCaseString(new byte[] { 0xFF }));
    Assert.AreEqual("0123456789abcdef", Hexadecimal.ToLowerCaseString(new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF }));
  }
#endif

  [Test]
  public void ToLowerCaseString_OfArraySegment()
  {
    Assert.AreEqual(string.Empty, Hexadecimal.ToLowerCaseString(ArraySegment<byte>.Empty));
    Assert.AreEqual("00", Hexadecimal.ToLowerCaseString(new ArraySegment<byte>(new byte[] { 0x00 })));
    Assert.AreEqual("ff", Hexadecimal.ToLowerCaseString(new ArraySegment<byte>(new byte[] { 0xFF })));
    Assert.AreEqual("0123456789abcdef", Hexadecimal.ToLowerCaseString(new ArraySegment<byte>(new byte[] { 0xFF, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xFF }, 1, 8)));
  }
}
