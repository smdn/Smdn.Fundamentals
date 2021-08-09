// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET471 || NETSTANDARD1_6 || NETSTANDARD2_0 ||  NETSTANDARD2_1
#define RUNTIME_INFORMATION
using System.Runtime.InteropServices;
#endif

using System;
using System.IO;

using Smdn.OperatingSystem;

namespace Smdn {
  public static class Platform {
    static Platform()
    {
      // System.BitConverter.IsLittleEndian
      unsafe {
        int i = 1;
        byte* b = (byte*)&i;

        if (b[0] == 1)
          Endianness = Endianness.LittleEndian;
        else if (b[3] == 1)
          Endianness = Endianness.BigEndian;
        else
          Endianness = Endianness.Unknown;
      }
    }

    public static readonly Endianness Endianness;

    public static bool IsRunningOnWindows =>
#if RUNTIME_INFORMATION
      RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
      (int)Environment.OSVersion.Platform < 4;
#endif

    public static bool IsRunningOnUnix
#if RUNTIME_INFORMATION
      => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#else
    {
      get {
        var platform = (int)Environment.OSVersion.Platform;

        return (platform == 4 || platform == 6 || platform == 128);
      }
    }
#endif

    public static StringComparison PathStringComparison => IsRunningOnWindows ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
    public static StringComparer PathStringComparer => IsRunningOnWindows ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

    private static string kernelName = null;

    public static string KernelName {
      get {
        if (kernelName == null) {
#if RUNTIME_INFORMATION
          kernelName = $"{RuntimeInformation.OSDescription} {RuntimeInformation.ProcessArchitecture}"; // default
#else
          kernelName = Environment.OSVersion.Platform.ToString(); // default
#endif

#if SYSTEM_DIAGNOSTICS_PROCESS
          try {
            if (IsRunningOnUnix)
              kernelName = Shell.Execute("uname -srvom").Trim();
          }
          catch {
            // ignore exceptions
          }
#endif
        }

        return kernelName;
      }
    }

    private static string distributionName = null;

    public static string DistributionName {
      get {
        if (distributionName == null) {
#if RUNTIME_INFORMATION
          distributionName = RuntimeInformation.OSDescription; // default
#else
          distributionName = Environment.OSVersion.VersionString; // default
#endif

#if SYSTEM_DIAGNOSTICS_PROCESS
          try {
            if (IsRunningOnUnix)
              distributionName = Shell.Execute("lsb_release -ds").Trim();
          }
          catch {
            // ignore exceptions
          }
#endif
        }

        return distributionName;
      }
    }

    private static string processorName = null;

    public static string ProcessorName {
      get
      {
        if (processorName == null) {
#if RUNTIME_INFORMATION
          processorName = RuntimeInformation.OSArchitecture.ToString(); // default
#else
          processorName = string.Empty; // default
#endif

          try {
            if (IsRunningOnUnix) {
              foreach (var line in File.ReadAllLines("/proc/cpuinfo")) {
                if (line.StartsWith("model name", StringComparison.Ordinal)) {
                  processorName = line.Substring(line.IndexOf(':') + 1).Trim();
                  break;
                }
              }
            }
            else {
              // TODO:
            }
          }
          catch {
            // ignore exceptions
          }
        }

        return processorName;
      }
    }
  }
}
