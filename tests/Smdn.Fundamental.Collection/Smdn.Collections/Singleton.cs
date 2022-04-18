// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using NUnit.Framework;
using Assert = Smdn.Test.NUnit.Assertion.Assert;

namespace Smdn.Collections {
  [TestFixture]
  public class SingletonTests {
    [Test]
    public void TestCreateList_ValueType()
    {
      foreach (var list in new[] {
        Singleton.CreateList(42),
        new List<int> { 42 },
      }) {
        Assert.AreEqual(42, list[0]);
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(1, list.Count());
        Assert.IsNotEmpty(list);
        CollectionAssert.AreEqual(new[] { 42 }, list);
      }
    }

    [Test]
    public void TestCreateList_ReferenceType()
    {
      foreach (var list in new[] {
        Singleton.CreateList("foo"),
        new List<string> { "foo" },
      }) {
        Assert.AreEqual("foo", list[0]);
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(1, list.Count());
        Assert.IsNotEmpty(list);
        CollectionAssert.AreEqual(new[] { "foo" }, list);
      }
    }

    [Test]
    public void TestCreateList_ReferenceType_ElementNull()
    {
      foreach (var list in new[] {
        Singleton.CreateList<string>(null),
        new List<string> { null },
      }) {
        Assert.IsNull(list[0]);
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(1, list.Count());
        Assert.IsNotEmpty(list);
        CollectionAssert.AreEqual(new string[] { null }, list);
      }
    }

    [Test]
    public void TestCreateList_IndexOutOfRange()
    {
      foreach (var list in new[] {
        Singleton.CreateList(42),
        new List<int> { 42 },
      }) {
        var ex1 = Assert.Throws<ArgumentOutOfRangeException>(() => Assert.IsNotNull(list[1]));

        Assert.AreEqual("index", ex1.ParamName);
        //Assert.AreEqual(1, ex1.ActualValue);

        var ex2 = Assert.Throws<ArgumentOutOfRangeException>(() => Assert.IsNotNull(list[-1]));

        Assert.AreEqual("index", ex2.ParamName);
        //Assert.AreEqual(-1, ex2.ActualValue);
      }
    }

    [Test]
    public void TestBinarySerialization()
    {
      foreach (var test in new[] {
        new {List = Singleton.CreateList(42), Test = "Singleton"},
        new {List = (IReadOnlyList<int>)new List<int> { 42 }, Test = "List<T>"},
      }) {
        Assert.IsSerializable(test.List, deserialized => {
          Assert.AreNotSame(test.List, deserialized, test.Test);
          CollectionAssert.AreEqual(test.List, deserialized, test.Test);
        });
      }
    }
  }
}
