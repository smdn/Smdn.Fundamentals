using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class ArrayExtensionsTests {
    [Test]
    public void TestAppend()
    {
      var arr = new[] {0, 1, 2};

      Assert.AreEqual(new[] {0, 1, 2, 3}, arr.Append(3));
      Assert.AreEqual(new[] {0, 1, 2, 3, 4, 5}, arr.Append(3, 4, 5));

      arr = new int[] {};

      Assert.AreEqual(new[] {9}, arr.Append(9));
    }

    [Test]
    public void TestPrepend()
    {
      var arr = new[] {3, 4, 5};

      Assert.AreEqual(new[] {0, 3, 4, 5}, arr.Prepend(0));
      Assert.AreEqual(new[] {0, 1, 2, 3, 4, 5}, arr.Prepend(0, 1, 2));

      arr = new int[] {};

      Assert.AreEqual(new[] {9}, arr.Prepend(9));
    }

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
      Assert.AreEqual(new int[] {}, array.Slice(0, 0));
      Assert.AreEqual(new int[] {}, array.Slice(10, 0));
      Assert.AreEqual(new int[] {}, (new int[] {0}).Slice(1));
    }

    [Test]
    public void TestSliceCheckRange()
    {
      var array = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

      try {
        array.Slice(-1, 1);
        Assert.Fail("ArgumentOutOfRangeException not thrown");
      }
      catch (ArgumentOutOfRangeException) {
      }

      try {
        array.Slice(1, -1);
        Assert.Fail("ArgumentOutOfRangeException not thrown");
      }
      catch (ArgumentOutOfRangeException) {
      }

      try {
        array.Slice(11, 0);
        Assert.Fail("ArgumentException not thrown");
      }
      catch (ArgumentException) {
      }

      try {
        array.Slice(0, 11);
        Assert.Fail("ArgumentException not thrown");
      }
      catch (ArgumentException) {
      }
    }

    [Test]
    public void TestShuffle()
    {
      var array0 = new int[] {};
      var shuffle0 = ArrayExtensions.Shuffle(array0);

      Assert.IsFalse(object.ReferenceEquals(array0, shuffle0));
      Assert.AreEqual(array0, shuffle0);

      var array1 = new int[] {0};
      var shuffle1 = ArrayExtensions.Shuffle(array1);

      Assert.IsFalse(object.ReferenceEquals(array1, shuffle1));
      Assert.AreEqual(array1, shuffle1);

      var array = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

      Assert.IsFalse(object.ReferenceEquals(array, ArrayExtensions.Shuffle(array)));

      for (var act = 0; act < 10; act++) {
        if (ArrayExtensions.EqualsAll(array, ArrayExtensions.Shuffle(array)))
          if (ArrayExtensions.EqualsAll(array, ArrayExtensions.Shuffle(array)))
            Assert.Fail();
      }
    }

    private class Sequencial : Random {
      public override int Next(int maxValue)
      {
        return maxValue - 1;
      }

      public override int Next(int minValue, int maxValue)
      {
        return maxValue - 1;
      }
    }

    [Test]
    public void TestShuffleWithSpecifiedRandom()
    {
      var array = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

      Assert.AreEqual(array, ArrayExtensions.Shuffle(array, new Sequencial()));
    }

    [Test]
    public void TestEqualsAllWithIEquatable()
    {
      var array1 = new[] {0, 1, 2, 3, 4, 5};
      var array2 = new[] {0, 1, 2, 3, 4, 5};
      var array3 = new[] {0, 1, 2, 3, 4};
      var array4 = new[] {0, 1, 2, 3, 4, 6};

      Assert.IsTrue(ArrayExtensions.EqualsAll((int[])null, (int[])null), "compare with null and null");
      Assert.IsFalse(ArrayExtensions.EqualsAll(array1, null), "compare with null 1");
      Assert.IsFalse(ArrayExtensions.EqualsAll(null, array1), "compare with null 2");

      Assert.IsTrue(array1.EqualsAll(array2), "different instance, same elements 1");
      Assert.IsTrue(array2.EqualsAll(array1), "different instance, same elements 2");

      Assert.IsFalse(array1.EqualsAll(array3), "different length 1");
      Assert.IsFalse(array3.EqualsAll(array1), "different length 2");

      Assert.IsFalse(array1.EqualsAll(array4), "different element 1");
      Assert.IsFalse(array4.EqualsAll(array1), "different element 2");
    }

    [Test]
    public void TestEqualsAllWithIEqualityComaparer()
    {
      var array1 = new[] {0, 1, 2, 3, 4, 5};
      var array2 = new[] {0, 1, 2, 3, 4, 5};
      var array3 = new[] {0, 1, 2, 3, 4};
      var array4 = new[] {0, 1, 2, 3, 4, 6};

      Assert.IsTrue(ArrayExtensions.EqualsAll((int[])null, (int[])null, EqualityComparer<int>.Default), "compare with null and null");
      Assert.IsFalse(ArrayExtensions.EqualsAll(array1, null, EqualityComparer<int>.Default), "compare with null 1");
      Assert.IsFalse(ArrayExtensions.EqualsAll(null, array1, EqualityComparer<int>.Default), "compare with null 2");

      Assert.IsTrue(array1.EqualsAll(array2, EqualityComparer<int>.Default), "different instance, same elements 1");
      Assert.IsTrue(array2.EqualsAll(array1, EqualityComparer<int>.Default), "different instance, same elements 2");

      Assert.IsFalse(array1.EqualsAll(array3, EqualityComparer<int>.Default), "different length 1");
      Assert.IsFalse(array3.EqualsAll(array1, EqualityComparer<int>.Default), "different length 2");

      Assert.IsFalse(array1.EqualsAll(array4, EqualityComparer<int>.Default), "different element 1");
      Assert.IsFalse(array4.EqualsAll(array1, EqualityComparer<int>.Default), "different element 2");
    }
  }
}