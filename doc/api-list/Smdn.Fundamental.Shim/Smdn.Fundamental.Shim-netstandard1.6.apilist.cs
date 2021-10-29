// Smdn.Fundamental.Shim.dll (Smdn.Fundamental.Shim-3.0.0 (netstandard1.6))
//   Name: Smdn.Fundamental.Shim
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard1.6)
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release

using System.IO;

namespace Smdn {
  public static class ArrayShim {
    public static TOutput[] ConvertAll<TInput, TOutput>(this TInput[] array, Func<TInput, TOutput> converter) {}
  }

  public static class StringShim {
    public static bool EndsWith(this string str, char @value) {}
    public static bool StartsWith(this string str, char @value) {}
  }
}

namespace Smdn.IO {
  public static class BinaryReaderExtensions {
    public static void Close(this BinaryReader reader) {}
  }

  public static class BinaryWriterExtensions {
    public static void Close(this BinaryWriter writer) {}
  }

  public static class TextReaderShim {
    public static void Close(this TextReader reader) {}
  }

  public static class TextWriterExtensions {
    public static void Close(this TextWriter writer) {}
  }
}

