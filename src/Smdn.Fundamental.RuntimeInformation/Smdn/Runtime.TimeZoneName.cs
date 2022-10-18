// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_APPCONTEXT
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
        var useNlsValue =
          Environment.GetEnvironmentVariable("DOTNET_SYSTEM_GLOBALIZATION_USENLS")
#if SYSTEM_APPCONTEXT
          ?? AppContext.GetData("System.Globalization.UseNls");
#endif

        if (useNlsValue is string useNlsString && bool.TryParse(useNlsString, out var useNls) && useNls)
          return false; // .NET runtime is configured to use NLS
      }

      // in all other cases, .NET runtime uses ICU, or IANA time zone name
      return true;
    }
  }
}
