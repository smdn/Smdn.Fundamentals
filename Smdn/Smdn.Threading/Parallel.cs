// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009-2010 smdn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Threading;

#if NET_3_5
using System.Linq;
#else
using Smdn.Collections;
#endif

namespace Smdn.Threading {
#if !NET_4_0
  public static class Parallel {
    public static void For(int fromInclusive, int toExclusive, Action<int> action)
    {
      if (fromInclusive == toExclusive)
        return;

      var count = toExclusive - fromInclusive;

      if (count <= 0)
        return;
      else if (action == null)
        throw new ArgumentNullException("action");

      if (count == 1) {
        action(fromInclusive);
      }
      else {
        using (var wait = new AutoResetEvent(false)) {
          var exceptions = new List<Exception>();

          for (var i = fromInclusive; i < toExclusive; i++) {
            ThreadPool.QueueUserWorkItem(delegate(object state) {
              try {
                try {
                  action((int)state);
                }
                catch (Exception ex) {
                  lock (exceptions) {
                    exceptions.Add(ex);
                  }
                }
              }
              finally {
                if (Interlocked.Decrement(ref count) == 0)
                  wait.Set();
              }
            }, i);
          }

          wait.WaitOne();

          if (0 < exceptions.Count)
            throw new AggregateException(exceptions);
        }
      }
    }

    public static void ForEach<T>(IEnumerable<T> enumerable, Action<T> action)
    {
      if (enumerable == null)
        throw new ArgumentNullException("enumerable");

      var count = enumerable.Count();

      if (count == 0)
        return;
      else if (action == null)
        throw new ArgumentNullException("action");

      if (count == 1) {
        try {
          action(enumerable.First());
        }
        catch (Exception ex) {
          throw new AggregateException(ex);
        }
      }
      else {
        using (var wait = new AutoResetEvent(false)) {
          var exceptions = new List<Exception>();

          foreach (var e in enumerable) {
            ThreadPool.QueueUserWorkItem(delegate(object state) {
              try {
                try {
                  action((T)state);
                }
                catch (Exception ex) {
                  lock (exceptions) {
                    exceptions.Add(ex);
                  }
                }
              }
              finally {
                if (Interlocked.Decrement(ref count) == 0)
                  wait.Set();
              }
            }, e);
          }

          wait.WaitOne();

          if (0 < exceptions.Count)
            throw new AggregateException(exceptions);
        }
      }
    }
  }
#endif
}
