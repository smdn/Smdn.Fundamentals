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

namespace Smdn.IO.Streams {
  public class ExtendStream : ExtendStreamBase {
    protected override bool CanSeekPrependedData {
      get { return (prependStream == null) ? true : prependStream.CanSeek; }
    }

    protected override bool CanSeekAppendedData {
      get { return (appendStream == null) ? true : appendStream.CanSeek; }
    }

    public ExtendStream(Stream innerStream, byte[] prependData, byte[] appendData)
      : this(innerStream, prependData, appendData, true)
    {
    }

    public ExtendStream(Stream innerStream, byte[] prependData, byte[] appendData, bool leaveInnerStreamOpen)
      : this(innerStream,
             (prependData == null) ? null : new MemoryStream(prependData, false),
             (appendData == null) ? null : new MemoryStream(appendData, false),
             leaveInnerStreamOpen,
             true)
    {
    }

    public ExtendStream(Stream innerStream, Stream prependStream, Stream appendStream)
      : this(innerStream, prependStream, appendStream, true, false)
    {
    }

    public ExtendStream(Stream innerStream, Stream prependStream, Stream appendStream, bool leaveInnerStreamOpen)
      : this(innerStream, prependStream, appendStream, leaveInnerStreamOpen, false)
    {
    }

    private ExtendStream(Stream innerStream, Stream prependStream, Stream appendStream, bool leaveInnerStreamOpen, bool closeExtensionStream)
      : base(innerStream, (prependStream == null) ? 0 : prependStream.Length, (appendStream == null) ? 0 : appendStream.Length, leaveInnerStreamOpen)
    {
      this.prependStream = prependStream;
      this.appendStream = appendStream;
      this.closeExtensionStream = closeExtensionStream;
    }

#if NET46
    public override void Close()
#else
    protected override void Dispose(bool disposing)
#endif
    {
      if (closeExtensionStream) {
        if (prependStream != null) {
#if NET46
          prependStream.Close();
#else
          prependStream.Dispose();
#endif

          // TODO: must be set to null
          prependStream = null;
        }

        if (appendStream != null) {
#if NET46
          appendStream.Close();
#else
          appendStream.Dispose();
#endif

          // TODO: must be set to null
          appendStream = null;
        }
      }

#if NET46
      base.Close();
#else
      base.Dispose(disposing);
#endif
    }

    protected override void SetPrependedDataPosition(long position)
    {
      if (prependStream != null)
        prependStream.Position = position;
    }

    protected override void SetAppendedDataPosition(long position)
    {
      if (appendStream != null)
        appendStream.Position = position;
    }

    protected override void ReadPrependedData(byte[] buffer, int offset, int count)
    {
      for (;;) {
        var read = prependStream.Read(buffer, offset, count);

        offset += read;
        count  -= read;

        if (read <= 0 || count <= 0)
          break;
      }
    }

    protected override void ReadAppendedData(byte[] buffer, int offset, int count)
    {
      for (;;) {
        var read = appendStream.Read(buffer, offset, count);

        offset += read;
        count  -= read;

        if (read <= 0 || count <= 0)
          break;
      }
    }

    private Stream appendStream;
    private Stream prependStream;
    private readonly bool closeExtensionStream;
  }
}
