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
        Assert.That(list[0], Is.EqualTo(42));
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list.Count(), Is.EqualTo(1));
        Assert.That(list, Is.Not.Empty);
        Assert.That(list, Is.EqualTo(new[] { 42 }).AsCollection);
      }
    }

    [Test]
    public void TestCreateList_ReferenceType()
    {
      foreach (var list in new[] {
        Singleton.CreateList("foo"),
        new List<string> { "foo" },
      }) {
        Assert.That(list[0], Is.EqualTo("foo"));
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list.Count(), Is.EqualTo(1));
        Assert.That(list, Is.Not.Empty);
        Assert.That(list, Is.EqualTo(new[] { "foo" }).AsCollection);
      }
    }

    [Test]
    public void TestCreateList_ReferenceType_ElementNull()
    {
      foreach (var list in new[] {
        Singleton.CreateList<string>(null),
        new List<string> { null },
      }) {
        Assert.That(list[0], Is.Null);
        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list.Count(), Is.EqualTo(1));
        Assert.That(list, Is.Not.Empty);
        Assert.That(list, Is.EqualTo(new string[] { null }).AsCollection);
      }
    }

    [Test]
    public void TestCreateList_IndexOutOfRange()
    {
      foreach (var list in new[] {
        Singleton.CreateList(42),
        new List<int> { 42 },
      }) {
        var ex1 = Assert.Throws<ArgumentOutOfRangeException>(() => Assert.That(list[1], Is.Zero));

        Assert.That(ex1!.ParamName, Is.EqualTo("index"));
        //Assert.AreEqual(1, ex1.ActualValue);

        var ex2 = Assert.Throws<ArgumentOutOfRangeException>(() => Assert.That(list[-1], Is.Zero));

        Assert.That(ex2!.ParamName, Is.EqualTo("index"));
        //Assert.AreEqual(-1, ex2.ActualValue);
      }
    }

#if !NET8_0_OR_GREATER
    [Test]
    public void TestBinarySerialization()
    {
      foreach (var test in new[] {
        new {List = Singleton.CreateList(42), Test = "Singleton"},
        new {List = (IReadOnlyList<int>)new List<int> { 42 }, Test = "List<T>"},
      }) {
        Assert.IsSerializable(test.List, deserialized => {
          Assert.That(deserialized, Is.Not.SameAs(test.List), test.Test);
          Assert.That(deserialized, Is.EqualTo(test.List).AsCollection, test.Test);
        });
      }
    }
#endif
  }
}
