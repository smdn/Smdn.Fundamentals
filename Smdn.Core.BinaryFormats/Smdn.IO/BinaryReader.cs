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
  public class BinaryReader : BinaryReaderBase {
    public Endianness Endianness {
      get { return endianness; }
    }

    public BinaryReader(Stream stream)
      : this(stream, false)
    {
    }

    public BinaryReader(Stream stream, bool leaveBaseStreamOpen)
      : this(stream, Platform.Endianness, leaveBaseStreamOpen)
    {
    }

    protected BinaryReader(Stream baseStream, Endianness endianness, bool leaveBaseStreamOpen)
      : base(baseStream, leaveBaseStreamOpen)
    {
      this.endianness = endianness;
      this.store = new byte[8];
    }

    public override short ReadInt16()
    {
      ReadBytesUnchecked(store, 0, 2, true);

      return BinaryConvert.ToInt16(store, 0, endianness);
    }

    [CLSCompliant(false)]
    public override ushort ReadUInt16()
    {
      ReadBytesUnchecked(store, 0, 2, true);

      return BinaryConvert.ToUInt16(store, 0, endianness);
    }

    public override int ReadInt32()
    {
      ReadBytesUnchecked(store, 0, 4, true);

      return BinaryConvert.ToInt32(store, 0, endianness);
    }

    [CLSCompliant(false)]
    public override uint ReadUInt32()
    {
      ReadBytesUnchecked(store, 0, 4, true);

      return BinaryConvert.ToUInt32(store, 0, endianness);
    }

    public override long ReadInt64()
    {
      ReadBytesUnchecked(store, 0, 8, true);

      return BinaryConvert.ToInt64(store, 0, endianness);
    }

    [CLSCompliant(false)]
    public override ulong ReadUInt64()
    {
      ReadBytesUnchecked(store, 0, 8, true);

      return BinaryConvert.ToUInt64(store, 0, endianness);
    }

    public override UInt24 ReadUInt24()
    {
      ReadBytesUnchecked(store, 0, 3, true);

      return BinaryConvert.ToUInt24(store, 0, endianness);
    }

    public override UInt48 ReadUInt48()
    {
      ReadBytesUnchecked(store, 0, 6, true);

      return BinaryConvert.ToUInt48(store, 0, endianness);
    }

    public override FourCC ReadFourCC()
    {
      ReadBytesUnchecked(store, 0, 4, true);

      return new FourCC(store, 0);
    }

    private readonly Endianness endianness;
    private byte[] store;
  }
}
