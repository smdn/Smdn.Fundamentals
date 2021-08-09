// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Runtime.InteropServices;

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
      RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static bool IsRunningOnUnix
      => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    public static StringComparison PathStringComparison => IsRunningOnWindows ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
    public static StringComparer PathStringComparer => IsRunningOnWindows ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

    private static string kernelName = null;

    public static string KernelName {
      get {
        if (kernelName == null) {
          kernelName = $"{RuntimeInformation.OSDescription} {RuntimeInformation.ProcessArchitecture}"; // default

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
          distributionName = RuntimeInformation.OSDescription; // default

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
          processorName = RuntimeInformation.OSArchitecture.ToString(); // default

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
