// SPDX-FileCopyrightText: 2026 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace PropertyInfoExtensionsExtensionMembersTestTypes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ExtensionPropertyAttribute : Attribute { }

internal static class EnclosingClassForUriExtensionMembers {
  extension(Uri uri) {
    [ExtensionProperty]
    public bool IsNull => uri is null;
  }

  public static int NonExtensionProperty {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }
}
