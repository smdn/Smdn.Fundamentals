// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Smdn.Test.NUnit;

public partial class Assert : global::NUnit.Framework.Assert {
  private static TimeSpan MeasureExecutionTime(TestDelegate code)
  {
    var sw = Stopwatch.StartNew();

    code();

    return sw.Elapsed;
  }

  private static TimeSpan MeasureExecutionTime(AsyncTestDelegate code)
  {
    static async Task<TimeSpan> MeasureCore(AsyncTestDelegate c)
    {
      var sw = Stopwatch.StartNew();

      await c().ConfigureAwait(false);

      return sw.Elapsed;
    }

    return MeasureCore(code).GetAwaiter().GetResult(); // XXX
  }

  public static void Elapses(TimeSpan expected, TestDelegate code, string message = null)
    => That(MeasureExecutionTime(code), Is.GreaterThanOrEqualTo(expected), message ?? "elapses");

  public static void ElapsesAsync(TimeSpan expected, AsyncTestDelegate code, string message = null)
    => That(MeasureExecutionTime(code), Is.GreaterThanOrEqualTo(expected), message ?? "elapses");

  public static void NotElapse(TimeSpan expected, TestDelegate code, string message = null)
    => That(MeasureExecutionTime(code), Is.LessThanOrEqualTo(expected), message ?? "not elapse");

  public static void NotElapseAsync(TimeSpan expected, AsyncTestDelegate code, string message = null)
    => That(MeasureExecutionTime(code), Is.LessThanOrEqualTo(expected), message ?? "not elapse");

  public static void ElapsesInRange(TimeSpan expectedMin, TimeSpan expectedMax, TestDelegate code, string message = null)
    => That(MeasureExecutionTime(code), Is.InRange(expectedMin, expectedMax), message ?? "elapses in range");

  public static void ElapsesInRangeAsync(TimeSpan expectedMin, TimeSpan expectedMax, AsyncTestDelegate code, string message = null)
    => That(MeasureExecutionTime(code), Is.InRange(expectedMin, expectedMax), message ?? "elapses in range");

  public static TException ThrowsOrAggregates<TException>(TestDelegate code)
    where TException : Exception
  {
    try {
      code();

      Fail("expected exception {0} not thrown", typeof(TException).FullName);

      return null;
    }
    catch (AssertionException) {
      throw;
    }
    catch (Exception ex) {
      var aggregateException = ex as AggregateException;

      if (aggregateException == null) {
        IsInstanceOf<TException>(ex);

        return ex as TException;
      }
      else {
        IsInstanceOf<TException>(aggregateException.Flatten().InnerException);

        return aggregateException.InnerException as TException;
      }
    }
  }
}
