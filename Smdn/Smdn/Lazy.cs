// 
// Author:
//       smdn <smdn@smdn.jp>
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

namespace Smdn {
  /*
   * System.Lazy<T> is available from .NET Framework 4
   */
#if !NET_4_0
  [Serializable]
  /*[ComVisible(false)]*/
  /*[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]*/
  public class Lazy<T> {
    public T Value {
      get
      {
        if (isValueCreated)
          return val;

        if (initLock == null) {
          val = valueFactory();
          isValueCreated = true;
        }
        else {
          lock (initLock) {
            val = valueFactory();
            isValueCreated = true;
          }
        }

        return val;
      }
    }

    public bool IsValueCreated {
      get { return isValueCreated; }
    }

    /// <summary>Initializes a new instance of the Lazy<(Of <(T>)>) class. When lazy initialization occurs, the default constructor of the target type is used.</summary>
    /// <remarks>An instance that is created with this constructor may be used concurrently from multiple threads.</remarks>
    public Lazy()
      : this(true)
    {
    }

    public Lazy(bool isThreadSafe)
      : this(Activator.CreateInstance<T>, isThreadSafe)
    {
    }

    public Lazy(Func<T> valueFactory)
      : this(valueFactory, true)
    {
    }

    /// <summary>Initializes a new instance of the Lazy<(Of <(T>)>) class. When lazy initialization occurs, the specified initialization function and initialization mode are used.</summary>
    /// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value when it is needed.</param>
    /// <param name="isThreadSafe"> true to make this instance usable concurrently by multiple threads; false to make this instance usable by only one thread at a time.</param>
    public Lazy(Func<T> valueFactory, bool isThreadSafe)
    {
      if (valueFactory == null)
        throw new ArgumentNullException("valueFactory");

      this.valueFactory = valueFactory;

      initLock = isThreadSafe ? new object() : null;
    }

    /// <returns>Calling this method causes initialization.</returns>
    public override string ToString()
    {
      if (isValueCreated)
        return Value.ToString();
      else
        return "value is not created";
    }

    private T val;
    private Func<T> valueFactory;
    private bool isValueCreated = false;
    private object initLock;
  }
#endif
}
