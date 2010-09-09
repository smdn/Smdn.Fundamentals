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
  public class LittleEndianBinaryReader : System.IO.BinaryReader {
    public bool EndOfStream {
      get { return BinaryReaderImpl.IsEndOfStream(this); }
    }

    public LittleEndianBinaryReader(Stream stream)
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

    public override short ReadInt16()
    {
      return BinaryReaderImpl.ReadInt16LE(this);
    }

    [CLSCompliant(false)]
    public override ushort ReadUInt16()
    {
      return BinaryReaderImpl.ReadUInt16LE(this);
    }

    public override int ReadInt32()
    {
      return BinaryReaderImpl.ReadInt32LE(this);
    }

    [CLSCompliant(false)]
    public override uint ReadUInt32()
    {
      return BinaryReaderImpl.ReadUInt32LE(this);
    }

    public override long ReadInt64()
    {
      return BinaryReaderImpl.ReadInt64LE(this);
    }

    [CLSCompliant(false)]
    public override ulong ReadUInt64()
    {
      return BinaryReaderImpl.ReadUInt64LE(this);
    }

    public virtual UInt24 ReadUInt24()
    {
      return BinaryReaderImpl.ReadUInt24LE(this);
    }

    public virtual UInt48 ReadUInt48()
    {
      return BinaryReaderImpl.ReadUInt48LE(this);
    }

    public virtual FourCC ReadFourCC()
    {
      return BinaryReaderImpl.ReadFourCC(this);
    }
  }
}
