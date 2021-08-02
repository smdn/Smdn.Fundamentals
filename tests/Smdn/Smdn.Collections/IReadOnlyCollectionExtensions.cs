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

      CollectionAssert.AreEqual(expected, collection.ConvertAll(i => i * 2));
    }

    [Test]
    public void TestConvertAll_ArgumentCollectionEmpty()
    {
      IReadOnlyCollection<int> collection = new int[0];

      Assert.IsEmpty(collection.ConvertAll(i => i * 2));
    }

    [Test]
    public void TestConvertAll_ArgumentCollectionNull()
    {
      IReadOnlyCollection<int> collection = null;

      Assert.Throws<ArgumentNullException>(() => collection.ConvertAll(i => i * 2));
    }

    [Test]
    public void TestConvertAll_ArgumentConverterNull()
    {
      IReadOnlyCollection<int> collection = new[] { 0, 1, 2, 3, 4 };
      Converter<int, int> converter = null;

      Assert.Throws<ArgumentNullException>(() => collection.ConvertAll(converter));
    }
  }
}
