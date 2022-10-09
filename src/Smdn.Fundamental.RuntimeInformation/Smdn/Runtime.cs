// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
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
      // return; // mono?
    }

    if (Type.GetType("Mono.Runtime") != null) {
      /*
       * http://mono-project.com/FAQ:_Technical
       */
      RuntimeEnvironment = RuntimeEnvironment.Mono;
      Name = "Mono";
    }
#if false
    else if (Type.GetType("FXAssembly") != null) {
      RuntimeEnvironment = RuntimeEnvironment.NetFx;
      Name = ".NET Framework";
    }
    // XXX
    else if (typeof(Runtime).GetTypeInfo().Assembly.GetReferencedAssemblies().Any(n => n.Name.Equals("System.Runtime", StringComparison.Ordinal))) {
      RuntimeEnvironment = RuntimeEnvironment.NetCore;
      Name = ".NET Core";
    }
#endif
    else {
      RuntimeEnvironment = RuntimeEnvironment.Unknown;
      Name = ".NET Framework compatible";
    }
  }

  public static bool IsRunningOnNetFx => RuntimeEnvironment == RuntimeEnvironment.NetFx;
  public static bool IsRunningOnNetCore => RuntimeEnvironment == RuntimeEnvironment.NetCore;
  public static bool IsRunningOnMono => RuntimeEnvironment == RuntimeEnvironment.Mono;

  [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
  public static bool IsRunningOnWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

  [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
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
