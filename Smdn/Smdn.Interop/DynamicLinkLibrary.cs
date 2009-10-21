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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Smdn.Interop {
  public class DynamicLinkLibrary : DynamicLibrary {
    public DynamicLinkLibrary(string path)
      : base(path)
    {
    }

    ~DynamicLinkLibrary()
    {
      Dispose(false);
    }

    protected override IntPtr Open(string path)
    {
      if (Runtime.IsRunningOnUnix)
        throw new PlatformNotSupportedException();

      var hModule = kernel32.LoadLibrary(path);

      if (hModule == IntPtr.Zero)
        throw new Win32Exception(Marshal.GetLastWin32Error(), string.Format("LoadLibrary failed: {0}", path));

      return hModule;
    }

    protected override void Close(IntPtr handle)
    {
      kernel32.FreeLibrary(Handle);
    }

    protected override IntPtr GetFunctionPointer(int index)
    {
      if (index <= 0)
        throw new ArgumentOutOfRangeException("index", "must be non-zero positive number");

      var ptr = kernel32.GetProcAddress(Handle, new IntPtr(index & 0x0000ffff));

      if (ptr == IntPtr.Zero)
        throw new FunctionNotFoundException(string.Format("#{0}", index), base.Path);
      else
        return ptr;
    }

    protected override IntPtr GetFunctionPointer(string name)
    {
      var ptr = kernel32.GetProcAddress(Handle, name);

      if (ptr == IntPtr.Zero)
        throw new FunctionNotFoundException(name, base.Path);
      else
        return ptr;
    }
  }
}
