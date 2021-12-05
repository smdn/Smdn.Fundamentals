// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;

namespace Smdn.Reflection {
  public static class MemberInfoExtensions {
    public static Accessibility GetAccessibility(this MemberInfo member)
    {
      switch (member) {
        case null:
          throw new ArgumentNullException(nameof(member));

        case Type t:
          if (t.IsPublic || t.IsNestedPublic) return Accessibility.Public;
          if (t.IsNotPublic || t.IsNestedAssembly) return Accessibility.Assembly;
          if (t.IsNestedFamily) return Accessibility.Family;
          if (t.IsNestedFamORAssem) return Accessibility.FamilyOrAssembly;
          if (t.IsNestedFamANDAssem) return Accessibility.FamilyAndAssembly;
          if (t.IsNestedPrivate) return Accessibility.Private;
          break;

        case FieldInfo f:
          if (f.IsPublic) return Accessibility.Public;
          if (f.IsAssembly) return Accessibility.Assembly;
          if (f.IsFamily) return Accessibility.Family;
          if (f.IsFamilyOrAssembly) return Accessibility.FamilyOrAssembly;
          if (f.IsFamilyAndAssembly) return Accessibility.FamilyAndAssembly;
          if (f.IsPrivate) return Accessibility.Private;
          break;

        case MethodBase m:
          if (m.IsPublic) return Accessibility.Public;
          if (m.IsAssembly) return Accessibility.Assembly;
          if (m.IsFamily) return Accessibility.Family;
          if (m.IsFamilyOrAssembly) return Accessibility.FamilyOrAssembly;
          if (m.IsFamilyAndAssembly) return Accessibility.FamilyAndAssembly;
          if (m.IsPrivate) return Accessibility.Private;
          break;

        case PropertyInfo p:
          throw new InvalidOperationException("cannot get accessibility of " + nameof(PropertyInfo));

        case EventInfo ev:
          throw new InvalidOperationException("cannot get accessibility of " + nameof(EventInfo));

        default:
          throw new NotSupportedException("unknown member type");
      }

      return Accessibility.Undefined;
    }

    public static bool IsPrivateOrAssembly(this MemberInfo member)
    {
      switch (member) {
        case null:
          throw new ArgumentNullException(nameof(member));

        case Type t:
          if (t.IsNotPublic) return true;
          if (t.IsNestedAssembly) return true;
          if (t.IsNestedFamANDAssem) return true;
          if (t.IsNestedPrivate) return true;

          return false;

        case FieldInfo f:
          if (f.IsPrivate) return true;
          if (f.IsAssembly) return true;
          if (f.IsFamilyAndAssembly) return true;

          return false;

        case MethodBase m:
          if (m.IsPrivate) return true;
          if (m.IsAssembly) return true;
          if (m.IsFamilyAndAssembly) return true;

          return false;

        case PropertyInfo p:
          throw new InvalidOperationException("cannot get accessibility of " + nameof(PropertyInfo));

        case EventInfo ev:
          throw new InvalidOperationException("cannot get accessibility of " + nameof(EventInfo));

        default:
          throw new NotSupportedException("unknown member type");
      }
    }
  }
}
