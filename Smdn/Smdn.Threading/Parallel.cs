// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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

using Smdn.Collections;

namespace Smdn.Threading {
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
          for (var i = fromInclusive; i < toExclusive; i++) {
            ThreadPool.QueueUserWorkItem(delegate(object state) {
              try {
                action((int)state);
              }
              finally {
                if (Interlocked.Decrement(ref count) == 0)
                  wait.Set();
              }
            }, i);
          }

          wait.WaitOne();
        }
      }
    }

    public static void ForEach<T>(IEnumerable<T> enumerable, Action<T> action)
    {
      if (enumerable == null)
        throw new ArgumentNullException();

      var count = enumerable.Count();

      if (count == 0)
        return;
      else if (action == null)
        throw new ArgumentNullException("action");

      if (count == 1) {
        action(enumerable.First());
      }
      else {
        using (var wait = new AutoResetEvent(false)) {
          foreach (var e in enumerable) {
            ThreadPool.QueueUserWorkItem(delegate(object state) {
              try {
                action((T)state);
              }
              finally {
                if (Interlocked.Decrement(ref count) == 0)
                  wait.Set();
              }
            }, e);
          }

          wait.WaitOne();
        }
      }
    }
  }
}
