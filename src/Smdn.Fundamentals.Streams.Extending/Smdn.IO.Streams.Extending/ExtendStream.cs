// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Smdn.IO;

namespace Smdn.IO.Streams.Extending {
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

#if SYSTEM_IO_STREAM_CLOSE
    public override void Close()
#else
    protected override void Dispose(bool disposing)
#endif
    {
      if (!leavePrependStreamOpen)
#if SYSTEM_IO_STREAM_CLOSE
        prependStream?.Close();
#else
        prependStream?.Dispose();
#endif

      prependStream = null;

      if (!leaveAppendStreamOpen)
#if SYSTEM_IO_STREAM_CLOSE
        appendStream?.Close();
#else
        appendStream?.Dispose();
#endif

      appendStream = null;

#if SYSTEM_IO_STREAM_CLOSE
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
