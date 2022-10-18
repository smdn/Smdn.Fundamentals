// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_ASSEMBLY_GETREFERENCEDASSEMBLIES
using System.Linq;
#endif
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Smdn;

public static partial class Runtime {
  internal static readonly Version RuntimeVersionNET5 = new(5, 0);

  public static RuntimeEnvironment RuntimeEnvironment { get; }
  public static string Name { get; }

  private static readonly Lazy<Version?> lazyVersion;
  public static Version? Version => lazyVersion.Value;

  static Runtime()
  {
    lazyVersion = new(GetRuntimeVersion, LazyThreadSafetyMode.PublicationOnly);

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
        ?.Any(IsAssemblyNameSystemRuntime)
        ?? false
    ) {
      clr = RuntimeEnvironment.NetCore;
      name = DetermineNetCoreRuntimeName();
    }
    else if (
      typeof(Runtime)
        .GetTypeInfo()
        .Assembly
        .GetReferencedAssemblies()
        .Any(IsAssemblyNameSystemRuntime)
    ) {
      clr = RuntimeEnvironment.NetCore;
      name = DetermineNetCoreRuntimeName();
    }
#endif

    RuntimeEnvironment = clr;
    Name = name ?? ".NET compatible runtime"; // fallback

#if SYSTEM_ASSEMBLY_GETREFERENCEDASSEMBLIES
    static bool IsAssemblyNameSystemRuntime(AssemblyName n)
      => "System.Runtime".Equals(n.Name, StringComparison.Ordinal);

    static string DetermineNetCoreRuntimeName()
#if SYSTEM_ENVIRONMENT_VERSION
      => RuntimeVersionNET5 <= Environment.Version ? ".NET" : ".NET Core";
#else
      => ".NET Core";
#endif
#endif
  }

  public static bool IsRunningOnNetFx => RuntimeEnvironment == RuntimeEnvironment.NetFx;
  public static bool IsRunningOnNetCore => RuntimeEnvironment == RuntimeEnvironment.NetCore;
  public static bool IsRunningOnMono => RuntimeEnvironment == RuntimeEnvironment.Mono;

  public static string VersionString => RuntimeInformation.FrameworkDescription;

  private static Version? GetRuntimeVersion()
  {
    if (RuntimeEnvironment == RuntimeEnvironment.Mono) {
      // attempt to get version from return value of Mono.Runtime.GetDisplayName()
      var displayName = (string?)Type
        .GetType("Mono.Runtime")
        ?.GetTypeInfo()
        ?.GetDeclaredMethod("GetDisplayName")
        ?.Invoke(null, null);

      if (displayName is not null) {
        foreach (var s in displayName.Split(' ')) {
          if (Version.TryParse(s, out var v))
            return v;
        }
      }
    }

    // attempt to get version from the string of RuntimeInformation.FrameworkDescription
    foreach (var s in RuntimeInformation.FrameworkDescription.Split(' ')) {
      if (Version.TryParse(s, out var v))
        return v;
    }

#if SYSTEM_ENVIRONMENT_VERSION
    return Environment.Version;
#else
    return null;
#endif
  }
}
