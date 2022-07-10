// Smdn.Fundamental.String.Replacement.dll (Smdn.Fundamental.String.Replacement-3.0.2)
//   Name: Smdn.Fundamental.String.Replacement
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+9805a70fc9d7e01ace74c4db2ba31bb729f9e567
//   TargetFramework: .NETStandard,Version=v1.0
//   Configuration: Release
#nullable enable annotations

using Smdn;

namespace Smdn {
  public delegate string? ReplaceCharEvaluator(char ch, string str, int index);
  public delegate string? ReplaceStringEvaluator(string matched, string str, int index);

  public static class StringReplacementExtensions {
    public static string Remove(this string str, params string[] oldValues) {}
    public static string RemoveChars(this string str, params char[] oldChars) {}
    public static string Replace(this string str, char[] oldChars, ReplaceCharEvaluator evaluator) {}
    public static string Replace(this string str, string[] oldValues, ReplaceStringEvaluator evaluator) {}
  }
}
