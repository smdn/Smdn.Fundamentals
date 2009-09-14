// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Smdn.Interop {
  public static class FileManager {
    public static void Browse()
    {
      Browse(null, false);
    }

    public static void Browse(string path)
    {
      Browse(path, false);
    }

    public static void Browse(string path, bool selected)
    {
      ProcessStartInfo psi = null;

      if (Runtime.IsRunningOnWindows) {
        psi = new ProcessStartInfo("explorer.exe");
        psi.UseShellExecute = false;

        if (path != null) {
          if (selected)
            psi.Arguments = "/select," + path;
          else
            psi.Arguments = path;
        }
      }
      else if (Runtime.IsRunningOnUnix) {
        string filemanager;

        if (FindNautilus(out filemanager)) {
          psi = new ProcessStartInfo(filemanager);
          psi.UseShellExecute = false;

          if (path != null) {
            if (File.Exists(path))
              path = Path.GetDirectoryName(path);

            psi.Arguments = string.Format("--no-default-window {0}", path);
          }
        }
        /*
        else {
          // Konquerer or else
        }
        */
      }

      if (psi == null)
        return;

      using (var process = Process.Start(psi)) {
        process.Close();
      }
    }

    public static void ShowProperty(string path)
    {
      ShowProperty(path, IntPtr.Zero);
    }

    public static void ShowProperty(string path, IntPtr hWnd)
    {
      if (path == null)
        throw new ArgumentNullException("path");

      if (Runtime.IsRunningOnWindows) {
        // this code is based on SantaMarta.Win32APIWrapper.Shell.FileProperties
        var info = new SHELLEXECUTEINFO();

        info.cbSize   = SHELLEXECUTEINFO.Size;
        info.fMask    = SEE_MASK.NOCLOSEPROCESS | SEE_MASK.INVOKEIDLIST | SEE_MASK.FLAG_NO_UI;
        info.hwnd     = hWnd;
        info.lpFile   = path;
        info.lpVerb   = "properties";
        info.nShow    = 0;

        shell32.ShellExecuteEx(ref info);
      }
      /*
      else {
        // TODO
      }
      */
    }

    private static bool FindNautilus(out string path)
    {
      path = null;

      if (!Runtime.IsRunningOnUnix)
        return false;

      if (0 == Shell.Execute("which nautilus", out path)) {
        path = path.Trim();
        return true;
      }
      else {
        return false;
      }
    }
  }
}
