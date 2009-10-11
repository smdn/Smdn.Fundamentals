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

namespace Smdn.Collections {
  public static class IEnumerableExtensions {
    public static IEnumerable<TOutput> ConvertAll<TInput, TOutput>(this IEnumerable<TInput> enumerable, Converter<TInput, TOutput> converter)
    {
      foreach (var input in enumerable) {
        yield return converter(input);
      }
    }

    public static int Count<T>(this IEnumerable<T> enumerable)
    {
      if (enumerable is System.Collections.ICollection)
        return (enumerable as System.Collections.ICollection).Count;

      // XXX
      var count = 0;

#pragma warning disable 0168 // declared but never used
      foreach (var element in enumerable) {
        count++;
      }
#pragma warning restore 0168

      return count;
    }
  }
}
