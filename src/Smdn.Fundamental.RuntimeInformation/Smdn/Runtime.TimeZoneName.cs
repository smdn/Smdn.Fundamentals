// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
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
        if (!IsRunningOnDotNet5OrOver)
          return false; // .NET Core on Windows uses NLS

        // .NET >= 5.0 on Windows uses ICU by default and can also be configured to use NLS
        // ref:
        //    https://learn.microsoft.com/ja-jp/dotnet/core/runtime-config/globalization#nls
        //    https://github.com/dotnet/runtime/blob/main/docs/design/features/framework-version-resolution.md
        if (GetRuntimeConfigurationSystemGlobalizationUseNls())
          return false; // .NET runtime is configured to use NLS
      }

      // in all other cases, .NET runtime uses ICU, or IANA time zone name
      return true;
    }
  }

  private static bool GetRuntimeConfigurationSystemGlobalizationUseNls()
  {
#if SYSTEM_ENVIRONMENT_GETENVIRONMENTVARIABLE
    var useNlsEnvVarString = Environment.GetEnvironmentVariable("DOTNET_SYSTEM_GLOBALIZATION_USENLS");

    if (useNlsEnvVarString is not null) {
      // 'true' or '1'
      if (bool.TryParse(useNlsEnvVarString, out var useNlsEnvVarBool) && useNlsEnvVarBool)
        return true;
      if (int.TryParse(useNlsEnvVarString, out var useNlsEnvVarInt) && useNlsEnvVarInt == 1)
        return true;
    }
#endif

#if SYSTEM_APPCONTEXT_GETDATA
    var useNlsConfig = AppContext.GetData("System.Globalization.UseNls");

    if (useNlsConfig is string useNlsConfigString && bool.TryParse(useNlsConfigString, out var useNlsConfigBool) && useNlsConfigBool)
      return true;
#endif

    return false;
  }
}
