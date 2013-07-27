// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2013 smdn
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
using System.Collections;
using System.ComponentModel;

namespace Smdn.Threading {
  public class QueueingBackgroundWorker : BackgroundWorker {
    public event EventHandler AllWorkerCompleted;
    public event EventHandler Cancelled;

    public int PendingWorkerCount {
      get
      {
        lock (pendingWorkerArgs.SyncRoot) {
          return pendingWorkerArgs.Count;
        }
      }
    }

    public QueueingBackgroundWorker()
    {
      WorkerSupportsCancellation = true;
    }

    public void ClearPendingWorker()
    {
      lock (pendingWorkerArgs.SyncRoot) {
        pendingWorkerArgs.Clear();
      }

      cancelled = false;
    }

    public void QueueWorkerAsync()
    {
      QueueWorkerAsync(null);
    }

    public void QueueWorkerAsync(object argument)
    {
      lock (pendingWorkerArgs.SyncRoot) {
        if (IsBusy || cancelled)
          pendingWorkerArgs.Enqueue(argument);
        else
          RunWorkerAsync(argument);
      }
    }

    public void CancelPendingAndRunningWorkerAsync()
    {
      if (IsBusy) {
        CancelAsync();

        cancelled = true;
      }
      else if (!cancelled) {
        cancelled = true;

        OnCancelled(EventArgs.Empty);
      }
    }

    protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
    {
      try {
        base.OnRunWorkerCompleted(e);
      }
      finally {
        var allCompleted = true;

        lock (pendingWorkerArgs.SyncRoot) {
          if (!cancelled && 0 < pendingWorkerArgs.Count) {
            RunWorkerAsync(pendingWorkerArgs.Dequeue());
            allCompleted = false;
          }
        }

        if (cancelled)
          OnCancelled(EventArgs.Empty);
        else if (allCompleted)
          OnAllWorkerCompleted(EventArgs.Empty);
      }
    }

    protected virtual void OnAllWorkerCompleted(EventArgs e)
    {
      var ev = this.AllWorkerCompleted;

      if (ev != null)
        ev(this, EventArgs.Empty);
    }

    protected virtual void OnCancelled(EventArgs e)
    {
      var ev = this.Cancelled;

      if (ev != null)
        ev(this, EventArgs.Empty);
    }

    private Queue pendingWorkerArgs = new Queue();
    private bool cancelled = false;
  }
}
