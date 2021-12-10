// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Diagnostics;

namespace Smdn.OperatingSystem {
#if SYSTEM_DIAGNOSTICS_PROCESS
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class Shell {
    private static bool IsRunningOnUnix =>
      System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux) ||
      System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);

    public static ProcessStartInfo CreateProcessStartInfo(string command, params string[] arguments)
    {
      return CreateProcessStartInfo(command, arguments == null ? string.Empty : string.Join(" ", arguments));
    }

    public static ProcessStartInfo CreateProcessStartInfo(string command, string arguments)
    {
      if (command == null)
        throw new ArgumentNullException(nameof(command));

      ProcessStartInfo psi;

      if (IsRunningOnUnix) {
        if (arguments != null)
          arguments = arguments.Replace("\"", "\\\"");

        psi = new ProcessStartInfo("/bin/sh", string.Format("-c \"{0} {1}\"", command, arguments));
      }
      else {
        psi = new ProcessStartInfo("cmd", string.Format("/c {0} {1}", command, arguments)) {
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
    {
      return Execute(command, out stdout, out _);
    }

    public static int Execute(string command, out string stdout, out string stderr)
    {
      var psi = CreateProcessStartInfo(command, string.Empty);

      psi.RedirectStandardOutput = true;
      psi.RedirectStandardError  = true;

      using var process = Process.Start(psi);

      stdout = process.StandardOutput.ReadToEnd();
      stderr = process.StandardError.ReadToEnd();

      process.WaitForExit();

      return process.ExitCode;
    }
  }
#endif
}
