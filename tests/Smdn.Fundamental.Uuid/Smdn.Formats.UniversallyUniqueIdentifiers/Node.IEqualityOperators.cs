// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif
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
    Assert.That(Node.Parse(x) == Node.Parse(y), Is.EqualTo(isEqual), "op ==");
    Assert.That(Node.Parse(x) != Node.Parse(y), Is.EqualTo(!isEqual), "op !=");

#if FEATURE_GENERIC_MATH
    Assert.AreEqual(isEqual, OpEquality(Node.Parse(x), Node.Parse(y)), "IEqualityOperators ==");
    Assert.AreEqual(!isEqual, OpInequality(Node.Parse(x), Node.Parse(y)), "IEqualityOperators !=");

    static bool OpEquality<T>(T x, T y) where T : IEqualityOperators<T, T, bool> => x == y;
    static bool OpInequality<T>(T x, T y) where T : IEqualityOperators<T, T, bool> => x != y;
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
    => Assert.That(Node.Parse(x).Equals(Node.Parse(y)), Is.EqualTo(expected));

  [Test]
  public void TestEquals_Object()
  {
    Assert.That(Node.Parse("00:00:00:00:00:00").Equals((object)Node.Parse("00:00:00:00:00:00")), Is.True, "#1");
    Assert.That(Node.Parse("00:00:00:00:00:00").Equals((object)Node.Parse("00:00:00:00:00:01")), Is.False, "#2");
    Assert.That(Node.Parse("00:00:00:00:00:00").Equals(null), Is.False, "#3");
    Assert.That(Node.Parse("00:00:00:00:00:00").Equals("00:00:00:00:00:00"), Is.False, "#4");
    Assert.That(Node.Parse("00:00:00:00:00:00").Equals(0), Is.False, "#5");
  }
}
