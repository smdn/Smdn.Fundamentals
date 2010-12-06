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
    private const int defaultStorageSize = 8;

    public Endianness Endianness {
      get { return endianness; }
    }

    public BinaryReader(Stream stream)
      : this(stream, Platform.Endianness, false, defaultStorageSize)
    {
    }

    public BinaryReader(Stream stream, bool leaveBaseStreamOpen)
      : this(stream, Platform.Endianness, leaveBaseStreamOpen, defaultStorageSize)
    {
    }

    protected BinaryReader(Stream baseStream, Endianness endianness, bool leaveBaseStreamOpen)
      : this(baseStream, endianness, leaveBaseStreamOpen, defaultStorageSize)
    {
    }

    protected BinaryReader(Stream baseStream, Endianness endianness, bool leaveBaseStreamOpen, int storageSize)
      : base(baseStream, leaveBaseStreamOpen)
    {
      if (storageSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive("storageSize", storageSize);

      this.endianness = endianness;
      this.Storage = new byte[storageSize];
    }

    public override byte ReadByte()
    {
      ReadBytesUnchecked(Storage, 0, 1, true);

      return Storage[0];
    }

    [CLSCompliant(false)]
    public override sbyte ReadSByte()
    {
      ReadBytesUnchecked(Storage, 0, 1, true);

      return unchecked((sbyte)Storage[0]);
    }

    public override short ReadInt16()
    {
      ReadBytesUnchecked(Storage, 0, 2, true);

      return BinaryConvert.ToInt16(Storage, 0, endianness);
    }

    [CLSCompliant(false)]
    public override ushort ReadUInt16()
    {
      ReadBytesUnchecked(Storage, 0, 2, true);

      return BinaryConvert.ToUInt16(Storage, 0, endianness);
    }

    public override int ReadInt32()
    {
      ReadBytesUnchecked(Storage, 0, 4, true);

      return BinaryConvert.ToInt32(Storage, 0, endianness);
    }

    [CLSCompliant(false)]
    public override uint ReadUInt32()
    {
      ReadBytesUnchecked(Storage, 0, 4, true);

      return BinaryConvert.ToUInt32(Storage, 0, endianness);
    }

    public override long ReadInt64()
    {
      ReadBytesUnchecked(Storage, 0, 8, true);

      return BinaryConvert.ToInt64(Storage, 0, endianness);
    }

    [CLSCompliant(false)]
    public override ulong ReadUInt64()
    {
      ReadBytesUnchecked(Storage, 0, 8, true);

      return BinaryConvert.ToUInt64(Storage, 0, endianness);
    }

    public virtual UInt24 ReadUInt24()
    {
      ReadBytesUnchecked(Storage, 0, 3, true);

      return BinaryConvertExtensions.ToUInt24(Storage, 0, endianness);
    }

    public virtual UInt48 ReadUInt48()
    {
      ReadBytesUnchecked(Storage, 0, 6, true);

      return BinaryConvertExtensions.ToUInt48(Storage, 0, endianness);
    }

    public virtual FourCC ReadFourCC()
    {
      ReadBytesUnchecked(Storage, 0, 4, true);

      return new FourCC(Storage, 0);
    }

    private readonly Endianness endianness;
    protected readonly byte[] Storage;
  }
}
