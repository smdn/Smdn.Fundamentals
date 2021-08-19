// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_ENVIRONMENT_VERSION
#endif

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Smdn {
  public static class Runtime {
    private static readonly RuntimeEnvironment runtimeEnvironment;
    private static readonly string name;

    static Runtime()
    {
      if (RuntimeInformation.FrameworkDescription.Contains(".NET Framework")) {
        runtimeEnvironment = RuntimeEnvironment.NetFx;
        name = ".NET Framework";
        return;
      }
      else if (RuntimeInformation.FrameworkDescription.Contains(".NET Core")) {
        runtimeEnvironment = RuntimeEnvironment.NetCore;
        name = ".NET Core";
        return;
      }
      else if (RuntimeInformation.FrameworkDescription.Contains("Mono")) {
        runtimeEnvironment = RuntimeEnvironment.Mono;
        name = "Mono";
        //return; // mono?
      }

      if (Type.GetType("Mono.Runtime") != null) {
        /*
         * http://mono-project.com/FAQ:_Technical
         */
        runtimeEnvironment = RuntimeEnvironment.Mono;
        name = "Mono";
      }
#if false
      else if (Type.GetType("FXAssembly") != null) {
        runtimeEnvironment = RuntimeEnvironment.NetFx;
        name = ".NET Framework";
      }
      // XXX
      else if (typeof(Runtime).GetTypeInfo().Assembly.GetReferencedAssemblies().Any(n => n.Name.Equals("System.Runtime", StringComparison.Ordinal))) {
        runtimeEnvironment = RuntimeEnvironment.NetCore;
        name = ".NET Core";
      }
#endif
      else {
        runtimeEnvironment = RuntimeEnvironment.Unknown;
        name = ".NET Framework compatible";
      }
    }

    public static RuntimeEnvironment RuntimeEnvironment {
      get { return runtimeEnvironment; }
    }

    public static string Name {
      get { return name; }
    }

    public static bool IsRunningOnNetFx {
      get { return runtimeEnvironment == RuntimeEnvironment.NetFx; }
    }

    public static bool IsRunningOnNetCore {
      get { return runtimeEnvironment == RuntimeEnvironment.NetCore; }
    }

    public static bool IsRunningOnMono {
      get { return runtimeEnvironment == RuntimeEnvironment.Mono; }
    }

    [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
    public static bool IsRunningOnWindows => Platform.IsRunningOnWindows;

    [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
    public static bool IsRunningOnUnix => Platform.IsRunningOnUnix;

    public static string VersionString => RuntimeInformation.FrameworkDescription;

    public static Version Version {
      get {
        switch (runtimeEnvironment) {
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
}
