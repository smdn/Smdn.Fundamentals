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
      Assert.That(obj, Is.Not.Null);
      Assert.That(obj, Is.InstanceOf<int[]>());
      Assert.That(obj as int[], Is.EqualTo(arr).AsCollection);

      testActionCalled = true;
    });

#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    Assert.That(testActionCalled, Is.True, "test action called");
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
      Assert.That(obj, Is.Not.Null);
      Assert.That(obj, Is.InstanceOf<Uri>());
      Assert.That((obj as Uri).AbsoluteUri, Is.EqualTo(url));

      testActionCalled = true;
    });

#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    Assert.That(testActionCalled, Is.True, "test action called");
#else
    Assert.IsFalse(testActionCalled, "test action called");
#endif
  }

  [Test]
  public void IsSerializable_WithTestAction_IntPtr()
  {
    var testActionCalled = false;

    Assert.IsSerializable(new IntPtr(1), obj => {
      Assert.That(obj, Is.InstanceOf<IntPtr>());
      Assert.That(obj, Is.EqualTo(new IntPtr(1)));

      testActionCalled = true;
    });

#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    Assert.That(testActionCalled, Is.True, "test action called");
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
      Assert.That(obj, Is.InstanceOf<DateTime>());
      Assert.That(obj, Is.EqualTo(now));

      testActionCalled = true;
    });

#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    Assert.That(testActionCalled, Is.True, "test action called");
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
      Assert.That(obj, Is.Not.Null);
      Assert.That(obj, Is.InstanceOf<NotSupportedException>());

      var ex = (obj as NotSupportedException);

      Assert.That(ex.Message, Is.EqualTo(message));

      Assert.That(ex.InnerException, Is.Not.Null);
      Assert.That(ex.InnerException, Is.InstanceOf<NotImplementedException>());
      Assert.That(ex!.InnerException!.Message, Is.EqualTo(innerException.Message));

      testActionCalled = true;
    });

#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    Assert.That(testActionCalled, Is.True, "test action called");
#else
    Assert.IsFalse(testActionCalled, "test action called");
#endif
  }
}
