// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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
    public static void WriteToEnd(this Stream fromStream, Stream toStream)
    {
      WriteToEnd(fromStream, toStream, 4096);
    }

    public static void WriteToEnd(this Stream fromStream, Stream toStream, int bufferSize)
    {
      if (toStream == null)
        throw new ArgumentNullException("toStream");
      if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException("bufferSize", "must be non-zero positive number");

      var buffer = new byte[bufferSize];

      for (;;) {
        var read = fromStream.Read(buffer, 0, bufferSize);

        if (read <= 0)
          break;

        toStream.Write(buffer, 0, read);
      }
    }

    public static void WriteToEnd(this Stream stream, System.IO.BinaryWriter writer)
    {
      WriteToEnd(stream, writer, 4096);
    }

    public static void WriteToEnd(this Stream stream, System.IO.BinaryWriter writer, int bufferSize)
    {
      if (writer == null)
        throw new ArgumentNullException("writer");
      if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException("bufferSize", "must be non-zero positive number");

      var buffer = new byte[bufferSize];

      for (;;) {
        var read = stream.Read(buffer, 0, bufferSize);

        if (read <= 0)
          break;

        writer.Write(buffer, 0, read);
      }
    }
  }
}
