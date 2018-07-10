// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2017 smdn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#if NET471 || NETSTANDARD2_0 || NETSTANDARD1_6
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

#if NET || NETSTANDARD2_0
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

#if !RUNTIME_INFORMATION
    private static string distributionName = null;
#endif

    public static string DistributionName {
      get {
#if RUNTIME_INFORMATION
        return RuntimeInformation.OSDescription;
#else
        if (distributionName == null) {
          distributionName = Environment.OSVersion.VersionString; // default

#if NET || NETSTANDARD2_0
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
#endif
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
