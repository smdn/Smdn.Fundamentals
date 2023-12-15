// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public class ArrayShimTests {
  [Test]
  public void TestShimType_Empty()
    => Assert.That(
#if SYSTEM_ARRAY_EMPTY
      typeof(System.Array)
#else
      typeof(Smdn.ArrayShim)
#endif
      ,
      Is.EqualTo(typeof(ShimTypeSystemArrayEmpty))
    );

  [Test]
  public void TestEmpty()
  {
    var empty = ShimTypeSystemArrayEmpty.Empty<int>();

    Assert.That(empty, Is.Not.Null);
    Assert.That(empty.GetType(), Is.EqualTo(typeof(int[])));
    Assert.That(empty, Is.Empty);
  }

  [Test]
  public void TestShimType_ConvertAll()
    => Assert.That(
#if SYSTEM_ARRAY_CONVERTALL
      typeof(System.Array)
#else
      typeof(Smdn.ArrayShim)
#endif
      ,
      Is.EqualTo(typeof(ShimTypeSystemArrayConvertAll))
    );

  [Test]
  public void TestConvertAll()
  {
    var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    var expected = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    Assert.That(
      ShimTypeSystemArrayConvertAll.ConvertAll(array, i => i.ToString("D", null)),
      Is.EqualTo(expected).AsCollection
    );
  }

  [Test]
  public void TestConvertAll_ArgumentArrayEmpty()
  {
    Assert.That(
      ShimTypeSystemArrayConvertAll.ConvertAll(Enumerable.Empty<int>().ToArray(), i => i),
      Is.Empty
    );
  }

  [Test]
  public void TestConvertAll_ArgumentArrayNull()
  {
    int[] array = null!;

    Assert.Throws<ArgumentNullException>(() => ShimTypeSystemArrayConvertAll.ConvertAll(array, i => i));
  }

  [Test]
  public void TestConvertAll_ArgumentConverterNull()
  {
    var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
#if SYSTEM_CONVERTER
    Converter<int, long> converter = null!;
#else
    Func<int, long> converter = null!;
#endif

    Assert.Throws<ArgumentNullException>(() => ShimTypeSystemArrayConvertAll.ConvertAll(array, converter));
  }
}
