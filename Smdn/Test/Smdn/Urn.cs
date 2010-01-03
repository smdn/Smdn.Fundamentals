using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class UrnTests {
    [Test]
    public void TestCreate()
    {
      Assert.AreEqual(new Uri("urn:ietf:rfc:2141"), Urn.Create("ietf", "rfc:2141"));
      Assert.AreEqual(new Uri("URN:ISBN:4-8399-0454-5"), Urn.Create("ISBN", "4-8399-0454-5"));
      Assert.AreEqual(new Uri("urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6"), Urn.Create("uuid", "f81d4fae-7dec-11d0-a765-00a0c91e6bf6"));
    }

    [Test]
    public void TestGetNamespaceIdentifier()
    {
      Assert.AreEqual("ietf", Urn.GetNamespaceIdentifier("urn:ietf:rfc:2141"));
      Assert.AreEqual("ietf", Urn.GetNamespaceIdentifier(new Uri("urn:ietf:rfc:2141")));
      Assert.AreEqual("ISBN", Urn.GetNamespaceIdentifier("URN:ISBN:4-8399-0454-5"));
      Assert.AreEqual("ISBN", Urn.GetNamespaceIdentifier(new Uri("URN:ISBN:4-8399-0454-5")));
      Assert.AreEqual("uuid", Urn.GetNamespaceIdentifier("urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6"));
      Assert.AreEqual("uuid", Urn.GetNamespaceIdentifier(new Uri("urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6")));
      Assert.AreEqual("iso",  Urn.GetNamespaceIdentifier("urn:iso:std:iso:9999:-1:ed-1:en"));

      try {
        Urn.GetNamespaceIdentifier("http://localhost");
        Assert.Fail("ArgumentException not thrown");
      }
      catch (ArgumentException) {
      }
    }

    [Test]
    public void TestGetNamespaceSpecificString()
    {
      Assert.AreEqual("rfc:2141", Urn.GetNamespaceSpecificString("urn:ietf:rfc:2141", Urn.NamespaceIetf));
      Assert.AreEqual("rfc:2141", Urn.GetNamespaceSpecificString("URN:IETF:rfc:2141", Urn.NamespaceIetf));
      Assert.AreEqual("rfc:2141", Urn.GetNamespaceSpecificString(new Uri("urn:ietf:rfc:2141"), Urn.NamespaceIetf));
      Assert.AreEqual("4-8399-0454-5", Urn.GetNamespaceSpecificString("URN:ISBN:4-8399-0454-5", Urn.NamespaceIsbn));
      Assert.AreEqual("f81d4fae-7dec-11d0-a765-00a0c91e6bf6", Urn.GetNamespaceSpecificString("urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6", Urn.NamespaceUuid));
      Assert.AreEqual("std:iso:9999:-1:ed-1:en",  Urn.GetNamespaceSpecificString("urn:iso:std:iso:9999:-1:ed-1:en", Urn.NamespaceIso));

      try {
        Urn.GetNamespaceSpecificString("urn:ietf:rfc:2141", Urn.NamespaceUuid);
        Assert.Fail("ArgumentException not thrown");
      }
      catch (ArgumentException) {
      }
   }
  }
}