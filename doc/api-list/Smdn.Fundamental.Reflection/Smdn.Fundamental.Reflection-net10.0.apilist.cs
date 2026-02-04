// Smdn.Fundamental.Reflection.dll (Smdn.Fundamental.Reflection-3.10.0)
//   Name: Smdn.Fundamental.Reflection
//   AssemblyVersion: 3.10.0.0
//   InformationalVersion: 3.10.0+0c4f7eca96a3065716196aecf88ea084671726e6
//   TargetFramework: .NETCoreApp,Version=v10.0
//   Configuration: Release
//   Metadata: IsAotCompatible=True
//   Metadata: RepositoryUrl=https://github.com/smdn/Smdn.Fundamentals
//   Metadata: RepositoryBranch=main
//   Metadata: RepositoryCommit=0c4f7eca96a3065716196aecf88ea084671726e6
//   Referenced assemblies:
//     System.Collections, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Collections.Immutable, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Linq, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Memory, Version=10.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Runtime, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
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
    public static bool IsFixedBuffer(this FieldInfo f) {}
    public static bool IsPropertyBackingField(this FieldInfo f) {}
    public static bool IsReadOnly(this FieldInfo f) {}
    public static bool IsRequired(this FieldInfo f) {}
    public static bool TryGetEventFromBackingField(this FieldInfo backingField, [NotNullWhen(true)] out EventInfo? ev) {}
    public static bool TryGetFixedBufferElementTypeAndLength(this FieldInfo f, [NotNullWhen(true)] out Type? elementType, out int length) {}
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
    public static MethodInfo? GetImmediateOverriddenMethod(this MethodInfo m) {}
    public static bool IsAsyncStateMachine(this MethodInfo m) {}
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
    public static bool TryGetExtensionMarkerType(this MethodInfo m, [NotNullWhen(true)] out Type? extensionMarkerType) {}
    public static bool TryGetExtensionParameter(this MethodInfo m, [NotNullWhen(true)] out ParameterInfo? extensionParameter) {}
    public static bool TryGetPropertyFromAccessorMethod(this MethodInfo? accessor, [NotNullWhen(true)] out PropertyInfo? property) {}
  }

  public static class ParameterInfoExtensions {
    public static bool CanTakeArbitraryLengthOfArgs(this ParameterInfo param) {}
    public static EventInfo? GetDeclaringEvent(this ParameterInfo param) {}
    public static PropertyInfo? GetDeclaringProperty(this ParameterInfo param) {}
    public static bool IsExtensionMethodFirstParameter(this ParameterInfo param) {}
    public static bool IsRefReadOnly(this ParameterInfo param) {}
    public static bool IsReturnParameter(this ParameterInfo param) {}
    public static bool IsScopedRef(this ParameterInfo param) {}
  }

  public static class PropertyInfoExtensions {
    public static FieldInfo? GetBackingField(this PropertyInfo property) {}
    public static bool IsAccessorReadOnly(this PropertyInfo property) {}
    public static bool IsOverride(this PropertyInfo property) {}
    public static bool IsRequired(this PropertyInfo property) {}
    public static bool IsSetMethodInitOnly(this PropertyInfo property) {}
    public static bool IsStatic(this PropertyInfo property) {}
    public static bool TryGetExtensionMarkerType(this PropertyInfo property, [NotNullWhen(true)] out Type? extensionMarkerType) {}
    public static bool TryGetExtensionParameter(this PropertyInfo property, [NotNullWhen(true)] out ParameterInfo? extensionParameter) {}
  }

  public static class TypeExtensions {
    public sealed class <G>$6B6159A298CA62A8C577E514BE40E3DD {
      public static class <M>$7CDC5A43786EC8158CD79D3A9001ECC1 {
        [CompilerGenerated]
        public static void <Extension>$(Type t) {}
      }

      [ExtensionMarker("<M>$7CDC5A43786EC8158CD79D3A9001ECC1")]
      public IEnumerable<Type> EnumerateExtensionGroupingTypes() {}
      [ExtensionMarker("<M>$7CDC5A43786EC8158CD79D3A9001ECC1")]
      public IEnumerable<(Type ExtensionMarkerType, ParameterInfo? ExtensionParameter)> EnumerateExtensionMarkerTypeAndParameterPairs() {}
      [ExtensionMarker("<M>$7CDC5A43786EC8158CD79D3A9001ECC1")]
      public IEnumerable<(MethodInfo ImplementationMethod, MethodInfo ExtensionMember)> EnumerateExtensionMemberAndImplementationPairs() {}
      [ExtensionMarker("<M>$7CDC5A43786EC8158CD79D3A9001ECC1")]
      public ParameterInfo? GetExtensionParameter() {}
      [ExtensionMarker("<M>$7CDC5A43786EC8158CD79D3A9001ECC1")]
      public bool HasExtensionMembers() {}
      [ExtensionMarker("<M>$7CDC5A43786EC8158CD79D3A9001ECC1")]
      public bool IsExtensionEnclosingClass() {}
      [ExtensionMarker("<M>$7CDC5A43786EC8158CD79D3A9001ECC1")]
      public bool IsExtensionGroupingType() {}
      [ExtensionMarker("<M>$7CDC5A43786EC8158CD79D3A9001ECC1")]
      public bool IsExtensionMarkerType() {}
    }

    public static IEnumerable<Type> EnumerateExtensionGroupingTypes(this Type t) {}
    public static IEnumerable<(Type ExtensionMarkerType, ParameterInfo? ExtensionParameter)> EnumerateExtensionMarkerTypeAndParameterPairs(this Type t) {}
    public static IEnumerable<(MethodInfo ImplementationMethod, MethodInfo ExtensionMember)> EnumerateExtensionMemberAndImplementationPairs(this Type t) {}
    public static MethodInfo? GetDelegateSignatureMethod(this Type t) {}
    public static IEnumerable<Type> GetExplicitBaseTypeAndInterfaces(this Type t) {}
    public static ParameterInfo? GetExtensionParameter(this Type t) {}
    public static string GetGenericTypeName(this Type t) {}
    public static IEnumerable<string> GetNamespaces(this Type t) {}
    public static IEnumerable<string> GetNamespaces(this Type t, Func<Type, bool> isLanguagePrimitive) {}
    public static bool HasExtensionMembers(this Type t) {}
    public static bool IsByRefLikeValueType(this Type t) {}
    public static bool IsConcreteDelegate(this Type t) {}
    public static bool IsDelegate(this Type t) {}
    public static bool IsEnumFlags(this Type t) {}
    public static bool IsExtensionEnclosingClass(this Type t) {}
    public static bool IsExtensionGroupingType(this Type t) {}
    public static bool IsExtensionMarkerType(this Type t) {}
    public static bool IsFixedBufferFieldType(this Type t) {}
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
