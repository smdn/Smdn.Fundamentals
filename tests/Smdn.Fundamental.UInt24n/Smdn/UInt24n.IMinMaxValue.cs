// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;


partial class UInt24Tests {
  [Test] public void MinValue() => Assert.That(UInt24.MinValue, Is.EqualTo(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x00 }, isBigEndian: true)));
  [Test] public void MaxValue() => Assert.That(UInt24.MaxValue, Is.EqualTo(new UInt24(stackalloc byte[3] { 0xFF, 0xFF, 0xFF }, isBigEndian: true)));
}

partial class UInt48Tests {
  [Test] public void MinValue() => Assert.That(UInt48.MinValue, Is.EqualTo(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isBigEndian: true)));
  [Test] public void MaxValue() => Assert.That(UInt48.MaxValue, Is.EqualTo(new UInt48(stackalloc byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, isBigEndian: true)));
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void IMinMaxValue_MinValue()
  {
    Assert.AreEqual(UInt24.MinValue, GetMinValue<UInt24>(), nameof(UInt24));
    Assert.AreEqual(UInt48.MinValue, GetMinValue<UInt48>(), nameof(UInt48));

    static TUInt24n GetMinValue<TUInt24n>() where TUInt24n : IMinMaxValue<TUInt24n>
      => TUInt24n.MinValue;
  }

  [Test]
  public void IMinMaxValue_MaxValue()
  {
    Assert.AreEqual(UInt24.MaxValue, GetMaxValue<UInt24>(), nameof(UInt24));
    Assert.AreEqual(UInt48.MaxValue, GetMaxValue<UInt48>(), nameof(UInt48));

    static TUInt24n GetMaxValue<TUInt24n>() where TUInt24n : IMinMaxValue<TUInt24n>
      => TUInt24n.MaxValue;
  }
#endif
}
