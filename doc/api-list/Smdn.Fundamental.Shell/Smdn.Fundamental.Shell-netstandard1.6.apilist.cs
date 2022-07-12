// Smdn.Fundamental.Shell.dll (Smdn.Fundamental.Shell-3.0.1)
//   Name: Smdn.Fundamental.Shell
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+9d1fd8df380c745be94459a5479a55ac12ab3ada
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release

using System;
using System.Collections.Generic;
using Smdn.OperatingSystem;

namespace Smdn.OperatingSystem {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class EnvironmentVariable {
    public static string CombineEnvironmentVariables(IDictionary<string, string> variables) {}
    public static Dictionary<string, string> ParseEnvironmentVariables(string variables) {}
    public static Dictionary<string, string> ParseEnvironmentVariables(string variables, bool throwIfInvalid) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class ShellString :
    IEquatable<ShellString>,
    IEquatable<string>
  {
    public ShellString(string raw) {}

    public string Expanded { get; }
    public bool IsEmpty { get; }
    public string Raw { get; set; }

    public ShellString Clone() {}
    public bool Equals(ShellString other) {}
    public bool Equals(string other) {}
    public override bool Equals(object obj) {}
    public static string Expand(ShellString str) {}
    public override int GetHashCode() {}
    public static bool IsNullOrEmpty(ShellString str) {}
    public override string ToString() {}
    public static bool operator == (ShellString x, ShellString y) {}
    public static explicit operator string(ShellString str) {}
    public static implicit operator ShellString(string str) {}
    public static bool operator != (ShellString x, ShellString y) {}
  }
}
