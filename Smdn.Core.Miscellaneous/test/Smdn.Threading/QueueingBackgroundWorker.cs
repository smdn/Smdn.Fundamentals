using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using NUnit.Framework;

namespace Smdn.Threading {
  [TestFixture()]
  public class QueueingBackgroundWorkerTests {
    [Test]
    public void TestQueueWorkerAsync()
    {
      using (var worker = new QueueingBackgroundWorker()) {
        var maxWorker = 10;
        var ranWorkers = new bool[maxWorker];
        var doneWorkers = new bool[maxWorker];
        var completedWorkers = new bool[maxWorker];
        var results = new int[maxWorker];
        var exceptions = new List<Exception>();
        var allDone = false;

        worker.DoWork += delegate(object sender, DoWorkEventArgs e) {
          var arg = (int)e.Argument;

          ranWorkers[arg] = true;
          Console.WriteLine("DoWork {0}", e.Argument);

          if (arg == 5)
            throw new InvalidOperationException(arg.ToString());
          else
            Thread.Sleep(50);

          e.Result = arg;
          doneWorkers[arg] = true;
        };
        worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e) {
          Console.WriteLine("RunWorkerCompleted");

          if (e.Error == null) {
            var result = (int)e.Result;
            completedWorkers[result] = true;
            results[result] = result;
          }
          else {
            exceptions.Add(e.Error);
          }
        };
        worker.AllWorkerCompleted += delegate(object sender, EventArgs e) {
          allDone = true;
          Console.WriteLine("AllWorkerCompleted");
        };

        for (var i = 0; i < 10; i++) {
          worker.QueueWorkerAsync(i);
        }

        Assert.Greater(worker.PendingWorkerCount, 0);

        for (var wait = 0;; wait++) {
          Thread.Sleep(25);

          if (allDone)
            break;

          if (1000 < wait)
            Assert.Fail("not completed");
        }

        for (var i = 0; i < maxWorker; i++) {
          Assert.IsTrue(ranWorkers[i], "DoWork #{0}", i);
        }
        for (var i = 0; i < maxWorker; i++) {
          if (i != 5)
            Assert.IsTrue(doneWorkers[i], "DoWork completed #{0}", i);
        }
        for (var i = 0; i < maxWorker; i++) {
          if (i != 5) {
            Assert.IsTrue(completedWorkers[i], "RunWorkerCompleted #{0}", i);
            Assert.AreEqual(i, results[i], "Result #{0}", i);
          }
        }

        Assert.IsFalse(completedWorkers[5], "RunWorkerCompleted");
        Assert.AreEqual(1, exceptions.Count);
        Assert.IsInstanceOf<InvalidOperationException>(exceptions[0]);
        Assert.AreEqual("5", exceptions[0].Message);
      }
    }

    [Test]
    public void TestQueueWorkerAsyncAfterCancelPendingAndRunningWorkerAsync()
    {
      using (var worker = new QueueingBackgroundWorker()) {
        var cancelled = false;

        worker.DoWork += delegate {
          Thread.Sleep(25);
        };
        worker.Cancelled += delegate {
          cancelled = true;
        };

        for (var i = 0; i < 10; i++) {
          worker.QueueWorkerAsync(i);
        }

        worker.CancelPendingAndRunningWorkerAsync();
        worker.QueueWorkerAsync(11);

        Thread.Sleep(100);

        Assert.IsTrue(cancelled, "cancelled");

        var pendingWorkerCount = worker.PendingWorkerCount;

        Assert.Greater(pendingWorkerCount, 0);

        worker.QueueWorkerAsync(12);

        Assert.AreEqual(pendingWorkerCount + 1, worker.PendingWorkerCount);
      }
    }

    [Test]
    public void TestCancelPendingAndRunningWorkerAsync()
    {
      using (var worker = new QueueingBackgroundWorker()) {
        var maxWorker = 10;
        var ranWorkers = new bool[maxWorker];
        var allDone = false;
        var cancelled = false;

        worker.DoWork += delegate(object sender, DoWorkEventArgs e) {
          ranWorkers[(int)e.Argument] = true;

          Thread.Sleep(50);
        };
        worker.AllWorkerCompleted += delegate {
          allDone = true;
        };
        worker.Cancelled += delegate {
          cancelled = true;
        };

        for (var i = 0; i < maxWorker; i++) {
          worker.QueueWorkerAsync(i);
        }

        Thread.Sleep(150);

        worker.CancelPendingAndRunningWorkerAsync();

        for (var wait = 0;; wait++) {
          Thread.Sleep(25);

          if (cancelled)
            break;

          if (1000 < wait)
            Assert.Fail("not cancelled");
        }

        Assert.Greater(worker.PendingWorkerCount, 0);
        Assert.IsFalse(allDone, "AllWorkerCompleted must not be raised");

        var ranWorkerCount = maxWorker - worker.PendingWorkerCount;

        for (var i = 0; i < ranWorkerCount; i++) {
          Assert.IsTrue(ranWorkers[i], "DoWork raised #{0}", i);
        }
        for (var i = ranWorkerCount; i < maxWorker; i++) {
          Assert.IsFalse(ranWorkers[i], "DoWork not raised #{0}", i);
        }
      }
    }

    [Test]
    public void TestCancelPendingAndRunningWorkerAsyncNotQueued()
    {
      using (var worker = new QueueingBackgroundWorker()) {
        var cancelled = false;

        worker.Cancelled += delegate {
          cancelled = true;
        };

        worker.CancelPendingAndRunningWorkerAsync();

        Assert.IsTrue(cancelled, "Cancelled must be raised");
      }
    }
  }
}