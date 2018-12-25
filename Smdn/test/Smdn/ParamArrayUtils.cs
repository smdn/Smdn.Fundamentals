using System;
using System.Linq;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class ParamArrayUtilsTests {
    [Test]
    public void TestToEnumerable_ReferenceType()
    {
      CollectionAssert.AreEqual(new[] { "foo" }, ParamArrayUtils.ToEnumerable("foo"));
      CollectionAssert.AreEqual(new[] { "foo", "bar" }, ParamArrayUtils.ToEnumerable("foo", "bar"));
      CollectionAssert.AreEqual(new[] { "foo", null }, ParamArrayUtils.ToEnumerable("foo", (string)null));
      CollectionAssert.AreEqual(new[] { "foo", "bar", null }, ParamArrayUtils.ToEnumerable("foo", "bar", (string)null));
      CollectionAssert.AreEqual(new[] { "foo", null, "bar" }, ParamArrayUtils.ToEnumerable("foo", (string)null, "bar"));

      CollectionAssert.AreEqual(new[] { "foo" }, ParamArrayUtils.ToEnumerable("foo", (string[])null));
      CollectionAssert.AreEqual(new[] { "foo", "bar" }, ParamArrayUtils.ToEnumerable("foo", new[] { "bar" }));
      CollectionAssert.AreEqual(new[] { "foo", null, "bar" }, ParamArrayUtils.ToEnumerable("foo", new[] { null, "bar" }));
    }

    [Test]
    public void TestToEnumerable_ValueType()
    {
      CollectionAssert.AreEqual(new[] { 0 }, ParamArrayUtils.ToEnumerable(0));
      CollectionAssert.AreEqual(new[] { 0, 1 }, ParamArrayUtils.ToEnumerable(0, 1));
      CollectionAssert.AreEqual(new[] { 0, 1, 2 }, ParamArrayUtils.ToEnumerable(0, 1, 2));

      CollectionAssert.AreEqual(new[] { 0 }, ParamArrayUtils.ToEnumerable(0, (int[])null));
      CollectionAssert.AreEqual(new[] { 0, 1 }, ParamArrayUtils.ToEnumerable(0, new[] { 1 }));
    }

    [Test]
    public void TestToEnumerableNonNullable()
    {
      const string paramName = "param";

      CollectionAssert.AreEqual(new[] { "foo" }, ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo"));
      CollectionAssert.AreEqual(new[] { "foo", "bar" }, ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", "bar"));
      CollectionAssert.AreEqual(new[] { "foo", "bar", "baz" }, ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", "bar", "baz"));

      CollectionAssert.AreEqual(new[] { "foo" }, ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", (string[])null));
      CollectionAssert.AreEqual(new[] { "foo", "bar" }, ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", new[] { "bar" }));
    }

    [Test]
    public void TestToEnumerableNonNullable_ContainsNull()
    {
      const string paramName = "param";
      ArgumentException ex;

      ex = Assert.Throws<ArgumentException>(() => ParamArrayUtils.ToEnumerableNonNullable(paramName, (string)null).ToList());
      Assert.AreEqual(paramName, ex.ParamName);

      ex = Assert.Throws<ArgumentException>(() => ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", (string)null).ToList());
      Assert.AreEqual(paramName, ex.ParamName);

      ex = Assert.Throws<ArgumentException>(() => ParamArrayUtils.ToEnumerableNonNullable(paramName, "foo", new string[] { null }).ToList());
      Assert.AreEqual(paramName, ex.ParamName);
    }
  }
}
