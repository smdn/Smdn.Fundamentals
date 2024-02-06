// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Smdn.Reflection;

public static class MethodBaseExtensions {
  public static IEnumerable<Type> GetSignatureTypes(this MethodBase m)
  {
    return GetSignatureTypesCore(
      m ?? throw new ArgumentNullException(nameof(m))
    );

    static IEnumerable<Type> GetSignatureTypesCore(MethodBase m)
    {
      foreach (var p in m.GetParameters())
        yield return p.ParameterType;

      if (m is MethodInfo mm)
        yield return mm.ReturnType;
    }
  }

  public static bool IsExplicitlyImplemented(this MethodBase m)
    => TryFindExplicitInterfaceMethod(
      m ?? throw new ArgumentNullException(nameof(m)),
      out _,
      findOnlyPublicInterfaces: false,
      throwException: false
    );

  public static bool TryFindExplicitInterfaceMethod(
    this MethodBase m,
    out MethodInfo? explicitInterfaceMethod,
    bool findOnlyPublicInterfaces = false
  )
    => TryFindExplicitInterfaceMethod(
      m, // allow null
      out explicitInterfaceMethod,
      findOnlyPublicInterfaces,
      throwException: false
    );

  public static MethodInfo? FindExplicitInterfaceMethod(this MethodBase m, bool findOnlyPublicInterfaces = false)
  {
    TryFindExplicitInterfaceMethod(
      m ?? throw new ArgumentNullException(nameof(m)),
      out var im,
      findOnlyPublicInterfaces,
      throwException: true
    );

    return im;
  }

  private static bool TryFindExplicitInterfaceMethod(
    MethodBase? m,
    out MethodInfo? explicitInterfaceMethod,
    bool findOnlyPublicInterfaces,
    bool throwException
  )
  {
    static IEnumerable<Type> GetInterfaces(Type t, bool findOnlyPublicInterfaces)
    {
      foreach (var iface in t.GetInterfaces()) {
        if (findOnlyPublicInterfaces && !(iface.IsPublic || iface.IsNestedPublic || iface.IsNestedFamily || iface.IsNestedFamORAssem))
          continue;

        yield return iface;
      }
    }

    explicitInterfaceMethod = default;

    if (m is not MethodInfo im)
      return false;

    if (!(im.IsStatic || im.IsFinal)) // explicit interface method must be final or static (in case of static interface members)
      return false;
    if (!im.IsPrivate) // explicit interface method must be private
      return false;
    if (im.DeclaringType is null) {
      return throwException
        ? throw new NotSupportedException($"can not get {nameof(MemberInfo.DeclaringType)} of {m}")
        : false;
    }

    if (im.DeclaringType.IsInterface) {
      // when 'm' is
      // - a method of an interface, and
      // - a method that explicitly implements a method of another interface implemented by the interface
      // e.g. m = System.Numerics.INumberBase`1.System.IUtf8SpanFormattable.TryFormat
      var indexOfTypeAndMemberDelimiter = m.Name.LastIndexOf('.');

      if (indexOfTypeAndMemberDelimiter < 0)
        return false;

      var interfaceTypeNameOfExplicitImplementation = m.Name.AsSpan(0, indexOfTypeAndMemberDelimiter);
      var interfaceMethodNameOfExplicitImplementation = m.Name.AsSpan(indexOfTypeAndMemberDelimiter + 1);

      foreach (var iface in GetInterfaces(im.DeclaringType, findOnlyPublicInterfaces)) {
        if (!interfaceTypeNameOfExplicitImplementation.Equals(iface.FullName, StringComparison.Ordinal))
          continue;

        foreach (var ifaceMethod in iface.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
          if (!interfaceMethodNameOfExplicitImplementation.Equals(ifaceMethod.Name, StringComparison.Ordinal))
            continue;

          explicitInterfaceMethod = ifaceMethod;

          return true;
        }
      }

      return false;
    }

    foreach (var iface in GetInterfaces(im.DeclaringType, findOnlyPublicInterfaces)) {
      InterfaceMapping interfaceMap = default;

      try {
        interfaceMap = im.DeclaringType.GetInterfaceMap(iface);
      }
      catch (NotSupportedException ex) {
        return throwException
          ? throw new NotSupportedException("cannot get interface map on assemblies loaded in reflection-only context", ex)
          : false;
      }

      for (var index = 0; index < interfaceMap.TargetMethods.Length; index++) {
        if (interfaceMap.TargetMethods[index] == im) {
          explicitInterfaceMethod = interfaceMap.InterfaceMethods[index];

          return true;
        }
      }
    }

    return false;
  }

