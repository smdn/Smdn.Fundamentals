// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

using Assert = Smdn.Test.NUnit.Assertion.Assert;

namespace Smdn.Text.Encodings;

[TestFixture]
public class EncodingNotSupportedExceptionTests {
  [Test]
  public void TestConstruct()
  {
    var ex1 = new EncodingNotSupportedException();

    Assert.That(ex1.EncodingName, Is.Null, "ex1.Name");
    Assert.That(ex1.Message, Is.Not.Null, "ex1.Message");
    Assert.That(ex1.InnerException, Is.Null, "ex1.InnerException");

    var ex2 = new EncodingNotSupportedException("ex2");

    Assert.That(ex2.EncodingName, Is.EqualTo("ex2"), "ex2.Name");
    Assert.That(ex2.Message, Is.Not.Null, "ex2.Message");
    Assert.That(ex2.InnerException, Is.Null, "ex2.InnerException");

    var ex3 = new EncodingNotSupportedException("ex3", new ArgumentException());

    Assert.That(ex3.EncodingName, Is.EqualTo("ex3"), "ex3.Name");
    Assert.That(ex3.Message, Is.Not.Null, "ex3.Message");
    Assert.That(ex3.InnerException, Is.Not.Null, "ex3.InnerException");

    var ex4 = new EncodingNotSupportedException("ex4", "hoge");

    Assert.That(ex4.EncodingName, Is.EqualTo("ex4"), "ex4.Name");
    Assert.That(ex4.Message, Is.EqualTo("hoge"), "ex4.Message");
    Assert.That(ex4.InnerException, Is.Null, "ex4.InnerException");

    var ex5 = new EncodingNotSupportedException("ex5", "hoge", new ArgumentException());

    Assert.That(ex5.EncodingName, Is.EqualTo("ex5"), "ex5.Name");
    Assert.That(ex5.Message, Is.EqualTo("hoge"), "ex5.Message");
    Assert.That(ex5.InnerException, Is.Not.Null, "ex5.InnerException");
  }

#if SYSTEM_EXCEPTION_CTOR_SERIALIZATIONINFO
  [Test]
  public void TestSerializeBinary()
  {
    var ex1 = new EncodingNotSupportedException();

    Assert.That(ex1.EncodingName, Is.Null);

#if !NET8_0_OR_GREATER
    Assert.IsSerializable(ex1, deserialized => {
      Assert.That(deserialized.EncodingName, Is.Null);
    });
#endif

    var ex2 = new EncodingNotSupportedException("x-unsupported-encoding");

    Assert.That(ex2.EncodingName, Is.EqualTo("x-unsupported-encoding"));

#if !NET8_0_OR_GREATER
    Assert.IsSerializable(ex2, deserialized => {
      Assert.That(deserialized.EncodingName, Is.EqualTo("x-unsupported-encoding"));
    });
#endif
  }
#endif
}
