// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2013 smdn
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

namespace Smdn {
  public static class Shell {
    public static ProcessStartInfo CreateProcessStartInfo(string command, params string[] arguments)
    {
      return CreateProcessStartInfo(command, arguments == null ? string.Empty : string.Join(" ", arguments));
    }

    public static ProcessStartInfo CreateProcessStartInfo(string command, string arguments)
    {
      if (command == null)
        throw new ArgumentNullException("command");

      ProcessStartInfo psi;

      if (Runtime.IsRunningOnUnix) {
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
      string stdout;

      Execute(command, out stdout);

      return stdout;
    }

    public static int Execute(string command, out string stdout)
    {
      string discard;

      return Execute(command, out stdout, out discard);
    }

    public static int Execute(string command, out string stdout, out string stderr)
    {
      var psi = CreateProcessStartInfo(command, string.Empty);

      psi.RedirectStandardOutput = true;
      psi.RedirectStandardError  = true;

      using (var process = Process.Start(psi)) {
        process.WaitForExit();

        stdout = process.StandardOutput.ReadToEnd();
        stderr = process.StandardError.ReadToEnd();

        return process.ExitCode;
      }
    }
  }
}
