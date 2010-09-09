// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2010 smdn
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
using System.Threading;

namespace Smdn.Threading {
  public class ProcessLock : IDisposable {
    public bool CreatedNew {
      get; private set;
    }

    public ProcessLock()
      : this(GetMutexName(), false)
    {
    }

    public ProcessLock(string mutexName)
      : this(mutexName, false)
    {
    }

    public ProcessLock(bool global)
      : this(GetMutexName(), global)
    {
    }

    public ProcessLock(string mutexName, bool global)
    {
      if (mutexName == null)
        throw new ArgumentNullException("mutexName");

      bool createdNew = false;

      // http://msdn.microsoft.com/en-us/library/aa382954%28VS.85%29.aspx
      // http://seancallanan.spaces.live.com/Blog/cns!83CBEA993700A445!170.entry
      // TODO: MutexSecurity
      mutex = new Mutex(true, (global ? "Global\\" + mutexName : mutexName), out createdNew);

      this.CreatedNew = createdNew;
    }

    private static string GetMutexName()
    {
      return Assembly.GetEntryAssembly().GetName().Name;
    }

    ~ProcessLock()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;

      if (mutex != null) {
        if (CreatedNew)
          mutex.ReleaseMutex();

        mutex.Close();
        mutex = null;
      }
    }

    private Mutex mutex;
  }
}
