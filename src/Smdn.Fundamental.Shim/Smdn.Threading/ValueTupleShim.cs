// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.Threading;

#if SYSTEM_THREADING_TASKS_VALUETASK
public static class ValueTaskShim {
#if !SYSTEM_THREADING_TASKS_VALUETASK_COMPLETEDTASK
  public static ValueTask CompletedTask => default;
#endif

#if !SYSTEM_THREADING_TASKS_VALUETASK_FROMCANCELED
  public static ValueTask FromCanceled(CancellationToken cancellationToken)
#if SYSTEM_THREADING_TASKS_TASK_FROMCANCELED
    => new(Task.FromCanceled(cancellationToken));
#else
    => new(new Task(static () => {}, cancellationToken));
#endif
#endif

#if !SYSTEM_THREADING_TASKS_VALUETASK_FROMCANCELED
  public static ValueTask<TResult> FromCanceled<TResult>(CancellationToken cancellationToken)
#if SYSTEM_THREADING_TASKS_TASK_FROMCANCELED
    => new(Task.FromCanceled<TResult>(cancellationToken));
#else
    => new(new Task<TResult>(static () => default, cancellationToken));
#endif
#endif

#if !SYSTEM_THREADING_TASKS_VALUETASK_FROMRESULT
  public static ValueTask<TResult> FromResult<TResult>(TResult result) => new(result);
#endif
}
#endif
