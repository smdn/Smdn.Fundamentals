// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2014 smdn
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

#if false
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Smdn.Collections {
  /*
   * System.Collections.Generic.HashSet<T> is available from .NET Framework 3.5
   * Starting with the .NET Framework version 4, the HashSet<T> class implements the ISet<T> interface.
   */
#if !NET_4_0
  [Serializable]
  public partial class HashSet<T> :
    System.Collections.Generic.HashSet<T>,
    ISet<T>
  {
    public HashSet()
      : base()
    {
    }

    public HashSet(IEqualityComparer<T> comparer)
      : base(comparer)
    {
    }

    public HashSet(IEnumerable<T> collection)
      : base(collection)
    {
    }

    public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
      : base(collection, comparer)
    {
    }
  }
#endif
}
#endif