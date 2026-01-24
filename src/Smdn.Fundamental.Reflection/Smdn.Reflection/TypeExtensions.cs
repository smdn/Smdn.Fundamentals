// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using Smdn.Reflection.Attributes;

namespace Smdn.Reflection;

public static class TypeExtensions {
  /// <summary>The utility functions which supports types from reflection-only context.</summary>
  private static class ROCType {
    public static bool Equals(Type type, Type t)
      =>
        string.Equals(type.FullName, t.FullName, StringComparison.Ordinal) &&
        string.Equals(type.Assembly.GetName().Name, t.Assembly.GetName().Name, StringComparison.Ordinal);

    public static bool IsSubclassOf(Type type, Type t)
    {
      if (Equals(type, t))
        return false;

      Type? ty = type;

      for (; ; ) {
        ty = ty.BaseType;

        if (ty is null)
          return false;

        if (Equals(ty, t))
          return true;
      }
    }

    public static bool IsValueType(Type type)
      => IsSubclassOf(type, typeof(ValueType));

    public static bool IsEnum(Type type)
      => IsSubclassOf(type, typeof(Enum));

    /// <returns>Returns <see cref="Type"/> which represents the underlying type if <paramref name="nullableType"/> is a <see cref="Nullable{T}"/>, otherwise <see langword="null"/>.</returns>
    public static Type? GetUnderlyingTypeOfNullable(Type nullableType)
    {
      if (!nullableType.IsGenericType) // is not Nullable<T>
        return null;

      if (nullableType.IsGenericTypeDefinition) // is not concrete type
        return null;

      if (Equals(nullableType.GetGenericTypeDefinition(), typeof(Nullable<>)))
        return nullableType.GetGenericArguments()[0];

      return null; // is not Nullable<T>
    }
  }

  public static bool IsDelegate(this Type t)
    =>
      ROCType.IsSubclassOf(t ?? throw new ArgumentNullException(nameof(t)), typeof(Delegate)) ||
      ROCType.Equals(t, typeof(Delegate));

  public static bool IsConcreteDelegate(this Type t)
    =>
      !ROCType.Equals(t ?? throw new ArgumentNullException(nameof(t)), typeof(Delegate)) &&
      !ROCType.Equals(t, typeof(MulticastDelegate)) &&
      ROCType.IsSubclassOf(t, typeof(Delegate));

  public static bool IsEnumFlags(this Type t)
    =>
      ROCType.IsEnum(t ?? throw new ArgumentNullException(nameof(t))) &&
      t.GetCustomAttributesData().Any(
        static d => string.Equals(d.AttributeType.FullName, typeof(FlagsAttribute).FullName, StringComparison.Ordinal)
      );

  public static bool IsReadOnlyValueType(this Type t)
    =>
      ROCType.IsValueType(t ?? throw new ArgumentNullException(nameof(t))) &&
      t.HasIsReadOnlyAttribute();

  public static bool IsByRefLikeValueType(this Type t)
    =>
      ROCType.IsValueType(t ?? throw new ArgumentNullException(nameof(t))) &&
      t.GetCustomAttributesData().Any(
        static d => string.Equals(d.AttributeType.FullName, "System.Runtime.CompilerServices.IsByRefLikeAttribute", StringComparison.Ordinal)
      );

