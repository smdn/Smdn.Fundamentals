using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class LazyTests {
    [Test]
    public void TestConstructValueTypeWithDefaultConstructor()
    {
      var lazy = new Lazy<int>();

      Assert.IsFalse(lazy.IsValueCreated);
      Assert.AreEqual(0, lazy.Value);
      Assert.IsTrue(lazy.IsValueCreated);
    }

    private class ReferenceType {
    }

    [Test]
    public void TestConstructReferenceTypeWithDefaultConstructor()
    {
      var lazy = new Lazy<ReferenceType>();

      Assert.IsFalse(lazy.IsValueCreated);
      Assert.IsInstanceOfType(typeof(ReferenceType), lazy.Value);
      Assert.IsTrue(lazy.IsValueCreated);
    }

    [Test]
    public void TestConstructWithFactory()
    {
      var lazy = new Lazy<int>(delegate{ return 3; });

      Assert.IsFalse(lazy.IsValueCreated);
      Assert.AreEqual(3, lazy.Value);
      Assert.IsTrue(lazy.IsValueCreated);
    }

    [Test]
    public void TestToString()
    {
      var lazy = new Lazy<int>(delegate{ return 3; });

      Assert.IsFalse(lazy.IsValueCreated);
      Assert.AreEqual("3", lazy.ToString());
      Assert.IsTrue(lazy.IsValueCreated);
    }
  }
}