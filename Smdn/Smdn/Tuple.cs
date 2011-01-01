// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2011 smdn
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

namespace Smdn {
  /*
   * System.Tuple is available from .NET Framework 4
   */
#if !NET_4_0
  public static class Tuple {
    public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
    {
      return new Tuple<T1, T2>(item1, item2);
    }

    public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
    {
      return new Tuple<T1, T2, T3>(item1, item2, item3);
    }
  }

  [SerializableAttribute]
  public class Tuple<T1, T2> /* : IStructuralEquatable, IStructuralComparable, IComparable */ {
    public T1 Item1 {
      get; private set;
    }

    public T2 Item2 {
      get; private set;
    }

    public Tuple(T1 item1, T2 item2)
    {
      this.Item1 = item1;
      this.Item2 = item2;
    }
  }

  [SerializableAttribute]
  public class Tuple<T1, T2, T3> /* : IStructuralEquatable, IStructuralComparable, IComparable */ {
    public T1 Item1 {
      get; private set;
    }

    public T2 Item2 {
      get; private set;
    }

    public T3 Item3 {
      get; private set;
    }

    public Tuple(T1 item1, T2 item2, T3 item3)
    {
      this.Item1 = item1;
      this.Item2 = item2;
      this.Item3 = item3;
    }
  }
#endif
}
