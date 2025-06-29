// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Smdn.OperatingSystem;

namespace Smdn.IO;

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

    if (Platform.IsRunningOnWindows) {
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
    else if (Platform.IsRunningOnUnix) {
      if (FindNautilus(out var fileManager)) {
        psi = new ProcessStartInfo(fileManager);
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

    if (Platform.IsRunningOnWindows) {
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

    if (!Platform.IsRunningOnUnix)
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
