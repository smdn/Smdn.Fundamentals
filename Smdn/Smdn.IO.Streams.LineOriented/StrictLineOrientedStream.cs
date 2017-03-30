// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2017 smdn
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
using System.IO;

using Smdn.Text;

namespace Smdn.IO.Streams.LineOriented {
  public class StrictLineOrientedStream : LineOrientedStream {
    public StrictLineOrientedStream(Stream stream)
      : base(stream, Ascii.Octets.GetCRLF(), true, DefaultBufferSize, DefaultLeaveStreamOpen)
    {
    }

    public StrictLineOrientedStream(Stream stream, int bufferSize)
      : base(stream, Ascii.Octets.GetCRLF(), true, bufferSize, DefaultLeaveStreamOpen)
    {
    }

    public StrictLineOrientedStream(Stream stream, bool leaveStreamOpen)
      : base(stream, Ascii.Octets.GetCRLF(), true, DefaultBufferSize, leaveStreamOpen)
    {
    }
    
    public StrictLineOrientedStream(Stream stream, int bufferSize, bool leaveStreamOpen)
      : base(stream, Ascii.Octets.GetCRLF(), true, bufferSize, leaveStreamOpen)
    {
    }

    public StrictLineOrientedStream(Stream stream, byte[] newLine)
      : base(stream, newLine, true, DefaultBufferSize, DefaultLeaveStreamOpen)
    {
    }

    public StrictLineOrientedStream(Stream stream, byte[] newLine, int bufferSize)
      : base(stream, newLine, true, bufferSize, DefaultLeaveStreamOpen)
    {
    }

    public StrictLineOrientedStream(Stream stream, byte[] newLine, bool leaveStreamOpen)
      : base(stream, newLine, true, DefaultBufferSize, leaveStreamOpen)
    {
    }

    public StrictLineOrientedStream(Stream stream, byte[] newLine, int bufferSize, bool leaveStreamOpen)
      : base(stream, newLine, true, bufferSize, leaveStreamOpen)
    {
    }
  }
}
