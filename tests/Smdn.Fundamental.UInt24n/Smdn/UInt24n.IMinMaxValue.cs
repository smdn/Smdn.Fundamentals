// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IMINMAXVALUE
using System.Numerics;
#endif

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
#if SYSTEM_NUMERICS_IMINMAXVALUE
  [Test]
  public void IMinMaxValue_MinValue()
  {
    Assert.That(GetMinValue<UInt24>(), Is.EqualTo(UInt24.MinValue), nameof(UInt24));
    Assert.That(GetMinValue<UInt48>(), Is.EqualTo(UInt48.MinValue), nameof(UInt48));

    static TUInt24n GetMinValue<TUInt24n>() where TUInt24n : IMinMaxValue<TUInt24n>
      => TUInt24n.MinValue;
  }

  [Test]
  public void IMinMaxValue_MaxValue()
  {
    Assert.That(GetMaxValue<UInt24>(), Is.EqualTo(UInt24.MaxValue), nameof(UInt24));
    Assert.That(GetMaxValue<UInt48>(), Is.EqualTo(UInt48.MaxValue), nameof(UInt48));

    static TUInt24n GetMaxValue<TUInt24n>() where TUInt24n : IMinMaxValue<TUInt24n>
      => TUInt24n.MaxValue;
  }
#endif
}
