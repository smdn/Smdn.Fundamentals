// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#if SYSTEM_ASSEMBLY_GETREFERENCEDASSEMBLIES
using System.Linq;
#endif
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Smdn;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
#pragma warning disable CA1724
public static partial class Runtime {
#pragma warning restore CA1724
  internal static readonly Version RuntimeVersionNET5 = new(5, 0);

  // use KeyValuePair`2 instead since ValueTuple`2 cannot be used
  private static readonly KeyValuePair<RuntimeEnvironment, string> RuntimeEnvironmentAndName = GetRuntimeEnvironmentAndName();

  public static RuntimeEnvironment RuntimeEnvironment => RuntimeEnvironmentAndName.Key;
  public static string Name => RuntimeEnvironmentAndName.Value;

  private static readonly Lazy<Version?> LazyVersion = new(GetRuntimeVersion, LazyThreadSafetyMode.PublicationOnly);
  public static Version? Version => LazyVersion.Value;

  private static KeyValuePair<RuntimeEnvironment, string> GetRuntimeEnvironmentAndName()
  {
    static bool FrameworkDescriptionContains(string value)
      => RuntimeInformation.FrameworkDescription
#if SYSTEM_STRING_CONTAINS_STRING_STRINGCOMPARISON
        .Contains(value, StringComparison.Ordinal);
#else
        .Contains(value);
#endif

    if (FrameworkDescriptionContains(".NET Framework"))
      return new(RuntimeEnvironment.NetFx, ".NET Framework");
    if (FrameworkDescriptionContains(".NET Core"))
      return new(RuntimeEnvironment.NetCore, ".NET Core");
    if (FrameworkDescriptionContains("Mono"))
      return new(RuntimeEnvironment.Mono, "Mono");

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

    return new(
      clr,
      name ?? ".NET compatible runtime" // fallback
    );

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

  public static bool IsRunningOnDotNet5OrOver
    => RuntimeEnvironment == RuntimeEnvironment.NetCore && RuntimeVersionNET5 <= Version;

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
