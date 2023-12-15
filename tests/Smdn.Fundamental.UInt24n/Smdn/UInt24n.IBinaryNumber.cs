// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void TestOpBitwiseAnd()
  {
    Assert.That(UInt24.Zero & UInt24.Zero, Is.EqualTo(UInt24.Zero), "0 & 0");
    Assert.That(UInt24.One & UInt24.Zero, Is.EqualTo(UInt24.Zero), "1 & 0");
    Assert.That(UInt24.Zero & UInt24.One, Is.EqualTo(UInt24.Zero), "0 & 1");
    Assert.That(UInt24.One & UInt24.One, Is.EqualTo(UInt24.One), "1 & 1");
    Assert.That(UInt24.One & UInt24.MaxValue, Is.EqualTo(UInt24.One), "1 & Max");
    Assert.That(UInt24.MaxValue & UInt24.One, Is.EqualTo(UInt24.One), "Max & 1");
    Assert.That(UInt24.MaxValue & UInt24.MaxValue, Is.EqualTo(UInt24.MaxValue), "Max & Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpBitwiseAnd()
  {
    Assert.That(UInt48.Zero & UInt48.Zero, Is.EqualTo(UInt48.Zero), "0 & 0");
    Assert.That(UInt48.One & UInt48.Zero, Is.EqualTo(UInt48.Zero), "1 & 0");
    Assert.That(UInt48.Zero & UInt48.One, Is.EqualTo(UInt48.Zero), "0 & 1");
    Assert.That(UInt48.One & UInt48.One, Is.EqualTo(UInt48.One), "1 & 1");
    Assert.That(UInt48.One & UInt48.MaxValue, Is.EqualTo(UInt48.One), "1 & Max");
    Assert.That(UInt48.MaxValue & UInt48.One, Is.EqualTo(UInt48.One), "Max & 1");
    Assert.That(UInt48.MaxValue & UInt48.MaxValue, Is.EqualTo(UInt48.MaxValue), "Max & Max");
  }
}

#if FEATURE_GENERIC_MATH
partial class UInt24nTests {
  [Test]
  public void IBitwiseOperators_BitwiseAnd()
  {
    Assert.AreEqual(UInt24.Zero, BitwiseAnd(UInt24.Zero, UInt24.Zero), "UInt24 0 & 0");
    Assert.AreEqual(UInt24.Zero, BitwiseAnd(UInt24.One, UInt24.Zero), "UInt24 1 & 0");
    Assert.AreEqual(UInt24.Zero, BitwiseAnd(UInt24.Zero, UInt24.One), "UInt24 0 & 1");
    Assert.AreEqual(UInt24.One, BitwiseAnd(UInt24.One, UInt24.One), "UInt24 1 & 1");
    Assert.AreEqual(UInt24.One, BitwiseAnd(UInt24.One, UInt24.MaxValue), "UInt24 1 & Max");
    Assert.AreEqual(UInt24.One, BitwiseAnd(UInt24.MaxValue, UInt24.One), "UInt24 Max & 1");
    Assert.AreEqual(UInt24.MaxValue, BitwiseAnd(UInt24.MaxValue, UInt24.MaxValue), "UInt24 Max & Max");

    Assert.AreEqual(UInt48.Zero, BitwiseAnd(UInt48.Zero, UInt48.Zero), "UInt48 0 & 0");
    Assert.AreEqual(UInt48.Zero, BitwiseAnd(UInt48.One, UInt48.Zero), "UInt48 1 & 0");
    Assert.AreEqual(UInt48.Zero, BitwiseAnd(UInt48.Zero, UInt48.One), "UInt48 0 & 1");
    Assert.AreEqual(UInt48.One, BitwiseAnd(UInt48.One, UInt48.One), "UInt48 1 & 1");
    Assert.AreEqual(UInt48.One, BitwiseAnd(UInt48.One, UInt48.MaxValue), "UInt48 1 & Max");
    Assert.AreEqual(UInt48.One, BitwiseAnd(UInt48.MaxValue, UInt48.One), "UInt48 Max & 1");
    Assert.AreEqual(UInt48.MaxValue, BitwiseAnd(UInt48.MaxValue, UInt48.MaxValue), "UInt48 Max & Max");

    static TUInt24n BitwiseAnd<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IBitwiseOperators<TUInt24n, TUInt24n, TUInt24n>
      => x & y;
  }
}
#endif

partial class UInt24Tests {
  [Test]
  public void TestOpBitwiseOr()
  {
    Assert.That(UInt24.Zero | UInt24.Zero, Is.EqualTo(UInt24.Zero), "0 | 0");
    Assert.That(UInt24.One | UInt24.Zero, Is.EqualTo(UInt24.One), "1 | 0");
    Assert.That(UInt24.Zero | UInt24.One, Is.EqualTo(UInt24.One), "0 | 1");
    Assert.That(UInt24.One | UInt24.One, Is.EqualTo(UInt24.One), "1 | 1");
    Assert.That(UInt24.One | UInt24.MaxValue, Is.EqualTo(UInt24.MaxValue), "1 | Max");
    Assert.That(UInt24.MaxValue | UInt24.One, Is.EqualTo(UInt24.MaxValue), "Max | 1");
    Assert.That(UInt24.MaxValue | UInt24.MaxValue, Is.EqualTo(UInt24.MaxValue), "Max | Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpBitwiseOr()
  {
    Assert.That(UInt48.Zero | UInt48.Zero, Is.EqualTo(UInt48.Zero), "0 | 0");
    Assert.That(UInt48.One | UInt48.Zero, Is.EqualTo(UInt48.One), "1 | 0");
    Assert.That(UInt48.Zero | UInt48.One, Is.EqualTo(UInt48.One), "0 | 1");
    Assert.That(UInt48.One | UInt48.One, Is.EqualTo(UInt48.One), "1 | 1");
    Assert.That(UInt48.One | UInt48.MaxValue, Is.EqualTo(UInt48.MaxValue), "1 | Max");
    Assert.That(UInt48.MaxValue | UInt48.One, Is.EqualTo(UInt48.MaxValue), "Max | 1");
    Assert.That(UInt48.MaxValue | UInt48.MaxValue, Is.EqualTo(UInt48.MaxValue), "Max | Max");
  }
}

#if FEATURE_GENERIC_MATH
partial class UInt24nTests {
  [Test]
  public void IBitwiseOperators_BitwiseOr()
  {
    Assert.AreEqual(UInt24.Zero, BitwiseOr(UInt24.Zero, UInt24.Zero), "UInt24 0 | 0");
    Assert.AreEqual(UInt24.One, BitwiseOr(UInt24.One, UInt24.Zero), "UInt24 1 | 0");
    Assert.AreEqual(UInt24.One, BitwiseOr(UInt24.Zero, UInt24.One), "UInt24 0 | 1");
    Assert.AreEqual(UInt24.One, BitwiseOr(UInt24.One, UInt24.One), "UInt24 1 | 1");
    Assert.AreEqual(UInt24.MaxValue, BitwiseOr(UInt24.One, UInt24.MaxValue), "UInt24 1 | Max");
    Assert.AreEqual(UInt24.MaxValue, BitwiseOr(UInt24.MaxValue, UInt24.One), "UInt24 Max | 1");
    Assert.AreEqual(UInt24.MaxValue, BitwiseOr(UInt24.MaxValue, UInt24.MaxValue), "UInt24 Max | Max");

    Assert.AreEqual(UInt48.Zero, BitwiseOr(UInt48.Zero, UInt48.Zero), "UInt48 0 | 0");
    Assert.AreEqual(UInt48.One, BitwiseOr(UInt48.One, UInt48.Zero), "UInt48 1 | 0");
    Assert.AreEqual(UInt48.One, BitwiseOr(UInt48.Zero, UInt48.One), "UInt48 0 | 1");
    Assert.AreEqual(UInt48.One, BitwiseOr(UInt48.One, UInt48.One), "UInt48 1 | 1");
    Assert.AreEqual(UInt48.MaxValue, BitwiseOr(UInt48.One, UInt48.MaxValue), "UInt48 1 | Max");
    Assert.AreEqual(UInt48.MaxValue, BitwiseOr(UInt48.MaxValue, UInt48.One), "UInt48 Max | 1");
    Assert.AreEqual(UInt48.MaxValue, BitwiseOr(UInt48.MaxValue, UInt48.MaxValue), "UInt48 Max | Max");

    static TUInt24n BitwiseOr<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IBitwiseOperators<TUInt24n, TUInt24n, TUInt24n>
      => x | y;
  }
}
#endif

partial class UInt24Tests {
  [Test]
  public void TestOpExclusiveOr()
  {
    Assert.That(UInt24.Zero ^ UInt24.Zero, Is.EqualTo(UInt24.Zero), "0 ^ 0");
    Assert.That(UInt24.One ^ UInt24.Zero, Is.EqualTo(UInt24.One), "1 ^ 0");
    Assert.That(UInt24.Zero ^ UInt24.One, Is.EqualTo(UInt24.One), "0 ^ 1");
    Assert.That(UInt24.One ^ UInt24.One, Is.EqualTo(UInt24.Zero), "1 ^ 1");
    Assert.That(UInt24.One ^ UInt24.MaxValue, Is.EqualTo(UInt24.MaxValue - UInt24.One), "1 ^ Max");
    Assert.That(UInt24.MaxValue ^ UInt24.One, Is.EqualTo(UInt24.MaxValue - UInt24.One), "Max ^ 1");
    Assert.That(UInt24.MaxValue ^ UInt24.MaxValue, Is.EqualTo(UInt24.Zero), "Max ^ Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpExclusiveOr()
  {
    Assert.That(UInt48.Zero ^ UInt48.Zero, Is.EqualTo(UInt48.Zero), "0 ^ 0");
    Assert.That(UInt48.One ^ UInt48.Zero, Is.EqualTo(UInt48.One), "1 ^ 0");
    Assert.That(UInt48.Zero ^ UInt48.One, Is.EqualTo(UInt48.One), "0 ^ 1");
    Assert.That(UInt48.One ^ UInt48.One, Is.EqualTo(UInt48.Zero), "1 ^ 1");
    Assert.That(UInt48.One ^ UInt48.MaxValue, Is.EqualTo(UInt48.MaxValue - UInt48.One), "1 ^ Max");
    Assert.That(UInt48.MaxValue ^ UInt48.One, Is.EqualTo(UInt48.MaxValue - UInt48.One), "Max ^ 1");
    Assert.That(UInt48.MaxValue ^ UInt48.MaxValue, Is.EqualTo(UInt48.Zero), "Max ^ Max");
  }
}

#if FEATURE_GENERIC_MATH
partial class UInt24nTests {
  [Test]
  public void IBitwiseOperators_ExclusiveOr()
  {
    Assert.AreEqual(UInt24.Zero, ExclusiveOr(UInt24.Zero, UInt24.Zero), "UInt24 0 ^ 0");
    Assert.AreEqual(UInt24.One, ExclusiveOr(UInt24.One, UInt24.Zero), "UInt24 1 ^ 0");
    Assert.AreEqual(UInt24.One, ExclusiveOr(UInt24.Zero, UInt24.One), "UInt24 0 ^ 1");
    Assert.AreEqual(UInt24.Zero, ExclusiveOr(UInt24.One, UInt24.One), "UInt24 1 ^ 1");
    Assert.AreEqual(UInt24.MaxValue - UInt24.One, ExclusiveOr(UInt24.One, UInt24.MaxValue), "UInt24 1 ^ Max");
    Assert.AreEqual(UInt24.MaxValue - UInt24.One, ExclusiveOr(UInt24.MaxValue, UInt24.One), "UInt24 Max ^ 1");
    Assert.AreEqual(UInt24.Zero, ExclusiveOr(UInt24.MaxValue, UInt24.MaxValue), "UInt24 Max ^ Max");

    Assert.AreEqual(UInt48.Zero, ExclusiveOr(UInt48.Zero, UInt48.Zero), "UInt48 0 ^ 0");
    Assert.AreEqual(UInt48.One, ExclusiveOr(UInt48.One, UInt48.Zero), "UInt48 1 ^ 0");
    Assert.AreEqual(UInt48.One, ExclusiveOr(UInt48.Zero, UInt48.One), "UInt48 0 ^ 1");
    Assert.AreEqual(UInt48.Zero, ExclusiveOr(UInt48.One, UInt48.One), "UInt48 1 ^ 1");
    Assert.AreEqual(UInt48.MaxValue - UInt48.One, ExclusiveOr(UInt48.One, UInt48.MaxValue), "UInt48 1 ^ Max");
    Assert.AreEqual(UInt48.MaxValue - UInt48.One, ExclusiveOr(UInt48.MaxValue, UInt48.One), "UInt48 Max ^ 1");
    Assert.AreEqual(UInt48.Zero, ExclusiveOr(UInt48.MaxValue, UInt48.MaxValue), "UInt48 Max ^ Max");

    static TUInt24n ExclusiveOr<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : IBitwiseOperators<TUInt24n, TUInt24n, TUInt24n>
      => x ^ y;
  }
}
#endif

partial class UInt24Tests {
  [Test]
  public void TestOpOnesComplement()
  {
    Assert.That(~UInt24.Zero, Is.EqualTo(UInt24.MaxValue), "~0");
    Assert.That(~UInt24.One, Is.EqualTo(UInt24.MaxValue - UInt24.One), "~1");
    Assert.That(~UInt24.MaxValue, Is.EqualTo(UInt24.Zero), "~Max");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestOpOnesComplement()
  {
    Assert.That(~UInt48.Zero, Is.EqualTo(UInt48.MaxValue), "~0");
    Assert.That(~UInt48.One, Is.EqualTo(UInt48.MaxValue - UInt48.One), "~1");
    Assert.That(~UInt48.MaxValue, Is.EqualTo(UInt48.Zero), "~Max");
  }
}

#if FEATURE_GENERIC_MATH
partial class UInt24nTests {
  [Test]
  public void IBitwiseOperators_OnesComplement()
  {
    Assert.AreEqual(UInt24.MaxValue, OnesComplement(UInt24.Zero), "UInt24 ~0");
    Assert.AreEqual(UInt24.MaxValue - UInt24.One, OnesComplement(UInt24.One), "UInt24 ~1");
    Assert.AreEqual(UInt24.Zero, OnesComplement(UInt24.MaxValue), "UInt24 ~Max");

    Assert.AreEqual(UInt48.MaxValue, OnesComplement(UInt48.Zero), "UInt48 ~0");
    Assert.AreEqual(UInt48.MaxValue - UInt48.One, OnesComplement(UInt48.One), "UInt48 ~1");
    Assert.AreEqual(UInt48.Zero, OnesComplement(UInt48.MaxValue), "UInt48 ~Max");

    static TUInt24n OnesComplement<TUInt24n>(TUInt24n value) where TUInt24n : IBitwiseOperators<TUInt24n, TUInt24n, TUInt24n>
      => ~value;
  }
}
#endif

partial class UInt24Tests {
  [Test]
  public void TestIsPow2()
  {
    Assert.That(UInt24.IsPow2(UInt24.Zero), Is.False, "IsPow2(0)");
    Assert.That(UInt24.IsPow2(UInt24.One), Is.True, "IsPow2(1)");
    Assert.That(UInt24.IsPow2(UInt24.One + UInt24.One), Is.True, "IsPow2(2)");
    Assert.That(UInt24.IsPow2(UInt24.MaxValue), Is.False, "IsPow2(Max)");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestIsPow2()
  {
    Assert.That(UInt48.IsPow2(UInt48.Zero), Is.False, "IsPow2(0)");
    Assert.That(UInt48.IsPow2(UInt48.One), Is.True, "IsPow2(1)");
    Assert.That(UInt48.IsPow2(UInt48.One + UInt48.One), Is.True, "IsPow2(2)");
    Assert.That(UInt48.IsPow2(UInt48.MaxValue), Is.False, "IsPow2(Max)");
  }
}

#if FEATURE_GENERIC_MATH
partial class UInt24nTests {
  [Test]
  public void IBinaryNumber_IsPow2()
  {
    Assert.IsFalse(IsPow2(UInt24.Zero), "UInt24 IsPow2(0)");
    Assert.IsTrue(IsPow2(UInt24.One), "UInt24 IsPow2(1)");
    Assert.IsTrue(IsPow2(UInt24.One + UInt24.One), "UInt24 IsPow2(2)");
    Assert.IsFalse(IsPow2(UInt24.MaxValue), "UInt24 IsPow2(Max)");

    Assert.IsFalse(IsPow2(UInt48.Zero), "UInt48 IsPow2(0)");
    Assert.IsTrue(IsPow2(UInt48.One), "UInt48 IsPow2(1)");
    Assert.IsTrue(IsPow2(UInt48.One + UInt48.One), "UInt48 IsPow2(2)");
    Assert.IsFalse(IsPow2(UInt48.MaxValue), "UInt48 IsPow2(Max)");

    static bool IsPow2<TUInt24n>(TUInt24n value) where TUInt24n : IBinaryNumber<TUInt24n>
      => TUInt24n.IsPow2(value);
  }
}
#endif

partial class UInt24Tests {
  [Test]
  public void TestLog2()
  {
    Assert.That(UInt24.Log2(UInt24.Zero), Is.EqualTo(0), "Log2(0)");
    Assert.That(UInt24.Log2(UInt24.One), Is.EqualTo(0), "Log2(1)");
    Assert.That(UInt24.Log2(UInt24.One + UInt24.One), Is.EqualTo(1), "Log2(2)");
    Assert.That(UInt24.Log2(UInt24.MaxValue), Is.EqualTo(23), "Log2(Max)");
  }
}

partial class UInt48Tests {
  [Test]
  public void TestLog2()
  {
    Assert.That(UInt48.Log2(UInt48.Zero), Is.EqualTo(0), "Log2(0)");
    Assert.That(UInt48.Log2(UInt48.One), Is.EqualTo(0), "Log2(1)");
    Assert.That(UInt48.Log2(UInt48.One + UInt48.One), Is.EqualTo(1), "Log2(2)");
    Assert.That(UInt48.Log2(UInt48.MaxValue), Is.EqualTo(47), "Log2(Max)");
  }
}

#if FEATURE_GENERIC_MATH
partial class UInt24nTests {
  [Test]
  public void IBinaryNumber_Log2()
  {
    Assert.AreEqual(UInt24.Zero, Log2(UInt24.Zero), "UInt24 Log2(0)");
    Assert.AreEqual(UInt24.Zero, Log2(UInt24.One), "UInt24 Log2(1)");
    Assert.AreEqual(UInt24.One, Log2(UInt24.One + UInt24.One), "UInt24 Log2(2)");
    Assert.AreEqual((UInt24)23, Log2(UInt24.MaxValue), "UInt24 Log2(Max)");

    Assert.AreEqual(UInt48.Zero, Log2(UInt48.Zero), "UInt48 Log2(0)");
    Assert.AreEqual(UInt48.Zero, Log2(UInt48.One), "UInt48 Log2(1)");
    Assert.AreEqual(UInt48.One, Log2(UInt48.One + UInt48.One), "UInt48 Log2(2)");
    Assert.AreEqual((UInt48)47, Log2(UInt48.MaxValue), "UInt48 Log2(Max)");

    static TUInt24n Log2<TUInt24n>(TUInt24n value) where TUInt24n : IBinaryNumber<TUInt24n>
      => TUInt24n.Log2(value);
  }
}
#endif
