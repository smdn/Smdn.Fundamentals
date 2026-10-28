// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Reflection;
using System.Threading;

namespace Smdn.Threading;

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
      throw new ArgumentNullException(nameof(mutexName));

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