  private static readonly Dictionary<string, MethodSpecialName> SpecialMethodNames = new(StringComparer.Ordinal) {
    // comparison
    { "op_Equality", MethodSpecialName.Equality },
    { "op_Inequality", MethodSpecialName.Inequality },
    { "op_LessThan", MethodSpecialName.LessThan },
    { "op_GreaterThan", MethodSpecialName.GreaterThan },
    { "op_LessThanOrEqual", MethodSpecialName.LessThanOrEqual },
    { "op_GreaterThanOrEqual", MethodSpecialName.GreaterThanOrEqual },

    // unary
    { "op_UnaryPlus", MethodSpecialName.UnaryPlus },
    { "op_UnaryNegation", MethodSpecialName.UnaryNegation },
    { "op_CheckedUnaryNegation", MethodSpecialName.CheckedUnaryNegation },
    { "op_LogicalNot", MethodSpecialName.LogicalNot },
    { "op_OnesComplement", MethodSpecialName.OnesComplement },
    { "op_True", MethodSpecialName.True },
    { "op_False", MethodSpecialName.False },
    { "op_Increment", MethodSpecialName.Increment },
    { "op_CheckedIncrement", MethodSpecialName.CheckedIncrement },
    { "op_Decrement", MethodSpecialName.Decrement },
    { "op_CheckedDecrement", MethodSpecialName.CheckedDecrement },

    // binary
    { "op_Addition", MethodSpecialName.Addition },
    { "op_CheckedAddition", MethodSpecialName.CheckedAddition },
    { "op_Subtraction", MethodSpecialName.Subtraction },
    { "op_CheckedSubtraction", MethodSpecialName.CheckedSubtraction },
    { "op_Multiply", MethodSpecialName.Multiply },
    { "op_CheckedMultiply", MethodSpecialName.CheckedMultiply },
    { "op_Division", MethodSpecialName.Division },
    { "op_CheckedDivision", MethodSpecialName.CheckedDivision },
    { "op_Modulus", MethodSpecialName.Modulus },
    { "op_BitwiseAnd", MethodSpecialName.BitwiseAnd },
    { "op_BitwiseOr", MethodSpecialName.BitwiseOr },
    { "op_ExclusiveOr", MethodSpecialName.ExclusiveOr },
    { "op_RightShift", MethodSpecialName.RightShift },
    { "op_UnsignedRightShift", MethodSpecialName.UnsignedRightShift },
    { "op_LeftShift", MethodSpecialName.LeftShift },

    // type cast
    { "op_Explicit", MethodSpecialName.Explicit },
    { "op_CheckedExplicit", MethodSpecialName.CheckedExplicit },
    { "op_Implicit", MethodSpecialName.Implicit },
  };

  public static MethodSpecialName GetNameType(this MethodBase m)
  {
    if (m is null)
      throw new ArgumentNullException(nameof(m));

    if (!m.IsSpecialName)
      return MethodSpecialName.None;

    if (SpecialMethodNames.TryGetValue(m.Name, out var methodSpecialName))
      return methodSpecialName;

    if (m is ConstructorInfo)
      return MethodSpecialName.Constructor;

    return MethodSpecialName.Unknown;
  }
}
