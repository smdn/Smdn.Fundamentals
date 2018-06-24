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
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Smdn.OperatingSystem;

namespace Smdn.IO {
  public static class FileManager {
    [DllImport("shell32.dll")]
    private static extern bool SHObjectProperties(IntPtr hwnd,
                                                  uint shopObjectType,
                                                  [MarshalAs(UnmanagedType.LPWStr)] string pszObjectName,
                                                  [MarshalAs(UnmanagedType.LPWStr)] string pszPropertyPage);

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
          // quote
          path = "\"" + path + "\"";

          if (selected)
            psi.Arguments = "/select," + path;
          else
            psi.Arguments = path;
        }
      }
      else if (Runtime.IsRunningOnUnix) {
        if (FindNautilus(out var filemanager)) {
          psi = new ProcessStartInfo(filemanager);
          psi.UseShellExecute = false;

          if (path != null) {
            if (File.Exists(path)) {
              path = Path.GetDirectoryName(path);
            }
            else if (selected) {
              var parent = Directory.GetParent(path);

              if (parent != null)
                path = parent.FullName;
            }

            path = "\"" + path.Replace("\"", "\\\"") + "\"";

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
        if (process != null) // Process.Start may return null
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
        throw new ArgumentNullException(nameof(path));

      if (Runtime.IsRunningOnWindows) {
        const int SHOP_FILEPATH = 0x2;

        SHObjectProperties(IntPtr.Zero,
                           SHOP_FILEPATH,
                           path,
                           string.Empty);
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

    /// <summary>open or execute path with UseShellExecute</summary>
    public static void Open(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof(path));

      var psi = new ProcessStartInfo();

      psi.FileName = path;
      psi.ErrorDialog = true;
      psi.UseShellExecute = true;

      try {
        using (var process = Process.Start(psi)) {
          if (process != null) // Process.Start may return null
            process.Close();
        }
      }
      catch (System.ComponentModel.Win32Exception) {
        // ignore
      }
    }
  }
}
