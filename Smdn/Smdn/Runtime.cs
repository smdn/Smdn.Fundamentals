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
using System.Reflection;

namespace Smdn {
  public static class Runtime {
    /*
     * http://mono-project.com/FAQ:_Technical
     */
    public static bool IsRunningOnMono {
      get { return Type.GetType("Mono.Runtime") != null; }
    }

    public static bool IsRunningOnWindows {
      get { return (int)Environment.OSVersion.Platform < 4; }
    }

    public static bool IsRunningOnUnix {
      get
      {
        var platform = (int)Environment.OSVersion.Platform;

        return (platform == 4 || platform == 6 || platform == 128);
      }
    }

    public static bool IsSimdRuntimeAvailable {
      get
      {
        if (isSimdRuntimeAvailable == null)
          isSimdRuntimeAvailable = GetSimdRuntimeAvailable();

        return isSimdRuntimeAvailable.Value;
      }
    }

    public static Version Version {
      get { return Environment.Version; }
    }

    private static bool GetSimdRuntimeAvailable()
    {
      // return Mono.Simd.SimdRuntime.AccelMode != Mono.Simd.AccelMode.None;

      try {
        Assembly.Load("Mono.Simd, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");

#if true
        const string code = @"
namespace Smdn {
  public static class SimdRuntime {
    public static bool GetSimdRuntimeAvailable() {
      return Mono.Simd.SimdRuntime.AccelMode != Mono.Simd.AccelMode.None;
    }
  }
}";

        using (var provider = new Microsoft.CSharp.CSharpCodeProvider(new System.Collections.Generic.Dictionary<string, string>() {{"CompilerVersion", "v2.0"}})) {
          var options = new System.CodeDom.Compiler.CompilerParameters(new[] {"Mono.Simd"});

          options.GenerateInMemory = true;
          options.IncludeDebugInformation = false;

          var result = provider.CompileAssemblyFromSource(options, code);

          var simdRuntimeType = result.CompiledAssembly.GetType("Smdn.SimdRuntime", true);

          return (bool)simdRuntimeType.GetMethod("GetSimdRuntimeAvailable", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
        }
#else
        var simdRuntimeType = assm.GetType("Mono.Simd.SimdRuntime", true);

        // this will not return actual acceleration mode
        // (Mono does not emit replaced intrinsics, so this returns AccelMode.None always)
        var accelMode = (int)simdRuntimeType.GetProperty("AccelMode", BindingFlags.Static | BindingFlags.Public).GetValue(null, null));

        return accelMode != 0;
#endif
      }
      catch {
        return false;
      }
    }

    private static bool? isSimdRuntimeAvailable = null;
  }
}
