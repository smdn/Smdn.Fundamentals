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
    Assert.That(list, Is.Not.Null);
    Assert.That(list, Is.Not.Empty);
    Assert.That(list.Select(cad => cad.AttributeType), Has.Member(attributeTypeWhichExpectedToBeContained));
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

  [Test]
  public void HasCompilerGeneratedAttribute_ArgumentNull()
    => Assert.That(
      () => ((ICustomAttributeProvider)null!).HasCompilerGeneratedAttribute(),
      Throws.ArgumentNullException
    );

  private class HasCompilerGeneratedAttributeTestCases {
    public int P0 { get; set; }
    public int P1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event EventHandler E0;
    public event EventHandler E1 {
      add => throw new NotImplementedException();
      remove => throw new NotImplementedException();
    }

    public record R {
      public void M() => throw new NotImplementedException();
    }
  }

  private static System.Collections.IEnumerable YieldTestCases_HasCompilerGeneratedAttribute()
  {
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetProperty(nameof(HasCompilerGeneratedAttributeTestCases.P0)),
      false,
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetProperty(nameof(HasCompilerGeneratedAttributeTestCases.P0))!.GetMethod,
      true,
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetProperty(nameof(HasCompilerGeneratedAttributeTestCases.P0))!.SetMethod,
      true,
    };

    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetProperty(nameof(HasCompilerGeneratedAttributeTestCases.P1)),
      false,
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetProperty(nameof(HasCompilerGeneratedAttributeTestCases.P1))!.GetMethod,
      false,
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetProperty(nameof(HasCompilerGeneratedAttributeTestCases.P1))!.SetMethod,
      false,
    };

    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetEvent(nameof(HasCompilerGeneratedAttributeTestCases.E0)),
      false,
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetEvent(nameof(HasCompilerGeneratedAttributeTestCases.E0))!.AddMethod,
      true,
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetEvent(nameof(HasCompilerGeneratedAttributeTestCases.E0))!.RemoveMethod,
      true,
    };

    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetEvent(nameof(HasCompilerGeneratedAttributeTestCases.E1)),
      false,
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetEvent(nameof(HasCompilerGeneratedAttributeTestCases.E1))!.AddMethod,
      false,
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).GetEvent(nameof(HasCompilerGeneratedAttributeTestCases.E1))!.RemoveMethod,
      false,
    };

    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases.R),
      false,
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases.R).GetMethod(nameof(Equals), BindingFlags.Instance | BindingFlags.Public, null, [typeof(object)], null),
      true,
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases.R).GetMethod(nameof(ToString), BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null),
      true,
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases.R).GetMethod(nameof(HasCompilerGeneratedAttributeTestCases.R.M), BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null),
      false,
    };

    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).Assembly,
      false, // does not throw
    };
    yield return new object[] {
      typeof(HasCompilerGeneratedAttributeTestCases).Module,
      false, // does not throw
    };
  }

  [TestCaseSource(nameof(YieldTestCases_HasCompilerGeneratedAttribute))]
  public void HasCompilerGeneratedAttribute(
    ICustomAttributeProvider attributeProvider,
    bool expected
  )
    => Assert.That(
      attributeProvider.HasCompilerGeneratedAttribute(),
      Is.EqualTo(expected)
    );
}
