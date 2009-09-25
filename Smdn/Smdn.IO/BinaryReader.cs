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
    public bool EndOfStream {
      get { return BinaryReaderImpl.IsEndOfStream(this); }
    }

    public BinaryReader(Stream stream)
      : base(stream)
    {
    }

    protected byte[] ReadBytesOrThrowException(int count)
    {
      return BinaryReaderImpl.ReadBytesOrThrowException(this, count);
    }

    public byte[] ReadBytes(long count)
    {
      return BinaryReaderImpl.ReadBytes(this, count);
    }

    public byte[] ReadToEnd()
    {
      return BinaryReaderImpl.ReadToEnd(this);
    }

    public short ReadInt16BE()
    {
      return BinaryReaderImpl.ReadInt16BE(this);
    }

    public short ReadInt16LE()
    {
      return BinaryReaderImpl.ReadInt16LE(this);
    }

    [CLSCompliant(false)]
    public ushort ReadUInt16BE()
    {
      return BinaryReaderImpl.ReadUInt16BE(this);
    }

    [CLSCompliant(false)]
    public ushort ReadUInt16LE()
    {
      return BinaryReaderImpl.ReadUInt16LE(this);
    }

    public int ReadInt32BE()
    {
      return BinaryReaderImpl.ReadInt32BE(this);
    }

    public int ReadInt32LE()
    {
      return BinaryReaderImpl.ReadInt32LE(this);
    }

    [CLSCompliant(false)]
    public uint ReadUInt32BE()
    {
      return BinaryReaderImpl.ReadUInt32BE(this);
    }

    [CLSCompliant(false)]
    public uint ReadUInt32LE()
    {
      return BinaryReaderImpl.ReadUInt32LE(this);
    }

    public long ReadInt64BE()
    {
      return BinaryReaderImpl.ReadInt64BE(this);
    }

    public long ReadInt64LE()
    {
      return BinaryReaderImpl.ReadInt64LE(this);
    }

    [CLSCompliant(false)]
    public ulong ReadUInt64BE()
    {
      return BinaryReaderImpl.ReadUInt64BE(this);
    }

    [CLSCompliant(false)]
    public ulong ReadUInt64LE()
    {
      return BinaryReaderImpl.ReadUInt64LE(this);
    }

    public UInt24 ReadUInt24BE()
    {
      return BinaryReaderImpl.ReadUInt24BE(this);
    }

    public UInt24 ReadUInt24LE()
    {
      return BinaryReaderImpl.ReadUInt24LE(this);
    }

    public UInt48 ReadUInt48BE()
    {
      return BinaryReaderImpl.ReadUInt48BE(this);
    }

    public UInt48 ReadUInt48LE()
    {
      return BinaryReaderImpl.ReadUInt48LE(this);
    }

    public FourCC ReadFourCC()
    {
      return BinaryReaderImpl.ReadFourCC(this);
    }
  }
}
