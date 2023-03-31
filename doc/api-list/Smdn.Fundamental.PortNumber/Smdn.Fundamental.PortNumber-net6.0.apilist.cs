// Smdn.Fundamental.PortNumber.dll (Smdn.Fundamental.PortNumber-3.0.0)
//   Name: Smdn.Fundamental.PortNumber
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0+6f2d0f65a01b169e05b6e4adc7526ce8bbb67e78
//   TargetFramework: .NETCoreApp,Version=v6.0
//   Configuration: Release
//   Referenced assemblies:
//     System.Collections, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Net.NetworkInformation, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Net.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
#nullable enable annotations

using System;
using System.Collections.Generic;

namespace Smdn.Net {
  public static class PortNumberUtils {
    public const int MaxIanaDynamicPort = 65535;
    public const int MaxIanaSystemPort = 1023;
    public const int MaxIanaUserPort = 49151;
    public const int MinIanaDynamicPort = 49152;
    public const int MinIanaSystemPort = 0;
    public const int MinIanaUserPort = 1024;

    public static TService CreateServiceWithAvailablePort<TService>(Func<int, TService> createService, Predicate<Exception> isPortInUseException) {}
    public static TService CreateServiceWithAvailablePort<TService>(Func<int, TService> createService, Predicate<int>? exceptPort, Predicate<Exception> isPortInUseException) {}
    public static TService CreateServiceWithAvailablePort<TService>(Func<int, TService> createService, int exceptPort, Predicate<Exception> isPortInUseException) {}
    public static IEnumerable<int> EnumerateIanaDynamicPorts() {}
    public static IEnumerable<int> EnumerateIanaDynamicPorts(Predicate<int>? exceptPort) {}
    public static bool TryFindAvailablePort(Predicate<int>? exceptPort, out int port) {}
    public static bool TryFindAvailablePort(out int port) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.2.1.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
