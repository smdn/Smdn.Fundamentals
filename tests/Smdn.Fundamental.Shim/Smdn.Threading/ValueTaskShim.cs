// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_THREADING_TASKS_VALUETASK
using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

using ShimSystemThreadingTasksValueTaskCompletedTask =
#if SYSTEM_THREADING_TASKS_VALUETASK_COMPLETEDTASK
  System.Threading.Tasks.ValueTask;
#else
  Smdn.Threading.ValueTaskShim;
#endif

using ShimSystemThreadingTasksValueTaskFromCanceled =
#if SYSTEM_THREADING_TASKS_VALUETASK_FROMCANCELED
  System.Threading.Tasks.ValueTask;
#else
  Smdn.Threading.ValueTaskShim;
#endif

using ShimSystemThreadingTasksValueTaskFromResult =
#if SYSTEM_THREADING_TASKS_VALUETASK_FROMRESULT
  System.Threading.Tasks.ValueTask;
#else
  Smdn.Threading.ValueTaskShim;
#endif

namespace Smdn.Threading;

[TestFixture()]
public class ValueTaskShimTests {
  [Test]
  public void CompletedTask()
  {
    var t = ShimSystemThreadingTasksValueTaskCompletedTask.CompletedTask;

    Assert.IsTrue(t.IsCompletedSuccessfully, nameof(ValueTask.IsCompletedSuccessfully));
    Assert.IsTrue(t.IsCompleted, nameof(ValueTask.IsCompleted));
    Assert.IsFalse(t.IsCanceled, nameof(ValueTask.IsCanceled));
    Assert.IsFalse(t.IsFaulted, nameof(ValueTask.IsFaulted));

    Assert.DoesNotThrowAsync(async () => await t);
  }

  [Test]
  public void FromCanceled()
  {
    var cts = new CancellationTokenSource();
    var token = cts.Token;

    cts.Cancel();

    var t = ShimSystemThreadingTasksValueTaskFromCanceled.FromCanceled(token);

    Assert.IsFalse(t.IsCompletedSuccessfully, nameof(ValueTask.IsCompletedSuccessfully));
    Assert.IsTrue(t.IsCompleted, nameof(ValueTask.IsCompleted));
    Assert.IsTrue(t.IsCanceled, nameof(ValueTask.IsCanceled));
    Assert.IsFalse(t.IsFaulted, nameof(ValueTask.IsFaulted));

    Assert.ThrowsAsync<TaskCanceledException>(async () => await t);
  }

  [Test]
  public void FromCanceledOfTResult()
  {
    var cts = new CancellationTokenSource();
    var token = cts.Token;

    cts.Cancel();

    var t = ShimSystemThreadingTasksValueTaskFromCanceled.FromCanceled<int>(token);

    Assert.IsFalse(t.IsCompletedSuccessfully, nameof(ValueTask.IsCompletedSuccessfully));
    Assert.IsTrue(t.IsCompleted, nameof(ValueTask.IsCompleted));
    Assert.IsTrue(t.IsCanceled, nameof(ValueTask.IsCanceled));
    Assert.IsFalse(t.IsFaulted, nameof(ValueTask.IsFaulted));

    Assert.ThrowsAsync<TaskCanceledException>(async () => await t);
  }

  [Test]
  public void FromResult()
  {
    var expectedResult = 1;
    var t = ShimSystemThreadingTasksValueTaskFromResult.FromResult(expectedResult);

    Assert.IsTrue(t.IsCompletedSuccessfully, nameof(ValueTask.IsCompletedSuccessfully));
    Assert.IsTrue(t.IsCompleted, nameof(ValueTask.IsCompleted));
    Assert.IsFalse(t.IsCanceled, nameof(ValueTask.IsCanceled));
    Assert.IsFalse(t.IsFaulted, nameof(ValueTask.IsFaulted));

    int actualResult = default;

    Assert.DoesNotThrowAsync(async () => actualResult = await t);
    Assert.AreEqual(actualResult, expectedResult, "result");
  }
}
#endif // #if SYSTEM_THREADING_TASKS_VALUETASK
