// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_ICOMPARISONOPERATORS
using System.Numerics;

#endif
using NUnit.Framework;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

#pragma warning disable IDE0040
partial class NodeTests {
#pragma warning restore IDE0040
  [TestCase("00:00:00:00:00:00", "00:00:00:00:00:00", false,  true)]
  [TestCase("00:00:00:00:00:00", "00:00:00:00:00:01",  true,  true)]
  [TestCase("00:00:00:00:00:00", "00:00:00:00:01:00",  true,  true)]
  [TestCase("00:00:00:00:00:00", "00:00:00:01:00:00",  true,  true)]
  [TestCase("00:00:00:00:00:00", "00:00:01:00:00:00",  true,  true)]
  [TestCase("00:00:00:00:00:00", "00:01:00:00:00:00",  true,  true)]
  [TestCase("00:00:00:00:00:00", "01:00:00:00:00:00",  true,  true)]
  [TestCase("00:00:00:00:00:01", "00:00:00:00:00:00", false,  false)]
  [TestCase("00:00:00:00:01:00", "00:00:00:00:00:00", false,  false)]
  [TestCase("00:00:00:01:00:00", "00:00:00:00:00:00", false,  false)]
  [TestCase("00:00:01:00:00:00", "00:00:00:00:00:00", false,  false)]
  [TestCase("00:01:00:00:00:00", "00:00:00:00:00:00", false,  false)]
  [TestCase("01:00:00:00:00:00", "00:00:00:00:00:00", false,  false)]
  [TestCase("FF:FF:FF:FF:FF:FF", "FF:FF:FF:FF:FF:FF", false,   true)]
  public void TestOpLessThan_OpLessThanOrEqual(string x, string y, bool lessThan, bool lessThanOrEqual)
  {
#pragma warning disable CA1305
    Assert.That(Node.Parse(x) < Node.Parse(y), Is.EqualTo(lessThan), "op <");
    Assert.That(Node.Parse(x) <= Node.Parse(y), Is.EqualTo(lessThanOrEqual), "op <=");

#if SYSTEM_NUMERICS_ICOMPARISONOPERATORS
    Assert.That(OpLessThan(Node.Parse(x), Node.Parse(y)), Is.EqualTo(lessThan), "IComparisonOperators <");
    Assert.That(OpLessThanOrEqual(Node.Parse(x), Node.Parse(y)), Is.EqualTo(lessThanOrEqual), "IComparisonOperators <=");

    static bool OpLessThan<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x < y;
    static bool OpLessThanOrEqual<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x <= y;
#endif
#pragma warning restore CA1305
  }

  [TestCase("00:00:00:00:00:00", "00:00:00:00:00:00", false,  true)]
  [TestCase("00:00:00:00:00:00", "00:00:00:00:00:01", false,  false)]
  [TestCase("00:00:00:00:00:00", "00:00:00:00:01:00", false,  false)]
  [TestCase("00:00:00:00:00:00", "00:00:00:01:00:00", false,  false)]
  [TestCase("00:00:00:00:00:00", "00:00:01:00:00:00", false,  false)]
  [TestCase("00:00:00:00:00:00", "00:01:00:00:00:00", false,  false)]
  [TestCase("00:00:00:00:00:00", "01:00:00:00:00:00", false,  false)]
  [TestCase("00:00:00:00:00:01", "00:00:00:00:00:00",  true,   true)]
  [TestCase("00:00:00:00:01:00", "00:00:00:00:00:00",  true,   true)]
  [TestCase("00:00:00:01:00:00", "00:00:00:00:00:00",  true,   true)]
  [TestCase("00:00:01:00:00:00", "00:00:00:00:00:00",  true,   true)]
  [TestCase("00:01:00:00:00:00", "00:00:00:00:00:00",  true,   true)]
  [TestCase("01:00:00:00:00:00", "00:00:00:00:00:00",  true,   true)]
  [TestCase("FF:FF:FF:FF:FF:FF", "FF:FF:FF:FF:FF:FF", false,   true)]
  public void TestOpGreaterThan_OpGreaterThanOrEqual(string x, string y, bool greaterThan, bool greaterThanOrEqual)
  {
#pragma warning disable CA1305
    Assert.That(Node.Parse(x) > Node.Parse(y), Is.EqualTo(greaterThan), "op >");
    Assert.That(Node.Parse(x) >= Node.Parse(y), Is.EqualTo(greaterThanOrEqual), "op >=");

#if SYSTEM_NUMERICS_ICOMPARISONOPERATORS
    Assert.That(OpGreaterThan(Node.Parse(x), Node.Parse(y)), Is.EqualTo(greaterThan), "IComparisonOperators >");
    Assert.That(OpGreaterThanOrEqual(Node.Parse(x), Node.Parse(y)), Is.EqualTo(greaterThanOrEqual), "IComparisonOperators >=");

    static bool OpGreaterThan<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x > y;
    static bool OpGreaterThanOrEqual<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x >= y;
#endif
#pragma warning restore CA1305
  }

  [TestCase("00:00:00:00:00:00", "00:00:00:00:00:00",  0)]
  [TestCase("00:00:00:00:00:00", "00:00:00:00:00:01", -1)]
  [TestCase("00:00:00:00:00:00", "00:00:00:00:01:00", -1)]
  [TestCase("00:00:00:00:00:00", "00:00:00:01:00:00", -1)]
  [TestCase("00:00:00:00:00:00", "00:00:01:00:00:00", -1)]
  [TestCase("00:00:00:00:00:00", "00:01:00:00:00:00", -1)]
  [TestCase("00:00:00:00:00:00", "01:00:00:00:00:00", -1)]
  [TestCase("00:00:00:00:00:01", "00:00:00:00:00:00", +1)]
  [TestCase("00:00:00:00:01:00", "00:00:00:00:00:00", +1)]
  [TestCase("00:00:00:01:00:00", "00:00:00:00:00:00", +1)]
  [TestCase("00:00:01:00:00:00", "00:00:00:00:00:00", +1)]
  [TestCase("00:01:00:00:00:00", "00:00:00:00:00:00", +1)]
  [TestCase("01:00:00:00:00:00", "00:00:00:00:00:00", +1)]
  [TestCase("FF:FF:FF:FF:FF:FF", "FF:FF:FF:FF:FF:FF",  0)]
  public void TestCompareTo(string x, string y, int expected)
#pragma warning disable CA1305
    => Assert.That(Math.Sign(Node.Parse(x).CompareTo(Node.Parse(y))), Is.EqualTo(expected));
#pragma warning restore CA1305

  [Test]
  public void TestCompareTo_Object()
  {
#pragma warning disable CA1305
    Assert.That(0 == Node.Parse("00:00:00:00:00:00").CompareTo((object)Node.Parse("00:00:00:00:00:00")), "#1");
    Assert.That(0 > Node.Parse("00:00:00:00:00:00").CompareTo((object)Node.Parse("00:00:00:00:00:01")), "#2");
    Assert.That(0 < Node.Parse("00:00:00:00:00:00").CompareTo(null), "#3");

    Assert.Throws<ArgumentException>(() => Node.Parse("00:00:00:00:00:00").CompareTo("00:00:00:00:00:00"), "#4");
    Assert.Throws<ArgumentException>(() => Node.Parse("00:00:00:00:00:00").CompareTo(0), "#5");
#pragma warning restore CA1305
  }
}
