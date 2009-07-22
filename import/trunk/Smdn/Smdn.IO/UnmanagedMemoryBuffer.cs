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

    public IntPtr Ptr {
      get
      {
        CheckDisposed();
        return ptr;
      }
    }

    public int Size {
      get
      {
        CheckDisposed();
        return size;
      }
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

    public UnmanagedMemoryBuffer(byte[] data, AllocProc alloc, FreeProc free)
      : this(data.Length, alloc, free)
    {
      Marshal.Copy(data, 0, this.ptr, data.Length);
    }

    public UnmanagedMemoryBuffer(char[] data, AllocProc alloc, FreeProc free)
      : this(data.Length * Marshal.SizeOf(typeof(char)), alloc, free)
    {
      Marshal.Copy(data, 0, this.ptr, data.Length);
    }

    public UnmanagedMemoryBuffer(short[] data, AllocProc alloc, FreeProc free)
      : this(data.Length * Marshal.SizeOf(typeof(short)), alloc, free)
    {
      Marshal.Copy(data, 0, this.ptr, data.Length);
    }

    public UnmanagedMemoryBuffer(int[] data, AllocProc alloc, FreeProc free)
      : this(data.Length * Marshal.SizeOf(typeof(int)), alloc, free)
    {
      Marshal.Copy(data, 0, this.ptr, data.Length);
    }

    public UnmanagedMemoryBuffer(long[] data, AllocProc alloc, FreeProc free)
      : this(data.Length * Marshal.SizeOf(typeof(long)), alloc, free)
    {
      Marshal.Copy(data, 0, this.ptr, data.Length);
    }

    protected virtual void Alloc(AllocProc alloc, int cb)
    {
      this.ptr = alloc(cb);

      if (this.ptr == IntPtr.Zero)
        throw new OutOfMemoryException("buffer allocation failed");

      this.size   = cb;
    }

    ~UnmanagedMemoryBuffer()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Free();
    }

    public virtual void Free()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (ptr == IntPtr.Zero)
        // disposed
        return;

      free(ptr);

      if (disposing) {
        realloc = null;
        free = null;
      }

      ptr = IntPtr.Zero;
    }

    public virtual void ReAlloc(int cb)
    {
      CheckDisposed();

      if (realloc == null)
        throw new InvalidOperationException("realloc not allowed");

      if (cb < 0)
        throw new ArgumentOutOfRangeException("cb must be zero or positive number");

      var newPtr = realloc(ptr, cb);

      if (newPtr == IntPtr.Zero)
        throw new OutOfMemoryException("buffer reallocation failed");

      ptr = newPtr;
      size = cb;
    }

    public unsafe void* ToPointer()
    {
      CheckDisposed();

      return ptr.ToPointer();
    }

    public byte[] ToByteArray()
    {
      CheckDisposed();

      var bytes = new byte[size];

      Marshal.Copy(ptr, bytes, 0, size);

      return bytes;
    }

    public UnmanagedMemoryStream ToStream()
    {
      CheckDisposed();

      unsafe {
        return new UnmanagedMemoryStream((byte*)ptr.ToPointer(), size, size, FileAccess.ReadWrite);
      }
    }

    private void CheckDisposed()
    {
      if (ptr == IntPtr.Zero)
        throw new ObjectDisposedException(GetType().Name);
    }

    private IntPtr ptr = IntPtr.Zero;
    private int size = 0;
    private ReAllocProc realloc = null;
    private FreeProc free = null;
  }
}
