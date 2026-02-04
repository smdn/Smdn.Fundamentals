// SPDX-FileCopyrightText: 2026 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TypeExtensionsExtensionMembersTestTypes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
public class ExpectedExtensionParameterAttribute(Type type, string? name) : Attribute {
  public Type Type { get; } = type;
  public string? Name { get; } = name;
}

public static class EnclosingClassForNonGenericExtensionMembers {
  extension(Uri? uri) {
    [ExpectedExtensionParameter(typeof(Uri), nameof(uri))]
    public Uri ThrowIfNull() => uri ?? throw new ArgumentNullException(nameof(uri));

    [ExpectedExtensionParameter(typeof(Uri), nameof(uri))]
    public bool IsNull => uri is null;
  }

  extension(Uri) {
    [ExpectedExtensionParameter(typeof(Uri), null)]
    public static Uri operator -(Uri @this, Uri other) => @this;
  }
}

public static class EnclosingClassForOpenGenericExtensionMembers {
  extension<T>(IEnumerable<T> items) where T : struct {
    [ExpectedExtensionParameter(typeof(IEnumerable<>), nameof(items))]
    public IEnumerable<T> ThrowIfNull() => items ?? throw new ArgumentNullException(nameof(items));
  }

  extension<T>(IEnumerable<T>) where T : class, IDisposable {
    [ExpectedExtensionParameter(typeof(IEnumerable<>), null)]
    public static IEnumerable<T> operator -(IEnumerable<T> @this) => @this;
  }
}

public static class EnclosingClassForCloseGenericExtensionMembers {
  extension(IEnumerable<Uri> items) {
    [ExpectedExtensionParameter(typeof(IEnumerable<Uri>), nameof(items))]
    public IEnumerable<Uri> ThrowIfNull() => items ?? throw new ArgumentNullException(nameof(items));
  }

  extension(IEnumerable<Uri>) {
    [ExpectedExtensionParameter(typeof(IEnumerable<Uri>), null)]
    public static IEnumerable<Uri> operator -(IEnumerable<Uri> @this) => @this;
  }
}

public static class EnclosingClassForEmptyExtensionMembersWithParameterName {
  extension(Guid g) {
    // no extension members
  }
}

public static class EnclosingClassForEmptyExtensionMembersWithoutParameterName {
  extension(Guid) {
    // no extension members
  }
}

// [Extension]
public static class FakeEnclosingClassForNonGenericExtensionMembers {
  // [Extension]
  public sealed class FakeExtensionGroupingTypeWithParameterName {
    public static class FakeExtensionMarkerTypeWithParameterName {
      [CompilerGenerated]
      public static void Extension(Uri uri) => throw new NotImplementedException();
    }

    public static class FakeExtensionMarkerTypeWithoutParameterName {
      [CompilerGenerated]
      public static void Extension(Uri _) => throw new NotImplementedException();
    }

    // [ExtensionMarker(nameof(FakeExtensionGroupingTypeWithParameterName))]
    public Uri ThrowIfNull(Uri uri) => uri ?? throw new ArgumentNullException(nameof(uri));

    // [ExtensionMarker(nameof(FakeExtensionGroupingTypeWithParameterName))]
    public bool IsNull {
      // [ExtensionMarker(nameof(FakeExtensionGroupingTypeWithParameterName))]
      get => throw new NotImplementedException();
    }

    // [ExtensionMarker(nameof(FakeExtensionMarkerTypeWithoutParameterName))]
    // public static Uri operator -(Uri @this, Uri other) => @this;
  }

  // [Extension]
  public static Uri ThrowIfNull(this Uri uri) => throw new NotImplementedException();

  public static bool get_IsNull(Uri uri) => throw new NotImplementedException();

  public static Uri op_Subtraction(Uri @this, Uri other) => throw new NotImplementedException();
}

public static class ExtensionAndImplementation_NonGenericParameter {
  extension(Uri x) {
    public bool M() => throw new NotImplementedException();
    public static bool MStatic() => throw new NotImplementedException();

    public bool P {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }
    public static bool PStatic {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public static Uri operator +(Uri @this, Uri @other) => throw new NotImplementedException(); // op_Addition
    public static Uri operator +(Uri @this) => throw new NotImplementedException(); // op_UnaryPlus
#if SYSTEM_RUNTIME_COMPILERSERVICES_COMPILERFEATUREREQUIREDATTRIBUTE
    public void operator +=(Uri other) => throw new NotImplementedException(); // op_AdditionAssignment
#endif
  }

  public static bool MNonExtension(Uri x) => throw new NotImplementedException();
  public static bool MNonExtensionStatic() => throw new NotImplementedException();
  public static bool get_PNonExtension(Uri x) => throw new NotImplementedException();
  public static void set_PNonExtension(Uri x, bool value) => throw new NotImplementedException();
  public static bool get_PNonExtensionStatic() => throw new NotImplementedException();
  public static void set_PNonExtensionStatic(bool value) => throw new NotImplementedException();
}

public static class ExtensionAndImplementation_ConstructedGenericParameter {
  extension(IEnumerable<Guid> x) {
    public bool M() => throw new NotImplementedException();
    public static bool MStatic() => throw new NotImplementedException();

