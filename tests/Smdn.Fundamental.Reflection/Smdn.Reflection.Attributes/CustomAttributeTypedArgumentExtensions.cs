// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Smdn.Reflection.Attributes;

[TestFixture()]
public class CustomAttributeTypedArgumentExtensionsTests {
  [AttributeUsage(AttributeTargets.All)]
  private class TestAttribute : Attribute {
    public TestAttribute(
      DateTimeKind arg0,
      int arg1,
      string arg2
    )
    { }
  }

  [TestAttribute(DateTimeKind.Local, 42, "foo")]
  private class C { }

  private static System.Collections.IEnumerable YieldTestCases_GetTypedValue()
  {
    var ctorArgs = CustomAttributeData
      .GetCustomAttributes(typeof(C))
      .First(a => a.AttributeType == typeof(TestAttribute))
      .ConstructorArguments;

    yield return new object[] { ctorArgs[0], typeof(DateTimeKind), DateTimeKind.Local };
    yield return new object[] { ctorArgs[1], typeof(int), 42 };
    yield return new object[] { ctorArgs[2], typeof(string), "foo" };
  }

  [TestCaseSource(nameof(YieldTestCases_GetTypedValue))]
  public void GetTypedValue(CustomAttributeTypedArgument typedArg, Type expectedType, object expectedValue)
  {
    object val = typedArg.GetTypedValue()!;

    Assert.That(val.GetType(), Is.EqualTo(expectedType), "type of value");
    Assert.That(val, Is.EqualTo(expectedValue), "value");
  }
}
