// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;


namespace Smdn.Reflection;

[TestFixture()]
public class MemberInfoExtensionsTests {
  private void TestGetAccessibility(MemberInfo member, Accessibility expected)
    => Assert.AreEqual(expected, member.GetAccessibility());

  [TestCase(typeof(MemberInfoExtensionsTestTypes.C1), null, Accessibility.Public)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C2), null, Accessibility.Assembly)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C.C3), null, Accessibility.Public)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C.C4), null, Accessibility.Assembly)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "C5", Accessibility.Family)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C.C6), null, Accessibility.FamilyOrAssembly)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "C7", Accessibility.FamilyAndAssembly)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "C8", Accessibility.Private)]
  public void GetAccessibility_Types(Type type, string nestedTypeName, Accessibility expected)
  {
    if (nestedTypeName != null)
      type = type.GetNestedType(nestedTypeName, BindingFlags.Public | BindingFlags.NonPublic);

    TestGetAccessibility(type, expected);
  }

  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), nameof(MemberInfoExtensionsTestTypes.C.M1), Accessibility.Public)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), nameof(MemberInfoExtensionsTestTypes.C.M2), Accessibility.Assembly)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "M3", Accessibility.Family)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), nameof(MemberInfoExtensionsTestTypes.C.M4), Accessibility.FamilyOrAssembly)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "M5", Accessibility.FamilyAndAssembly)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "M6", Accessibility.Private)]
  public void GetAccessibility_Methods(Type type, string memberName, Accessibility expected)
    => TestGetAccessibility(
      type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).First(),
      expected
    );

  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), nameof(MemberInfoExtensionsTestTypes.C.F1), Accessibility.Public)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), nameof(MemberInfoExtensionsTestTypes.C.F2), Accessibility.Assembly)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "F3", Accessibility.Family)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), nameof(MemberInfoExtensionsTestTypes.C.F4), Accessibility.FamilyOrAssembly)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "F5", Accessibility.FamilyAndAssembly)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "F6", Accessibility.Private)]
  public void GetAccessibility_Fields(Type type, string memberName, Accessibility expected)
    => TestGetAccessibility(
      type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).First(),
      expected
    );

  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "P1")]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "E1")]
  public void GetAccessibility_OtherMembers(Type type, string memberName)
  {
    var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).First();

    Assert.Throws<InvalidOperationException>(() => member.GetAccessibility());
  }

  [Test]
  public void GetAccessibility_ArgumentNull()
  {
    MemberInfo member = null;

    Assert.Throws<ArgumentNullException>(() => member.GetAccessibility());
  }

  [TestCase(typeof(MemberInfoExtensionsTestTypes.C1), null, null, false)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C2), null, null, true)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C.C3), null, null, false)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C.C4), null, null, true)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "C5", null, false)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C.C6), null, null, false)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "C7", null, true)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), "C8", null, true)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, nameof(MemberInfoExtensionsTestTypes.C.M1), false)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, nameof(MemberInfoExtensionsTestTypes.C.M2), true)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, "M3", false)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, nameof(MemberInfoExtensionsTestTypes.C.M4), false)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, "M5", true)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, "M6", true)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, nameof(MemberInfoExtensionsTestTypes.C.F1), false)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, nameof(MemberInfoExtensionsTestTypes.C.F2), true)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, "F3", false)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, nameof(MemberInfoExtensionsTestTypes.C.F4), false)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, "F5", true)]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), null, "F6", true)]
  public void IsPrivateOrAssembly(Type type, string nestedTypeName, string memberName, bool expected)
  {
    if (nestedTypeName != null)
      type = type.GetNestedType(nestedTypeName, BindingFlags.Public | BindingFlags.NonPublic);

    if (memberName == null) {
      Assert.AreEqual(expected, type.IsPrivateOrAssembly(), type!.FullName);
    }
    else {
      var member = type!.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).First();

      Assert.AreEqual(expected, member.IsPrivateOrAssembly(), $"{type.FullName}.{member.Name}");
    }
  }

  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), nameof(MemberInfoExtensionsTestTypes.C.P1))]
  [TestCase(typeof(MemberInfoExtensionsTestTypes.C), nameof(MemberInfoExtensionsTestTypes.C.E1))]
  public void IsPrivateOrAssembly_MemberInvalid(Type type, string memberName)
  {
    var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).First();

    Assert.Throws<InvalidOperationException>(() => member.IsPrivateOrAssembly(), $"{type.FullName}.{member.Name}");
  }

  [Test]
  public void IsPrivateOrAssembly_ArgumentNull()
  {
    MemberInfo member = null;

    Assert.Throws<ArgumentNullException>(() => member.IsPrivateOrAssembly());
  }
}
