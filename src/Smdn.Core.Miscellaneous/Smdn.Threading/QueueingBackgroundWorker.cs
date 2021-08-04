// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
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
      => AllWorkerCompleted?.Invoke(this, EventArgs.Empty);

    protected virtual void OnCancelled(EventArgs e)
      => Cancelled?.Invoke(this, EventArgs.Empty);

    private Queue pendingWorkerArgs = new Queue();
    private bool cancelled = false;
  }
}
