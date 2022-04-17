// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Smdn.Test.NUnit;

public partial class Assert : global::NUnit.Framework.Assert {
  private static readonly TimeSpan mergin = TimeSpan.FromMilliseconds(20);

  public static void Elapses(TimeSpan expectedSpan, TestDelegate code)
  {
    var sw = Stopwatch.StartNew();

    code();

    sw.Stop();

    GreaterOrEqual(sw.Elapsed + mergin, expectedSpan);
  }

  public static void Elapses(TimeSpan expectedSpanRangeMin, TimeSpan expectedSpanRangeMax, TestDelegate code)
  {
    var sw = Stopwatch.StartNew();

    code();

    sw.Stop();

    GreaterOrEqual(sw.Elapsed + mergin, expectedSpanRangeMin);
    LessOrEqual(sw.Elapsed - mergin, expectedSpanRangeMax);
  }

  public static void NotElapse(TimeSpan expectedSpan, TestDelegate code)
  {
    var sw = Stopwatch.StartNew();

    code();

    sw.Stop();

    LessOrEqual(sw.Elapsed - mergin, expectedSpan);
  }

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
