// Smdn.Fundamental.Stream.TextReader.dll (Smdn.Fundamental.Stream.TextReader-3.0.1)
//   Name: Smdn.Fundamental.Stream.TextReader
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+c3fd43ebb35828d30dd34657e9cb09c2a9037f21
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
//   Referenced assemblies:
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Smdn.IO {
  public static class TextReaderReadAllLinesExtensions {
    public static IReadOnlyList<string> ReadAllLines(this TextReader reader) {}
    public static Task<IReadOnlyList<string>> ReadAllLinesAsync(this TextReader reader) {}
  }
}
