// Smdn.Fundamental.RegularExpression.dll (Smdn.Fundamental.RegularExpression-3.0.0)
//   Name: Smdn.Fundamental.RegularExpression
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard1.6)
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release
//   Referenced assemblies:
//     System.Runtime, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Text.RegularExpressions, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a

using System.Text.RegularExpressions;

namespace Smdn.Text.RegularExpressions {
  public static class RegexExtensions {
    public static bool IsMatch(this Regex regex, string input, int startIndex, out Match match) {}
    public static bool IsMatch(this Regex regex, string input, out Match match) {}
  }
}
