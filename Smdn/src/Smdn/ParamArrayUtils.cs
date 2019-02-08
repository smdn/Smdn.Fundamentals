// 
// Copyright (c) 2018 smdn <smdn@smdn.jp>
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

using Smdn.Collections;

namespace Smdn {
  public static class ParamArrayUtils {
    public static IEnumerable<TParam> ToEnumerable<TParam>(TParam first, params TParam[] subsequence)
    {
      yield return first;

      if (subsequence == null)
        yield break;

      foreach (var item in subsequence)
        yield return item;
    }

    public static IEnumerable<TParam> ToEnumerableNonNullable<TParam>(string paramName, TParam first, params TParam[] subsequence) where TParam : class
    {
      if (first == null)
        throw ExceptionUtils.CreateAllItemsOfArgumentMustBeNonNull(paramName);

      yield return first;

      if (subsequence == null)
        yield break;

      foreach (var item in subsequence) {
        if (item == null)
          throw ExceptionUtils.CreateAllItemsOfArgumentMustBeNonNull(paramName);

        yield return item;
      }
    }

    public static IReadOnlyList<TParam> ToList<TParam>(TParam first, params TParam[] subsequence)
    {
      if (subsequence == null || subsequence.Length == 0)
        return Singleton.CreateList(first);

      var list = new TParam[1 + subsequence.Length];

      list[0] = first;

      Array.Copy(subsequence, 0, list, 1, subsequence.Length);

      return list;
    }

    public static IReadOnlyList<TParam> ToListNonNullable<TParam>(string paramName, TParam first, params TParam[] subsequence) where TParam : class
    {
      if (first == null)
        throw ExceptionUtils.CreateAllItemsOfArgumentMustBeNonNull(paramName);

      if (subsequence == null || subsequence.Length == 0)
        return Singleton.CreateList(first);

      var list = new TParam[1 + subsequence.Length];
      var index = 0;

      list[index++] = first;

      foreach (var item in subsequence) {
        list[index++] = item ?? throw ExceptionUtils.CreateAllItemsOfArgumentMustBeNonNull(paramName);
      }

      return list;
    }
  }
}
