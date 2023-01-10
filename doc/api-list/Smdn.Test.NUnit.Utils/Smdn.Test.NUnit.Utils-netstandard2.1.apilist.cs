// Smdn.Test.NUnit.Utils.dll (Smdn.Test.NUnit.Utils-1.0.0)
//   Name: Smdn.Test.NUnit.Utils
//   AssemblyVersion: 1.0.0.0
//   InformationalVersion: 1.0.0+7598ad3f6134b7fcb21ff256682be15e2cd04b61
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release
//   Referenced assemblies:
//     System.Text.Encoding.CodePages, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     nunit.framework, Version=3.13.2.0, Culture=neutral, PublicKeyToken=2638cd05610744eb

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Smdn.Test.NUnit {
  public static class Encodings {
    public static Encoding EucJP { get; }
    public static Encoding Jis { get; }
    public static Encoding Latin1 { get; }
    public static Encoding ShiftJis { get; }
  }

  public static class IOUtils {
    public static void UsingCurrentDirectory(string path, Action action) {}
    public static async Task UsingCurrentDirectoryAsync(string path, Func<Task> action) {}
    public static void UsingDirectory(string path, Action<DirectoryInfo> action) {}
    public static void UsingDirectory(string path, bool ensureDirectoryCreated, Action<DirectoryInfo> action) {}
    public static Task UsingDirectoryAsync(string path, Func<DirectoryInfo, Task> action) {}
    public static async Task UsingDirectoryAsync(string path, bool ensureDirectoryCreated, Func<DirectoryInfo, Task> action) {}
    public static void UsingFile(string path, Action<FileInfo> action) {}
    public static async Task UsingFileAsync(string path, Func<FileInfo, Task> action) {}
  }
}

namespace Smdn.Test.NUnit.Assertion {
  public class Assert : Assert {
    public static void Elapses(TimeSpan expected, TestDelegate code, string message = null) {}
    public static void ElapsesAsync(TimeSpan expected, AsyncTestDelegate code, string message = null) {}
    public static void ElapsesInRange(TimeSpan expectedMin, TimeSpan expectedMax, TestDelegate code, string message = null) {}
    public static void ElapsesInRangeAsync(TimeSpan expectedMin, TimeSpan expectedMax, AsyncTestDelegate code, string message = null) {}
    public static void IsSerializable<TSerializable>(TSerializable obj, Action<TSerializable> testDeserializedObject = null) {}
    public static void IsSerializable<TSerializable>(TSerializable obj, IFormatter serializationFormatter, IFormatter deserializationFormatter, Action<TSerializable> testDeserializedObject = null) {}
    public static void NotElapse(TimeSpan expected, TestDelegate code, string message = null) {}
    public static void NotElapseAsync(TimeSpan expected, AsyncTestDelegate code, string message = null) {}
    public static TException ThrowsOrAggregates<TException>(TestDelegate code) where TException : Exception {}

    public Assert() {}
  }
}
