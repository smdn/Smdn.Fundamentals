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
using System.Collections;
using System.Collections.Generic;

#if NET_3_5
using System.Linq;
#endif

namespace Smdn.Collections {
  public static class IEnumerableExtensions {
    public static IEnumerable<T> EnumerateDepthFirst<T>(this IEnumerable<T> nestedEnumerable)
      where T : IEnumerable<T>
    {
      if (nestedEnumerable == null)
        throw new ArgumentNullException("nestedEnumerable");

      var stack = new Stack<IEnumerator<T>>();
      var enumerator = nestedEnumerable.GetEnumerator();

      for (;;) {
        if (enumerator.MoveNext()) {
          stack.Push(enumerator);

          yield return enumerator.Current;

          enumerator = enumerator.Current.GetEnumerator();
        }
        else {
          if (stack.Count == 0)
            yield break;

          enumerator = stack.Pop();
        }
      }
    }
  }
}
