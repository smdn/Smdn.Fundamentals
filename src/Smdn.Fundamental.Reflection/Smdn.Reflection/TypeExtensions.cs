// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Smdn.Reflection;

public static class TypeExtensions {
  public static bool IsDelegate(this Type t) => t.IsSubclassOf(typeof(System.Delegate)) || t == typeof(System.Delegate);
  public static bool IsConcreteDelegate(this Type t) => t != typeof(System.Delegate) && t != typeof(System.MulticastDelegate) && t.IsSubclassOf(typeof(System.Delegate));

  public static bool IsEnumFlags(this Type t) => t.IsEnum && t.GetCustomAttribute<FlagsAttribute>() != null;

  public static bool IsReadOnlyValueType(this Type t) => t.IsValueType && t.GetCustomAttributes(false).Any(a => string.Equals(a.GetType().FullName, "System.Runtime.CompilerServices.IsReadOnlyAttribute", StringComparison.Ordinal));
  public static bool IsByRefLikeValueType(this Type t) => t.IsValueType && t.GetCustomAttributes(false).Any(a => string.Equals(a.GetType().FullName, "System.Runtime.CompilerServices.IsByRefLikeAttribute", StringComparison.Ordinal));

  public static MethodInfo GetDelegateSignatureMethod(this Type t)
    => IsDelegate(t) ? t.GetMethod("Invoke") : null;

  public static IEnumerable<Type> GetExplicitBaseTypeAndInterfaces(this Type t)
  {
    if (t.IsEnum || t.IsDelegate())
      yield break;

    // explicit base type
    if (t.BaseType != null && t.BaseType != typeof(object) && t.BaseType != typeof(ValueType))
      yield return t.BaseType;

    // interfaces
    var allInterfaces = t.GetInterfaces();
    var interfaces = allInterfaces.Except(allInterfaces.SelectMany(i => i.GetInterfaces())) // flatten
                                  .Except(t.BaseType?.GetInterfaces() ?? Type.EmptyTypes);

    foreach (var iface in interfaces)
      yield return iface;
  }

  public static IEnumerable<string> GetNamespaces(this Type t)
    => GetAllNamespaces(t, type => false).Distinct();

  public static IEnumerable<string> GetNamespaces(this Type t, Func<Type, bool> isLanguagePrimitive)
    => GetAllNamespaces(t, isLanguagePrimitive).Distinct();

  private static IEnumerable<string> GetAllNamespaces(this Type t, Func<Type, bool> isLanguagePrimitive)
  {
    var elementType = t.IsArray || t.IsByRef || t.IsPointer
      ? t.GetElementType()
      : Nullable.GetUnderlyingType(t);

    if (elementType is not null) {
      foreach (var ns in GetNamespaces(elementType, isLanguagePrimitive))
        yield return ns;

      yield break;
    }

    if (t.IsGenericParameter && t.ContainsGenericParameters)
      yield break;

    if (t.IsConstructedGenericType || (t.IsGenericType && t.ContainsGenericParameters)) {
      yield return t.Namespace;

      foreach (var ns in t.GetGenericArguments().SelectMany(type => GetNamespaces(type, isLanguagePrimitive)))
        yield return ns;

      yield break;
    }

    if (!isLanguagePrimitive(t))
      yield return t.Namespace;
  }

  public static string GetGenericTypeName(this Type t)
  {
    if (!t.IsGenericType)
      throw new ArgumentException($"{t} is not a generic type", nameof(t));

    var name = t.GetGenericTypeDefinition().Name;
    var posTypeArgsDelimiter = name.LastIndexOf('`');

    return 0 < posTypeArgsDelimiter
      ? name.Substring(0, posTypeArgsDelimiter)
      : name;
  }

  private struct DefaultLayoutStruct { }
  private static readonly StructLayoutAttribute DefaultStructLayoutAttribute = typeof(DefaultLayoutStruct).StructLayoutAttribute;

  /// <remarks>The value of <see ref="StructLayoutAttribute.Size"> is not considered.</remarks>
  public static bool IsStructLayoutDefault(this Type t)
  {
    if (t is null)
      throw new ArgumentNullException(nameof(t));
    if (!t.IsValueType || t.IsEnum || t == typeof(ValueType))
      throw new ArgumentException($"{t} is not a struct type", nameof(t));

    return
      t.StructLayoutAttribute.Value == DefaultStructLayoutAttribute.Value &&
      t.StructLayoutAttribute.Pack == DefaultStructLayoutAttribute.Pack &&
#if false
      t.StructLayoutAttribute.Size == 0 &&
#endif
      t.StructLayoutAttribute.CharSet == DefaultStructLayoutAttribute.CharSet;
  }
}
