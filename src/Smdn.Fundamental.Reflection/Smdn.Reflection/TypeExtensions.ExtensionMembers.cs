// SPDX-FileCopyrightText: 2026 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cspell:ignore specialname
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Smdn.Reflection.Attributes;

namespace Smdn.Reflection;

#pragma warning disable IDE0040
static partial class TypeExtensions {
#pragma warning restore IDE0040
  private const BindingFlags BindingFlagsForExtensionMembers =
    BindingFlags.Static |
    BindingFlags.Instance |
    BindingFlags.Public |
    BindingFlags.NonPublic |
    BindingFlags.DeclaredOnly;

  private const BindingFlags BindingFlagsForExtensionMarkerMethods =
    BindingFlags.Static |
    BindingFlags.Public |
    BindingFlags.DeclaredOnly;

  private const BindingFlags BindingFlagsForImplementationMethods =
    BindingFlags.Static |
    BindingFlags.Public |
    BindingFlags.NonPublic |
    BindingFlags.DeclaredOnly;

  private static bool IsClass(Type t)
  {
    if (t.IsInterface)
      return false; // interfaces cannot be a class
    if (ROCType.IsEnum(t))
      return false; // enums cannot be a class
    if (IsDelegate(t))
      return false; // delegates cannot be a class

    return true;
  }

  private static bool IsExtensionMarkerMethod(MethodInfo m)
  {
    // the extension marker methods are marked with the `specialname` flag
    if (!m.IsSpecialName)
      return false;

    // the extension marker methods are static, non-generic, void-returning
    if (!m.IsStatic)
      return false;
#if SYSTEM_REFLECTION_METHODBASE_ISCONSTRUCTEDGENERICMETHOD
    if (m.IsConstructedGenericMethod)
#else
    if (m.IsGenericMethod)
#endif
      return false;
    if (m.ReturnType != typeof(void))
      return false;

    // the extension marker method has a name "<Extension>$"
    return m.Name.Equals("<Extension>$", StringComparison.Ordinal);
  }

  /// <param name="t"><see cref="Type"/> instance to be tested.</param>
  /// <param name="extensionParameter">
  /// If the extension marker method has a parameter, <see cref="ParameterInfo"/> of that parameter is set;
  /// otherwise, <see langword="null"/> is set.
  /// </param>
  /// <returns>
  /// If <paramref name="t"/> is the extension marker type,
  /// an extension marker method exists in <paramref name="t"/>,
  /// and the extension marker method has an extension parameter, then <see langword="true"/>;
  /// otherwise <see langword="false"/>.
  /// </returns>
  private static bool IsExtensionMarkerTypeCore(
    Type t,
    out ParameterInfo? extensionParameter
  )
  {
    extensionParameter = null;

    if (!IsClass(t))
      return false;

    // the extension marker types are marked with the `specialname` flag
    if (!t.IsSpecialName)
      return false;

    // the extension marker types are public and static
    if (!t.IsNestedPublic)
      return false;
    if (!(t.IsAbstract && t.IsSealed)) // static class
      return false;

    return IsExtensionMarkerTypeUnchecked(t, out extensionParameter);
  }

  /// <param name="t"><see cref="Type"/> instance to be tested.</param>
  /// <param name="extensionParameter">
  /// If the extension marker method has a parameter, <see cref="ParameterInfo"/> of that parameter is set;
  /// otherwise, <see langword="null"/> is set.
  /// </param>
  /// <returns>
  /// If <paramref name="t"/> is the extension marker type,
  /// an extension marker method exists in <paramref name="t"/>,
  /// and the extension marker method has an extension parameter, then <see langword="true"/>;
  /// otherwise <see langword="false"/>.
  /// </returns>
  internal static bool IsExtensionMarkerTypeUnchecked(
    Type t,
    out ParameterInfo? extensionParameter
  )
  {
    // each extension marker type contains a single method, the extension marker method
    var extensionMarkerMethod = t
      .GetMethods(BindingFlagsForExtensionMarkerMethods)
      .FirstOrDefault(IsExtensionMarkerMethod);

    if (extensionMarkerMethod is null) {
      extensionParameter = null;

      // If no extension members are declared in an extension grouping type,
      // no extension marker method is generated.
      // Therefore, determine whether a type is an extension marker type based on
      // whether it is declared in an extension grouping type.
      if (t.DeclaringType is not { } declaringType)
        return false;

      return declaringType.IsExtensionGroupingType();
    }

    // each extension marker method has a single parameter, the extension parameter
    extensionParameter = extensionMarkerMethod
      .GetParameters()
      .FirstOrDefault();

    return true;
  }

#pragma warning disable CA1034 // see https://github.com/dotnet/sdk/issues/51681
#pragma warning disable CS1734 // see https://github.com/dotnet/roslyn/pull/81269
  /// <param name="t">The instance of the <see cref="Type"/> being invoked by the extension member.</param>
  extension(Type t) {
    /// <summary>
    /// Gets a value that indicates whether the type is an extension enclosing class or not.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="t"/> is <see langword="null"/>.</exception>
    /// <seealso href="https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-14.0/extensions#static-classes-as-extension-containers">
    /// Extension members - Static classes as extension containers
    /// </seealso>
    // this cannot be an extension property because since it must throw an ArgumentNullException
    public bool IsExtensionEnclosingClass()
    {
      if (t is null)
        throw new ArgumentNullException(nameof(t));

      // must be a class
      if (!IsClass(t))
        return false;

      // must be a static class (IsAbstract && IsSealed)
      if (!t.IsAbstract)
        return false;
      if (!t.IsSealed)
        return false;

      // must be a non-generic class
      if (t.IsGenericType)
        return false;

      // must be a top-level (not nested) class
      if (t.IsNested)
        return false;

      // must be marked with the [Extension] attribute
      if (!t.HasExtensionAttribute())
        return false;

      // must have one or more extension grouping types
      return t
        .GetNestedTypes(BindingFlags.Public | BindingFlags.DeclaredOnly)
        .Any(IsExtensionGroupingType);
    }

