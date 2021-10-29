// Smdn.Fundamental.Reflection.dll (Smdn.Fundamental.Reflection-3.0.1 (net5.0))
//   Name: Smdn.Fundamental.Reflection
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1 (net5.0)
//   TargetFramework: .NETCoreApp,Version=v5.0
//   Configuration: Release

using System;
using System.Collections.Generic;
using System.Reflection;
using Smdn.Reflection;

namespace Smdn.Reflection {
  public enum Accessibility : int {
    Assembly = 3,
    Family = 4,
    FamilyAndAssembly = 2,
    FamilyOrAssembly = 5,
    Private = 1,
    Public = 6,
    Undefined = 0,
  }

  public enum MethodSpecialName : int {
    Addition = 16,
    BitwiseAnd = 21,
    BitwiseOr = 22,
    Constructor = 1,
    Decrement = 15,
    Division = 19,
    Equality = 2,
    ExclusiveOr = 23,
    Explicit = 26,
    False = 13,
    GreaterThan = 5,
    GreaterThanOrEqual = 7,
    Implicit = 27,
    Increment = 14,
    Inequality = 3,
    LeftShift = 25,
    LessThan = 4,
    LessThanOrEqual = 6,
    LogicalNot = 10,
    Modulus = 20,
    Multiply = 18,
    None = 0,
    OnesComplement = 11,
    RightShift = 24,
    Subtraction = 17,
    True = 12,
    UnaryNegation = 9,
    UnaryPlus = 8,
    Unknown = -1,
  }

  public static class EventInfoExtensions {
    public static IEnumerable<MethodInfo> GetMethods(this EventInfo ev) {}
    public static IEnumerable<MethodInfo> GetMethods(this EventInfo ev, bool nonPublic) {}
  }

  public static class MemberInfoExtensions {
    public static Accessibility GetAccessibility(this MemberInfo member) {}
    public static bool IsPrivateOrAssembly(this MemberInfo member) {}
  }

  public static class MethodBaseExtensions {
    public static MethodInfo FindExplicitInterfaceMethod(this MethodBase m, bool findOnlyPublicInterfaces = false) {}
    public static MethodSpecialName GetNameType(this MethodBase m) {}
    public static IEnumerable<Type> GetSignatureTypes(this MethodBase m) {}
    public static bool IsExplicitlyImplemented(this MethodBase m) {}
  }

  public static class PropertyInfoExtensions {
    public static bool IsSetMethodInitOnly(this PropertyInfo property) {}
  }

  public static class TypeExtensions {
    public static MethodInfo GetDelegateSignatureMethod(this Type t) {}
    public static IEnumerable<Type> GetExplicitBaseTypeAndInterfaces(this Type t) {}
    public static string GetGenericTypeName(this Type t) {}
    public static IEnumerable<string> GetNamespaces(this Type t) {}
    public static IEnumerable<string> GetNamespaces(this Type t, Func<Type, bool> isLanguagePrimitive) {}
    public static bool IsByRefLikeValueType(this Type t) {}
    public static bool IsConcreteDelegate(this Type t) {}
    public static bool IsDelegate(this Type t) {}
    public static bool IsEnumFlags(this Type t) {}
    public static bool IsReadOnlyValueType(this Type t) {}
  }
}

