// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

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
      t.GetCustomAttributesData().Any(
        static d => string.Equals(d.AttributeType.FullName, "System.Runtime.CompilerServices.IsReadOnlyAttribute", StringComparison.Ordinal)
      );

  public static bool IsByRefLikeValueType(this Type t)
    =>
      ROCType.IsValueType(t ?? throw new ArgumentNullException(nameof(t))) &&
      t.GetCustomAttributesData().Any(
        static d => string.Equals(d.AttributeType.FullName, "System.Runtime.CompilerServices.IsByRefLikeAttribute", StringComparison.Ordinal)
      );

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

  private static IEnumerable<string> GetAllNamespaces(this Type t, Func<Type, bool> isLanguagePrimitive)
  {
    var elementType = t.IsArray || t.IsByRef || t.IsPointer
      ? t.GetElementType()
      : ROCType.GetUnderlyingTypeOfNullable(t);

    if (elementType is not null) {
      foreach (var ns in GetNamespaces(elementType, isLanguagePrimitive))
        yield return ns;

      yield break;
    }

    if (t.IsGenericParameter && t.ContainsGenericParameters)
      yield break;

    if (t.IsConstructedGenericType || (t.IsGenericType && t.ContainsGenericParameters)) {
      if (t.Namespace is not null)
        yield return t.Namespace;

      foreach (var ns in t.GetGenericArguments().SelectMany(type => GetNamespaces(type, isLanguagePrimitive)))
        yield return ns;

      yield break;
    }

    if (!isLanguagePrimitive(t) && t.Namespace is not null)
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
    return EnumerateTypeHierarchy(t.DeclaringType!)
      .SelectMany(th => th.GetNestedTypes(bindingFlagsVisibility))
      .Any(tn =>
        !tn.IsNestedPrivate && // cannot hide nested private types
        string.Equals(tn.Name, t.Name, StringComparison.Ordinal)
      );

    static IEnumerable<Type> EnumerateTypeHierarchy(Type t)
    {
      Type? _t = t;

      for (; ; ) {
        if ((_t = _t?.BaseType) is not null)
          yield return _t;
        else
          break;
      }
    }
  }
}
