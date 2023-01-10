// Smdn.Fundamental.Csv.dll (Smdn.Fundamental.Csv-3.1.0)
//   Name: Smdn.Fundamental.Csv
//   AssemblyVersion: 3.1.0.0
//   InformationalVersion: 3.1.0+ad2cc00a0ff6e5aaffa22f38b16db8977775c0d2
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Shim, Version=3.1.3.0, Culture=neutral
//     System.Buffers, Version=4.0.2.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Collections, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Diagnostics.Debug, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.IO, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.IO.FileSystem, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.IO.FileSystem.Primitives, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Linq, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Runtime, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.Extensions, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.InteropServices, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Text.Encoding, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a

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
