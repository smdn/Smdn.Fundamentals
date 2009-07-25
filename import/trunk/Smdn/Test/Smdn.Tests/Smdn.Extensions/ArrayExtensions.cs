using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn.Extensions {
  [TestFixture()]
  public class ArrayExtensionsTests {
    [Test]
    public void TestConcat()
    {
      var arr1 = new[] {0, 1, 2};
      var arr2 = new[] {3, 4, 5};
      var arr3 = new[] {6, 7, 8};

      Assert.AreEqual(new[] {0, 1, 2, 3, 4, 5}, arr1.Concat(arr2));
      Assert.AreEqual(new[] {0, 1, 2, 3, 4, 5, 6, 7, 8}, arr1.Concat(arr2, arr3));
    }

    [Test]
    public void TestSlice()
    {
      var array = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

      Assert.AreEqual(new[] {0, 1, 2}, array.Slice(0, 3));
      Assert.AreEqual(new[] {7, 8, 9}, array.Slice(7, 3));
      Assert.AreEqual(new[] {2, 3, 4, 5, 6}, array.Slice(2, 5));
      Assert.AreEqual(new[] {6, 7, 8, 9}, array.Slice(6));
    }

    [Test]
    public void TestIsEqualsToWithIEquatable()
    {
      var array1 = new[] {0, 1, 2, 3, 4, 5};
      var array2 = new[] {0, 1, 2, 3, 4, 5};
      var array3 = new[] {0, 1, 2, 3, 4};
      var array4 = new[] {0, 1, 2, 3, 4, 6};

      Assert.IsFalse(ArrayExtensions.IsEqualsTo(array1, null), "compare with null 1");
      Assert.IsFalse(ArrayExtensions.IsEqualsTo(null, array1), "compare with null 2");

      Assert.IsTrue(array1.IsEqualsTo(array2), "different instance, same elements 1");
      Assert.IsTrue(array2.IsEqualsTo(array1), "different instance, same elements 2");

      Assert.IsFalse(array1.IsEqualsTo(array3), "different length 1");
      Assert.IsFalse(array3.IsEqualsTo(array1), "different length 2");

      Assert.IsFalse(array1.IsEqualsTo(array4), "different element 1");
      Assert.IsFalse(array4.IsEqualsTo(array1), "different element 2");
    }

    [Test]
    public void TestIsEqualsToWithIEqualityComaparer()
    {
      var array1 = new[] {0, 1, 2, 3, 4, 5};
      var array2 = new[] {0, 1, 2, 3, 4, 5};
      var array3 = new[] {0, 1, 2, 3, 4};
      var array4 = new[] {0, 1, 2, 3, 4, 6};

      Assert.IsFalse(ArrayExtensions.IsEqualsTo(array1, null, EqualityComparer<int>.Default), "compare with null 1");
      Assert.IsFalse(ArrayExtensions.IsEqualsTo(null, array1, EqualityComparer<int>.Default), "compare with null 2");

      Assert.IsTrue(array1.IsEqualsTo(array2, EqualityComparer<int>.Default), "different instance, same elements 1");
      Assert.IsTrue(array2.IsEqualsTo(array1, EqualityComparer<int>.Default), "different instance, same elements 2");

      Assert.IsFalse(array1.IsEqualsTo(array3, EqualityComparer<int>.Default), "different length 1");
      Assert.IsFalse(array3.IsEqualsTo(array1, EqualityComparer<int>.Default), "different length 2");

      Assert.IsFalse(array1.IsEqualsTo(array4, EqualityComparer<int>.Default), "different element 1");
      Assert.IsFalse(array4.IsEqualsTo(array1, EqualityComparer<int>.Default), "different element 2");
    }
  }
}