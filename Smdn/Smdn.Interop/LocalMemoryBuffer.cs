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
  public sealed class LocalMemoryBuffer : UnmanagedMemoryBuffer {
    public static LocalMemoryBuffer FromHLOCAL(IntPtr hMem)
    {
      return FromHLOCAL(hMem, false);
    }

    public static LocalMemoryBuffer FromHLOCAL(IntPtr hMem, bool lockMemory)
    {
      if (!Runtime.IsRunningOnWindows)
        throw new PlatformNotSupportedException();

      return new LocalMemoryBuffer(hMem, lockMemory);
    }

    private static int LocalSize(IntPtr ptr)
    {
      var ret = (int)kernel32.LocalSize(ptr);

      if (ret == 0)
        throw new Win32Exception(Marshal.GetLastWin32Error());
      else
        return ret;
    }

    private static void LocalFree(IntPtr ptr)
    {
      if (IntPtr.Zero != kernel32.LocalFree(ptr))
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    /*
     * instance members
     */
    public IntPtr PtrLocked {
      get { return ptrLocked; }
    }

    private LocalMemoryBuffer(IntPtr hMem, bool lockMemory)
      : base(hMem, LocalSize(hMem), LocalFree)
    {
      if (lockMemory) {
        ptrLocked = kernel32.LocalLock(hMem);

        if (ptrLocked == IntPtr.Zero)
          throw new Win32Exception(Marshal.GetLastWin32Error());
      }
      else {
        ptrLocked = IntPtr.Zero;
      }
    }

    public override void Free()
    {
      if (ptrLocked != IntPtr.Zero && !kernel32.LocalUnlock(Ptr)) {
        var lastError = Marshal.GetLastWin32Error();

        if (lastError != 0)
          throw new Win32Exception(lastError);
      }

      ptrLocked = IntPtr.Zero;

      base.Free();
    }

    private IntPtr ptrLocked;
  }
}
