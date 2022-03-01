// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public class ArrayShimTests {
  [Test]
  public void TestConvert()
  {
    var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    var expected = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    CollectionAssert.AreEqual(expected,
                              array.Convert(i => i.ToString("D")));
  }

  [Test]
  public void TestConvert_ArgumentArrayEmpty()
  {
    Assert.IsEmpty(Enumerable.Empty<int>().ToArray().Convert(i => i));
  }

  [Test]
  public void TestConvert_ArgumentArrayNull()
  {
    int[] array = null;

    Assert.Throws<ArgumentNullException>(() => array.Convert(i => i));
  }

  [Test]
  public void TestConvert_ArgumentConverterNull()
  {
    var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
#if SYSTEM_CONVERTER
    Converter<int, long> converter = null;
#else
    Func<int, long> converter = null;
#endif

    Assert.Throws<ArgumentNullException>(() => array.Convert(converter));
  }
}
