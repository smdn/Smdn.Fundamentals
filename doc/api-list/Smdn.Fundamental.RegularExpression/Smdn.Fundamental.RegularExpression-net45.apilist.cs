// Smdn.Fundamental.RegularExpression.dll (Smdn.Fundamental.RegularExpression-3.0.0)
//   Name: Smdn.Fundamental.RegularExpression
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net45)
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
//   Referenced assemblies:
//     System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089

using System.Text.RegularExpressions;

namespace Smdn.Text.RegularExpressions {
  public static class RegexExtensions {
    public static bool IsMatch(this Regex regex, string input, int startIndex, out Match match) {}
    public static bool IsMatch(this Regex regex, string input, out Match match) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.6.0.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.4.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
