// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.Extending;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class ExtendStream : ExtendStreamBase {
  protected override bool CanSeekPrependedData => prependStream?.CanSeek ?? true;
  protected override bool CanSeekAppendedData => appendStream?.CanSeek ?? true;

  public ExtendStream(
    Stream innerStream,
    byte[]? prependData,
    byte[]? appendData,
    bool leaveInnerStreamOpen = true
  )
    : this(
      innerStream,
      prependData is null ? null : new MemoryStream(prependData, false),
      appendData is null ? null : new MemoryStream(appendData, false),
      leaveInnerStreamOpen,
      leavePrependStreamOpen: false,
      leaveAppendStreamOpen: false
    )
  {
  }

  public ExtendStream(
    Stream innerStream,
    Stream? prependStream,
    Stream? appendStream,
    bool leaveInnerStreamOpen = true,
    bool leavePrependStreamOpen = true,
    bool leaveAppendStreamOpen = true
  )
    : base(
      innerStream,
      prependStream?.Length ?? 0L,
      appendStream?.Length ?? 0L,
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
    if (prependStream is not null)
      prependStream.Position = position;
  }

  protected override void SetAppendedDataPosition(long position)
  {
    if (appendStream is not null)
      appendStream.Position = position;
  }

  private static Exception CreateAccessToNullPrependDataException()
    => new InvalidOperationException("prepend data is not set");

  protected override int ReadPrependedData(byte[] buffer, int offset, int count)
    => prependStream is null
      ? throw CreateAccessToNullPrependDataException()
      : prependStream.Read(buffer, offset, count);

  protected override Task<int> ReadPrependedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    => prependStream is null
      ? throw CreateAccessToNullPrependDataException()
      : prependStream.ReadAsync(buffer, offset, count, cancellationToken);

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  protected override ValueTask<int> ReadPrependedDataAsync(Memory<byte> buffer, CancellationToken cancellationToken)
    => prependStream is null
      ? throw CreateAccessToNullPrependDataException()
      : prependStream.ReadAsync(buffer, cancellationToken);
#endif

  private static Exception CreateAccessToNullAppendDataException()
    => new InvalidOperationException("append data is not set");

  protected override int ReadAppendedData(byte[] buffer, int offset, int count)
    => appendStream is null
      ? throw CreateAccessToNullAppendDataException()
      : appendStream.Read(buffer, offset, count);

  protected override Task<int> ReadAppendedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    => appendStream is null
      ? throw CreateAccessToNullAppendDataException()
      : appendStream.ReadAsync(buffer, offset, count, cancellationToken);

#if SYSTEM_IO_STREAM_READASYNC_MEMORY_OF_BYTE
  protected override ValueTask<int> ReadAppendedDataAsync(Memory<byte> buffer, CancellationToken cancellationToken)
    => appendStream is null
      ? throw CreateAccessToNullAppendDataException()
      : appendStream.ReadAsync(buffer, cancellationToken);
#endif

  private Stream? prependStream;
  private Stream? appendStream;
  private readonly bool leavePrependStreamOpen;
  private readonly bool leaveAppendStreamOpen;
}
