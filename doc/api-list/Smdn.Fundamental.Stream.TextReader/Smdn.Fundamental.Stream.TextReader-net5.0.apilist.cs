// Smdn.Fundamental.Stream.TextReader.dll (Smdn.Fundamental.Stream.TextReader-3.0.0)
//   Name: Smdn.Fundamental.Stream.TextReader
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net5.0)
//   TargetFramework: .NETCoreApp,Version=v5.0
//   Configuration: Release
#nullable enable annotations

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Smdn.IO {
  public static class TextReaderReadAllLinesExtensions {
    public static IReadOnlyList<string> ReadAllLines(this TextReader reader) {}
    public static Task<IReadOnlyList<string>> ReadAllLinesAsync(this TextReader reader) {}
  }
}
