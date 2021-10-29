// Smdn.Fundamental.Shim.dll (Smdn.Fundamental.Shim-3.0.0 (netstandard2.0))
//   Name: Smdn.Fundamental.Shim
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard2.0)
//   TargetFramework: .NETStandard,Version=v2.0
//   Configuration: Release


namespace Smdn {
  public static class ArrayShim {
  }

  public static class StringShim {
    public static bool EndsWith(this string str, char @value) {}
    public static bool StartsWith(this string str, char @value) {}
  }
}

