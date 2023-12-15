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
  public void ShimType_CompletedTask()
    => Assert.That(
#if SYSTEM_THREADING_TASKS_VALUETASK_COMPLETEDTASK
      typeof(System.Threading.Tasks.ValueTask)
#else
      typeof(Smdn.Threading.ValueTaskShim)
#endif
      ,
      Is.EqualTo(typeof(ShimTypeSystemThreadingTasksValueTaskCompletedTask))
    );

  [Test]
  public void CompletedTask()
  {
    var t = ShimTypeSystemThreadingTasksValueTaskCompletedTask.CompletedTask;

    Assert.That(t.IsCompletedSuccessfully, Is.True, nameof(ValueTask.IsCompletedSuccessfully));
    Assert.That(t.IsCompleted, Is.True, nameof(ValueTask.IsCompleted));
    Assert.That(t.IsCanceled, Is.False, nameof(ValueTask.IsCanceled));
    Assert.That(t.IsFaulted, Is.False, nameof(ValueTask.IsFaulted));

    Assert.DoesNotThrowAsync(async () => await t);
  }

  [Test]
  public void ShimType_FromCanceled()
    => Assert.That(
#if SYSTEM_THREADING_TASKS_VALUETASK_FROMCANCELED
      typeof(System.Threading.Tasks.ValueTask)
#else
      typeof(Smdn.Threading.ValueTaskShim)
#endif
      ,
      Is.EqualTo(typeof(ShimTypeSystemThreadingTasksValueTaskFromCanceled))
    );

  [Test]
  public void FromCanceled()
  {
    var cts = new CancellationTokenSource();
    var token = cts.Token;

    cts.Cancel();

    var t = ShimTypeSystemThreadingTasksValueTaskFromCanceled.FromCanceled(token);

    Assert.That(t.IsCompletedSuccessfully, Is.False, nameof(ValueTask.IsCompletedSuccessfully));
    Assert.That(t.IsCompleted, Is.True, nameof(ValueTask.IsCompleted));
    Assert.That(t.IsCanceled, Is.True, nameof(ValueTask.IsCanceled));
    Assert.That(t.IsFaulted, Is.False, nameof(ValueTask.IsFaulted));

    Assert.ThrowsAsync<TaskCanceledException>(async () => await t);
  }

  [Test]
  public void FromCanceledOfTResult()
  {
    var cts = new CancellationTokenSource();
    var token = cts.Token;

    cts.Cancel();

    var t = ShimTypeSystemThreadingTasksValueTaskFromCanceled.FromCanceled<int>(token);

    Assert.That(t.IsCompletedSuccessfully, Is.False, nameof(ValueTask.IsCompletedSuccessfully));
    Assert.That(t.IsCompleted, Is.True, nameof(ValueTask.IsCompleted));
    Assert.That(t.IsCanceled, Is.True, nameof(ValueTask.IsCanceled));
    Assert.That(t.IsFaulted, Is.False, nameof(ValueTask.IsFaulted));

    Assert.ThrowsAsync<TaskCanceledException>(async () => await t);
  }

  [Test]
  public void ShimType_FromResult()
    => Assert.That(
#if SYSTEM_THREADING_TASKS_VALUETASK_FROMRESULT
      typeof(System.Threading.Tasks.ValueTask)
#else
      typeof(Smdn.Threading.ValueTaskShim)
#endif
      ,
      Is.EqualTo(typeof(ShimTypeSystemThreadingTasksValueTaskFromResult))
    );

  [Test]
  public void FromResult()
  {
    var expectedResult = 1;
    var t = ShimTypeSystemThreadingTasksValueTaskFromResult.FromResult(expectedResult);

    Assert.That(t.IsCompletedSuccessfully, Is.True, nameof(ValueTask.IsCompletedSuccessfully));
    Assert.That(t.IsCompleted, Is.True, nameof(ValueTask.IsCompleted));
    Assert.That(t.IsCanceled, Is.False, nameof(ValueTask.IsCanceled));
    Assert.That(t.IsFaulted, Is.False, nameof(ValueTask.IsFaulted));

    int actualResult = default;

    Assert.DoesNotThrowAsync(async () => actualResult = await t);
    Assert.That(actualResult, Is.EqualTo(expectedResult), "result");
  }
}
#endif // #if SYSTEM_THREADING_TASKS_VALUETASK
