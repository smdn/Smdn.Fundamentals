// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_IEQUALITYOPERATORS
using System.Numerics;
#endif
using NUnit.Framework;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

#pragma warning disable IDE0040
partial class NodeTests {
#pragma warning restore IDE0040
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
#pragma warning disable CA1305
    Assert.That(Node.Parse(x) == Node.Parse(y), Is.EqualTo(isEqual), "op ==");
    Assert.That(Node.Parse(x) != Node.Parse(y), Is.EqualTo(!isEqual), "op !=");

#if SYSTEM_NUMERICS_IEQUALITYOPERATORS
    Assert.That(OpEquality(Node.Parse(x), Node.Parse(y)), Is.EqualTo(isEqual), "IEqualityOperators ==");
    Assert.That(OpInequality(Node.Parse(x), Node.Parse(y)), Is.EqualTo(!isEqual), "IEqualityOperators !=");

    static bool OpEquality<T>(T x, T y) where T : IEqualityOperators<T, T, bool> => x == y;
    static bool OpInequality<T>(T x, T y) where T : IEqualityOperators<T, T, bool> => x != y;
#endif
#pragma warning restore CA1305
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
#pragma warning disable CA1305
    => Assert.That(Node.Parse(x).Equals(Node.Parse(y)), Is.EqualTo(expected));
#pragma warning restore CA1305

  [Test]
  public void TestEquals_Object()
  {
#pragma warning disable CA1305
    Assert.That(Node.Parse("00:00:00:00:00:00").Equals((object)Node.Parse("00:00:00:00:00:00")), Is.True, "#1");
    Assert.That(Node.Parse("00:00:00:00:00:00").Equals((object)Node.Parse("00:00:00:00:00:01")), Is.False, "#2");
    Assert.That(Node.Parse("00:00:00:00:00:00").Equals(null), Is.False, "#3");
    Assert.That(Node.Parse("00:00:00:00:00:00").Equals("00:00:00:00:00:00"), Is.False, "#4");
    Assert.That(Node.Parse("00:00:00:00:00:00").Equals(0), Is.False, "#5");
#pragma warning restore CA1305
  }
}
