using System;
using NUnit.Framework;

namespace Smdn.Text.Encodings {
  [TestFixture]
  public class EncodingNotSupportedExceptionTests {
    [Test]
    public void TestConstruct()
    {
      var ex1 = new EncodingNotSupportedException();

      Assert.IsNull(ex1.EncodingName, "ex1.Name");
      Assert.IsNotNull(ex1.Message, "ex1.Message");
      Assert.IsNull(ex1.InnerException, "ex1.InnerException");

      var ex2 = new EncodingNotSupportedException("ex2");

      Assert.AreEqual("ex2", ex2.EncodingName, "ex2.Name");
      Assert.IsNotNull(ex2.Message, "ex2.Message");
      Assert.IsNull(ex2.InnerException, "ex2.InnerException");

      var ex3 = new EncodingNotSupportedException("ex3", new ArgumentException());

      Assert.AreEqual("ex3", ex3.EncodingName, "ex3.Name");
      Assert.IsNotNull(ex3.Message, "ex3.Message");
      Assert.IsNotNull(ex3.InnerException, "ex3.InnerException");

      var ex4 = new EncodingNotSupportedException("ex4", "hoge");

      Assert.AreEqual("ex4", ex4.EncodingName, "ex4.Name");
      Assert.AreEqual("hoge", ex4.Message, "ex4.Message");
      Assert.IsNull(ex4.InnerException, "ex4.InnerException");

      var ex5 = new EncodingNotSupportedException("ex5", "hoge", new ArgumentException());

      Assert.AreEqual("ex5", ex5.EncodingName, "ex5.Name");
      Assert.AreEqual("hoge", ex5.Message, "ex5.Message");
      Assert.IsNotNull(ex5.InnerException, "ex5.InnerException");
    }

    [Test]
    public void TestSerializeBinary()
    {
      var ex1 = new EncodingNotSupportedException();

      Assert.IsNull(ex1.EncodingName);

      TestUtils.SerializeBinary(ex1, delegate(EncodingNotSupportedException deserialized) {
        Assert.IsNull(deserialized.EncodingName);
      });

      var ex2 = new EncodingNotSupportedException("x-unsupported-encoding");

      Assert.AreEqual("x-unsupported-encoding", ex2.EncodingName);

      TestUtils.SerializeBinary(ex2, delegate(EncodingNotSupportedException deserialized) {
        Assert.AreEqual("x-unsupported-encoding", deserialized.EncodingName);
      });
    }
  }
}

