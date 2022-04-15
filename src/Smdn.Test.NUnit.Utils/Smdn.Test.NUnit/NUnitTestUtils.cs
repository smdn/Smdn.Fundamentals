// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace Smdn.Test.NUnit {
  public static partial class TestUtils {
    public static partial class Assert {
      //private static readonly TimeSpan mergin = TimeSpan.FromTicks(Stopwatch.Frequency);
      private static readonly TimeSpan mergin = TimeSpan.FromMilliseconds(20);

      public static void Elapses(TimeSpan expectedSpan, TestDelegate code)
      {
        var sw = Stopwatch.StartNew();

        code();

        sw.Stop();

        NUnitAssert.GreaterOrEqual(sw.Elapsed + mergin, expectedSpan);
      }

      public static void Elapses(TimeSpan expectedSpanRangeMin, TimeSpan expectedSpanRangeMax, TestDelegate code)
      {
        var sw = Stopwatch.StartNew();

        code();

        sw.Stop();

        NUnitAssert.GreaterOrEqual(sw.Elapsed + mergin, expectedSpanRangeMin);
        NUnitAssert.LessOrEqual(sw.Elapsed - mergin, expectedSpanRangeMax);
      }

      public static void NotElapse(TimeSpan expectedSpan, TestDelegate code)
      {
        var sw = Stopwatch.StartNew();

        code();

        sw.Stop();

        NUnitAssert.LessOrEqual(sw.Elapsed - mergin, expectedSpan);
      }

      public static TException ThrowsOrAggregates<TException>(TestDelegate code)
        where TException : Exception
      {
        try {
          code();

          NUnitAssert.Fail("expected exception {0} not thrown", typeof(TException).FullName);

          return null;
        }
        catch (AssertionException) {
          throw;
        }
        catch (Exception ex) {
          var aggregateException = ex as AggregateException;

          if (aggregateException == null) {
            NUnitAssert.IsInstanceOf<TException>(ex);

            return ex as TException;
          }
          else {
            NUnitAssert.IsInstanceOf<TException>(aggregateException.Flatten().InnerException);

            return aggregateException.InnerException as TException;
          }
        }
      }
    } // Assert

    public static void Repeat(int count, Action action)
    {
      for (var i = 0; i < count; i++) {
        action();
      }
    }
  }
}
