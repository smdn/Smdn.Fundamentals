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
  public class GlobalMemoryBuffer : UnmanagedMemoryBuffer {
    private static IntPtr ReAllocHGlobal(IntPtr ptr, int cb)
    {
      return Marshal.ReAllocHGlobal(ptr, new IntPtr(cb));
    }

    public GlobalMemoryBuffer(int cb)
      : base(cb, Marshal.AllocHGlobal, ReAllocHGlobal, Marshal.FreeHGlobal)
    {
    }

    public GlobalMemoryBuffer(byte[] data)
      : base(data, Marshal.AllocHGlobal, Marshal.FreeHGlobal)
    {
    }

    public GlobalMemoryBuffer(char[] data)
      : base(data, Marshal.AllocHGlobal, Marshal.FreeHGlobal)
    {
    }

    public GlobalMemoryBuffer(short[] data)
      : base(data, Marshal.AllocHGlobal, Marshal.FreeHGlobal)
    {
    }

    public GlobalMemoryBuffer(int[] data)
      : base(data, Marshal.AllocHGlobal, Marshal.FreeHGlobal)
    {
    }

    public GlobalMemoryBuffer(long[] data)
      : base(data, Marshal.AllocHGlobal, Marshal.FreeHGlobal)
    {
    }
  }
}
