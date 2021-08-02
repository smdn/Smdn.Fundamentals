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

#if NET471 || NETSTANDARD1_6 || NETSTANDARD2_0 || NETSTANDARD2_1
#define RUNTIME_INFORMATION
using System.Runtime.InteropServices;
#endif

using System;
using System.Reflection;
using System.Linq;

namespace Smdn {
  public static class Runtime {
    private static readonly RuntimeEnvironment runtimeEnvironment;
    private static readonly string name;

    static Runtime()
    {
#if RUNTIME_INFORMATION
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

    [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
    public static bool IsRunningOnWindows => Platform.IsRunningOnWindows;

    [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
    public static bool IsRunningOnUnix => Platform.IsRunningOnUnix;

    private static string versionString = null;

    public static string VersionString {
      get {
        if (versionString == null) {
#if RUNTIME_INFORMATION
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
#if RUNTIME_INFORMATION
          case RuntimeEnvironment.NetFx:
          case RuntimeEnvironment.NetCore: {
            foreach (var s in RuntimeInformation.FrameworkDescription.Split(' ')) {
              if (Version.TryParse(s, out var v))
                return v;
            }
            break;
          }
#endif

          case RuntimeEnvironment.Mono: {
#if NETFRAMEWORK
            var displayName = (string)Type.GetType("Mono.Runtime").InvokeMember("GetDisplayName", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding, null, null, Type.EmptyTypes);
#else
            var displayName = (string)Type.GetType("Mono.Runtime").GetTypeInfo().GetDeclaredMethod("GetDisplayName").Invoke(null, null);
#endif

            foreach (var s in displayName.Split(' ')) {
              if (Version.TryParse(s, out var v))
                return v;
            }

            break;
          }
        }

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
        return Environment.Version;
#else
        return null;
#endif
      }
    }
  }
}