  public static bool IsRecord(this Type t)
  {
    if (t is null)
      throw new ArgumentNullException(nameof(t));

    if (ROCType.IsEnum(t))
      return false; // enums cannot be record types
    if (t.IsInterface)
      return false; // interfaces cannot be record types
    if (IsDelegate(t))
      return false; // delegates cannot be record types

    // Records (C# reference) - Value equality
    // ref: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record#value-equality
    // "* An override of Object.Equals(Object). It's an error if the override is declared explicitly."
    // "* Overrides of operators == and !=. It's an error if the operators are declared explicitly."
    // "* If the record type is derived from a base record type Base, Equals(Base? other). It's an error if the override is declared explicitly."
    var areEqualityMethodsCompilerGenerated = true;

    // Type.GetMethod(string, BindingFlags, params Type[]) may throw following exception:
    //   System.ArgumentException : Type must be a type provided by the MetadataLoadContext. (Parameter 'types')
    // To avoid this, get the type of System.Object from the same context with `t` instead of using `typeof(object)`.
    var typeOfObject = GetTypeOfObject(t);

    areEqualityMethodsCompilerGenerated &= t
      .GetMethod(
        name: nameof(Equals),
        bindingAttr: BindingFlags.Instance | BindingFlags.Public,
        binder: null,
        types: [typeOfObject /* instead of typeof(object) */],
        modifiers: null
      )
      ?.HasCompilerGeneratedAttribute() ?? false;

    areEqualityMethodsCompilerGenerated &= t
      .GetMethod(
        name: "op_Equality",
        bindingAttr: BindingFlags.Static | BindingFlags.Public,
        binder: null,
        types: [t, t],
        modifiers: null
      )
      ?.HasCompilerGeneratedAttribute() ?? false;

    areEqualityMethodsCompilerGenerated &= t
      .GetMethod(
        name: "op_Inequality",
        bindingAttr: BindingFlags.Static | BindingFlags.Public,
        binder: null,
        types: [t, t],
        modifiers: null
      )
      ?.HasCompilerGeneratedAttribute() ?? false;

    if (t.BaseType is Type baseType && IsRecord(baseType)) {
      areEqualityMethodsCompilerGenerated &= t
        .GetMethod(
          name: nameof(Equals),
          bindingAttr: BindingFlags.Instance | BindingFlags.Public,
          binder: null,
          types: [baseType],
          modifiers: null
        )
        ?.HasCompilerGeneratedAttribute() ?? false;
    }

    return areEqualityMethodsCompilerGenerated;

    static Type GetTypeOfObject(Type? t)
    {
      for (; ; ) {
        if (t is null)
          throw new InvalidOperationException($"Could not get {typeof(Type)} of {typeof(object)}.");

        if (ROCType.Equals(t, typeof(object)))
          return t;

#if DEBUG
        if (t.IsInterface)
          throw new InvalidOperationException($"Can not get {typeof(Type)} of {typeof(object)} from interface types.");
#endif

        t = t.BaseType;
      }
    }
  }

  /// <returns>Returns <see cref="MethodInfo"/> which represents the signature of the delegate if <paramref name="t"/> is a <see cref="Delegate"/>, otherwise <see langword="null"/>.</returns>
  public static MethodInfo? GetDelegateSignatureMethod(this Type t)
    => IsDelegate(t ?? throw new ArgumentNullException(nameof(t))) ? t.GetMethod("Invoke") : null;

  public static IEnumerable<Type> GetExplicitBaseTypeAndInterfaces(this Type t)
  {
    return GetExplicitBaseTypeAndInterfacesCore(
      t ?? throw new ArgumentNullException(nameof(t))
    );

    static IEnumerable<Type> GetExplicitBaseTypeAndInterfacesCore(Type t)
    {
      if (ROCType.IsEnum(t) || t.IsDelegate())
        yield break;

      // explicit base type
      if (
        t.BaseType is not null &&
        !ROCType.Equals(t.BaseType, typeof(object)) &&
        !ROCType.Equals(t.BaseType, typeof(ValueType))
      ) {
        yield return t.BaseType;
      }

      // interfaces
      var allInterfaces = t.GetInterfaces();
      var interfaces = allInterfaces
        .Except(
          allInterfaces.SelectMany(static i => i.GetInterfaces()) // flatten
        )
        .Except(t.BaseType?.GetInterfaces() ?? Type.EmptyTypes);

      foreach (var iface in interfaces)
        yield return iface;
    }
  }

  public static IEnumerable<string> GetNamespaces(this Type t)
    => GetAllNamespaces(
      t ?? throw new ArgumentNullException(nameof(t)),
      static type => false
    ).Distinct();

  public static IEnumerable<string> GetNamespaces(this Type t, Func<Type, bool> isLanguagePrimitive)
    => GetAllNamespaces(
      t ?? throw new ArgumentNullException(nameof(t)),
      isLanguagePrimitive ?? throw new ArgumentNullException(nameof(isLanguagePrimitive))
    ).Distinct();

  private static IEnumerable<string> GetAllNamespaces(Type t, Func<Type, bool> isLanguagePrimitive)
  {
    var elementType = t.IsArray || t.IsByRef || t.IsPointer
      ? t.GetElementType()
      : ROCType.GetUnderlyingTypeOfNullable(t);

    if (elementType is not null) {
      foreach (var ns in GetAllNamespaces(elementType, isLanguagePrimitive))
        yield return ns;

      yield break;
    }

    if (t.IsGenericParameter && t.ContainsGenericParameters)
      yield break;

    if (t.IsConstructedGenericType || (t.IsGenericType && t.ContainsGenericParameters)) {
      foreach (var ns in t.GetGenericArguments().SelectMany(type => GetAllNamespaces(type, isLanguagePrimitive)))
        yield return ns;
    }

    if (isLanguagePrimitive(t))
      yield break;

    if (t.Namespace is not null)
      yield return t.Namespace;
  }

