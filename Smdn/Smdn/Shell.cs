// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2014 smdn
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
  [Obsolete("use Smdn.OperatingSystem.Shell instead")]
  public static class Shell {
    public static ProcessStartInfo CreateProcessStartInfo(string command, params string[] arguments)
    {
      return Smdn.OperatingSystem.Shell.CreateProcessStartInfo(command, arguments);
    }

    public static ProcessStartInfo CreateProcessStartInfo(string command, string arguments)
    {
      return Smdn.OperatingSystem.Shell.CreateProcessStartInfo(command, arguments);
    }

    public static string Execute(string command)
    {
      return Smdn.OperatingSystem.Shell.Execute(command);
    }

    public static int Execute(string command, out string stdout)
    {
      return Smdn.OperatingSystem.Shell.Execute(command, out stdout);
    }

    public static int Execute(string command, out string stdout, out string stderr)
    {
      return Smdn.OperatingSystem.Shell.Execute(command, out stdout, out stderr);
    }
  }
}
