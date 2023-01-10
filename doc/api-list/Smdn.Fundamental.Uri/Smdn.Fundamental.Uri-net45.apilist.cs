// Smdn.Fundamental.Uri.dll (Smdn.Fundamental.Uri-3.0.1)
//   Name: Smdn.Fundamental.Uri
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1 (net45)
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Collection, Version=3.0.2.0, Culture=neutral
//     Smdn.Fundamental.Shim, Version=3.1.3.0, Culture=neutral
//     System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089

using System;
using System.Collections.Generic;

namespace Smdn {
  public static class UriExtensions {
    public static IReadOnlyDictionary<string, string> GetSplittedQueries(this Uri uri) {}
  }

  public static class UriUtils {
    public static string JoinQueryParameters(IEnumerable<KeyValuePair<string, string>> queryParameters) {}
    public static IReadOnlyDictionary<string, string> SplitQueryParameters(string queryParameters) {}
    public static IReadOnlyDictionary<string, string> SplitQueryParameters(string queryParameters, IEqualityComparer<string> comparer) {}
  }
}
