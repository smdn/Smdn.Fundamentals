// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2014 smdn
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
  [Obsolete("use Smdn.IO.Binary.LittleEndianBinaryReader instead")]
  public class LittleEndianBinaryReader : Smdn.IO.BinaryReader {
    public LittleEndianBinaryReader(Stream stream)
      : this(stream, false)
    {
    }

    public LittleEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen)
      : base(stream, Endianness.LittleEndian, leaveBaseStreamOpen)
    {
    }

    protected LittleEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen, int storageSize)
      : base(stream, Endianness.LittleEndian, leaveBaseStreamOpen, storageSize)
    {
    }

    public override short ReadInt16()
    {
      ReadBytesUnchecked(Storage, 0, 2, true);

      return BinaryConvert.ToInt16LE(Storage, 0);
    }

    [CLSCompliant(false)]
    public override ushort ReadUInt16()
    {
      ReadBytesUnchecked(Storage, 0, 2, true);

      return BinaryConvert.ToUInt16LE(Storage, 0);
    }

    public override int ReadInt32()
    {
      ReadBytesUnchecked(Storage, 0, 4, true);

      return BinaryConvert.ToInt32LE(Storage, 0);
    }

    [CLSCompliant(false)]
    public override uint ReadUInt32()
    {
      ReadBytesUnchecked(Storage, 0, 4, true);

      return BinaryConvert.ToUInt32LE(Storage, 0);
    }

    public override long ReadInt64()
    {
      ReadBytesUnchecked(Storage, 0, 8, true);

      return BinaryConvert.ToInt64LE(Storage, 0);
    }

    [CLSCompliant(false)]
    public override ulong ReadUInt64()
    {
      ReadBytesUnchecked(Storage, 0, 8, true);

      return BinaryConvert.ToUInt64LE(Storage, 0);
    }

    public override UInt24 ReadUInt24()
    {
      ReadBytesUnchecked(Storage, 0, 3, true);

      return BinaryConvertExtensions.ToUInt24LE(Storage, 0);
    }

    public override UInt48 ReadUInt48()
    {
      ReadBytesUnchecked(Storage, 0, 6, true);

      return BinaryConvertExtensions.ToUInt48LE(Storage, 0);
    }
  }
}
