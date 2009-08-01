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

namespace Smdn.Interop {
  public static class kernel32 {
    [DllImport("kernel32")] public static extern IntPtr LoadLibrary(string lpFileName);
    [DllImport("kernel32")] public static extern bool FreeLibrary(IntPtr hModule);
    [DllImport("kernel32")] public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    [DllImport("kernel32")] public static extern IntPtr GetProcessHeap();
    [DllImport("kernel32")] public static extern IntPtr HeapAlloc(IntPtr hHeap, uint dwFlags, uint dwBytes);
    [DllImport("kernel32")] public static extern IntPtr HeapReAlloc(IntPtr hHeap, uint dwFlags, IntPtr lpMem, uint dwBytes);
    [DllImport("kernel32")] public static extern bool HeapFree(IntPtr hHeap, uint dwFlags, IntPtr lpMem);
    [DllImport("kernel32")] public static extern uint HeapSize(IntPtr hHeap, int flags, IntPtr lpMem);
  }
}
