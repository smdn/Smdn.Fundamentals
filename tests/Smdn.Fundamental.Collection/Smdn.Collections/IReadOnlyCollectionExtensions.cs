// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn.Collections {
  [TestFixture]
  public class IReadOnlyCollectionExtensionsTests {
    [Test]
    public void TestConvertAll()
    {
      IReadOnlyCollection<int> collection = new[] { 0, 1, 2, 3, 4 };
      var expected = new[] { 0, 2, 4, 6, 8 };

      Assert.That(collection.ConvertAll(i => i * 2), Is.EqualTo(expected).AsCollection);
    }

    [Test]
    public void TestConvertAll_ArgumentCollectionEmpty()
    {
      IReadOnlyCollection<int> collection = new int[0];

      Assert.That(collection.ConvertAll(i => i * 2), Is.Empty);
    }

    [Test]
    public void TestConvertAll_ArgumentCollectionNull()
    {
      IReadOnlyCollection<int> collection = null!;

      Assert.Throws<ArgumentNullException>(() => collection.ConvertAll(i => i * 2));
    }

    [Test]
    public void TestConvertAll_ArgumentConverterNull()
    {
      IReadOnlyCollection<int> collection = new[] { 0, 1, 2, 3, 4 };

      Assert.Throws<ArgumentNullException>(() => collection.ConvertAll<int, int>(converter: null!));
    }
  }
}
