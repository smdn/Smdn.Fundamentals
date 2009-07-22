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
using System.Runtime.InteropServices;

namespace Smdn.IO {
  public class HeapMemoryBuffer : UnmanagedMemoryBuffer {
    private const uint HEAP_ZERO_MEMORY = 0x00000008;

    [DllImport("kernel32")]
    private static extern IntPtr GetProcessHeap();

    private static IntPtr hHeap = GetProcessHeap();

    /// <value>same as return value of GetProcessHeap</value>
    public static IntPtr ProcessHeapHandle {
      get { return hHeap; }
    }

    [DllImport("kernel32")]
    private static extern IntPtr HeapAlloc(IntPtr hHeap, uint dwFlags, uint dwBytes);

    private static IntPtr HeapAlloc(int cb)
    {
      return HeapAlloc(hHeap, HEAP_ZERO_MEMORY, (uint)cb);
    }

    [DllImport("kernel32")]
    private static extern IntPtr HeapReAlloc(IntPtr hHeap, uint dwFlags, IntPtr lpMem, uint dwBytes);

    private static IntPtr HeapReAlloc(IntPtr ptr, int cb)
    {
      return HeapReAlloc(hHeap, HEAP_ZERO_MEMORY, ptr, (uint)cb);
    }

    [DllImport("kernel32")]
    private static extern bool HeapFree(IntPtr hHeap, uint dwFlags, IntPtr lpMem);

    private static void HeapFree(IntPtr ptr)
    {
      HeapFree(hHeap, 0, ptr);
    }

    /*
    [DllImport("kernel32")]
    private static extern uint HeapSize(IntPtr hHeap, int flags, IntPtr lpMem);

    public uint Size {
      get { return HeapSize(hHeap, 0, base.Buffer); }
    }
    */

    public HeapMemoryBuffer(int cb)
      : base(cb, HeapAlloc, HeapReAlloc, HeapFree)
    {
      if (!(Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows))
        throw new PlatformNotSupportedException("supported only on Windows NT/Windows 95 or over");
    }

    public HeapMemoryBuffer(byte[] data)
      : base(data, HeapAlloc, HeapFree)
    {
      if (!(Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows))
        throw new PlatformNotSupportedException("supported only on Windows NT/Windows 95 or over");
    }

    public HeapMemoryBuffer(char[] data)
      : base(data, HeapAlloc, HeapFree)
    {
      if (!(Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows))
        throw new PlatformNotSupportedException("supported only on Windows NT/Windows 95 or over");
    }
  }
}
