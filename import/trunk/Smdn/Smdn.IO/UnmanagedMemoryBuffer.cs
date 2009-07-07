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
using System.IO;
using System.Runtime.InteropServices;

namespace Smdn.IO {
  public class UnmanagedMemoryBuffer : IDisposable {
    public delegate IntPtr AllocProc(int cb);
    public delegate IntPtr ReAllocProc(IntPtr ptr, int cb);
    public delegate void FreeProc(IntPtr ptr);

    public static AllocProc   DefaultAlloc   = Marshal.AllocCoTaskMem;
    public static ReAllocProc DefaultReAlloc = Marshal.ReAllocCoTaskMem;
    public static FreeProc    DefaultFree    = Marshal.FreeCoTaskMem;

    public static UnmanagedMemoryBuffer CreateCoTaskMem(int cb)
    {
      return new UnmanagedMemoryBuffer(cb, Marshal.AllocCoTaskMem, Marshal.ReAllocCoTaskMem, Marshal.FreeCoTaskMem);
    }

    public static UnmanagedMemoryBuffer CreateCoTaskMem(byte[] data)
    {
      return new UnmanagedMemoryBuffer(data, Marshal.AllocCoTaskMem, Marshal.FreeCoTaskMem);
    }

    public static UnmanagedMemoryBuffer CreateHGlobal(int cb)
    {
      return new UnmanagedMemoryBuffer(cb, Marshal.AllocHGlobal, ReAllocHGlobal, Marshal.FreeHGlobal);
    }

    public static UnmanagedMemoryBuffer CreateHGlobal(byte[] data)
    {
      return new UnmanagedMemoryBuffer(data, Marshal.AllocHGlobal, Marshal.FreeHGlobal);
    }

    private static IntPtr ReAllocHGlobal(IntPtr ptr, int cb)
    {
      return Marshal.ReAllocHGlobal(ptr, new IntPtr(cb));
    }

    public IntPtr Buffer {
      get
      {
        CheckDisposed();
        return buffer;
      }
    }

    public int Size {
      get
      {
        CheckDisposed();
        return size;
      }
    }

    public UnmanagedMemoryBuffer(int cb)
      : this(cb, DefaultAlloc, DefaultReAlloc, DefaultFree)
    {
    }

    public UnmanagedMemoryBuffer(int cb, AllocProc alloc, FreeProc free)
      : this(cb, alloc, null, free)
    {
    }

    public UnmanagedMemoryBuffer(int cb, AllocProc alloc, ReAllocProc realloc, FreeProc free)
    {
      if (cb < 0)
        throw new ArgumentOutOfRangeException("cb must be greater than or equals to 0");
      if (alloc == null)
        throw new ArgumentNullException("alloc");
      if (free == null)
        throw new ArgumentNullException("free");

      this.realloc = realloc;
      this.free    = free;

      Alloc(alloc, cb);
    }

    public UnmanagedMemoryBuffer(byte[] data)
      : this(data, DefaultAlloc, DefaultFree)
    {
    }

    public UnmanagedMemoryBuffer(byte[] data, AllocProc alloc, FreeProc free)
    {
      if (data == null)
        throw new ArgumentNullException("data");
      if (alloc == null)
        throw new ArgumentNullException("alloc");
      if (free == null)
        throw new ArgumentNullException("free");

      this.realloc = null;
      this.free    = free;

      Alloc(alloc, data.Length);

      Marshal.Copy(data, 0, this.buffer, size);
    }

    protected virtual void Alloc(AllocProc alloc, int cb)
    {
      this.size   = cb;
      this.buffer = alloc(cb);

      if (this.buffer == IntPtr.Zero)
        throw new OutOfMemoryException("buffer allocation failed");
    }

    public void Dispose()
    {
      Free();
    }

    public virtual void Free()
    {
      Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing) {
        if (buffer != IntPtr.Zero) {
          free(buffer);
          buffer = IntPtr.Zero;
        }
      }
    }

    public virtual void ReAlloc(int cb)
    {
      CheckDisposed();

      if (realloc == null)
        throw new InvalidOperationException("realloc not allowed");

      if (cb < 0)
        throw new ArgumentOutOfRangeException("cb must be zero or positive number");

      buffer = realloc(buffer, cb);

      if (buffer == IntPtr.Zero)
        throw new OutOfMemoryException("buffer reallocation failed");

      size = cb;
    }

    public unsafe void* ToPointer()
    {
      CheckDisposed();

      return buffer.ToPointer();
    }

    public UnmanagedMemoryStream ToStream()
    {
      CheckDisposed();

      unsafe {
        return new UnmanagedMemoryStream((byte*)buffer.ToPointer(), size, size, FileAccess.ReadWrite);
      }
    }

    private void CheckDisposed()
    {
      if (buffer == IntPtr.Zero)
        throw new ObjectDisposedException(GetType().Name);
    }

    private IntPtr buffer = IntPtr.Zero;
    private int size = 0;
    private readonly ReAllocProc realloc = null;
    private readonly FreeProc free = null;
  }
}
