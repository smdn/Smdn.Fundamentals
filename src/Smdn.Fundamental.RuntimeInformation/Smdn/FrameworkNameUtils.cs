// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET5_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NETSTANDARD1_3_OR_GREATER
#define SYSTEM_RUNTIME_VERSIONING_FRAMEWORKNAME
#endif

#if SYSTEM_RUNTIME_VERSIONING_FRAMEWORKNAME
using System;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.Runtime.Versioning;

namespace Smdn;

public static class FrameworkNameUtils {
  private static readonly Version versionNET5 = new(5, 0);

  public static bool TryGetMoniker(
    string? frameworkName,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out string? frameworkMoniker
  )
  {
    frameworkMoniker = default;

    if (frameworkName is null)
      return false;

    FrameworkName name;

    try {
      name = new(frameworkName);
    }
    catch (ArgumentException) {
      return false;
    }

    return TryGetMoniker(name, out frameworkMoniker);
  }

  public static bool TryGetMoniker(
    FrameworkName? frameworkName,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out string? frameworkMoniker
  )
  {
    frameworkMoniker = default;

    if (frameworkName is null)
      return false;

    // TODO: frameworkName.Profile, osSpecifier
    switch (frameworkName.Identifier) {
      case ".NETCoreApp" when versionNET5 <= frameworkName.Version:
        frameworkMoniker = $"net{frameworkName.Version.Major}.{frameworkName.Version.Minor}";
        return true;

      case ".NETCoreApp":
        frameworkMoniker = $"netcoreapp{frameworkName.Version.Major}.{frameworkName.Version.Minor}";
        return true;

      case ".NETStandard":
        frameworkMoniker = $"netstandard{frameworkName.Version.Major}.{frameworkName.Version.Minor}";
        return true;

      case ".NETFramework" when frameworkName.Version.Build == -1:
        frameworkMoniker = $"net{frameworkName.Version.Major}{frameworkName.Version.Minor}";
        return true;

      case ".NETFramework":
        frameworkMoniker = $"net{frameworkName.Version.Major}{frameworkName.Version.Minor}{frameworkName.Version.Build}";
        return true;

      default:
        return false;
    }
  }
}
#endif // SYSTEM_RUNTIME_VERSIONING_FRAMEWORKNAME
