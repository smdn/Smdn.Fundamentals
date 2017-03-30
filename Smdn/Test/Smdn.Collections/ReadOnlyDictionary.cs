using System;
using NUnit.Framework;

namespace Smdn.Collections {
  [TestFixture]
  public class ReadOnlyDictionaryTests {
    [Test]
    public void TestEmpty()
    {
      var empty = ReadOnlyDictionary<string, string>.Empty;

      Assert.AreEqual(0, empty.Count);
    }
  }
}
