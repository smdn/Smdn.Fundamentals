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
  public class BinaryReader : System.IO.BinaryReader {
    public BinaryReader(Stream stream)
      : base(stream)
    {
    }

    public byte[] ReadToEnd()
    {
      if (BaseStream.CanSeek) {
        return ReadBytes(BaseStream.Length - BaseStream.Position);
      }
      else {
        // TODO: use BaseStream.Read
        //return base.ReadBytes((int)(BaseStream.Length - BaseStream.Position));
        throw new NotSupportedException();
      }
    }

    public byte[] ReadBytes(long count)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", "must be zero or positive number");

      if (count == 0)
        return new byte[] {};

      var readTotal = 0L;
      byte[] buffer = null;

      while (readTotal < count) {
        var bytesToRead = count - readTotal;
        var bytes = base.ReadBytes((int.MaxValue < bytesToRead) ? int.MaxValue : (int)bytesToRead);

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

    private byte[] ReadBytesOrThrow(int length)
    {
      var bytes = ReadBytes(length);

      if (bytes.Length < length)
        throw new EndOfStreamException();
      else
        return bytes;
    }

    public short ReadInt16BE()
    {
      var bytes = ReadBytesOrThrow(2);

      return (short)(bytes[0] << 8 | bytes[1]);
    }

    public short ReadInt16LE()
    {
      var bytes = ReadBytesOrThrow(2);

      return (short)(bytes[1] << 8 | bytes[0]);
    }

    public ushort ReadUInt16BE()
    {
      var bytes = ReadBytesOrThrow(2);

      return (ushort)(bytes[0] << 8 | bytes[1]);
    }

    public ushort ReadUInt16LE()
    {
      var bytes = ReadBytesOrThrow(2);

      return (ushort)(bytes[1] << 8 | bytes[0]);
    }

    public int ReadInt32BE()
    {
      var bytes = ReadBytesOrThrow(4);

      return (int)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);
    }

    public int ReadInt32LE()
    {
      var bytes = ReadBytesOrThrow(4);

      return (int)(bytes[3] << 24 | bytes[2] << 16 | bytes[1] << 8 | bytes[0]);
    }

    public uint ReadUInt32BE()
    {
      var bytes = ReadBytesOrThrow(4);

      return (uint)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);
    }

    public uint ReadUInt32LE()
    {
      var bytes = ReadBytesOrThrow(4);

      return (uint)(bytes[3] << 24 | bytes[2] << 16 | bytes[1] << 8 | bytes[0]);
    }

    public long ReadInt64BE()
    {
      return (long)ReadUInt64BE();
    }

    public long ReadInt64LE()
    {
      return (long)ReadUInt64LE();
    }

    public ulong ReadUInt64BE()
    {
      var high = (ulong)ReadUInt32BE();
      var low  = (ulong)ReadUInt32BE();

      return (ulong)(high << 32 | low);
    }

    public ulong ReadUInt64LE()
    {
      var low  = (ulong)ReadUInt32LE();
      var high = (ulong)ReadUInt32LE();

      return (ulong)(high << 32 | low);
    }

    public UInt48 ReadUInt48BE()
    {
      return new UInt48(ReadBytesOrThrow(6), true);
    }

    public UInt48 ReadUInt48LE()
    {
      return new UInt48(ReadBytesOrThrow(6), false);
    }

    public FourCC ReadFourCC()
    {
      return new FourCC(ReadBytesOrThrow(4));
    }
  }
}
