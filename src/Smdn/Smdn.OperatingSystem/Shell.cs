// 
// Copyright (c) 2009 smdn <smdn@smdn.jp>
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
using System.Diagnostics;

namespace Smdn.OperatingSystem {
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
  public static class Shell {
    public static ProcessStartInfo CreateProcessStartInfo(string command, params string[] arguments)
    {
      return CreateProcessStartInfo(command, arguments == null ? string.Empty : string.Join(" ", arguments));
    }

    public static ProcessStartInfo CreateProcessStartInfo(string command, string arguments)
    {
      if (command == null)
        throw new ArgumentNullException(nameof(command));

      ProcessStartInfo psi;

      if (Platform.IsRunningOnUnix) {
        if (arguments != null)
          arguments = arguments.Replace("\"", "\\\"");

        psi = new ProcessStartInfo("/bin/sh", string.Format("-c \"{0} {1}\"", command, arguments));
      }
      else {
        psi = new ProcessStartInfo("cmd", string.Format("/c {0} {1}", command, arguments));
        psi.CreateNoWindow = true;
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

      using (var process = Process.Start(psi)) {
        stdout = process.StandardOutput.ReadToEnd();
        stderr = process.StandardError.ReadToEnd();

        process.WaitForExit();

        return process.ExitCode;
      }
    }
  }
#endif
}
