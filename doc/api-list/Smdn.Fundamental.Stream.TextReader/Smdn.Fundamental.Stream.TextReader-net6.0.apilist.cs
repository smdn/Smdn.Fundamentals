// Smdn.Fundamental.Stream.TextReader.dll (Smdn.Fundamental.Stream.TextReader-3.0.1)
//   Name: Smdn.Fundamental.Stream.TextReader
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+c3fd43ebb35828d30dd34657e9cb09c2a9037f21
//   TargetFramework: .NETCoreApp,Version=v6.0
//   Configuration: Release
//   Referenced assemblies:
//     System.Collections, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Smdn.IO {
  public static class TextReaderReadAllLinesExtensions {
    public static IReadOnlyList<string> ReadAllLines(this TextReader reader) {}
    public static Task<IReadOnlyList<string>> ReadAllLinesAsync(this TextReader reader) {}
  }
}
