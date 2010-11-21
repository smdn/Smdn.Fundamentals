// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2010 smdn
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
  internal static class BinaryReaderImpl {
    internal static bool IsEndOfStream(System.IO.BinaryReader r)
    {
      if (r.BaseStream.CanSeek) {
        var eos = (r.BaseStream.ReadByte() < 0);

        if (!eos)
          r.BaseStream.Seek(-1, SeekOrigin.Current);

        return eos;
      }
      else {
        return false;
      }
    }

    internal static byte[] ReadToEnd(System.IO.BinaryReader r)
    {
      if (r.BaseStream.CanSeek) {
        return ReadBytes(r, r.BaseStream.Length - r.BaseStream.Position);
      }
      else {
        using (var readStream = new MemoryStream()) {
          r.BaseStream.CopyTo(readStream, 1024);

          readStream.Close();

          return readStream.ToArray();
        }
      }
    }

    internal static byte[] ReadBytes(System.IO.BinaryReader r, long count)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", "must be zero or positive number");

      if (count == 0)
        return new byte[] {};

      var readTotal = 0L;
      byte[] buffer = null;

      while (readTotal < count) {
        var bytesToRead = count - readTotal;
        var bytes = r.ReadBytes((int.MaxValue < bytesToRead) ? int.MaxValue : (int)bytesToRead);

        if (buffer == null) {
          if (bytes.LongLength < bytesToRead || bytes.LongLength == count)
            return bytes; // EOF or read required bytes
          else
            buffer = new byte[count];
        }

        Array.Copy(bytes, 0L, buffer, readTotal, bytes.LongLength);

        readTotal += bytes.LongLength;
      }

      if (readTotal < count) {
        var readBuffer = new byte[readTotal];

        Array.Copy(buffer, 0L, readBuffer, 0L, readTotal);

        return readBuffer;
      }
      else {
        return buffer;
      }
    }

    internal static byte[] ReadBytesOrThrowException(System.IO.BinaryReader r, int length)
    {
      var bytes = r.ReadBytes(length);

      if (bytes.Length < length)
        throw new EndOfStreamException();
      else
        return bytes;
    }

    internal static short ReadInt16BE(System.IO.BinaryReader r)
    {
      var bytes = ReadBytesOrThrowException(r, 2);

      return (short)(bytes[0] << 8 | bytes[1]);
    }

    internal static short ReadInt16LE(System.IO.BinaryReader r)
    {
      var bytes = ReadBytesOrThrowException(r, 2);

      return (short)(bytes[1] << 8 | bytes[0]);
    }

    internal static ushort ReadUInt16BE(System.IO.BinaryReader r)
    {
      var bytes = ReadBytesOrThrowException(r, 2);

      return (ushort)(bytes[0] << 8 | bytes[1]);
    }

    internal static ushort ReadUInt16LE(System.IO.BinaryReader r)
    {
      var bytes = ReadBytesOrThrowException(r, 2);

      return (ushort)(bytes[1] << 8 | bytes[0]);
    }

    internal static int ReadInt32BE(System.IO.BinaryReader r)
    {
      var bytes = ReadBytesOrThrowException(r, 4);

      return (int)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);
    }

    internal static int ReadInt32LE(System.IO.BinaryReader r)
    {
      var bytes = ReadBytesOrThrowException(r, 4);

      return (int)(bytes[3] << 24 | bytes[2] << 16 | bytes[1] << 8 | bytes[0]);
    }

    internal static uint ReadUInt32BE(System.IO.BinaryReader r)
    {
      var bytes = ReadBytesOrThrowException(r, 4);

      return (uint)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);
    }

    internal static uint ReadUInt32LE(System.IO.BinaryReader r)
    {
      var bytes = ReadBytesOrThrowException(r, 4);

      return (uint)(bytes[3] << 24 | bytes[2] << 16 | bytes[1] << 8 | bytes[0]);
    }

    internal static long ReadInt64BE(System.IO.BinaryReader r)
    {
      return (long)ReadUInt64BE(r);
    }

    internal static long ReadInt64LE(System.IO.BinaryReader r)
    {
      return (long)ReadUInt64LE(r);
    }

    internal static ulong ReadUInt64BE(System.IO.BinaryReader r)
    {
      var high = (ulong)ReadUInt32BE(r);
      var low  = (ulong)ReadUInt32BE(r);

      return (ulong)(high << 32 | low);
    }

    internal static ulong ReadUInt64LE(System.IO.BinaryReader r)
    {
      var low  = (ulong)ReadUInt32LE(r);
      var high = (ulong)ReadUInt32LE(r);

      return (ulong)(high << 32 | low);
    }

    internal static UInt24 ReadUInt24BE(System.IO.BinaryReader r)
    {
      return new UInt24(ReadBytesOrThrowException(r, 3), 0, true);
    }

    internal static UInt24 ReadUInt24LE(System.IO.BinaryReader r)
    {
      return new UInt24(ReadBytesOrThrowException(r, 3), 0, false);
    }

    internal static UInt48 ReadUInt48BE(System.IO.BinaryReader r)
    {
      return new UInt48(ReadBytesOrThrowException(r, 6), 0, true);
    }

    internal static UInt48 ReadUInt48LE(System.IO.BinaryReader r)
    {
      return new UInt48(ReadBytesOrThrowException(r, 6), 0, false);
    }

    internal static FourCC ReadFourCC(System.IO.BinaryReader r)
    {
      return new FourCC(ReadBytesOrThrowException(r, 4), 0);
    }
  }
}
