// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETFRAMEWORK || NETSTANDARD1_3_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_ENVIRONMENT_GETENVIRONMENTVARIABLE
#endif
#if NET47_OR_GREATER || NETSTANDARD1_6_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_APPCONTEXT_GETDATA
#endif

using System;
using System.Runtime.InteropServices;

namespace Smdn;

#pragma warning disable IDE0040
partial class Runtime {
#pragma warning restore IDE0040
  // ref: https://learn.microsoft.com/ja-jp/dotnet/core/extensions/globalization-icu
  public static bool SupportsIanaTimeZoneName {
    get {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
        if (IsRunningOnNetFx)
          return false; // .NET Framework uses NLS (National Language Support), or Windows time zone name
        if (Version < RuntimeVersionNET5)
          return false; // .NET Core on Windows uses NLS

        // .NET >= 5.0 on Windows uses ICU by default and can also be configured to use NLS
        // ref:
        //    https://learn.microsoft.com/ja-jp/dotnet/core/runtime-config/globalization#nls
        //    https://github.com/dotnet/runtime/blob/main/docs/design/features/framework-version-resolution.md
        var useNlsValue = GetRuntimeConfigurationSystemGlobalizationUseNls();

        if (useNlsValue is string useNlsString && bool.TryParse(useNlsString, out var useNls) && useNls)
          return false; // .NET runtime is configured to use NLS
      }

      // in all other cases, .NET runtime uses ICU, or IANA time zone name
      return true;
    }
  }

  private static object? GetRuntimeConfigurationSystemGlobalizationUseNls()
  {
#if SYSTEM_ENVIRONMENT_GETENVIRONMENTVARIABLE
    var envvar = Environment.GetEnvironmentVariable("DOTNET_SYSTEM_GLOBALIZATION_USENLS");

    if (envvar is not null)
      return envvar;
#endif

#if SYSTEM_APPCONTEXT_GETDATA
    var configProperty = AppContext.GetData("System.Globalization.UseNls");

    if (configProperty is not null)
      return configProperty;
#endif

    return null;
  }
}
