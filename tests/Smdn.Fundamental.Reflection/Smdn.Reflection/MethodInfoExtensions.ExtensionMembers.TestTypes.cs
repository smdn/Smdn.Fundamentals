// SPDX-FileCopyrightText: 2026 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace MethodInfoExtensionsExtensionMembersTestTypes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ExtensionMethodAttribute : Attribute { }

internal static class EnclosingClassForUriExtensionMembers {
  extension(Uri uri) {
    [ExtensionMethod]
    public Uri ThrowIfNull() => uri ?? throw new ArgumentNullException(nameof(uri));

    public int ExtensionProperty {
      [ExtensionMethod] get => throw new NotImplementedException();
      [ExtensionMethod] set => throw new NotImplementedException();
    }
  }

  extension(Uri) {
    [ExtensionMethod]
    public static Uri operator -(Uri @this, Uri other) => @this;
  }

  public static void NonExtensionMethod() => throw new NotImplementedException();
}
