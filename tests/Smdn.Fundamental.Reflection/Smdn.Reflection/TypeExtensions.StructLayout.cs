// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Smdn.Reflection {
#pragma warning disable IDE0044 // Make field readonly
  namespace TestCases {
    namespace StructLayouts {
#pragma warning disable 0169
      struct S0 {}

      struct S1 {
        int x;
      }

      struct S2 {
        object x;
      }

      [StructLayout(LayoutKind.Auto)]
      struct SLayoutKindAuto {}

      [StructLayout(LayoutKind.Sequential)]
      struct SLayoutKindSequential {
        int x;
        int y;
      }

      [StructLayout(LayoutKind.Explicit)]
      struct SLayoutKindExplicit {
        [FieldOffset(0)] int x;
        [FieldOffset(4)] int y;
      }

      [StructLayout(LayoutKind.Sequential, Pack = 0)]
      struct SPack0 {
        byte x;
        int y;
      }

      [StructLayout(LayoutKind.Sequential, Pack = 1)]
      struct SPack1 {
        byte x;
        int y;
      }

      [StructLayout(LayoutKind.Sequential, Pack = 2)]
      struct SPack2 {
        byte x;
        int y;
      }

      [StructLayout(LayoutKind.Sequential, Pack = 4)]
      struct SPack4 {
        byte x;
        int y;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
      struct SCharSetNotSpecified {
        string s;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
      struct SCharSetAnsi {
        string s;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      struct SCharSetUnicode {
        string s;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      struct SCharSetAuto {
        string s;
      }

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.None)]
      struct SCharSetNone {
        string s;
      }

      [StructLayout(LayoutKind.Sequential)]
      struct SSizeNotSpecified {
        byte x;
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      struct SSize1 {
        byte x;
      }

      [StructLayout(LayoutKind.Sequential, Size = 2)]
      struct SSize2 {
        byte x;
      }
    }
#pragma warning restore 0169
  }
#pragma warning restore IDE0044

  partial class TypeExtensionsTests {
    [TestCase(typeof(TestCases.StructLayouts.S0), true)]
    [TestCase(typeof(TestCases.StructLayouts.S1), true)]
    [TestCase(typeof(TestCases.StructLayouts.S2), true)]
    [TestCase(typeof(TestCases.StructLayouts.SLayoutKindAuto), false)]
    [TestCase(typeof(TestCases.StructLayouts.SLayoutKindSequential), true)]
    [TestCase(typeof(TestCases.StructLayouts.SLayoutKindExplicit), false)]
    [TestCase(typeof(TestCases.StructLayouts.SPack0), true)]
    [TestCase(typeof(TestCases.StructLayouts.SPack1), false)]
    [TestCase(typeof(TestCases.StructLayouts.SPack2), false)]
    [TestCase(typeof(TestCases.StructLayouts.SPack4), false)]
    [TestCase(typeof(TestCases.StructLayouts.SCharSetNotSpecified), true)]
    [TestCase(typeof(TestCases.StructLayouts.SCharSetAuto), false)]
    [TestCase(typeof(TestCases.StructLayouts.SCharSetAnsi), true)]
    [TestCase(typeof(TestCases.StructLayouts.SCharSetUnicode), false)]
    [TestCase(typeof(TestCases.StructLayouts.SCharSetNone), true)]
    public void IsStructLayoutDefault(Type type, bool expected)
      => Assert.That(type.IsStructLayoutDefault(), Is.EqualTo(expected), type.FullName);

    [TestCase(typeof(TestCases.StructLayouts.SSizeNotSpecified), true)]
    [TestCase(typeof(TestCases.StructLayouts.SSize1), true)]
    [TestCase(typeof(TestCases.StructLayouts.SSize2), true)]
    public void IsStructLayoutDefault_SizeMustNotBetConsidered(Type type, bool expected)
      => Assert.That(type.IsStructLayoutDefault(), Is.EqualTo(expected), type.FullName);

    [Test]
    public void IsStructLayoutDefault_ArgumentNull()
    {
      Type type = null;

      Assert.Throws<ArgumentNullException>(() => type.IsStructLayoutDefault());
    }

    [TestCase(typeof(ValueType))]
    [TestCase(typeof(object))]
    [TestCase(typeof(Func<>))]
    [TestCase(typeof(LayoutKind))]
    public void IsStructLayoutDefault_InvalidType(Type type)
      => Assert.Throws<ArgumentException>(() => type.IsStructLayoutDefault(), type.FullName);
  }
}
