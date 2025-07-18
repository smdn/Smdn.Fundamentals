// Smdn.Fundamental.Csv.dll (Smdn.Fundamental.Csv-3.1.1)
//   Name: Smdn.Fundamental.Csv
//   AssemblyVersion: 3.1.1.0
//   InformationalVersion: 3.1.1+50cd3a5ddb6026e07a1bf790427b237a96c07bb8
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release
//   Referenced assemblies:
//     netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Smdn.Formats {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class CsvRecord {
    public static IReadOnlyList<string> Split(ReadOnlySpan<char> csv) {}
    public static IReadOnlyList<string> Split(string csv) {}
    public static string ToJoined(IEnumerable<string> csv) {}
    public static string ToJoined(params string[] csv) {}
    public static string ToJoinedNullable(IEnumerable<string> csv) {}
    public static string ToJoinedNullable(params string[] csv) {}
    [Obsolete("use Split instead")]
    public static IEnumerable<string> ToSplitted(string csv) {}
    [Obsolete("use Split instead")]
    public static IEnumerable<string> ToSplittedNullable(string csv) {}
  }
}

namespace Smdn.Formats.Csv {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class CsvReader : StreamReader {
    public CsvReader(Stream stream) {}
    public CsvReader(Stream stream, Encoding encoding) {}
    public CsvReader(StreamReader reader) {}
    public CsvReader(StreamReader reader, Encoding encoding) {}
    public CsvReader(string path) {}
    public CsvReader(string path, Encoding encoding) {}

    public char Delimiter { get; set; }
    public char Quotator { get; set; }

    public IReadOnlyList<string> ReadRecord() {}
    public IEnumerable<IReadOnlyList<string>> ReadRecords() {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class CsvWriter : StreamWriter {
    public CsvWriter(Stream stream) {}
    public CsvWriter(Stream stream, Encoding encoding) {}
    public CsvWriter(StreamWriter writer) {}
    public CsvWriter(StreamWriter writer, Encoding encoding) {}
    public CsvWriter(string path) {}
    public CsvWriter(string path, Encoding encoding) {}

    public char Delimiter { get; set; }
    public bool EscapeAlways { get; set; }
    public char Quotator { get; set; }

    public void WriteLine(params object[] columns) {}
    public void WriteLine(params string[] columns) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.6.0.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.4.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
