// Smdn.Fundamental.ParamArray.dll (Smdn.Fundamental.ParamArray-3.0.0)
//   Name: Smdn.Fundamental.ParamArray
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net45)
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Collection, Version=3.0.2.0, Culture=neutral
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089

namespace Smdn {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ParamArrayUtils {
    public static IEnumerable<TParam> ToEnumerable<TParam>(TParam first, params TParam[] subsequence) {}
    public static IEnumerable<TParam> ToEnumerableNonNullable<TParam>(string paramName, TParam first, params TParam[] subsequence) where TParam : class {}
    public static IReadOnlyList<TParam> ToList<TParam>(TParam first, params TParam[] subsequence) {}
    public static IReadOnlyList<TParam> ToListNonNullable<TParam>(string paramName, TParam first, params TParam[] subsequence) where TParam : class {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.1.7.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
