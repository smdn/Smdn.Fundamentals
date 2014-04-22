// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010-2014 smdn
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
using System.Runtime.Serialization;

namespace Smdn.Formats {
  [Serializable]
  public class EncodingNotSupportedException : NotSupportedException {
    /*
     * XXX: code page not supported
    public int CodePage {
      get; private set;
    }
    */

    public string EncodingName {
      get; private set;
    }

    public EncodingNotSupportedException()
      : this(null,
             "encoding is not supported by runtime",
             null)
    {
    }

    public EncodingNotSupportedException(string encodingName)
      : this(encodingName,
             string.Format("encoding '{0}' is not supported by runtime", encodingName),
             null)
    {
    }
    
    public EncodingNotSupportedException(string encodingName, Exception innerException)
      : this(encodingName,
             string.Format("encoding '{0}' is not supported by runtime", encodingName),
             innerException)
    {
    }

    public EncodingNotSupportedException(string encodingName, string message)
      : this(encodingName, message, null)
    {
    }

    public EncodingNotSupportedException(string encodingName, string message, Exception innerException)
      : base(message, innerException)
    {
      EncodingName = encodingName;
    }

    protected EncodingNotSupportedException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      EncodingName = info.GetString("EncodingName");
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);

      info.AddValue("EncodingName", EncodingName);
    }
  }
}

