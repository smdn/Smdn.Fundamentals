// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.InteropServices;

namespace Smdn;

#pragma warning disable IDE0040
partial class Runtime {
#pragma warning restore IDE0040
  private static readonly Version runtimeVersionNET5 = new(5, 0);

  // ref: https://learn.microsoft.com/ja-jp/dotnet/core/extensions/globalization-icu
  public static bool SupportsIanaTimeZoneName {
    get {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
        if (IsRunningOnNetFx)
          return false; // .NET Framework uses NLS (National Language Support), or Windows time zone name
        if (Version < runtimeVersionNET5)
          return false; // .NET Core on Windows uses NLS

        // .NET >= 5.0 on Windows uses ICU by default and can also be configured to use NLS
        // ref:
        //    https://learn.microsoft.com/ja-jp/dotnet/core/runtime-config/globalization#nls
        //    https://github.com/dotnet/runtime/blob/main/docs/design/features/framework-version-resolution.md
        var useNlsValue =
          Environment.GetEnvironmentVariable("DOTNET_SYSTEM_GLOBALIZATION_USENLS") ??
          AppContext.GetData("System.Globalization.UseNls");

        if (useNlsValue is string useNlsString && bool.TryParse(useNlsString, out var useNls) && useNls)
          return false; // .NET runtime is configured to use NLS
      }

      // in all other cases, .NET runtime uses ICU, or IANA time zone name
      return true;
    }
  }
}
