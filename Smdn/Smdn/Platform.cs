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

    private static string kernelName = null;

    public static string KernelName {
      get
      {
        if (kernelName == null) {
          kernelName = Environment.OSVersion.Platform.ToString(); // default

          try {
            if (Runtime.IsRunningOnUnix)
              kernelName = Shell.Execute("uname -srvom").Trim();
          }
          catch {
            // ignore exceptions
          }
        }

        return kernelName;
      }
    }

    private static string distributionName = null;

    public static string DistributionName {
      get
      {
        if (distributionName == null) {
          distributionName = Environment.OSVersion.VersionString; // default

          try {
            if (Runtime.IsRunningOnUnix)
              distributionName = Shell.Execute("lsb_release -ds").Trim();
          }
          catch {
            // ignore exceptions
          }
        }

        return distributionName;
      }
    }

    private static string processorName = null;

    public static string ProcessorName {
      get
      {
        if (processorName == null) {
          processorName = string.Empty; // default

          try {
            if (Runtime.IsRunningOnUnix) {
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
