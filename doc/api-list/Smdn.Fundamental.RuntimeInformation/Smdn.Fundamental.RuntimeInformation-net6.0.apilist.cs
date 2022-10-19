// Smdn.Fundamental.RuntimeInformation.dll (Smdn.Fundamental.RuntimeInformation-3.0.1)
//   Name: Smdn.Fundamental.RuntimeInformation
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+90ccc6113020a51c2d5326ab073866967dd8ec17
//   TargetFramework: .NETCoreApp,Version=v6.0
//   Configuration: Release
#nullable enable annotations

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Smdn;

namespace Smdn {
  public enum RuntimeEnvironment : int {
    Mono = 2,
    NetCore = 3,
    NetFx = 1,
    Unknown = 0,
  }

  public static class FrameworkNameUtils {
    public static bool TryGetMoniker(FrameworkName? frameworkName, [NotNullWhen(true)] out string? frameworkMoniker) {}
    public static bool TryGetMoniker(string? frameworkName, [NotNullWhen(true)] out string? frameworkMoniker) {}
  }

  public static class Runtime {
    public static bool IsRunningOnDotNet5OrOver { get; }
    public static bool IsRunningOnMono { get; }
    public static bool IsRunningOnNetCore { get; }
    public static bool IsRunningOnNetFx { get; }
    [Obsolete("use Smdn.Platform.IsRunningOnUnix")]
    public static bool IsRunningOnUnix { get; }
    [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
    public static bool IsRunningOnWindows { get; }
    public static string Name { get; }
    public static RuntimeEnvironment RuntimeEnvironment { get; }
    public static bool SupportsIanaTimeZoneName { get; }
    public static Version? Version { get; }
    public static string VersionString { get; }
  }
}
