// Smdn.Fundamental.Reflection.dll (Smdn.Fundamental.Reflection-3.9.1)
//   Name: Smdn.Fundamental.Reflection
//   AssemblyVersion: 3.9.1.0
//   InformationalVersion: 3.9.1+3b727c0b578dc23d7a0901ea3541b039f8bf3f05
//   TargetFramework: .NETCoreApp,Version=v8.0
//   Configuration: Release
//   Metadata: IsTrimmable=True
//   Metadata: RepositoryUrl=https://github.com/smdn/Smdn.Fundamentals
//   Metadata: RepositoryBranch=main
//   Metadata: RepositoryCommit=3b727c0b578dc23d7a0901ea3541b039f8bf3f05
//   Referenced assemblies:
//     System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Collections.Immutable, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Linq, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Memory, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
#nullable enable annotations

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    AdditionAssignment = 39,
    BitwiseAnd = 21,
    BitwiseAndAssignment = 44,
    BitwiseOr = 22,
    BitwiseOrAssignment = 45,
    CheckedAddition = 32,
    CheckedAdditionAssignment = 52,
    CheckedDecrement = 31,
    CheckedDecrementAssignment = 51,
    CheckedDivision = 35,
    CheckedDivisionAssignment = 55,
    CheckedExplicit = 36,
    CheckedIncrement = 30,
    CheckedIncrementAssignment = 50,
    CheckedMultiplicationAssignment = 54,
    CheckedMultiply = 34,
    CheckedSubtraction = 33,
    CheckedSubtractionAssignment = 53,
    CheckedUnaryNegation = 29,
    Constructor = 1,
    Decrement = 15,
    DecrementAssignment = 38,
    Division = 19,
    DivisionAssignment = 42,
    Equality = 2,
    ExclusiveOr = 23,
    ExclusiveOrAssignment = 46,
    Explicit = 26,
    False = 13,
    GreaterThan = 5,
    GreaterThanOrEqual = 7,
    Implicit = 27,
    Increment = 14,
    IncrementAssignment = 37,
    Inequality = 3,
    LeftShift = 25,
    LeftShiftAssignment = 47,
    LessThan = 4,
    LessThanOrEqual = 6,
    LogicalNot = 10,
    Modulus = 20,
    ModulusAssignment = 43,
    MultiplicationAssignment = 41,
    Multiply = 18,
    None = 0,
    OnesComplement = 11,
    RightShift = 24,
    RightShiftAssignment = 48,
    Subtraction = 17,
    SubtractionAssignment = 40,
    True = 12,
    UnaryNegation = 9,
    UnaryPlus = 8,
    Unknown = -1,
    UnsignedRightShift = 28,
    UnsignedRightShiftAssignment = 49,
  }

  public static class EventInfoExtensions {
    public static FieldInfo? GetBackingField(this EventInfo ev) {}
    public static IEnumerable<MethodInfo> GetMethods(this EventInfo ev) {}
    public static IEnumerable<MethodInfo> GetMethods(this EventInfo ev, bool nonPublic) {}
    public static bool IsOverride(this EventInfo ev) {}
    public static bool IsReadOnly(this EventInfo ev) {}
    public static bool IsStatic(this EventInfo ev) {}
  }

  public static class FieldInfoExtensions {
    public static bool IsEventBackingField(this FieldInfo f) {}
    public static bool IsPropertyBackingField(this FieldInfo f) {}
    public static bool IsReadOnly(this FieldInfo f) {}
    public static bool TryGetEventFromBackingField(this FieldInfo backingField, [NotNullWhen(true)] out EventInfo? ev) {}
    public static bool TryGetPropertyFromBackingField(this FieldInfo backingField, [NotNullWhen(true)] out PropertyInfo? property) {}
  }

  public static class MemberInfoExtensions {
    public static Accessibility GetAccessibility(this MemberInfo member) {}
    public static bool IsHidingInheritedMember(this MemberInfo member, bool nonPublic) {}
    public static bool IsPrivateOrAssembly(this MemberInfo member) {}
  }

  public static class MethodBaseExtensions {
    public static MethodInfo? FindExplicitInterfaceMethod(this MethodBase m, bool findOnlyPublicInterfaces = false) {}
    public static MethodSpecialName GetNameType(this MethodBase m) {}
    public static IEnumerable<Type> GetSignatureTypes(this MethodBase m) {}
    public static bool IsExplicitlyImplemented(this MethodBase m) {}
    public static bool TryFindExplicitInterfaceMethod(this MethodBase m, out MethodInfo? explicitInterfaceMethod, bool findOnlyPublicInterfaces = false) {}
  }

  public static class MethodInfoExtensions {
    public static bool IsDelegateSignatureMethod(this MethodInfo m) {}
    public static bool IsEventAccessorMethod(this MethodInfo m) {}
    public static bool IsEventAddMethod(this MethodInfo m) {}
    public static bool IsEventRemoveMethod(this MethodInfo m) {}
    [Obsolete("use IsOverride instead")]
    public static bool IsOverridden(this MethodInfo m) {}
    public static bool IsOverride(this MethodInfo m) {}
    public static bool IsPropertyAccessorMethod(this MethodInfo m) {}
    public static bool IsPropertyGetMethod(this MethodInfo m) {}
    public static bool IsPropertySetMethod(this MethodInfo m) {}
    public static bool IsReadOnly(this MethodInfo m) {}
    public static bool TryGetEventFromAccessorMethod(this MethodInfo? accessor, [NotNullWhen(true)] out EventInfo? ev) {}
    public static bool TryGetPropertyFromAccessorMethod(this MethodInfo? accessor, [NotNullWhen(true)] out PropertyInfo? property) {}
  }

  public static class ParameterInfoExtensions {
    public static EventInfo? GetDeclaringEvent(this ParameterInfo param) {}
    public static PropertyInfo? GetDeclaringProperty(this ParameterInfo param) {}
    public static bool IsReturnParameter(this ParameterInfo param) {}
  }

  public static class PropertyInfoExtensions {
    public static FieldInfo? GetBackingField(this PropertyInfo property) {}
    public static bool IsAccessorReadOnly(this PropertyInfo property) {}
    public static bool IsOverride(this PropertyInfo property) {}
    public static bool IsSetMethodInitOnly(this PropertyInfo property) {}
    public static bool IsStatic(this PropertyInfo property) {}
  }

  public static class TypeExtensions {
    public static MethodInfo? GetDelegateSignatureMethod(this Type t) {}
    public static IEnumerable<Type> GetExplicitBaseTypeAndInterfaces(this Type t) {}
    public static string GetGenericTypeName(this Type t) {}
    public static IEnumerable<string> GetNamespaces(this Type t) {}
    public static IEnumerable<string> GetNamespaces(this Type t, Func<Type, bool> isLanguagePrimitive) {}
    public static bool IsByRefLikeValueType(this Type t) {}
    public static bool IsConcreteDelegate(this Type t) {}
    public static bool IsDelegate(this Type t) {}
    public static bool IsEnumFlags(this Type t) {}
    public static bool IsHidingInheritedType(this Type t, bool nonPublic) {}
    public static bool IsReadOnlyValueType(this Type t) {}
    public static bool IsRecord(this Type t) {}
    public static bool IsStructLayoutDefault(this Type t) {}
  }
}

namespace Smdn.Reflection.Attributes {
  public static class CustomAttributeTypedArgumentExtensions {
    public static object? GetTypedValue(this CustomAttributeTypedArgument typedArg) {}
  }

  public static class ICustomAttributeProviderExtensions {
    public static IList<CustomAttributeData> GetCustomAttributeDataList(this ICustomAttributeProvider attributeProvider) {}
    public static bool HasCompilerGeneratedAttribute(this ICustomAttributeProvider attributeProvider) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.7.1.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.5.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
