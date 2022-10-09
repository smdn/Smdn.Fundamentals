// Smdn.Fundamental.RuntimeInformation.dll (Smdn.Fundamental.RuntimeInformation-3.0.0)
//   Name: Smdn.Fundamental.RuntimeInformation
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0+e3a3698ec4cf72ab95c7f0cbe69fc08aef244a88
//   TargetFramework: .NETStandard,Version=v2.0
//   Configuration: Release
#nullable enable annotations

using System;
using Smdn;

namespace Smdn {
  public enum RuntimeEnvironment : int {
    Mono = 2,
    NetCore = 3,
    NetFx = 1,
    Unknown = 0,
  }

  public static class Runtime {
    public static bool IsRunningOnMono { get; }
    public static bool IsRunningOnNetCore { get; }
    public static bool IsRunningOnNetFx { get; }
    [Obsolete("use Smdn.Platform.IsRunningOnUnix")]
    public static bool IsRunningOnUnix { get; }
    [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
    public static bool IsRunningOnWindows { get; }
    public static string Name { get; }
    public static RuntimeEnvironment RuntimeEnvironment { get; }
    public static Version? Version { get; }
    public static string VersionString { get; }
  }
}
