// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.LineOriented;

#pragma warning disable IDE0040
partial class LineOrientedStream {
#pragma warning restore IDE0040
  private const int CopyToDefaultBufferSize = 1;

#pragma warning disable CS0109
  public new Task CopyToAsync(
    Stream destination,
    CancellationToken cancellationToken
  )
#pragma warning restore CS0109
    => CopyToAsync(
      destination: destination,
      bufferSize: CopyToDefaultBufferSize,
      cancellationToken: cancellationToken
    );

  /// <param name="destination">The destination stream.</param>
  /// <param name="bufferSize">The value of this parameter does not affect to the behavior of the method.</param>
  public override Task CopyToAsync(
    Stream destination,
    int bufferSize = CopyToDefaultBufferSize,
    CancellationToken cancellationToken = default
  )
  {
    CheckDisposed();

#if SYSTEM_IO_STREAM_VALIDATECOPYTOARGUMENTS
    ValidateCopyToArguments(destination, bufferSize);
#else
    if (destination is null)
      throw new ArgumentNullException(nameof(destination));
    if (!destination.CanWrite)
      throw new NotSupportedException("The destination stream does not support writing."); // ExceptionUtils.CreateArgumentMustBeWritableStream(nameof(destination));
    if (bufferSize <= 0)
      throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(bufferSize), bufferSize);
#endif

    return ReadAsyncCore(
      destination,
      null, // read to end
      cancellationToken
    );
  }
}
