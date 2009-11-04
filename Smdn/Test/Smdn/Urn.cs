using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class UrnTests {
    [Test]
    public void TestConstruct()
    {
      new Urn("urn:ietf:rfc:2141");
      new Urn("Urn:ietf:rfc:2141");
      new Urn("URN:ISBN:4-8399-0454-5");
      new Urn("urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6");

      Assert.AreEqual(new Urn("urn:ietf:rfc:2141"), new Urn("ietf", "rfc:2141"));
      Assert.AreEqual(new Urn("URN:ISBN:4-8399-0454-5"), new Urn("ISBN", "4-8399-0454-5"));
      Assert.AreEqual(new Urn("urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6"), new Urn("uuid", "f81d4fae-7dec-11d0-a765-00a0c91e6bf6"));
    }

    [Test, ExpectedException(typeof(ArgumentException))]
    public void TestConstructInvalidScheme()
    {
      new Urn("http://localhost/");
    }

    [Test]
    public void TestNamespaceIdentifier()
    {
      Assert.AreEqual("ietf", (new Urn("urn:ietf:rfc:2141")).NamespaceIdentifier);
      Assert.AreEqual("ISBN", (new Urn("URN:ISBN:4-8399-0454-5")).NamespaceIdentifier);
      Assert.AreEqual("uuid", (new Urn("urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6")).NamespaceIdentifier);
    }

    [Test]
    public void TestNamespaceSpecificString()
    {
      Assert.AreEqual("rfc:2141", (new Urn("urn:ietf:rfc:2141")).NamespaceSpecificString);
      Assert.AreEqual("4-8399-0454-5", (new Urn("URN:ISBN:4-8399-0454-5")).NamespaceSpecificString);
      Assert.AreEqual("f81d4fae-7dec-11d0-a765-00a0c91e6bf6", (new Urn("urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6")).NamespaceSpecificString);
    }
  }
}