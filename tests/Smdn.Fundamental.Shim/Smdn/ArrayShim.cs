// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

#if SYSTEM_ARRAY_CONVERTALL
using _Array = System.Array; // System.Array.ConvertAll
#else
using _Array = Smdn.ArrayShim; // Smdn.ArrayShim.ConvertAll
#endif

namespace Smdn;

[TestFixture()]
public class ArrayShimTests {
  [Test]
  public void TestConvertAll()
  {
    var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    var expected = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    CollectionAssert.AreEqual(
      expected,
      _Array.ConvertAll(array, i => i.ToString("D"))
    );
  }

  [Test]
  public void TestConvertAll_ArgumentArrayEmpty()
  {
    Assert.IsEmpty(
      _Array.ConvertAll(Enumerable.Empty<int>().ToArray(), i => i)
    );
  }

  [Test]
  public void TestConvertAll_ArgumentArrayNull()
  {
    int[] array = null;

    Assert.Throws<ArgumentNullException>(() => _Array.ConvertAll(array, i => i));
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

    Assert.Throws<ArgumentNullException>(() => _Array.ConvertAll(array, converter));
  }
}
