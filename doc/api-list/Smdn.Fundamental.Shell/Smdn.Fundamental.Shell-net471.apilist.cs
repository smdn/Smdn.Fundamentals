// Smdn.Fundamental.Shell.dll (Smdn.Fundamental.Shell-3.0.0 (net471))
//   Name: Smdn.Fundamental.Shell
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net471)
//   TargetFramework: .NETFramework,Version=v4.7.1
//   Configuration: Release

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Smdn.OperatingSystem;

namespace Smdn.OperatingSystem {
  // Forwarded to "Smdn.Fundamental.Shell, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class EnvironmentVariable {
    public static string CombineEnvironmentVariables(IDictionary<string, string> variables) {}
    public static Dictionary<string, string> ParseEnvironmentVariables(string variables) {}
    public static Dictionary<string, string> ParseEnvironmentVariables(string variables, bool throwIfInvalid) {}
  }

  // Forwarded to "Smdn.Fundamental.Shell, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class PipeOutStream : Stream {
    public PipeOutStream(ProcessStartInfo startInfo) {}
    public PipeOutStream(ProcessStartInfo startInfo, DataReceivedEventHandler onErrorDataReceived) {}
    public PipeOutStream(ProcessStartInfo startInfo, DataReceivedEventHandler onOutputDataReceived, DataReceivedEventHandler onErrorDataReceived) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public override long Length { get; }
    public override long Position { get; set; }
    public Process Process { get; }
    public ProcessStartInfo StartInfo { get; }
    public int WaitForExitTimeout { get; }

    public override void Close() {}
    public override void Flush() {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override void WriteByte(byte @value) {}
  }

  // Forwarded to "Smdn.Fundamental.Shell, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class Shell {
    public static ProcessStartInfo CreateProcessStartInfo(string command, params string[] arguments) {}
    public static ProcessStartInfo CreateProcessStartInfo(string command, string arguments) {}
    public static int Execute(string command, out string stdout) {}
    public static int Execute(string command, out string stdout, out string stderr) {}
    public static string Execute(string command) {}
  }

  // Forwarded to "Smdn.Fundamental.Shell, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class ShellString :
    ICloneable,
    IEquatable<ShellString>,
    IEquatable<string>
  {
    public ShellString(string raw) {}

    public string Expanded { get; }
    public bool IsEmpty { get; }
    public string Raw { get; set; }

    public ShellString Clone() {}
    public bool Equals(ShellString other) {}
    public bool Equals(string other) {}
    public override bool Equals(object obj) {}
    public static string Expand(ShellString str) {}
    public override int GetHashCode() {}
    public static bool IsNullOrEmpty(ShellString str) {}
    object ICloneable.Clone() {}
    public override string ToString() {}
    public static bool operator == (ShellString x, ShellString y) {}
    public static explicit operator string(ShellString str) {}
    public static implicit operator ShellString(string str) {}
    public static bool operator != (ShellString x, ShellString y) {}
  }
}

