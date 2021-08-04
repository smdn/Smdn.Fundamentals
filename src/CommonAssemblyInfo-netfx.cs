// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Reflection;

[assembly: AssemblyCompany("smdn.jp (https://smdn.jp)")]
[assembly: AssemblyCopyright(AssemblyInfo.Copyright)]
[assembly: AssemblyTitle(AssemblyInfo.Name)]
[assembly: AssemblyProduct(AssemblyInfo.Name + "-" + AssemblyInfo.Version + " (" + AssemblyInfo.TargetFramework + ")")]
[assembly: AssemblyVersion(AssemblyInfo.Version)]
[assembly: AssemblyInformationalVersion(AssemblyInfo.Version + AssemblyInfo.VersionSuffix)]
[assembly: AssemblyConfiguration(AssemblyInfo.TargetFramework + AssemblyInfo.Configuration)]

internal static partial class AssemblyInfo {
  internal const string TargetFramework =
#if NET471
    ".NET 4.7.1"
#elif NET461
    ".NET 4.6.1"
#elif NET46
    ".NET 4.6"
#elif NET45
    ".NET 4.5"
#elif NETSTANDARD2_0
    ".NET Standard 2.0"
#elif NETSTANDARD1_6
    ".NET Standard 1.6"
#else
    ".NET"
#endif
    ;

  internal const string TargetFrameworkSuffix = 
#if NET471
    "net4.7.1"
#elif NET461
    "net4.6.1"
#elif NET46
    "net4.6"
#elif NET45
    "net4.5"
#elif NETSTANDARD2_0
    "netstandard2.0"
#elif NETSTANDARD1_6
    "netstandard1.6"
#else
    "net"
#endif
    ;

  internal const string Configuration =
#if DEBUG
    " Debug"
#else
    ""
#endif
    ;
}

