using System;
using NUnit.Framework;

namespace Smdn.Threading {
  [TestFixture()]
  public class ParallelTests {
    [Test]
    public void TestForEach()
    {
      var ret = new[] {false, false, false};

      Parallel.ForEach(new[] {0, 1, 2}, delegate(int index) {
        Assert.IsFalse(ret[index]);
        ret[index] = true;
      });

      Assert.IsTrue(ret[0]);
      Assert.IsTrue(ret[1]);
      Assert.IsTrue(ret[2]);
    }

    [Test]
    public void TestForEachOneElement()
    {
      var ret = new[] {false};

      Parallel.ForEach(new[] {0}, delegate(int index) {
        Assert.IsFalse(ret[index]);
        ret[index] = true;
      });

      Assert.IsTrue(ret[0]);
    }

    [Test]
    public void TestForEachZeroElement()
    {
      Parallel.ForEach(new int[] {}, delegate(int index) {
        Assert.Fail("action called");
      });
    }
  }
}
