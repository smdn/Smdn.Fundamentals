// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection {
  [TestFixture()]
  public class PropertyInfoExtensionsTests {
    class C {
      public int P0 { get; init; }
      public int P1 { get; set; }
      public int P2 { get; } = 0;

      public static int SP0 { get; } = 0;
      public static int SP1 { set { } }
      public static int SP2 { get; set; }
    }

    static class SC {
      public static int SP0 { get; } = 0;
    }

    struct S {
      public int P0 { get; init; }
      public int P1 { get; set; }
      public int P2 { get => 0; }
      public readonly int P3 { get; init; }
    }

    readonly struct ROS {
      public int P0 { get; init; }
      //public int P1 { get; set; }
      public int P2 { get => 0; }
    }

    [TestCase(typeof(C), nameof(C.P0), false)]
    [TestCase(typeof(C), nameof(C.P1), false)]
    [TestCase(typeof(C), nameof(C.P2), false)]
    [TestCase(typeof(C), nameof(C.SP0), true)]
    [TestCase(typeof(C), nameof(C.SP1), true)]
    [TestCase(typeof(C), nameof(C.SP2), true)]
    [TestCase(typeof(SC), nameof(SC.SP0), true)]
    public void TestIsStatic(Type type, string propertyName, bool expected)
    {
      var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

      Assert.AreEqual(expected, property.IsStatic(), $"{type.Name}.{property!.Name}");
    }

    [TestCase(typeof(C), nameof(C.P0), true)]
    [TestCase(typeof(C), nameof(C.P1), false)]
    [TestCase(typeof(S), nameof(S.P0), true)]
    [TestCase(typeof(S), nameof(S.P1), false)]
    [TestCase(typeof(S), nameof(S.P3), true)]
    [TestCase(typeof(S), nameof(ROS.P0), true)]
    public void TestIsSetMethodInitOnly(Type type, string propertyName, bool expected)
    {
      var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

      Assert.AreEqual(expected, property.IsSetMethodInitOnly(), $"{type.Name}.{property!.Name}");
    }

    [TestCase(typeof(C), nameof(C.P2))]
    [TestCase(typeof(S), nameof(S.P2))]
    [TestCase(typeof(ROS), nameof(ROS.P2))]
    public void TestIsSetMethodInitOnly_ReadOnly(Type type, string propertyName)
    {
      var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

      Assert.Throws<InvalidOperationException>(() => property.IsSetMethodInitOnly(), $"{type.Name}.{property!.Name}");
    }
  }
}