    /// <summary>
    /// Enumerates the extension grouping types from the <see cref="Type"/> of an extension enclosing class.
    /// </summary>
    /// <returns>
    /// Returns the <see cref="IEnumerable{T}"/> of the extension grouping types.
    /// Returns empty if <paramref name="t"/> is not a <see cref="Type"/> of an extension enclosing class.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="t"/> is <see langword="null"/>.</exception>
    /// <remarks>This method is implemented by using deferred execution.</remarks>
    public IEnumerable<Type> EnumerateExtensionGroupingTypes()
    {
      if (t is null)
        throw new ArgumentNullException(nameof(t));

      return t.IsExtensionEnclosingClass()
        ? t.GetNestedTypes().Where(IsExtensionGroupingType)
        : [];
    }

    /// <summary>
    /// Gets a value that indicates whether the type has one or more extension member or not.
    /// </summary>
    /// <remarks>
    /// An extension member is a method, a property, or an operator that is declared in an extension grouping type.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="t"/> is <see langword="null"/>.</exception>
    /// <seealso href="https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-14.0/extensions#extension-members-2">
    /// Extension members - Extension members
    /// </seealso>
    // this cannot be an extension property because since it must throw an ArgumentNullException
    public bool HasExtensionMembers()
    {
      if (t is null)
        throw new ArgumentNullException(nameof(t));

      return
        t.GetMethods(BindingFlagsForExtensionMembers).Any(static m => m.HasExtensionMarkerAttribute()) | // including operators
        t.GetProperties(BindingFlagsForExtensionMembers).Any(static p => p.HasExtensionMarkerAttribute());
    }

    /// <summary>
    /// Gets a value that indicates whether the type is an extension grouping type or not.
    /// </summary>
    /// <remarks>
    /// An extension grouping type is a type that groups extension methods, properties, or operators.
    /// The extension grouping types have names that start with <c>&lt;G&gt;$</c>.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="t"/> is <see langword="null"/>.</exception>
    /// <seealso href="https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-14.0/extensions#extension-grouping-types">
    /// Extension members - Extension grouping types
    /// </seealso>
    // this cannot be an extension property because since it must throw an ArgumentNullException
    public bool IsExtensionGroupingType()
    {
      if (t is null)
        throw new ArgumentNullException(nameof(t));

      if (!IsClass(t))
        return false;

      // the extension grouping types are marked with the `specialname` flag
      if (!t.IsSpecialName)
        return false;

      // the extension grouping types are marked with the [Extension] attribute
      if (!t.HasExtensionAttribute())
        return false;

      // extension grouping types are declared as nested classes within the enclosing static classes
      if (!t.IsNested)
        return false;

      // the enclosing static classes of the extension grouping types are marked with the [Extension] attribute
      if (t.DeclaringType is not { } declaringType)
        return false;
      if (!declaringType.HasExtensionAttribute())
        return false;

      return true;
    }

