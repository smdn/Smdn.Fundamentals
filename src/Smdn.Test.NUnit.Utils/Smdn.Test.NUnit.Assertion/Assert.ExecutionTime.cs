// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Diagnostics;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Smdn.Test.NUnit.Assertion;

#pragma warning disable IDE0040
partial class Assert {
#pragma warning restore IDE0040
  private static TimeSpan MeasureExecutionTime(Action code)
  {
    var sw = Stopwatch.StartNew();

    code();

    return sw.Elapsed;
  }

  private static async Task<TimeSpan> MeasureExecutionTimeAsync(Func<Task> code)
  {
    var sw = Stopwatch.StartNew();

    await code().ConfigureAwait(false);

    return sw.Elapsed;
  }

  public static void Elapses(TimeSpan expected, Action code, string message = null)
    => That(MeasureExecutionTime(code ?? throw new ArgumentNullException(nameof(code))), Is.GreaterThanOrEqualTo(expected), message ?? "elapses");

  public static void ElapsesAsync(TimeSpan expected, Func<Task> code, string message = null)
    => That(
      async () => await MeasureExecutionTimeAsync(code ?? throw new ArgumentNullException(nameof(code))).ConfigureAwait(false),
      Is.GreaterThanOrEqualTo(expected),
      message ?? "elapses"
    );

  public static void NotElapse(TimeSpan expected, Action code, string message = null)
    => That(MeasureExecutionTime(code ?? throw new ArgumentNullException(nameof(code))), Is.LessThanOrEqualTo(expected), message ?? "not elapse");

  public static void NotElapseAsync(TimeSpan expected, Func<Task> code, string message = null)
    => That(
      async () => await MeasureExecutionTimeAsync(code ?? throw new ArgumentNullException(nameof(code))).ConfigureAwait(false),
      Is.LessThanOrEqualTo(expected),
      message ?? "not elapse"
    );

  public static void ElapsesInRange(TimeSpan expectedMin, TimeSpan expectedMax, Action code, string message = null)
    => That(MeasureExecutionTime(code ?? throw new ArgumentNullException(nameof(code))), Is.InRange(expectedMin, expectedMax), message ?? "elapses in range");

  public static void ElapsesInRangeAsync(TimeSpan expectedMin, TimeSpan expectedMax, Func<Task> code, string message = null)
    => That(
      async () => await MeasureExecutionTimeAsync(code ?? throw new ArgumentNullException(nameof(code))).ConfigureAwait(false),
      Is.InRange(expectedMin, expectedMax),
      message ?? "elapses in range"
    );
}
