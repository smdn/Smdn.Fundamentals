// 
// Copyright (c) 2009 smdn <smdn@smdn.jp>
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
using System.Threading;
using System.Threading.Tasks;

using Smdn.IO;

namespace Smdn.IO.Streams {
  public class ExtendStream : ExtendStreamBase {
    protected override bool CanSeekPrependedData => prependStream?.CanSeek ?? true;
    protected override bool CanSeekAppendedData => appendStream?.CanSeek ?? true;

    public ExtendStream(
      Stream innerStream,
      byte[] prependData,
      byte[] appendData,
      bool leaveInnerStreamOpen = true
    )
      : this(
        innerStream,
        (prependData == null) ? null : new MemoryStream(prependData, false),
        (appendData == null) ? null : new MemoryStream(appendData, false),
        leaveInnerStreamOpen,
        leavePrependStreamOpen: false,
        leaveAppendStreamOpen: false
      )
    {
    }

    public ExtendStream(
      Stream innerStream,
      Stream prependStream,
      Stream appendStream,
      bool leaveInnerStreamOpen = true,
      bool leavePrependStreamOpen = true,
      bool leaveAppendStreamOpen = true
    )
      : base(
        innerStream,
        (prependStream == null) ? 0L : prependStream.Length,
        (appendStream == null) ? 0L : appendStream.Length,
        leaveInnerStreamOpen
      )
    {
      this.prependStream = prependStream;
      this.appendStream = appendStream;
      this.leavePrependStreamOpen = leavePrependStreamOpen;
      this.leaveAppendStreamOpen = leaveAppendStreamOpen;
    }

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    public override void Close()
#else
    protected override void Dispose(bool disposing)
#endif
    {
      if (!leavePrependStreamOpen)
        prependStream?.Close();

      prependStream = null;

      if (!leaveAppendStreamOpen)
        appendStream?.Close();

      appendStream = null;

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
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

    protected override int ReadPrependedData(byte[] buffer, int offset, int count)
      => prependStream.Read(buffer, offset, count);

    protected override Task<int> ReadPrependedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
      => prependStream.ReadAsync(buffer, offset, count, cancellationToken);

    protected override int ReadAppendedData(byte[] buffer, int offset, int count)
      => appendStream.Read(buffer, offset, count);

    protected override Task<int> ReadAppendedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
      => appendStream.ReadAsync(buffer, offset, count, cancellationToken);

    private Stream prependStream;
    private Stream appendStream;
    private readonly bool leavePrependStreamOpen;
    private readonly bool leaveAppendStreamOpen;
  }
}