    /// <summary>
    /// Enumerates the extension marker types and their extension parameters from the extension grouping type.
    /// </summary>
    /// <returns>
    /// Returns the <see cref="IEnumerable{T}"/> of the extension marker types and their extension parameters.
    /// Returns empty if <paramref name="t"/> is not a <see cref="Type"/> of an extension grouping type.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="t"/> is <see langword="null"/>.</exception>
    /// <remarks>This method is implemented by using deferred execution.</remarks>
    public
    IEnumerable<(Type ExtensionMarkerType, ParameterInfo? ExtensionParameter)>
    EnumerateExtensionMarkerTypeAndParameterPairs()
    {
      if (t is null)
        throw new ArgumentNullException(nameof(t));

      return IsExtensionGroupingType(t) ? Core(t) : [];

      static
      IEnumerable<(Type ExtensionMarkerType, ParameterInfo? ExtensionParameter)>
      Core(Type extensionGroupingType)
      {
        foreach (var nestedType in extensionGroupingType.GetNestedTypes(BindingFlags.Public | BindingFlags.DeclaredOnly)) {
          if (IsExtensionMarkerTypeCore(nestedType, out var extensionParameter)) {
            yield return (
              nestedType,
              extensionParameter
#if !NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
              !
#endif
            );
          }
        }
      }
    }

    /// <summary>
    /// Gets a value that indicates whether the type is an extension marker type or not.
    /// </summary>
    /// <remarks>
    /// An extension marker type is a type that re-declares the type parameters of its containing grouping type.
    /// The extension marker types have names that start with <c>&lt;M&gt;$</c>.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="t"/> is <see langword="null"/>.</exception>
    /// <seealso href="https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-14.0/extensions#extension-marker-types">
    /// Extension members - Extension marker types
    /// </seealso>
    // this cannot be an extension property because since it must throw an ArgumentNullException
    public bool IsExtensionMarkerType()
    {
      if (t is null)
        throw new ArgumentNullException(nameof(t));

      return IsExtensionMarkerTypeCore(t, out _);
    }

    /// <summary>
    /// Gets the <see cref="ParameterInfo"/> of the extension parameter from the extension marker type.
    /// </summary>
    /// <returns>
    /// The <see cref="ParameterInfo"/> of the extension parameter of the extension marker type.
    /// If the extension marker method does not exist, i.e., in the case of an empty extension declaration, returns <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="t"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="t"/> is not a <see cref="Type"/> of a extension marker type.</exception>
    public ParameterInfo? GetExtensionParameter()
    {
      if (t is null)
        throw new ArgumentNullException(nameof(t));

      if (!IsExtensionMarkerTypeCore(t, out var extensionParameter))
        throw new InvalidOperationException($"The type '{t.FullName}' is not an extension marker type.");

      return extensionParameter;
    }

