// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class ParamArrayUtilsTests {
    [Test]
    public void TestToEnumerable_ReferenceType()
    {
      Assert.That(ParamArrayUtils.ToEnumerable("foo"), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerable("foo", "bar"), Is.EqualTo(new[] { "foo", "bar" }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerable("foo", (string)null), Is.EqualTo(new[] { "foo", null }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerable("foo", "bar", (string)null), Is.EqualTo(new[] { "foo", "bar", null }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerable("foo", (string)null, "bar"), Is.EqualTo(new[] { "foo", null, "bar" }).AsCollection);

      Assert.That(ParamArrayUtils.ToEnumerable("foo", (string[])null), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerable("foo", new string[0]), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerable("foo", new[] { "bar" }), Is.EqualTo(new[] { "foo", "bar" }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerable("foo", new[] { null, "bar" }), Is.EqualTo(new[] { "foo", null, "bar" }).AsCollection);
    }

    [Test]
    public void TestToEnumerable_ValueType()
    {
      Assert.That(ParamArrayUtils.ToEnumerable(0), Is.EqualTo(new[] { 0 }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerable(0, 1), Is.EqualTo(new[] { 0, 1 }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerable(0, 1, 2), Is.EqualTo(new[] { 0, 1, 2 }).AsCollection);

      Assert.That(ParamArrayUtils.ToEnumerable(0, (int[])null), Is.EqualTo(new[] { 0 }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerable(0, new int[0]), Is.EqualTo(new[] { 0 }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerable(0, new[] { 1 }), Is.EqualTo(new[] { 0, 1 }).AsCollection);
    }

    [Test]
    public void TestToEnumerableNonNullable()
    {
      const string paramName = "param";

      Assert.That(ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo"), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", "bar"), Is.EqualTo(new[] { "foo", "bar" }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", "bar", "baz"), Is.EqualTo(new[] { "foo", "bar", "baz" }).AsCollection);

      Assert.That(ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", (string[])null), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", new string[0]), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", new[] { "bar" }), Is.EqualTo(new[] { "foo", "bar" }).AsCollection);
    }

    [Test]
    public void TestToEnumerableNonNullable_ContainsNull()
    {
      const string paramName = "param";
      ArgumentException ex;

      ex = Assert.Throws<ArgumentException>(() => ParamArrayUtils.ToEnumerableNonNullable(paramName, (string)null).ToList());
      Assert.That(ex!.ParamName, Is.EqualTo(paramName));

      ex = Assert.Throws<ArgumentException>(() => ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", (string)null).ToList());
      Assert.That(ex!.ParamName, Is.EqualTo(paramName));

      ex = Assert.Throws<ArgumentException>(() => ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", new string[] { null }).ToList());
      Assert.That(ex!.ParamName, Is.EqualTo(paramName));
    }

    [Test]
    public void TestToList_ReferenceType()
    {
      Assert.That(ParamArrayUtils.ToList("foo"), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToList("foo", "bar"), Is.EqualTo(new[] { "foo", "bar" }).AsCollection);
      Assert.That(ParamArrayUtils.ToList("foo", (string)null), Is.EqualTo(new[] { "foo", null }).AsCollection);
      Assert.That(ParamArrayUtils.ToList("foo", "bar", (string)null), Is.EqualTo(new[] { "foo", "bar", null }).AsCollection);
      Assert.That(ParamArrayUtils.ToList("foo", (string)null, "bar"), Is.EqualTo(new[] { "foo", null, "bar" }).AsCollection);

      Assert.That(ParamArrayUtils.ToList("foo", (string[])null), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToList("foo", new string[0]), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToList("foo", new[] { "bar" }), Is.EqualTo(new[] { "foo", "bar" }).AsCollection);
      Assert.That(ParamArrayUtils.ToList("foo", new[] { null, "bar" }), Is.EqualTo(new[] { "foo", null, "bar" }).AsCollection);
    }

    [Test]
    public void TestToList_ValueType()
    {
      Assert.That(ParamArrayUtils.ToList(0), Is.EqualTo(new[] { 0 }).AsCollection);
      Assert.That(ParamArrayUtils.ToList(0, 1), Is.EqualTo(new[] { 0, 1 }).AsCollection);
      Assert.That(ParamArrayUtils.ToList(0, 1, 2), Is.EqualTo(new[] { 0, 1, 2 }).AsCollection);

      Assert.That(ParamArrayUtils.ToList(0, (int[])null), Is.EqualTo(new[] { 0 }).AsCollection);
      Assert.That(ParamArrayUtils.ToList(0, new int[0]), Is.EqualTo(new[] { 0 }).AsCollection);
      Assert.That(ParamArrayUtils.ToList(0, new[] { 1 }), Is.EqualTo(new[] { 0, 1 }).AsCollection);
    }

    [Test]
    public void TestToListNonNullable()
    {
      const string paramName = "param";

      Assert.That(ParamArrayUtils.ToListNonNullable(paramName, "foo"), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToListNonNullable(paramName, "foo", "bar"), Is.EqualTo(new[] { "foo", "bar" }).AsCollection);
      Assert.That(ParamArrayUtils.ToListNonNullable(paramName, "foo", "bar", "baz"), Is.EqualTo(new[] { "foo", "bar", "baz" }).AsCollection);

      Assert.That(ParamArrayUtils.ToListNonNullable(paramName, "foo", (string[])null), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToListNonNullable(paramName, "foo", new string[0]), Is.EqualTo(new[] { "foo" }).AsCollection);
      Assert.That(ParamArrayUtils.ToListNonNullable(paramName, "foo", new[] { "bar" }), Is.EqualTo(new[] { "foo", "bar" }).AsCollection);
    }

    [Test]
    public void TestToListNonNullable_ContainsNull()
    {
      const string paramName = "param";
      ArgumentException ex;

      ex = Assert.Throws<ArgumentException>(() => ParamArrayUtils.ToListNonNullable(paramName, (string)null).ToList());
      Assert.That(ex!.ParamName, Is.EqualTo(paramName));

      ex = Assert.Throws<ArgumentException>(() => ParamArrayUtils.ToListNonNullable(paramName, "foo", (string)null).ToList());
      Assert.That(ex!.ParamName, Is.EqualTo(paramName));

      ex = Assert.Throws<ArgumentException>(() => ParamArrayUtils.ToListNonNullable(paramName, "foo", new string[] { null }).ToList());
      Assert.That(ex!.ParamName, Is.EqualTo(paramName));
    }
  }
}
