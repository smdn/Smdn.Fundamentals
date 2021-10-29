// Smdn.Fundamental.Shim.dll (Smdn.Fundamental.Shim-3.0.0 (net45))
//   Name: Smdn.Fundamental.Shim
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net45)
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release


namespace Smdn {
  public static class ArrayShim {
    public static T[] Empty<T>() {}
  }

  public static class StringShim {
    public static bool EndsWith(this string str, char @value) {}
    public static bool StartsWith(this string str, char @value) {}
  }
}

