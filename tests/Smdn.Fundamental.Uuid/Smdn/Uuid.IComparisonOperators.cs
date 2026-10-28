// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NUMERICS_ICOMPARISONOPERATORS
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

partial class UuidTests {
  [Test]
  public void TestCompareTo()
  {
    Assert.That(Uuid.RFC4122NamespaceDns.CompareTo(Uuid.RFC4122NamespaceDns), Is.Zero);
    Assert.That(Uuid.RFC4122NamespaceDns.CompareTo((Guid)Uuid.RFC4122NamespaceDns), Is.Zero);
    Assert.That(Uuid.RFC4122NamespaceDns.CompareTo((object)Uuid.RFC4122NamespaceDns), Is.Zero);
    Assert.That(Uuid.RFC4122NamespaceDns.CompareTo(null), Is.EqualTo(1));
    Assert.That(Uuid.RFC4122NamespaceDns.CompareTo(Guid.Empty), Is.Not.Zero);

    Assert.Throws<ArgumentException>(() => Uuid.RFC4122NamespaceDns.CompareTo(1));

    var ux = new Uuid("00000000-0000-0000-0000-000000000000");
    var uy = new Uuid("00000001-0000-0000-0000-000000000000");
    var gx = new Guid("00000000-0000-0000-0000-000000000000");
    var gy = new Guid("00000001-0000-0000-0000-000000000000");

    Assert.That(ux.CompareTo(ux) == 0);
    Assert.That(ux.CompareTo(gx) == 0);
    Assert.That(uy.CompareTo(uy) == 0);
    Assert.That(gy.CompareTo(gy) == 0);

    Assert.That(ux.CompareTo(uy) < 0);
    Assert.That(ux.CompareTo(gy) < 0);
    Assert.That(0 < uy.CompareTo(ux));
    Assert.That(0 < uy.CompareTo(gx));

    ux = new Uuid("00000000-0000-0000-0000-000000000000");
    uy = new Uuid("00000000-0000-0000-0000-000000000001");
    gx = new Guid("00000000-0000-0000-0000-000000000000");
    gy = new Guid("00000000-0000-0000-0000-000000000001");

    Assert.That(ux.CompareTo(ux) == 0);
    Assert.That(ux.CompareTo(gx) == 0);
    Assert.That(uy.CompareTo(uy) == 0);
    Assert.That(gy.CompareTo(gy) == 0);

    Assert.That(ux.CompareTo(uy) < 0);
    Assert.That(ux.CompareTo(gy) < 0);
    Assert.That(0 < uy.CompareTo(ux));
    Assert.That(0 < uy.CompareTo(gx));
  }

  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000", false,  true)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000001",  true,  true)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0001-000000000000",  true,  true)]
  [TestCase("00000000-0000-0000-0000-FFFFFFFFFFFF", "00000000-0000-0000-0001-000000000000",  true,  true)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0100-000000000000",  true,  true)]
  [TestCase("00000000-0000-0000-00FF-FFFFFFFFFFFF", "00000000-0000-0000-0100-000000000000",  true,  true)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0001-0000-000000000000",  true,  true)]
  [TestCase("00000000-0000-0000-FFFF-FFFFFFFFFFFF", "00000000-0000-0001-0000-000000000000",  true,  true)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0001-0000-0000-000000000000",  true,  true)]
  [TestCase("00000000-0000-FFFF-FFFF-FFFFFFFFFFFF", "00000000-0001-0000-0000-000000000000",  true,  true)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000001-0000-0000-0000-000000000000",  true,  true)]
  [TestCase("00000000-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "00000001-0000-0000-0000-000000000000",  true,  true)]
  [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000000", false, false)]
  [TestCase("00000000-0000-0000-0001-000000000000", "00000000-0000-0000-0000-000000000000", false, false)]
  [TestCase("00000000-0000-0000-0001-000000000000", "00000000-0000-0000-0000-FFFFFFFFFFFF", false, false)]
  [TestCase("00000000-0000-0000-0100-000000000000", "00000000-0000-0000-0000-000000000000", false, false)]
  [TestCase("00000000-0000-0000-0100-000000000000", "00000000-0000-0000-00FF-FFFFFFFFFFFF", false, false)]
  [TestCase("00000000-0000-0001-0000-000000000000", "00000000-0000-0000-0000-000000000000", false, false)]
  [TestCase("00000000-0000-0001-0000-000000000000", "00000000-0000-0000-FFFF-FFFFFFFFFFFF", false, false)]
  [TestCase("00000000-0001-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000", false, false)]
  [TestCase("00000000-0001-0000-0000-000000000000", "00000000-0000-FFFF-FFFF-FFFFFFFFFFFF", false, false)]
  [TestCase("00000001-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000", false, false)]
  [TestCase("00000001-0000-0000-0000-000000000000", "00000000-FFFF-FFFF-FFFF-FFFFFFFFFFFF", false, false)]
  [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", false,  true)]
  public void TestOpLessThan_OpLessThanOrEqual(string x, string y, bool lessThan, bool lessThanOrEqual)
  {
    Assert.That(new Uuid(x) < new Uuid(y), Is.EqualTo(lessThan), "op <");
    Assert.That(new Uuid(x) <= new Uuid(y), Is.EqualTo(lessThanOrEqual), "op <=");

#if SYSTEM_NUMERICS_ICOMPARISONOPERATORS
    Assert.That(OpLessThan(new Uuid(x), new Uuid(y)), Is.EqualTo(lessThan), "IComparisonOperators <");
    Assert.That(OpLessThanOrEqual(new Uuid(x), new Uuid(y)), Is.EqualTo(lessThanOrEqual), "IComparisonOperators <=");

    static bool OpLessThan<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x < y;
    static bool OpLessThanOrEqual<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x <= y;
#endif
  }

  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000", false,  true)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000001", false, false)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0001-000000000000", false, false)]
  [TestCase("00000000-0000-0000-0000-FFFFFFFFFFFF", "00000000-0000-0000-0001-000000000000", false, false)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0100-000000000000", false, false)]
  [TestCase("00000000-0000-0000-00FF-FFFFFFFFFFFF", "00000000-0000-0000-0100-000000000000", false, false)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0000-0001-0000-000000000000", false, false)]
  [TestCase("00000000-0000-0000-FFFF-FFFFFFFFFFFF", "00000000-0000-0001-0000-000000000000", false, false)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000000-0001-0000-0000-000000000000", false, false)]
  [TestCase("00000000-0000-FFFF-FFFF-FFFFFFFFFFFF", "00000000-0001-0000-0000-000000000000", false, false)]
  [TestCase("00000000-0000-0000-0000-000000000000", "00000001-0000-0000-0000-000000000000", false, false)]
  [TestCase("00000000-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "00000001-0000-0000-0000-000000000000", false, false)]
  [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000000",  true,  true)]
  [TestCase("00000000-0000-0000-0001-000000000000", "00000000-0000-0000-0000-000000000000",  true,  true)]
  [TestCase("00000000-0000-0000-0001-000000000000", "00000000-0000-0000-0000-FFFFFFFFFFFF",  true,  true)]
  [TestCase("00000000-0000-0000-0100-000000000000", "00000000-0000-0000-0000-000000000000",  true,  true)]
  [TestCase("00000000-0000-0000-0100-000000000000", "00000000-0000-0000-00FF-FFFFFFFFFFFF",  true,  true)]
  [TestCase("00000000-0000-0001-0000-000000000000", "00000000-0000-0000-0000-000000000000",  true,  true)]
  [TestCase("00000000-0000-0001-0000-000000000000", "00000000-0000-0000-FFFF-FFFFFFFFFFFF",  true,  true)]
  [TestCase("00000000-0001-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000",  true,  true)]
  [TestCase("00000000-0001-0000-0000-000000000000", "00000000-0000-FFFF-FFFF-FFFFFFFFFFFF",  true,  true)]
  [TestCase("00000001-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000",  true,  true)]
  [TestCase("00000001-0000-0000-0000-000000000000", "00000000-FFFF-FFFF-FFFF-FFFFFFFFFFFF",  true,  true)]
  [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", false,  true)]
  public void TestOpGreaterThan_OpGreaterThanOrEqual(string x, string y, bool greaterThan, bool greaterThanOrEqual)
  {
    Assert.That(new Uuid(x) > new Uuid(y), Is.EqualTo(greaterThan), "op >");
    Assert.That(new Uuid(x) >= new Uuid(y), Is.EqualTo(greaterThanOrEqual), "op >=");

#if SYSTEM_NUMERICS_ICOMPARISONOPERATORS
    Assert.That(OpGreaterThan(new Uuid(x), new Uuid(y)), Is.EqualTo(greaterThan), "IComparisonOperators >");
    Assert.That(OpGreaterThanOrEqual(new Uuid(x), new Uuid(y)),  Is.EqualTo(greaterThanOrEqual),"IComparisonOperators >=");

    static bool OpGreaterThan<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x > y;
    static bool OpGreaterThanOrEqual<T>(T x, T y) where T : IComparisonOperators<T, T, bool> => x >= y;
#endif
  }
}
