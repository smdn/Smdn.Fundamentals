// Smdn.Fundamental.FileSystem.dll (Smdn.Fundamental.FileSystem-3.0.2)
//   Name: Smdn.Fundamental.FileSystem
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+88a74531a79909c9ffa85e23006da7fc8c31296f
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Shim, Version=3.1.3.0, Culture=neutral
//     Smdn.Fundamental.String.Replacement, Version=3.0.2.0, Culture=neutral
//     System.Collections, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.IO, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.IO.FileSystem, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Linq, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.Extensions, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.InteropServices.RuntimeInformation, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Text.Encoding, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
#nullable enable annotations

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Smdn;

namespace Smdn.IO {
  public static class DirectoryInfoExtensions {
    public static IEnumerable<DirectoryInfo> GetDirectories(this DirectoryInfo directory, Predicate<DirectoryInfo> searchPattern) {}
    public static IEnumerable<DirectoryInfo> GetDirectories(this DirectoryInfo directory, SearchOption searchOption, Predicate<DirectoryInfo> searchPattern) {}
    public static IEnumerable<FileSystemInfo> GetFileSystemInfos(this DirectoryInfo directory, Predicate<FileSystemInfo> searchPattern) {}
    public static IEnumerable<FileSystemInfo> GetFileSystemInfos(this DirectoryInfo directory, SearchOption searchOption, Predicate<FileSystemInfo> searchPattern) {}
    public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo directory, Predicate<FileInfo> searchPattern) {}
    public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo directory, SearchOption searchOption, Predicate<FileInfo> searchPattern) {}
  }

  public static class DirectoryUtils {
    public static IEnumerable<string> GetDirectories(string directory, Predicate<string> searchPattern) {}
    public static IEnumerable<string> GetDirectories(string directory, SearchOption searchOption, Predicate<string> searchPattern) {}
    public static IEnumerable<string> GetFiles(string directory, Predicate<string> searchPattern) {}
    public static IEnumerable<string> GetFiles(string directory, SearchOption searchOption, Predicate<string> searchPattern) {}
  }

  public static class PathUtils {
    public static StringComparer DefaultPathStringComparer { get; }
    public static StringComparison DefaultPathStringComparison { get; }

    public static bool AreExtensionEqual(string path, string pathOrExtension) {}
    public static bool ArePathEqual(string pathX, string pathY) {}
    public static bool AreSameFile(string pathX, string pathY) {}
    public static string ChangeDirectoryName(string path, string newDirectoryName) {}
    public static string ChangeFileName(string path, string newFileName) {}
    public static bool ContainsShellEscapeChar(string path, Encoding encoding) {}
    public static bool ContainsShellPipeChar(string path, Encoding encoding) {}
    public static bool ContainsShellSpecialChars(string path, Encoding encoding, params byte[] specialChars) {}
    public static string GetRelativePath(string basePath, string path) {}
    public static string RemoveInvalidFileNameChars(string path) {}
    public static string RemoveInvalidPathChars(string path) {}
    public static string ReplaceInvalidFileNameChars(string path, ReplaceCharEvaluator evaluator) {}
    public static string ReplaceInvalidFileNameChars(string path, string newValue) {}
    public static string ReplaceInvalidFileNameCharsWithBlanks(string path) {}
    public static string ReplaceInvalidPathChars(string path, ReplaceCharEvaluator evaluator) {}
    public static string ReplaceInvalidPathChars(string path, string newValue) {}
    public static string ReplaceInvalidPathCharsWithBlanks(string path) {}
  }
}
