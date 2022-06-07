// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public class EventInfoExtensionsTests {
#pragma warning disable 0067
  public event EventHandler E1;

  public event EventHandler E2 {
    add {
      throw new NotImplementedException();
    }
    remove {
      throw new NotImplementedException();
    }
  }

  private event EventHandler E3;

  class C {
    public event EventHandler E0;
    private event EventHandler E1;

    public static event EventHandler SE0;
    private static event EventHandler SE1;
  }
#pragma warning restore 0067

  [TestCase(typeof(C), nameof(C.E0), false)]
  [TestCase(typeof(C), "E1", false)]
  [TestCase(typeof(C), nameof(C.SE0), true)]
  [TestCase(typeof(C), "SE1", true)]
  public void IsStatic(Type type, string eventName, bool expected)
  {
    var ev = type.GetEvent(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    Assert.AreEqual(expected, ev!.IsStatic(), $"{type.Name}.{ev!.Name}");
  }

  [Test]
  public void IsStatic_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((EventInfo)null!).IsStatic());

  [Test]
  public void GetMethods()
  {
    var e1 = GetType().GetEvent("E1", BindingFlags.Instance | BindingFlags.Public)!;

    CollectionAssert.IsNotEmpty(e1.GetMethods());
    Assert.AreEqual(2, e1.GetMethods().Count());

    var e2 = GetType().GetEvent("E2", BindingFlags.Instance | BindingFlags.Public)!;

    CollectionAssert.IsNotEmpty(e2.GetMethods());
    Assert.AreEqual(2, e2.GetMethods().Count());

    var e3 = GetType().GetEvent("E3", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;

    CollectionAssert.IsEmpty(e3.GetMethods(false));
    CollectionAssert.IsNotEmpty(e3.GetMethods(true));
  }

  [Test]
  public void GetMethods_ArgumentNull()
  {
    EventInfo ev = null!;

    Assert.Throws<ArgumentNullException>(() => ev.GetMethods());
    Assert.Throws<ArgumentNullException>(() => ev.GetMethods(nonPublic: true), "nonPublic: true");
    Assert.Throws<ArgumentNullException>(() => ev.GetMethods(nonPublic: false), "nonPublic: false");
  }

#if false
Public Custom Event E As EventHandler
  AddHandler(value As EventHandler)
    Throw New NotImplementedException()
  End AddHandler

  RemoveHandler(value as EventHandler)
    Throw New NotImplementedException()
  End RemoveHandler

  RaiseEvent(sender As Object, e As EventArgs)
    Throw New NotImplementedException()
  End RaiseEvent
End Event
#endif
}
