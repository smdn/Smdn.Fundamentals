using System;
using System.Threading;
using NUnit.Framework;

namespace Smdn.Threading {
  [TestFixture()]
  public class LazyInitializerTests {
    class TestClass {
      public TestClass()
      {
      }
    }

    [Test]
    public void TestEnsureInitialized()
    {
      TestClass c = null;

      LazyInitializer.EnsureInitialized(ref c, delegate {
        return new TestClass();
      });

      Assert.IsNotNull(c);
    }

    [Test]
    public void TestEnsureInitializedAlreadyInitialized()
    {
      TestClass c = new TestClass();
      TestClass initializedValue = c;

      LazyInitializer.EnsureInitialized(ref c, delegate {
        Assert.Fail("initializer called");
        return new TestClass();
      });

      Assert.IsNotNull(c);
      Assert.AreSame(initializedValue, c);
    }

    [Test]
    public void TestEnsureInitializedValueFactoryReturnsNull()
    {
      TestClass c = null;

      Assert.Throws<InvalidOperationException>(delegate {
        LazyInitializer.EnsureInitialized(ref c, delegate {
          return (TestClass)null;
        });
      });
    }
  }
}