  public static string GetGenericTypeName(this Type t)
  {
    if (t is null)
      throw new ArgumentNullException(nameof(t));
    if (!t.IsGenericType)
      throw new ArgumentException($"{t} is not a generic type", nameof(t));

    var name = t.GetGenericTypeDefinition().Name;
    var posTypeArgsDelimiter = name.LastIndexOf('`');

    return 0 < posTypeArgsDelimiter
      ? name.Substring(0, posTypeArgsDelimiter)
      : name;
  }

  private struct DefaultLayoutStruct { }
  private static readonly StructLayoutAttribute DefaultStructLayoutAttribute = typeof(DefaultLayoutStruct).StructLayoutAttribute!;

  /// <remarks>The value of <see ref="StructLayoutAttribute.Size"/> is not considered.</remarks>
  public static bool IsStructLayoutDefault(this Type t)
  {
    if (t is null)
      throw new ArgumentNullException(nameof(t));
    if (!t.IsValueType || t.IsEnum || t == typeof(ValueType))
      throw new ArgumentException($"{t} is not a struct type", nameof(t));
    if (t.StructLayoutAttribute is null)
      throw new InvalidOperationException($"{nameof(Type)}.{nameof(Type.StructLayoutAttribute)} is null");

    return
      t.StructLayoutAttribute.Value == DefaultStructLayoutAttribute.Value &&
      t.StructLayoutAttribute.Pack == DefaultStructLayoutAttribute.Pack &&
#if false
      t.StructLayoutAttribute.Size == 0 &&
#endif
      t.StructLayoutAttribute.CharSet == DefaultStructLayoutAttribute.CharSet;
  }

  public static bool IsHidingInheritedType(this Type t, bool nonPublic)
  {
    if (t is null)
      throw new ArgumentNullException(nameof(t));
    if (!t.IsNested)
      return false; // non-nested type never hides any types

    var bindingFlagsVisibility = nonPublic
      ? BindingFlags.Public | BindingFlags.NonPublic
      : BindingFlags.Public;

    // is hiding any nested type in type hierarchy?
    return EnumerateTypeHierarchy(t.DeclaringType!, includeSelf: false)
      .SelectMany(th => th.GetNestedTypes(bindingFlagsVisibility))
      .Any(tn =>
        !tn.IsNestedPrivate && // cannot hide nested private types
        string.Equals(tn.Name, t.Name, StringComparison.Ordinal)
      );
  }

  internal static IEnumerable<Type> EnumerateBaseTypeOrInterfaces(Type t)
  {
    if (t.IsInterface) {
      foreach (var i in t.GetInterfaces()) {
        yield return i;
      }
    }
    else if (t.BaseType is not null) {
      yield return t.BaseType;
    }
  }

  internal static IEnumerable<Type> EnumerateTypeHierarchy(Type t, bool includeSelf)
  {
    Type? ty = t;

    if (includeSelf)
      yield return ty;

    for (; ; ) {
      if ((ty = ty?.BaseType) is not null)
        yield return ty;
      else
        break;
    }
  }

  internal static IEnumerable<Type> EnumerateNestedTypeInFlattenHierarchy(
    Type t,
    string? name,
    bool nonPublic,
    Func<Type, bool>? predicate
  )
  {
    var bindingFlags = nonPublic ? BindingFlags.Public | BindingFlags.NonPublic : BindingFlags.Public;
    var typeHierarchy = EnumerateTypeHierarchy(t, includeSelf: true);

    var nestedTypes = name is null
      ? typeHierarchy.SelectMany(t => t.GetNestedTypes(bindingFlags))
#pragma warning disable IDE0004
      : (IEnumerable<Type>)typeHierarchy.Select(t => t.GetNestedType(name, bindingFlags)).Where(static t => t is not null);
#pragma warning restore IDE0004

    if (predicate is not null)
      nestedTypes = nestedTypes.Where(predicate);

    return nestedTypes;
  }
}
