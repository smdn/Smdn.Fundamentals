// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_THREADING_TASKS_VALUETASK
using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Smdn.Threading;

[TestFixture()]
public class ValueTaskShimTests {
  [Test]
  public void CompletedTask()
  {
    var t = ShimTypeSystemThreadingTasksValueTaskCompletedTask.CompletedTask;

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

    var t = ShimTypeSystemThreadingTasksValueTaskFromCanceled.FromCanceled(token);

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

    var t = ShimTypeSystemThreadingTasksValueTaskFromCanceled.FromCanceled<int>(token);

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
    var t = ShimTypeSystemThreadingTasksValueTaskFromResult.FromResult(expectedResult);

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