    public bool P {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }
    public static bool PStatic {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public static IEnumerable<Guid> operator +(IEnumerable<Guid> @this, IEnumerable<Guid> @other) => throw new NotImplementedException(); // op_Addition
    public static IEnumerable<Guid> operator +(IEnumerable<Guid> @this) => throw new NotImplementedException(); // op_UnaryPlus
#if SYSTEM_RUNTIME_COMPILERSERVICES_COMPILERFEATUREREQUIREDATTRIBUTE
    public void operator +=(IEnumerable<Guid> other) => throw new NotImplementedException(); // op_AdditionAssignment
#endif
  }

  public static bool MNonExtension(IEnumerable<Guid> x) => throw new NotImplementedException();
  public static bool MNonExtensionStatic() => throw new NotImplementedException();
  public static bool get_PNonExtension(IEnumerable<Guid> x) => throw new NotImplementedException();
  public static void set_PNonExtension(IEnumerable<Guid> x, bool value) => throw new NotImplementedException();
  public static bool get_PNonExtensionStatic() => throw new NotImplementedException();
  public static void set_PNonExtensionStatic(bool value) => throw new NotImplementedException();
}

public static class ExtensionAndImplementation_GenericTypeDefinitionParameter {
  extension<T>(IEnumerable<T> x) {
    public bool M() => throw new NotImplementedException();
    public static bool MStatic() => throw new NotImplementedException();

    public bool P {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }
    public static bool PStatic {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public static IEnumerable<T> operator +(IEnumerable<T> @this, IEnumerable<T> @other) => throw new NotImplementedException(); // op_Addition
    public static IEnumerable<T> operator +(IEnumerable<T> @this) => throw new NotImplementedException(); // op_UnaryPlus
#if SYSTEM_RUNTIME_COMPILERSERVICES_COMPILERFEATUREREQUIREDATTRIBUTE
    public void operator +=(IEnumerable<T> other) => throw new NotImplementedException(); // op_AdditionAssignment
#endif
  }

  public static bool MNonExtension<T>(IEnumerable<T> x) => throw new NotImplementedException();
  public static bool MNonExtensionStatic<T>() => throw new NotImplementedException();
  public static bool get_PNonExtension<T>(IEnumerable<T> x) => throw new NotImplementedException();
  public static void set_PNonExtension<T>(IEnumerable<T> x, bool value) => throw new NotImplementedException();
  public static bool get_PNonExtensionStatic<T>() => throw new NotImplementedException();
  public static void set_PNonExtensionStatic<T>(bool value) => throw new NotImplementedException();
}

public static class ExtensionAndImplementation_MultipleExtensionGroupingTypes {
  extension(Uri x) {
    public bool MNonGeneric() => throw new NotImplementedException();
    public static bool MNonGenericStatic() => throw new NotImplementedException();

    public bool PNonGeneric {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }
    public static bool PNonGenericStatic {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public static Uri operator +(Uri @this, Uri @other) => throw new NotImplementedException(); // op_Addition
    public static Uri operator +(Uri @this) => throw new NotImplementedException(); // op_UnaryPlus
#if SYSTEM_RUNTIME_COMPILERSERVICES_COMPILERFEATUREREQUIREDATTRIBUTE
    public void operator +=(Uri other) => throw new NotImplementedException(); // op_AdditionAssignment
#endif
  }

  extension(IEnumerable<Guid> x) {
    public bool MConstructedGeneric() => throw new NotImplementedException();
    public static bool MConstructedGenericStatic() => throw new NotImplementedException();

    public bool PConstructedGeneric {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }
    public static bool PConstructedGenericStatic {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public static IEnumerable<Guid> operator +(IEnumerable<Guid> @this, IEnumerable<Guid> @other) => throw new NotImplementedException(); // op_Addition
    public static IEnumerable<Guid> operator +(IEnumerable<Guid> @this) => throw new NotImplementedException(); // op_UnaryPlus
#if SYSTEM_RUNTIME_COMPILERSERVICES_COMPILERFEATUREREQUIREDATTRIBUTE
    public void operator +=(IEnumerable<Guid> other) => throw new NotImplementedException(); // op_AdditionAssignment
#endif
  }

  extension<T>(IEnumerable<T> x) {
    public bool MGenericDef() => throw new NotImplementedException();
    public static bool MGenericDefStatic() => throw new NotImplementedException();

    public bool PGenericDef {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }
    public static bool PGenericDefStatic {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public static IEnumerable<T> operator +(IEnumerable<T> @this, IEnumerable<T> @other) => throw new NotImplementedException(); // op_Addition
    public static IEnumerable<T> operator +(IEnumerable<T> @this) => throw new NotImplementedException(); // op_UnaryPlus
#if SYSTEM_RUNTIME_COMPILERSERVICES_COMPILERFEATUREREQUIREDATTRIBUTE
    public void operator +=(IEnumerable<T> other) => throw new NotImplementedException(); // op_AdditionAssignment
#endif
  }
}
