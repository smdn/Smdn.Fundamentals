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

#if false
    [Test]
    public void TRest()
    {
      const int eee = 10000000;

      var sw1 = System.Diagnostics.Stopwatch.StartNew();
      for (var i = 0; i < eee; i++) {
        foreach (var e in new int[0]) {
        }
      }
      Console.WriteLine(sw1.Elapsed);
      var sw2 = System.Diagnostics.Stopwatch.StartNew();
      for (var i = 0; i < eee; i++) {
        foreach (var e in EmptyArray<int>.Instance) {
        }
      }
      Console.WriteLine(sw2.Elapsed);
      Assert.Fail();
    }
#endif
  }
}
