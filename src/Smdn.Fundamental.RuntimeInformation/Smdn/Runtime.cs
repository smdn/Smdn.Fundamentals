// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_ASSEMBLY_GETREFERENCEDASSEMBLIES
using System.Linq;
#endif
using System.Reflection;
using System.Runtime.InteropServices;

namespace Smdn;

public static class Runtime {
  public static RuntimeEnvironment RuntimeEnvironment { get; }
  public static string Name { get; }

  static Runtime()
  {
    if (RuntimeInformation.FrameworkDescription.Contains(".NET Framework")) {
      RuntimeEnvironment = RuntimeEnvironment.NetFx;
      Name = ".NET Framework";
      return;
    }
    else if (RuntimeInformation.FrameworkDescription.Contains(".NET Core")) {
      RuntimeEnvironment = RuntimeEnvironment.NetCore;
      Name = ".NET Core";
      return;
    }
    else if (RuntimeInformation.FrameworkDescription.Contains("Mono")) {
      RuntimeEnvironment = RuntimeEnvironment.Mono;
      Name = "Mono";
      return;
    }

    var clr = RuntimeEnvironment.Unknown;
    string? name = null;

    if (Type.GetType("Mono.Runtime") is not null) {
      /*
       * http://mono-project.com/FAQ:_Technical
       */
      clr = RuntimeEnvironment.Mono;
      name = "Mono";
    }
    else if (Type.GetType("FXAssembly") is not null) {
      clr = RuntimeEnvironment.NetFx;
      name = ".NET Framework";
    }
#if SYSTEM_ASSEMBLY_GETREFERENCEDASSEMBLIES
    else if (
      Assembly
        .GetEntryAssembly()
        ?.GetReferencedAssemblies()
        ?.Any(static n => "System.Runtime".Equals(n.Name, StringComparison.Ordinal))
        ?? false
    ) {
      clr = RuntimeEnvironment.NetCore;
#if SYSTEM_ENVIRONMENT_VERSION
      name = 5 <= Environment.Version.Major ? ".NET" : ".NET Core";
#else
      name = ".NET Core";
#endif
    }
#endif

    RuntimeEnvironment = clr;
    Name = name ?? ".NET compatible runtime"; // fallback
  }

  public static bool IsRunningOnNetFx => RuntimeEnvironment == RuntimeEnvironment.NetFx;
  public static bool IsRunningOnNetCore => RuntimeEnvironment == RuntimeEnvironment.NetCore;
  public static bool IsRunningOnMono => RuntimeEnvironment == RuntimeEnvironment.Mono;

  [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
  public static bool IsRunningOnWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

  [Obsolete("use Smdn.Platform.IsRunningOnUnix")]
  public static bool IsRunningOnUnix
    => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

  public static string VersionString => RuntimeInformation.FrameworkDescription;

  public static Version Version {
    get {
      switch (RuntimeEnvironment) {
        case RuntimeEnvironment.NetFx:
        case RuntimeEnvironment.NetCore: {
          foreach (var s in RuntimeInformation.FrameworkDescription.Split(' ')) {
            if (Version.TryParse(s, out var v))
              return v;
          }

          break;
        }

        case RuntimeEnvironment.Mono: {
          var displayName = (string)Type.GetType("Mono.Runtime").GetTypeInfo().GetDeclaredMethod("GetDisplayName").Invoke(null, null);

          foreach (var s in displayName.Split(' ')) {
            if (Version.TryParse(s, out var v))
              return v;
          }

          break;
        }
      }

#if SYSTEM_ENVIRONMENT_VERSION
      return Environment.Version;
#else
      return null;
#endif
    }
  }
}
