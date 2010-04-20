using System;
using NUnit.Framework;

#if NET_4_0
using System.Threading.Tasks;
#endif

namespace Smdn.Threading {
  [TestFixture()]
  public class ParallelTests {
    [Test]
    public void TestFor()
    {
      Parallel.For(0, 0, delegate(int i) {
        Assert.Fail("action called");
      });

      Parallel.For(0, -1, delegate(int i) {
        Assert.Fail("action called");
      });

      Parallel.For(0, 1, delegate(int i) {
        Assert.AreEqual(0, i);
      });

      var test1 = new int[10];

      Parallel.For(20, 30, delegate(int i) {
        test1[i - 20] = i;
      });

      Assert.AreEqual(new[] {20, 21, 22, 23, 24, 25, 26, 27, 28, 29}, test1);
    }

    [Test]
    public void TestForExceptionInAction()
    {
      Parallel.For(0, 10, delegate(int i) {
        throw new Exception();
      });
    }

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
    public void TestForEachExceptionInAction()
    {
      Parallel.ForEach(new[] {0, 1, 2}, delegate(int index) {
        throw new Exception();
      });
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
