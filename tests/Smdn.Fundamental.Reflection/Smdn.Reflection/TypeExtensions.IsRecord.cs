// SPDX-FileCopyrightText: 2025 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace Smdn.Reflection;

[TestFixture()]
public partial class TypeExtensionsTests {
  public record R0 { }
  public record class R1 { }
  public record class R1X : R1 { }
  public record struct R2 { }
  public readonly record struct R3 { }

  public record class R4 {
    public virtual bool Equals(R4 other) => throw new NotImplementedException();
    public override int GetHashCode() => throw new NotImplementedException();
    public override string ToString() => throw new NotImplementedException();
  }

  public record class R4X : R4 {
    public virtual bool Equals(R4X other) => throw new NotImplementedException();
    public override int GetHashCode() => throw new NotImplementedException();
    public override string ToString() => throw new NotImplementedException();
  }

  [TestCase(typeof(R0), true)]
  [TestCase(typeof(R1), true)]
  [TestCase(typeof(R1X), true)]
  [TestCase(typeof(R2), true)]
  [TestCase(typeof(R3), true)]
  [TestCase(typeof(R4), true)]
  [TestCase(typeof(R4X), true)]
  [TestCase(typeof(void), false)]
  [TestCase(typeof(int), false)]
  [TestCase(typeof(List<>), false)]
  [TestCase(typeof(List<int>), false)]
  [TestCase(typeof(Enum), false)]
  [TestCase(typeof(System.DayOfWeek), false)]
  [TestCase(typeof(System.DateTimeKind), false)]
  [TestCase(typeof(System.Guid), false)]
  [TestCase(typeof(System.Delegate), false)]
  [TestCase(typeof(System.MulticastDelegate), false)]
  [TestCase(typeof(System.Action), false)]
  [TestCase(typeof(System.Func<int>), false)]
  [TestCase(typeof(System.IDisposable), false)]
  [TestCase(typeof(System.IEquatable<int>), false)]
  public void IsRecord(Type type, bool expected)
    => Assert.That(type.IsRecord(), Is.EqualTo(expected));

  [Test]
  public void IsRecord_ArgumentNull()
    => Assert.That(
      () => ((Type)null!).IsRecord(),
      Throws.ArgumentNullException
    );
}
