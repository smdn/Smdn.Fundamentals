// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if !NET8_0_OR_GREATER
#define ENABLE_SERIALIZATION // SYSLIB0011 (https://aka.ms/binaryformatter)
#endif

#if ENABLE_SERIALIZATION && SYSTEM_RUNTIME_SERIALIZATION_SERIALIZATIONBINDER

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Smdn.Test.NUnit.Assertion;

internal sealed class DeserializationBinder : SerializationBinder {
  private static readonly string[] TypeArgumentListSplitter = { "]," };

  private static Type GetTypeFromLoadedAssemblies(string typeName)
  {
    var typeFullName = typeName;
    string openGenericTypeFullName = null;

    var indexOfTypeArguments = typeName.IndexOf("[[", StringComparison.Ordinal);

    if (0 <= indexOfTypeArguments)
      openGenericTypeFullName = typeName.Substring(0, indexOfTypeArguments);

    var indexOfAssemblyName = (openGenericTypeFullName ?? typeName).IndexOf(", ", StringComparison.Ordinal);

    if (0 <= indexOfAssemblyName)
      typeFullName = (openGenericTypeFullName ?? typeName).Substring(0, indexOfAssemblyName);

    var type = AppDomain
      .CurrentDomain
      .GetAssemblies()
      .SelectMany(static a => a.GetTypes())
      .FirstOrDefault(t =>
        string.Equals(typeName, t.AssemblyQualifiedName, StringComparison.Ordinal) ||
        string.Equals(openGenericTypeFullName, t.FullName, StringComparison.Ordinal) ||
        string.Equals(typeFullName, t.FullName, StringComparison.Ordinal)
      ) ?? throw new InvalidOperationException($"could not find type {typeName}");

    if (!type.IsGenericType /* 0 <= indexOfTypeArguments */)
      return type;

    // get substring of generic type arguments: "T[[T1], [T2], [T3]]" -> "[[T1], [T2], [T3]]"
    var typeArgumentList = typeName.Substring(indexOfTypeArguments);

    var typeArguments = typeArgumentList
      .Substring(1, typeArgumentList.Length - 2) // remove outermost square brackets: "[[T1], [T2], [T3]]" -> "[T1], [T2], [T3]"
      .Split(TypeArgumentListSplitter, StringSplitOptions.None) // split type arguments: "[T1], [T2], [T3]" -> {"[T1", "[T2", "[T3]"}
      .Select(ta => ta.TrimStart('[').TrimEnd(']')) // trim square brackets: {"[T1", "[T2", "[T3]"} -> {"T1", "T2", "T3"}
      .Select(GetTypeFromLoadedAssemblies) // get types from type names
      .ToArray();

    return type.MakeGenericType(typeArguments);
  }

  public override Type BindToType(string assemblyName, string typeName)
  {
    // try to get type
    var typeToDeserialize =
      Type.GetType(typeName, throwOnError: false) ??
      GetTypeFromLoadedAssemblies(typeName) ??
      throw new InvalidOperationException($"could not bind to type: {typeName}, {assemblyName}");

    if (string.Equals(assemblyName, typeToDeserialize.Assembly.FullName, StringComparison.Ordinal))
      return typeToDeserialize;

    // is forwarded type?
    var attrTypeForwardedFrom = typeToDeserialize.GetCustomAttribute<System.Runtime.CompilerServices.TypeForwardedFromAttribute>();

    if (string.Equals(assemblyName, attrTypeForwardedFrom?.AssemblyFullName, StringComparison.Ordinal))
      return typeToDeserialize;

    throw new InvalidOperationException($"could not bind to type: {typeName}, {assemblyName} (a candidate for binding: {typeToDeserialize.AssemblyQualifiedName})");
  }
}
#endif
