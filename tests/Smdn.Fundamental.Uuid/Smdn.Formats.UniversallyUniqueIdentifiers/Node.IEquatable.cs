// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

partial class NodeTests {
  [TestCase("00:00:00:00:00:00", "00:00:00:00:00:00",  true)]
  [TestCase("00:00:00:00:00:00", "00:00:00:00:00:01", false)]
  [TestCase("00:00:00:00:00:00", "00:00:00:00:01:00", false)]
  [TestCase("00:00:00:00:00:00", "00:00:00:01:00:00", false)]
  [TestCase("00:00:00:00:00:00", "00:00:01:00:00:00", false)]
  [TestCase("00:00:00:00:00:00", "00:01:00:00:00:00", false)]
  [TestCase("00:00:00:00:00:00", "01:00:00:00:00:00", false)]
  [TestCase("00:00:00:00:00:01", "00:00:00:00:00:00", false)]
  [TestCase("00:00:00:00:01:00", "00:00:00:00:00:00", false)]
  [TestCase("00:00:00:01:00:00", "00:00:00:00:00:00", false)]
  [TestCase("00:00:01:00:00:00", "00:00:00:00:00:00", false)]
  [TestCase("00:01:00:00:00:00", "00:00:00:00:00:00", false)]
  [TestCase("01:00:00:00:00:00", "00:00:00:00:00:00", false)]
  [TestCase("FF:FF:FF:FF:FF:FF", "FF:FF:FF:FF:FF:FF",  true)]
  public void TestOpEquality_OpInequality(string x, string y, bool isEqual)
  {
    Assert.AreEqual(isEqual, Node.Parse(x) == Node.Parse(y), "op ==");
    Assert.AreEqual(!isEqual, Node.Parse(x) != Node.Parse(y), "op !=");

#if FEATURE_GENERIC_MATH
    Assert.AreEqual(isEqual, OpEquality(Node.Parse(x), Node.Parse(y)), "IEqualityOperators ==");
    Assert.AreEqual(!isEqual, OpInequality(Node.Parse(x), Node.Parse(y)), "IEqualityOperators !=");

    static bool OpEquality<T>(T x, T y) where T : IEqualityOperators<T, T> => x == y;
    static bool OpInequality<T>(T x, T y) where T : IEqualityOperators<T, T> => x != y;
#endif
  }

  [TestCase("00:00:00:00:00:00", "00:00:00:00:00:00",  true)]
  [TestCase("00:00:00:00:00:00", "00:00:00:00:00:01", false)]
  [TestCase("00:00:00:00:00:00", "00:00:00:00:01:00", false)]
  [TestCase("00:00:00:00:00:00", "00:00:00:01:00:00", false)]
  [TestCase("00:00:00:00:00:00", "00:00:01:00:00:00", false)]
  [TestCase("00:00:00:00:00:00", "00:01:00:00:00:00", false)]
  [TestCase("00:00:00:00:00:00", "01:00:00:00:00:00", false)]
  [TestCase("00:00:00:00:00:01", "00:00:00:00:00:00", false)]
  [TestCase("00:00:00:00:01:00", "00:00:00:00:00:00", false)]
  [TestCase("00:00:00:01:00:00", "00:00:00:00:00:00", false)]
  [TestCase("00:00:01:00:00:00", "00:00:00:00:00:00", false)]
  [TestCase("00:01:00:00:00:00", "00:00:00:00:00:00", false)]
  [TestCase("01:00:00:00:00:00", "00:00:00:00:00:00", false)]
  [TestCase("FF:FF:FF:FF:FF:FF", "FF:FF:FF:FF:FF:FF",  true)]
  public void TestEquals(string x, string y, bool expected)
    => Assert.AreEqual(expected, Node.Parse(x).Equals(Node.Parse(y)));

  [Test]
  public void TestEquals_Object()
  {
    Assert.IsTrue(Node.Parse("00:00:00:00:00:00").Equals((object)Node.Parse("00:00:00:00:00:00")), "#1");
    Assert.IsFalse(Node.Parse("00:00:00:00:00:00").Equals((object)Node.Parse("00:00:00:00:00:01")), "#2");
    Assert.IsFalse(Node.Parse("00:00:00:00:00:00").Equals(null), "#3");
    Assert.IsFalse(Node.Parse("00:00:00:00:00:00").Equals("00:00:00:00:00:00"), "#4");
    Assert.IsFalse(Node.Parse("00:00:00:00:00:00").Equals(0), "#5");
  }
}
