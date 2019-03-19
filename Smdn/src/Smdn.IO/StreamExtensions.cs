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

namespace Smdn.IO {
  public static class StreamExtensions {
#if !(NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1)
    public static void Close(this Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

      stream.Dispose();
    }
#endif

    private const int defaultCopyBufferSize = 10 * 1024;

    public static void CopyTo(
      this Stream stream,
      System.IO.BinaryWriter writer,
      int bufferSize = defaultCopyBufferSize
    )
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));
      if (writer == null)
        throw new ArgumentNullException(nameof(writer));
      if (bufferSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(bufferSize), bufferSize);

      var buffer = new byte[bufferSize]; // TODO: array pool

      for (;;) {
        var read = stream.Read(buffer, 0, bufferSize);

        if (read <= 0)
          break;

        writer.Write(buffer, 0, read);
      }
    }

    public static Task CopyToAsync(
      this Stream stream,
      System.IO.BinaryWriter writer,
      int bufferSize = defaultCopyBufferSize,
      CancellationToken cancellationToken = default
    )
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));
      if (writer == null)
        throw new ArgumentNullException(nameof(writer));
      if (bufferSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(bufferSize), bufferSize);

      return _CopyToAsync();

      async Task _CopyToAsync()
      {
        var buffer = new byte[bufferSize]; // TODO: array pool

        for (; ; ) {
          var read = await stream.ReadAsync(buffer, 0, bufferSize, cancellationToken).ConfigureAwait(false);

          if (read <= 0)
            break;

          writer.Write(buffer, 0, read/*, cancellationToken*/);
        }
      }
    }

    private const int defaultReadBufferSize = 4096;
    private const int defaultInitialBufferCapacity = 4096;

    public static byte[] ReadToEnd(
      this Stream stream,
      int readBufferSize = defaultReadBufferSize,
      int initialCapacity = defaultInitialBufferCapacity
    )
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));
      if (readBufferSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(readBufferSize), readBufferSize);
      if (initialCapacity < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(initialCapacity), initialCapacity);

      using (var outStream = new MemoryStream(initialCapacity)) {
        stream.CopyTo(outStream, readBufferSize);

        outStream.Close();

        return outStream.ToArray();
      }
    }

    public static Task<byte[]> ReadToEndAsync(
      this Stream stream,
      int readBufferSize = defaultReadBufferSize,
      int initialCapacity = defaultInitialBufferCapacity,
      CancellationToken cancellationToken = default
    )
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));
      if (readBufferSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(readBufferSize), readBufferSize);
      if (initialCapacity < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(initialCapacity), initialCapacity);

      return _ReadToEndAsync();

      async Task<byte[]> _ReadToEndAsync()
      {
        using (var outStream = new MemoryStream(initialCapacity)) {
          await stream.CopyToAsync(outStream, readBufferSize, cancellationToken).ConfigureAwait(false);

          outStream.Close();

          return outStream.ToArray();
        }
      }
    }

    public static void Write(this Stream stream, ArraySegment<byte> segment)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

      stream.Write(segment.Array, segment.Offset, segment.Count);
    }
  }
}
