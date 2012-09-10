// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2012 smdn
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

namespace Smdn.IO {
  public static class FileManager {
    [DllImport("shell32.dll")] private static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

#region "shellapi.h"
    private enum SEE_MASK : uint {
      DEFAULT             = 0x00000000,
      INVOKEIDLIST        = 0x0000000c,
      NOCLOSEPROCESS      = 0x00000040,
      DOENVSUBST          = 0x00000200,
      FLAG_NO_UI          = 0x00000400,
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SHELLEXECUTEINFO {
      public int cbSize;
      public SEE_MASK fMask;
      public IntPtr hwnd;
      public string lpVerb;
      public string lpFile;
      public string lpParameters;
      public string lpDirectory;
      public int nShow;
      public IntPtr hInstApp;
      public IntPtr lpIDList;
      public string lpClass;
      public IntPtr hkeyClass;
      public uint dwHotKey;
      public IntPtr hIcon;
      public IntPtr hProcess;

      public static readonly int Size = Marshal.SizeOf(typeof(SHELLEXECUTEINFO));
    }
#endregion

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
            if (File.Exists(path)) {
              path = Path.GetDirectoryName(path);
            }
            else if (selected) {
              var parent = Directory.GetParent(path);

              if (parent != null)
                path = parent.FullName;
            }

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

        ShellExecuteEx(ref info);
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
        throw new ArgumentNullException("path");

      var psi = new ProcessStartInfo();

      psi.FileName = path;
      psi.ErrorDialog = true;
      psi.UseShellExecute = true;

      try {
        using (var process = Process.Start(psi)) {
          process.Close();
        }
      }
      catch (System.ComponentModel.Win32Exception) {
        // ignore
      }
    }
  }
}
