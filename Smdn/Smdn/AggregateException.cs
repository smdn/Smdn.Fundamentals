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
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Smdn {
    /*
   * System.AggregateException is available from .NET Framework 4
   */
#if !NET_4_0
  [Serializable]
  public class AggregateException : Exception {
    public ReadOnlyCollection<Exception> InnerExceptions {
      get { return innerExceptions; }
    }

    public AggregateException()
      : this(new Exception[0])
    {
    }

    public AggregateException(Exception innerException)
      : this(new[] {innerException})
    {
    }

    public AggregateException(Exception[] innerExceptions)
      : base()
    {
      this.innerExceptions = GetExceptions(innerExceptions);
    }

    public AggregateException(IEnumerable<Exception> innerExceptions)
      : base()
    {
      this.innerExceptions = GetExceptions(innerExceptions);
    }

    public AggregateException(string message)
      : this(message, new Exception[0])
    {
    }

    public AggregateException(string message, Exception innerException)
      : this(message, new[] {innerException})
    {
    }

    public AggregateException(string message, Exception[] innerExceptions)
      : base(message)
    {
      this.innerExceptions = GetExceptions(innerExceptions);
    }

    public AggregateException(string message, IEnumerable<Exception> innerExceptions)
      : base(message)
    {
      this.innerExceptions = GetExceptions(innerExceptions);
    }

    public AggregateException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    private static ReadOnlyCollection<Exception> GetExceptions(IEnumerable<Exception> innerExceptions)
    {
      if (innerExceptions == null)
        throw new ArgumentNullException("innerExceptions");

      var exceptions = new List<Exception>();

      foreach (var exception in exceptions) {
        if (exception == null)
          throw new ArgumentException("innerExceptions contains null", "innerExceptions");

        exceptions.Add(exception);
      }

      return new ReadOnlyCollection<Exception>(exceptions);
    }

    [NonSerialized] // TODO
    private ReadOnlyCollection<Exception> innerExceptions;
  }
#endif
}
