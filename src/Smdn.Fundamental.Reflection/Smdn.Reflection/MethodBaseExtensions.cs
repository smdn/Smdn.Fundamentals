// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Smdn.Reflection {
  public static class MethodBaseExtensions {
    public static IEnumerable<Type> GetSignatureTypes(this MethodBase m)
    {
      foreach (var p in m.GetParameters())
        yield return p.ParameterType;

      if (m is MethodInfo mm)
        yield return mm.ReturnType;
    }

    public static bool IsExplicitlyImplemented(this MethodBase m)
    {
      return FindExplicitInterfaceMethod(m, findOnlyPublicInterfaces: false) != null;
    }

    public static MethodInfo FindExplicitInterfaceMethod(this MethodBase m, bool findOnlyPublicInterfaces = false)
    {
      if (m is MethodInfo im && im.IsFinal && im.IsPrivate) {
        foreach (var iface in im.DeclaringType.GetInterfaces()) {
          if (findOnlyPublicInterfaces && !(iface.IsPublic || iface.IsNestedPublic || iface.IsNestedFamily || iface.IsNestedFamORAssem))
            continue;

          var interfaceMap = im.DeclaringType.GetInterfaceMap(iface);

          for (var index = 0; index < interfaceMap.TargetMethods.Length; index++) {
            if (interfaceMap.TargetMethods[index] == im)
              return interfaceMap.InterfaceMethods[index];
          }
        }
      }

      return null;
    }

    private static readonly Dictionary<string, MethodSpecialName> specialMethodNames = new Dictionary<string, MethodSpecialName>(StringComparer.Ordinal) {
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
      { "op_LogicalNot", MethodSpecialName.LogicalNot },
      { "op_OnesComplement", MethodSpecialName.OnesComplement },
      { "op_True", MethodSpecialName.True },
      { "op_False", MethodSpecialName.False },
      { "op_Increment", MethodSpecialName.Increment },
      { "op_Decrement", MethodSpecialName.Decrement },

      // binary
      { "op_Addition", MethodSpecialName.Addition },
      { "op_Subtraction", MethodSpecialName.Subtraction },
      { "op_Multiply", MethodSpecialName.Multiply },
      { "op_Division", MethodSpecialName.Division },
      { "op_Modulus", MethodSpecialName.Modulus },
      { "op_BitwiseAnd", MethodSpecialName.BitwiseAnd },
      { "op_BitwiseOr", MethodSpecialName.BitwiseOr },
      { "op_ExclusiveOr", MethodSpecialName.ExclusiveOr },
      { "op_RightShift", MethodSpecialName.RightShift },
      { "op_LeftShift", MethodSpecialName.LeftShift },

      // type cast
      { "op_Explicit", MethodSpecialName.Explicit },
      { "op_Implicit", MethodSpecialName.Implicit },
    };

    public static MethodSpecialName GetNameType(this MethodBase m)
    {
      if (!m.IsSpecialName)
        return MethodSpecialName.None;

      if (specialMethodNames.TryGetValue(m.Name, out var methodSpecialName))
        return methodSpecialName;

      if (m is ConstructorInfo)
        return MethodSpecialName.Constructor;

      return MethodSpecialName.Unknown;
    }
  }
}
