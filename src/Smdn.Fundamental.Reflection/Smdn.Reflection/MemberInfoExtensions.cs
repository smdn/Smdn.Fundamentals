// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Reflection;

namespace Smdn.Reflection;

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

  public static bool IsHidingInheritedMember(this MemberInfo member, bool nonPublic)
  {
    if (member is null)
      throw new ArgumentNullException(nameof(member));
    if (member is ConstructorInfo)
      return false; // constructors can not be 'new'

    if (member is MethodInfo mayBeAccessor) {
#if !NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
#pragma warning disable CS8604
#endif
      if (mayBeAccessor.TryGetPropertyFromAccessorMethod(out var property))
        return IsHidingInheritedMember(property, nonPublic); // TODO: or throw exception?
      if (mayBeAccessor.TryGetEventFromAccessorMethod(out var ev))
        return IsHidingInheritedMember(ev, nonPublic); // TODO: or throw exception?
#pragma warning restore CS8604
    }

    if (member is FieldInfo mayBeEventBackingField) {
#if !NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
#pragma warning disable CS8604
#endif
      if (mayBeEventBackingField.TryGetEventFromBackingField(out var ev))
        return IsHidingInheritedMember(ev, nonPublic);
#pragma warning restore CS8604
    }

    var declaringType = member.DeclaringType;

    if (declaringType is null)
      return false; // XXX: ???

    /*
     * test nested types
     */
    var sameNameNestedTypesInHerarchy = declaringType.BaseType is null
      ? Enumerable.Empty<MemberInfo>()
      : TypeExtensions.EnumerateNestedTypeInFlattenHierarchy(
          declaringType.BaseType,
          member.Name,
          nonPublic,
          static t => !t.IsNestedPrivate // cannot hide private nested types
        );

    if (sameNameNestedTypesInHerarchy.Any())
      return true;

    /*
     * test inherited members
     */
    var memberBindingFlags = BindingFlags.FlattenHierarchy;

    // TODO: member.IsStatic ? BindingFlags.Static : BindingFlags.Instance
    memberBindingFlags |= BindingFlags.Instance | BindingFlags.Static;

    memberBindingFlags |= nonPublic
      ? BindingFlags.Public | BindingFlags.NonPublic
      : BindingFlags.Public;

    var sameNameMembersInHierarchy = TypeExtensions.EnumerateBaseTypeOrInterfaces(declaringType)
      .SelectMany(t => t.GetMember(member.Name, memberBindingFlags)) // inherited nested types are not included
      .Where(static member => member switch {
        // cannot hide private members / nested types
        FieldInfo f => !f.IsPrivate,
        MethodInfo m => !m.IsPrivate,
        PropertyInfo p => !PropertyInfoExtensions.IsPrivate(p),
        EventInfo ev => !EventInfoExtensions.IsPrivate(ev),

        // cannot hide constructors
        ConstructorInfo ctor => false,

        // exclude nested types (already tested on above)
        Type t => false,

        _ => true,
      });

    if (member is MethodInfo method) {
      if (!declaringType.IsInterface && method.IsOverride()) {
        // `override` method does not hide any inherited methods
        sameNameMembersInHierarchy =
          sameNameMembersInHierarchy.Where(static member => member is not MethodInfo);
      }
      else {
        // interface method and non-`override` method only hides the method which have same signature
        sameNameMembersInHierarchy = sameNameMembersInHierarchy
          .Where(member => member switch {
            MethodInfo methodWithSameName => MethodInfoExtensions.SignatureEqual(method, methodWithSameName),
            _ => true,
          });
      }
    }
    else if (member is PropertyInfo property && PropertyInfoExtensions.IsOverride(property)) {
      // `override` property does not hide any inherited properties
      sameNameMembersInHierarchy =
        sameNameMembersInHierarchy.Where(static member => member is not PropertyInfo);
    }
    else if (member is EventInfo ev && EventInfoExtensions.IsOverride(ev)) {
      // `override` event does not hide any inherited events
      sameNameMembersInHierarchy =
        sameNameMembersInHierarchy.Where(static member => member is not EventInfo);
    }

    return sameNameMembersInHierarchy.Any();
  }
}
