// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Runtime.InteropServices;

#if SYSTEM_DIAGNOSTICS_PROCESS
using Smdn.OperatingSystem;
#endif

namespace Smdn;

public static class Platform {
  public static readonly Endianness Endianness = GetEndianness();

  private static unsafe Endianness GetEndianness()
  {
    int i = 1;
    var b = (byte*)&i;

    return (b[0], b[3]) switch {
      (1, 0) => Endianness.LittleEndian,
      (0, 1) => Endianness.BigEndian,
      _ => Endianness.Unknown,
    };
  }

  public static bool IsRunningOnWindows =>
    RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

  public static bool IsRunningOnUnix
    => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

  [Obsolete("use Smdn.IO.PathUtils.DefaultPathStringComparison instead")]
  public static StringComparison PathStringComparison => Smdn.IO.PathUtils.DefaultPathStringComparison;
  [Obsolete("use Smdn.IO.PathUtils.DefaultPathStringComparer instead")]
  public static StringComparer PathStringComparer => Smdn.IO.PathUtils.DefaultPathStringComparer;

  private static string kernelName = null;

  public static string KernelName {
    get {
#pragma warning disable IDE0074
      if (kernelName == null) {
        kernelName = $"{RuntimeInformation.OSDescription} {RuntimeInformation.ProcessArchitecture}"; // default

#if SYSTEM_DIAGNOSTICS_PROCESS
        try {
          if (IsRunningOnUnix)
            kernelName = Shell.Execute("uname -srvom").Trim();
        }
#pragma warning disable CA1031
        catch {
          // ignore exceptions
        }
#pragma warning restore CA1031
#endif
      }
#pragma warning restore IDE0074

      return kernelName;
    }
  }

  private static string distributionName = null;

  public static string DistributionName {
    get {
#pragma warning disable IDE0074
      if (distributionName == null) {
        distributionName = RuntimeInformation.OSDescription; // default

#if SYSTEM_DIAGNOSTICS_PROCESS
        try {
          if (IsRunningOnUnix)
            distributionName = Shell.Execute("lsb_release -ds").Trim();
        }
#pragma warning disable CA1031
        catch {
          // ignore exceptions
        }
#pragma warning restore CA1031
#endif
      }
#pragma warning restore IDE0074

      return distributionName;
    }
  }

  private static string processorName = null;

  public static string ProcessorName {
    get {
      if (processorName == null) {
        processorName = RuntimeInformation.OSArchitecture.ToString(); // default

        try {
          if (IsRunningOnUnix) {
            foreach (var line in File.ReadAllLines("/proc/cpuinfo")) {
              if (line.StartsWith("model name", StringComparison.Ordinal)) {
                var indexOfDelimiter =
#if SYSTEM_STRING_INDEXOF_CHAR_STRINGCOMPARISON
                  line.IndexOf(':', StringComparison.Ordinal);
#else
                  line.IndexOf(':');
#endif

                processorName = line.Substring(indexOfDelimiter + 1).Trim();
                break;
              }
            }
          }
          else {
            // TODO:
          }
        }
#pragma warning disable CA1031
        catch {
          // ignore exceptions
        }
#pragma warning restore CA1031
      }

      return processorName;
    }
  }
}