    /// <summary>
    /// Enumerates the pairs of the extension member and its implementation method from
    /// the <see cref="Type"/> of an extension enclosing class.
    /// </summary>
    /// <returns>
    /// Returns the <see cref="IEnumerable{T}"/> represents the pairs of the extension member and its implementation method.
    /// Returns empty if <paramref name="t"/> is not a <see cref="Type"/> of an extension enclosing class.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="t"/> is <see langword="null"/>.</exception>
    /// <remarks>This method is implemented by using deferred execution.</remarks>
#pragma warning disable CA1502 // TODO: simplify
    public
    IEnumerable<(MethodInfo ImplementationMethod, MethodInfo ExtensionMember)>
    EnumerateExtensionMemberAndImplementationPairs()
    {
      if (t is null)
        throw new ArgumentNullException(nameof(t));

      return Core(t);

      static
      IEnumerable<(MethodInfo ImplementationMethod, MethodInfo ExtensionMember)>
      Core(Type enclosingClass)
      {
        var possibleImplementationMethods = enclosingClass.GetMethods(BindingFlagsForImplementationMethods);

        foreach (var groupingType in enclosingClass.EnumerateExtensionGroupingTypes()) {
          // create map of extension member and its extension parameter
          var extensionMemberAndParameterPairs = ResolveExtensionMemberAndParameterPairs(
            extensionGroupingType: groupingType,
            extensionMemberAndMarkerTypeNamePairs: EnumerateExtensionMemberAndMarkerTypeNamePairs(groupingType)
          );

          // resolves extension members corresponding to implementation methods
          foreach (var possibleImplementationMethod in possibleImplementationMethods) {
            // resolve extension member with signatures equivalent to implementation method
            var (extensionMember, _) = extensionMemberAndParameterPairs.FirstOrDefault(
              pair =>
                pair is (var extensionMember, var extensionParameter) &&
                // - extension members and their corresponding implementation methods have the same name
                string.Equals(possibleImplementationMethod.Name, extensionMember.Name, StringComparison.Ordinal) &&
                // - extension members for the types (static extension members) does not
                //   refer the extension parameter in its method signature,
                // - extension members for the instances refer the extension parameter
                //   as the first parameter
                IsSameSignature(
                  implMethod: possibleImplementationMethod,
                  extMethod: extensionMember,
                  extParameter: extensionMember.IsStatic ? null : extensionParameter
                )
            );

            if (extensionMember is not null) {
              yield return (
                ImplementationMethod: possibleImplementationMethod,
                ExtensionMember: extensionMember
              );
            }
          }
        }
      }

      static
      IEnumerable<(MethodInfo Member, string MarkerTypeName)>
      EnumerateExtensionMemberAndMarkerTypeNamePairs(Type groupingType)
      {
        // since the implementation methods are associated with the corresponding
        // property accessor methods rather than the property itself,
        // PropertyInfo should be is excluded here
        foreach (var member in groupingType.GetMethods(BindingFlagsForExtensionMembers)) {
          if (ExtensionMembersUtils.TryGetExtensionMarkerTypeName(member, out var extensionGroupingTypeName)) {
            yield return (
              member,
              extensionGroupingTypeName
#if !NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
              !
#endif
            );
          }
        }
      }

      static
      IEnumerable<(MethodInfo Member, ParameterInfo Parameter)>
      ResolveExtensionMemberAndParameterPairs(
        Type extensionGroupingType,
        IEnumerable<(MethodInfo, string)> extensionMemberAndMarkerTypeNamePairs
      )
      {
        var resolvedExtensionParameters = new Dictionary<string, ParameterInfo>(
          capacity: 8, // TODO: best initial capacity
          StringComparer.Ordinal
        );

        foreach (var (extensionMember, markerTypeName) in extensionMemberAndMarkerTypeNamePairs) {
          if (!resolvedExtensionParameters.TryGetValue(markerTypeName, out var extensionParameter)) {
            extensionParameter = extensionGroupingType
              .GetNestedType(markerTypeName, BindingFlags.Public)
              ?.GetExtensionParameter();
          }

          if (extensionParameter is not null) {
            resolvedExtensionParameters[markerTypeName] = extensionParameter;

            yield return (extensionMember, extensionParameter);
          }
        }
      }

      static bool IsSameSignature(
        MethodInfo implMethod,
        MethodInfo extMethod,
        ParameterInfo? extParameter
      )
      {
        if (!IsSameType(implMethod.ReturnType, extMethod.ReturnType, implMethod.IsGenericMethodDefinition))
          return false; // return type unmatched

        var implMethodParams = implMethod.GetParameters();
        var extMethodParams = extMethod.GetParameters();

        if (extParameter is not null)
          // prepend extension parameter as a first parameter
          extMethodParams = [extParameter, .. extMethodParams];

        if (implMethodParams.Length != extMethodParams.Length)
          return false; // parameter count unmatched

        for (var i = 0; i < implMethodParams.Length; i++) {
          if (!string.Equals(implMethodParams[i].Name, extMethodParams[i].Name, StringComparison.Ordinal))
            return false; // parameter name unmatched
          if (!IsSameType(implMethodParams[i].ParameterType, extMethodParams[i].ParameterType, implMethod.IsGenericMethodDefinition))
            return false; // parameter type unmatched
        }

        return true;
      }

      static bool IsSameType(Type impl, Type ext, bool isGenericDefinition)
      {
#if SYSTEM_STRING_INDEXOF_CHAR_STRINGCOMPARISON
        const StringComparison SecondArgForIndexOfChar = StringComparison.Ordinal;
#else
        const int SecondArgForIndexOfChar = 0; // specify startIndex instead comparisonType
#endif

        if (impl.FullName is null)
          return ext.FullName is null;
        if (ext.FullName is null)
          return false;

        if (isGenericDefinition) {
          if (impl.IsGenericTypeDefinition && ext.IsGenericTypeDefinition) {
            // exclude generic type parameter specifications to treat
            // IEnumerable`1[T] (impl) and IEnumerable`1[$T0] (ext) as equivalent
            return MemoryExtensions.Equals(
              impl.FullName.AsSpan(0, impl.FullName.IndexOf('[', SecondArgForIndexOfChar)),
              ext.FullName.AsSpan(0, ext.FullName.IndexOf('[', SecondArgForIndexOfChar)),
              StringComparison.Ordinal
            );
          }

          if (impl.IsGenericTypeDefinition != ext.IsGenericTypeDefinition)
            return false;
        }

        return impl == ext;
      }
    }
#pragma warning restore CA1502
  } // extension(Type t)
#pragma warning restore CA1034, CS1734
}
