// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Smdn.OperatingSystem;

#if SYSTEM_DIAGNOSTICS_PROCESS
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class Shell {
  private static bool IsRunningOnUnix =>
    System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux) ||
    System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);

  public static ProcessStartInfo CreateProcessStartInfo(string command, params string[] arguments)
    => CreateProcessStartInfo(command, arguments == null ? string.Empty : string.Join(" ", arguments));

  public static ProcessStartInfo CreateProcessStartInfo(string command, string arguments)
  {
    if (command == null)
      throw new ArgumentNullException(nameof(command));

    ProcessStartInfo psi;

    if (IsRunningOnUnix) {
#pragma warning disable SA1001, SA1113
      arguments = arguments?.Replace(
        "\"",
        "\\\""
#if SYSTEM_STRING_REPLACE_STRING_STRING_STRINGCOMPARISON
        , StringComparison.Ordinal
#endif
      );
#pragma warning restore SA1001, SA1113

      psi = new ProcessStartInfo("/bin/sh", $"-c \"{command} {arguments}\"");
    }
    else {
      psi = new ProcessStartInfo("cmd", $"/c {command} {arguments}") {
        CreateNoWindow = true,
      };
    }

    psi.UseShellExecute = false;

    return psi;
  }

  public static string Execute(string command)
  {
    Execute(command, out var stdout);

    return stdout;
  }

  public static int Execute(string command, out string stdout)
    => Execute(
      command: command,
      arguments: null,
      environmentVariables: null,
      out stdout,
      out _
    );

  public static int Execute(string command, out string stdout, out string stderr)
    => Execute(
      command: command,
      arguments: null,
      environmentVariables: null,
      out stdout,
      out stderr
    );

#nullable enable
  public static int Execute(
    string command,
    IEnumerable<string>? arguments,
    IReadOnlyDictionary<string, string>? environmentVariables,
    out string stdout,
    out string stderr
  )
  {
    var args = arguments is null ? string.Empty : string.Join(" ", arguments);
    var psi = CreateProcessStartInfo(command, args);

    if (environmentVariables is not null) {
      foreach (var pair in environmentVariables) {
        psi.EnvironmentVariables[pair.Key] = pair.Value;
      }
    }

    psi.RedirectStandardOutput = true;
    psi.RedirectStandardError = true;

    using var process = Process.Start(psi);

    stdout = process.StandardOutput.ReadToEnd();
    stderr = process.StandardError.ReadToEnd();

    process.WaitForExit();

    return process.ExitCode;
  }
#pragma warning disable IDE0241
#nullable restore
#pragma warning restore IDE0241
}
#endif
