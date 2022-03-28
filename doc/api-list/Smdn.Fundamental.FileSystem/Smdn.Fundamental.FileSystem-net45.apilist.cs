// Smdn.Fundamental.FileSystem.dll (Smdn.Fundamental.FileSystem-3.0.1.1)
//   Name: Smdn.Fundamental.FileSystem
//   AssemblyVersion: 3.0.1.1
//   InformationalVersion: 3.0.1.1+5f06cefdd6e0e438d2e42e688f01cb7d40790529
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release

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
    public static bool ContainsShellEscapeChar(string path) {}
    public static bool ContainsShellEscapeChar(string path, Encoding encoding) {}
    public static bool ContainsShellPipeChar(string path) {}
    public static bool ContainsShellPipeChar(string path, Encoding encoding) {}
    public static bool ContainsShellSpecialChars(string path, Encoding encoding, params byte[] specialChars) {}
    public static string GetRelativePath(string basePath, string path) {}
    public static string RemoveInvalidFileNameChars(string path) {}
    public static string RemoveInvalidPathChars(string path) {}
    public static string RenameUnique(string file) {}
    public static string ReplaceInvalidFileNameChars(string path, ReplaceCharEvaluator evaluator) {}
    public static string ReplaceInvalidFileNameChars(string path, string newValue) {}
    public static string ReplaceInvalidFileNameCharsWithBlanks(string path) {}
    public static string ReplaceInvalidPathChars(string path, ReplaceCharEvaluator evaluator) {}
    public static string ReplaceInvalidPathChars(string path, string newValue) {}
    public static string ReplaceInvalidPathCharsWithBlanks(string path) {}
  }
}

