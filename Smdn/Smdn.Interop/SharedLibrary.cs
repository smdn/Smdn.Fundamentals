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

namespace Smdn.Interop {
  public class SharedLibrary : DynamicLibrary {
    public SharedLibrary(string path)
      : base(path)
    {
    }

    ~SharedLibrary()
    {
      Dispose(false);
    }

    protected override IntPtr Open(string path)
    {
      /*
#define RTLD_LAZY 0x00001
#define RTLD_NOW  0x00002
#define RTLD_BINDING_MASK   0x3
#define RTLD_NOLOAD 0x00004
#define RTLD_DEEPBIND 0x00008
#define RTLD_GLOBAL 0x00100
#define RTLD_LOCAL  0
#define RTLD_NODELETE 0x01000
      */

      if (!Runtime.IsRunningOnUnix)
        throw new PlatformNotSupportedException();

      var handle = libdl.dlopen(path, 1);

      if (handle == IntPtr.Zero)
        throw new DllNotFoundException(libdl.dlerror() ?? string.Format("dlopen failed: {0}", path));

      return handle;
    }

    protected override void Close(IntPtr handle)
    {
      libdl.dlclose(handle);
    }

    protected override IntPtr GetFunctionPointer(string symbol)
    {
      libdl.dlerror(); // clear previous error

      var ptr = libdl.dlsym(Handle, symbol);

      if (ptr == IntPtr.Zero) {
        var err = libdl.dlerror();

        if (err == null)
          return ptr;
        else
          throw new FunctionNotFoundException(symbol, base.Path, err);
      }
      else {
        return ptr;
      }
    }
  }
}
