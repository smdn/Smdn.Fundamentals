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
    add => throw new NotImplementedException();
    remove => throw new NotImplementedException();
  }

  private event EventHandler E3;

  public static event EventHandler SE0;
  public static event EventHandler SE1 {
    add => throw new NotImplementedException();
    remove => throw new NotImplementedException();
  }

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

    Assert.That(ev!.IsStatic(), Is.EqualTo(expected), $"{type.Name}.{ev!.Name}");
  }

  [Test]
  public void IsStatic_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((EventInfo)null!).IsStatic());

  [Test]
  public void GetMethods()
  {
    var e1 = GetType().GetEvent("E1", BindingFlags.Instance | BindingFlags.Public)!;

    Assert.That(e1.GetMethods(), Is.Not.Empty);
    Assert.That(e1.GetMethods().Count(), Is.EqualTo(2));

    var e2 = GetType().GetEvent("E2", BindingFlags.Instance | BindingFlags.Public)!;

    Assert.That(e2.GetMethods(), Is.Not.Empty);
    Assert.That(e2.GetMethods().Count(), Is.EqualTo(2));

    var e3 = GetType().GetEvent("E3", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;

    Assert.That(e3.GetMethods(false), Is.Empty);
    Assert.That(e3.GetMethods(true), Is.Not.Empty);
  }

  [Test]
  public void GetMethods_ArgumentNull()
  {
    EventInfo ev = null!;

    Assert.Throws<ArgumentNullException>(() => ev.GetMethods());
    Assert.Throws<ArgumentNullException>(() => ev.GetMethods(nonPublic: true), "nonPublic: true");
    Assert.Throws<ArgumentNullException>(() => ev.GetMethods(nonPublic: false), "nonPublic: false");
  }

  [TestCase(typeof(EventInfoExtensionsTests), nameof(E1), true)]
  [TestCase(typeof(EventInfoExtensionsTests), nameof(E2), false)]
  [TestCase(typeof(EventInfoExtensionsTests), nameof(E3), true)]
  [TestCase(typeof(EventInfoExtensionsTests), nameof(SE0), true)]
  [TestCase(typeof(EventInfoExtensionsTests), nameof(SE1), false)]
  public void GetBackingField(Type type, string propertyName, bool hasBackingField)
  {
    var ev = type.GetEvent(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    if (hasBackingField)
      Assert.That(ev!.GetBackingField(), Is.Not.Null);
    else
      Assert.That(ev!.GetBackingField(), Is.Null);
  }

  [Test]
  public void GetBackingField_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((EventInfo)null!).GetBackingField());

#pragma warning disable CS0067
  class CStatic {
    public static event EventHandler EStatic;
  }

  abstract class CAbstract {
    public abstract event EventHandler EAbstract;
    public virtual event EventHandler EVirtual;
  }

  class COverride : CAbstract {
    public override event EventHandler EAbstract;
    public override event EventHandler EVirtual;
  }

  class CSealed : COverride {
    public sealed override event EventHandler EAbstract;
    public sealed override event EventHandler EVirtual;
  }

  class CVirtual {
    public virtual event EventHandler EVirtual;
    public virtual event EventHandler EVirtualInherited;
  }

  abstract class CNew : CVirtual {
    public new event EventHandler EVirtual;
  }

  abstract class CNewVirtual : CVirtual {
    public new virtual event EventHandler EVirtual;
  }
#pragma warning restore CS0067

  [TestCase(typeof(C), nameof(C.E0), false)]
  [TestCase(typeof(C), nameof(C.SE0), false)]
  [TestCase(typeof(CStatic), nameof(CStatic.EStatic), false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.EAbstract), false)]
  [TestCase(typeof(CAbstract), nameof(CAbstract.EVirtual), false)]
  [TestCase(typeof(COverride), nameof(COverride.EAbstract), true)]
  [TestCase(typeof(COverride), nameof(COverride.EVirtual), true)]
  [TestCase(typeof(CSealed), nameof(CSealed.EAbstract), true)]
  [TestCase(typeof(CSealed), nameof(CSealed.EVirtual), true)]
  [TestCase(typeof(CVirtual), nameof(CVirtual.EVirtual), false)]
  [TestCase(typeof(CNew), nameof(CNew.EVirtual), false)]
  [TestCase(typeof(CNewVirtual), nameof(CNewVirtual.EVirtual), false)]
  public void IsOverride(Type type, string eventName, bool expected)
  {
    var ev = type.GetEvent(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    Assert.That(ev!.IsOverride(), Is.EqualTo(expected), $"{type.Name}.{ev!.Name}");
  }

  [TestCase(typeof(CVirtual), nameof(CVirtual.EVirtualInherited), typeof(CVirtual), false)]
  [TestCase(typeof(CNewVirtual), nameof(CNewVirtual.EVirtualInherited), typeof(CVirtual), false)] // = CVirtual.EVirtualInherited
  public void IsOverride_IgnoreReflectedType(Type type, string eventName, Type declaringType, bool expected)
  {
    var ev = type.GetEvent(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    Assert.That(type, Is.EqualTo(ev!.ReflectedType), nameof(ev.ReflectedType));
    Assert.That(declaringType, Is.EqualTo(ev!.DeclaringType), nameof(ev.DeclaringType));
    Assert.That(ev!.IsOverride(), Is.EqualTo(expected), $"{type.Name}.{ev!.Name}");
  }

  [Test]
  public void IsOverride_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((PropertyInfo)null!).IsOverride());

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

  // ref: https://learn.microsoft.com/ja-jp/dotnet/csharp/language-reference/proposals/csharp-8.0/readonly-instance-members
  struct SReadOnlyEvent {
    public event Action<EventArgs> E {
      add => throw new NotImplementedException();
      remove => throw new NotImplementedException();
    }

    public readonly event Action<EventArgs> EReadOnly {
      add => throw new NotImplementedException();
      remove => throw new NotImplementedException();
    }
  }

  [TestCase(typeof(SReadOnlyEvent), nameof(SReadOnlyEvent.E), false)]
  [TestCase(typeof(SReadOnlyEvent), nameof(SReadOnlyEvent.EReadOnly), true)]
  public void IsReadOnly(Type type, string eventName, bool expected)
  {
    var ev = type.GetEvent(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

    Assert.That(ev!.IsReadOnly(), Is.EqualTo(expected), $"{type.Name}.{ev!.Name}");
  }

  [Test]
  public void IsReadOnly_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((EventInfo)null!).IsReadOnly());
}
