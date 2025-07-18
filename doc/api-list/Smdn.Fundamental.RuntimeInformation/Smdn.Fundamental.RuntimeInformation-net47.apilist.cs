// Smdn.Fundamental.RuntimeInformation.dll (Smdn.Fundamental.RuntimeInformation-3.0.3)
//   Name: Smdn.Fundamental.RuntimeInformation
//   AssemblyVersion: 3.0.3.0
//   InformationalVersion: 3.0.3+50cd3a5ddb6026e07a1bf790427b237a96c07bb8
//   TargetFramework: .NETFramework,Version=v4.7
//   Configuration: Release
//   Referenced assemblies:
//     System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
//     System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
//     System.Runtime.InteropServices.RuntimeInformation, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
#nullable enable annotations

using System;
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
    public static bool TryGetMoniker(FrameworkName? frameworkName, out string? frameworkMoniker) {}
    public static bool TryGetMoniker(string? frameworkName, out string? frameworkMoniker) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
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
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.6.0.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.4.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
