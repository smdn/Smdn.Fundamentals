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
  public abstract class DynamicLibrary : IDisposable {
#region "class members"
    public static DynamicLibrary Load(string path)
    {
      if (Runtime.IsRunningOnUnix)
        //return new SharedLibrary(path);
        throw new PlatformNotSupportedException();
      else
        return new DynamicLinkLibrary(path);
    }
#endregion

#region "instance members"
    public IntPtr Handle {
      get
      {
        CheckDisposed();
        return handle;
      }
    }

    public string Path {
      get
      {
        CheckDisposed();
        return path;
      }
    }

    protected DynamicLibrary(string path)
    {
      if (path == null)
        throw new ArgumentNullException(path);

      this.handle = Open(path);
      this.path = path;
    }

    protected abstract IntPtr Open(string path);

    void IDisposable.Dispose()
    {
      Free();
    }

    public void Free()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected abstract void Close(IntPtr handle);

    protected virtual void Dispose(bool disposing)
    {
      if (handle != IntPtr.Zero) {
        Close(handle);
        handle = IntPtr.Zero;
      }
    }

    protected abstract IntPtr GetFunctionPointer(string symbol);

    /// <remarks>TDelegate must be a delegate type</remarks>
    public TDelegate GetFunction<TDelegate>(string symbol) where TDelegate : class /* instead of delegate */
    {
      if (symbol == null)
        throw new ArgumentNullException("symbol");

      return GetDelegateForFunctionPointer<TDelegate>(GetFunctionPointer(symbol));
    }

    /// <remarks>TDelegate must be a delegate type</remarks>
    public static TDelegate GetDelegateForFunctionPointer<TDelegate>(IntPtr functionPointer) where TDelegate : class /* instead of delegate */
    {
      if (functionPointer == IntPtr.Zero)
        return null;
      else
        return System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(functionPointer, typeof(TDelegate)) as TDelegate;
    }

    protected void CheckDisposed()
    {
      if (handle == IntPtr.Zero)
        throw new ObjectDisposedException(GetType().FullName);
    }

    public override string ToString()
    {
      CheckDisposed();

      return string.Format("{0} (0x{1:X})", path, handle);
    }

    private IntPtr handle;
    private string path;
#endregion
  }
}
