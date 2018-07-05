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
using System.Reflection;
using System.Linq;

#if !NET
using System.Runtime.InteropServices;
#endif

namespace Smdn {
  public static class Runtime {
    private static readonly RuntimeEnvironment runtimeEnvironment;
    private static readonly string name;

    static Runtime()
    {
#if NET471 || NETSTANDARD2_0
      if (RuntimeInformation.FrameworkDescription.Contains(".NET Framework")) {
        runtimeEnvironment = RuntimeEnvironment.NetFx;
        name = ".NET Framework";
        return;
      }
      else if (RuntimeInformation.FrameworkDescription.Contains(".NET Core")) {
        runtimeEnvironment = RuntimeEnvironment.NetCore;
        name = ".NET Core";
        return;
      }
      else if (RuntimeInformation.FrameworkDescription.Contains("Mono")) {
        runtimeEnvironment = RuntimeEnvironment.Mono;
        name = "Mono";
        //return; // mono?
      }
#endif

      if (Type.GetType("Mono.Runtime") != null) {
        /*
         * http://mono-project.com/FAQ:_Technical
         */
        runtimeEnvironment = RuntimeEnvironment.Mono;
        name = "Mono";
      }
      else if (Type.GetType("FXAssembly") != null) {
        runtimeEnvironment = RuntimeEnvironment.NetFx;
        name = ".NET Framework";
      }
      // XXX
      else if (typeof(Runtime).GetTypeInfo().Assembly.GetReferencedAssemblies().Any(n => n.Name.Equals("System.Runtime", StringComparison.Ordinal))) {
        runtimeEnvironment = RuntimeEnvironment.NetCore;
        name = ".NET Core";
      }
      else {
        runtimeEnvironment = RuntimeEnvironment.Unknown;
        name = ".NET Framework compatible";
      }
    }

    public static RuntimeEnvironment RuntimeEnvironment {
      get { return runtimeEnvironment; }
    }

    public static string Name {
      get { return name; }
    }

    public static bool IsRunningOnNetFx {
      get { return runtimeEnvironment == RuntimeEnvironment.NetFx; }
    }

    public static bool IsRunningOnNetCore {
      get { return runtimeEnvironment == RuntimeEnvironment.NetCore; }
    }

    public static bool IsRunningOnMono {
      get { return runtimeEnvironment == RuntimeEnvironment.Mono; }
    }

    public static bool IsRunningOnWindows {
      get {
#if NET471 || NETSTANDARD2_0 || NETSTANDARD1_6
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
        return (int)Environment.OSVersion.Platform < 4;
#endif
      }
    }

    public static bool IsRunningOnUnix {
      get
      {
#if NET471 || NETSTANDARD2_0 || NETSTANDARD1_6
        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
               RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#else
        var platform = (int)Environment.OSVersion.Platform;

        return (platform == 4 || platform == 6 || platform == 128);
#endif
      }
    }

    private static string versionString = null;

    public static string VersionString {
      get {
        if (versionString == null) {
#if NET471 || NETSTANDARD2_0
          versionString = RuntimeInformation.FrameworkDescription;
#else
          versionString = string.Format("{0} {1}", name, Version);
#endif
        }

        return versionString;
      }
    }

    public static Version Version {
      get
      {
        switch (runtimeEnvironment) {
          case RuntimeEnvironment.Mono: {
#if NET
            var displayName = (string)Type.GetType("Mono.Runtime").InvokeMember("GetDisplayName", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding, null, null, Type.EmptyTypes);
#else
            var displayName = (string)Type.GetType("Mono.Runtime").GetTypeInfo().GetDeclaredMethod("GetDisplayName").Invoke(null, null);
#endif

            foreach (var s in displayName.Split(' ')) {
              try {
                return new Version(s);
              }
              catch (ArgumentException) {
                // ignore
              }
            }

            break;
          }
        }

#if NET || NETSTANDARD2_0
        return Environment.Version;
#else
        return null;
#endif
      }
    }
  }
}
