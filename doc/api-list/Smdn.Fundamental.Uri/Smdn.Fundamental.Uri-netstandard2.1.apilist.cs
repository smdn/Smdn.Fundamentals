// Smdn.Fundamental.Uri.dll (Smdn.Fundamental.Uri-3.0.1 (netstandard2.1))
//   Name: Smdn.Fundamental.Uri
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1 (netstandard2.1)
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

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

