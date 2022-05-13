// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_IO_STREAM_COPYTO_VIRTUAL
#endif

#if !SYSTEM_IO_STREAM_VALIDATECOPYTOARGUMENTS
using System; // NotSupportedException
#endif
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.IO.Streams.LineOriented;

#pragma warning disable IDE0040
partial class LineOrientedStream {
#pragma warning restore IDE0040
  private const int CopyToDefaultBufferSize = 1;

  public new void CopyTo(Stream destination)
    => CopyTo(
      destination: destination,
      bufferSize: CopyToDefaultBufferSize
    );

#if SYSTEM_IO_STREAM_COPYTO_VIRTUAL
  /// <param name="destination">The destination stream.</param>
  /// <param name="bufferSize">The value of this parameter does not affect to the behavior of the method.</param>
  public override void CopyTo(
    Stream destination,
    int bufferSize
  )
  {
    ThrowIfDisposed();

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

    ReadToStream(
      destination: destination,
      bytesToRead: null // read to end
    );
  }
#endif

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
    int bufferSize,
    CancellationToken cancellationToken = default
  )
  {
    ThrowIfDisposed();

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

    return ReadToStreamAsync(
      destination: destination,
      bytesToRead: null, // read to end
      cancellationToken: cancellationToken
    );
  }
}
