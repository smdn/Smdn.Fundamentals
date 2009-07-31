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
      try {
        var assm = Assembly.Load("Mono.Simd, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
        var simdRuntimeType = assm.GetType("Mono.Simd.SimdRuntime");

        // return Mono.Simd.SimdRuntime.AccelMode != AccelMode.None;
        return 0 != (int)simdRuntimeType.GetProperty("AccellMode", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
      }
      catch {
        return false;
      }
    }

    private static bool? isSimdRuntimeAvailable = null;
  }
}
