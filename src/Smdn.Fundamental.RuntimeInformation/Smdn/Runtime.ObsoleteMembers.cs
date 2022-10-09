// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.InteropServices;

namespace Smdn;

#pragma warning disable IDE0040
partial class Runtime {
#pragma warning restore IDE0040
  [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
  public static bool IsRunningOnWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

  [Obsolete("use Smdn.Platform.IsRunningOnUnix")]
  public static bool IsRunningOnUnix
    => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
}
