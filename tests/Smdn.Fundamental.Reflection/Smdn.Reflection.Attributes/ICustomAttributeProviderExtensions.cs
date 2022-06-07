// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NUnit.Framework;

[assembly: Smdn.Reflection.Attributes.ICustomAttributeProviderExtensionsTestsAssemblyAttribute]
[module: Smdn.Reflection.Attributes.ICustomAttributeProviderExtensionsTestsModuleAttribute]

namespace Smdn.Reflection.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class ICustomAttributeProviderExtensionsTestsAssemblyAttribute : Attribute {
  public ICustomAttributeProviderExtensionsTestsAssemblyAttribute() { }
}

[AttributeUsage(AttributeTargets.Module)]
public class ICustomAttributeProviderExtensionsTestsModuleAttribute : Attribute {
  public ICustomAttributeProviderExtensionsTestsModuleAttribute() { }
}

[TestFixture()]
public class ICustomAttributeProviderExtensionsTests {
  [Serializable]
  public class C {
    [Obsolete]
    [return: MarshalAs(UnmanagedType.Bool)]
    public bool M(
      [CallerFilePath] string sourceFilePath = default,
      [CallerLineNumber] int sourceLineNumber = default
    )
      => throw new NotImplementedException();
  }

  private static void AssertCustomAttributeDataList(IList<CustomAttributeData> list, Type attributeTypeWhichExpectedToBeContained)
  {
    Assert.IsNotNull(list);
    CollectionAssert.IsNotEmpty(list);
    CollectionAssert.Contains(list.Select(cad => cad.AttributeType), attributeTypeWhichExpectedToBeContained);
  }

  private static System.Collections.IEnumerable YieldTestCases_GetCustomAttributeDataList_OfMemberInfo()
  {
    yield return new object[] { typeof(C), typeof(SerializableAttribute) };
    yield return new object[] { typeof(C).GetMethod(nameof(C.M)), typeof(ObsoleteAttribute) };
  }

  [TestCaseSource(nameof(YieldTestCases_GetCustomAttributeDataList_OfMemberInfo))]
  public void GetCustomAttributeDataList_OfMemberInfo(MemberInfo member, Type attributeTypeWhichExpectedToBeContained)
    => AssertCustomAttributeDataList(member.GetCustomAttributeDataList(), attributeTypeWhichExpectedToBeContained);

  private static System.Collections.IEnumerable YieldTestCases_GetCustomAttributeDataList_OfParameterInfo()
  {
    var paras = typeof(C).GetMethod(nameof(C.M))!.GetParameters();

    yield return new object[] { paras[0], typeof(CallerFilePathAttribute) };
    yield return new object[] { paras[1], typeof(CallerLineNumberAttribute) };
    yield return new object[] { typeof(C).GetMethod(nameof(C.M))!.ReturnParameter, typeof(MarshalAsAttribute) };
  }

  [TestCaseSource(nameof(YieldTestCases_GetCustomAttributeDataList_OfParameterInfo))]
  public void GetCustomAttributeDataList_OfParameterInfo(ParameterInfo param, Type attributeTypeWhichExpectedToBeContained)
    => AssertCustomAttributeDataList(param.GetCustomAttributeDataList(), attributeTypeWhichExpectedToBeContained);

  private static System.Collections.IEnumerable YieldTestCases_GetCustomAttributeDataList_OfAssembly()
  {
    yield return new object[] {
      typeof(ICustomAttributeProviderExtensionsTestsAssemblyAttribute).Assembly,
      typeof(ICustomAttributeProviderExtensionsTestsAssemblyAttribute)
    };
  }

  [TestCaseSource(nameof(YieldTestCases_GetCustomAttributeDataList_OfAssembly))]
  public void GetCustomAttributeDataList_OfAssembly(Assembly assm, Type attributeTypeWhichExpectedToBeContained)
    => AssertCustomAttributeDataList(assm.GetCustomAttributeDataList(), attributeTypeWhichExpectedToBeContained);

  private static System.Collections.IEnumerable YieldTestCases_GetCustomAttributeDataList_OfModule()
  {
    yield return new object[] {
      typeof(ICustomAttributeProviderExtensionsTestsModuleAttribute).Module,
      typeof(ICustomAttributeProviderExtensionsTestsModuleAttribute)
    };
  }

  [TestCaseSource(nameof(YieldTestCases_GetCustomAttributeDataList_OfModule))]
  public void GetCustomAttributeDataList_OfModule(Module module, Type attributeTypeWhichExpectedToBeContained)
    => AssertCustomAttributeDataList(module.GetCustomAttributeDataList(), attributeTypeWhichExpectedToBeContained);

  [Test]
  public void GetCustomAttributeDataList_ArgumentNull()
    => Assert.Throws<ArgumentNullException>(() => ((ICustomAttributeProvider)null!).GetCustomAttributeDataList());

  private class CustomAttributeProvider : ICustomAttributeProvider {
    public object[] GetCustomAttributes (bool inherit) => throw new NotImplementedException();
    public object[] GetCustomAttributes (Type attributeType, bool inherit) => throw new NotImplementedException();
    public bool IsDefined (Type attributeType, bool inherit) => throw new NotImplementedException();
  }

  [Test]
  public void GetCustomAttributeDataList_UnsupportedAttributeProvider()
  {
    CustomAttributeProvider provider = new();

    Assert.Throws<ArgumentException>(() => provider.GetCustomAttributeDataList());
  }
}
