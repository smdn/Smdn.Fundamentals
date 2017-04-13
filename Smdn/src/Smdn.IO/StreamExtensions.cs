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

namespace Smdn.IO {
  public static class StreamExtensions {
    public static void CopyTo(this Stream stream, System.IO.BinaryWriter writer)
    {
      CopyTo(stream, writer, 10 * 1024);
    }

    public static void CopyTo(this Stream stream, System.IO.BinaryWriter writer, int bufferSize)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");
      if (writer == null)
        throw new ArgumentNullException("writer");
      if (bufferSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive("bufferSize", bufferSize);

      var buffer = new byte[bufferSize];

      for (;;) {
        var read = stream.Read(buffer, 0, bufferSize);

        if (read <= 0)
          break;

        writer.Write(buffer, 0, read);
      }
    }

    public static byte[] ReadToEnd(this Stream stream)
    {
      return ReadToEnd(stream, 4096, 4096);
    }

    public static byte[] ReadToEnd(this Stream stream, int initialCapacity)
    {
      return ReadToEnd(stream, 4096, initialCapacity);
    }

    public static byte[] ReadToEnd(this Stream stream, int readBufferSize, int initialCapacity)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");
      if (readBufferSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive("readBufferSize", readBufferSize);
      if (initialCapacity < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("initialCapacity", initialCapacity);

      using (var outStream = new MemoryStream(initialCapacity)) {
        stream.CopyTo(outStream, readBufferSize);

#if NET46
        outStream.Close();
#else
        outStream.Dispose();
#endif

        return outStream.ToArray();
      }
    }

    public static void Write(this Stream stream, ArraySegment<byte> segment)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");

      stream.Write(segment.Array, segment.Offset, segment.Count);
    }
  }
}
