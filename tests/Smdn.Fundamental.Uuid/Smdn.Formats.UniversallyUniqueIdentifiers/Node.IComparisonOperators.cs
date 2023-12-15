// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif
using NUnit.Framework;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

partial class NodeTests {
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
    Assert.That(Node.Parse(x) < Node.Parse(y), Is.EqualTo(lessThan), "op <");
    Assert.That(Node.Parse(x) <= Node.Parse(y), Is.EqualTo(lessThanOrEqual), "op <=");

#if FEATURE_GENERIC_MATH
    Assert.AreEqual(lessThan, OpLessThan(Node.Parse(x), Node.Parse(y)), "IComparisonOperators <");
    Assert.AreEqual(lessThanOrEqual, OpLessThanOrEqual(Node.Parse(x), Node.Parse(y)), "IComparisonOperators <=");

    static bool OpLessThan<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x < y;
    static bool OpLessThanOrEqual<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x <= y;
#endif
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
    Assert.That(Node.Parse(x) > Node.Parse(y), Is.EqualTo(greaterThan), "op >");
    Assert.That(Node.Parse(x) >= Node.Parse(y), Is.EqualTo(greaterThanOrEqual), "op >=");

#if FEATURE_GENERIC_MATH
    Assert.AreEqual(greaterThan, OpGreaterThan(Node.Parse(x), Node.Parse(y)), "IComparisonOperators >");
    Assert.AreEqual(greaterThanOrEqual, OpGreaterThanOrEqual(Node.Parse(x), Node.Parse(y)), "IComparisonOperators >=");

    static bool OpGreaterThan<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x > y;
    static bool OpGreaterThanOrEqual<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x >= y;
#endif
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
    => Assert.That(Math.Sign(Node.Parse(x).CompareTo(Node.Parse(y))), Is.EqualTo(expected));

  [Test]
  public void TestCompareTo_Object()
  {
    Assert.That(0 == Node.Parse("00:00:00:00:00:00").CompareTo((object)Node.Parse("00:00:00:00:00:00")), "#1");
    Assert.That(0 > Node.Parse("00:00:00:00:00:00").CompareTo((object)Node.Parse("00:00:00:00:00:01")), "#2");
    Assert.That(0 < Node.Parse("00:00:00:00:00:00").CompareTo(null), "#3");

    Assert.Throws<ArgumentException>(() => Node.Parse("00:00:00:00:00:00").CompareTo("00:00:00:00:00:00"), "#4");
    Assert.Throws<ArgumentException>(() => Node.Parse("00:00:00:00:00:00").CompareTo(0), "#5");
  }
}
