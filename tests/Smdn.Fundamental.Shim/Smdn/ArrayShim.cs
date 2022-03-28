// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public class ArrayShimTests {
  [Test]
  public void TestEmpty()
  {
    var empty = ShimTypeSystemArrayEmpty.Empty<int>();

    Assert.IsNotNull(empty);
    Assert.AreEqual(typeof(int[]), empty.GetType());
    CollectionAssert.IsEmpty(empty);
  }

  [Test]
  public void TestConvertAll()
  {
    var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    var expected = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    CollectionAssert.AreEqual(
      expected,
      ShimTypeSystemArrayConvertAll.ConvertAll(array, i => i.ToString("D"))
    );
  }

  [Test]
  public void TestConvertAll_ArgumentArrayEmpty()
  {
    Assert.IsEmpty(
      ShimTypeSystemArrayConvertAll.ConvertAll(Enumerable.Empty<int>().ToArray(), i => i)
    );
  }

  [Test]
  public void TestConvertAll_ArgumentArrayNull()
  {
    int[] array = null;

    Assert.Throws<ArgumentNullException>(() => ShimTypeSystemArrayConvertAll.ConvertAll(array, i => i));
  }

  [Test]
  public void TestConvertAll_ArgumentConverterNull()
  {
    var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
#if SYSTEM_CONVERTER
    Converter<int, long> converter = null;
#else
    Func<int, long> converter = null;
#endif

    Assert.Throws<ArgumentNullException>(() => ShimTypeSystemArrayConvertAll.ConvertAll(array, converter));
  }
}
