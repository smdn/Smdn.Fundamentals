using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn.Collections {
  [TestFixture]
  public class ReadOnlyDictionaryTests {
    [Test]
    public void TestEmpty()
    {
      var empty = Smdn.Collections.ReadOnlyDictionary<string, string>.Empty;

      Assert.AreEqual(0, empty.Count);
    }
  }
}
