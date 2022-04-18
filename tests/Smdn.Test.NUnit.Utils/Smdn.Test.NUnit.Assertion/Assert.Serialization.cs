// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
using System.Runtime.Serialization.Formatters.Binary;
#endif
using NUnit.Framework;

namespace Smdn.Test.NUnit.Assertion;

[TestFixture]
public class AssertSerializationTests {
  [Test] public void IsSerializable_ArrayOfInt32() => Assert.IsSerializable(new int[] { 0, 1, 2 });
  [Test] public void IsSerializable_Uri() => Assert.IsSerializable(new Uri("http://example.com/example/"));
  [Test] public void IsSerializable_IntPtr() => Assert.IsSerializable(IntPtr.Zero);
  [Test] public void IsSerializable_DateTime() => Assert.IsSerializable(DateTime.MinValue);
  [Test] public void IsSerializable_Exception() => Assert.IsSerializable(new InvalidOperationException());

  [Test]
  public void IsSerializable_WithTestAction_ArrayOfInt32()
  {
    var testActionCalled = false;
    var arr = new int[] { 0, 1, 2 };

    Assert.IsSerializable(arr, obj => {
      Assert.IsNotNull(obj);
      Assert.IsInstanceOf<int[]>(obj);
      CollectionAssert.AreEqual(arr, obj as int[]);

      testActionCalled = true;
    });

#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    Assert.IsTrue(testActionCalled, "test action called");
#else
    Assert.IsFalse(testActionCalled, "test action called");
#endif
  }

  [Test]
  public void IsSerializable_WithTestAction_Uri()
  {
    var url = "http://example.com/example/";
    var testActionCalled = false;

    Assert.IsSerializable(new Uri(url), obj => {
      Assert.IsNotNull(obj);
      Assert.IsInstanceOf<Uri>(obj);
      Assert.AreEqual(url, (obj as Uri).AbsoluteUri);

      testActionCalled = true;
    });

#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    Assert.IsTrue(testActionCalled, "test action called");
#else
    Assert.IsFalse(testActionCalled, "test action called");
#endif
  }

  [Test]
  public void IsSerializable_WithTestAction_IntPtr()
  {
    var testActionCalled = false;

    Assert.IsSerializable(new IntPtr(1), obj => {
      Assert.IsNotNull(obj);
      Assert.IsInstanceOf<IntPtr>(obj);
      Assert.AreEqual(new IntPtr(1), obj);

      testActionCalled = true;
    });

#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    Assert.IsTrue(testActionCalled, "test action called");
#else
    Assert.IsFalse(testActionCalled, "test action called");
#endif
  }

  [Test]
  public void IsSerializable_WithTestAction_DateTime()
  {
    var now = DateTime.Now;
    var testActionCalled = false;

    Assert.IsSerializable(now, obj => {
      Assert.IsNotNull(obj);
      Assert.IsInstanceOf<DateTime>(obj);
      Assert.AreEqual(now, obj);

      testActionCalled = true;
    });

#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    Assert.IsTrue(testActionCalled, "test action called");
#else
    Assert.IsFalse(testActionCalled, "test action called");
#endif
  }

  [Test]
  public void IsSerializable_WithTestAction_Exception()
  {
    var innerException = new NotImplementedException("inner exception");
    var message = "is serializable";
    var testActionCalled = false;

    Assert.IsSerializable(new NotSupportedException(message, innerException), obj => {
      Assert.IsNotNull(obj);
      Assert.IsInstanceOf<NotSupportedException>(obj);

      var ex = (obj as NotSupportedException);

      Assert.AreEqual(message, ex.Message);

      Assert.IsNotNull(ex.InnerException);
      Assert.IsInstanceOf<NotImplementedException>(ex.InnerException);
      Assert.AreEqual(innerException.Message, ex.InnerException.Message);

      testActionCalled = true;
    });

#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    Assert.IsTrue(testActionCalled, "test action called");
#else
    Assert.IsFalse(testActionCalled, "test action called");
#endif
  }
}
