using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class EmptyArrayTests {
    [Test]
    public void TestValue()
    {
      var intArray = EmptyArray<int>.Instance;

      Assert.AreEqual(0, intArray.Length);
      Assert.AreSame(intArray, EmptyArray<int>.Instance);
    }
  }
}
