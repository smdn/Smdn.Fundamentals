// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection;

partial class TypeExtensionsTests {
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class HidingInheritedTypeAttribute : Attribute
  {
    public bool HidingNonPublic { get; set; }
  }

  public class CLevel0 {
    private class CPrivate { }
    protected class CProtected { }
    protected class CProtectedHiddenByChildClass { }
    protected class CProtectedHiddenByGrandchildClass { }
    internal class CInternal { }
    internal class CInternalHiddenByChildClass { }
    internal class CInternalHiddenByGrandchildClass { }
    public class CPublic { }
    public class CPublicHiddenByChildClass { }
    public class CPublicHiddenByGrandchildClass { }
  }

  public class CLevel1 : CLevel0 {
    private class CPrivate { } // cannot hide nested private types
    [HidingInheritedType(HidingNonPublic = true)] new protected class CProtectedHiddenByChildClass { }
    [HidingInheritedType(HidingNonPublic = true)] new public class CInternalHiddenByChildClass { }
    [HidingInheritedType(HidingNonPublic = false)] new private class CPublicHiddenByChildClass { }
  }

  public class CLevel2 : CLevel1 {
    [HidingInheritedType(HidingNonPublic = true)] new protected class CProtectedHiddenByGrandchildClass { }
    [HidingInheritedType(HidingNonPublic = true)] new public class CInternalHiddenByGrandchildClass { }
    [HidingInheritedType(HidingNonPublic = false)] new private class CPublicHiddenByGrandchildClass { }

    [HidingInheritedType(HidingNonPublic = true)] new public class CProtectedHiddenByChildClass { } // redeclared in CLevel1
    [HidingInheritedType(HidingNonPublic = false)] new public class CInternalHiddenByChildClass { } // redeclared in CLevel1
  }

  private static System.Collections.IEnumerable YieldTestCases_IsHidingInheritedType()
  {
    const bool publicOnly = false;
    const bool publicAndNonPublic = true;

    foreach (var testTargetType in new[] {
      typeof(CLevel0),
      typeof(CLevel1),
      typeof(CLevel2),
    }) {
      yield return new object[] { publicAndNonPublic, testTargetType, false };
      yield return new object[] { publicOnly, testTargetType, false };

      foreach (var t in testTargetType.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)) {
        var attr = t.GetCustomAttribute<HidingInheritedTypeAttribute>();

        var isHiding = attr is not null;
        var isHidingNonPublic = attr is not null && attr.HidingNonPublic;

        yield return new object[] { publicAndNonPublic, t, isHiding };
        yield return new object[] { publicOnly, t, isHiding && !isHidingNonPublic };
      }
    }
  }

  [TestCaseSource(nameof(YieldTestCases_IsHidingInheritedType))]
  public void IsHidingInheritedType(
    bool nonPublic,
    Type t,
    bool expected
  )
    => Assert.That(t.IsHidingInheritedType(nonPublic), Is.EqualTo(expected));

  [TestCase(true)]
  [TestCase(false)]
  public void IsHidingInheritedType_ArgumentNull(bool nonPublic)
    => Assert.Throws<ArgumentNullException>(() => ((Type)null!).IsHidingInheritedType(nonPublic));
}
