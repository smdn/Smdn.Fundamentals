// Smdn.Fundamental.ParamArray.dll (Smdn.Fundamental.ParamArray-3.0.0)
//   Name: Smdn.Fundamental.ParamArray
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard1.6)
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release
#nullable enable annotations


namespace Smdn {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ParamArrayUtils {
    public static IEnumerable<TParam> ToEnumerable<TParam>(TParam first, params TParam[] subsequence) {}
    public static IEnumerable<TParam> ToEnumerableNonNullable<TParam>(string paramName, TParam first, params TParam[] subsequence) where TParam : class {}
    public static IReadOnlyList<TParam> ToList<TParam>(TParam first, params TParam[] subsequence) {}
    public static IReadOnlyList<TParam> ToListNonNullable<TParam>(string paramName, TParam first, params TParam[] subsequence) where TParam : class {}
  }
}
