// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void TestOpLeftShift()
  {
    Assert.That(UInt24.Zero << 0, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0000u), "0 << 0");
    Assert.That(UInt24.Zero << 1, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0000u), "0 << 1");

    Assert.That(UInt24.One << 0, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "1 << 0");
    Assert.That(UInt24.One << 1, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0010u), "1 << 1");
    Assert.That(UInt24.One << 23, Is.EqualTo((UInt24)0b_1000_0000_0000_0000_0000_0000u), "1 << 23");
    Assert.That(UInt24.One << 24, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "1 << 24");

    Assert.That(UInt24.MaxValue << 0, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max << 0");

    Assert.That(UInt24.MaxValue << 1, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1110u), "Max << 1");
    Assert.That(UInt24.MaxValue << 23, Is.EqualTo((UInt24)0b_1000_0000_0000_0000_0000_0000u), "Max << 23");
    Assert.That(UInt24.MaxValue << 24, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max << (24 = 0)");
    Assert.That(UInt24.MaxValue << 25, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1110u), "Max << (25 = 1)");
    Assert.That(UInt24.MaxValue << 48, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max << (48 = 0)");
    Assert.That(UInt24.MaxValue << 49, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1110u), "Max << (49 = 1)");

    Assert.That(UInt24.MaxValue << -1, Is.EqualTo((UInt24)0b_1000_0000_0000_0000_0000_0000u), "Max << (-1 = 23)");
    Assert.That(UInt24.MaxValue << -23, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1110u), "Max << (-23 = 1)");
    Assert.That(UInt24.MaxValue << -24, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max << (-24 = 0)");
    Assert.That(UInt24.MaxValue << -25, Is.EqualTo((UInt24)0b_1000_0000_0000_0000_0000_0000u), "Max << (-25 = 23)");
    Assert.That(UInt24.MaxValue << -48, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max << (-48 = 0)");
    Assert.That(UInt24.MaxValue << -49, Is.EqualTo((UInt24)0b_1000_0000_0000_0000_0000_0000u), "Max << (-49 = 23)");
  }

  [Test]
  public void TestOpRightShift()
  {
    Assert.That(UInt24.Zero >> 0, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0000u), "0 >> 0");
    Assert.That(UInt24.Zero >> 1, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0000u), "0 >> 1");

    Assert.That(UInt24.One >> 0, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "1 >> 0");
    Assert.That(UInt24.One >> 1, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0000u), "1 >> 1");
    Assert.That(UInt24.One >> 23, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0000u), "1 >> 23");
    Assert.That(UInt24.One >> 24, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "1 >> 24");

    Assert.That(UInt24.MaxValue >> 0, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max >> 0");

    Assert.That(UInt24.MaxValue >> 1, Is.EqualTo((UInt24)0b_0111_1111_1111_1111_1111_1111u), "Max >> 1");
    Assert.That(UInt24.MaxValue >> 23, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "Max >> 23");
    Assert.That(UInt24.MaxValue >> 24, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max >> (24 = 0)");
    Assert.That(UInt24.MaxValue >> 25, Is.EqualTo((UInt24)0b_0111_1111_1111_1111_1111_1111u), "Max >> (25 = 1)");
    Assert.That(UInt24.MaxValue >> 48, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max >> (48 = 0)");
    Assert.That(UInt24.MaxValue >> 49, Is.EqualTo((UInt24)0b_0111_1111_1111_1111_1111_1111u), "Max >> (49 = 1)");

    Assert.That(UInt24.MaxValue >> -1, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "Max >> (-1 = 23)");
    Assert.That(UInt24.MaxValue >> -23, Is.EqualTo((UInt24)0b_0111_1111_1111_1111_1111_1111u), "Max >> (-23 = 1)");
    Assert.That(UInt24.MaxValue >> -24, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max >> (-24 = 0)");
    Assert.That(UInt24.MaxValue >> -25, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "Max >> (-25 = 23)");
    Assert.That(UInt24.MaxValue >> -48, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max >> (-48 = 0)");
    Assert.That(UInt24.MaxValue >> -49, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "Max >> (-49 = 23)");
  }

  [Test]
  public void TestOpUnsignedRightShift()
  {
    Assert.That(UInt24.Zero >>> 0, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0000u), "0 >>> 0");
    Assert.That(UInt24.Zero >>> 1, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0000u), "0 >>> 1");

    Assert.That(UInt24.One >>> 0, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "1 >>> 0");
    Assert.That(UInt24.One >>> 1, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0000u), "1 >>> 1");
    Assert.That(UInt24.One >>> 23, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0000u), "1 >>> 23");
    Assert.That(UInt24.One >>> 24, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "1 >>> 24");

    Assert.That(UInt24.MaxValue >>> 0, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max >>> 0");

    Assert.That(UInt24.MaxValue >>> 1, Is.EqualTo((UInt24)0b_0111_1111_1111_1111_1111_1111u), "Max >>> 1");
    Assert.That(UInt24.MaxValue >>> 23, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "Max >>> 23");
    Assert.That(UInt24.MaxValue >>> 24, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max >>> (24 = 0)");
    Assert.That(UInt24.MaxValue >>> 25, Is.EqualTo((UInt24)0b_0111_1111_1111_1111_1111_1111u), "Max >>> (25 = 1)");
    Assert.That(UInt24.MaxValue >>> 48, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max >>> (48 = 0)");
    Assert.That(UInt24.MaxValue >>> 49, Is.EqualTo((UInt24)0b_0111_1111_1111_1111_1111_1111u), "Max >>> (49 = 1)");

    Assert.That(UInt24.MaxValue >>> -1, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "Max >>> (-1 = 23)");
    Assert.That(UInt24.MaxValue >>> -23, Is.EqualTo((UInt24)0b_0111_1111_1111_1111_1111_1111u), "Max >>> (-23 = 1)");
    Assert.That(UInt24.MaxValue >>> -24, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max >>> (-24 = 0)");
    Assert.That(UInt24.MaxValue >>> -25, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "Max >>> (-25 = 23)");
    Assert.That(UInt24.MaxValue >>> -48, Is.EqualTo((UInt24)0b_1111_1111_1111_1111_1111_1111u), "Max >>> (-48 = 0)");
    Assert.That(UInt24.MaxValue >>> -49, Is.EqualTo((UInt24)0b_0000_0000_0000_0000_0000_0001u), "Max >>> (-49 = 23)");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpLeftShift()
  {
    Assert.That(UInt48.Zero << 0, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "0 << 0");
    Assert.That(UInt48.Zero << 1, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "0 << 1");

    Assert.That(UInt48.One << 0, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "1 << 0");
    Assert.That(UInt48.One << 1, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0010uL), "1 << 1");
    Assert.That(UInt48.One << 47, Is.EqualTo((UInt48)0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "1 << 47");
    Assert.That(UInt48.One << 48, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "1 << 48");

    Assert.That(UInt48.MaxValue << 0, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max << 0");

    Assert.That(UInt48.MaxValue << 1, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110uL), "Max << 1");
    Assert.That(UInt48.MaxValue << 47, Is.EqualTo((UInt48)0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "Max << 47");
    Assert.That(UInt48.MaxValue << 48, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max << (48 = 0)");
    Assert.That(UInt48.MaxValue << 49, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110uL), "Max << (49 = 1)");
    Assert.That(UInt48.MaxValue << 96, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max << (96 = 0)");
    Assert.That(UInt48.MaxValue << 97, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110uL), "Max << (97 = 1)");

    Assert.That(UInt48.MaxValue << -1, Is.EqualTo((UInt48)0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "Max << (-1 = 47)");
    Assert.That(UInt48.MaxValue << -47, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110uL), "Max << (-47 = 1)");
    Assert.That(UInt48.MaxValue << -48, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max << (-48 = 0)");
    Assert.That(UInt48.MaxValue << -49, Is.EqualTo((UInt48)0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "Max << (-49 = 47)");
    Assert.That(UInt48.MaxValue << -96, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max << (-96 = 0)");
    Assert.That(UInt48.MaxValue << -97, Is.EqualTo((UInt48)0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "Max << (-97 = 47)");
  }

  [Test]
  public void TestOpRightShift()
  {
    Assert.That(UInt48.Zero >> 0, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "0 >> 0");
    Assert.That(UInt48.Zero >> 1, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "0 >> 1");

    Assert.That(UInt48.One >> 0, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "1 >> 0");
    Assert.That(UInt48.One >> 1, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "1 >> 1");
    Assert.That(UInt48.One >> 47, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "1 >> 47");
    Assert.That(UInt48.One >> 48, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "1 >> 48");

    Assert.That(UInt48.MaxValue >> 0, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >> 0");

    Assert.That(UInt48.MaxValue >> 1, Is.EqualTo((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >> 1");
    Assert.That(UInt48.MaxValue >> 47, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "Max >> 47");
    Assert.That(UInt48.MaxValue >> 48, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >> (48 = 0)");
    Assert.That(UInt48.MaxValue >> 49, Is.EqualTo((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >> (49 = 1)");
    Assert.That(UInt48.MaxValue >> 96, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >> (96 = 0)");
    Assert.That(UInt48.MaxValue >> 97, Is.EqualTo((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >> (97 = 1)");

    Assert.That(UInt48.MaxValue >> -1, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "Max >> (-1 = 47)");
    Assert.That(UInt48.MaxValue >> -47, Is.EqualTo((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >> (-47 = 1)");
    Assert.That(UInt48.MaxValue >> -48, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >> (-48 = 0)");
    Assert.That(UInt48.MaxValue >> -49, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "Max >> (-49 = 47)");
    Assert.That(UInt48.MaxValue >> -96, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >> (-96 = 0)");
    Assert.That(UInt48.MaxValue >> -97, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "Max >> (-97 = 47)");
  }


  [Test]
  public void TestOpUnsignedRightShift()
  {
    Assert.That(UInt48.Zero >>> 0, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "0 >>> 0");
    Assert.That(UInt48.Zero >>> 1, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "0 >>> 1");

    Assert.That(UInt48.One >>> 0, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "1 >>> 0");
    Assert.That(UInt48.One >>> 1, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "1 >>> 1");
    Assert.That(UInt48.One >>> 47, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL), "1 >>> 47");
    Assert.That(UInt48.One >>> 48, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "1 >>> 48");

    Assert.That(UInt48.MaxValue >>> 0, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >>> 0");

    Assert.That(UInt48.MaxValue >>> 1, Is.EqualTo((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >>> 1");
    Assert.That(UInt48.MaxValue >>> 47, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "Max >>> 47");
    Assert.That(UInt48.MaxValue >>> 48, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >>> (48 = 0)");
    Assert.That(UInt48.MaxValue >>> 49, Is.EqualTo((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >>> (49 = 1)");
    Assert.That(UInt48.MaxValue >>> 96, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >>> (96 = 0)");
    Assert.That(UInt48.MaxValue >>> 97, Is.EqualTo((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >>> (97 = 1)");

    Assert.That(UInt48.MaxValue >>> -1, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "Max >>> (-1 = 47)");
    Assert.That(UInt48.MaxValue >>> -47, Is.EqualTo((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >>> (-47 = 1)");
    Assert.That(UInt48.MaxValue >>> -48, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >>> (-48 = 0)");
    Assert.That(UInt48.MaxValue >>> -49, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "Max >>> (-49 = 47)");
    Assert.That(UInt48.MaxValue >>> -96, Is.EqualTo((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL), "Max >>> (-96 = 0)");
    Assert.That(UInt48.MaxValue >>> -97, Is.EqualTo((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL), "Max >>> (-97 = 47)");
  }
}

#if FEATURE_GENERIC_MATH
partial class UInt24nTests {
  [Test]
  public void IShiftOperators_OpLeftShift()
  {
    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0000u, ShiftLeft(UInt24.Zero, 0), "0 << 0");
    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0000u, ShiftLeft(UInt24.Zero, 1), "0 << 1");

    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0001u, ShiftLeft(UInt24.One, 0), "1 << 0");
    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0010u, ShiftLeft(UInt24.One, 1), "1 << 1");
    Assert.AreEqual((UInt24)0b_1000_0000_0000_0000_0000_0000u, ShiftLeft(UInt24.One, 23), "1 << 23");
    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0001u, ShiftLeft(UInt24.One, 24), "1 << 24");

    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1111u, ShiftLeft(UInt24.MaxValue, 0), "Max << 0");

    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1110u, ShiftLeft(UInt24.MaxValue, 1), "Max << 1");
    Assert.AreEqual((UInt24)0b_1000_0000_0000_0000_0000_0000u, ShiftLeft(UInt24.MaxValue, 23), "Max << 23");
    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1111u, ShiftLeft(UInt24.MaxValue, 24), "Max << (24 = 0)");
    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1110u, ShiftLeft(UInt24.MaxValue, 25), "Max << (25 = 1)");
    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1111u, ShiftLeft(UInt24.MaxValue, 48), "Max << (48 = 0)");
    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1110u, ShiftLeft(UInt24.MaxValue, 49), "Max << (49 = 1)");

    Assert.AreEqual((UInt24)0b_1000_0000_0000_0000_0000_0000u, ShiftLeft(UInt24.MaxValue, -1), "Max << (-1 = 23)");
    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1110u, ShiftLeft(UInt24.MaxValue, -23), "Max << (-23 = 1)");
    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1111u, ShiftLeft(UInt24.MaxValue, -24), "Max << (-24 = 0)");
    Assert.AreEqual((UInt24)0b_1000_0000_0000_0000_0000_0000u, ShiftLeft(UInt24.MaxValue, -25), "Max << (-25 = 23)");
    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1111u, ShiftLeft(UInt24.MaxValue, -48), "Max << (-48 = 0)");
    Assert.AreEqual((UInt24)0b_1000_0000_0000_0000_0000_0000u, ShiftLeft(UInt24.MaxValue, -49), "Max << (-49 = 23)");

    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL, ShiftLeft(UInt48.Zero, 0), "0 << 0");
    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL, ShiftLeft(UInt48.Zero, 1), "0 << 1");

    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL, ShiftLeft(UInt48.One, 0), "1 << 0");
    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0010uL, ShiftLeft(UInt48.One, 1), "1 << 1");
    Assert.AreEqual((UInt48)0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL, ShiftLeft(UInt48.One, 47), "1 << 47");
    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL, ShiftLeft(UInt48.One, 48), "1 << 48");

    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftLeft(UInt48.MaxValue, 0), "Max << 0");

    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110uL, ShiftLeft(UInt48.MaxValue, 1), "Max << 1");
    Assert.AreEqual((UInt48)0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL, ShiftLeft(UInt48.MaxValue, 47), "Max << 47");
    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftLeft(UInt48.MaxValue, 48), "Max << (48 = 0)");
    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110uL, ShiftLeft(UInt48.MaxValue, 49), "Max << (49 = 1)");
    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftLeft(UInt48.MaxValue, 96), "Max << (96 = 0)");
    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110uL, ShiftLeft(UInt48.MaxValue, 97), "Max << (97 = 1)");

    Assert.AreEqual((UInt48)0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL, ShiftLeft(UInt48.MaxValue, -1), "Max << (-1 = 47)");
    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1110uL, ShiftLeft(UInt48.MaxValue, -47), "Max << (-47 = 1)");
    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftLeft(UInt48.MaxValue, -48), "Max << (-48 = 0)");
    Assert.AreEqual((UInt48)0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL, ShiftLeft(UInt48.MaxValue, -49), "Max << (-49 = 47)");
    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftLeft(UInt48.MaxValue, -96), "Max << (-96 = 0)");
    Assert.AreEqual((UInt48)0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL, ShiftLeft(UInt48.MaxValue, -97), "Max << (-97 = 47)");

    static TUInt24n ShiftLeft<TUInt24n>(TUInt24n value, int shiftAmount) where TUInt24n : IShiftOperators<TUInt24n, int, TUInt24n>
      => value << shiftAmount;
  }

  [TestCase(true)]
  [TestCase(false)]
  public void IShiftOperators_OpRightShift(bool performUnsignedRightShift)
  {
    var op = performUnsignedRightShift ? ">>>" : ">>";

    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0000u, ShiftRight(UInt24.Zero, 0, performUnsignedRightShift), $"0 {op} 0");
    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0000u, ShiftRight(UInt24.Zero, 1, performUnsignedRightShift), $"0 {op} 1");

    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0001u, ShiftRight(UInt24.One, 0, performUnsignedRightShift), $"1 {op} 0");
    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0000u, ShiftRight(UInt24.One, 1, performUnsignedRightShift), $"1 {op} 1");
    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0000u, ShiftRight(UInt24.One, 23, performUnsignedRightShift), $"1 {op} 23");
    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0001u, ShiftRight(UInt24.One, 24, performUnsignedRightShift), $"1 {op} 24");

    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1111u, ShiftRight(UInt24.MaxValue, 0, performUnsignedRightShift), $"Max {op} 0");

    Assert.AreEqual((UInt24)0b_0111_1111_1111_1111_1111_1111u, ShiftRight(UInt24.MaxValue, 1, performUnsignedRightShift), $"Max {op} 1");
    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0001u, ShiftRight(UInt24.MaxValue, 23, performUnsignedRightShift), $"Max {op} 23");
    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1111u, ShiftRight(UInt24.MaxValue, 24, performUnsignedRightShift), $"Max {op} (24 = 0)");
    Assert.AreEqual((UInt24)0b_0111_1111_1111_1111_1111_1111u, ShiftRight(UInt24.MaxValue, 25, performUnsignedRightShift), $"Max {op} (25 = 1)");
    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1111u, ShiftRight(UInt24.MaxValue, 48, performUnsignedRightShift), $"Max {op} (48 = 0)");
    Assert.AreEqual((UInt24)0b_0111_1111_1111_1111_1111_1111u, ShiftRight(UInt24.MaxValue, 49, performUnsignedRightShift), $"Max {op} (49 = 1)");

    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0001u, ShiftRight(UInt24.MaxValue, -1, performUnsignedRightShift), $"Max {op} (-1 = 23)");
    Assert.AreEqual((UInt24)0b_0111_1111_1111_1111_1111_1111u, ShiftRight(UInt24.MaxValue, -23, performUnsignedRightShift), $"Max {op} (-23 = 1)");
    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1111u, ShiftRight(UInt24.MaxValue, -24, performUnsignedRightShift), $"Max {op} (-24 = 0)");
    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0001u, ShiftRight(UInt24.MaxValue, -25, performUnsignedRightShift), $"Max {op} (-25 = 23)");
    Assert.AreEqual((UInt24)0b_1111_1111_1111_1111_1111_1111u, ShiftRight(UInt24.MaxValue, -48, performUnsignedRightShift), $"Max {op} (-48 = 0)");
    Assert.AreEqual((UInt24)0b_0000_0000_0000_0000_0000_0001u, ShiftRight(UInt24.MaxValue, -49, performUnsignedRightShift), $"Max {op} (-49 = 23)");

    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL, ShiftRight(UInt48.Zero, 0, performUnsignedRightShift), $"0 {op} 0");
    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL, ShiftRight(UInt48.Zero, 1, performUnsignedRightShift), $"0 {op} 1");

    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL, ShiftRight(UInt48.One, 0, performUnsignedRightShift), $"1 {op} 0");
    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL, ShiftRight(UInt48.One, 1, performUnsignedRightShift), $"1 {op} 1");
    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000uL, ShiftRight(UInt48.One, 47, performUnsignedRightShift), $"1 {op} 47");
    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL, ShiftRight(UInt48.One, 48, performUnsignedRightShift), $"1 {op} 48");

    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftRight(UInt48.MaxValue, 0, performUnsignedRightShift), $"Max {op} 0");

    Assert.AreEqual((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftRight(UInt48.MaxValue, 1, performUnsignedRightShift), $"Max {op} 1");
    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL, ShiftRight(UInt48.MaxValue, 47, performUnsignedRightShift), $"Max {op} 47");
    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftRight(UInt48.MaxValue, 48, performUnsignedRightShift), $"Max {op} (48 = 0)");
    Assert.AreEqual((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftRight(UInt48.MaxValue, 49, performUnsignedRightShift), $"Max {op} (49 = 1)");
    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftRight(UInt48.MaxValue, 96, performUnsignedRightShift), $"Max {op} (96 = 0)");
    Assert.AreEqual((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftRight(UInt48.MaxValue, 97, performUnsignedRightShift), $"Max {op} (97 = 1)");

    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL, ShiftRight(UInt48.MaxValue, -1, performUnsignedRightShift), $"Max {op} (-1 = 47)");
    Assert.AreEqual((UInt48)0b_0111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftRight(UInt48.MaxValue, -47, performUnsignedRightShift), $"Max {op} (-47 = 1)");
    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftRight(UInt48.MaxValue, -48, performUnsignedRightShift), $"Max {op} (-48 = 0)");
    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL, ShiftRight(UInt48.MaxValue, -49, performUnsignedRightShift), $"Max {op} (-49 = 47)");
    Assert.AreEqual((UInt48)0b_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111_1111uL, ShiftRight(UInt48.MaxValue, -96, performUnsignedRightShift), $"Max {op} (-96 = 0)");
    Assert.AreEqual((UInt48)0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0001uL, ShiftRight(UInt48.MaxValue, -97, performUnsignedRightShift), $"Max {op} (-97 = 47)");

    static TUInt24n ShiftRight<TUInt24n>(TUInt24n value, int shiftAmount, bool performUnsignedRightShift) where TUInt24n : IShiftOperators<TUInt24n, int, TUInt24n>
      => performUnsignedRightShift
        ? value >>> shiftAmount
        : value >> shiftAmount;
  }
}
#endif
