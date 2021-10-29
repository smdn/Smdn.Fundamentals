// Smdn.Fundamental.RegularExpression.dll (Smdn.Fundamental.RegularExpression-3.0.0 (netstandard2.1))
//   Name: Smdn.Fundamental.RegularExpression
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard2.1)
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

using System.Text.RegularExpressions;

namespace Smdn.Text.RegularExpressions {
  public static class RegexExtensions {
    public static bool IsMatch(this Regex regex, string input, int startIndex, out Match match) {}
    public static bool IsMatch(this Regex regex, string input, out Match match) {}
  }
}

