// Smdn.Fundamental.Uri.dll (Smdn.Fundamental.Uri-3.0.1)
//   Name: Smdn.Fundamental.Uri
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1 (netstandard1.6)
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Collection, Version=3.0.0.0, Culture=neutral
//     Smdn.Fundamental.Shim, Version=3.0.0.0, Culture=neutral
//     System.Collections, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a

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
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.1.7.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
