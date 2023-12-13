// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class ArrayExtensionsTests {
    [Test]
    public void TestAppend()
    {
      var arr = new[] {0, 1, 2};

      Assert.That(arr.Append(3), Is.EqualTo(new[] {0, 1, 2, 3}));
      Assert.That(arr.Append(3, 4, 5), Is.EqualTo(new[] {0, 1, 2, 3, 4, 5}));

      arr = new int[] {};

      Assert.That(arr.Append(9), Is.EqualTo(new[] {9}));
    }

    [Test]
    public void TestPrepend()
    {
      var arr = new[] {3, 4, 5};

      Assert.That(arr.Prepend(0), Is.EqualTo(new[] {0, 3, 4, 5}));
      Assert.That(arr.Prepend(0, 1, 2), Is.EqualTo(new[] {0, 1, 2, 3, 4, 5}));

      arr = new int[] {};

      Assert.That(arr.Prepend(9), Is.EqualTo(new[] {9}));
    }

    [Test]
    public void TestConcat()
    {
      var arr1 = new[] {0, 1, 2};
      var arr2 = new[] {3, 4, 5};
      var arr3 = new[] {6, 7, 8};

      Assert.That(arr1.Concat(arr2), Is.EqualTo(new[] {0, 1, 2, 3, 4, 5}));
      Assert.That(arr1.Concat(arr2, arr3), Is.EqualTo(new[] {0, 1, 2, 3, 4, 5, 6, 7, 8}));
    }

    [Test]
    public void TestSlice()
    {
      var array = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

      Assert.That(array.Slice(0, 3), Is.EqualTo(new[] {0, 1, 2}));
      Assert.That(array.Slice(7, 3), Is.EqualTo(new[] {7, 8, 9}));
      Assert.That(array.Slice(2, 5), Is.EqualTo(new[] {2, 3, 4, 5, 6}));
      Assert.That(array.Slice(6), Is.EqualTo(new[] {6, 7, 8, 9}));
      Assert.That(array.Slice(0, 0), Is.EqualTo(new int[] {}));
      Assert.That(array.Slice(10, 0), Is.EqualTo(new int[] {}));
      Assert.That((new int[] {0}).Slice(1), Is.EqualTo(new int[] {}));
    }

    [Test]
    public void TestSliceCheckRange()
    {
      var array = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

      Assert.Throws<ArgumentOutOfRangeException>(() => array.Slice(-1, 1), "#1");
      Assert.Throws<ArgumentOutOfRangeException>(() => array.Slice(1, -1), "#2");

      Assert.Throws<ArgumentException>(() => array.Slice(11, 0), "#3");
      Assert.Throws<ArgumentException>(() => array.Slice(0, 11), "#4");
    }

    [Test]
    public void TestShuffle()
    {
      var array0 = new int[] {};
      var shuffle0 = ArrayExtensions.Shuffle(array0);

      Assert.That(object.ReferenceEquals(array0, shuffle0), Is.False);
      Assert.That(shuffle0, Is.EqualTo(array0));

      var array1 = new int[] {0};
      var shuffle1 = ArrayExtensions.Shuffle(array1);

      Assert.That(object.ReferenceEquals(array1, shuffle1), Is.False);
      Assert.That(shuffle1, Is.EqualTo(array1));

      var array = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

      Assert.That(object.ReferenceEquals(array, ArrayExtensions.Shuffle(array)), Is.False);

      for (var act = 0; act < 10; act++) {
        if (array.SequenceEqual(ArrayExtensions.Shuffle(array)))
          if (array.SequenceEqual(ArrayExtensions.Shuffle(array)))
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

      Assert.That(ArrayExtensions.Shuffle(array, new Sequencial()), Is.EqualTo(array));
    }

    [Test]
    public void TestRepeat_ArgumentNull()
    {
      int[] array = null;

      Assert.Throws<ArgumentNullException>(() => array.Repeat(0));
    }

    [TestCase(-1)]
    [TestCase(-2)]
    public void TestRepeat_CountOutOfRange(int count)
    {
      var array = new[] { 0 };

      Assert.Throws<ArgumentOutOfRangeException>(() => array.Repeat(count));
    }

    [Test]
    public void TestRepeat()
    {
      var array1 = new[] { 0 };

      Assert.That(array1.Repeat(0), Is.Empty, "#1-0");
      Assert.That(array1.Repeat(1), Is.EqualTo(new[] { 0 }).AsCollection, "#1-1");
      Assert.That(array1.Repeat(2), Is.EqualTo(new[] { 0, 0 }).AsCollection, "#1-2");
      Assert.That(array1.Repeat(3), Is.EqualTo(new[] { 0, 0, 0 }).AsCollection, "#1-3");

      var array2 = new[] { 0, 1 };

      Assert.That(array2.Repeat(0), Is.Empty, "#2-0");
      Assert.That(array2.Repeat(1), Is.EqualTo(new[] { 0, 1 }).AsCollection, "#2-1");
      Assert.That(array2.Repeat(2), Is.EqualTo(new[] { 0, 1, 0, 1 }).AsCollection, "#2-2");
      Assert.That(array2.Repeat(3), Is.EqualTo(new[] { 0, 1, 0, 1, 0, 1 }).AsCollection, "#2-3");

      var array3 = new[] { 0, 1, 2 };

      Assert.That(array3.Repeat(0), Is.Empty, "#3-0");
      Assert.That(array3.Repeat(1), Is.EqualTo(new[] { 0, 1, 2 }).AsCollection, "#3-1");
      Assert.That(array3.Repeat(2), Is.EqualTo(new[] { 0, 1, 2, 0, 1, 2 }).AsCollection, "#3-2");
      Assert.That(array3.Repeat(3), Is.EqualTo(new[] { 0, 1, 2, 0, 1, 2, 0, 1, 2 }).AsCollection, "#3-3");
    }

    [Test]
    public void TestRepeat_EmptyArray()
    {
      var array = new int[0];

      Assert.That(array.Repeat(0), Is.Empty);
      Assert.That(array.Repeat(1), Is.Empty);
      Assert.That(array.Repeat(2), Is.Empty);
    }
  }
}
